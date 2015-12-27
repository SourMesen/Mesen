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
#include "HdPpu.h"

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

void Console::Release()
{
	Console::Instance.reset(new Console());
}

void Console::Initialize(string romFilename, stringstream *filestream, string ipsFilename)
{
	MessageManager::SendNotification(ConsoleNotificationType::GameStopped);
	shared_ptr<BaseMapper> mapper = MapperFactory::InitializeFromFile(romFilename, filestream, ipsFilename);

	if(mapper) {
		_romFilepath = romFilename;
				
		VideoDecoder::GetInstance()->StopThread();

		_mapper = mapper;
		_memoryManager.reset(new MemoryManager(_mapper));
		_cpu.reset(new CPU(_memoryManager.get()));
		if(HdNesPack::HasHdPack(_romFilepath)) {
			_ppu.reset(new HdPpu(_memoryManager.get()));
		} else {
			_ppu.reset(new PPU(_memoryManager.get()));
		}
		_apu.reset(new APU(_memoryManager.get()));

		_controlManager.reset(new ControlManager());

		_memoryManager->RegisterIODevice(_mapper.get());
		_memoryManager->RegisterIODevice(_ppu.get());
		_memoryManager->RegisterIODevice(_apu.get());
		_memoryManager->RegisterIODevice(_controlManager.get());

		ResetComponents(false);

		_initialized = true;

		VideoDecoder::GetInstance()->StartThread();
	
		FolderUtilities::AddKnowGameFolder(FolderUtilities::GetFolderName(romFilename));
		MessageManager::DisplayMessage("Game loaded", FolderUtilities::GetFilename(romFilename, false));
	} else {
		MessageManager::DisplayMessage("Error", string("Could not load file: ") + FolderUtilities::GetFilename(romFilename, true));
	}
}

void Console::ApplyIpsPatch(string ipsFilename)
{
	Console::Pause();
	Instance->Initialize(GetROMPath(), nullptr, ipsFilename);
	Console::Resume();
}

void Console::LoadROM(string filepath, stringstream *filestream)
{
	Console::Pause();
	Instance->Initialize(filepath, filestream);
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
		if(softReset) {
			Instance->ResetComponents(softReset);
		} else {
			//Full reset of all objects to ensure the emulator always starts in the exact same state
			Instance->Initialize(Instance->_romFilepath);
		}
		Console::Resume();
	}
}

void Console::ResetComponents(bool softReset)
{
	Movie::Stop();

	if(_debugger) {
		//Reset debugger and break on first instruction
		StopDebugger();
		GetDebugger();
		_debugger->Step(1);
	}

	_ppu->Reset();
	_apu->Reset(softReset);
	_cpu->Reset(softReset);
	_memoryManager->Reset(softReset);

	_apu->StopAudio(true);

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
	if(_debugger) {
		_debugger->Run();
	}
	_stopLock.Acquire();
	_stopLock.Release();
}

void Console::Pause()
{
	if(Console::Instance->_debugger) {
		//Make sure debugger resumes if we try to pause the emu, otherwise we will get deadlocked.
		Console::Instance->_debugger->Run();
	}
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
	double targetTime;
	double elapsedTime = 0;
	uint32_t lastFrameNumber = -1;

	_runLock.Acquire();
	_stopLock.Acquire();

	UpdateNesModel(targetTime, true);

	VideoDecoder::GetInstance()->StartThread();
		
	while(true) { 
		_cpu->Exec();
		uint32_t currentFrameNumber = PPU::GetFrameCount();
		if(currentFrameNumber != lastFrameNumber) {
			lastFrameNumber = currentFrameNumber;

			//Sleep until we're ready to start the next frame
			clockTimer.WaitUntil(targetTime);

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

			//Get next target time, and adjust based on whether we are ahead or behind
			double timeLag = EmulationSettings::GetEmulationSpeed() == 0 ? 0 : clockTimer.GetElapsedMS() - targetTime;
			UpdateNesModel(targetTime, false);
			clockTimer.Reset();
			targetTime -= timeLag;
			if(targetTime < 0) {
				targetTime = 0;
			}
			
			if(_stop) {
				_stop = false;
				break;
			}
		}
	}
	_apu->StopAudio(true);
	Movie::Stop();

	VideoDecoder::GetInstance()->StopThread();

	_stopLock.Release();
	_runLock.Release();
}

void Console::UpdateNesModel(double &frameDelay, bool showMessage)
{
	NesModel model = EmulationSettings::GetNesModel();
	uint32_t emulationSpeed = EmulationSettings::GetEmulationSpeed();
	if(model == NesModel::Auto) {
		model = _mapper->IsPalRom() ? NesModel::PAL : NesModel::NTSC;
	}
	
	if(emulationSpeed == 0) {
		frameDelay = 0;
	} else {
		frameDelay = (model == NesModel::NTSC ? 16.63926405550947 : 19.99720920217466); //60.1fps (NTSC), 50.01fps (PAL)
		frameDelay /= (double)emulationSpeed / 100.0;
	}

	_ppu->SetNesModel(model);
	_apu->SetNesModel(model);
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

std::weak_ptr<Debugger> Console::GetDebugger()
{
	if(!_debugger) {
		_debugger.reset(new Debugger(Console::Instance, _cpu, _ppu, _memoryManager, _mapper));
	}
	return _debugger;
}

void Console::StopDebugger()
{
	_debugger.reset();
}