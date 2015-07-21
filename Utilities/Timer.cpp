#include "stdafx.h"

#include <Windows.h>
#include "Timer.h"

Timer::Timer() 
{
	LARGE_INTEGER li;
	if(!QueryPerformanceFrequency(&li)) {
		throw;
	}

	_frequency = double(li.QuadPart) / 1000.0;

	QueryPerformanceCounter(&li);
	_start = li.QuadPart;
}

void Timer::Reset()
{
	LARGE_INTEGER li;
	QueryPerformanceCounter(&li);
	_start = li.QuadPart;
}

double Timer::GetElapsedMS()
{
	LARGE_INTEGER li;
	QueryPerformanceCounter(&li);
	return double(li.QuadPart - _start) / _frequency;
}

uint32_t Timer::GetElapsedTicks()
{
	LARGE_INTEGER li;
	QueryPerformanceCounter(&li);
	return uint32_t(li.QuadPart - _start);
}