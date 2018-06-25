#pragma once
#include "IMemoryHandler.h"

class IMemoryManager
{
public:
	virtual void RegisterIODevice(IMemoryHandler *handler) = 0;
	virtual void UnregisterIODevice(IMemoryHandler *handler) = 0;
};