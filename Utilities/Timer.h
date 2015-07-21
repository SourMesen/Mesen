#pragma once
#include "stdafx.h"

class Timer
{
	private:
		double _frequency = 0.0;
		uint64_t _start;

	public:
		Timer();
		void Reset();
		double GetElapsedMS();
		uint32_t Timer::GetElapsedTicks();
};