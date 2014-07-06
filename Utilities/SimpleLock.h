#pragma once 
#include "stdafx.h"

class SimpleLock
{
private:
	atomic_flag _lock;

public:
	void Acquire()
	{
		while(_lock.test_and_set());
	}

	bool IsFree()
	{
		if(!_lock.test_and_set()) {
			_lock.clear();
			return true;
		}
		return false;
	}

	void WaitForRelease()
	{
		//Wait until we are able to grab a lock, and then release it again
		Acquire();
		Release();
	}

	void Release()
	{
		_lock.clear();
	}
};