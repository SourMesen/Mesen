#include "stdafx.h"
#include <Windowsx.h>
#include <Richedit.h>
#include <Commctrl.h>
#include "DebugWindow.h"
#include "../Core/Debugger.h"
#include "../Core/Breakpoint.h"
#include "../Core/Console.h"
#include "../Utilities/utf8conv.h"

#define WM_UPDATECPUSTATUS	(WM_APP + 1)

DebugWindow* DebugWindow::Instance = nullptr;
WNDPROC DebugWindow::OriginalRichEditWndProc = nullptr;

DebugWindow::DebugWindow(HWND parentWnd, shared_ptr<Debugger> debugger)
{
	Console::RegisterNotificationListener(this);
	DebugWindow::Instance = this;
	_debugger = debugger;

	LoadLibrary(TEXT("Riched20.dll"));
	_hWnd = CreateDialog(nullptr, MAKEINTRESOURCE(IDD_DEBUG), parentWnd, DebuggerWndProc);
	if(_hWnd) {
		ShowWindow(_hWnd, SW_SHOW);
	}
}

DebugWindow::~DebugWindow()
{
	Console::UnregisterNotificationListener(this);
}

wstring DebugWindow::GetWordUnderMouse(HWND hEdit, int x, int y)
{
	POINT pt = { x, y };
	int ci = SendMessage(hEdit, EM_CHARFROMPOS, 0, (LPARAM)&pt); //current character index
	if(ci < 0) {
		return L"";
	}
	int lix = SendMessage(hEdit, EM_EXLINEFROMCHAR, 0, ci); //line index
	int co = ci - SendMessage(hEdit, EM_LINEINDEX, lix, 0); //current character offset/position
	wchar_t buffer[1024] = { 0, }; //buffer to hold characters (1024 should be enough for the average line)
	((WORD *)buffer)[0] = 1024; //set first byte to size of buffer as specified in MSDN
	SendMessage(hEdit, EM_GETLINE, lix, (LPARAM)buffer);
	if(buffer[co] == ' ' || buffer[co] == '\n' || buffer[co] == '\r') {
		return L""; //currently at a space character or end of line
	}
	std::wostringstream str; //string stream to hold the resultant word
	int i = co;
	bool forward = i == 0 ? true : false; //direction the loop is searching in
	//search for spaces between words
	if(i == lstrlenW(buffer) - 1) {
		return L"";
	}
	while(i < lstrlenW(buffer)) {
		if(!forward) {
			//decrease pointer looking for start of line or space
			i--;
			if(buffer[i] == ' ' || buffer[i] == '\n' || buffer[i] == '\r') {
				i++; //skip over this character
				forward = true;
			} else if(i > 0) {
				continue; //not at 0 keep looping backwards
			} else {
				forward = true; //at 0 so exit this part of the loop
			}
		}
		//increment pointer looking forward for a space		
		if(buffer[i] == ' ' || buffer[i] == '\n' || buffer[i] == '\r') {
			break; //break out of the loop
		}
		//append current characters to output stream
		str << buffer[i];
		++i;
	}
	return str.str(); // <- and here is our word
}

#pragma comment(linker,"\"/manifestdependency:type='win32' name='Microsoft.Windows.Common-Controls' version='6.0.0.0' processorArchitecture='*' publicKeyToken='6595b64144ccf1df' language='*'\"")
void DebugWindow::CreateToolTipForRect()
{
	HWND toolTipWnd = CreateWindowEx(WS_EX_TOPMOST,
		TOOLTIPS_CLASSW, 0, WS_POPUP | TTS_NOPREFIX | TTS_ALWAYSTIP,
		CW_USEDEFAULT, CW_USEDEFAULT, CW_USEDEFAULT, CW_USEDEFAULT,
		0, 0, GetModuleHandle(nullptr), 0);

	TOOLINFOW ti = {};
	ti.cbSize = sizeof(TOOLINFOW);
	ti.uFlags = TTF_ABSOLUTE | TTF_IDISHWND /* | TTF_TRACK */; // Don't specify TTF_TRACK here. Otherwise the tooltip won't show up.
	ti.hwnd = toolTipWnd; // By doing this, you don't have to create another window.
	ti.hinst = NULL;
	ti.uId = (UINT)toolTipWnd;
	ti.lpszText = L"";

	SendMessage(toolTipWnd, TTM_ADDTOOLW, 0, (LPARAM)&ti);
	SendMessage(toolTipWnd, TTM_SETMAXTIPWIDTH, 0, (LPARAM)350);

	_toolTipWnd = toolTipWnd;
}

