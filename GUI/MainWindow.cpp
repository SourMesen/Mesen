#include "stdafx.h"
#include "Resource.h"
#include "MainWindow.h"
#include "..\Core\Console.h"
#include "..\Core\Timer.h"
#include "InputManager.h"

using namespace DirectX;

namespace NES {
	MainWindow* MainWindow::Instance = nullptr;

	bool MainWindow::Initialize()
	{
		if(FAILED(InitWindow())) {
			return false;
		}

		if(!_renderer.Initialize(_hInstance, _hWnd)) {
			return false;
		}

		if(_soundManager.Initialize(_hWnd)) {
			return false;
		}

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
				_renderer.Render();
			}
			std::this_thread::sleep_for(std::chrono::duration<int, std::milli>(1));
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
		_hWnd = CreateWindow(L"NESEmu", L"NESEmu",
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

	void MainWindow::InitializeOptions()
	{
		Console::SetFlags(EmulationFlags::LimitFPS);
	}

	wstring MainWindow::SelectROM()
	{
		wchar_t buffer[2000];

		OPENFILENAME ofn;
		ZeroMemory(&ofn, sizeof(ofn));
		ofn.lStructSize = sizeof(ofn);
		ofn.hwndOwner = nullptr;
		ofn.lpstrFile = buffer;
		ofn.lpstrFile[0] = '\0';
		ofn.nMaxFile = sizeof(buffer);
		ofn.lpstrFilter = L"NES Roms\0*.NES\0All\0*.*";
		ofn.nFilterIndex = 1;
		ofn.lpstrFileTitle = nullptr;
		ofn.nMaxFileTitle = 0;
		ofn.lpstrInitialDir = nullptr;
		ofn.Flags = OFN_PATHMUSTEXIST | OFN_FILEMUSTEXIST;

		GetOpenFileName(&ofn);

		return wstring(buffer);
	}

	void MainWindow::Start(wstring romFilename = L"")
	{
		if(_emuThread) {
			Stop(false);
		}

		if(romFilename.length() > 0) {
			_currentROM = romFilename;
			_console.reset(new Console(_currentROM));
		}

		if(!_console) {
			_console.reset(new Console(_currentROM));
		}

		if(_console) {
			_emuThread.reset(new thread(&Console::Run, _console.get()));

			SetMenuEnabled(ID_NES_PAUSE, true);
			SetMenuEnabled(ID_NES_RESET, true);
			SetMenuEnabled(ID_NES_STOP, true);
			SetMenuEnabled(ID_NES_RESUME, false);
		}
	}

	void MainWindow::Stop(bool powerOff)
	{
		_soundManager.Reset();
		if(_console) {
			_console->Stop();
			if(powerOff) {
				_console.release();
			}
		}
		if(_emuThread) {
			_emuThread->join();
			_emuThread.release();
		}

		SetMenuEnabled(ID_NES_PAUSE, false);
		SetMenuEnabled(ID_NES_RESET, !powerOff);
		SetMenuEnabled(ID_NES_STOP, !powerOff);
		SetMenuEnabled(ID_NES_RESUME, true);
	}

	void MainWindow::Reset()
	{
		if(_console) {
			_soundManager.Reset();
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
		} else {
			Console::ClearFlags(EmulationFlags::LimitFPS);
		}
	}

	void MainWindow::SaveTestResult()
	{
		if(_console) {
			_console->SaveTestResult();
		}
	}

	vector<wstring> MainWindow::GetFilesInFolder(wstring folderMask)
	{
		HANDLE hFind;
		WIN32_FIND_DATA data;

		vector<wstring> files;

		hFind = FindFirstFile(folderMask.c_str(), &data);
		if(hFind != INVALID_HANDLE_VALUE) {
			do {
				files.push_back(data.cFileName);
			} while(FindNextFile(hFind, &data));
			FindClose(hFind);
		}

		return files;
	}

	void MainWindow::RunTests()
	{
		Stop(true);
		int passCount = 0;
		int failCount = 0;
		int totalCount = 0;
		for(wstring testROM : GetFilesInFolder(L"TestSuite/*.nes")) {
			ifstream testResult(L"TestSuite/" + testROM + L".trt", ios::in | ios::binary);

			if(testResult) {
				uint8_t* expectedResult = new uint8_t[256 * 240 * 4];

				Console *console = new Console(L"TestSuite/" + testROM);
				std::wcout << testROM << ": ";
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
				std::wcout << testROM << ": [No result]" << std::endl;
			}
			totalCount++;
		}
		Stop(true);

		std::cout << "------------------------" << std::endl;
		std::cout << passCount << " / " << totalCount << " + " << failCount << " FAILED" << std::endl;
		std::cout << "------------------------" << std::endl;
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
				switch (wmId) {
					case ID_FILE_OPEN:
						filename = mainWindow->SelectROM();
						if(filename.length() > 0) {
							mainWindow->Start(filename);
						}
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

			case WM_WINDOWPOSCHANGING:
				WINDOWPOS* windowPos;
				windowPos = (WINDOWPOS*)lParam;
				
				RECT clientRect;
				RECT windowRect;
				LONG xGap;
				LONG yGap;
				GetWindowRect(hWnd, &windowRect);
				GetClientRect(hWnd, &clientRect);

				xGap = (windowRect.right - windowRect.left) - (clientRect.right - clientRect.left);
				yGap = (windowRect.bottom - windowRect.top) - (clientRect.bottom - clientRect.top);

				windowPos->cy = (windowPos->cx - xGap) * 240 / 256 + yGap;
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