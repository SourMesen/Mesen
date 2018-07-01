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
		if(EmulationSettings::GetOverclockRate() == 100 || !EmulationSettings::GetOverclockAdjustApu()) {
			ClockAudio();
		} else {
			_clocksNeeded += 1.0 / ((double)EmulationSettings::GetOverclockRate() / 100);
			while(_clocksNeeded >= 1.0) {
				ClockAudio();
				_clocksNeeded--;
			}
		}
	}
}