void DebugWindow::ShowTooltip(wstring text, int x, int y)
{
	TOOLINFOW ti = {};
	ti.cbSize = sizeof(TOOLINFOW);
	ti.hwnd = _toolTipWnd;
	ti.uId = (UINT)_toolTipWnd;
	ti.lpszText = (LPWSTR)text.c_str();
	SendMessage(_toolTipWnd, TTM_UPDATETIPTEXTW, 0, (LPARAM)&ti); // This will update the tooltip content.
	SendMessage(_toolTipWnd, TTM_TRACKACTIVATE, (WPARAM)true, (LPARAM)&ti);
	SendMessage(_toolTipWnd, TTM_TRACKPOSITION, 0, (LPARAM)MAKELONG(x, y)); // Update the position of your tooltip. Screen coordinate.
}

void DebugWindow::HideTooltip()
{
	TOOLINFOW ti = {};
	ti.cbSize = sizeof(TOOLINFOW);
	ti.hwnd = _toolTipWnd;
	ti.uId = (UINT)_toolTipWnd;
	ti.lpszText = (LPWSTR)L"";

	SendMessage(_toolTipWnd, TTM_TRACKACTIVATE, (WPARAM)false, (LPARAM)&ti);
}

void DebugWindow::GenerateTooltip(LPARAM lParam)
{
	static LPARAM lastPos = 0;
	wstring currentWord;

	if(lastPos != lParam) {
		currentWord = GetWordUnderMouse(_richEdit, GET_X_LPARAM(lParam), GET_Y_LPARAM(lParam));
		if(!currentWord.empty()) {
			std::wostringstream output;
			if(currentWord[0] == '$') {
				uint16_t addr = std::stoi(currentWord.substr(1), nullptr, 16);
				output << currentWord << L" = " << std::hex << std::uppercase << std::setfill(L'0') << std::setw(2) << (short)_debugger->GetMemoryValue(addr);

				if(addr >= 0x8000) {
					_clickAddr = addr;
					output << std::endl << "Click to go to location";
				}
				ShowTooltip(output.str().c_str(), GET_X_LPARAM(lParam), GET_Y_LPARAM(lParam));
			} else if(currentWord[0] == L'●') {
				
			} else {
				_clickAddr = -1;
				HideTooltip();
			}
		} else {
			_clickAddr = -1;
			HideTooltip();
		}
		lastPos = lParam;
	}
}

void DebugWindow::ClearLineStyling(int32_t lineNumber)
{
	SendMessage(_richEdit, WM_SETREDRAW, false, 0);

	Color bgColor = { 255, 255, 255 };
	Color textColor = { 0, 0, 0 };
		
	SetLineColor(textColor, bgColor, lineNumber);
	SetLineSymbol(0, bgColor, lineNumber);

	SendMessage(_richEdit, WM_SETREDRAW, true, 0);
	InvalidateRect(_richEdit, nullptr, true);

	RefreshBreakpointList();
}

void DebugWindow::ToggleBreakpoint()
{
	int lineNumber = GetCurrentLine();
	uint16_t addr = GetLineAddr(lineNumber);
	shared_ptr<Breakpoint> existingBreakpoint = _debugger->GetMatchingBreakpoint(BreakpointType::Execute, addr);
	if(existingBreakpoint) {
		_debugger->RemoveBreakpoint(existingBreakpoint);
		ClearLineStyling(lineNumber);
		SetActiveStatement(_lastActiveLine);
	} else {
		_debugger->AddBreakpoint(BreakpointType::Execute, addr, true);
		RefreshBreakpointList();
	}
}

void DebugWindow::SetBreakpoint(int32_t lineNumber)
{
	SendMessage(_richEdit, WM_SETREDRAW, false, 0);
	
	Color bgColor = { 229, 20, 0 };
	Color textColor = { 255, 255, 255 };
		
	SetLineColor(textColor, bgColor, lineNumber);
	SetLineSymbol(9679, bgColor, lineNumber);
	
	SendMessage(_richEdit, WM_SETREDRAW, true, 0);
	InvalidateRect(_richEdit, nullptr, true);
}

