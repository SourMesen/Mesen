#pragma once
#include "stdafx.h"
#include "Snapshotable.h"
#include "EmulationSettings.h"
#include "APU.h"

class BaseExpansionAudio : public Snapshotable
{
private:
	double _clocksNeeded = 0;

protected: 
	virtual void ClockAudio() = 0;

	void StreamState(bool saving) override
	{
		Stream(_clocksNeeded);
	}

public:
	void Clock()
	{
		if(APU::IsApuEnabled()) {
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
};