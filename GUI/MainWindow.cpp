#include "stdafx.h"
#include "Resource.h"
#include "MainWindow.h"
#include "../Core/Console.h"
#include "../Utilities/ConfigManager.h"
#include "../Utilities/FolderUtilities.h"
#include "InputManager.h"

using namespace DirectX;

namespace NES 
{
	MainWindow* MainWindow::Instance = nullptr;

	bool MainWindow::Initialize()
	{
		if(FAILED(InitWindow())) {
			return false;
		}

		_renderer.reset(new Renderer(_hWnd));
		_soundManager.reset(new SoundManager(_hWnd));

		Console::RegisterMessageManager(_renderer.get());
		Console::RegisterNotificationListener(this);

		return true;
	}

	void CreateConsole()
	{
		CONSOLE_SCREEN_BUFFER_INFO consoleInfo;
		int consoleHandleR, consoleHandleW;
		long stdioHandle;
		FILE *fptr;

		AllocConsole();
		std::wstring strW = L"Dev Console";
		SetConsoleTitle(strW.c_str());

		EnableMenuItem(GetSystemMenu(GetConsoleWindow(), FALSE), SC_CLOSE, MF_GRAYED);
		DrawMenuBar(GetConsoleWindow());

		GetConsoleScreenBufferInfo(GetStdHandle(STD_OUTPUT_HANDLE), &consoleInfo);

		stdioHandle = (long)GetStdHandle(STD_INPUT_HANDLE);
		consoleHandleR = _open_osfhandle(stdioHandle, _O_TEXT);
		fptr = _fdopen(consoleHandleR, "r");
		*stdin = *fptr;
		setvbuf(stdin, NULL, _IONBF, 0);

		stdioHandle = (long)GetStdHandle(STD_OUTPUT_HANDLE);
		consoleHandleW = _open_osfhandle(stdioHandle, _O_TEXT);
		fptr = _fdopen(consoleHandleW, "w");
		*stdout = *fptr;
		setvbuf(stdout, NULL, _IONBF, 0);

		stdioHandle = (long)GetStdHandle(STD_ERROR_HANDLE);
		*stderr = *fptr;
		setvbuf(stderr, NULL, _IONBF, 0);
	}

	int MainWindow::Run()
	{
		//#if _DEBUG
			CreateConsole();
		//#endif

		Initialize();

		InitializeOptions();
		InputManager inputManager(_hWnd);
		ControlManager::RegisterControlDevice(&inputManager, 0);

		HACCEL hAccel = LoadAccelerators(_hInstance, MAKEINTRESOURCE(IDC_Accelerator));
		if(hAccel == nullptr) {
			//error
			std::cout << "error";
		}

		MSG msg = { 0 };
		while(WM_QUIT != msg.message) {
			if(PeekMessage(&msg, nullptr, 0, 0, PM_REMOVE)) {
				if(!TranslateAccelerator(_hWnd, hAccel, &msg)) {
					TranslateMessage(&msg);
					DispatchMessage(&msg);
				}
			} else {
				_renderer->Render();
			}

			UpdateMenu();

			std::this_thread::sleep_for(std::chrono::duration<int, std::milli>(1));

			if(_playingMovie) {
				if(!Movie::Playing()) {
					_playingMovie = false;
					//Pause game
					Stop(false);
				}
			}
		}

		return (int)msg.wParam;
	}

	//--------------------------------------------------------------------------------------
	// Register class and create window
	//--------------------------------------------------------------------------------------
	HRESULT MainWindow::InitWindow()
	{
		// Register class
		WNDCLASSEX wcex;
		wcex.cbSize = sizeof(WNDCLASSEX);
		wcex.style = CS_HREDRAW | CS_VREDRAW;
		wcex.lpfnWndProc = WndProc;
		wcex.cbClsExtra = 0;
		wcex.cbWndExtra = 0;
		wcex.hInstance = _hInstance;
		wcex.hIcon = LoadIcon(_hInstance, (LPCTSTR)IDI_GUI);
		wcex.hCursor = LoadCursor(nullptr, IDC_ARROW);
		wcex.hbrBackground = (HBRUSH)(COLOR_WINDOW + 1);
		wcex.lpszMenuName = MAKEINTRESOURCE(IDC_GUI);
		wcex.lpszClassName = L"NESEmu";
		wcex.hIconSm = LoadIcon(wcex.hInstance, (LPCTSTR)IDI_SMALL);
		if(!RegisterClassEx(&wcex))
			return E_FAIL;

		// Create window
		RECT rc = { 0, 0, 800, 700 };
		AdjustWindowRect(&rc, WS_OVERLAPPEDWINDOW, FALSE);
		_hWnd = CreateWindow(L"NESEmu", _windowName,
			WS_OVERLAPPEDWINDOW,
			CW_USEDEFAULT, CW_USEDEFAULT, rc.right - rc.left, rc.bottom - rc.top, nullptr, nullptr, _hInstance,
			nullptr);
		if(!_hWnd) {
			return E_FAIL;
		}

		ShowWindow(_hWnd, _nCmdShow);

		return S_OK;
	}

