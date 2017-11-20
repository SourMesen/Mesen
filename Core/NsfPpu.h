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
	using PPU::PPU;
};