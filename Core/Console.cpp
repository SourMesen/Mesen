#include "stdafx.h"
#include <thread>
#include "Console.h"
#include "CPU.h"
#include "PPU.h"
#include "APU.h"
#include "MemoryManager.h"
#include "AutoSaveManager.h"
#include "BaseMapper.h"
#include "ControlManager.h"
#include "VsControlManager.h"
#include "MapperFactory.h"
#include "Debugger.h"
#include "MessageManager.h"
#include "EmulationSettings.h"
#include "../Utilities/sha1.h"
#include "../Utilities/Timer.h"
#include "../Utilities/FolderUtilities.h"
#include "../Utilities/PlatformUtilities.h"
#include "VirtualFile.h"
#include "HdBuilderPpu.h"
#include "HdPpu.h"
#include "NsfPpu.h"
#include "SoundMixer.h"
#include "NsfMapper.h"
#include "MovieManager.h"
#include "RewindManager.h"
#include "SaveStateManager.h"
#include "HdPackBuilder.h"
#include "HdAudioDevice.h"
#include "FDS.h"
#include "SystemActionManager.h"
#include "FdsSystemActionManager.h"
#include "VsSystemActionManager.h"
#include "FamilyBasicDataRecorder.h"
#include "IBarcodeReader.h"
#include "IBattery.h"
#include "KeyManager.h"
#include "BatteryManager.h"
#include "DebugHud.h"
#include "RomLoader.h"
#include "CheatManager.h"
#include "VideoDecoder.h"
#include "VideoRenderer.h"
#include "DebugHud.h"
#include "NotificationManager.h"

Console::Console(shared_ptr<Console> master)
{
	_master = master;
	_model = NesModel::NTSC;
}

Console::~Console()
{
	MovieManager::Stop();
}

void Console::Init()
{
	_notificationManager.reset(new NotificationManager());
	
	_videoRenderer.reset(new VideoRenderer(shared_from_this()));
	_videoDecoder.reset(new VideoDecoder(shared_from_this()));

	_saveStateManager.reset(new SaveStateManager(shared_from_this()));
	_cheatManager.reset(new CheatManager(shared_from_this()));
	_debugHud.reset(new DebugHud());

	_soundMixer.reset(new SoundMixer(shared_from_this()));
	_soundMixer->SetNesModel(_model);

	if(_master) {
		_emulationThreadId = _master->_emulationThreadId;
	}
}

void Console::Release(bool forShutdown)
{
	if(_slave) {
		_slave->Release(true);
		_slave.reset();
	}

	if(forShutdown) {
		_videoDecoder->StopThread();
		_videoRenderer->StopThread();

		_videoDecoder.reset();
		_videoRenderer.reset();

		_debugHud.reset();
		_saveStateManager.reset();
		_cheatManager.reset();

		_soundMixer.reset();
		_notificationManager.reset();
	}

	if(_master) {
		_master->_notificationManager->SendNotification(ConsoleNotificationType::VsDualSystemStopped);
	}

	_rewindManager.reset();
	_autoSaveManager.reset();

	_hdPackBuilder.reset();
	_hdData.reset();
	_hdAudioDevice.reset();

	_systemActionManager.reset();
	
	_master.reset();
	_cpu.reset();
	_ppu.reset();
	_apu.reset();
	_debugger.reset();
	_mapper.reset();
	_memoryManager.reset();
	_controlManager.reset();
}

shared_ptr<SaveStateManager> Console::GetSaveStateManager()
{
	return _saveStateManager;
}

shared_ptr<VideoDecoder> Console::GetVideoDecoder()
{
	return _videoDecoder;
}

shared_ptr<VideoRenderer> Console::GetVideoRenderer()
{
	return _videoRenderer;
}

shared_ptr<DebugHud> Console::GetDebugHud()
{
	return _debugHud;
}

void Console::SaveBatteries()
{
	shared_ptr<BaseMapper> mapper = _mapper;
	shared_ptr<ControlManager> controlManager = _controlManager;
	
	if(mapper) {
		mapper->SaveBattery();
	}

	if(controlManager) {
		shared_ptr<IBattery> device = std::dynamic_pointer_cast<IBattery>(controlManager->GetControlDevice(BaseControlDevice::ExpDevicePort));
		if(device) {
			device->SaveBattery();
		}
	}
}

bool Console::LoadMatchingRom(string romName, HashInfo hashInfo)
{
	if(_initialized) {
		string currentRomFilepath = GetRomPath().GetFilePath();
		if(!currentRomFilepath.empty()) {
			HashInfo gameHashInfo = GetRomInfo().Hash;
			if(gameHashInfo.Crc32 == hashInfo.Crc32 || gameHashInfo.Sha1.compare(hashInfo.Sha1) == 0 || gameHashInfo.PrgChrMd5.compare(hashInfo.PrgChrMd5) == 0) {
				//Current game matches, power cycle game and return
				PowerCycle();
				return true;
			}
		}
	}

	string match = FindMatchingRom(romName, hashInfo);
	if(!match.empty()) {
		return Initialize(match);
	}
	return false;
}

