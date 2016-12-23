#include "stdafx.h"
#include <thread>
#include "Console.h"
#include "BaseMapper.h"
#include "ControlManager.h"
#include "VsControlManager.h"
#include "MapperFactory.h"
#include "Debugger.h"
#include "MessageManager.h"
#include "RomLoader.h"
#include "EmulationSettings.h"
#include "../Utilities/Timer.h"
#include "../Utilities/FolderUtilities.h"
#include "../Utilities/PlatformUtilities.h"
#include "HdPpu.h"
#include "NsfPpu.h"
#include "SoundMixer.h"
#include "NsfMapper.h"
#include "ShortcutKeyHandler.h"

shared_ptr<Console> Console::Instance(new Console());

Console::Console()
{
	_resetRequested = false;
	_lagCounter = 0;
}

Console::~Console()
{
	Movie::Stop();
	SoundMixer::StopRecording();
}

shared_ptr<Console> Console::GetInstance()
{
	return Console::Instance;
}

void Console::Release()
{
	Console::Instance.reset(new Console());
}

void Console::Initialize(string romFilename, stringstream *filestream, string ipsFilename, int32_t archiveFileIndex)
{
	SoundMixer::StopAudio();

	if(_mapper) {
		//Ensure we save any battery file before loading a new game
		_mapper->SaveBattery();
	}

	MessageManager::SendNotification(ConsoleNotificationType::GameStopped);
	shared_ptr<BaseMapper> mapper = MapperFactory::InitializeFromFile(romFilename, filestream, ipsFilename, archiveFileIndex);

	if(mapper) {
		_romFilepath = romFilename;
				
		_autoSaveManager.reset(new AutoSaveManager());
		VideoDecoder::GetInstance()->StopThread();

		_mapper = mapper;
		_memoryManager.reset(new MemoryManager(_mapper));
		_cpu.reset(new CPU(_memoryManager.get()));
		if(HdNesPack::HasHdPack(_romFilepath)) {
			_ppu.reset(new HdPpu(_memoryManager.get()));
		} else if(NsfMapper::GetInstance()) {
			//Disable most of the PPU for NSFs
			_ppu.reset(new NsfPpu(_memoryManager.get()));
		} else {
			_ppu.reset(new PPU(_memoryManager.get()));
		}
		_apu.reset(new APU(_memoryManager.get()));

		_controlManager.reset(_mapper->GetGameSystem() == GameSystem::VsUniSystem ? new VsControlManager() : new ControlManager());
		_controlManager->UpdateControlDevices();

		_memoryManager->RegisterIODevice(_ppu.get());
		_memoryManager->RegisterIODevice(_apu.get());
		_memoryManager->RegisterIODevice(_controlManager.get());
		_memoryManager->RegisterIODevice(_mapper.get());

		_model = NesModel::Auto;
		UpdateNesModel(false);

		_initialized = true;

		if(_debugger) {
			auto lock = _debuggerLock.AcquireSafe();
			StopDebugger();
			GetDebugger();
		}

		ResetComponents(false);
		
		VideoDecoder::GetInstance()->StartThread();
	
		FolderUtilities::AddKnownGameFolder(FolderUtilities::GetFolderName(romFilename));

		string modelName = _model == NesModel::PAL ? "PAL" : (_model == NesModel::Dendy ? "Dendy" : "NTSC");
		string messageTitle = MessageManager::Localize("GameLoaded") + " (" + modelName + ")";
		MessageManager::DisplayMessage(messageTitle, FolderUtilities::GetFilename(_mapper->GetRomName(), false));
		if(EmulationSettings::GetOverclockRate() != 100) {
			MessageManager::DisplayMessage("ClockRate", std::to_string(EmulationSettings::GetOverclockRate()) + "%");
		}
	} else {
		MessageManager::DisplayMessage("Error", "CouldNotLoadFile", FolderUtilities::GetFilename(romFilename, true));
	}
}

void Console::LoadROM(string filepath, stringstream *filestream, int32_t archiveFileIndex, string ipsFile)
{
	Console::Pause();
	Instance->Initialize(filepath, filestream, ipsFile, archiveFileIndex);
	Console::Resume();
}

bool Console::LoadROM(string filename, uint32_t crc32Hash)
{
	string currentRomFilepath = Console::GetROMPath();
	string currentFolder = FolderUtilities::GetFolderName(currentRomFilepath);
	if(!currentRomFilepath.empty()) {
		if(Console::GetCrc32() == crc32Hash) {
			//Current game matches, no need to do anything
			return true;
		}
	}

	int32_t archiveFileIndex = -1;
	for(string folder : FolderUtilities::GetKnownGameFolders()) {
		string match = RomLoader::FindMatchingRomInFolder(folder, filename, crc32Hash, true, archiveFileIndex);
		if(!match.empty()) {
			Console::LoadROM(match, nullptr, archiveFileIndex);
			return true;
		}
	}

	//Perform slow CRC32 search for ROM
	for(string folder : FolderUtilities::GetKnownGameFolders()) {
		string match = RomLoader::FindMatchingRomInFolder(folder, filename, crc32Hash, false, archiveFileIndex);
		if(!match.empty()) {
			Console::LoadROM(match, nullptr, archiveFileIndex);
			return true;
		}
	}

	return false;
}

