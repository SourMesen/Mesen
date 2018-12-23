#pragma once
#include "stdafx.h"
#include "DebugBreakHelper.h"
#include "Console.h"

class ConsolePauseHelper
{
private:
	unique_ptr<DebugBreakHelper> _breakHelper;
	shared_ptr<Debugger> _debugger;
	Console* _console;

public:
	ConsolePauseHelper(Console* console)
	{
		_console = console;
		_debugger = console->GetDebugger(false);
		if(_debugger) {
			//Pause using the debugger (on the next instruction), when possible
			_breakHelper.reset(new DebugBreakHelper(_debugger.get()));
		} else {
			//If the debugger isn't active, pause at the end of the next frame
			console->Pause();
		}
	}

	~ConsolePauseHelper()
	{
		if(!_debugger) {
			_console->Resume();
		}
	}
};