string Console::FindMatchingRom(string romName, HashInfo hashInfo)
{
	if(_initialized) {
		VirtualFile currentRom = GetRomPath();
		if(currentRom.IsValid() && !GetPatchFile().IsValid()) {
			HashInfo gameHashInfo = GetRomInfo().Hash;
			if(gameHashInfo.Crc32 == hashInfo.Crc32 || gameHashInfo.Sha1.compare(hashInfo.Sha1) == 0 || gameHashInfo.PrgChrMd5.compare(hashInfo.PrgChrMd5) == 0) {
				//Current game matches
				return currentRom;
			}
		}
	}

	string lcRomname = romName;
	std::transform(lcRomname.begin(), lcRomname.end(), lcRomname.begin(), ::tolower);
	std::unordered_set<string> validExtensions = { { ".nes", ".fds", "*.unif", "*.unif", "*.nsf", "*.nsfe", "*.7z", "*.zip" } };
	vector<string> romFiles;
	for(string folder : FolderUtilities::GetKnownGameFolders()) {
		vector<string> files = FolderUtilities::GetFilesInFolder(folder, validExtensions, true);
		romFiles.insert(romFiles.end(), files.begin(), files.end());
	}

	string match = RomLoader::FindMatchingRom(romFiles, romName, hashInfo, true);
	if(!match.empty()) {
		return match;
	}

	//Perform slow CRC32 search for ROM
	match = RomLoader::FindMatchingRom(romFiles, romName, hashInfo, false);
	if(!match.empty()) {
		return match;
	}

	return "";
}

bool Console::Initialize(string romFile, string patchFile)
{
	VirtualFile rom = romFile;
	VirtualFile patch = patchFile;
	return Initialize(rom, patch);
}

bool Console::Initialize(VirtualFile &romFile)
{
	VirtualFile patchFile;
	return Initialize(romFile, patchFile);
}

bool Console::Initialize(VirtualFile &romFile, VirtualFile &patchFile)
{
	Pause();
	if(!_romFilepath.empty() && _mapper) {
		//Ensure we save any battery file before loading a new game
		SaveBatteries();
	}

	if(romFile.IsValid()) {
		_videoDecoder->StopThread();

		LoadHdPack(romFile, patchFile);
		if(patchFile.IsValid()) {
			if(romFile.ApplyPatch(patchFile)) {
				MessageManager::DisplayMessage("Patch", "ApplyingPatch", patchFile.GetFileName());
			} else {
				//Patch failed
			}
		}
		vector<uint8_t> fileData;
		romFile.ReadFile(fileData);

		BatteryManager::Initialize(FolderUtilities::GetFilename(romFile.GetFileName(), false));
		shared_ptr<BaseMapper> mapper = MapperFactory::InitializeFromFile(shared_from_this(), romFile.GetFileName(), fileData);
		if(mapper) {
			_soundMixer->StopAudio(true);

			bool isDifferentGame = _romFilepath != (string)romFile || _patchFilename != (string)patchFile;
			if(_mapper) {
				if(isDifferentGame) {
					//Save current game state before loading another one
					_saveStateManager->SaveRecentGame(GetRomInfo().RomName, _romFilepath, _patchFilename);
				}

				//Send notification only if a game was already running and we successfully loaded the new one
				_notificationManager->SendNotification(ConsoleNotificationType::GameStopped, (void*)1);
			}

			if(isDifferentGame) {
				_romFilepath = romFile;
				_patchFilename = patchFile;
				
				//Changed game, stop all recordings
				MovieManager::Stop();
				_soundMixer->StopRecording();
				StopRecordingHdPack();
			}

			_mapper = mapper;
			_memoryManager.reset(new MemoryManager(shared_from_this()));
			_cpu.reset(new CPU(shared_from_this()));
			_apu.reset(new APU(shared_from_this()));

			if(_slave) {
				_slave->Release(false);
				_slave.reset();
			}

			RomInfo romInfo = _mapper->GetRomInfo();
			if(!_master && romInfo.VsType == VsSystemType::VsDualSystem) {
				_slave.reset(new Console(shared_from_this()));
				_slave->Init();
				_slave->Initialize(romFile, patchFile);
			}

			switch(romInfo.System) {
				case GameSystem::FDS:
					EmulationSettings::SetPpuModel(PpuModel::Ppu2C02);
					_systemActionManager.reset(new FdsSystemActionManager(shared_from_this(), _mapper));
					break;
				
				case GameSystem::VsSystem:
					EmulationSettings::SetPpuModel(romInfo.VsPpuModel);
					_systemActionManager.reset(new VsSystemActionManager(shared_from_this()));
					break;
				
				default: 
					EmulationSettings::SetPpuModel(PpuModel::Ppu2C02);
					_systemActionManager.reset(new SystemActionManager(shared_from_this())); break;
			}

			//Temporarely disable battery saves to prevent battery files from being created for the wrong game (for Battle Box & Turbo File)
			BatteryManager::SetSaveEnabled(false);

			uint32_t pollCounter = 0;
			if(_controlManager && !isDifferentGame) {
				//When power cycling, poll counter must be preserved to allow movies to playback properly
				pollCounter = _controlManager->GetPollCounter();
			}

			if(romInfo.System == GameSystem::VsSystem) {
				_controlManager.reset(new VsControlManager(shared_from_this(), _systemActionManager, _mapper->GetMapperControlDevice()));
			} else {
				_controlManager.reset(new ControlManager(shared_from_this(), _systemActionManager, _mapper->GetMapperControlDevice()));
			}
			_controlManager->SetPollCounter(pollCounter);
			_controlManager->UpdateControlDevices();
			
			//Re-enable battery saves
			BatteryManager::SetSaveEnabled(true);
			
			if(_hdData && (!_hdData->Tiles.empty() || !_hdData->Backgrounds.empty())) {
				_ppu.reset(new HdPpu(shared_from_this(), _hdData.get()));
			} else if(std::dynamic_pointer_cast<NsfMapper>(_mapper)) {
				//Disable most of the PPU for NSFs
				_ppu.reset(new NsfPpu(shared_from_this()));
			} else {
				_ppu.reset(new PPU(shared_from_this()));
			}

			_memoryManager->SetMapper(_mapper);
			_memoryManager->RegisterIODevice(_ppu.get());
			_memoryManager->RegisterIODevice(_apu.get());
			_memoryManager->RegisterIODevice(_controlManager.get());
			_memoryManager->RegisterIODevice(_mapper.get());

			if(_hdData && (!_hdData->BgmFilesById.empty() || !_hdData->SfxFilesById.empty())) {
				_hdAudioDevice.reset(new HdAudioDevice(shared_from_this(), _hdData.get()));
				_memoryManager->RegisterIODevice(_hdAudioDevice.get());
			}

			_model = NesModel::Auto;
			UpdateNesModel(false);

			_initialized = true;

			if(_debugger) {
				//Reset debugger if it was running before
				auto lock = _debuggerLock.AcquireSafe();
				StopDebugger();
				GetDebugger();
			}

			ResetComponents(false);

#ifndef LIBRETRO
			//Don't use auto-save manager for libretro
			//Only enable auto-save for the master console (VS Dualsystem)
			if(IsMaster()) {
				_autoSaveManager.reset(new AutoSaveManager(shared_from_this()));
			}
#endif
			_rewindManager.reset(new RewindManager(shared_from_this()));
			_notificationManager->RegisterNotificationListener(_rewindManager);

			_videoDecoder->StartThread();

			FolderUtilities::AddKnownGameFolder(romFile.GetFolderPath());

			if(IsMaster()) {
				string modelName = _model == NesModel::PAL ? "PAL" : (_model == NesModel::Dendy ? "Dendy" : "NTSC");
				string messageTitle = MessageManager::Localize("GameLoaded") + " (" + modelName + ")";
				MessageManager::DisplayMessage(messageTitle, FolderUtilities::GetFilename(GetRomInfo().RomName, false));
				if(EmulationSettings::GetOverclockRate() != 100) {
					MessageManager::DisplayMessage("ClockRate", std::to_string(EmulationSettings::GetOverclockRate()) + "%");
				}
				EmulationSettings::ClearFlags(EmulationFlags::ForceMaxSpeed);

				if(_slave) {
					_notificationManager->SendNotification(ConsoleNotificationType::VsDualSystemStarted);
				}
			}
			Resume();
			return true;
		}
	}

	//Reset battery source to current game if new game failed to load
	BatteryManager::Initialize(FolderUtilities::GetFilename(GetRomInfo().RomName, false));
	if(_mapper) {
		_videoDecoder->StartThread();
	}

	MessageManager::DisplayMessage("Error", "CouldNotLoadFile", romFile.GetFileName());
	Resume();
	return false;
}

