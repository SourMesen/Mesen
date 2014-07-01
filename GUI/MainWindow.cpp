#include "stdafx.h"
#include "Resource.h"
#include "MainWindow.h"
#include "../Core/Console.h"
#include "../Utilities/ConfigManager.h"
#include "InputManager.h"

using namespace DirectX;

namespace NES {
	MainWindow* MainWindow::Instance = nullptr;

	bool MainWindow::Initialize()
	{
		if(FAILED(InitWindow())) {
			return false;
		}

		_renderer.reset(new Renderer(_hWnd));
		_soundManager.reset(new SoundManager(_hWnd));

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
		#if _DEBUG
			CreateConsole();
		#endif

		Initialize();

		InitializeOptions();
		InputManager inputManager;
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
			std::this_thread::sleep_for(std::chrono::duration<int, std::milli>(1));

			if(_playingMovie) {
				if(!Movie::Playing()) {
					_playingMovie = false;
					_renderer->DisplayMessage(L"Movie ended.", 3000);

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

	void MainWindow::AddToMRU(wstring romFilepath)
	{
		wstring MRU0 = ConfigManager::GetValue<wstring>(Config::MRU0);
		wstring MRU1 = ConfigManager::GetValue<wstring>(Config::MRU1);
		wstring MRU2 = ConfigManager::GetValue<wstring>(Config::MRU2);
		wstring MRU3 = ConfigManager::GetValue<wstring>(Config::MRU3);
		wstring MRU4 = ConfigManager::GetValue<wstring>(Config::MRU4);

		if(MRU0.compare(romFilepath) == 0) {
			return;
		} else if(MRU1.compare(romFilepath) == 0) {
			MRU1 = MRU0;
			MRU0 = romFilepath;
		} else if(MRU2.compare(romFilepath) == 0) {
			MRU2 = MRU1;
			MRU1 = MRU0;
			MRU0 = romFilepath;
		} else if(MRU3.compare(romFilepath) == 0) {
			MRU3 = MRU2;
			MRU2 = MRU1;
			MRU1 = MRU0;
			MRU0 = romFilepath;
		} else {
			MRU4 = MRU3;
			MRU3 = MRU2;
			MRU2 = MRU1;
			MRU1 = MRU0;
			MRU0 = romFilepath;
		}

		ConfigManager::SetValue(Config::MRU0, MRU0);
		ConfigManager::SetValue(Config::MRU1, MRU1);
		ConfigManager::SetValue(Config::MRU2, MRU2);
		ConfigManager::SetValue(Config::MRU3, MRU3);
		ConfigManager::SetValue(Config::MRU4, MRU4);
		
		UpdateMRUMenu();
	}

	void MainWindow::UpdateMRUMenu()
	{
		wstring MRU0 = ConfigManager::GetValue<wstring>(Config::MRU0);
		wstring MRU1 = ConfigManager::GetValue<wstring>(Config::MRU1);
		wstring MRU2 = ConfigManager::GetValue<wstring>(Config::MRU2);
		wstring MRU3 = ConfigManager::GetValue<wstring>(Config::MRU3);
		wstring MRU4 = ConfigManager::GetValue<wstring>(Config::MRU4);

		HMENU hMenu = GetMenu(_hWnd);

		MENUITEMINFOW info;
		//Initialize MENUITEMINFO structure:
		memset(&info, 0, sizeof(info));
		info.cbSize = sizeof(info);
		info.fMask = MIIM_TYPE;
		info.fType = MFT_STRING;
		info.cch = 256;

		if(!MRU0.empty()) {
			info.dwTypeData = (LPWSTR)MRU0.c_str();
			SetMenuItemInfo(hMenu, ID_RECENTFILES_MRU1, false, &info);
		}

		if(!MRU1.empty()) {
			info.dwTypeData = (LPWSTR)MRU1.c_str();
			SetMenuItemInfo(hMenu, ID_RECENTFILES_MRU2, false, &info);
		}

		if(!MRU2.empty()) {
			info.dwTypeData = (LPWSTR)MRU2.c_str();
			SetMenuItemInfo(hMenu, ID_RECENTFILES_MRU3, false, &info);
		}

		if(!MRU3.empty()) {
			info.dwTypeData = (LPWSTR)MRU3.c_str();
			SetMenuItemInfo(hMenu, ID_RECENTFILES_MRU4, false, &info);
		}

		if(!MRU4.empty()) {
			info.dwTypeData = (LPWSTR)MRU4.c_str();
			SetMenuItemInfo(hMenu, ID_RECENTFILES_MRU5, false, &info);
		}
	}

	wstring MainWindow::OpenFile(LPCWSTR filter, bool forSave)
	{
		wchar_t buffer[2000];

		OPENFILENAME ofn;
		ZeroMemory(&ofn, sizeof(ofn));
		ofn.lStructSize = sizeof(ofn);
		ofn.hwndOwner = nullptr;
		ofn.lpstrFile = buffer;
		ofn.lpstrFile[0] = '\0';
		ofn.nMaxFile = sizeof(buffer);
		ofn.lpstrFilter = filter;
		ofn.nFilterIndex = 1;
		ofn.lpstrFileTitle = nullptr;
		ofn.nMaxFileTitle = 0;
		ofn.lpstrInitialDir = nullptr;
		if(forSave) {
			ofn.Flags = OFN_OVERWRITEPROMPT;
			GetSaveFileName(&ofn);
		} else {
			ofn.Flags = OFN_PATHMUSTEXIST | OFN_FILEMUSTEXIST;
			GetOpenFileName(&ofn);
		}
			
		return wstring(buffer);
	}

	wstring MainWindow::SelectROM(wstring filepath)
	{
		if(filepath.empty()) {
			filepath = OpenFile(L"NES Roms (*.nes)\0*.NES\0All (*.*)\0*.*", false);
		}
		
		if(!filepath.empty()) {
			AddToMRU(filepath);
			Start(filepath);
		}

		return filepath;
	}

	void MainWindow::Start(wstring romFilepath)
	{
		if(_emuThread) {
			Stop(false);
		}

		if(romFilepath.length() > 0) {
			_currentROM = romFilepath;
			_currentROMName = GetFilename(romFilepath, false);
			SetWindowText(_hWnd, (wstring(_windowName) + L": " + _currentROMName).c_str());
			_console.reset(new Console(_currentROM));
		}

		if(!_console && _currentROM.length() > 0) {
			_console.reset(new Console(_currentROM));
		}

		if(_console) {
			_emuThread.reset(new thread(&Console::Run, _console.get()));

			SetMenuEnabled(ID_NES_PAUSE, true);
			SetMenuEnabled(ID_NES_RESET, true);
			SetMenuEnabled(ID_NES_STOP, true);
			SetMenuEnabled(ID_NES_RESUME, false);

			SetMenuEnabled(ID_FILE_QUICKLOAD, true);			
			SetMenuEnabled(ID_FILE_QUICKSAVE, true);

			SetMenuEnabled(ID_MOVIES_PLAY, true);
			SetMenuEnabled(ID_RECORDFROM_START, true);
			SetMenuEnabled(ID_RECORDFROM_NOW, true);
			SetMenuEnabled(ID_MOVIES_STOP, true);

			_renderer->ClearFlags(UIFlags::ShowPauseScreen);
			if(IsMenuChecked(ID_OPTIONS_SHOWFPS)) {
				_renderer->SetFlags(UIFlags::ShowFPS);
			}
		}
	}

	void MainWindow::Stop(bool powerOff)
	{
		_renderer->ClearFlags(UIFlags::ShowFPS | UIFlags::ShowPauseScreen);

		_soundManager->Reset();
		if(_console) {
			_console->Stop();
		}
		if(_emuThread) {
			_emuThread->join();

			if(powerOff) {
				_console.reset(nullptr);
			} else {
				_renderer->SetFlags(UIFlags::ShowPauseScreen);
			}

			_emuThread.reset(nullptr);
		}

		SetMenuEnabled(ID_NES_PAUSE, false);
		SetMenuEnabled(ID_NES_RESET, !powerOff);
		SetMenuEnabled(ID_NES_STOP, !powerOff);
		SetMenuEnabled(ID_FILE_QUICKLOAD, !powerOff);
		SetMenuEnabled(ID_FILE_QUICKSAVE, !powerOff);

		SetMenuEnabled(ID_MOVIES_PLAY, !powerOff);
		SetMenuEnabled(ID_RECORDFROM_START, !powerOff);
		SetMenuEnabled(ID_RECORDFROM_NOW, !powerOff);
		SetMenuEnabled(ID_MOVIES_STOP, !powerOff);

		SetMenuEnabled(ID_NES_RESUME, true);
	}

	void MainWindow::Reset()
	{
		if(_console) {
			_soundManager->Reset();
			_console->Reset();
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

	vector<wstring> MainWindow::GetFolders(wstring rootFolder)
	{
		HANDLE hFind;
		WIN32_FIND_DATA data;

		vector<wstring> folders;

		hFind = FindFirstFile((rootFolder + L"*").c_str(), &data);
		if(hFind != INVALID_HANDLE_VALUE) {
			do {
				if(data.dwFileAttributes == FILE_ATTRIBUTE_DIRECTORY && wcscmp(data.cFileName, L".") != 0 && wcscmp(data.cFileName, L"..") != 0) {
					wstring subfolder = rootFolder + data.cFileName + L"\\";
					folders.push_back(subfolder);
					for(wstring folderName : GetFolders(subfolder.c_str())) {
						folders.push_back(folderName);
					}
				}
			}
			while(FindNextFile(hFind, &data));
			FindClose(hFind);
		}

		return folders;
	}

	vector<wstring> MainWindow::GetFilesInFolder(wstring rootFolder, wstring mask, bool recursive)
	{
		HANDLE hFind;
		WIN32_FIND_DATA data;

		vector<wstring> folders;
		vector<wstring> files;
		folders.push_back(rootFolder);

		if(recursive) {
			for(wstring subFolder : GetFolders(rootFolder)) {
				folders.push_back(subFolder);
			}
		}

		for(wstring folder : folders) {
			hFind = FindFirstFile((folder + mask).c_str(), &data);
			if(hFind != INVALID_HANDLE_VALUE) {
				do {
					files.push_back(folder + data.cFileName);
				} while(FindNextFile(hFind, &data));
				FindClose(hFind);
			}
		}

		return files;
	}

	void MainWindow::RunTests()
	{
		Stop(true);
		int passCount = 0;
		int failCount = 0;
		int totalCount = 0;
		for(wstring testROM : GetFilesInFolder(L"..\\TestSuite\\", L"*.nes", true)) {
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
		_renderer->DisplayMessage(L"Savestate slot: " + std::to_wstring(_currentSaveSlot + 1), 3000);

		SetMenuCheck(ID_SAVESTATESLOT_1, _currentSaveSlot == 0);
		SetMenuCheck(ID_SAVESTATESLOT_2, _currentSaveSlot == 1);
		SetMenuCheck(ID_SAVESTATESLOT_3, _currentSaveSlot == 2);
		SetMenuCheck(ID_SAVESTATESLOT_4, _currentSaveSlot == 3);
		SetMenuCheck(ID_SAVESTATESLOT_5, _currentSaveSlot == 4);
	}

	wstring MainWindow::GetFilename(wstring filepath, bool includeExtension)
	{
		wstring filename = filepath.substr(filepath.find_last_of(L"/\\") + 1);
		if(!includeExtension) {
			filename = filename.substr(0, filename.find_last_of(L"."));
		}
		return filename;
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
						if(mainWindow->_console->LoadState(ConfigManager::GetHomeFolder() + mainWindow->_currentROMName + L".ss" + std::to_wstring(mainWindow->_currentSaveSlot + 1))) {
							mainWindow->_renderer->DisplayMessage(L"State loaded.", 3000);
						} else {
							mainWindow->_renderer->DisplayMessage(L"Slot is empty.", 3000);
						}
						break;
					case ID_FILE_QUICKSAVE:
						mainWindow->_console->SaveState(ConfigManager::GetHomeFolder() + mainWindow->_currentROMName + L".ss" + std::to_wstring(mainWindow->_currentSaveSlot + 1));
						mainWindow->_renderer->DisplayMessage(L"State saved.", 3000);
						break;
					case ID_CHANGESLOT:
						mainWindow->SelectSaveSlot(mainWindow->_currentSaveSlot + 1);
						break;
					case ID_FILE_EXIT:
						DestroyWindow(hWnd);
						break;

					case ID_NES_RESUME:
						mainWindow->Start();
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
						filename = mainWindow->OpenFile(L"Movie Files (*.nmo)\0*.nmo\0All (*.*)\0*.*", false);
						if(!filename.empty()) {
							mainWindow->_renderer->DisplayMessage(L"Playing movie: " + mainWindow->GetFilename(filename, true), 3000);
							mainWindow->_playingMovie = true;
							Movie::Play(filename);
						}
						mainWindow->SetMenuEnabled(ID_MOVIES_STOP, true);
						break;
					case ID_RECORDFROM_START:
						filename = mainWindow->OpenFile(L"Movie Files (*.nmo)\0*.nmo\0All (*.*)\0*.*", true);
						if(!filename.empty()) {
							mainWindow->_renderer->DisplayMessage(L"Recording...", 3000);
							Movie::Record(filename, false);
						}						
						mainWindow->SetMenuEnabled(ID_MOVIES_STOP, true);
						break;
					case ID_RECORDFROM_NOW:
						filename = mainWindow->OpenFile(L"Movie Files (*.nmo)\0*.nmo\0All (*.*)\0*.*", false);
						if(!filename.empty()) {
							mainWindow->_renderer->DisplayMessage(L"Recording...", 3000);
							Movie::Record(filename, false);
						}						
						mainWindow->SetMenuEnabled(ID_MOVIES_STOP, true);
						break;
					case ID_MOVIES_STOP:
						Movie::Stop();
						mainWindow->_playingMovie = false;
						mainWindow->SetMenuEnabled(ID_MOVIES_STOP, false);
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
				mainWindow->Stop(true);
				PostQuitMessage(0);
				break;

			default:
				return DefWindowProc(hWnd, message, wParam, lParam);
		}

		return 0;
	}

}