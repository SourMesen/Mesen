#pragma once

#include "stdafx.h"

class IAudioDevice
{
	public:
		virtual void PlayBuffer(int16_t *soundBuffer, uint32_t bufferSize) = 0;
};