void Console::ProcessCpuClock()
{
	_mapper->ProcessCpuClock();
	_ppu->ProcessCpuClock();
	_apu->ProcessCpuClock();
}

CPU* Console::GetCpu()
{
	return _cpu.get();
}

PPU* Console::GetPpu()
{
	return _ppu.get();
}

APU* Console::GetApu()
{
	return _apu.get();
}

shared_ptr<SoundMixer> Console::GetSoundMixer()
{
	return _soundMixer;
}

shared_ptr<NotificationManager> Console::GetNotificationManager()
{
	return _notificationManager;
}

bool Console::IsDualSystem()
{
	return _slave != nullptr || _master != nullptr;
}

shared_ptr<Console> Console::GetDualConsole()
{
	//When called from the master, returns the slave.
	//When called from the slave, returns the master.
	//Returns a null pointer when not running a dualsystem game
	return _slave ? _slave : _master;
}

bool Console::IsMaster()
{
	return !_master;
}

BaseMapper* Console::GetMapper()
{
	return _mapper.get();
}

ControlManager* Console::GetControlManager()
{
	return _controlManager.get();
}

MemoryManager* Console::GetMemoryManager()
{
	return _memoryManager.get();
}

CheatManager* Console::GetCheatManager()
{
	return _cheatManager.get();
}

RewindManager* Console::GetRewindManager()
{
	return _rewindManager.get();
}

VirtualFile Console::GetRomPath()
{
	return static_cast<VirtualFile>(_romFilepath);
}

VirtualFile Console::GetPatchFile()
{
	return (VirtualFile)_patchFilename;
}

RomInfo Console::GetRomInfo()
{
	return _mapper ? _mapper->GetRomInfo() : (RomInfo {});
}

uint32_t Console::GetFrameCount()
{
	return _ppu ? _ppu->GetFrameCount() : 0;
}

NesModel Console::GetModel()
{
	return _model;
}

shared_ptr<SystemActionManager> Console::GetSystemActionManager()
{
	return _systemActionManager;
}

void Console::PowerCycle()
{
	if(_initialized && !_romFilepath.empty()) {
		VirtualFile romFile = _romFilepath;
		VirtualFile patchFile = _patchFilename;
		Initialize(romFile, patchFile);
	}
}

void Console::Reset(bool softReset)
{
	if(_initialized) {
		if(softReset) {
			_systemActionManager->Reset();
		} else {
			_systemActionManager->PowerCycle();
		}

		//Resume from code break if needed (otherwise reset doesn't happen right away)
		shared_ptr<Debugger> debugger = _debugger;
		if(debugger) {
			debugger->Suspend();
			debugger->Run();
		}
	}
}

