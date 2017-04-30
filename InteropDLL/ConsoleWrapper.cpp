#include "stdafx.h"

#include "../Core/MessageManager.h"
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
#include "../Utilities/AviWriter.h"

#ifdef WIN32
	#include "../Windows/Renderer.h"
	#include "../Windows/SoundManager.h"
	#include "../Windows/WindowsKeyManager.h"
#else
	#include "../Linux/SdlRenderer.h"
	#include "../Linux/SdlSoundManager.h"
	#include "../Linux/LinuxKeyManager.h"
#endif

IRenderingDevice *_renderer = nullptr;
IAudioDevice *_soundManager = nullptr;
IKeyManager *_keyManager = nullptr;
void*  _windowHandle = nullptr;
void* _viewerHandle = nullptr;
string _returnString;
string _logString;
RecordedRomTest *_recordedRomTest = nullptr;

typedef void (__stdcall *NotificationListenerCallback)(int);

namespace InteropEmu {
	class InteropNotificationListener : public INotificationListener
	{
		NotificationListenerCallback _callback;
	public:
		InteropNotificationListener(NotificationListenerCallback callback)
		{
			_callback = callback;
		}

		void ProcessNotification(ConsoleNotificationType type, void* parameter)
		{
			_callback((int)type);
		}
	};

	struct RomInfo
	{
		const char* RomName;
		uint32_t Crc32;
		uint32_t PrgCrc32;
		RomFormat Format;
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

			if(windowHandle != nullptr && viewerHandle != nullptr) {
				_windowHandle = windowHandle;
				_viewerHandle = viewerHandle;

				if(!noVideo) {
					#ifdef _WIN32
						_renderer = new NES::Renderer((HWND)_viewerHandle);
					#else 
						_renderer = new SdlRenderer(_viewerHandle);
					#endif
				} 

				if(!noAudio) {
					#ifdef _WIN32
						_soundManager = new SoundManager((HWND)_windowHandle);
					#else
						_soundManager = new SdlSoundManager(); 
					#endif
				}

				if(!noInput) {
					#ifdef _WIN32
						_keyManager = new WindowsKeyManager((HWND)_windowHandle);
					#else 
						_keyManager = new LinuxKeyManager();
					#endif				
					
					ControlManager::RegisterKeyManager(_keyManager);
				}
			}
		}

		DllExport bool __stdcall IsRunning() { return Console::IsRunning(); }

		DllExport void __stdcall LoadROM(char* filename, int32_t archiveFileIndex, char* patchFile) { Console::LoadROM(filename, nullptr, archiveFileIndex, patchFile); }
		DllExport void __stdcall AddKnownGameFolder(char* folder) { FolderUtilities::AddKnownGameFolder(folder); }

		DllExport const char* __stdcall GetArchiveRomList(char* filename) { 
			std::ostringstream out;
			for(string romName : RomLoader::GetArchiveRomList(filename)) {
				out << romName << "[!|!]";
			}
			_returnString = out.str();
			return _returnString.c_str();
		}

		DllExport void __stdcall SetControllerType(uint32_t port, ControllerType type) { EmulationSettings::SetControllerType(port, type); }
		DllExport void __stdcall SetControllerKeys(uint32_t port, KeyMappingSet mappings) { EmulationSettings::SetControllerKeys(port, mappings); }
		DllExport void __stdcall SetExpansionDevice(ExpansionPortDevice device) { EmulationSettings::SetExpansionDevice(device); }
		DllExport void __stdcall SetConsoleType(ConsoleType type) { EmulationSettings::SetConsoleType(type); }
		DllExport void __stdcall SetEmulatorKeys(EmulatorKeyMappingSet mappings) { EmulationSettings::SetEmulatorKeys(mappings); }

		DllExport ControllerType __stdcall GetControllerType(uint32_t port) { return EmulationSettings::GetControllerType(port); }
		DllExport ExpansionPortDevice GetExpansionDevice() { return EmulationSettings::GetExpansionDevice(); }
		DllExport ConsoleType __stdcall GetConsoleType() { return EmulationSettings::GetConsoleType(); }
		
