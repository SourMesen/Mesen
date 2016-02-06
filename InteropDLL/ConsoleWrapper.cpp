#include "stdafx.h"

#include "../Core/MessageManager.h"
#include "../Core/Console.h"
#include "../Windows/Renderer.h"
#include "../Windows/SoundManager.h"
#include "../Windows/WindowsKeyManager.h"
#include "../Core/GameServer.h"
#include "../Core/GameClient.h"
#include "../Core/ClientConnectionData.h"
#include "../Core/SaveStateManager.h"
#include "../Core/CheatManager.h"
#include "../Core/EmulationSettings.h"
#include "../Core/VideoDecoder.h"
#include "../Core/AutoRomTest.h"
#include "../Core/FDS.h"

NES::Renderer *_renderer = nullptr;
SoundManager *_soundManager = nullptr;
HWND _windowHandle = nullptr;
HWND _viewerHandle = nullptr;
string _returnString;
AutoRomTest *_autoRomTest = nullptr;

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

	extern "C" {
		DllExport bool __stdcall TestDll()
		{
			return true;
		}

		DllExport void __stdcall InitializeEmu(const char* homeFolder, HWND windowHandle, HWND dxViewerHandle)
		{
			FolderUtilities::SetHomeFolder(homeFolder);

			if(windowHandle != nullptr && dxViewerHandle != nullptr) {
				_windowHandle = windowHandle;
				_viewerHandle = dxViewerHandle;

				_renderer = new NES::Renderer(_viewerHandle);
				_soundManager = new SoundManager(_windowHandle);

				ControlManager::RegisterKeyManager(new WindowsKeyManager(_windowHandle));
			}
		}

		DllExport void __stdcall LoadROM(char* filename) { Console::LoadROM(filename); }
		DllExport void __stdcall ApplyIpsPatch(char* filename) { Console::ApplyIpsPatch(filename); }
		DllExport void __stdcall AddKnowGameFolder(char* folder) { FolderUtilities::AddKnowGameFolder(folder); }

		DllExport void __stdcall SetControllerType(uint32_t port, ControllerType type) { EmulationSettings::SetControllerType(port, type); }
		DllExport void __stdcall SetControllerKeys(uint32_t port, KeyMappingSet mappings) { EmulationSettings::SetControllerKeys(port, mappings); }
		DllExport void __stdcall SetExpansionDevice(ExpansionPortDevice device) { EmulationSettings::SetExpansionDevice(device); }
		DllExport void __stdcall SetConsoleType(ConsoleType type) { EmulationSettings::SetConsoleType(type); }
		
		DllExport bool __stdcall HasZapper() { return ControlManager::HasZapper(); }
		DllExport void __stdcall ZapperSetTriggerState(int32_t port, bool pulled) { ControlManager::ZapperSetTriggerState(port, pulled); }
		DllExport void __stdcall ZapperSetPosition(int32_t port, int32_t x, int32_t y) { ControlManager::ZapperSetPosition(port, x, y); }

		DllExport uint32_t __stdcall GetPressedKey() { return ControlManager::GetPressedKey(); }
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
		DllExport const char* __stdcall GetROMPath() 
		{
			_returnString = Console::GetROMPath();
			return _returnString.c_str();
		}

		DllExport void __stdcall Reset() { Console::Reset(); }

		DllExport void __stdcall StartServer(uint16_t port) { GameServer::StartServer(port); }
		DllExport void __stdcall StopServer() { GameServer::StopServer(); }
		DllExport bool __stdcall IsServerRunning() { return GameServer::Started(); }

		DllExport void __stdcall Connect(char* host, uint16_t port, char* playerName, uint8_t* avatarData, uint32_t avatarSize)
		{
			shared_ptr<ClientConnectionData> connectionData(new ClientConnectionData(
				host,
				port,
				playerName,
				avatarData,
				avatarSize
				));

			GameClient::Connect(connectionData);
		}

		DllExport void __stdcall Disconnect() { GameClient::Disconnect(); }
		DllExport bool __stdcall IsConnected() { return GameClient::Connected(); }

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
			delete _renderer;
			delete _soundManager;
		}

		DllExport void __stdcall TakeScreenshot() { VideoDecoder::GetInstance()->TakeScreenshot(FolderUtilities::GetFilename(Console::GetROMPath(), false)); }

		DllExport INotificationListener* __stdcall RegisterNotificationCallback(NotificationListenerCallback callback)
		{
			INotificationListener* listener = new InteropNotificationListener(callback);
			MessageManager::RegisterNotificationListener(listener);
			return listener;
		}
		DllExport void __stdcall UnregisterNotificationCallback(INotificationListener *listener) { MessageManager::UnregisterNotificationListener(listener); }

		DllExport void __stdcall DisplayMessage(char* title, char* message) { MessageManager::DisplayMessage(title, message); }

		DllExport void __stdcall SaveState(uint32_t stateIndex) { SaveStateManager::SaveState(stateIndex); }
		DllExport uint32_t __stdcall LoadState(uint32_t stateIndex) { return SaveStateManager::LoadState(stateIndex); }
		DllExport int64_t  __stdcall GetStateInfo(uint32_t stateIndex) { return SaveStateManager::GetStateInfo(stateIndex); }

		DllExport void __stdcall MoviePlay(char* filename) { Movie::Play(filename); }
		DllExport void __stdcall MovieRecord(char* filename, bool reset) { Movie::Record(filename, reset); }
		DllExport void __stdcall MovieStop() { Movie::Stop(); }
		DllExport bool __stdcall MoviePlaying() { return Movie::Playing(); }
		DllExport bool __stdcall MovieRecording() { return Movie::Recording(); }

		DllExport int32_t __stdcall RomTestRun(char* filename)
		{
			AutoRomTest romTest; 
			return romTest.Run(filename);
		}

		DllExport void __stdcall RomTestRecord(char* filename, bool reset) 
		{
			if(_autoRomTest) {
				delete _autoRomTest;
			}
			_autoRomTest = new AutoRomTest();
			_autoRomTest->Record(filename, reset); 
		}

		DllExport void __stdcall RomTestRecordFromMovie(char* testFilename, char* movieFilename) 
		{
			_autoRomTest = new AutoRomTest();
			_autoRomTest->RecordFromMovie(testFilename, movieFilename);
		}

		DllExport void __stdcall RomTestRecordFromTest(char* newTestFilename, char* existingTestFilename) 
		{
			_autoRomTest = new AutoRomTest();
			_autoRomTest->RecordFromTest(newTestFilename, existingTestFilename);
		}

		DllExport void __stdcall RomTestStop() 
		{
			if(_autoRomTest) {
				_autoRomTest->Stop();
				delete _autoRomTest;
				_autoRomTest = nullptr;
			}
		}

		DllExport bool __stdcall RomTestRecording() { return _autoRomTest != nullptr; }

		DllExport void __stdcall CheatAddCustom(uint32_t address, uint8_t value, int32_t compareValue, bool isRelativeAddress) { CheatManager::AddCustomCode(address, value, compareValue, isRelativeAddress); }
		DllExport void __stdcall CheatAddGameGenie(char* code) { CheatManager::AddGameGenieCode(code); }
		DllExport void __stdcall CheatAddProActionRocky(uint32_t code) { CheatManager::AddProActionRockyCode(code); }
		DllExport void __stdcall CheatClear() { CheatManager::ClearCodes(); }

		DllExport void __stdcall SetFlags(EmulationFlags flags) { EmulationSettings::SetFlags(flags); }
		DllExport void __stdcall ClearFlags(EmulationFlags flags) { EmulationSettings::ClearFlags(flags); }
		DllExport void __stdcall SetChannelVolume(uint32_t channel, double volume) { EmulationSettings::SetChannelVolume((AudioChannel)channel, volume); }
		DllExport void __stdcall SetMasterVolume(double volume) { EmulationSettings::SetMasterVolume(volume); }
		DllExport void __stdcall SetSampleRate(uint32_t sampleRate) { EmulationSettings::SetSampleRate(sampleRate); }
		DllExport void __stdcall SetAudioLatency(uint32_t msLatency) { EmulationSettings::SetAudioLatency(msLatency); }
		DllExport void __stdcall SetNesModel(uint32_t model) { EmulationSettings::SetNesModel((NesModel)model); }
		DllExport void __stdcall SetOverscanDimensions(uint32_t left, uint32_t right, uint32_t top, uint32_t bottom) { EmulationSettings::SetOverscanDimensions(left, right, top, bottom); }
		DllExport void __stdcall SetEmulationSpeed(uint32_t emulationSpeed) { EmulationSettings::SetEmulationSpeed(emulationSpeed); }
		DllExport void __stdcall SetVideoScale(uint32_t scale) { EmulationSettings::SetVideoScale(scale); }
		DllExport void __stdcall SetVideoFilter(VideoFilterType filter) { EmulationSettings::SetVideoFilterType(filter); }
		DllExport void __stdcall GetRgbPalette(uint32_t *paletteBuffer) { EmulationSettings::GetRgbPalette(paletteBuffer); }
		DllExport void __stdcall SetRgbPalette(uint32_t *paletteBuffer) { EmulationSettings::SetRgbPalette(paletteBuffer); }

		DllExport const char* __stdcall GetAudioDevices() 
		{ 
			_returnString = _soundManager->GetAvailableDevices();
			return _returnString.c_str();
		}

		DllExport void __stdcall SetAudioDevice(char* audioDevice) { _soundManager->SetAudioDevice(audioDevice); }

		DllExport void __stdcall GetScreenSize(ScreenSize &size) { VideoDecoder::GetInstance()->GetScreenSize(size); }
		
		//FDS functions
		DllExport uint32_t __stdcall FdsGetSideCount() { return FDS::GetSideCount(); }
		DllExport void __stdcall FdsEjectDisk() { FDS::EjectDisk(); }
		DllExport void __stdcall FdsInsertDisk(uint32_t diskNumber) { FDS::InsertDisk(diskNumber); }
		DllExport void __stdcall FdsSwitchDiskSide() { FDS::SwitchDiskSide(); }
	}
}