void DebugWindow::SetActiveStatement(int32_t lineNumber)
{
	SendMessage(_richEdit, WM_SETREDRAW, false, 0);

	if(lineNumber == -1) {
		lineNumber = GetCurrentLine();
	}

	if(_lastActiveLine != -1) {
		ClearLineStyling(_lastActiveLine);
	}

	Color bgColor = { 174, 254, 79 };
	Color textColor = { 0, 0, 0 };
	Color symbolColor = { 124, 204, 49 };
		
	_lastActiveLine = SetLineColor(textColor, bgColor, lineNumber);
	SetLineSymbol(187, symbolColor, lineNumber);

	SendMessage(_richEdit, WM_SETREDRAW, true, 0);
	InvalidateRect(_richEdit, nullptr, true);
}

int DebugWindow::GetCurrentLine()
{
	int lineStartIndex = SendMessage(_richEdit, EM_LINEINDEX, -1, 0);
	int lineLength = SendMessage(_richEdit, EM_LINELENGTH, lineStartIndex, 0);
	int lineIndex = SendMessage(_richEdit, EM_EXLINEFROMCHAR, 0, lineStartIndex);

	return lineIndex;
}

string DebugWindow::GetRTFColor(Color color)
{
	string result;
	result += "\\red" + std::to_string(color.R);
	result += "\\green" + std::to_string(color.G);
	result += "\\blue" + std::to_string(color.B) + ";";
	return result;
}

int DebugWindow::SetLineColor(Color textColor, Color bgColor, int32_t lineNumber)
{
	int caretPosition = 0;
	SendMessage(_richEdit, EM_GETSEL, (WPARAM)&caretPosition, 0);

	string lineFormat = "{\\rtf1\\ansi{\\fonttbl{\\f0 Consolas;}}{\\colortbl ;";
	lineFormat += GetRTFColor(bgColor);
	lineFormat += GetRTFColor(textColor);
	lineFormat += "}\\f0\\cf2\\highlight1\n";
	wchar_t lineBuffer[1024] = { 0 }; //buffer to hold characters (1024 should be enough for the average line)
	((WORD *)lineBuffer)[0] = 1024; //set first byte to size of buffer as specified in MSDN

	int lineStartIndex = SendMessage(_richEdit, EM_LINEINDEX, lineNumber, 0);
	int lineLength = SendMessage(_richEdit, EM_LINELENGTH, lineStartIndex, 0);
	int lineIndex = SendMessage(_richEdit, EM_EXLINEFROMCHAR, 0, lineStartIndex);
	SendMessage(_richEdit, EM_GETLINE, lineIndex, (LPARAM)lineBuffer);
	lineBuffer[lineLength] = 0;

	SendMessage(_richEdit, EM_SETSEL, lineStartIndex + 9, lineStartIndex + lineLength);
	SendMessage(_richEdit, EM_REPLACESEL, false, (LPARAM)(lineFormat + utf8util::UTF8FromUTF16(lineBuffer+9)).c_str());

	SendMessage(_richEdit, EM_SETSEL, caretPosition, caretPosition);

	return lineIndex;
}

void DebugWindow::SetLineSymbol(int16_t symbolCode, Color symbolColor, int32_t lineNumber)
{
	int caretPosition = 0;
	SendMessage(_richEdit, EM_GETSEL, (WPARAM)&caretPosition, 0);

	Color bgColor = { 230, 230, 230 };
	//9679: Breakpoint
	//10145: Arrow
	string symbol = "{\\rtf1\\ansi{\\colortbl ;";
	symbol += GetRTFColor(symbolColor);
	symbol += GetRTFColor(bgColor);
	symbol += "}\\f0\\cf1\\highlight2\n";
	if(symbolCode > 0) {
		symbol += "\\b\\u" + std::to_string(symbolCode) + "?";
	} else {
		symbol += " ";
	}
		
	int lineStartIndex = SendMessage(_richEdit, EM_LINEINDEX, lineNumber, 0);
					
	SendMessage(_richEdit, EM_SETSEL, lineStartIndex + 1, lineStartIndex + 2);
	SendMessage(_richEdit, EM_REPLACESEL, false, (LPARAM)(symbol).c_str());
	
	SendMessage(_richEdit, EM_SETSEL, caretPosition, caretPosition);
}