	INT_PTR CALLBACK MainWindow::About(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam)
	{
		UNREFERENCED_PARAMETER(lParam);
		switch(message) {
			case WM_INITDIALOG:
				return (INT_PTR)TRUE;

			case WM_COMMAND:
				if(LOWORD(wParam) == IDOK || LOWORD(wParam) == IDCANCEL) {
					EndDialog(hDlg, LOWORD(wParam));
					return (INT_PTR)TRUE;
				}
				break;
		}
		return (INT_PTR)FALSE;
	}
	
	INT_PTR CALLBACK MainWindow::ConnectWndProc(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam)
	{
		UNREFERENCED_PARAMETER(lParam);
		wchar_t hostName[1000];
		wstring lastHost;

		switch(message) {
			case WM_INITDIALOG:

				lastHost = ConfigManager::GetValue<wstring>(Config::LastNetPlayHost);
				SetDlgItemText(hDlg, IDC_HOSTNAME, lastHost.size() > 0 ? lastHost.c_str() : L"localhost");
				return (INT_PTR)TRUE;

			case WM_COMMAND:
				if(LOWORD(wParam) == IDOK || LOWORD(wParam) == IDCANCEL) {
					if(LOWORD(wParam) == IDOK) {
						GetDlgItemText(hDlg, IDC_HOSTNAME, (LPWSTR)hostName, 1000);
						ConfigManager::SetValue(Config::LastNetPlayHost, wstring(hostName));
						GameClient::Connect(utf8util::UTF8FromUTF16(hostName).c_str(), 8888);
					}

					EndDialog(hDlg, LOWORD(wParam));
					return (INT_PTR)TRUE;
				}
				break;
		}
		return (INT_PTR)FALSE;
	}

	INT_PTR CALLBACK MainWindow::ControllerSetup(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam)
	{
		UNREFERENCED_PARAMETER(lParam);
		static HWND _focusedControl;
		switch(message) {
			case WM_INITDIALOG:
				HWND cboPort1;
				cboPort1 = GetDlgItem(hDlg, IDC_CBOPORT1);
				SendMessage(cboPort1, CB_ADDSTRING, 0, (LPARAM)_T("<none>"));
				SendMessage(cboPort1, CB_ADDSTRING, 0, (LPARAM)_T("Gamepad"));
				SendMessage(cboPort1, CB_SETCURSEL, 0, 0);
				return (INT_PTR)TRUE;

			case WM_SETFOCUS:
			case WM_SETCURSOR:
				_focusedControl = GetFocus();
				break;

			case WM_KEYDOWN:
				break;

			case WM_COMMAND:
				if(LOWORD(wParam) == IDOK || LOWORD(wParam) == IDCANCEL) {
					if(LOWORD(wParam) == IDOK) {
						//Save settings
						ConfigManager::SetValue(Config::Player1_ButtonA, "V");
					}

					EndDialog(hDlg, LOWORD(wParam));
					return (INT_PTR)TRUE;
				}
				break;
		}
		return (INT_PTR)FALSE;
	}

	INT_PTR CALLBACK MainWindow::InputConfig(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam)
	{
		UNREFERENCED_PARAMETER(lParam);
		switch(message) {
			case WM_INITDIALOG:
				HWND cboPort1;
				cboPort1 = GetDlgItem(hDlg, IDC_CBOPORT1);
				SendMessage(cboPort1, CB_ADDSTRING, 0, (LPARAM)_T("<none>"));
				SendMessage(cboPort1, CB_ADDSTRING, 0, (LPARAM)_T("Gamepad"));
				SendMessage(cboPort1, CB_SETCURSEL, 0, 0);
				return (INT_PTR)TRUE;

			case WM_COMMAND:
				if(LOWORD(wParam) == IDC_BTNPORT1KEYS) {
					DialogBox(nullptr, MAKEINTRESOURCE(IDD_CONTROLLERSETUP), hDlg, ControllerSetup);
				} else if(LOWORD(wParam) == IDOK || LOWORD(wParam) == IDCANCEL) {
					if(LOWORD(wParam) == IDOK) {
						//Save settings
						ConfigManager::SetValue(Config::Player1_ButtonA, "V");
					}

					EndDialog(hDlg, LOWORD(wParam));
					return (INT_PTR)TRUE;
				}
				break;
		}
		return (INT_PTR)FALSE;
	}