		DllExport bool __stdcall HasZapper() { return EmulationSettings::HasZapper(); }
		DllExport bool __stdcall HasFourScore() { return EmulationSettings::CheckFlag(EmulationFlags::HasFourScore); }
		DllExport bool __stdcall HasArkanoidPaddle() { return EmulationSettings::HasArkanoidPaddle(); }

		DllExport void __stdcall SetMousePosition(double x, double y) { ControlManager::SetMousePosition(x, y); }

		DllExport void __stdcall UpdateInputDevices() { if(_keyManager) { _keyManager->UpdateDevices(); } }
		DllExport uint32_t __stdcall GetPressedKey() { return ControlManager::GetPressedKey(); }
		DllExport void __stdcall SetKeyState(int32_t scanCode, bool state) { if(_keyManager) { _keyManager->SetKeyState(scanCode, state); } }
		DllExport void __stdcall ResetKeyState() { if(_keyManager) { _keyManager->ResetKeyState(); } }
		DllExport const char* __stdcall GetKeyName(uint32_t keyCode) 
		{
			_returnString = ControlManager::GetKeyName(keyCode);
			return _returnString.c_str();
		}
		DllExport uint32_t __stdcall GetKeyCode(char* keyName) { 
			if(keyName) {
				return ControlManager::GetKeyCode(keyName);
			} else {
				return 0;
			}
		}

		DllExport void __stdcall Run()
		{
			if(Console::GetInstance()) {
				Console::GetInstance()->Run();
			}
		}

		DllExport void __stdcall Resume() { EmulationSettings::ClearFlags(EmulationFlags::Paused); }
		DllExport bool __stdcall IsPaused() { return EmulationSettings::CheckFlag(EmulationFlags::Paused); }
		DllExport void __stdcall Stop()
		{
			if(Console::GetInstance()) {
				Console::GetInstance()->Stop();
			}
		}

		DllExport const void __stdcall GetRomInfo(RomInfo &romInfo, char* filename, int32_t archiveFileIndex) 
		{
			string romPath = filename;
			if(romPath.empty()) {
				_returnString = Console::GetRomName();
				romInfo.RomName = _returnString.c_str();
				romInfo.Crc32 = Console::GetCrc32();
				romInfo.PrgCrc32 = Console::GetPrgCrc32();
				romInfo.Format = Console::GetRomFormat();
			} else {
				RomLoader romLoader;
				if(romLoader.LoadFile(filename, nullptr, "", archiveFileIndex)) {
					RomData romData = romLoader.GetRomData();

					_returnString = romData.RomName;
					romInfo.RomName = _returnString.c_str();
					romInfo.Crc32 = romData.Crc32;
					romInfo.PrgCrc32 = romData.PrgCrc32;
					romInfo.Format = RomFormat::Unknown;
				} else {
					_returnString = "";
					romInfo.RomName = _returnString.c_str();
					romInfo.Crc32 = 0;
					romInfo.PrgCrc32 = 0;
					romInfo.Format = RomFormat::Unknown;
				}
			}
		}

		DllExport void __stdcall Reset() { Console::Reset(); }
		DllExport void __stdcall ResetLagCounter() { Console::ResetLagCounter(); }

		DllExport void __stdcall StartServer(uint16_t port, char* hostPlayerName) { GameServer::StartServer(port, hostPlayerName); }
		DllExport void __stdcall StopServer() { GameServer::StopServer(); }
		DllExport bool __stdcall IsServerRunning() { return GameServer::Started(); }

		DllExport void __stdcall Connect(char* host, uint16_t port, char* playerName, bool spectator)
		{
			shared_ptr<ClientConnectionData> connectionData(new ClientConnectionData(
				host,
				port,
				playerName,
				spectator
			));

			GameClient::Connect(connectionData);
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
			Console::Release();
			GameServer::StopServer();
			GameClient::Disconnect();
			MessageManager::RegisterMessageManager(nullptr);
			
			if(_renderer) {
				delete _renderer;
				_renderer = nullptr;
			}
			if(_soundManager) {
				delete _soundManager;
				_soundManager = nullptr;
			}

			VideoDecoder::GetInstance()->StopThread();
			VideoDecoder::Release();
		}

		DllExport void __stdcall TakeScreenshot() { VideoDecoder::GetInstance()->TakeScreenshot(); }

