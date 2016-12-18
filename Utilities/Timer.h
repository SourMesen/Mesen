#pragma once
#include "stdafx.h"

class Timer
{
	private:
		#ifdef WIN32
		double _frequency = 0.0;
		#endif
		uint64_t _start;

	public:
		Timer();
		void Reset();
		double GetElapsedMS();
		void WaitUntil(double targetMillisecond);
};