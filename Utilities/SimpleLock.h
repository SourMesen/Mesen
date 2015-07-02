#pragma once 
#include "stdafx.h"

class SimpleLock
{
private:
	DWORD _holderThreadID;
	uint32_t _lockCount;
	atomic_flag _lock;

public:
	SimpleLock();
	~SimpleLock();
	void Acquire();
	bool IsFree();
	void WaitForRelease();
	void Release();
};