void Console::ResetComponents(bool softReset)
{
	if(_slave) {
		//Always reset/power cycle the slave alongside the master CPU
		_slave->ResetComponents(softReset);
	}

	_soundMixer->StopAudio(true);

	_memoryManager->Reset(softReset);
	if(!EmulationSettings::CheckFlag(EmulationFlags::DisablePpuReset) || !softReset) {
		_ppu->Reset();
	}
	_apu->Reset(softReset);
	_cpu->Reset(softReset, _model);
	_controlManager->Reset(softReset);

	KeyManager::UpdateDevices();

	//This notification MUST be sent before the UpdateInputState() below to allow MovieRecorder to grab the first frame's worth of inputs
	_notificationManager->SendNotification(softReset ? ConsoleNotificationType::GameReset : ConsoleNotificationType::GameLoaded);

	if(softReset) {
		shared_ptr<Debugger> debugger = _debugger;
		if(debugger) {
			debugger->ResetCounters();
			debugger->ProcessEvent(EventType::Reset);
			debugger->Resume();
		}
	}

	_controlManager->UpdateInputState();
}

void Console::Stop(int stopCode)
{
	_stop = true;
	_stopCode = stopCode;

	shared_ptr<Debugger> debugger = _debugger;
	if(debugger) {
		debugger->Suspend();
	}
	_stopLock.Acquire();
	_stopLock.Release();
}

void Console::Pause()
{
	shared_ptr<Debugger> debugger = _debugger;
	if(debugger) {
		//Make sure debugger resumes if we try to pause the emu, otherwise we will get deadlocked.
		debugger->Suspend();
	}

	if(_master) {
		//When trying to pause/resume the slave, we need to pause/resume the master instead
		_master->Pause();
	} else {
		_pauseLock.Acquire();
		//Spin wait until emu pauses
		_runLock.Acquire();
	}
}

void Console::Resume()
{
	if(_master) {
		//When trying to pause/resume the slave, we need to pause/resume the master instead
		_master->Resume();
	} else {
		_runLock.Release();
		_pauseLock.Release();
	}
	
	shared_ptr<Debugger> debugger = _debugger;
	if(debugger) {
		//Make sure debugger resumes if we try to pause the emu, otherwise we will get deadlocked.
		debugger->Resume();
	}
}

void Console::RunSingleFrame()
{
	//Used by Libretro
	uint32_t lastFrameNumber = _ppu->GetFrameCount();
	_emulationThreadId = std::this_thread::get_id();
	UpdateNesModel(true);

	while(_ppu->GetFrameCount() == lastFrameNumber) {
		_cpu->Exec();
		if(_slave) {
			RunSlaveCpu();
		}
	}

	EmulationSettings::DisableOverclocking(_disableOcNextFrame || NsfMapper::GetInstance());
	_disableOcNextFrame = false;

	_systemActionManager->ProcessSystemActions();
	_apu->EndFrame();
}

void Console::RunSlaveCpu()
{
	int32_t cycleGap;
	while(true) {
		//Run the slave until it catches up to the master CPU (and take into account the CPU count overflow that occurs every ~20mins)
		cycleGap = _cpu->GetCycleCount() - _slave->_cpu->GetCycleCount();
		if(cycleGap > 5 || cycleGap < -10000 || _ppu->GetFrameCount() > _slave->_ppu->GetFrameCount()) {
			_slave->_cpu->Exec();
		} else {
			break;
		}
	}
}