string Console::GetROMPath()
{
	return Instance->_romFilepath;
}

string Console::GetRomName()
{
	if(Instance->_mapper) {
		return Instance->_mapper->GetRomName();
	} else {
		return "";
	}
}

uint32_t Console::GetCrc32()
{
	if(Instance->_mapper) {
		return Instance->_mapper->GetCrc32();
	} else {
		return 0;
	}
}

uint32_t Console::GetPrgCrc32()
{
	if(Instance->_mapper) {
		return Instance->_mapper->GetPrgCrc32();
	} else {
		return 0;
	}
}

NesModel Console::GetModel()
{
	return Instance->_model;
}

void Console::Reset(bool softReset)
{
	Movie::Stop();
	SoundMixer::StopRecording();

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
	SoundMixer::StopRecording();

	_memoryManager->Reset(softReset);
	_ppu->Reset();
	_apu->Reset(softReset);
	_cpu->Reset(softReset);
	_controlManager->Reset(softReset);

	_lagCounter = 0;

	SoundMixer::StopAudio(true);

	if(softReset) {
		if(_debugger) {
			auto lock = _debuggerLock.AcquireSafe();
			StopDebugger();
			GetDebugger();
		}

		MessageManager::SendNotification(ConsoleNotificationType::GameReset);
	} else {
		MessageManager::SendNotification(ConsoleNotificationType::GameLoaded);
	}
}

void Console::Stop()
{
	_stop = true;

	shared_ptr<Debugger> debugger = _debugger;
	if(debugger) {
		debugger->Run();
	}
	_stopLock.Acquire();
	_stopLock.Release();
}

void Console::Pause()
{
	shared_ptr<Debugger> debugger = Console::Instance->_debugger;
	if(debugger) {
		//Make sure debugger resumes if we try to pause the emu, otherwise we will get deadlocked.
		debugger->Suspend();
	}
	Console::Instance->_pauseLock.Acquire();
	//Spin wait until emu pauses
	Console::Instance->_runLock.Acquire();
}

void Console::Resume()
{
	Console::Instance->_runLock.Release();
	Console::Instance->_pauseLock.Release();
	
	shared_ptr<Debugger> debugger = Console::Instance->_debugger;
	if(debugger) {
		//Make sure debugger resumes if we try to pause the emu, otherwise we will get deadlocked.
		debugger->Resume();
	}
}

void Console::Run()
{
	Timer clockTimer;
	double targetTime;
	uint32_t lastFrameNumber = -1;
	
	ShortcutKeyHandler shortcutKeyHandler;
	_autoSaveManager.reset(new AutoSaveManager());

	_runLock.Acquire();
	_stopLock.Acquire();

	targetTime = GetFrameDelay();

	VideoDecoder::GetInstance()->StartThread();

	PlatformUtilities::DisableScreensaver();
		
	while(true) { 
		try {
			_cpu->Exec();
		} catch(const std::runtime_error &ex) {
			MessageManager::DisplayMessage("Error", "GameCrash", ex.what());
			break;
		}

		uint32_t currentFrameNumber = PPU::GetFrameCount();
		if(currentFrameNumber != lastFrameNumber) {
			EmulationSettings::DisableOverclocking(_disableOcNextFrame);
			_disableOcNextFrame = false;

			lastFrameNumber = currentFrameNumber;

			//Sleep until we're ready to start the next frame
			clockTimer.WaitUntil(targetTime);
			
			if(_resetRequested) {
				//Used by NSF player to reset console after changing track
				ResetComponents(true);
				_resetRequested = false;
			}

			if(!_pauseLock.IsFree()) {
				//Need to temporarely pause the emu (to save/load a state, etc.)
				_runLock.Release();

				//Spin wait until we are allowed to start again
				_pauseLock.WaitForRelease();

				_runLock.Acquire();
			}

			bool paused = EmulationSettings::IsPaused();
			if(paused && !_stop) {
				MessageManager::SendNotification(ConsoleNotificationType::GamePaused);
				
				//Prevent audio from looping endlessly while game is paused
				SoundMixer::StopAudio();

				_runLock.Release();
				
				PlatformUtilities::EnableScreensaver();
				while(paused && !_stop && _debugger == nullptr) {
					//Sleep until emulation is resumed
					std::this_thread::sleep_for(std::chrono::duration<int, std::milli>(100));
					paused = EmulationSettings::IsPaused();
				}

				if(_debugger != nullptr) {
					//Prevent pausing when debugger is active
					EmulationSettings::ClearFlags(EmulationFlags::Paused);
				}

				PlatformUtilities::DisableScreensaver();
				_runLock.Acquire();								
				MessageManager::SendNotification(ConsoleNotificationType::GameResumed);
			}

			//Get next target time, and adjust based on whether we are ahead or behind
			double timeLag = EmulationSettings::GetEmulationSpeed() == 0 ? 0 : clockTimer.GetElapsedMS() - targetTime;
			UpdateNesModel(true);
			targetTime = GetFrameDelay();

			clockTimer.Reset();
			targetTime -= timeLag;
			if(targetTime < 0) {
				targetTime = 0;
			}

			if(_controlManager->GetLagFlag()) {
				_lagCounter++;
			}
			
			if(_stop) {
				_stop = false;
				break;
			}
		}
	}
	SoundMixer::StopAudio();
	Movie::Stop();
	SoundMixer::StopRecording();
	PlatformUtilities::EnableScreensaver();

	_autoSaveManager.reset();

	VideoDecoder::GetInstance()->StopThread();

	_initialized = false;
	_romFilepath = "";

	_stopLock.Release();
	_runLock.Release();
}

