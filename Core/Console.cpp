#include "stdafx.h"
#include <thread>
#include "Console.h"
#include "FileLoader.h"
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
#include "RomLoader.h"
#include "EmulationSettings.h"
#include "../Utilities/sha1.h"
#include "../Utilities/Timer.h"
#include "../Utilities/FolderUtilities.h"
#include "../Utilities/PlatformUtilities.h"
#include "HdBuilderPpu.h"
#include "HdPpu.h"
#include "NsfPpu.h"
#include "SoundMixer.h"
#include "NsfMapper.h"
#include "ShortcutKeyHandler.h"
#include "MovieManager.h"
#include "RewindManager.h"
#include "SaveStateManager.h"
#include "HdPackBuilder.h"

shared_ptr<Console> Console::Instance(new Console());

Console::Console()
{
	_resetRequested = false;
	_lagCounter = 0;
	_archiveFileIndex = -1;
}

Console::~Console()
{
	MovieManager::Stop();
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

bool Console::Initialize(string romFilename, stringstream *filestream, string patchFilename, int32_t archiveFileIndex)
{
	SoundMixer::StopAudio();

	if(!_romFilepath.empty() && _mapper) {
		//Ensure we save any battery file before loading a new game
		_mapper->SaveBattery();

		//Save current game state before loading another one
		SaveStateManager::SaveRecentGame(_mapper->GetRomName(), _romFilepath, _patchFilename, _archiveFileIndex);
	}
	
	vector<uint8_t> fileData;	
	if(FileLoader::LoadFile(romFilename, filestream, archiveFileIndex, fileData)) {
		LoadHdPack(romFilename, fileData, patchFilename);
		if(!patchFilename.empty()) {
			FileLoader::ApplyPatch(patchFilename, fileData);
		}

		shared_ptr<BaseMapper> mapper = MapperFactory::InitializeFromFile(romFilename, fileData);
		if(mapper) {
			if(_mapper) {
				//Send notification only if a game was already running and we successfully loaded the new one
				MessageManager::SendNotification(ConsoleNotificationType::GameStopped);
			}

			_romFilepath = romFilename;
			_patchFilename = patchFilename;
			_archiveFileIndex = archiveFileIndex;

			_autoSaveManager.reset(new AutoSaveManager());
			VideoDecoder::GetInstance()->StopThread();
			
			_mapper = mapper;
			_memoryManager.reset(new MemoryManager(_mapper));
			_cpu.reset(new CPU(_memoryManager.get()));

			if(_hdData) {
				_ppu.reset(new HdPpu(_mapper.get()));
			} else if(NsfMapper::GetInstance()) {
				//Disable most of the PPU for NSFs
				_ppu.reset(new NsfPpu(_mapper.get()));
			} else {
				_ppu.reset(new PPU(_mapper.get()));
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

			_rewindManager.reset(new RewindManager());

			VideoDecoder::GetInstance()->StartThread();

			FolderUtilities::AddKnownGameFolder(FolderUtilities::GetFolderName(romFilename));

			string modelName = _model == NesModel::PAL ? "PAL" : (_model == NesModel::Dendy ? "Dendy" : "NTSC");
			string messageTitle = MessageManager::Localize("GameLoaded") + " (" + modelName + ")";
			MessageManager::DisplayMessage(messageTitle, FolderUtilities::GetFilename(_mapper->GetRomName(), false));
			if(EmulationSettings::GetOverclockRate() != 100) {
				MessageManager::DisplayMessage("ClockRate", std::to_string(EmulationSettings::GetOverclockRate()) + "%");
			}
			return true;
		}
	}

	MessageManager::DisplayMessage("Error", "CouldNotLoadFile", FolderUtilities::GetFilename(romFilename, true));
	return false;
}

bool Console::LoadROM(string filepath, stringstream *filestream, int32_t archiveFileIndex, string patchFilepath)
{
	Console::Pause();
	bool result = Instance->Initialize(filepath, filestream, patchFilepath, archiveFileIndex);
	Console::Resume();
	return result;
}

bool Console::LoadROM(string romName, uint32_t crc32Hash)
{
	HashInfo hashInfo;
	hashInfo.Crc32Hash = crc32Hash;
	return Console::LoadROM(romName, hashInfo);
}

bool Console::LoadROM(string romName, string sha1Hash)
{
	HashInfo hashInfo;
	hashInfo.Sha1Hash = sha1Hash;
	return Console::LoadROM(romName, hashInfo);
}

bool Console::LoadROM(string romName, HashInfo hashInfo)
{
	string currentRomFilepath = Console::GetROMPath();
	string currentFolder = FolderUtilities::GetFolderName(currentRomFilepath);
	if(!currentRomFilepath.empty()) {
		HashInfo gameHashInfo = Instance->_mapper->GetHashInfo();
		if(gameHashInfo.Crc32Hash == hashInfo.Crc32Hash || gameHashInfo.Sha1Hash.compare(hashInfo.Sha1Hash) == 0 || gameHashInfo.PrgChrMd5Hash.compare(hashInfo.PrgChrMd5Hash) == 0) {
			//Current game matches, no need to do anything
			return true;
		}
	}

	int32_t archiveFileIndex = -1;
	for(string folder : FolderUtilities::GetKnownGameFolders()) {
		string match = RomLoader::FindMatchingRomInFolder(folder, romName, hashInfo, true, archiveFileIndex);
		if(!match.empty()) {
			return Console::LoadROM(match, nullptr, archiveFileIndex);
		}
	}

	//Perform slow CRC32 search for ROM
	for(string folder : FolderUtilities::GetKnownGameFolders()) {
		string match = RomLoader::FindMatchingRomInFolder(folder, romName, hashInfo, false, archiveFileIndex);
		if(!match.empty()) {
			return Console::LoadROM(match, nullptr, archiveFileIndex);
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

RomFormat Console::GetRomFormat()
{
	if(Instance->_mapper) {
		return Instance->_mapper->GetRomFormat();
	} else {
		return RomFormat::Unknown;
	}
}

bool Console::IsChrRam()
{
	if(Instance->_mapper) {
		return Instance->_mapper->HasChrRam();
	} else {
		return false;
	}
}

uint32_t Console::GetCrc32()
{
	if(Instance->_mapper) {
		return Instance->_mapper->GetHashInfo().Crc32Hash;
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

void Console::PowerCycle()
{
	if(Instance->_initialized && !Instance->_romFilepath.empty()) {
		LoadROM(Instance->_romFilepath, nullptr, Instance->_archiveFileIndex, Instance->_patchFilename);
	}
}

void Console::Reset(bool softReset)
{
	if(Instance->_initialized) {
		if(softReset && EmulationSettings::CheckFlag(EmulationFlags::DisablePpuReset)) {
			//Allow mid-frame resets to allow the PPU to get out-of-sync
			RequestReset();
		} else {
			MovieManager::Stop();
			SoundMixer::StopRecording();

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
}

void Console::ResetComponents(bool softReset)
{
	MovieManager::Stop();
	if(!softReset) {
		SoundMixer::StopRecording();
		_hdPackBuilder.reset();
	}
	
	_memoryManager->Reset(softReset);
	if(!EmulationSettings::CheckFlag(EmulationFlags::DisablePpuReset) || !softReset) {
		_ppu->Reset();
	}
	_apu->Reset(softReset);
	_cpu->Reset(softReset, _model);
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

	UpdateNesModel(true);

	bool crashed = false;
	while(true) { 
		try {
			_cpu->Exec();
		} catch(const std::runtime_error &ex) {
			crashed = true;
			MessageManager::DisplayMessage("Error", "GameCrash", ex.what());
			break;
		}

		if(_resetRequested) {
			//Used by NSF player to reset console after changing track
			//Also used with DisablePpuReset option to reset mid-frame
			MovieManager::Stop();
			ResetComponents(true);
			_resetRequested = false;
		}

		uint32_t currentFrameNumber = PPU::GetFrameCount();
		if(currentFrameNumber != lastFrameNumber) {
			if(_controlManager->GetLagFlag()) {
				_lagCounter++;
			}

			_rewindManager->ProcessEndOfFrame();
			EmulationSettings::DisableOverclocking(_disableOcNextFrame || NsfMapper::GetInstance());
			_disableOcNextFrame = false;

			lastFrameNumber = PPU::GetFrameCount();

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
				MessageManager::SendNotification(ConsoleNotificationType::GamePaused);
				
				//Prevent audio from looping endlessly while game is paused
				SoundMixer::StopAudio();

				_runLock.Release();
				
				PlatformUtilities::EnableScreensaver();
				while(paused && !_stop && _debugger == nullptr) {
					//Sleep until emulation is resumed
					std::this_thread::sleep_for(std::chrono::duration<int, std::milli>(30));
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
			
			if(_stop) {
				_stop = false;
				break;
			}
		}
	}

	if(!crashed) {
		SaveStateManager::SaveRecentGame(_mapper->GetRomName(), _romFilepath, _patchFilename, _archiveFileIndex);
	}

	MessageManager::SendNotification(ConsoleNotificationType::GameStopped);

	_rewindManager.reset();
	SoundMixer::StopAudio();
	MovieManager::Stop();
	SoundMixer::StopRecording();
	PlatformUtilities::EnableScreensaver();

	_autoSaveManager.reset();

	VideoDecoder::GetInstance()->StopThread();

	EmulationSettings::ClearFlags(EmulationFlags::Paused);

	_initialized = false;

	if(!_romFilepath.empty() && _mapper) {
		//Ensure we save any battery file before unloading anything
		_mapper->SaveBattery();
	}

	_romFilepath = "";
	_mapper.reset();
	_ppu.reset();
	_cpu.reset();
	_memoryManager.reset();
	_controlManager.reset();

	_hdPackBuilder.reset();
	_hdData.reset();

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
		MovieManager::Stop();

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
	//Send any unprocessed sound to the SoundMixer - needed for rewind
	Instance->_apu->EndFrame();

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

void Console::SetNextFrameOverclockStatus(bool disabled)
{
	Instance->_disableOcNextFrame = disabled;
}

HdPackData* Console::GetHdData()
{
	return Instance->_hdData.get();
}

void Console::LoadHdPack(string romFilename, vector<uint8_t> &fileData, string &patchFilename)
{
	_hdData.reset();
	if(EmulationSettings::CheckFlag(EmulationFlags::UseHdPacks)) {
		string hdPackFolder = FolderUtilities::CombinePath(FolderUtilities::GetHdPackFolder(), FolderUtilities::GetFilename(romFilename, false));
		string hdPackDefinitionFile = FolderUtilities::CombinePath(hdPackFolder, "hires.txt");
		_hdData.reset(new HdPackData());
		if(!HdPackLoader::LoadHdNesPack(hdPackDefinitionFile, *_hdData.get())) {
			_hdData.reset();
		} else {
			string sha1hash = SHA1::GetHash(fileData);
			auto result = _hdData->PatchesByHash.find(sha1hash);
			if(result != _hdData->PatchesByHash.end()) {
				patchFilename = FolderUtilities::CombinePath(hdPackFolder, result->second);
			}
		}
	}
}

void Console::StartRecordingHdPack(string saveFolder, ScaleFilterType filterType, uint32_t scale, uint32_t flags, uint32_t chrRamBankSize)
{
	Console::Pause();
	std::stringstream saveState;
	Instance->SaveState(saveState);
	
	Instance->_hdPackBuilder.reset();
	Instance->_hdPackBuilder.reset(new HdPackBuilder(saveFolder, filterType, scale, flags, chrRamBankSize, !Instance->_mapper->HasChrRom()));

	Instance->_memoryManager->UnregisterIODevice(Instance->_ppu.get());
	Instance->_ppu.reset();
	Instance->_ppu.reset(new HdBuilderPpu(Instance->_mapper.get(), Instance->_hdPackBuilder.get(), chrRamBankSize));
	Instance->_memoryManager->RegisterIODevice(Instance->_ppu.get());

	Instance->LoadState(saveState);
	Console::Resume();
}

void Console::StopRecordingHdPack()
{
	Console::Pause();
	std::stringstream saveState;
	Instance->SaveState(saveState);
	
	Instance->_memoryManager->UnregisterIODevice(Instance->_ppu.get());
	Instance->_ppu.reset();
	Instance->_ppu.reset(new PPU(Instance->_mapper.get()));
	Instance->_memoryManager->RegisterIODevice(Instance->_ppu.get());

	Instance->_hdPackBuilder.reset();

	Instance->LoadState(saveState);
	Console::Resume();
}