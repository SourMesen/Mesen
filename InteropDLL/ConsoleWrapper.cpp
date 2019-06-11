#include "stdafx.h"

#include "../Core/MessageManager.h"
#include "../Core/NotificationManager.h"
#include "../Core/Console.h"
#include "../Core/GameServer.h"
#include "../Core/GameClient.h"
#include "../Core/ClientConnectionData.h"
#include "../Core/SaveStateManager.h"
#include "../Core/CheatManager.h"
#include "../Core/EmulationSettings.h"
#include "../Core/VideoDecoder.h"
#include "../Core/VideoRenderer.h"
#include "../Core/HistoryViewer.h"
#include "../Core/AutomaticRomTest.h"
#include "../Core/RecordedRomTest.h"
#include "../Core/FDS.h"
#include "../Core/VsControlManager.h"
#include "../Core/SoundMixer.h"
#include "../Core/RomLoader.h"
#include "../Core/NsfMapper.h"
#include "../Core/IRenderingDevice.h"
#include "../Core/IAudioDevice.h"
#include "../Core/MovieManager.h"
#include "../Core/VirtualFile.h"
#include "../Core/HdPackBuilder.h"
#include "../Utilities/AviWriter.h"
#include "../Core/ShortcutKeyHandler.h"
#include "../Core/SystemActionManager.h"
#include "../Core/FdsSystemActionManager.h"
#include "../Core/VsSystemActionManager.h"
#include "../Core/KeyManager.h"
#include "../Core/GameDatabase.h"
#include "../Core/RewindManager.h"
#include "../Utilities/SimpleLock.h"

#ifdef _WIN32
	#include "../Windows/Renderer.h"
	#include "../Windows/SoundManager.h"
	#include "../Windows/WindowsKeyManager.h"
#else
	#include "../Linux/SdlRenderer.h"
	#include "../Linux/SdlSoundManager.h"
	#include "../Linux/LinuxKeyManager.h"
#endif

unique_ptr<IRenderingDevice> _renderer;
unique_ptr<IAudioDevice> _soundManager;
unique_ptr<IKeyManager> _keyManager;
unique_ptr<ShortcutKeyHandler> _shortcutKeyHandler;

unique_ptr<IRenderingDevice> _dualRenderer;
unique_ptr<IAudioDevice> _dualSoundManager;

shared_ptr<Console> _historyConsole;
unique_ptr<IRenderingDevice> _historyRenderer;
unique_ptr<IAudioDevice> _historySoundManager;

void* _windowHandle = nullptr;
void* _viewerHandle = nullptr;
string _returnString;
string _logString;
shared_ptr<Console> _console;
EmulationSettings *_settings = nullptr;
shared_ptr<RecordedRomTest> _recordedRomTest;
SimpleLock _externalNotificationListenerLock;
vector<shared_ptr<INotificationListener>> _externalNotificationListeners;

typedef void (__stdcall *NotificationListenerCallback)(int, void*);

shared_ptr<Console> GetConsoleById(ConsoleId consoleId)
{
	shared_ptr<Console> console;
	switch(consoleId) {
		case ConsoleId::Master: console = _console; break;
		case ConsoleId::Slave: console = _console->GetDualConsole(); break;
		case ConsoleId::HistoryViewer: console = _historyConsole; break;
	}

	if(!console) {
		//Otherwise return the main CPU
		console = _console;
	}
	return console;
}

namespace InteropEmu {
	class InteropNotificationListener : public INotificationListener
	{
		NotificationListenerCallback _callback;
	public:
		InteropNotificationListener(NotificationListenerCallback callback)
		{
			_callback = callback;
		}

		virtual ~InteropNotificationListener()
		{
		}
		
		void ProcessNotification(ConsoleNotificationType type, void* parameter)
		{
			_callback((int)type, parameter);
		}
	};

	struct InteropRomInfo
	{
		const char* RomName;
		uint32_t Crc32;
		uint32_t PrgCrc32;
		RomFormat Format;
		bool IsChrRam;
		uint16_t MapperId;
		uint32_t FilePrgOffset;
		char Sha1[40];
	};

