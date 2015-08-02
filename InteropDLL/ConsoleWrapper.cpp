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
#include "../Core/StandardController.h"
#include "../Core/EmulationSettings.h"
#include "../Core/VideoDecoder.h"

static NES::Renderer *_renderer = nullptr;
static SoundManager *_soundManager = nullptr;
static vector<shared_ptr<StandardController>> _inputDevices;
static HWND _windowHandle = nullptr;
static HWND _viewerHandle = nullptr;
static string _returnString;

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

		void ProcessNotification(ConsoleNotificationType type)
		{
			_callback((int)type);
		}
	};

	extern "C" {
		DllExport void __stdcall InitializeEmu(char* homeFolder, HWND windowHandle, HWND dxViewerHandle)
		{
			FolderUtilities::SetHomeFolder(homeFolder);

			_windowHandle = windowHandle;
			_viewerHandle = dxViewerHandle;

			_renderer = new NES::Renderer(_viewerHandle);
			_soundManager = new SoundManager(_windowHandle);

			ControlManager::RegisterKeyManager(new WindowsKeyManager(_windowHandle));

			for(int i = 0; i < 4; i++) {
				_inputDevices.push_back(shared_ptr<StandardController>(new StandardController(i)));
			}
		}

		DllExport void __stdcall LoadROM(char* filename) { Console::LoadROM(filename); }
		DllExport void __stdcall AddKnowGameFolder(char* folder) { FolderUtilities::AddKnowGameFolder(folder); }

		DllExport void __stdcall AddKeyMappings(uint32_t port, KeyMapping mapping) { _inputDevices[port]->AddKeyMappings(mapping); }
		DllExport void __stdcall ClearKeyMappings(uint32_t port) { _inputDevices[port]->ClearKeyMappings(); }

		DllExport uint32_t __stdcall GetPressedKey() { return ControlManager::GetPressedKey(); }
		DllExport const char* __stdcall GetKeyName(uint32_t keyCode) 
		{
			_returnString = ControlManager::GetKeyName(keyCode);
			return _returnString.c_str();
		}
		DllExport uint32_t __stdcall GetKeyCode(char* keyName) { return ControlManager::GetKeyCode(keyName); }

		DllExport void __stdcall Run()
		{
			if(Console::GetInstance()) {
				Console::GetInstance()->Run();
			}
		}

		DllExport void __stdcall Resume() { EmulationSettings::ClearFlags(EmulationFlags::Paused); }
		DllExport int __stdcall IsPaused() { return EmulationSettings::CheckFlag(EmulationFlags::Paused); }
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
		DllExport int __stdcall IsServerRunning() { return GameServer::Started(); }

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
		DllExport int __stdcall IsConnected() { return GameClient::Connected(); }

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

		DllExport void __stdcall Render() { _renderer->Render(); }
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
		DllExport int __stdcall MoviePlaying() { return Movie::Playing(); }
		DllExport int __stdcall MovieRecording() { return Movie::Recording(); }

		DllExport void __stdcall CheatAddCustom(uint32_t address, uint8_t value, int32_t compareValue, bool isRelativeAddress) { CheatManager::AddCustomCode(address, value, compareValue, isRelativeAddress); }
		DllExport void __stdcall CheatAddGameGenie(char* code) { CheatManager::AddGameGenieCode(code); }
		DllExport void __stdcall CheatAddProActionRocky(uint32_t code) { CheatManager::AddProActionRockyCode(code); }
		DllExport void __stdcall CheatClear() { CheatManager::ClearCodes(); }

		DllExport void __stdcall SetFlags(uint32_t flags) { EmulationSettings::SetFlags(flags); }
		DllExport void __stdcall ClearFlags(uint32_t flags) { EmulationSettings::ClearFlags(flags); }
		DllExport void __stdcall SetChannelVolume(uint32_t channel, double volume) { EmulationSettings::SetChannelVolume((AudioChannel)channel, volume); }
		DllExport void __stdcall SetAudioLatency(uint32_t msLatency) { EmulationSettings::SetAudioLatency(msLatency); }
		DllExport void __stdcall SetNesModel(uint32_t model) { EmulationSettings::SetNesModel((NesModel)model); }
		DllExport void __stdcall SetOverscanDimensions(uint32_t left, uint32_t right, uint32_t top, uint32_t bottom) { EmulationSettings::SetOverscanDimensions(left, right, top, bottom); }

	}
}