wstring DebugWindow::IntToHex(int value, int minWidth)
{
	std::wostringstream output;
	output << std::hex << std::uppercase << std::setfill(L'0') << std::setw(minWidth) << value;
	return output.str();
}

uint32_t DebugWindow::HexToInt(wstring hex)
{
	size_t startPos = hex.find_first_not_of(L" \t$");
	if(startPos != string::npos) {
		hex = hex.substr(startPos);
	}

	return std::stoi(hex, nullptr, 16);
}

bool DebugWindow::GoToAddr(wstring addrText)
{
	uint32_t addr = HexToInt(addrText);
	for(int i = 0; i < 8; i++) {
		if(GetAddrCharIndex(addr) >= 0) {
			GoToAddr(addr);
			return true;
		}
		addr--;
	}

	return false;
}

void DebugWindow::GoToAddr(uint32_t addr)
{
	SendMessage(_richEdit, WM_SETREDRAW, false, 0);
		
	RefreshDisassembly();

	int charIndex = GetAddrCharIndex(addr);
	if(charIndex >= 0) {
		SetFocus(_richEdit);
		SendMessage(_richEdit, EM_SETSEL, charIndex + 6, charIndex + 6);
		SendMessage(_richEdit, EM_SCROLLCARET, 0, 0);
	}

	SendMessage(_richEdit, WM_SETREDRAW, true, 0);
	InvalidateRect(_richEdit, nullptr, true);
}

int32_t DebugWindow::GetAddrCharIndex(uint32_t addr)
{
	FINDTEXTW findText;
	findText.chrg.cpMin = 0;
	findText.chrg.cpMax = -1;
	wstring searchString = IntToHex(addr, 4) + L":";
	findText.lpstrText = searchString.c_str();

	return SendMessage(_richEdit, EM_FINDTEXTW, FR_DOWN, (LPARAM)&findText);
}

uint32_t DebugWindow::GetAddrLineNumber(uint32_t addr)
{
	int charIndex = GetAddrCharIndex(addr);

	if(charIndex >= 0) {
		return SendMessage(_richEdit, EM_EXLINEFROMCHAR, 0, charIndex);
	} else {
		return -1;
	}
}

uint32_t DebugWindow::GetLineAddr(int32_t lineNumber)
{
	int caretPosition = 0;
	SendMessage(_richEdit, EM_GETSEL, (WPARAM)&caretPosition, NULL);

	int lix = SendMessage(_richEdit, EM_EXLINEFROMCHAR, 0, caretPosition); //line index
	wchar_t buffer[1024] = { 0, }; //buffer to hold characters (1024 should be enough for the average line)
	((WORD *)buffer)[0] = 1024; //set first byte to size of buffer as specified in MSDN
	SendMessage(_richEdit, EM_GETLINE, lix, (LPARAM)buffer);

	wstring lineContent = buffer;

	return std::stoi(lineContent.substr(3), nullptr, 16);
}

void DebugWindow::RefreshDisassembly()
{
	if(_debugger->IsCodeChanged()) {
		string text;

		text += "{\\rtf1\\ansi{\\fonttbl{\\f0 Consolas;}{\\f1\\fnil\\fcharset128 \\'82\\'6c\\'82\\'72 \\'96\\'be\\'92\\'a9;}}";

		//0: black, 1: light gray
		Color lightGray = { 230, 230, 230 };
		text += "{\\colortbl ;";
		text += GetRTFColor(lightGray) + "}";

		text += _debugger->GetCode();

		SendMessage(_richEdit, WM_SETTEXT, 0, (LPARAM)text.c_str());

		RefreshBreakpointList();
	}
}

