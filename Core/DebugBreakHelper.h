#pragma once
#include "stdafx.h"
#include "Debugger.h"
#include "Console.h"

class DebugBreakHelper
{
private:
	Debugger* _debugger;
	bool _needResume = false;
	bool _isEmulationThread = false;

public:
	DebugBreakHelper(Debugger* debugger)
	{
		_debugger = debugger;

		_isEmulationThread = debugger->GetConsole()->GetEmulationThreadId() == std::this_thread::get_id();

		if(!_isEmulationThread) {
			//Only attempt to break if this is done in a thread other than the main emulation thread
			debugger->PreventResume();
			if(!debugger->IsExecutionStopped()) {
				debugger->Break();
				while(!debugger->IsExecutionStopped()) {}
				_needResume = true;
			}
		}
	}

	~DebugBreakHelper()
	{
		if(!_isEmulationThread) {
			if(_needResume) {
				_debugger->ResumeFromBreak();
			}
			_debugger->AllowResume();
		}
	}
};