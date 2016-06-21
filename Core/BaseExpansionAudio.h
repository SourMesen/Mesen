#pragma once
#include "stdafx.h"
#include "Snapshotable.h"
#include "EmulationSettings.h"

class BaseExpansionAudio : public Snapshotable
{
private:
	double _clocksNeeded = 0;

protected: 
	virtual void ClockAudio() = 0;

	void StreamState(bool saving)
	{
		Stream(_clocksNeeded);
	}

public:
	void Clock()
	{
		if(EmulationSettings::GetOverclockRate(true) == 100) {
			ClockAudio();
		} else {
			_clocksNeeded += 1.0 / ((double)EmulationSettings::GetOverclockRate(true) / 100);
			while(_clocksNeeded >= 1.0) {
				ClockAudio();
				_clocksNeeded--;
			}
		}
	}
};