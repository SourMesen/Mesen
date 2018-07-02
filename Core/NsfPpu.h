#pragma once
#include "stdafx.h"
#include "PPU.h"

class NsfPpu : public PPU
{
protected:
	void DrawPixel() override;
	void SendFrame() override;

public:
	using PPU::PPU;
};