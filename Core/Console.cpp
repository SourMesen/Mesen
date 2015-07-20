#include "stdafx.h"
#include <thread>
#include "Console.h"
#include "BaseMapper.h"
#include "MapperFactory.h"
#include "Debugger.h"
#include "MessageManager.h"
#include "EmulationSettings.h"
#include "../Utilities/Timer.h"
#include "../Utilities/FolderUtilities.h"

shared_ptr<Console> Console::Instance(new Console());

Console::Console()
{
}

Console::~Console()
{
	Movie::Stop();
}

shared_ptr<Console> Console::GetInstance()
{
	return Console::Instance;
}

void Console::Initialize(string filename)
{
	MessageManager::SendNotification(ConsoleNotificationType::GameStopped);
	shared_ptr<BaseMapper> mapper = MapperFactory::InitializeFromFile(filename);
	if(mapper) {
		_romFilepath = filename;

		_mapper = mapper;
		_memoryManager.reset(new MemoryManager(_mapper));
		_cpu.reset(new CPU(_memoryManager.get()));
		_ppu.reset(new PPU(_memoryManager.get()));
		_apu.reset(new APU(_memoryManager.get()));

		_controlManager.reset(new ControlManager());

		_memoryManager->RegisterIODevice(_mapper.get());
		_memoryManager->RegisterIODevice(_ppu.get());
		_memoryManager->RegisterIODevice(_apu.get());
		_memoryManager->RegisterIODevice(_controlManager.get());

		ResetComponents(false);

		_initialized = true;
	
		FolderUtilities::AddKnowGameFolder(FolderUtilities::GetFolderName(filename));
		MessageManager::DisplayMessage("Game loaded", FolderUtilities::GetFilename(filename, false));
	} else {
		MessageManager::DisplayMessage("Error", string("Could not load file: ") + FolderUtilities::GetFilename(filename, true));
	}
}

void Console::LoadROM(string filepath)
{
	Console::Pause();
	Instance->Initialize(filepath);
	Console::Resume();
}

bool Console::LoadROM(string filename, uint32_t crc32Hash)
{
	string currentRomFilepath = Console::GetROMPath();
	string currentFolder = FolderUtilities::GetFolderName(currentRomFilepath);
	if(!currentRomFilepath.empty()) {
		if(ROMLoader::GetCRC32(Console::GetROMPath()) == crc32Hash) {
			//Current game matches, no need to do anything
			return true;
		}

		//Try to find the game in the same folder as the current game's folder
		string match = ROMLoader::FindMatchingRomInFolder(currentFolder, filename, crc32Hash);
		if(!match.empty()) {
			Console::LoadROM(match);
			return true;
		}
	}

	for(string folder : FolderUtilities::GetKnowGameFolders()) {
		if(folder != currentFolder) {
			string match = ROMLoader::FindMatchingRomInFolder(folder, filename, crc32Hash);
			if(!match.empty()) {
				Console::LoadROM(match);
				return true;
			}
		}
	}
	return false;
}

string Console::GetROMPath()
{
	return Instance->_romFilepath;
}

void Console::Reset(bool softReset)
{
	Movie::Stop();
	if(Instance->_initialized) {
		Console::Pause();
		Instance->ResetComponents(softReset);
		Console::Resume();
	}
}

void Console::ResetComponents(bool softReset)
{
	Movie::Stop();

	_ppu->Reset();
	_apu->Reset(softReset);
	_cpu->Reset(softReset);
	_memoryManager->Reset(softReset);

	if(softReset) {
		MessageManager::SendNotification(ConsoleNotificationType::GameReset);
	} else {
		MessageManager::SendNotification(ConsoleNotificationType::GameLoaded);
	}
}

void Console::Stop()
{
	_stop = true;
	EmulationSettings::ClearFlags(EmulationFlags::Paused);
	_stopLock.Acquire();
	_stopLock.Release();
}

void Console::Pause()
{
	Console::Instance->_pauseLock.Acquire();
	//Spin wait until emu pauses
	Console::Instance->_runLock.Acquire();
}

void Console::Resume()
{
	Console::Instance->_runLock.Release();
	Console::Instance->_pauseLock.Release();
}

void Console::Run()
{
	Timer clockTimer;
	double elapsedTime = 0;
	double targetTime = 16.63926405550947; //~60.0988fps

	_runLock.Acquire();
	_stopLock.Acquire();

	uint32_t lastFrameNumber = -1;
	while(true) { 
		_cpu->Exec();
		uint32_t currentFrameNumber = PPU::GetFrameCount();
		if(currentFrameNumber != lastFrameNumber) {
			lastFrameNumber = currentFrameNumber;

			if(EmulationSettings::CheckFlag(EmulationFlags::LimitFPS)) {
				elapsedTime = clockTimer.GetElapsedMS();
				while(targetTime > elapsedTime) {
					if(targetTime - elapsedTime > 2) {
						std::this_thread::sleep_for(std::chrono::duration<int, std::milli>((int)(targetTime - elapsedTime - 1)));
					}
					elapsedTime = clockTimer.GetElapsedMS();
				}
			}
			
			if(!_pauseLock.IsFree()) {
				//Need to temporarely pause the emu (to save/load a state, etc.)
				_runLock.Release();

				//Spin wait until we are allowed to start again
				_pauseLock.WaitForRelease();

				_runLock.Acquire();
			}

			if(EmulationSettings::CheckFlag(EmulationFlags::Paused) && !_stop) {
				MessageManager::SendNotification(ConsoleNotificationType::GamePaused);
				_runLock.Release();
				
				//Prevent audio from looping endlessly while game is paused
				_apu->StopAudio();

				while(EmulationSettings::CheckFlag(EmulationFlags::Paused)) {
					//Sleep until emulation is resumed
					std::this_thread::sleep_for(std::chrono::duration<int, std::milli>(100));
				}
				_runLock.Acquire();
				MessageManager::SendNotification(ConsoleNotificationType::GameResumed);
			}
			clockTimer.Reset();
			
			if(_stop) {
				_stop = false;
				break;
			}
		}
	}
	_apu->StopAudio();
	_stopLock.Release();
	_runLock.Release();
}

void Console::SaveState(ostream &saveStream)
{
	if(Instance->_initialized) {
		Instance->_cpu->SaveSnapshot(&saveStream);
		Instance->_ppu->SaveSnapshot(&saveStream);
		Instance->_memoryManager->SaveSnapshot(&saveStream);
		Instance->_mapper->SaveSnapshot(&saveStream);
		Instance->_apu->SaveSnapshot(&saveStream);
		Instance->_controlManager->SaveSnapshot(&saveStream);
	}
}

void Console::LoadState(istream &loadStream)
{
	if(Instance->_initialized) {
		Instance->_cpu->LoadSnapshot(&loadStream);
		Instance->_ppu->LoadSnapshot(&loadStream);
		Instance->_memoryManager->LoadSnapshot(&loadStream);
		Instance->_mapper->LoadSnapshot(&loadStream);
		Instance->_apu->LoadSnapshot(&loadStream);
		Instance->_controlManager->LoadSnapshot(&loadStream);
		
		MessageManager::SendNotification(ConsoleNotificationType::StateLoaded);
	}
}

void Console::LoadState(uint8_t *buffer, uint32_t bufferSize)
{
	stringstream stream;
	stream.write((char*)buffer, bufferSize);
	stream.seekg(0, ios::beg);
	LoadState(stream);
}

shared_ptr<Debugger> Console::GetDebugger()
{
	return shared_ptr<Debugger>(new Debugger(Console::Instance, _cpu, _ppu, _memoryManager, _mapper));
}