void DebugWindow::InitializeDialog(HWND hDlg)
{
	_hWnd = hDlg;
	_richEdit = GetDlgItem(_hWnd, IDC_RICHEDIT21);
	OriginalRichEditWndProc = (WNDPROC)SetWindowLong(_richEdit, GWL_WNDPROC, (LONG)RichEditWndProc);

	SendMessage(_richEdit, EM_LIMITTEXT, 0xFFFFFF, NULL);

	CreateToolTipForRect();
	RefreshDisassembly();

	SetFocus(_richEdit);

	//CPU Status
	EnableWindow(GetDlgItem(_hWnd, IDC_CHKBRK), false);
	EnableWindow(GetDlgItem(_hWnd, IDC_CHKCARRY), false);
	EnableWindow(GetDlgItem(_hWnd, IDC_CHKDECIMAL), false);
	EnableWindow(GetDlgItem(_hWnd, IDC_CHKINTERRUPT), false);
	EnableWindow(GetDlgItem(_hWnd, IDC_CHKNEGATIVE), false);
	EnableWindow(GetDlgItem(_hWnd, IDC_CHKOVERFLOW), false);
	EnableWindow(GetDlgItem(_hWnd, IDC_CHKRESERVED), false);
	EnableWindow(GetDlgItem(_hWnd, IDC_CHKZERO), false);

	EnableWindow(GetDlgItem(_hWnd, IDC_CHKDMC), false);
	EnableWindow(GetDlgItem(_hWnd, IDC_CHKEXTERNAL), false);
	EnableWindow(GetDlgItem(_hWnd, IDC_CHKFRAMECOUNTER), false);

	InitializeWatch();
	InitializeBreakpointList();
}

void DebugWindow::InitializeBreakpointList()
{
	_breakpointList = GetDlgItem(_hWnd, IDC_BREAKPOINTLIST);
	ListView_SetExtendedListViewStyle(_breakpointList, LVS_EX_FULLROWSELECT | LVS_EX_CHECKBOXES);
	InsertColumn(_breakpointList, L"Relative Address", 80);
	InsertColumn(_breakpointList, L"ROM Addressing", 80);
	InsertColumn(_breakpointList, L"Type", 40);
	InsertColumn(_breakpointList, L"Address", 60);
}

void DebugWindow::RefreshBreakpointList()
{
	ListView_DeleteAllItems(_breakpointList);
	for(shared_ptr<Breakpoint> breakpoint : _debugger->GetBreakpoints()) {
		wstring relAddrString;
		wstring absAddrString = wstring(L"$") + IntToHex(breakpoint->GetAddr(), 4);
		if(breakpoint->IsAbsoluteAddr()) {
			uint32_t relAddr = _debugger->GetRelativeAddress(breakpoint->GetAddr());
			if(relAddr == -1) {
				relAddrString = L"<not in memory>";
			} else {
				relAddrString = wstring(L"$") + IntToHex(relAddr, 4);
			}
		} else {
			relAddrString = absAddrString;
		}

		vector<wstring> bpText = {
			absAddrString,
			breakpoint->GetTypeText(),
			breakpoint->IsAbsoluteAddr() ? L"✔" : L"",
			relAddrString
		};
		InsertRow(_breakpointList, bpText, (LPARAM)breakpoint.get(), breakpoint->IsEnabled());
	}

	for(uint32_t addr : _debugger->GetExecBreakpointAddresses()) {
		SetBreakpoint(GetAddrLineNumber(addr));
	}
}

void DebugWindow::EditBreakpoint()
{
	LVITEM item;
	item.iItem = ListView_GetNextItem(_breakpointList, -1, LVNI_SELECTED);
	if(item.iItem >= 0) {
		item.iSubItem = 0;
		item.mask = LVIF_PARAM;
		ListView_GetItem(_breakpointList, &item);

		DialogBoxParam(nullptr, MAKEINTRESOURCE(IDD_BREAKPOINT), _hWnd, BreakpointWndProc, item.lParam);

		RefreshBreakpointList();
	}
}

void DebugWindow::UpdateBreakpointWindow(HWND bpWnd, Breakpoint* breakpoint)
{
	SetDlgItemText(bpWnd, IDC_EDITBreakAddress, IntToHex(breakpoint->GetAddr(), 4).c_str());
	SendDlgItemMessage(bpWnd, IDC_CHKBreakEnabled, BM_SETCHECK, breakpoint->IsEnabled(), 0);
	SendDlgItemMessage(bpWnd, IDC_CHKAbsoluteAddr, BM_SETCHECK, breakpoint->IsAbsoluteAddr(), 0);
	int selectedValue = IDC_RADExec;
	switch(breakpoint->GetType()) {
		case BreakpointType::Execute: selectedValue = IDC_RADExec; break;
		case BreakpointType::Read: selectedValue = IDC_RADRead; break;
		case BreakpointType::Write: selectedValue = IDC_RADWrite; break;
	}
	CheckRadioButton(bpWnd, IDC_RADExec, IDC_RADWrite, selectedValue);
}

