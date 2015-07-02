#include "stdafx.h"
#include "SimpleLock.h"

SimpleLock::SimpleLock()
{
	_lock.clear();
	_lockCount = 0;
	_holderThreadID = ~0;
}

SimpleLock::~SimpleLock()
{
}

void SimpleLock::Acquire()
{
	if(_lockCount == 0 || _holderThreadID != GetCurrentThreadId()) {
		while(_lock.test_and_set());
		_holderThreadID = GetCurrentThreadId();
		_lockCount = 1;
	} else {
		//Same thread can acquire the same lock multiple times
		_lockCount++;
	}
}

bool SimpleLock::IsFree()
{
	if(!_lock.test_and_set()) {
		_lock.clear();
		return true;
	}
	return false;
}

void SimpleLock::WaitForRelease()
{
	//Wait until we are able to grab a lock, and then release it again
	Acquire();
	Release();
}

void SimpleLock::Release()
{
	if(_lockCount > 0 && _holderThreadID == GetCurrentThreadId()) {
		_lockCount--;
		if(_lockCount == 0) {
			_holderThreadID = ~0;
			_lock.clear();
		}
	} else {
		assert(false);
	}
}