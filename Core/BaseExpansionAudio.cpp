#include "stdafx.h"
#include "BaseExpansionAudio.h"
#include "Console.h"
#include "APU.h"

BaseExpansionAudio::BaseExpansionAudio(shared_ptr<Console> console)
{
	_console = console;
}

void BaseExpansionAudio::StreamState(bool saving)
{
	Stream(_clocksNeeded);
}

void BaseExpansionAudio::Clock()
{
	if(_console->GetApu()->IsApuEnabled()) {
		if(_console->GetSettings()->GetOverclockRate() == 100 || !_console->GetSettings()->GetOverclockAdjustApu()) {
			ClockAudio();
		} else {
			_clocksNeeded += 1.0 / ((double)_console->GetSettings()->GetOverclockRate() / 100);
			while(_clocksNeeded >= 1.0) {
				ClockAudio();
				_clocksNeeded--;
			}
		}
	}
}