void DebugWindow::CommitBreakpointChanges(HWND bpWnd, Breakpoint* breakpoint)
{
	wchar_t buffer[1000];
	GetDlgItemText(bpWnd, IDC_EDITBreakAddress, buffer, 1000);
	uint32_t addr = HexToInt(buffer);
	bool enabled = IsDlgButtonChecked(bpWnd, IDC_CHKBreakEnabled) == TRUE;
	bool isAbsoluteAddr = IsDlgButtonChecked(bpWnd, IDC_CHKAbsoluteAddr) == TRUE;
	BreakpointType type = BreakpointType::Execute;

	if(IsDlgButtonChecked(bpWnd, IDC_RADRead)) {
		type = BreakpointType::Read;
	} else if(IsDlgButtonChecked(bpWnd, IDC_RADWrite)) {
		type = BreakpointType::Write;
	}

	breakpoint->UpdateBreakpoint(type, addr, isAbsoluteAddr, enabled);
}

void DebugWindow::InsertColumn(HWND listView, LPWSTR text, int width)
{
	LVCOLUMN column;
	column.mask = LVCF_TEXT | LVCF_WIDTH;
	column.pszText = text;
	column.cx = width;

	SendMessage(listView, LVM_INSERTCOLUMN, 0, (LPARAM)&column);
}

void DebugWindow::InsertRow(HWND listView, vector<wstring> columnText, LPARAM lParam, bool checked)
{
	LVITEM item;
	item.mask = LVIF_TEXT | LVIF_PARAM;
	item.iItem = 0;
	item.iSubItem = 0;
	item.pszText = (LPWSTR)columnText[0].c_str();
	item.lParam = lParam;

	SendMessage(listView, LVM_INSERTITEM, 0, (LPARAM)&item);
	ListView_SetCheckState(listView, 0, checked);

	item.mask = LVIF_TEXT;
	for(int i = 1, len = columnText.size(); i < len; i++) {
		item.iSubItem++;
		item.pszText = (LPWSTR)columnText[i].c_str();
		SendMessage(listView, LVM_SETITEM, 0, (LPARAM)&item);
	}
}

void DebugWindow::InitializeWatch()
{
	_watchList = GetDlgItem(_hWnd, IDC_WATCHLIST);
	
	ListView_SetExtendedListViewStyle(_watchList, LVS_EX_FULLROWSELECT);

	InsertColumn(_watchList, L"Value", 100);
	InsertColumn(_watchList, L"Name", 50);
	InsertRow(_watchList, { L"" });
	InsertRow(_watchList, { L"aaaa" });
}

void DebugWindow::UpdateCPUStatus(State cpuState)
{
	SetDlgItemText(_hWnd, IDC_EDITA, IntToHex(cpuState.A, 2).c_str());
	SetDlgItemText(_hWnd, IDC_EDITX, IntToHex(cpuState.X, 2).c_str());
	SetDlgItemText(_hWnd, IDC_EDITY, IntToHex(cpuState.Y, 2).c_str());
	SetDlgItemText(_hWnd, IDC_EDITPC, IntToHex(cpuState.PC, 4).c_str());
	SetDlgItemText(_hWnd, IDC_EDITSP, IntToHex(cpuState.SP, 2).c_str());
	SetDlgItemText(_hWnd, IDC_EDITPS, IntToHex(cpuState.PS, 2).c_str());

	SendDlgItemMessage(_hWnd, IDC_CHKBRK, BM_SETCHECK, cpuState.PS & PSFlags::Break, 0);
	SendDlgItemMessage(_hWnd, IDC_CHKCARRY, BM_SETCHECK, cpuState.PS & PSFlags::Carry, 0);
	SendDlgItemMessage(_hWnd, IDC_CHKDECIMAL, BM_SETCHECK, cpuState.PS & PSFlags::Decimal, 0);
	SendDlgItemMessage(_hWnd, IDC_CHKINTERRUPT, BM_SETCHECK, cpuState.PS & PSFlags::Interrupt, 0);
	SendDlgItemMessage(_hWnd, IDC_CHKNEGATIVE, BM_SETCHECK, cpuState.PS & PSFlags::Negative, 0);
	SendDlgItemMessage(_hWnd, IDC_CHKOVERFLOW, BM_SETCHECK, cpuState.PS & PSFlags::Overflow, 0);
	SendDlgItemMessage(_hWnd, IDC_CHKRESERVED, BM_SETCHECK, cpuState.PS & PSFlags::Reserved, 0);
	SendDlgItemMessage(_hWnd, IDC_CHKZERO, BM_SETCHECK, cpuState.PS & PSFlags::Zero, 0);

	wstring stack;
	for(int i = cpuState.SP + 1; i <= 0xFF; i++) {
		stack += IntToHex(_debugger->GetMemoryValue(i + 0x100), 2) + L" ";
	}
	SetDlgItemText(_hWnd, IDC_EDITSTACK, stack.c_str());

	_activeAddr = cpuState.PC;
	GoToAddr(cpuState.PC);
	SetActiveStatement();
}
	
