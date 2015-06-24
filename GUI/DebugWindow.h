#pragma once
#include "stdafx.h"
#include "Resource.h"
#include "../Core/INotificationListener.h"
#include "NavigationHistory.h"

struct State;
class Debugger;
class Breakpoint;

struct Color
{
	uint8_t R;
	uint8_t G;
	uint8_t B;
};

class DebugWindow : public INotificationListener
{
private:
	static DebugWindow* Instance;
	
	NavigationHistory _history;

	HWND _hWnd = nullptr;
	shared_ptr<Debugger> _debugger;
	HWND _richEdit = nullptr;
	HWND _toolTipWnd = nullptr;

	HWND _watchList = nullptr;
	HWND _breakpointList = nullptr;

	HBRUSH _bgBrush;

	int32_t _lastActiveLine;
	int32_t _clickAddr;
	int32_t _activeAddr;

private:
	static WNDPROC OriginalRichEditWndProc;
	static INT_PTR CALLBACK RichEditWndProc(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam);
	static INT_PTR CALLBACK DebuggerWndProc(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam);
	static INT_PTR CALLBACK GoToAddrWndProc(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam);
	static INT_PTR CALLBACK BreakpointWndProc(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam);

	void InitializeDialog(HWND hDlg);
	
	void UpdateCPUStatus(State cpuState);

	void InsertColumn(HWND listView, LPWSTR text, int width);
	void InsertRow(HWND listView, vector<wstring> columnText, LPARAM lParam = NULL, bool checked = false);

	void InitializeWatch();
	void RefreshWatchList();
	
	void InitializeBreakpointList();
	void RefreshBreakpointList();
	void EditBreakpoint();
	void UpdateBreakpointWindow(HWND bpWnd, Breakpoint* breakpoint);
	void CommitBreakpointChanges(HWND bpWnd, Breakpoint* breakpoint);

	void RefreshDisassembly();

	wstring GetWordUnderMouse(HWND hEdit, int x, int y);
	void CreateToolTipForRect();

	void GenerateTooltip(LPARAM lParam);
	void ShowTooltip(wstring text, int x, int y);
	void HideTooltip();

	int SetLineColor(Color textColor, Color bgColor, int32_t lineNumber = -1);
	void SetLineSymbol(int16_t symbolCode, Color symbolColor, int32_t lineNumber = -1);
	void SetBreakpoint(int32_t lineNumber = -1);
	void SetActiveStatement(int32_t lineNumber = -1);
	void ClearLineStyling(int32_t lineNumber = -1);

	int GetCurrentLine();
	int32_t GetAddrCharIndex(uint32_t addr);
	uint32_t GetAddrLineNumber(uint32_t addr);
	uint32_t GetLineAddr(int32_t lineNumber);

	void ToggleBreakpoint();

	string GetRTFColor(Color color);

	void GoToAddr(uint32_t addr);
	bool GoToAddr(wstring addrText);
	wstring IntToHex(int value, int minWidth);
	uint32_t HexToInt(wstring hex);

public:
	DebugWindow(HWND parentWnd, shared_ptr<Debugger> debugger);
	~DebugWindow();

	HWND GetHWND()
	{
		return _hWnd;
	}

	virtual void ProcessNotification(ConsoleNotificationType type);
};