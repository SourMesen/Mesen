#include "stdafx.h"
#include "Renderer.h"

namespace NES {
	class MainWindow
	{
	private:
		HINSTANCE _hInstance;
		HWND _hWnd;
		int _nCmdShow;
		Renderer _renderer;

		bool Initialize();
		HRESULT InitWindow();
		static LRESULT CALLBACK WndProc(HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam);
		static INT_PTR CALLBACK About(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam);

		static void RunBenchmark();

	public:
		MainWindow(HINSTANCE hInstance, int nCmdShow) : _hInstance(hInstance), _nCmdShow(nCmdShow) { }
		int Run();
	};
}