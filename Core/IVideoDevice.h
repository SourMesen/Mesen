#pragma once

#include "stdafx.h"

struct HdPpuPixelInfo;

class IVideoDevice
{
	public:
		virtual void UpdateFrame(void *outputBuffer) = 0;
		virtual void UpdateHdFrame(void *outputBuffer, HdPpuPixelInfo *screenTiles) = 0;
};