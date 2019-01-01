#pragma once
#include "stdafx.h"
#include "PPU.h"

class NsfPpu : public PPU
{
protected:
	void DrawPixel() override;

public:
	using PPU::PPU;
};