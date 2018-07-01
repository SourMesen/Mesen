#pragma once
#include "stdafx.h"
#include "Snapshotable.h"
#include "EmulationSettings.h"

class MemoryManager;

class BaseExpansionAudio : public Snapshotable
{
private:
	double _clocksNeeded = 0;

protected: 
	shared_ptr<Console> _console = nullptr;

	virtual void ClockAudio() = 0;
	void StreamState(bool saving) override;

public:
	BaseExpansionAudio(shared_ptr<Console> console);

	void Clock();
};