	void MainWindow::InitializeOptions()
	{
		if(ConfigManager::GetValue<bool>(Config::LimitFPS)) {
			Console::SetFlags(EmulationFlags::LimitFPS);
			SetMenuCheck(ID_OPTIONS_LIMITFPS, true);
		}

		if(ConfigManager::GetValue<bool>(Config::ShowFPS)) {
			_renderer->SetFlags(UIFlags::ShowFPS);
			SetMenuCheck(ID_OPTIONS_SHOWFPS, true);
		}
		
		UpdateMRUMenu();
	}

	void MainWindow::UpdateMRUMenu()
	{
		wstring MRU[5] = {
			ConfigManager::GetValue<wstring>(Config::MRU0),
			ConfigManager::GetValue<wstring>(Config::MRU1),
			ConfigManager::GetValue<wstring>(Config::MRU2),
			ConfigManager::GetValue<wstring>(Config::MRU3),
			ConfigManager::GetValue<wstring>(Config::MRU4)
		};
		int menuIDs[5] = { ID_RECENTFILES_MRU1, ID_RECENTFILES_MRU2, ID_RECENTFILES_MRU3, ID_RECENTFILES_MRU4, ID_RECENTFILES_MRU5 };

		HMENU hMenu = GetMenu(_hWnd);

		MENUITEMINFOW info;
		memset(&info, 0, sizeof(info));
		info.cbSize = sizeof(info);
		info.fMask = MIIM_TYPE;
		info.fType = MFT_STRING;
		info.cch = 256;

		for(int i = 0; i < 5; i++) {
			if(!MRU[i].empty()) {
				info.dwTypeData = (LPWSTR)MRU[i].c_str();
				SetMenuItemInfo(hMenu, menuIDs[i], false, &info);
			}
		}
	}

	wstring MainWindow::SelectROM(wstring filepath)
	{
		if(filepath.empty()) {
			filepath = FolderUtilities::OpenFile(L"All supported formats (*.nes, *.zip)\0*.NES;*.ZIP\0NES Roms (*.nes)\0*.NES\0ZIP Archives (*.zip)\0*.ZIP\0All (*.*)\0*.*", ConfigManager::GetValue<wstring>(Config::LastGameFolder), false);
			if(!filepath.empty()) {
				ConfigManager::SetValue(Config::LastGameFolder, FolderUtilities::GetFolderName(filepath));
			}
		}
		
		if(!filepath.empty()) {
			ConfigManager::AddToMRU(filepath);			
			UpdateMRUMenu();
			Start(filepath);
		}

		return filepath;
	}

	void MainWindow::ProcessNotification(ConsoleNotificationType type)
	{
		switch(type) {
			case ConsoleNotificationType::GameLoaded:
				if(_console.get() != Console::GetInstance()) {
					_console.reset(Console::GetInstance());
				}

				StartEmuThread();
				break;

			case ConsoleNotificationType::GamePaused:
				_soundManager->Reset();
				break;
		}
	}