void Console::Run()
{
	Timer clockTimer;
	Timer lastFrameTimer;
	double targetTime;
	double timeLagData[16] = {};
	int timeLagDataIndex = 0;
	double lastFrameMin = 9999;
	double lastFrameMax = 0;

	uint32_t lastFrameNumber = -1;

	_runLock.Acquire();
	_stopLock.Acquire();

	_emulationThreadId = std::this_thread::get_id();
	if(_slave) {
		_slave->_emulationThreadId = std::this_thread::get_id();
	}

	targetTime = GetFrameDelay();

	_videoDecoder->StartThread();

	PlatformUtilities::DisableScreensaver();

	UpdateNesModel(true);

	_running = true;

	bool crashed = false;
	try {
		while(true) {
			_cpu->Exec();

			if(_slave) {
				RunSlaveCpu();
			}

			if(_ppu->GetFrameCount() != lastFrameNumber) {
				_soundMixer->ProcessEndOfFrame();
				if(_slave) {
					_slave->_soundMixer->ProcessEndOfFrame();
				}

				bool displayDebugInfo = EmulationSettings::CheckFlag(EmulationFlags::DisplayDebugInfo);
				if(displayDebugInfo) {
					DisplayDebugInformation(clockTimer, lastFrameTimer, lastFrameMin, lastFrameMax, timeLagData);
					if(_slave) {
						_slave->DisplayDebugInformation(clockTimer, lastFrameTimer, lastFrameMin, lastFrameMax, timeLagData);
					}
				}
				lastFrameTimer.Reset();

				_rewindManager->ProcessEndOfFrame();
				EmulationSettings::DisableOverclocking(_disableOcNextFrame || NsfMapper::GetInstance());
				_disableOcNextFrame = false;

				//Sleep until we're ready to start the next frame
				clockTimer.WaitUntil(targetTime);

				if(!_pauseLock.IsFree()) {
					//Need to temporarely pause the emu (to save/load a state, etc.)
					_runLock.Release();

					//Spin wait until we are allowed to start again
					_pauseLock.WaitForRelease();

					_runLock.Acquire();
				}

				bool paused = EmulationSettings::IsPaused();
				if(paused && !_stop) {
					_notificationManager->SendNotification(ConsoleNotificationType::GamePaused);

					//Prevent audio from looping endlessly while game is paused
					_soundMixer->StopAudio();
					if(_slave) {
						_slave->_soundMixer->StopAudio();
					}

					_runLock.Release();

					PlatformUtilities::EnableScreensaver();
					PlatformUtilities::RestoreTimerResolution();
					while(paused && !_stop) {
						//Sleep until emulation is resumed
						std::this_thread::sleep_for(std::chrono::duration<int, std::milli>(30));
						paused = EmulationSettings::IsPaused();
					}

					if(EmulationSettings::CheckFlag(EmulationFlags::DebuggerWindowEnabled)) {
						//Prevent pausing when debugger is active
						EmulationSettings::ClearFlags(EmulationFlags::Paused);
					}

					PlatformUtilities::DisableScreensaver();
					_runLock.Acquire();
					_notificationManager->SendNotification(ConsoleNotificationType::GameResumed);
					lastFrameTimer.Reset();
				}

				if(EmulationSettings::CheckFlag(EmulationFlags::UseHighResolutionTimer)) {
					PlatformUtilities::EnableHighResolutionTimer();
				} else {
					PlatformUtilities::RestoreTimerResolution();
				}

				_systemActionManager->ProcessSystemActions();

				//Get next target time, and adjust based on whether we are ahead or behind
				double timeLag = EmulationSettings::GetEmulationSpeed() == 0 ? 0 : clockTimer.GetElapsedMS() - targetTime;
				if(displayDebugInfo) {
					timeLagData[timeLagDataIndex] = timeLag;
					timeLagDataIndex = (timeLagDataIndex + 1) & 0x0F;
				}
				UpdateNesModel(true);
				targetTime = GetFrameDelay();

				clockTimer.Reset();
				targetTime -= timeLag;
				if(targetTime < 0) {
					targetTime = 0;
				}

				lastFrameNumber = _ppu->GetFrameCount();

				if(_stop) {
					_stop = false;
					break;
				}
			}
		}
	} catch(const std::runtime_error &ex) {
		crashed = true;
		_stopCode = -1;
		MessageManager::DisplayMessage("Error", "GameCrash", ex.what());
	}

	_running = false;

	_notificationManager->SendNotification(ConsoleNotificationType::BeforeEmulationStop);

	if(!crashed) {
		_saveStateManager->SaveRecentGame(GetRomInfo().RomName, _romFilepath, _patchFilename);
	}

	_videoDecoder->StopThread();
	StopRecordingHdPack();

	_soundMixer->StopAudio();
	MovieManager::Stop();
	_soundMixer->StopRecording();

	PlatformUtilities::EnableScreensaver();
	PlatformUtilities::RestoreTimerResolution();

	EmulationSettings::ClearFlags(EmulationFlags::Paused);
	EmulationSettings::ClearFlags(EmulationFlags::ForceMaxSpeed);

	_initialized = false;

	if(!_romFilepath.empty() && _mapper) {
		//Ensure we save any battery file before unloading anything
		SaveBatteries();
	}

	_romFilepath = "";

	Release(false);
	
	_stopLock.Release();
	_runLock.Release();

	_emulationThreadId = std::thread::id();

	_notificationManager->SendNotification(ConsoleNotificationType::GameStopped);
	_notificationManager->SendNotification(ConsoleNotificationType::EmulationStopped);
}

bool Console::IsRunning()
{
	if(_master) {
		//For slave CPU, return the master's state
		return _master->IsRunning();
	} else {
		return !_stopLock.IsFree() && _running;
	}
}

bool Console::IsPaused()
{
	if(_master) {
		//For slave CPU, return the master's state
		return _master->IsPaused();
	} else {
		return _runLock.IsFree() || !_pauseLock.IsFree() || !_running;
	}
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
		switch(_mapper->GetRomInfo().System) {
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
		_notificationManager->SendNotification(ConsoleNotificationType::ConfigChanged);
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
			case NesModel::NTSC: frameDelay = EmulationSettings::CheckFlag(EmulationFlags::IntegerFpsMode) ? 16.6666666666666666667 : 16.63926405550947; break;
			case NesModel::PAL:
			case NesModel::Dendy: frameDelay = EmulationSettings::CheckFlag(EmulationFlags::IntegerFpsMode) ? 20 : 19.99720920217466; break;
		}
		frameDelay /= (double)emulationSpeed / 100.0;
	}

	return frameDelay;
}

void Console::SaveState(ostream &saveStream)
{
	if(_initialized) {
		_cpu->SaveSnapshot(&saveStream);
		_ppu->SaveSnapshot(&saveStream);
		_memoryManager->SaveSnapshot(&saveStream);
		_apu->SaveSnapshot(&saveStream);
		_controlManager->SaveSnapshot(&saveStream);
		_mapper->SaveSnapshot(&saveStream);
		if(_hdAudioDevice) {
			_hdAudioDevice->SaveSnapshot(&saveStream);
		} else {
			Snapshotable::WriteEmptyBlock(&saveStream);
		}

		if(_slave) {
			//For VS Dualsystem, append the 2nd console's savestate
			_slave->SaveState(saveStream);
		}
	}
}

void Console::LoadState(istream &loadStream)
{
	LoadState(loadStream, SaveStateManager::FileFormatVersion);
}