INT_PTR CALLBACK DebugWindow::DebuggerWndProc(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam)
{
	NMLVDISPINFO *lpNmlvdispInfo;
	LPNMLISTVIEW pnmv;

	switch(message) {
		case WM_INITDIALOG:			
			DebugWindow::Instance->InitializeDialog(hDlg);
			return (INT_PTR)FALSE;

		case WM_SHOWWINDOW:
			DebugWindow::Instance->_debugger->Step(1);
			break;
		case WM_APPCOMMAND:
			switch(GET_APPCOMMAND_LPARAM(lParam)) {
				case APPCOMMAND_BROWSER_BACKWARD:
					
					break;
				
				case APPCOMMAND_BROWSER_FORWARD:
					
					break;
			}
			break;

		case WM_COMMAND:
			switch(LOWORD(wParam)) {
				case ID_DEBUG_RUN:
					DebugWindow::Instance->ClearLineStyling(DebugWindow::Instance->_lastActiveLine);
					DebugWindow::Instance->_debugger->Run();
					break;

				case ID_DEBUG_STEPINTO:
				case ID_DEBUG_STEPOVER:
					DebugWindow::Instance->_debugger->Step(1);
					break;

				case ID_DEBUG_STEPOUT:
					break;

				case ID_DEBUG_RUNTOCURSOR:
					break;

				case ID_DEBUG_TOGGLEBREAKPOINT:
					DebugWindow::Instance->ToggleBreakpoint();
					break;
				
				case ID_VIEW_NAVIGATEBACKWARD:
					DebugWindow::Instance->GoToAddr(DebugWindow::Instance->_history.GoBack());
					break;

				case ID_VIEW_NAVIGATEFORWARD:
					DebugWindow::Instance->GoToAddr(DebugWindow::Instance->_history.GoForward());
					break;

				case ID_VIEW_GOTO:
					DialogBox(nullptr, MAKEINTRESOURCE(IDD_GOTOADDR), DebugWindow::Instance->_hWnd, GoToAddrWndProc);
					break;

				case ID_VIEW_SHOWNEXTSTATEMENT:
					DebugWindow::Instance->GoToAddr(DebugWindow::Instance->_activeAddr);
					break;
			}
			break;

		case WM_UPDATECPUSTATUS:
			DebugWindow::Instance->UpdateCPUStatus(*(State*)wParam);
			break;

		case WM_NOTIFY:
			switch(((LPNMHDR)lParam)->code) {
				case LVN_ENDLABELEDIT:
					lpNmlvdispInfo = (NMLVDISPINFO*)lParam;
					if(((LPNMHDR)lParam)->hwndFrom == DebugWindow::Instance->_watchList && lpNmlvdispInfo->item.pszText != nullptr) {
						/*int itemCount = SendMessage(GetDlgItem(DebugWindow::Instance->_hWnd, IDC_WATCHLIST), LVM_GETITEMCOUNT, 0, 0);
						if(lpNmlvdispInfo->item.iItem == itemCount - 1) {
						//Editing the last one, which means we're adding a new item
						DebugWindow::Instance->InsertRow(GetDlgItem(DebugWindow::Instance->_hWnd, IDC_WATCHLIST), L"");
						}*/
						SetWindowLong(hDlg, DWL_MSGRESULT, true);
						return true;
					}
				case LVN_ITEMCHANGED:
					pnmv = (LPNMLISTVIEW)lParam;
					if((pnmv->uNewState & 0x1000) == 0x1000 && (pnmv->uOldState & 0x2000) == 0x2000) {
						//Unchecked
						((Breakpoint*)pnmv->lParam)->SetEnabled(false);
					} else if((pnmv->uOldState & 0x1000) == 0x1000 && (pnmv->uNewState & 0x2000) == 0x2000) {
						//Checked
						((Breakpoint*)pnmv->lParam)->SetEnabled(true);
					}
					break;
				case NM_DBLCLK:
					if(((LPNMHDR)lParam)->hwndFrom == DebugWindow::Instance->_breakpointList) {
						DebugWindow::Instance->EditBreakpoint();
					}
					break;
			}
			break;
	}
	return (INT_PTR)FALSE;
}

