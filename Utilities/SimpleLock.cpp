#include "stdafx.h"
#include <assert.h>
#include "SimpleLock.h"
#ifdef WIN32
	#include <Windows.h>
#endif

SimpleLock::SimpleLock()
{
	_lock.clear();
	_lockCount = 0;
	_holderThreadID = ~0;
}

SimpleLock::~SimpleLock()
{
}

uint32_t SimpleLock::GetThreadId()
{
#ifdef WIN32
	return GetCurrentThreadId();
#elif
	return std::thread::id;
#endif
}

LockHandler SimpleLock::AcquireSafe()
{
	return LockHandler(this);
}

void SimpleLock::Acquire()
{
	if(_lockCount == 0 || _holderThreadID != GetThreadId()) {
		while(_lock.test_and_set());
		_holderThreadID = GetThreadId();
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
	if(_lockCount > 0 && _holderThreadID == GetThreadId()) {
		_lockCount--;
		if(_lockCount == 0) {
			_holderThreadID = ~0;
			_lock.clear();
		}
	} else {
		assert(false);
	}
}


LockHandler::LockHandler(SimpleLock *lock)
{
	_lock = lock;
	_lock->Acquire();
}

LockHandler::~LockHandler()
{
	_lock->Release();
}