void Console::LoadState(istream &loadStream, uint32_t stateVersion)
{
	if(_initialized) {
		_cpu->LoadSnapshot(&loadStream, stateVersion);
		_ppu->LoadSnapshot(&loadStream, stateVersion);
		_memoryManager->LoadSnapshot(&loadStream, stateVersion);
		_apu->LoadSnapshot(&loadStream, stateVersion);
		_controlManager->LoadSnapshot(&loadStream, stateVersion);
		_mapper->LoadSnapshot(&loadStream, stateVersion);
		if(_hdAudioDevice) {
			_hdAudioDevice->LoadSnapshot(&loadStream, stateVersion);
		} else {
			Snapshotable::SkipBlock(&loadStream);
		}

		if(_slave) {
			//For VS Dualsystem, the slave console's savestate is appended to the end of the file
			_slave->LoadState(loadStream, stateVersion);
		}
		
		shared_ptr<Debugger> debugger = _debugger;
		if(debugger) {
			debugger->ResetCounters();
		}

		_notificationManager->SendNotification(ConsoleNotificationType::StateLoaded);
	}
}

void Console::LoadState(uint8_t *buffer, uint32_t bufferSize)
{
	//Send any unprocessed sound to the SoundMixer - needed for rewind
	_apu->EndFrame();

	stringstream stream;
	stream.write((char*)buffer, bufferSize);
	stream.seekg(0, ios::beg);
	LoadState(stream);
}

void Console::BreakIfDebugging()
{
	shared_ptr<Debugger> debugger = _debugger;
	if(debugger) {
		debugger->BreakImmediately();
	} else if(EmulationSettings::CheckFlag(EmulationFlags::BreakOnCrash)) {
		//When "Break on Crash" is enabled, open the debugger and break immediately if a crash occurs
		debugger = GetDebugger(true);
		debugger->BreakImmediately();
	}
}

std::shared_ptr<Debugger> Console::GetDebugger(bool autoStart)
{
	shared_ptr<Debugger> debugger = _debugger;
	if(!debugger && autoStart) {
		//Lock to make sure we don't try to start debuggers in 2 separate threads at once
		auto lock = _debuggerLock.AcquireSafe();
		debugger = _debugger;
		if(!debugger) {
			debugger.reset(new Debugger(shared_from_this(), _cpu, _ppu, _apu, _memoryManager, _mapper));
			_debugger = debugger;
		}
	}
	return debugger;
}

void Console::StopDebugger()
{
	_debugger.reset();
}

std::thread::id Console::GetEmulationThreadId()
{
	return _emulationThreadId;
}

uint32_t Console::GetLagCounter()
{
	return _controlManager->GetLagCounter();
}

void Console::ResetLagCounter()
{
	Console::Pause();
	_controlManager->ResetLagCounter();
	Console::Reset();
}

bool Console::IsDebuggerAttached()
{
	return (bool)_debugger;
}

void Console::SetNextFrameOverclockStatus(bool disabled)
{
	_disableOcNextFrame = disabled;
}

int32_t Console::GetStopCode()
{
	return _stopCode;
}

shared_ptr<HdPackData> Console::GetHdData()
{
	return _hdData;
}

bool Console::IsHdPpu()
{
	return _hdData && std::dynamic_pointer_cast<HdPpu>(_ppu) != nullptr;
}

void Console::LoadHdPack(VirtualFile &romFile, VirtualFile &patchFile)
{
	_hdData.reset();
	_hdAudioDevice.reset();
	if(EmulationSettings::CheckFlag(EmulationFlags::UseHdPacks)) {
		_hdData.reset(new HdPackData());
		if(!HdPackLoader::LoadHdNesPack(romFile, *_hdData.get())) {
			_hdData.reset();
		} else {
			auto result = _hdData->PatchesByHash.find(romFile.GetSha1Hash());
			if(result != _hdData->PatchesByHash.end()) {
				patchFile = result->second;
			}
		}
	}
}

void Console::StartRecordingHdPack(string saveFolder, ScaleFilterType filterType, uint32_t scale, uint32_t flags, uint32_t chrRamBankSize)
{
	Console::Pause();
	std::stringstream saveState;
	SaveState(saveState);
	
	_hdPackBuilder.reset();
	_hdPackBuilder.reset(new HdPackBuilder(shared_from_this(), saveFolder, filterType, scale, flags, chrRamBankSize, !_mapper->HasChrRom()));

	_memoryManager->UnregisterIODevice(_ppu.get());
	_ppu.reset();
	_ppu.reset(new HdBuilderPpu(shared_from_this(), _hdPackBuilder.get(), chrRamBankSize));
	_memoryManager->RegisterIODevice(_ppu.get());

	LoadState(saveState);
	Console::Resume();
}

void Console::StopRecordingHdPack()
{
	if(_hdPackBuilder) {
		Console::Pause();
		std::stringstream saveState;
		SaveState(saveState);

		_memoryManager->UnregisterIODevice(_ppu.get());
		_ppu.reset();
		_ppu.reset(new PPU(shared_from_this()));
		_memoryManager->RegisterIODevice(_ppu.get());
		_hdPackBuilder.reset();

		LoadState(saveState);
		Console::Resume();
	}
}

