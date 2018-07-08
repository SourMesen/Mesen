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

void* _windowHandle = nullptr;
void* _viewerHandle = nullptr;
string _returnString;
string _logString;
shared_ptr<Console> _console;
shared_ptr<RecordedRomTest> _recordedRomTest;
SimpleLock _externalNotificationListenerLock;
vector<shared_ptr<INotificationListener>> _externalNotificationListeners;

typedef void (__stdcall *NotificationListenerCallback)(int, void*);

shared_ptr<Console> GetConsoleById(int32_t consoleId)
{
	shared_ptr<Console> console;
	if(consoleId == 1) {
		//Get the VS Dualsystem's 2nd CPU, only if it's available (and requested)
		console = _console->GetDualConsole();
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
		char Sha1[40];
	};

	extern "C" {
		DllExport bool __stdcall TestDll()
		{
			return true;
		}

		DllExport uint32_t __stdcall GetMesenVersion() { return EmulationSettings::GetMesenVersion(); }

		DllExport void __stdcall InitializeEmu(const char* homeFolder, void *windowHandle, void *viewerHandle, bool noAudio, bool noVideo, bool noInput)
		{
			FolderUtilities::SetHomeFolder(homeFolder);
			_console.reset(new Console());
			_console->Init();
			_shortcutKeyHandler.reset(new ShortcutKeyHandler(_console));

			if(windowHandle != nullptr && viewerHandle != nullptr) {
				_windowHandle = windowHandle;
				_viewerHandle = viewerHandle;

				if(!noVideo) {
					#ifdef _WIN32
						_renderer.reset(new Renderer(_console, (HWND)_viewerHandle));
					#else 
						_renderer.reset(new SdlRenderer(_console, _viewerHandle));
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
					_dualRenderer.reset(new Renderer(slaveConsole, (HWND)viewerHandle));
					_dualSoundManager.reset(new SoundManager(slaveConsole, (HWND)windowHandle));
				#else 
					_dualRenderer.reset(new SdlRenderer(slaveConsole, viewerHandle));
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

		DllExport void __stdcall SetControllerType(uint32_t port, ControllerType type) { EmulationSettings::SetControllerType(port, type); }
		DllExport void __stdcall SetControllerKeys(uint32_t port, KeyMappingSet mappings) { EmulationSettings::SetControllerKeys(port, mappings); }
		DllExport void __stdcall SetZapperDetectionRadius(uint32_t detectionRadius) { EmulationSettings::SetZapperDetectionRadius(detectionRadius); }
		DllExport void __stdcall SetExpansionDevice(ExpansionPortDevice device) { EmulationSettings::SetExpansionDevice(device); }
		DllExport void __stdcall SetConsoleType(ConsoleType type) { EmulationSettings::SetConsoleType(type); }
		DllExport void __stdcall SetMouseSensitivity(MouseDevice device, double sensitivity) { EmulationSettings::SetMouseSensitivity(device, sensitivity); }
		
		DllExport void __stdcall ClearShortcutKeys() { EmulationSettings::ClearShortcutKeys(); }
		DllExport void __stdcall SetShortcutKey(EmulatorShortcut shortcut, KeyCombination keyCombination, int keySetIndex) { EmulationSettings::SetShortcutKey(shortcut, keyCombination, keySetIndex); }

		DllExport ControllerType __stdcall GetControllerType(uint32_t port) { return EmulationSettings::GetControllerType(port); }
		DllExport ExpansionPortDevice GetExpansionDevice() { return EmulationSettings::GetExpansionDevice(); }
		DllExport ConsoleType __stdcall GetConsoleType() { return EmulationSettings::GetConsoleType(); }
		
		DllExport bool __stdcall HasZapper() { return EmulationSettings::HasZapper(); }
		DllExport bool __stdcall HasFourScore() { return EmulationSettings::CheckFlag(EmulationFlags::HasFourScore); }
		DllExport bool __stdcall HasArkanoidPaddle() { return EmulationSettings::HasArkanoidPaddle(); }

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

		DllExport void __stdcall Resume() { EmulationSettings::ClearFlags(EmulationFlags::Paused); }
		DllExport bool __stdcall IsPaused() { return EmulationSettings::CheckFlag(EmulationFlags::Paused); }
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
		DllExport ControllerType __stdcall NetPlayGetControllerType(int32_t port) { return EmulationSettings::GetControllerType(port); }

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

		DllExport void __stdcall Pause()
		{
			if(!IsConnected()) {
				EmulationSettings::SetFlags(EmulationFlags::Paused);
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

			MessageManager::RegisterMessageManager(nullptr);
			
			_shortcutKeyHandler.reset();
		}

		DllExport void __stdcall TakeScreenshot() { _console->GetVideoDecoder()->TakeScreenshot(); }

		DllExport INotificationListener* __stdcall RegisterNotificationCallback(int32_t consoleId, NotificationListenerCallback callback)
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

		DllExport bool __stdcall CheckFlag(EmulationFlags flags) { return EmulationSettings::CheckFlag(flags); }
		DllExport void __stdcall SetFlags(EmulationFlags flags) { EmulationSettings::SetFlags(flags); }
		DllExport void __stdcall ClearFlags(EmulationFlags flags) { EmulationSettings::ClearFlags(flags); }
		DllExport void __stdcall SetRamPowerOnState(RamPowerOnState state) { EmulationSettings::SetRamPowerOnState(state); }
		DllExport void __stdcall SetDisplayLanguage(Language lang) { EmulationSettings::SetDisplayLanguage(lang); }
		DllExport void __stdcall SetChannelVolume(uint32_t channel, double volume) { EmulationSettings::SetChannelVolume((AudioChannel)channel, volume); }
		DllExport void __stdcall SetChannelPanning(uint32_t channel, double panning) { EmulationSettings::SetChannelPanning((AudioChannel)channel, panning); }
		DllExport void __stdcall SetEqualizerFilterType(EqualizerFilterType filter) { EmulationSettings::SetEqualizerFilterType(filter); }
		DllExport void __stdcall SetBandGain(uint32_t band, double gain) { EmulationSettings::SetBandGain(band, gain); }
		DllExport void __stdcall SetEqualizerBands(double *bands, uint32_t bandCount) { EmulationSettings::SetEqualizerBands(bands, bandCount); }
		DllExport void __stdcall SetMasterVolume(double volume, double volumeReduction) { EmulationSettings::SetMasterVolume(volume, volumeReduction); }
		DllExport void __stdcall SetSampleRate(uint32_t sampleRate) { EmulationSettings::SetSampleRate(sampleRate); }
		DllExport void __stdcall SetAudioLatency(uint32_t msLatency) { EmulationSettings::SetAudioLatency(msLatency); }
		DllExport void __stdcall SetStereoFilter(StereoFilter stereoFilter) { EmulationSettings::SetStereoFilter(stereoFilter); }
		DllExport void __stdcall SetStereoDelay(int32_t delay) { EmulationSettings::SetStereoDelay(delay); }
		DllExport void __stdcall SetStereoPanningAngle(double angle) { EmulationSettings::SetStereoPanningAngle(angle); }
		DllExport void __stdcall SetReverbParameters(double strength, double delay) { EmulationSettings::SetReverbParameters(strength, delay); }
		DllExport void __stdcall SetCrossFeedRatio(uint32_t ratio) { EmulationSettings::SetCrossFeedRatio(ratio); }

		DllExport NesModel __stdcall GetNesModel() { return _console->GetModel(); }
		DllExport void __stdcall SetNesModel(uint32_t model) { EmulationSettings::SetNesModel((NesModel)model); }
		DllExport void __stdcall SetOverscanDimensions(uint32_t left, uint32_t right, uint32_t top, uint32_t bottom) { EmulationSettings::SetOverscanDimensions(left, right, top, bottom); }
		DllExport void __stdcall SetEmulationSpeed(uint32_t emulationSpeed) { EmulationSettings::SetEmulationSpeed(emulationSpeed, true); }
		DllExport void __stdcall IncreaseEmulationSpeed() { EmulationSettings::IncreaseEmulationSpeed(); }
		DllExport void __stdcall DecreaseEmulationSpeed() { EmulationSettings::DecreaseEmulationSpeed(); }
		DllExport uint32_t __stdcall GetEmulationSpeed() { return EmulationSettings::GetEmulationSpeed(true); }
		DllExport void __stdcall SetTurboRewindSpeed(uint32_t turboSpeed, uint32_t rewindSpeed) { EmulationSettings::SetTurboRewindSpeed(turboSpeed, rewindSpeed); }
		DllExport void __stdcall SetRewindBufferSize(uint32_t seconds) { EmulationSettings::SetRewindBufferSize(seconds); }
		DllExport void __stdcall SetOverclockRate(uint32_t overclockRate, bool adjustApu) { EmulationSettings::SetOverclockRate(overclockRate, adjustApu); }
		DllExport void __stdcall SetPpuNmiConfig(uint32_t extraScanlinesBeforeNmi, uint32_t extraScanlinesAfterNmi) { EmulationSettings::SetPpuNmiConfig(extraScanlinesBeforeNmi, extraScanlinesAfterNmi); }
		DllExport void __stdcall SetVideoScale(double scale) { EmulationSettings::SetVideoScale(scale); }
		DllExport void __stdcall SetScreenRotation(uint32_t angle) { EmulationSettings::SetScreenRotation(angle); }
		DllExport void __stdcall SetExclusiveRefreshRate(uint32_t angle) { EmulationSettings::SetExclusiveRefreshRate(angle); }
		DllExport void __stdcall SetVideoAspectRatio(VideoAspectRatio aspectRatio, double customRatio) { EmulationSettings::SetVideoAspectRatio(aspectRatio, customRatio); }
		DllExport void __stdcall SetVideoFilter(VideoFilterType filter) { EmulationSettings::SetVideoFilterType(filter); }
		DllExport void __stdcall SetVideoResizeFilter(VideoResizeFilter filter) { EmulationSettings::SetVideoResizeFilter(filter); }
		DllExport void __stdcall GetRgbPalette(uint32_t *paletteBuffer) { EmulationSettings::GetRgbPalette(paletteBuffer); }
		DllExport void __stdcall SetRgbPalette(uint32_t *paletteBuffer) { EmulationSettings::SetRgbPalette(paletteBuffer); }
		DllExport void __stdcall SetPictureSettings(double brightness, double contrast, double saturation, double hue, double scanlineIntensity) { EmulationSettings::SetPictureSettings(brightness, contrast, saturation, hue, scanlineIntensity); }
		DllExport void __stdcall SetNtscFilterSettings(double artifacts, double bleed, double fringing, double gamma, double resolution, double sharpness, bool mergeFields, double yFilterLength, double iFilterLength, double qFilterLength, bool verticalBlend) { EmulationSettings::SetNtscFilterSettings(artifacts, bleed, fringing, gamma, resolution, sharpness, mergeFields, yFilterLength, iFilterLength, qFilterLength, verticalBlend, false); }
		DllExport void __stdcall SetPauseScreenMessage(char* message) { EmulationSettings::SetPauseScreenMessage(message); }

		DllExport void __stdcall SetInputDisplaySettings(uint8_t visiblePorts, InputDisplayPosition displayPosition, bool displayHorizontally) { EmulationSettings::SetInputDisplaySettings(visiblePorts, displayPosition, displayHorizontally); }
		DllExport void __stdcall SetAutoSaveOptions(uint32_t delayInMinutes, bool showMessage) { EmulationSettings::SetAutoSaveOptions(delayInMinutes, showMessage); }

		DllExport const char* __stdcall GetAudioDevices() 
		{ 
			_returnString = _soundManager ? _soundManager->GetAvailableDevices() : "";
			return _returnString.c_str();
		}

		DllExport void __stdcall SetAudioDevice(char* audioDevice) { if(_soundManager) { _soundManager->SetAudioDevice(audioDevice); } }

		DllExport void __stdcall GetScreenSize(ScreenSize &size, bool ignoreScale) { _console->GetVideoDecoder()->GetScreenSize(size, ignoreScale); }

		DllExport void __stdcall InputBarcode(uint64_t barCode, int32_t digitCount) { _console->InputBarcode(barCode, digitCount); }

		DllExport void __stdcall LoadTapeFile(char *filepath) { _console->LoadTapeFile(filepath); }
		DllExport void __stdcall StartRecordingTapeFile(char *filepath) { _console->StartRecordingTapeFile(filepath); }
		DllExport void __stdcall StopRecordingTapeFile() { _console->StopRecordingTapeFile(); }
		DllExport bool __stdcall IsRecordingTapeFile() { return _console->IsRecordingTapeFile(); }

		DllExport ConsoleFeatures __stdcall GetAvailableFeatures() { return _console->GetAvailableFeatures(); }

		//NSF functions
		DllExport bool __stdcall IsNsf() { return NsfMapper::GetInstance() != nullptr; }
		DllExport void __stdcall NsfSelectTrack(uint8_t trackNumber) {
			if(NsfMapper::GetInstance()) {
				NsfMapper::GetInstance()->SelectTrack(trackNumber);
			}
		}
		DllExport int32_t __stdcall NsfGetCurrentTrack() {
			if(NsfMapper::GetInstance()) {
				return NsfMapper::GetInstance()->GetCurrentTrack();
			}
			return -1;
		}
		DllExport void __stdcall NsfGetHeader(NsfHeader* header) { 
			if(NsfMapper::GetInstance()) {
				*header = NsfMapper::GetInstance()->GetNsfHeader();
			}
		}
		DllExport void __stdcall NsfSetNsfConfig(int32_t autoDetectSilenceDelay, int32_t moveToNextTrackTime, bool disableApuIrqs) {
			EmulationSettings::SetNsfConfig(autoDetectSilenceDelay, moveToNextTrackTime, disableApuIrqs);
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

		DllExport void __stdcall SetDipSwitches(uint32_t dipSwitches)
		{
			EmulationSettings::SetDipSwitches(dipSwitches);
		}

		DllExport bool __stdcall IsHdPpu() { return _console->IsHdPpu(); }

		DllExport void __stdcall HdBuilderStartRecording(char* saveFolder, ScaleFilterType filterType, uint32_t scale, uint32_t flags, uint32_t chrRamBankSize) { _console->StartRecordingHdPack(saveFolder, filterType, scale, flags, chrRamBankSize); }
		DllExport void __stdcall HdBuilderStopRecording() { _console->StopRecordingHdPack(); }

		DllExport void __stdcall HdBuilderGetChrBankList(uint32_t* bankBuffer) { HdPackBuilder::GetChrBankList(bankBuffer); }
		DllExport void __stdcall HdBuilderGetBankPreview(uint32_t bankNumber, uint32_t pageNumber, uint32_t *rgbBuffer) { HdPackBuilder::GetBankPreview(bankNumber, pageNumber, rgbBuffer); }
	}
}