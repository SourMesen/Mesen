#pragma once

#include "stdafx.h"

class IVideoDevice
{
	public:
		virtual void UpdateFrame(uint8_t *outputBuffer) = 0;
};