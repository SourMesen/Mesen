#include "stdafx.h"
#include "Resource.h"
#include "MainWindow.h"
#include "..\Core\Console.h"
#include "..\Core\Timer.h"
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

		if(!_renderer.Initialize(_hInstance, _hWnd)) {
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
		RECT rc = { 0, 0, 260, 270 };
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
		switch (message)
		{
		case WM_INITDIALOG:
			return (INT_PTR)TRUE;

		case WM_COMMAND:
			if (LOWORD(wParam) == IDOK || LOWORD(wParam) == IDCANCEL)
			{
				EndDialog(hDlg, LOWORD(wParam));
				return (INT_PTR)TRUE;
			}
			break;
		}
		return (INT_PTR)FALSE;
	}

	void MainWindow::OpenROM()
	{
		wchar_t buffer[2000];

		OPENFILENAME ofn;
		ZeroMemory(&ofn , sizeof(ofn));
		ofn.lStructSize = sizeof(ofn);
		ofn.hwndOwner = nullptr;
		ofn.lpstrFile = buffer;
		ofn.lpstrFile[0] = '\0';
		ofn.nMaxFile = sizeof(buffer);
		ofn.lpstrFilter = L"NES Roms\0*.NES\0All\0*.*";
		ofn.nFilterIndex = 1;
		ofn.lpstrFileTitle = nullptr;
		ofn.nMaxFileTitle = 0 ;
		ofn.lpstrInitialDir= nullptr;
		ofn.Flags = OFN_PATHMUSTEXIST|OFN_FILEMUSTEXIST ;

		GetOpenFileName(&ofn);
		
		wstring filename = wstring(buffer);

		if(filename.length() > 0) {
			Stop();

			_console.reset(new Console(filename));
			_emuThread.reset(new thread(&Console::Run, _console.get()));
		}
	}

	void MainWindow::Stop()
	{
		if(_console) {
			_console->Stop();
			_emuThread->join();

			_console.release();
		}
	}

	bool MainWindow::ToggleMenuCheck(int resourceID)
	{
		HMENU hMenu = GetMenu(_hWnd);		
		bool checked = (GetMenuState(hMenu, resourceID, MF_BYCOMMAND) & MF_CHECKED) == MF_CHECKED;
		CheckMenuItem(hMenu, resourceID, MF_BYCOMMAND | (checked ? MF_UNCHECKED : MF_CHECKED));
		return !checked;
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
		Stop();
		for(wstring testROM : GetFilesInFolder(L"TestSuite/*.nes")) {
			ifstream testResult(L"TestSuite/" + testROM + L".trt", ios::in | ios::binary);

			if(testResult) {
				uint8_t* expectedResult = new uint8_t[256 * 240 * 4];

				Console *console = new Console(L"TestSuite/" + testROM);
				std::wcout << testROM << ": ";
				if(console->RunTest(expectedResult)) {
					std::cout << "Passed";
				} else {
					std::cout << "FAILED";
				}
				std::cout << std::endl;

				testResult.close();

				delete[] expectedResult;
			} else {
				std::wcout << testROM << ": [NO KNOWN RESULT]" << std::endl;
			}
		}
	}

	LRESULT CALLBACK MainWindow::WndProc(HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam)
	{
		static MainWindow *mainWindow = MainWindow::GetInstance();
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
						mainWindow->OpenROM();
						break;
					case ID_TESTS_RUNTESTS:
						mainWindow->RunTests();
						break;
					case ID_TESTS_SAVETESTRESULT:
						mainWindow->SaveTestResult();
						break;
					case ID_OPTIONS_LIMITFPS:
						mainWindow->LimitFPS_Click();
						break;
					case ID_HELP_ABOUT:
						DialogBox(nullptr, MAKEINTRESOURCE(IDD_ABOUTBOX), hWnd, About);
						break;
					case ID_FILE_EXIT:
						DestroyWindow(hWnd);
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
				mainWindow->Stop();
				PostQuitMessage(0);
				break;

			default:
				return DefWindowProc(hWnd, message, wParam, lParam);
		}

		return 0;
	}

}