		DllExport INotificationListener* __stdcall RegisterNotificationCallback(NotificationListenerCallback callback)
		{
			INotificationListener* listener = new InteropNotificationListener(callback);
			MessageManager::RegisterNotificationListener(listener);
			return listener;
		}
		DllExport void __stdcall UnregisterNotificationCallback(INotificationListener *listener) { MessageManager::UnregisterNotificationListener(listener); }

		DllExport void __stdcall DisplayMessage(char* title, char* message, char* param1) { MessageManager::DisplayMessage(title, message, param1 ? param1 : ""); }
		DllExport const char* __stdcall GetLog()
		{
			_logString = MessageManager::GetLog();
			return _logString.c_str();
		}

		DllExport void __stdcall SaveState(uint32_t stateIndex) { SaveStateManager::SaveState(stateIndex); }
		DllExport uint32_t __stdcall LoadState(uint32_t stateIndex) { return SaveStateManager::LoadState(stateIndex); }
		DllExport int64_t  __stdcall GetStateInfo(uint32_t stateIndex) { return SaveStateManager::GetStateInfo(stateIndex); }

		DllExport void __stdcall MoviePlay(char* filename) { MovieManager::Play(filename); }
		DllExport void __stdcall MovieRecord(char* filename, bool reset) { MovieManager::Record(filename, reset); }
		DllExport void __stdcall MovieStop() { MovieManager::Stop(); }
		DllExport bool __stdcall MoviePlaying() { return MovieManager::Playing(); }
		DllExport bool __stdcall MovieRecording() { return MovieManager::Recording(); }

		DllExport void __stdcall AviRecord(char* filename, VideoCodec codec, uint32_t compressionLevel) { VideoRenderer::GetInstance()->StartRecording(filename, codec, compressionLevel); }
		DllExport void __stdcall AviStop() { VideoRenderer::GetInstance()->StopRecording(); }
		DllExport bool __stdcall AviIsRecording() { return VideoRenderer::GetInstance()->IsRecording(); }

		DllExport void __stdcall WaveRecord(char* filename) { SoundMixer::StartRecording(filename); }
		DllExport void __stdcall WaveStop() { SoundMixer::StopRecording(); }
		DllExport bool __stdcall WaveIsRecording() { return SoundMixer::IsRecording(); }

		DllExport int32_t __stdcall RunRecordedTest(char* filename)
		{
			RecordedRomTest romTest; 
			return romTest.Run(filename);
		}

		DllExport int32_t __stdcall RunAutomaticTest(char* filename)
		{
			AutomaticRomTest romTest;
			return romTest.Run(filename);
		}

		DllExport void __stdcall RomTestRecord(char* filename, bool reset) 
		{
			if(_recordedRomTest) {
				delete _recordedRomTest;
			}
			_recordedRomTest = new RecordedRomTest();
			_recordedRomTest->Record(filename, reset);
		}

		DllExport void __stdcall RomTestRecordFromMovie(char* testFilename, char* movieFilename) 
		{
			_recordedRomTest = new RecordedRomTest();
			_recordedRomTest->RecordFromMovie(testFilename, movieFilename);
		}

		DllExport void __stdcall RomTestRecordFromTest(char* newTestFilename, char* existingTestFilename) 
		{
			_recordedRomTest = new RecordedRomTest();
			_recordedRomTest->RecordFromTest(newTestFilename, existingTestFilename);
		}

		DllExport void __stdcall RomTestStop() 
		{
			if(_recordedRomTest) {
				_recordedRomTest->Stop();
				delete _recordedRomTest;
				_recordedRomTest = nullptr;
			}
		}

		DllExport bool __stdcall RomTestRecording() { return _recordedRomTest != nullptr; }

