#include "stdafx.h"
#include <thread>
#include "Console.h"
#include "BaseMapper.h"
#include "MapperFactory.h"
#include "Debugger.h"
#include "../Utilities/Timer.h"
#include "../Utilities/FolderUtilities.h"
#include "../Core/MessageManager.h"

shared_ptr<Console> Console::Instance = nullptr;
uint32_t Console::Flags = 0;
uint32_t Console::CurrentFPS = 0;

Console::Console(wstring filename)
{
	Initialize(filename);
}

Console::~Console()
{
	Movie::Stop();
	if(Console::Instance.get() == this) {
		Console::Instance.reset();
	}
}

shared_ptr<Console> Console::GetInstance()
{
	return Console::Instance;
}

void Console::Initialize(wstring filename)
{
	MessageManager::SendNotification(ConsoleNotificationType::GameStopped);
	if(Console::Instance == nullptr) {
		Console::Instance.reset(this);
	}

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

		MessageManager::DisplayMessage(L"Game loaded", FolderUtilities::GetFilename(filename, false));
	} else {
		MessageManager::DisplayMessage(L"Error", wstring(L"Could not load file: ") + FolderUtilities::GetFilename(filename, true));
	}

}

void Console::LoadROM(wstring filename)
{
	if(!Instance) {
		new Console(filename);
	} else {
		Console::Pause();
		Instance->Initialize(filename);
		Console::Resume();
	}
}

wstring Console::GetROMPath()
{
	wstring filepath;
	if(Instance) {
		filepath = Instance->_romFilepath;
	}
	return filepath;
}

void Console::Reset(bool softReset)
{
	Movie::Stop();
	if(Instance) {
		Console::Pause();
		Instance->ResetComponents(softReset);
		Console::Resume();
	}
}

void Console::ResetComponents(bool softReset)
{
	_ppu->Reset();
	_apu->Reset();
	_cpu->Reset(softReset);
	if(softReset) {
		MessageManager::SendNotification(ConsoleNotificationType::GameReset);
	} else {
		MessageManager::SendNotification(ConsoleNotificationType::GameLoaded);
	}
}

void Console::Stop()
{
	_stop = true;
	Console::ClearFlags(EmulationFlags::Paused);
	_stopLock.Acquire();
	_stopLock.Release();
}

void Console::Pause()
{
	if(Console::Instance) {
		Console::Instance->_pauseLock.Acquire();
		//Spin wait until emu pauses
		Console::Instance->_runLock.Acquire();
	}
}

void Console::Resume()
{
	if(Console::Instance) {
		Console::Instance->_runLock.Release();
		Console::Instance->_pauseLock.Release();
	}
}

void Console::SetFlags(int flags)
{
	Console::Flags |= flags;
}

void Console::ClearFlags(int flags)
{
	Console::Flags &= ~flags;
}

bool Console::CheckFlag(int flag)
{
	return (Console::Flags & flag) == flag;
}

uint32_t Console::GetFPS()
{
	return Console::CurrentFPS;
}

void Console::Run()
{
	Timer clockTimer;
	Timer fpsTimer;
	uint32_t lastFrameCount = 0;
	double elapsedTime = 0;
	double targetTime = 16.6666666666666666;
	
	_runLock.Acquire();
	_stopLock.Acquire();

	while(true) { 
		bool frameDone = _apu->Exec(_cpu->Exec());

		if(frameDone) {
			_cpu->EndFrame();

			if(CheckFlag(EmulationFlags::LimitFPS) && frameDone) {
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

			if(CheckFlag(EmulationFlags::Paused) && !_stop) {
				MessageManager::SendNotification(ConsoleNotificationType::GamePaused);
				_runLock.Release();
				
				//Prevent audio from looping endlessly while game is paused
				_apu->StopAudio();

				while(CheckFlag(EmulationFlags::Paused)) {
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
		
		if(fpsTimer.GetElapsedMS() > 1000) {
			uint32_t frameCount = _ppu->GetFrameCount();
			if((int32_t)frameCount - (int32_t)lastFrameCount < 0) {
				Console::CurrentFPS = 0;
			} else {
				Console::CurrentFPS = (int)(std::round((double)(frameCount - lastFrameCount) / (fpsTimer.GetElapsedMS() / 1000)));
			}
			lastFrameCount = frameCount;
			fpsTimer.Reset();
		}
	}
	_apu->StopAudio();
	_stopLock.Release();
	_runLock.Release();
}

void Console::SaveState(ostream &saveStream)
{
	if(Instance) {
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
	if(Instance) {
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