	void MainWindow::UpdateMenu()
	{
		bool running = (bool)_emuThread && !Console::CheckFlag(EmulationFlags::Paused);
		bool romLoaded = (bool)_console;
		bool clientConnected = GameClient::Connected();
		bool serverStarted = GameServer::Started();
		bool moviePlayingRecording = Movie::Playing() || Movie::Recording();

		SetMenuEnabled(ID_NES_PAUSE, running && !clientConnected);
		SetMenuEnabled(ID_NES_RESET, romLoaded && !clientConnected);
		SetMenuEnabled(ID_NES_STOP, romLoaded && !clientConnected);
		SetMenuEnabled(ID_NES_RESUME, !running && romLoaded);

		SetMenuEnabled(ID_FILE_QUICKLOAD, running && !clientConnected && !moviePlayingRecording);
		SetMenuEnabled(ID_FILE_QUICKSAVE, running);

		SetMenuEnabled(ID_MOVIES_PLAY, romLoaded && !clientConnected && !moviePlayingRecording);
		SetMenuEnabled(ID_RECORDFROM_START, romLoaded && !clientConnected && !moviePlayingRecording);
		SetMenuEnabled(ID_RECORDFROM_NOW, romLoaded && !moviePlayingRecording);
		SetMenuEnabled(ID_MOVIES_STOP, moviePlayingRecording);

		SetMenuEnabled(ID_NETPLAY_STARTSERVER, !serverStarted && !clientConnected && !Movie::Playing());
		SetMenuEnabled(ID_NETPLAY_STOPSERVER, serverStarted && !clientConnected);

		SetMenuEnabled(ID_NETPLAY_CONNECT, !serverStarted && !clientConnected && !Movie::Playing());
		SetMenuEnabled(ID_NETPLAY_DISCONNECT, !serverStarted && clientConnected);

		SetMenuEnabled(ID_TOOLS_TAKESCREENSHOT, running);

		SetMenuCheck(ID_SAVESTATESLOT_1, _currentSaveSlot == 0);
		SetMenuCheck(ID_SAVESTATESLOT_2, _currentSaveSlot == 1);
		SetMenuCheck(ID_SAVESTATESLOT_3, _currentSaveSlot == 2);
		SetMenuCheck(ID_SAVESTATESLOT_4, _currentSaveSlot == 3);
		SetMenuCheck(ID_SAVESTATESLOT_5, _currentSaveSlot == 4);
	}


	void MainWindow::StartEmuThread()
	{
		if(!_emuThread) {
			_emuThread.reset(new thread(&Console::Run, _console.get()));
		}

		_currentROM = _console->GetROMPath();
		_currentROMName = FolderUtilities::GetFilename(_console->GetROMPath(), false);
		SetWindowText(_hWnd, (wstring(_windowName) + L": " + _currentROMName).c_str());

		Console::ClearFlags(EmulationFlags::Paused);

		if(IsMenuChecked(ID_OPTIONS_SHOWFPS)) {
			_renderer->SetFlags(UIFlags::ShowFPS);
		}
	}

	void MainWindow::Start(wstring romFilepath)
	{
		if(romFilepath.length() == 0 && _console) {
			//Resume where we paused
			StartEmuThread();
		} else {
			if(romFilepath.length() > 0) {
				_currentROM = romFilepath;
			}
			Console::LoadROM(_currentROM);
		}
	}

	void MainWindow::Stop(bool powerOff)
	{
		if(powerOff && _emuThread) {
			_soundManager->Reset();

			if(_console) {
				_console->Stop();
			}
			_emuThread->join();
			_console.reset();
			_emuThread.reset();
		} else {
			Console::SetFlags(EmulationFlags::Paused);
		}
	}

	void MainWindow::Reset()
	{
		if(_console) {
			_soundManager->Reset();
			Console::Pause();
			Console::Reset();
			Console::Resume();
		}
	}

	void MainWindow::SetMenuEnabled(int resourceID, bool enabled)
	{
		HMENU hMenu = GetMenu(_hWnd);
		EnableMenuItem(hMenu, resourceID, enabled ? MF_ENABLED : MF_GRAYED);
	}

	bool MainWindow::IsMenuChecked(int resourceID)
	{
		HMENU hMenu = GetMenu(_hWnd);
		return (GetMenuState(hMenu, resourceID, MF_BYCOMMAND) & MF_CHECKED) == MF_CHECKED;
	}

	bool MainWindow::SetMenuCheck(int resourceID, bool checked)
	{
		HMENU hMenu = GetMenu(_hWnd);		
		CheckMenuItem(hMenu, resourceID, MF_BYCOMMAND | (checked ? MF_CHECKED : MF_UNCHECKED));
		return checked;
	}

	bool MainWindow::ToggleMenuCheck(int resourceID)
	{
		return SetMenuCheck(resourceID, !IsMenuChecked(resourceID));
	}

	void MainWindow::LimitFPS_Click()
	{
		if(ToggleMenuCheck(ID_OPTIONS_LIMITFPS)) {
			Console::SetFlags(EmulationFlags::LimitFPS);
			ConfigManager::SetValue(Config::LimitFPS, true);
		} else {
			Console::ClearFlags(EmulationFlags::LimitFPS);
			ConfigManager::SetValue(Config::LimitFPS, false);
		}
	}

