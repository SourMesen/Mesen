#include "stdafx.h"

#include "../Core/MessageManager.h"
#include "../Core/Console.h"
#include "../Windows/Renderer.h"
#include "../Windows/SoundManager.h"
#include "../Windows/InputManager.h"
#include "../Core/GameServer.h"
#include "../Core/GameClient.h"
#include "../Core/ClientConnectionData.h"
#include "../Core/SaveStateManager.h"
#include "../Core/CheatManager.h"

static NES::Renderer *_renderer = nullptr;
static SoundManager *_soundManager = nullptr;
static InputManager *_inputManager = nullptr;
static HWND _windowHandle = nullptr;
static HWND _viewerHandle = nullptr;
static wstring _romFilename;

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
		DllExport void __stdcall InitializeEmu(wchar_t* homeFolder, HWND windowHandle, HWND dxViewerHandle)
		{
			FolderUtilities::SetHomeFolder(homeFolder);

			_windowHandle = windowHandle;
			_viewerHandle = dxViewerHandle;

			_renderer = new NES::Renderer(_viewerHandle);
			MessageManager::RegisterMessageManager(_renderer);
			_renderer->SetFlags(NES::UIFlags::ShowFPS);

			_soundManager = new SoundManager(_windowHandle);
			_inputManager = new InputManager(_windowHandle, 0);
		}

		DllExport void __stdcall LoadROM(wchar_t* filename) { Console::LoadROM(filename); }

		DllExport void __stdcall AddKnowGameFolder(wchar_t* folder) { FolderUtilities::AddKnowGameFolder(folder); }

		DllExport void __stdcall Run()
		{
			if(Console::GetInstance()) {
				Console::GetInstance()->Run();
			}
		}

		DllExport void __stdcall Resume() { Console::ClearFlags(EmulationFlags::Paused); }
		DllExport int __stdcall IsPaused() { return Console::CheckFlag(EmulationFlags::Paused); }
		DllExport void __stdcall Stop()
		{
			if(Console::GetInstance()) {
				Console::GetInstance()->Stop();
			}
		}
		DllExport const wchar_t* __stdcall GetROMPath() 
		{
			_romFilename = Console::GetROMPath();
			return _romFilename.c_str();
		}

		DllExport void __stdcall Reset() { Console::Reset(); }
		DllExport void __stdcall SetFlags(int flags) { Console::SetFlags(flags); }
		DllExport void __stdcall ClearFlags(int flags) { Console::ClearFlags(flags); }

		DllExport void __stdcall StartServer(uint16_t port) { GameServer::StartServer(port); }
		DllExport void __stdcall StopServer() { GameServer::StopServer(); }
		DllExport int __stdcall IsServerRunning() { return GameServer::Started(); }

		DllExport void __stdcall Connect(char* host, uint16_t port, wchar_t* playerName, uint8_t* avatarData, uint32_t avatarSize)
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
				Console::SetFlags(EmulationFlags::Paused);
			}
		}

		DllExport void __stdcall Release()
		{
			GameServer::StopServer();
			GameClient::Disconnect();
			MessageManager::RegisterMessageManager(nullptr);
			delete _renderer;
			delete _soundManager;
			delete _inputManager;
		}

		DllExport void __stdcall Render() { _renderer->Render(); }
		DllExport void __stdcall TakeScreenshot() { _renderer->TakeScreenshot(Console::GetROMPath()); }

		DllExport INotificationListener* __stdcall RegisterNotificationCallback(NotificationListenerCallback callback)
		{
			INotificationListener* listener = new InteropNotificationListener(callback);
			MessageManager::RegisterNotificationListener(listener);
			return listener;
		}
		DllExport void __stdcall UnregisterNotificationCallback(INotificationListener *listener) { MessageManager::UnregisterNotificationListener(listener); }

		DllExport void __stdcall DisplayMessage(wchar_t* title, wchar_t* message) { MessageManager::DisplayMessage(title, message); }

		DllExport void __stdcall SaveState(uint32_t stateIndex) { SaveStateManager::SaveState(stateIndex); }
		DllExport uint32_t __stdcall LoadState(uint32_t stateIndex) { return SaveStateManager::LoadState(stateIndex); }
		DllExport int64_t  __stdcall GetStateInfo(uint32_t stateIndex) { return SaveStateManager::GetStateInfo(stateIndex); }

		DllExport void __stdcall MoviePlay(wchar_t* filename) { Movie::Play(filename); }
		DllExport void __stdcall MovieRecord(wchar_t* filename, int reset) { Movie::Record(filename, reset != 0); }
		DllExport void __stdcall MovieStop() { Movie::Stop(); }
		DllExport int __stdcall MoviePlaying() { return Movie::Playing(); }
		DllExport int __stdcall MovieRecording() { return Movie::Recording(); }

		DllExport void __stdcall CheatAddCustom(uint32_t address, uint8_t value, int32_t compareValue, int isRelativeAddress) { CheatManager::AddCustomCode(address, value, compareValue, isRelativeAddress != 0); }
		DllExport void __stdcall CheatAddGameGenie(char* code) { CheatManager::AddGameGenieCode(code); }
		DllExport void __stdcall CheatAddProActionRocky(uint32_t code) { CheatManager::AddProActionRockyCode(code); }
		DllExport void __stdcall CheatClear() { CheatManager::ClearCodes(); }
	}
}