bool Console::UpdateHdPackMode()
{
	//Switch back and forth between HD PPU and regular PPU as needed
	Console::Pause();

	VirtualFile romFile = _romFilepath;
	VirtualFile patchFile = _patchFilename;
	LoadHdPack(romFile, patchFile);

	bool isHdPackLoaded = std::dynamic_pointer_cast<HdPpu>(_ppu) != nullptr;
	bool hdPackFound = _hdData && (!_hdData->Tiles.empty() || !_hdData->Backgrounds.empty());

	bool modeChanged = false;
	if(isHdPackLoaded != hdPackFound) {
		std::stringstream saveState;
		SaveState(saveState);

		_memoryManager->UnregisterIODevice(_ppu.get());
		_ppu.reset();
		if(_hdData && (!_hdData->Tiles.empty() || !_hdData->Backgrounds.empty())) {
			_ppu.reset(new HdPpu(shared_from_this(), _hdData.get()));
		} else if(std::dynamic_pointer_cast<NsfMapper>(_mapper)) {
			//Disable most of the PPU for NSFs
			_ppu.reset(new NsfPpu(shared_from_this()));
		} else {
			_ppu.reset(new PPU(shared_from_this()));
		}
		_memoryManager->RegisterIODevice(_ppu.get());

		LoadState(saveState);
		modeChanged = true;
	}

	Console::Resume();
	
	return modeChanged;
}

ConsoleFeatures Console::GetAvailableFeatures()
{
	ConsoleFeatures features = ConsoleFeatures::None;
	shared_ptr<BaseMapper> mapper = _mapper;
	shared_ptr<ControlManager> controlManager = _controlManager;
	if(mapper && controlManager) {
		features = (ConsoleFeatures)((int)features | (int)mapper->GetAvailableFeatures());

		if(dynamic_cast<VsControlManager*>(controlManager.get())) {
			features = (ConsoleFeatures)((int)features | (int)ConsoleFeatures::VsSystem);
		}

		if(std::dynamic_pointer_cast<IBarcodeReader>(controlManager->GetControlDevice(BaseControlDevice::ExpDevicePort))) {
			features = (ConsoleFeatures)((int)features | (int)ConsoleFeatures::BarcodeReader);
		}

		if(std::dynamic_pointer_cast<FamilyBasicDataRecorder>(controlManager->GetControlDevice(BaseControlDevice::ExpDevicePort2))) {
			features = (ConsoleFeatures)((int)features | (int)ConsoleFeatures::TapeRecorder);
		}
	}
	return features;
}

void Console::InputBarcode(uint64_t barcode, uint32_t digitCount)
{
	shared_ptr<BaseMapper> mapper = _mapper;
	shared_ptr<ControlManager> controlManager = _controlManager;

	if(mapper) {
		shared_ptr<IBarcodeReader> barcodeReader = std::dynamic_pointer_cast<IBarcodeReader>(mapper->GetMapperControlDevice());
		if(barcodeReader) {
			barcodeReader->InputBarcode(barcode, digitCount);
		}
	}

	if(controlManager) {
		shared_ptr<IBarcodeReader> barcodeReader = std::dynamic_pointer_cast<IBarcodeReader>(controlManager->GetControlDevice(BaseControlDevice::ExpDevicePort));
		if(barcodeReader) {
			barcodeReader->InputBarcode(barcode, digitCount);
		}
	}
}

void Console::LoadTapeFile(string filepath)
{
	shared_ptr<ControlManager> controlManager = _controlManager;
	if(controlManager) {
		shared_ptr<FamilyBasicDataRecorder> dataRecorder = std::dynamic_pointer_cast<FamilyBasicDataRecorder>(controlManager->GetControlDevice(BaseControlDevice::ExpDevicePort2));
		if(dataRecorder) {
			Console::Pause();
			dataRecorder->LoadFromFile(filepath);
			Console::Resume();
		}
	}
}

void Console::StartRecordingTapeFile(string filepath)
{
	shared_ptr<ControlManager> controlManager = _controlManager;
	if(controlManager) {
		shared_ptr<FamilyBasicDataRecorder> dataRecorder = std::dynamic_pointer_cast<FamilyBasicDataRecorder>(controlManager->GetControlDevice(BaseControlDevice::ExpDevicePort2));
		if(dataRecorder) {
			Console::Pause();
			dataRecorder->StartRecording(filepath);
			Console::Resume();
		}
	}
}

void Console::StopRecordingTapeFile()
{
	shared_ptr<ControlManager> controlManager = _controlManager;
	if(controlManager) {
		shared_ptr<FamilyBasicDataRecorder> dataRecorder = std::dynamic_pointer_cast<FamilyBasicDataRecorder>(controlManager->GetControlDevice(BaseControlDevice::ExpDevicePort2));
		if(dataRecorder) {
			Console::Pause();
			dataRecorder->StopRecording();
			Console::Resume();
		}
	}
}

bool Console::IsRecordingTapeFile()
{
	shared_ptr<ControlManager> controlManager = _controlManager;
	if(controlManager) {
		shared_ptr<FamilyBasicDataRecorder> dataRecorder = std::dynamic_pointer_cast<FamilyBasicDataRecorder>(controlManager->GetControlDevice(BaseControlDevice::ExpDevicePort2));
		if(dataRecorder) {
			return dataRecorder->IsRecording();
		}
	}

	return false;
}

uint8_t* Console::GetRamBuffer(DebugMemoryType memoryType, uint32_t &size, int32_t &startAddr)
{
	//Only used by libretro port for achievements - should not be used by anything else.
	switch(memoryType) {
		default: break;

		case DebugMemoryType::InternalRam:
			size = MemoryManager::InternalRAMSize;
			startAddr = 0;
			return _memoryManager->GetInternalRAM();

		case DebugMemoryType::SaveRam:
			size = _mapper->GetMemorySize(DebugMemoryType::SaveRam);
			startAddr = _mapper->FromAbsoluteAddress(0, AddressType::SaveRam);
			return _mapper->GetSaveRam();

		case DebugMemoryType::WorkRam:
			size = _mapper->GetMemorySize(DebugMemoryType::WorkRam);
			startAddr = _mapper->FromAbsoluteAddress(0, AddressType::WorkRam);
			return _mapper->GetWorkRam();
	}

	throw std::runtime_error("unsupported memory type");
}

