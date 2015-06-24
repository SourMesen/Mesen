#include "stdafx.h"
#include "Renderer.h"
#include "SoundManager.h"
#include "../Core/Console.h"
#include "../Utilities/ConfigManager.h"
#include "../Core/GameServer.h"
#include "../Core/GameClient.h"
#include "DebugWindow.h"

namespace NES {
	class MainWindow : public INotificationListener
	{
	private:
		static MainWindow *Instance;

		const wchar_t* _windowName = L"NESEmu";

		HINSTANCE _hInstance;
		HWND _hWnd;
		int _nCmdShow;
		unique_ptr<Renderer> _renderer;
		unique_ptr<SoundManager> _soundManager;
		unique_ptr<Console> _console;
		unique_ptr<thread> _emuThread;
		wstring _currentROM;
		wstring _currentROMName;

		unique_ptr<DebugWindow> _debugWindow;

		int _currentSaveSlot = 0;

		bool _runningTests = false;
		bool _playingMovie = false;

	private:
		bool Initialize();
		HRESULT InitWindow();
		static LRESULT CALLBACK WndProc(HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam);
		static INT_PTR CALLBACK About(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam);
		static INT_PTR CALLBACK ConnectWndProc(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam);
		static INT_PTR CALLBACK InputConfig(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam);
		static INT_PTR CALLBACK ControllerSetup(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam);

		static MainWindow* GetInstance() { return MainWindow::Instance; }

		void SaveTestResult();
		void RunTests();

		void LimitFPS_Click();
		void ShowFPS_Click();

		void SetMenuEnabled(int resourceID, bool enabled);
		
		bool IsMenuChecked(int resourceID);
		bool SetMenuCheck(int resourceID, bool checked);
		bool ToggleMenuCheck(int resourceID);

		wstring SelectROM(wstring romFilepath = L"");
		void StartEmuThread();
		void Start(wstring romFilepath = L"");
		void Reset();
		void Stop(bool powerOff);

		void InitializeOptions();

		void SelectSaveSlot(int slot);
		void AddToMRU(wstring romFilepath);
		void UpdateMRUMenu();
		void UpdateMenu();

	public:
		MainWindow(HINSTANCE hInstance, int nCmdShow) : _hInstance(hInstance), _nCmdShow(nCmdShow) 
		{
			MainWindow::Instance = this;
		}

		int Run();

		virtual void MainWindow::ProcessNotification(ConsoleNotificationType type);
	};
}