	void MainWindow::ShowFPS_Click()
	{
		if(ToggleMenuCheck(ID_OPTIONS_SHOWFPS)) {
			_renderer->SetFlags(UIFlags::ShowFPS);
			ConfigManager::SetValue(Config::ShowFPS, true);
		} else {
			_renderer->ClearFlags(UIFlags::ShowFPS);
			ConfigManager::SetValue(Config::ShowFPS, false);
		}
	}

	void MainWindow::SaveTestResult()
	{
		if(_console) {
			_console->SaveTestResult();
		}
	}

	void MainWindow::RunTests()
	{
		Stop(true);
		int passCount = 0;
		int failCount = 0;
		int totalCount = 0;
		for(wstring testROM : FolderUtilities::GetFilesInFolder(L"..\\TestSuite\\", L"*.nes", true)) {
			ifstream testResult(testROM + L".trt", ios::in | ios::binary);

			if(testResult) {
				std::wcout << testROM.substr(13) << ": ";
				uint8_t* expectedResult = new uint8_t[256 * 240 * 4];
				testResult.read((char*)expectedResult, 256 * 240 * 4);

				Console *console = new Console(testROM);
				if(console->RunTest(expectedResult)) {
					std::cout << "Passed";
					passCount++;
				} else {
					std::cout << "FAILED";
					failCount++;
				}
				std::cout << std::endl;

				testResult.close();

				delete console;
				delete[] expectedResult;
			} else {
				//std::wcout << "[No result]" << std::endl;
			}
			totalCount++;
		}
		Stop(true);

		std::cout << "------------------------" << std::endl;
		std::cout << passCount << " / " << totalCount << " + " << failCount << " FAILED" << std::endl;
		std::cout << "------------------------" << std::endl;
	}

	void MainWindow::SelectSaveSlot(int slot)
	{
		_currentSaveSlot = slot % 5;
		_renderer->DisplayMessage(L"Savestate slot: " + std::to_wstring(_currentSaveSlot + 1));
	}