void Console::DebugAddTrace(const char * log)
{
	if(_debugger) {
		_debugger->AddTrace(log);
	}
}

void Console::DebugProcessPpuCycle()
{
	if(_debugger) {
		_debugger->ProcessPpuCycle();
	}
}

void Console::DebugProcessEvent(EventType type)
{
	if(_debugger) {
		_debugger->ProcessEvent(type);
	}
}

void Console::DebugProcessInterrupt(uint16_t cpuAddr, uint16_t destCpuAddr, bool forNmi)
{
	if(_debugger) {
		_debugger->ProcessInterrupt(cpuAddr, destCpuAddr, forNmi);
	}
}

void Console::DebugSetLastFramePpuScroll(uint16_t addr, uint8_t xScroll, bool updateHorizontalScrollOnly)
{
	if(_debugger) {
		_debugger->SetLastFramePpuScroll(addr, xScroll, updateHorizontalScrollOnly);
	}
}

bool Console::DebugProcessRamOperation(MemoryOperationType type, uint16_t & addr, uint8_t & value)
{
	if(_debugger) {
		return _debugger->ProcessRamOperation(type, addr, value);
	}
	return true;
}

void Console::DebugProcessVramReadOperation(MemoryOperationType type, uint16_t addr, uint8_t & value)
{
	if(_debugger) {
		_debugger->ProcessVramReadOperation(type, addr, value);
	}
}

void Console::DebugProcessVramWriteOperation(uint16_t addr, uint8_t & value)
{
	if(_debugger) {
		_debugger->ProcessVramWriteOperation(addr, value);
	}
}

void Console::DisplayDebugInformation(Timer &clockTimer, Timer &lastFrameTimer, double &lastFrameMin, double &lastFrameMax, double *timeLagData)
{
	AudioStatistics stats = _soundMixer->GetStatistics();
	
	int startFrame = _ppu->GetFrameCount();

	_debugHud->DrawRectangle(8, 8, 115, 40, 0x40000000, true, 1, startFrame);
	_debugHud->DrawRectangle(8, 8, 115, 40, 0xFFFFFF, false, 1, startFrame);

	_debugHud->DrawString(10, 10, "Audio Stats", 0xFFFFFF, 0xFF000000, 1, startFrame);
	_debugHud->DrawString(10, 21, "Latency: ", 0xFFFFFF, 0xFF000000, 1, startFrame);
	
	int color = (stats.AverageLatency > 0 && std::abs(stats.AverageLatency - EmulationSettings::GetAudioLatency()) > 3) ? 0xFF0000 : 0xFFFFFF;
	std::stringstream ss;
	ss << std::fixed << std::setprecision(2) << stats.AverageLatency << " ms";
	_debugHud->DrawString(54, 21, ss.str(), color, 0xFF000000, 1, startFrame);

	_debugHud->DrawString(10, 30, "Underruns: " + std::to_string(stats.BufferUnderrunEventCount), 0xFFFFFF, 0xFF000000, 1, startFrame);
	_debugHud->DrawString(10, 39, "Buffer Size: " + std::to_string(stats.BufferSize / 1024) + "kb", 0xFFFFFF, 0xFF000000, 1, startFrame);

	_debugHud->DrawRectangle(136, 8, 115, 58, 0x40000000, true, 1, startFrame);
	_debugHud->DrawRectangle(136, 8, 115, 58, 0xFFFFFF, false, 1, startFrame);
	_debugHud->DrawString(138, 10, "Video Stats", 0xFFFFFF, 0xFF000000, 1, startFrame);

	ss = std::stringstream();
	ss << "Exec Time: " << std::fixed << std::setprecision(2) << clockTimer.GetElapsedMS() << " ms";
	_debugHud->DrawString(138, 21, ss.str(), 0xFFFFFF, 0xFF000000, 1, startFrame);

	double avgTimeLag = 0;
	for(int i = 0; i < 16; i++) {
		avgTimeLag += timeLagData[i];
	}
	avgTimeLag /= 16;

	ss = std::stringstream();
	ss << "Time Gap: " << std::fixed << std::setprecision(2) << avgTimeLag << " ms";
	_debugHud->DrawString(138, 30, ss.str(), 0xFFFFFF, 0xFF000000, 1, startFrame);

	double lastFrame = lastFrameTimer.GetElapsedMS();

	ss = std::stringstream();
	ss << "Last Frame: " << std::fixed << std::setprecision(2) << lastFrame << " ms";
	_debugHud->DrawString(138, 39, ss.str(), 0xFFFFFF, 0xFF000000, 1, startFrame);

	if(_ppu->GetFrameCount() > 60) {
		lastFrameMin = (std::min)(lastFrame, lastFrameMin);
		lastFrameMax = (std::max)(lastFrame, lastFrameMax);
	} else {
		lastFrameMin = 9999;
		lastFrameMax = 0;
	}

	ss = std::stringstream();
	ss << "Min Delay: " << std::fixed << std::setprecision(2) << ((lastFrameMin < 9999) ? lastFrameMin : 0.0) << " ms";
	_debugHud->DrawString(138, 48, ss.str(), 0xFFFFFF, 0xFF000000, 1, startFrame);

	ss = std::stringstream();
	ss << "Max Delay: " << std::fixed << std::setprecision(2) << lastFrameMax << " ms";
	_debugHud->DrawString(138, 57, ss.str(), 0xFFFFFF, 0xFF000000, 1, startFrame);
}

