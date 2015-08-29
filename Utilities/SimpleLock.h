#pragma once 
#include "stdafx.h"

class SimpleLock
{
private:
	uint32_t _holderThreadID;
	uint32_t _lockCount;
	atomic_flag _lock;

	uint32_t GetThreadId();

public:
	SimpleLock();
	~SimpleLock();
	void Acquire();
	bool IsFree();
	void WaitForRelease();
	void Release();
};