bool Console::IsRunning()
{
	return !Instance->_stopLock.IsFree();
}

void Console::UpdateNesModel(bool sendNotification)
{
	bool configChanged = false;
	if(EmulationSettings::NeedControllerUpdate()) {
		_controlManager->UpdateControlDevices();
		configChanged = true;
	}

	NesModel model = EmulationSettings::GetNesModel();
	if(model == NesModel::Auto) {
		switch(_mapper->GetGameSystem()) {
			case GameSystem::NesPal: model = NesModel::PAL; break;
			case GameSystem::Dendy: model = NesModel::Dendy; break;
			default: model = NesModel::NTSC; break;
		}
	}
	if(_model != model) {
		_model = model;
		configChanged = true;

		if(sendNotification) {
			MessageManager::DisplayMessage("Region", model == NesModel::PAL ? "PAL" : (model == NesModel::Dendy ? "Dendy" : "NTSC"));
		}
	}

	_mapper->SetNesModel(model);
	_ppu->SetNesModel(model);
	_apu->SetNesModel(model);

	if(configChanged && sendNotification) {
		MessageManager::SendNotification(ConsoleNotificationType::ConfigChanged);
	}
}

double Console::GetFrameDelay()
{
	uint32_t emulationSpeed = EmulationSettings::GetEmulationSpeed();
	double frameDelay;
	if(emulationSpeed == 0) {
		frameDelay = 0;
	} else {
		//60.1fps (NTSC), 50.01fps (PAL/Dendy)
		switch(_model) {
			default:
			case NesModel::NTSC: frameDelay = 16.63926405550947; break;
			case NesModel::PAL:
			case NesModel::Dendy: frameDelay = 19.99720920217466; break;
		}
		frameDelay /= (double)emulationSpeed / 100.0;
	}

	return frameDelay;
}

void Console::SaveState(ostream &saveStream)
{
	if(Instance->_initialized) {
		Instance->_cpu->SaveSnapshot(&saveStream);
		Instance->_ppu->SaveSnapshot(&saveStream);
		Instance->_memoryManager->SaveSnapshot(&saveStream);
		Instance->_apu->SaveSnapshot(&saveStream);
		Instance->_controlManager->SaveSnapshot(&saveStream);
		Instance->_mapper->SaveSnapshot(&saveStream);
	}
}

void Console::LoadState(istream &loadStream)
{
	if(Instance->_initialized) {
		//Stop any movie that might have been playing/recording if a state is loaded
		//(Note: Loading a state is disabled in the UI while a movie is playing/recording)
		Movie::Stop();

		Instance->_cpu->LoadSnapshot(&loadStream);
		Instance->_ppu->LoadSnapshot(&loadStream);
		Instance->_memoryManager->LoadSnapshot(&loadStream);
		Instance->_apu->LoadSnapshot(&loadStream);
		Instance->_controlManager->LoadSnapshot(&loadStream);
		Instance->_mapper->LoadSnapshot(&loadStream);
		
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

std::shared_ptr<Debugger> Console::GetDebugger(bool autoStart)
{
	auto lock = _debuggerLock.AcquireSafe();
	if(!_debugger && autoStart) {
		_debugger.reset(new Debugger(Console::Instance, _cpu, _ppu, _memoryManager, _mapper));
	}
	return _debugger;
}

void Console::StopDebugger()
{
	auto lock = _debuggerLock.AcquireSafe();
	_debugger.reset();
}

void Console::RequestReset()
{
	Instance->_resetRequested = true;
}

NesModel Console::GetNesModel()
{
	return Instance->_model;
}

uint32_t Console::GetLagCounter()
{
	return Instance->_lagCounter;
}

void Console::ResetLagCounter()
{
	Instance->_lagCounter = 0;
}

bool Console::IsDebuggerAttached()
{
	return (bool)Instance->_debugger;
}

void Console::DisableOcNextFrame()
{
	Instance->_disableOcNextFrame = true;
}