		DllExport void __stdcall SetCheats(CheatInfo cheats[], uint32_t length) { CheatManager::SetCheats(cheats, length); }

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
		DllExport void __stdcall SetMasterVolume(double volume) { EmulationSettings::SetMasterVolume(volume); }
		DllExport void __stdcall SetSampleRate(uint32_t sampleRate) { EmulationSettings::SetSampleRate(sampleRate); }
		DllExport void __stdcall SetAudioLatency(uint32_t msLatency) { EmulationSettings::SetAudioLatency(msLatency); }
		DllExport void __stdcall SetStereoFilter(StereoFilter stereoFilter) { EmulationSettings::SetStereoFilter(stereoFilter); }
		DllExport void __stdcall SetStereoDelay(int32_t delay) { EmulationSettings::SetStereoDelay(delay); }
		DllExport void __stdcall SetStereoPanningAngle(double angle) { EmulationSettings::SetStereoPanningAngle(angle); }
		DllExport void __stdcall SetReverbParameters(double strength, double delay) { EmulationSettings::SetReverbParameters(strength, delay); }
		DllExport void __stdcall SetCrossFeedRatio(uint32_t ratio) { EmulationSettings::SetCrossFeedRatio(ratio); }

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
		DllExport void __stdcall SetVideoAspectRatio(VideoAspectRatio aspectRatio, double customRatio) { EmulationSettings::SetVideoAspectRatio(aspectRatio, customRatio); }
		DllExport void __stdcall SetVideoFilter(VideoFilterType filter) { EmulationSettings::SetVideoFilterType(filter); }
		DllExport void __stdcall SetVideoResizeFilter(VideoResizeFilter filter) { EmulationSettings::SetVideoResizeFilter(filter); }
		DllExport void __stdcall GetRgbPalette(uint32_t *paletteBuffer) { EmulationSettings::GetRgbPalette(paletteBuffer); }
		DllExport void __stdcall SetRgbPalette(uint32_t *paletteBuffer) { EmulationSettings::SetRgbPalette(paletteBuffer); }
		DllExport void __stdcall SetPictureSettings(double brightness, double contrast, double saturation, double hue, double scanlineIntensity) { EmulationSettings::SetPictureSettings(brightness, contrast, saturation, hue, scanlineIntensity); }
		DllExport void __stdcall SetNtscFilterSettings(double artifacts, double bleed, double fringing, double gamma, double resolution, double sharpness, bool mergeFields, double yFilterLength, double iFilterLength, double qFilterLength) { EmulationSettings::SetNtscFilterSettings(artifacts, bleed, fringing, gamma, resolution, sharpness, mergeFields, yFilterLength, iFilterLength, qFilterLength); }

		DllExport void __stdcall SetInputDisplaySettings(uint8_t visiblePorts, InputDisplayPosition displayPosition, bool displayHorizontally) { EmulationSettings::SetInputDisplaySettings(visiblePorts, displayPosition, displayHorizontally); }
		DllExport void __stdcall SetAutoSaveOptions(uint32_t delayInMinutes, bool showMessage) { EmulationSettings::SetAutoSaveOptions(delayInMinutes, showMessage); }

		DllExport const char* __stdcall GetAudioDevices() 
		{ 
			_returnString = _soundManager ? _soundManager->GetAvailableDevices() : "";
			return _returnString.c_str();
		}

		DllExport void __stdcall SetAudioDevice(char* audioDevice) { if(_soundManager) { _soundManager->SetAudioDevice(audioDevice); } }

		DllExport void __stdcall GetScreenSize(ScreenSize &size, bool ignoreScale) { VideoDecoder::GetInstance()->GetScreenSize(size, ignoreScale); }
		
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
		DllExport uint32_t __stdcall FdsGetSideCount() { return FDS::GetSideCount(); }
		DllExport void __stdcall FdsEjectDisk() { FDS::EjectDisk(); }
		DllExport void __stdcall FdsInsertDisk(uint32_t diskNumber) { FDS::InsertDisk(diskNumber); }
		DllExport void __stdcall FdsSwitchDiskSide() { FDS::SwitchDiskSide(); }

		//VS System functions
		DllExport bool __stdcall IsVsSystem() { return VsControlManager::GetInstance() != nullptr; }
		DllExport void __stdcall VsInsertCoin(uint32_t port) 
		{ 
			VsControlManager* vs = VsControlManager::GetInstance();
			if(vs) {
				vs->InsertCoin(port);
			}
		}

		DllExport void __stdcall VsSetGameConfig(PpuModel model, VsInputType inputType, uint8_t dipSwitches)
		{
			VsControlManager* vs = VsControlManager::GetInstance();
			if(vs) {
				EmulationSettings::SetPpuModel(model);
				vs->SetDipSwitches(dipSwitches);
				vs->SetInputType(inputType);
			}
		}
	}
}