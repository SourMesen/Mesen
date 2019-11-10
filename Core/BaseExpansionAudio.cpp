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
}

void BaseExpansionAudio::Clock()
{
	if(_console->GetApu()->IsApuEnabled()) {
		ClockAudio();
	}
}