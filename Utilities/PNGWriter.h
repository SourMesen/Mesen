#pragma once
#include "stdafx.h"

class PNGWriter
{
public:
	static bool WritePNG(string filename, uint8_t* buffer, uint32_t xSize, uint32_t ySize, uint32_t bitsPerPixel = 32);
};