INT_PTR CALLBACK DebugWindow::RichEditWndProc(HWND hEdit, UINT message, WPARAM wParam, LPARAM lParam)
{
	wstring currentWord;

	switch(message) {
		case WM_MOUSEMOVE:
			DebugWindow::Instance->GenerateTooltip(lParam);
			break;

		case WM_MOUSEWHEEL:
			//Disable smooth scrolling (scroll 1 line at a time)
			if((short)HIWORD(wParam) < 0) {
				SendMessage(hEdit, WM_VSCROLL, SB_LINEDOWN, 0);
			} else {
				SendMessage(hEdit, WM_VSCROLL, SB_LINEUP, 0);
			}
			return true;

		case WM_LBUTTONDOWN:
			if(DebugWindow::Instance->_clickAddr >= 0) {
				DebugWindow::Instance->_history.AddLocation(DebugWindow::Instance->_activeAddr);
				DebugWindow::Instance->GoToAddr(DebugWindow::Instance->_clickAddr);
				return true;
			}
			break;

		case WM_SETCURSOR:
			static HCURSOR handCursor = LoadCursor(NULL, IDC_HAND);
			if(DebugWindow::Instance->_clickAddr >= 0) {
				SetCursor(handCursor);
				return true;
			}
			break;
	}

	return CallWindowProc(OriginalRichEditWndProc, hEdit, message,wParam,lParam);
}

INT_PTR CALLBACK DebugWindow::GoToAddrWndProc(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam)
{
	UNREFERENCED_PARAMETER(lParam);
	HWND hEdit = GetDlgItem(hDlg, IDC_EDITADDRESS);
	switch(message) {
		case WM_INITDIALOG:
			SetFocus(hEdit);
			return (INT_PTR)FALSE;

		case WM_COMMAND:
			if(LOWORD(wParam) == IDOK || LOWORD(wParam) == IDCANCEL) {
				if(LOWORD(wParam) == IDOK) {
					wchar_t addressText[1000];
					GetWindowText(hEdit, addressText, sizeof(addressText));
					if(!DebugWindow::Instance->GoToAddr(addressText)) {
						MessageBox(hDlg, L"Invalid address.", L"Error", MB_OK | MB_ICONEXCLAMATION);
						return (INT_PTR)TRUE;
					}
				}
				EndDialog(hDlg, LOWORD(wParam));
				return (INT_PTR)TRUE;
			}
			break;
	}
	return (INT_PTR)FALSE;
}

INT_PTR CALLBACK DebugWindow::BreakpointWndProc(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam)
{
	HWND hEdit = GetDlgItem(hDlg, IDC_EDITBreakAddress);
	static Breakpoint *bp = nullptr;
	switch(message) {
		case WM_INITDIALOG:
			bp = (Breakpoint*)lParam;
			DebugWindow::Instance->UpdateBreakpointWindow(hDlg, bp);
			SetFocus(hEdit);
			return (INT_PTR)FALSE;

		case WM_COMMAND:
			if(LOWORD(wParam) == IDOK || LOWORD(wParam) == IDCANCEL) {
				if(LOWORD(wParam) == IDOK) {
					DebugWindow::Instance->CommitBreakpointChanges(hDlg, bp);
				}
				EndDialog(hDlg, LOWORD(wParam));
				return (INT_PTR)TRUE;
			}
			break;
	}
	return (INT_PTR)FALSE;
}

void DebugWindow::ProcessNotification(ConsoleNotificationType type)
{
	switch(type) {
		case ConsoleNotificationType::CodeBreak:
			if(_debugger) {
				SendMessage(_hWnd, WM_UPDATECPUSTATUS, (WPARAM)&_debugger->GetCPUState(), 0);
			}
			break;
	}
}