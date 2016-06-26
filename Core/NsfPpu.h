#pragma once
#include "stdafx.h"
#include "PPU.h"

class NsfPpu : public PPU
{
protected:
	void DrawPixel()
	{
	}

	void SendFrame()
	{
		MessageManager::SendNotification(ConsoleNotificationType::PpuFrameDone);
	}

public:
	NsfPpu(MemoryManager* memoryManager) : PPU(memoryManager)
	{
		_simpleMode = true;
	}
};