#include "stdafx.h"
#include "Renderer.h"
#include "../Core/Console.h"

namespace NES {
	class MainWindow
	{
	private:
		static MainWindow *Instance;
		HINSTANCE _hInstance;
		HWND _hWnd;
		int _nCmdShow;
		Renderer _renderer;
		unique_ptr<Console> _console;
		unique_ptr<thread> _emuThread;

		bool Initialize();
		HRESULT InitWindow();
		static LRESULT CALLBACK WndProc(HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam);
		static INT_PTR CALLBACK About(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam);

		static MainWindow* GetInstance() { return MainWindow::Instance; }

		void SaveTestResult();
		void RunTests();

		vector<wstring> GetFilesInFolder(wstring folderMask);

		void LimitFPS_Click();

		bool ToggleMenuCheck(int resourceID);

	public:
		MainWindow(HINSTANCE hInstance, int nCmdShow) : _hInstance(hInstance), _nCmdShow(nCmdShow) 
		{
			MainWindow::Instance = this;
		}
		int Run();
		void OpenROM();
		void Stop();
	};
}