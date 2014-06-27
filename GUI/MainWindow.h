#include "stdafx.h"
#include "Renderer.h"
#include "SoundManager.h"
#include "../Core/Console.h"

namespace NES {
	class MainWindow
	{
	private:
		static MainWindow *Instance;
		HINSTANCE _hInstance;
		HWND _hWnd;
		int _nCmdShow;
		unique_ptr<Renderer> _renderer;
		unique_ptr<SoundManager> _soundManager;
		unique_ptr<Console> _console;
		unique_ptr<thread> _emuThread;
		wstring _currentROM;

	private:
		bool Initialize();
		HRESULT InitWindow();
		static LRESULT CALLBACK WndProc(HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam);
		static INT_PTR CALLBACK About(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam);

		static MainWindow* GetInstance() { return MainWindow::Instance; }

		void SaveTestResult();
		void RunTests();
		
		vector<wstring> GetFolders(wstring rootFolder);
		vector<wstring> GetFilesInFolder(wstring folder, wstring mask, bool recursive);

		void LimitFPS_Click();
		void ShowFPS_Click();

		void SetMenuEnabled(int resourceID, bool enabled);
		
		bool IsMenuChecked(int resourceID);
		bool SetMenuCheck(int resourceID, bool checked);
		bool ToggleMenuCheck(int resourceID);

		wstring SelectROM();
		void Start(wstring romFilename);
		void Reset();
		void Stop(bool powerOff);

		void InitializeOptions();

	public:
		MainWindow(HINSTANCE hInstance, int nCmdShow) : _hInstance(hInstance), _nCmdShow(nCmdShow) 
		{
			MainWindow::Instance = this;
		}

		int Run();
	};
}