	LRESULT CALLBACK MainWindow::WndProc(HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam)
	{
		static MainWindow *mainWindow = MainWindow::GetInstance();
		wstring filename;
		PAINTSTRUCT ps;
		int wmId, wmEvent;
		HDC hdc;

		switch(message) {
			case WM_COMMAND:
				wmId    = LOWORD(wParam);
				wmEvent = HIWORD(wParam);
				// Parse the menu selections:
				switch(wmId) {
					case ID_FILE_OPEN:
						mainWindow->SelectROM();
						break;

					case ID_SAVESTATESLOT_1:
						mainWindow->SelectSaveSlot(0);
						break;
					case ID_SAVESTATESLOT_2:
						mainWindow->SelectSaveSlot(1);
						break;
					case ID_SAVESTATESLOT_3:
						mainWindow->SelectSaveSlot(2);
						break;
					case ID_SAVESTATESLOT_4:
						mainWindow->SelectSaveSlot(3);
						break;
					case ID_SAVESTATESLOT_5:
						mainWindow->SelectSaveSlot(4);
						break;

					case ID_RECENTFILES_MRU1:
						mainWindow->SelectROM(ConfigManager::GetValue<wstring>(Config::MRU0));
						break;
					case ID_RECENTFILES_MRU2:
						mainWindow->SelectROM(ConfigManager::GetValue<wstring>(Config::MRU1));
						break;
					case ID_RECENTFILES_MRU3:
						mainWindow->SelectROM(ConfigManager::GetValue<wstring>(Config::MRU2));
						break;
					case ID_RECENTFILES_MRU4:
						mainWindow->SelectROM(ConfigManager::GetValue<wstring>(Config::MRU3));
						break;
					case ID_RECENTFILES_MRU5:
						mainWindow->SelectROM(ConfigManager::GetValue<wstring>(Config::MRU4));
						break;

					case ID_FILE_QUICKLOAD:
						mainWindow->_console->LoadState(FolderUtilities::GetSaveStateFolder() + mainWindow->_currentROMName + L".ss" + std::to_wstring(mainWindow->_currentSaveSlot + 1));
						break;
					case ID_FILE_QUICKSAVE:
						mainWindow->_console->SaveState(FolderUtilities::GetSaveStateFolder() + mainWindow->_currentROMName + L".ss" + std::to_wstring(mainWindow->_currentSaveSlot + 1));
						break;
					case ID_CHANGESLOT:
						mainWindow->SelectSaveSlot(mainWindow->_currentSaveSlot + 1);
						break;
					case ID_FILE_EXIT:
						DestroyWindow(hWnd);
						break;

					case ID_NES_RESUME:
						Console::ClearFlags(EmulationFlags::Paused);
						break;
					case ID_NES_PAUSE:
						mainWindow->Stop(false);
						break;
					case ID_NES_STOP:
						mainWindow->Stop(true);
						break;
					case ID_NES_RESET:
						mainWindow->Reset();
						break;

					case ID_OPTIONS_LIMITFPS:
						mainWindow->LimitFPS_Click();
						break;
					case ID_OPTIONS_SHOWFPS:
						mainWindow->ShowFPS_Click();
						break;
					case ID_OPTIONS_INPUT:
						DialogBox(nullptr, MAKEINTRESOURCE(IDD_INPUTCONFIG), hWnd, InputConfig);
						break;

					case ID_MOVIES_PLAY:
						filename = FolderUtilities::OpenFile(L"Movie Files (*.nmo)\0*.nmo\0All (*.*)\0*.*", FolderUtilities::GetMovieFolder(), false);
						if(!filename.empty()) {
							mainWindow->_playingMovie = true;
							Movie::Play(filename);
						}
						break;
					case ID_RECORDFROM_START:
					case ID_RECORDFROM_NOW:
						filename = FolderUtilities::OpenFile(L"Movie Files (*.nmo)\0*.nmo\0All (*.*)\0*.*", FolderUtilities::GetMovieFolder(), true, L"nmo");
						if(!filename.empty()) {
							Movie::Record(filename, wmId == ID_RECORDFROM_START);
						}
						break;

					case ID_MOVIES_STOP:
						Movie::Stop();
						mainWindow->_playingMovie = false;
						break;

					case ID_NETPLAY_STARTSERVER:
						GameServer::StartServer();
						break;
					case ID_NETPLAY_STOPSERVER:
						GameServer::StopServer();
						break;

					case ID_NETPLAY_CONNECT:
						DialogBox(nullptr, MAKEINTRESOURCE(IDD_CONNECT), hWnd, ConnectWndProc);
						break;
					case ID_NETPLAY_DISCONNECT:
						GameClient::Disconnect();
						break;

					case ID_TOOLS_TAKESCREENSHOT:
						mainWindow->_renderer->TakeScreenshot(mainWindow->_currentROMName);
						break;

					case ID_TESTS_RUNTESTS:
						mainWindow->RunTests();
						break;
					case ID_TESTS_SAVETESTRESULT:
						mainWindow->SaveTestResult();
						break;

					case ID_HELP_ABOUT:
						DialogBox(nullptr, MAKEINTRESOURCE(IDD_ABOUTBOX), hWnd, About);
						break;

					default:
						return DefWindowProc(hWnd, message, wParam, lParam);
				}
				break;
			case WM_PAINT:
				hdc = BeginPaint(hWnd, &ps);
				EndPaint(hWnd, &ps);
				break;

			case WM_SIZE:
				if(wParam == SIZE_RESTORED) {
					RECT clientRect;
					RECT windowRect;
					LONG xGap;
					LONG yGap;
					GetWindowRect(hWnd, &windowRect);
					GetClientRect(hWnd, &clientRect);
					
					xGap = (windowRect.right - windowRect.left) - (clientRect.right - clientRect.left);
					yGap = (windowRect.bottom - windowRect.top) - (clientRect.bottom - clientRect.top);

					SetWindowPos(mainWindow->_hWnd, nullptr, windowRect.left, windowRect.top, windowRect.right - windowRect.left, (windowRect.bottom - windowRect.top - xGap) * 224 / 256 + yGap, 0);
				}
				break;

			case WM_WINDOWPOSCHANGING:
				WINDOWPOS* windowPos;
				windowPos = (WINDOWPOS*)lParam;
				
				if(!(windowPos->flags & SWP_NOSIZE)) {
					RECT clientRect;
					RECT windowRect;
					LONG xGap;
					LONG yGap;
					GetWindowRect(hWnd, &windowRect);
					GetClientRect(hWnd, &clientRect);
					
					xGap = (windowRect.right - windowRect.left) - (clientRect.right - clientRect.left);
					yGap = (windowRect.bottom - windowRect.top) - (clientRect.bottom - clientRect.top);

					windowPos->cy = (windowPos->cx - xGap) * 224 / 256 + yGap;
				}
				break;

			case WM_DESTROY:
				GameServer::StopServer();
				GameClient::Disconnect();
				mainWindow->Stop(true);
				PostQuitMessage(0);
				break;

			default:
				return DefWindowProc(hWnd, message, wParam, lParam);
		}

		return 0;
	}
}