	extern "C" {
		DllExport bool __stdcall TestDll()
		{
			return true;
		}

		DllExport uint32_t __stdcall GetMesenVersion() { return EmulationSettings::GetMesenVersion(); }

		DllExport void __stdcall InitDll()
		{
			_console.reset(new Console());
			_console->Init();
			_settings = _console->GetSettings();
		}

		DllExport void __stdcall InitializeEmu(const char* homeFolder, void *windowHandle, void *viewerHandle, bool noAudio, bool noVideo, bool noInput)
		{
			FolderUtilities::SetHomeFolder(homeFolder);
			_shortcutKeyHandler.reset(new ShortcutKeyHandler(_console));

			if(windowHandle != nullptr && viewerHandle != nullptr) {
				_windowHandle = windowHandle;
				_viewerHandle = viewerHandle;

				if(!noVideo) {
					#ifdef _WIN32
						_renderer.reset(new Renderer(_console, (HWND)_viewerHandle, true));
					#else 
						_renderer.reset(new SdlRenderer(_console, _viewerHandle, true));
					#endif
				} 

				if(!noAudio) {
					#ifdef _WIN32
						_soundManager.reset(new SoundManager(_console, (HWND)_windowHandle));
					#else
						_soundManager.reset(new SdlSoundManager(_console));
					#endif
				}

				if(!noInput) {
					#ifdef _WIN32
						_keyManager.reset(new WindowsKeyManager(_console, (HWND)_windowHandle));
					#else 
						_keyManager.reset(new LinuxKeyManager(_console));
					#endif				
					
					KeyManager::RegisterKeyManager(_keyManager.get());
				}
			}
		}
		
		DllExport void __stdcall InitializeDualSystem(void *windowHandle, void *viewerHandle)
		{
			shared_ptr<Console> slaveConsole = _console->GetDualConsole();
			if(slaveConsole){
				_console->Pause();
				#ifdef _WIN32
					_dualRenderer.reset(new Renderer(slaveConsole, (HWND)viewerHandle, false));
					_dualSoundManager.reset(new SoundManager(slaveConsole, (HWND)windowHandle));
				#else 
					_dualRenderer.reset(new SdlRenderer(slaveConsole, viewerHandle, false));
					_dualSoundManager.reset(new SdlSoundManager(slaveConsole));
				#endif
				_console->Resume();
			}
		}

		DllExport void __stdcall ReleaseDualSystemAudioVideo()
		{
			_console->Pause();
			_dualRenderer.reset();
			_dualSoundManager.reset();
			_console->Resume();
		}

		DllExport bool __stdcall HistoryViewerEnabled()
		{
			shared_ptr<RewindManager> rewindManager = _console->GetRewindManager();
			return rewindManager ? rewindManager->HasHistory() : false;
		}

		DllExport void __stdcall HistoryViewerInitialize(void *windowHandle, void *viewerHandle)
		{
			_historyConsole.reset(new Console(nullptr, _settings));
			_historyConsole->Init();
			_historyConsole->Initialize(_console->GetRomPath(), _console->GetPatchFile());
			_historyConsole->CopyRewindData(_console);

			//Force some settings
			_historyConsole->GetSettings()->SetEmulationSpeed(100);
			_historyConsole->GetSettings()->SetVideoScale(2);
			_historyConsole->GetSettings()->ClearFlags(EmulationFlags::InBackground | EmulationFlags::Rewind | EmulationFlags::ForceMaxSpeed | EmulationFlags::DebuggerWindowEnabled);

			#ifdef _WIN32
				_historyRenderer.reset(new Renderer(_historyConsole, (HWND)viewerHandle, false));
				_historySoundManager.reset(new SoundManager(_historyConsole, (HWND)windowHandle));
			#else 
				_historyRenderer.reset(new SdlRenderer(_historyConsole, viewerHandle, false));
				_historySoundManager.reset(new SdlSoundManager(_historyConsole));
			#endif
		}

		DllExport void __stdcall HistoryViewerRelease(void *windowHandle, void *viewerHandle)
		{
			_historyConsole->Stop();
			_historyConsole->Release(true);
			_historyRenderer.reset();
			_historySoundManager.reset();
			_historyConsole.reset();
		}

		DllExport void __stdcall HistoryViewerRun()
		{
			if(_historyConsole) {
				_historyConsole->Run();
			}
		}

		DllExport uint32_t __stdcall HistoryViewerGetHistoryLength()
		{
			if(_historyConsole) {
				return _historyConsole->GetHistoryViewer()->GetHistoryLength();
			}
			return 0;
		}

		DllExport void __stdcall HistoryViewerGetSegments(uint32_t* segmentBuffer, uint32_t &bufferSize)
		{
			if(_historyConsole) {
				_historyConsole->GetHistoryViewer()->GetHistorySegments(segmentBuffer, bufferSize);
			}
		}

		DllExport bool __stdcall HistoryViewerCreateSaveState(const char* outputFile, uint32_t position)
		{
			if(_historyConsole) {
				return _historyConsole->GetHistoryViewer()->CreateSaveState(outputFile, position);
			}
			return false;
		}

		DllExport bool __stdcall HistoryViewerSaveMovie(const char* movieFile, uint32_t startPosition, uint32_t endPosition)
		{
			if(_historyConsole) {
				return _historyConsole->GetHistoryViewer()->SaveMovie(movieFile, startPosition, endPosition);
			}
			return false;
		}

		DllExport void __stdcall HistoryViewerResumeGameplay(uint32_t resumeAtSecond)
		{
			if(_historyConsole) {
				_historyConsole->GetHistoryViewer()->ResumeGameplay(_console, resumeAtSecond);
			}
		}

		DllExport void __stdcall HistoryViewerSetPosition(uint32_t seekPosition)
		{
			if(_historyConsole) {
				_historyConsole->GetHistoryViewer()->SeekTo(seekPosition);
			}
		}

		DllExport uint32_t __stdcall HistoryViewerGetPosition()
		{
			if(_historyConsole) {
				return _historyConsole->GetHistoryViewer()->GetPosition();
			}
			return 0;
		}

		DllExport void __stdcall SetFullscreenMode(bool fullscreen, void *windowHandle, uint32_t monitorWidth, uint32_t monitorHeight)
		{
			if(_renderer) {
				_renderer->SetFullscreenMode(fullscreen, windowHandle, monitorWidth, monitorHeight);
			}
		}

		DllExport bool __stdcall IsRunning() { return _console->IsRunning(); }
		DllExport int32_t __stdcall GetStopCode() { return _console->GetStopCode(); }

		DllExport void __stdcall LoadROM(char* filename, char* patchFile) { _console->Initialize(filename, patchFile); }
		DllExport void __stdcall AddKnownGameFolder(char* folder) { FolderUtilities::AddKnownGameFolder(folder); }
		DllExport void __stdcall SetFolderOverrides(char* saveFolder, char* saveStateFolder, char* screenshotFolder) { FolderUtilities::SetFolderOverrides(saveFolder, saveStateFolder, screenshotFolder); }
		DllExport void __stdcall LoadRecentGame(char* filepath, bool resetGame) { _console->GetSaveStateManager()->LoadRecentGame(filepath, resetGame); }

		DllExport const char* __stdcall GetArchiveRomList(char* filename) { 
			std::ostringstream out;
			shared_ptr<ArchiveReader> reader = ArchiveReader::GetReader(filename);
			if(reader) {
				for(string romName : reader->GetFileList(VirtualFile::RomExtensions)) {
					out << romName << "[!|!]";
				}
			}
			_returnString = out.str();
			return _returnString.c_str();
		}

		DllExport void __stdcall SetControllerType(uint32_t port, ControllerType type) { _settings->SetControllerType(port, type); }
		DllExport void __stdcall SetControllerKeys(uint32_t port, KeyMappingSet mappings) { _settings->SetControllerKeys(port, mappings); }
		DllExport void __stdcall SetZapperDetectionRadius(uint32_t detectionRadius) { _settings->SetZapperDetectionRadius(detectionRadius); }
		DllExport void __stdcall SetControllerDeadzoneSize(uint32_t deadzoneSize) { _settings->SetControllerDeadzoneSize(deadzoneSize); }
		DllExport void __stdcall SetExpansionDevice(ExpansionPortDevice device) { _settings->SetExpansionDevice(device); }
		DllExport void __stdcall SetConsoleType(ConsoleType type) { _settings->SetConsoleType(type); }
		DllExport void __stdcall SetMouseSensitivity(MouseDevice device, double sensitivity) { _settings->SetMouseSensitivity(device, sensitivity); }
		
		DllExport void __stdcall ClearShortcutKeys() { _settings->ClearShortcutKeys(); }
		DllExport void __stdcall SetShortcutKey(EmulatorShortcut shortcut, KeyCombination keyCombination, int keySetIndex) { _settings->SetShortcutKey(shortcut, keyCombination, keySetIndex); }

		DllExport ControllerType __stdcall GetControllerType(uint32_t port) { return _settings->GetControllerType(port); }
		DllExport ExpansionPortDevice GetExpansionDevice() { return _settings->GetExpansionDevice(); }
		DllExport ConsoleType __stdcall GetConsoleType() { return _settings->GetConsoleType(); }
		
		DllExport bool __stdcall HasZapper() { return _settings->HasZapper(); }
		DllExport bool __stdcall HasFourScore() { return _settings->CheckFlag(EmulationFlags::HasFourScore); }
		DllExport bool __stdcall HasArkanoidPaddle() { return _settings->HasArkanoidPaddle(); }

		DllExport void __stdcall SetMousePosition(double x, double y) { KeyManager::SetMousePosition(x, y); }
		DllExport void __stdcall SetMouseMovement(int16_t x, int16_t y) { KeyManager::SetMouseMovement(x, y); }

		DllExport void __stdcall UpdateInputDevices() { if(_keyManager) { _keyManager->UpdateDevices(); } }
		DllExport void __stdcall GetPressedKeys(uint32_t *keyBuffer) { 
			vector<uint32_t> pressedKeys = KeyManager::GetPressedKeys();
			for(size_t i = 0; i < pressedKeys.size() && i < 3; i++) {
				keyBuffer[i] = pressedKeys[i];
			}
		}
		DllExport void __stdcall DisableAllKeys(bool disabled) {
			if(_keyManager) {
				_keyManager->SetDisabled(disabled);
			}
		}
		DllExport void __stdcall SetKeyState(int32_t scanCode, bool state) { 
			if(_keyManager) { 
				_keyManager->SetKeyState(scanCode, state); 
				_shortcutKeyHandler->ProcessKeys();
			} 
		}
		DllExport void __stdcall ResetKeyState() { if(_keyManager) { _keyManager->ResetKeyState(); } }
		DllExport const char* __stdcall GetKeyName(uint32_t keyCode) 
		{
			_returnString = KeyManager::GetKeyName(keyCode);
			return _returnString.c_str();
		}
		DllExport uint32_t __stdcall GetKeyCode(char* keyName) { 
			if(keyName) {
				return KeyManager::GetKeyCode(keyName);
			} else {
				return 0;
			}
		}

		DllExport void __stdcall Run()
		{
			if(_console) {
				_console->Run();
			}
		}

		DllExport void __stdcall Resume(ConsoleId consoleId) { GetConsoleById(consoleId)->GetSettings()->ClearFlags(EmulationFlags::Paused); }
		DllExport bool __stdcall IsPaused(ConsoleId consoleId) { return GetConsoleById(consoleId)->GetSettings()->CheckFlag(EmulationFlags::Paused); }
		DllExport void __stdcall Pause(ConsoleId consoleId)
		{
			if(!GameClient::Connected()) {
				GetConsoleById(consoleId)->GetSettings()->SetFlags(EmulationFlags::Paused);
			}
		}
		
		DllExport void __stdcall Stop()
		{
			if(_console) {
				GameServer::StopServer();
				GameClient::Disconnect();
				_console->Stop();
			}
		}

		DllExport void __stdcall GetRomInfo(InteropRomInfo &interopRomInfo, char* filename)
		{
			string romPath = filename;
			if(romPath.empty()) {
				_returnString = _console->GetRomPath();
				interopRomInfo.RomName = _returnString.c_str();
				RomInfo romInfo = _console->GetRomInfo();
				interopRomInfo.Crc32 = romInfo.Hash.Crc32;
				interopRomInfo.PrgCrc32 = romInfo.Hash.PrgCrc32;
				interopRomInfo.Format = romInfo.Format;
				interopRomInfo.IsChrRam = romInfo.HasChrRam;
				interopRomInfo.MapperId = romInfo.MapperID;
				interopRomInfo.FilePrgOffset = romInfo.FilePrgOffset;
				if(romInfo.Hash.Sha1.size() == 40) {
					memcpy(interopRomInfo.Sha1, romInfo.Hash.Sha1.c_str(), 40);
				}
			} else {
				RomLoader romLoader(true);
				if(romLoader.LoadFile(romPath)) {
					RomData romData = romLoader.GetRomData();

					_returnString = romPath;
					interopRomInfo.RomName = _returnString.c_str();
					interopRomInfo.Crc32 = romData.Info.Hash.Crc32;
					interopRomInfo.PrgCrc32 = romData.Info.Hash.PrgCrc32;
					interopRomInfo.Format = RomFormat::Unknown;
					interopRomInfo.IsChrRam = romData.ChrRom.size() == 0;
					interopRomInfo.FilePrgOffset = romData.Info.FilePrgOffset;
					interopRomInfo.MapperId = 0;
					if(romData.Info.Hash.Sha1.size() == 40) {
						memcpy(interopRomInfo.Sha1, romData.Info.Hash.Sha1.c_str(), 40);
					}
				} else {
					_returnString = "";
					interopRomInfo.RomName = _returnString.c_str();
					interopRomInfo.Crc32 = 0;
					interopRomInfo.PrgCrc32 = 0;
					interopRomInfo.Format = RomFormat::Unknown;
					interopRomInfo.IsChrRam = false;
					interopRomInfo.FilePrgOffset = 0;
					interopRomInfo.MapperId = 0;
				}
			}
		}

		DllExport void __stdcall Reset() { _console->Reset(true); }
		DllExport void __stdcall PowerCycle() { _console->Reset(false); }
		DllExport void __stdcall ResetLagCounter() { _console->ResetLagCounter(); }

		DllExport void __stdcall StartServer(uint16_t port, char* password, char* hostPlayerName) { GameServer::StartServer(_console, port, password, hostPlayerName); }
		DllExport void __stdcall StopServer() { GameServer::StopServer(); }
		DllExport bool __stdcall IsServerRunning() { return GameServer::Started(); }

		DllExport void __stdcall Connect(char* host, uint16_t port, char* password, char* playerName, bool spectator)
		{
			ClientConnectionData connectionData(host, port, password, playerName, spectator);
			GameClient::Connect(_console, connectionData);
		}

		DllExport void __stdcall Disconnect() { GameClient::Disconnect(); }
		DllExport bool __stdcall IsConnected() { return GameClient::Connected(); }
		DllExport ControllerType __stdcall NetPlayGetControllerType(int32_t port) { return _settings->GetControllerType(port); }

		DllExport int32_t __stdcall NetPlayGetAvailableControllers() 
		{ 
			if(GameServer::Started()) {
				return GameServer::GetAvailableControllers();
			} else {
				return GameClient::GetAvailableControllers();
			}		
		}

		DllExport void __stdcall NetPlaySelectController(int32_t port)
		{
			if(GameServer::Started()) {
				return GameServer::SetHostControllerPort(port);
			} else {
				return GameClient::SelectController(port);
			}
		}

		DllExport int32_t __stdcall NetPlayGetControllerPort() 
		{
			if(GameServer::Started()) {
				return GameServer::GetHostControllerPort();
			} else {
				return GameClient::GetControllerPort();
			}
		}

		DllExport void __stdcall Release()
		{
			ReleaseDualSystemAudioVideo();
			_shortcutKeyHandler.reset();
			
			GameServer::StopServer();
			GameClient::Disconnect();

			_console->Stop();

			_renderer.reset();
			_soundManager.reset();
			_keyManager.reset();

			_console->Release(true);
			_console.reset();
			
			_shortcutKeyHandler.reset();
		}

		DllExport void __stdcall TakeScreenshot() { _console->GetVideoDecoder()->TakeScreenshot(); }

		DllExport INotificationListener* __stdcall RegisterNotificationCallback(ConsoleId consoleId, NotificationListenerCallback callback)
		{
			auto lock = _externalNotificationListenerLock.AcquireSafe();
			auto listener = shared_ptr<INotificationListener>(new InteropNotificationListener(callback));
			_externalNotificationListeners.push_back(listener);
			GetConsoleById(consoleId)->GetNotificationManager()->RegisterNotificationListener(listener);
			return listener.get();
		}

		DllExport void __stdcall UnregisterNotificationCallback(INotificationListener *listener)
		{
			auto lock = _externalNotificationListenerLock.AcquireSafe();
			_externalNotificationListeners.erase(
				std::remove_if(
					_externalNotificationListeners.begin(),
					_externalNotificationListeners.end(),
					[=](shared_ptr<INotificationListener> ptr) { return ptr.get() == listener; }
				),
				_externalNotificationListeners.end()
			);
		}

		DllExport void __stdcall DisplayMessage(char* title, char* message, char* param1) { MessageManager::DisplayMessage(title, message, param1 ? param1 : ""); }
		DllExport const char* __stdcall GetLog()
		{
			_logString = MessageManager::GetLog();
			return _logString.c_str();
		}

		DllExport void __stdcall WriteLogEntry(char* message) { MessageManager::Log(message); }

		DllExport void __stdcall SaveState(uint32_t stateIndex) { _console->GetSaveStateManager()->SaveState(stateIndex); }
		DllExport void __stdcall LoadState(uint32_t stateIndex) { _console->GetSaveStateManager()->LoadState(stateIndex); }
		DllExport void __stdcall SaveStateFile(char* filepath) { _console->GetSaveStateManager()->SaveState(filepath); }
		DllExport void __stdcall LoadStateFile(char* filepath) { _console->GetSaveStateManager()->LoadState(filepath); }
		DllExport int64_t  __stdcall GetStateInfo(uint32_t stateIndex) { return _console->GetSaveStateManager()->GetStateInfo(stateIndex); }

		DllExport void __stdcall MoviePlay(char* filename) { MovieManager::Play(string(filename), _console); }
		
		DllExport void __stdcall MovieRecord(RecordMovieOptions *options)
		{
			RecordMovieOptions opt = *options;
			MovieManager::Record(opt, _console);
		}

		DllExport void __stdcall MovieStop() { MovieManager::Stop(); }
		DllExport bool __stdcall MoviePlaying() { return MovieManager::Playing(); }
		DllExport bool __stdcall MovieRecording() { return MovieManager::Recording(); }

		DllExport void __stdcall AviRecord(char* filename, VideoCodec codec, uint32_t compressionLevel) { _console->GetVideoRenderer()->StartRecording(filename, codec, compressionLevel); }
		DllExport void __stdcall AviStop() { _console->GetVideoRenderer()->StopRecording(); }
		DllExport bool __stdcall AviIsRecording() { return _console->GetVideoRenderer()->IsRecording(); }

		DllExport void __stdcall WaveRecord(char* filename) { _console->GetSoundMixer()->StartRecording(filename); }
		DllExport void __stdcall WaveStop() { _console->GetSoundMixer()->StopRecording(); }
		DllExport bool __stdcall WaveIsRecording() { return _console->GetSoundMixer()->IsRecording(); }

		DllExport int32_t __stdcall RunRecordedTest(char* filename)
		{
			_recordedRomTest.reset(new RecordedRomTest(_console));
			_console->GetNotificationManager()->RegisterNotificationListener(_recordedRomTest);
			return _recordedRomTest->Run(filename);
		}

		DllExport int32_t __stdcall RunAutomaticTest(char* filename)
		{
			shared_ptr<AutomaticRomTest> romTest(new AutomaticRomTest());
			return romTest->Run(filename);
		}

		DllExport void __stdcall RomTestRecord(char* filename, bool reset) 
		{
			_recordedRomTest.reset(new RecordedRomTest(_console));
			_console->GetNotificationManager()->RegisterNotificationListener(_recordedRomTest);
			_recordedRomTest->Record(filename, reset);
		}

		DllExport void __stdcall RomTestRecordFromMovie(char* testFilename, char* movieFilename) 
		{
			_recordedRomTest.reset(new RecordedRomTest(_console));
			_console->GetNotificationManager()->RegisterNotificationListener(_recordedRomTest);
			_recordedRomTest->RecordFromMovie(testFilename, string(movieFilename));
		}

		DllExport void __stdcall RomTestRecordFromTest(char* newTestFilename, char* existingTestFilename) 
		{
			_recordedRomTest.reset(new RecordedRomTest(_console));
			_console->GetNotificationManager()->RegisterNotificationListener(_recordedRomTest);
			_recordedRomTest->RecordFromTest(newTestFilename, existingTestFilename);
		}

		DllExport void __stdcall RomTestStop() 
		{
			if(_recordedRomTest) {
				_recordedRomTest->Stop();
				_recordedRomTest.reset();
			}
		}

		DllExport bool __stdcall RomTestRecording() { return _recordedRomTest != nullptr; }

		DllExport void __stdcall SetCheats(CheatInfo cheats[], uint32_t length) { _console->GetCheatManager()->SetCheats(cheats, length); }

		DllExport void __stdcall SetOsdState(bool enabled) { MessageManager::SetOsdState(enabled); }
		DllExport void __stdcall SetGameDatabaseState(bool enabled) { GameDatabase::SetGameDatabaseState(enabled); }

		DllExport bool __stdcall CheckFlag(EmulationFlags flags) { return _settings->CheckFlag(flags); }
		DllExport void __stdcall SetFlags(EmulationFlags flags) { _settings->SetFlags(flags); }
		DllExport void __stdcall ClearFlags(EmulationFlags flags) { _settings->ClearFlags(flags); }
		DllExport void __stdcall SetRamPowerOnState(RamPowerOnState state) { _settings->SetRamPowerOnState(state); }
		DllExport void __stdcall SetDisplayLanguage(Language lang) { _settings->SetDisplayLanguage(lang); }
		DllExport void __stdcall SetChannelVolume(uint32_t channel, double volume) { _settings->SetChannelVolume((AudioChannel)channel, volume); }
		DllExport void __stdcall SetChannelPanning(uint32_t channel, double panning) { _settings->SetChannelPanning((AudioChannel)channel, panning); }
		DllExport void __stdcall SetEqualizerFilterType(EqualizerFilterType filter) { _settings->SetEqualizerFilterType(filter); }
		DllExport void __stdcall SetBandGain(uint32_t band, double gain) { _settings->SetBandGain(band, gain); }
		DllExport void __stdcall SetEqualizerBands(double *bands, uint32_t bandCount) { _settings->SetEqualizerBands(bands, bandCount); }
		DllExport void __stdcall SetMasterVolume(double volume, double volumeReduction, ConsoleId consoleId) { GetConsoleById(consoleId)->GetSettings()->SetMasterVolume(volume, volumeReduction); }
		DllExport void __stdcall SetSampleRate(uint32_t sampleRate) { _settings->SetSampleRate(sampleRate); }
		DllExport void __stdcall SetAudioLatency(uint32_t msLatency) { _settings->SetAudioLatency(msLatency); }
		DllExport void __stdcall SetAudioFilterSettings(AudioFilterSettings settings) { _settings->SetAudioFilterSettings(settings); }

		DllExport NesModel __stdcall GetNesModel() { return _console->GetModel(); }
		DllExport void __stdcall SetNesModel(uint32_t model) { _settings->SetNesModel((NesModel)model); }
		DllExport void __stdcall SetOverscanDimensions(uint32_t left, uint32_t right, uint32_t top, uint32_t bottom) { _settings->SetOverscanDimensions(left, right, top, bottom); }
		DllExport void __stdcall SetEmulationSpeed(uint32_t emulationSpeed) { _settings->SetEmulationSpeed(emulationSpeed, true); }
		DllExport void __stdcall IncreaseEmulationSpeed() { _settings->IncreaseEmulationSpeed(); }
		DllExport void __stdcall DecreaseEmulationSpeed() { _settings->DecreaseEmulationSpeed(); }
		DllExport uint32_t __stdcall GetEmulationSpeed() { return _settings->GetEmulationSpeed(true); }
		DllExport void __stdcall SetTurboRewindSpeed(uint32_t turboSpeed, uint32_t rewindSpeed) { _settings->SetTurboRewindSpeed(turboSpeed, rewindSpeed); }
		DllExport void __stdcall SetRewindBufferSize(uint32_t seconds) { _settings->SetRewindBufferSize(seconds); }
		DllExport bool __stdcall IsRewinding() {
			shared_ptr<RewindManager> rewindManager = _console->GetRewindManager();
			return rewindManager ? rewindManager->IsRewinding() : false;
		}
		DllExport void __stdcall SetOverclockRate(uint32_t overclockRate, bool adjustApu) { _settings->SetOverclockRate(overclockRate, adjustApu); }
		DllExport void __stdcall SetPpuNmiConfig(uint32_t extraScanlinesBeforeNmi, uint32_t extraScanlinesAfterNmi) { _settings->SetPpuNmiConfig(extraScanlinesBeforeNmi, extraScanlinesAfterNmi); }
		DllExport void __stdcall SetVideoScale(double scale, ConsoleId consoleId) { GetConsoleById(consoleId)->GetSettings()->SetVideoScale(scale); }
		DllExport void __stdcall SetScreenRotation(uint32_t angle) { _settings->SetScreenRotation(angle); }
		DllExport void __stdcall SetExclusiveRefreshRate(uint32_t angle) { _settings->SetExclusiveRefreshRate(angle); }
		DllExport void __stdcall SetVideoAspectRatio(VideoAspectRatio aspectRatio, double customRatio) { _settings->SetVideoAspectRatio(aspectRatio, customRatio); }
		DllExport void __stdcall SetVideoFilter(VideoFilterType filter) { _settings->SetVideoFilterType(filter); }
		DllExport void __stdcall SetVideoResizeFilter(VideoResizeFilter filter) { _settings->SetVideoResizeFilter(filter); }
		DllExport void __stdcall GetRgbPalette(uint32_t *paletteBuffer) { _settings->GetUserRgbPalette(paletteBuffer); }
		DllExport void __stdcall SetRgbPalette(uint32_t *paletteBuffer, uint32_t paletteSize) { _settings->SetUserRgbPalette(paletteBuffer, paletteSize); }
		DllExport void __stdcall SetPictureSettings(double brightness, double contrast, double saturation, double hue, double scanlineIntensity) { _settings->SetPictureSettings(brightness, contrast, saturation, hue, scanlineIntensity); }
		DllExport void __stdcall SetNtscFilterSettings(double artifacts, double bleed, double fringing, double gamma, double resolution, double sharpness, bool mergeFields, double yFilterLength, double iFilterLength, double qFilterLength, bool verticalBlend) { _settings->SetNtscFilterSettings(artifacts, bleed, fringing, gamma, resolution, sharpness, mergeFields, yFilterLength, iFilterLength, qFilterLength, verticalBlend, false); }
		DllExport void __stdcall SetPauseScreenMessage(char* message) { _settings->SetPauseScreenMessage(message); }

		DllExport void __stdcall SetInputDisplaySettings(uint8_t visiblePorts, InputDisplayPosition displayPosition, bool displayHorizontally) { _settings->SetInputDisplaySettings(visiblePorts, displayPosition, displayHorizontally); }
		DllExport void __stdcall SetAutoSaveOptions(uint32_t delayInMinutes, bool showMessage) { _settings->SetAutoSaveOptions(delayInMinutes, showMessage); }

		DllExport const char* __stdcall GetAudioDevices() 
		{ 
			_returnString = _soundManager ? _soundManager->GetAvailableDevices() : "";
			return _returnString.c_str();
		}

		DllExport void __stdcall SetAudioDevice(char* audioDevice) { if(_soundManager) { _soundManager->SetAudioDevice(audioDevice); } }

		DllExport void __stdcall GetScreenSize(ConsoleId consoleId, ScreenSize &size, bool ignoreScale) { GetConsoleById(consoleId)->GetVideoDecoder()->GetScreenSize(size, ignoreScale); }

		DllExport void __stdcall InputBarcode(uint64_t barCode, int32_t digitCount) { _console->InputBarcode(barCode, digitCount); }

		DllExport void __stdcall LoadTapeFile(char *filepath) { _console->LoadTapeFile(filepath); }
		DllExport void __stdcall StartRecordingTapeFile(char *filepath) { _console->StartRecordingTapeFile(filepath); }
		DllExport void __stdcall StopRecordingTapeFile() { _console->StopRecordingTapeFile(); }
		DllExport bool __stdcall IsRecordingTapeFile() { return _console->IsRecordingTapeFile(); }

		DllExport ConsoleFeatures __stdcall GetAvailableFeatures() { return _console->GetAvailableFeatures(); }

		//NSF functions
		DllExport bool __stdcall IsNsf() { return _console->IsNsf(); }
		DllExport uint32_t __stdcall NsfGetFrameCount() { return _console->GetPpu()->GetFrameCount(); }
		DllExport void __stdcall NsfSelectTrack(uint8_t trackNumber) {
			NsfMapper* nsfMapper = dynamic_cast<NsfMapper*>(_console->GetMapper());
			if(nsfMapper) {
				nsfMapper->SelectTrack(trackNumber);
			}
		}
		DllExport int32_t __stdcall NsfGetCurrentTrack() {
			NsfMapper* nsfMapper = dynamic_cast<NsfMapper*>(_console->GetMapper());
			if(nsfMapper) {
				return nsfMapper->GetCurrentTrack();
			}
			return -1;
		}
		DllExport void __stdcall NsfGetHeader(NsfHeader* header) { 
			NsfMapper* nsfMapper = dynamic_cast<NsfMapper*>(_console->GetMapper());
			if(nsfMapper) {
				*header = nsfMapper->GetNsfHeader();
			}
		}
		DllExport void __stdcall NsfSetNsfConfig(int32_t autoDetectSilenceDelay, int32_t moveToNextTrackTime, bool disableApuIrqs) {
			_settings->SetNsfConfig(autoDetectSilenceDelay, moveToNextTrackTime, disableApuIrqs);
		}

		//FDS functions
		DllExport uint32_t __stdcall FdsGetSideCount() {
			shared_ptr<FdsSystemActionManager> sam = _console->GetSystemActionManager<FdsSystemActionManager>();
			return sam ? sam->GetSideCount() : 0;
		}

		DllExport void __stdcall FdsEjectDisk() {
			shared_ptr<FdsSystemActionManager> sam = _console->GetSystemActionManager<FdsSystemActionManager>();
			if(sam) {
				sam->EjectDisk();
			}
		}

		DllExport void __stdcall FdsInsertDisk(uint32_t diskNumber) {
			shared_ptr<FdsSystemActionManager> sam = _console->GetSystemActionManager<FdsSystemActionManager>();
			if(sam) {
				sam->InsertDisk(diskNumber);
			}
		}

		DllExport void __stdcall FdsSwitchDiskSide() {
			shared_ptr<FdsSystemActionManager> sam = _console->GetSystemActionManager<FdsSystemActionManager>();
			if(sam) {
				sam->SwitchDiskSide();
			}
		}

		DllExport bool __stdcall FdsIsAutoInsertDiskEnabled() {
			shared_ptr<FdsSystemActionManager> sam = _console->GetSystemActionManager<FdsSystemActionManager>();
			return sam ? sam->IsAutoInsertDiskEnabled() : false;
		}

		//VS System functions
		DllExport bool __stdcall IsVsSystem() {
			return dynamic_cast<VsControlManager*>(_console->GetControlManager()) != nullptr;
		}

		DllExport bool __stdcall IsVsDualSystem() {
			return dynamic_cast<VsControlManager*>(_console->GetControlManager()) != nullptr && _console->IsDualSystem();
		}

		DllExport void __stdcall VsInsertCoin(uint32_t port) {
			shared_ptr<VsSystemActionManager> sam = _console->GetSystemActionManager<VsSystemActionManager>();
			if(sam) {
				sam->InsertCoin(port);
			}
		}

		DllExport uint32_t __stdcall GetDipSwitchCount() { return _console->GetDipSwitchCount(); }

		DllExport void __stdcall SetDipSwitches(uint32_t dipSwitches)
		{
			_settings->SetDipSwitches(dipSwitches);
		}

		DllExport bool __stdcall IsHdPpu() { return _console->IsHdPpu(); }

		DllExport void __stdcall HdBuilderStartRecording(char* saveFolder, ScaleFilterType filterType, uint32_t scale, uint32_t flags, uint32_t chrRamBankSize) { _console->StartRecordingHdPack(saveFolder, filterType, scale, flags, chrRamBankSize); }
		DllExport void __stdcall HdBuilderStopRecording() { _console->StopRecordingHdPack(); }

		DllExport void __stdcall HdBuilderGetChrBankList(uint32_t* bankBuffer) { HdPackBuilder::GetChrBankList(bankBuffer); }
		DllExport void __stdcall HdBuilderGetBankPreview(uint32_t bankNumber, uint32_t pageNumber, uint32_t *rgbBuffer) { HdPackBuilder::GetBankPreview(bankNumber, pageNumber, rgbBuffer); }
	}
}