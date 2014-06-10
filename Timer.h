#include "stdafx.h"

class Timer
{
	private:
		double _frequency = 0.0;
		LARGE_INTEGER _start;

	public:
		Timer() {
			LARGE_INTEGER li;
			if(!QueryPerformanceFrequency(&li)) {
				throw;
			}

			_frequency = double(li.QuadPart) / 1000.0;

			QueryPerformanceCounter(&li);
			_start = li;
		}

		double GetElapsedMS()
		{
			LARGE_INTEGER li;
			QueryPerformanceCounter(&li);
			return double(li.QuadPart - _start.QuadPart) / _frequency;
		}
};