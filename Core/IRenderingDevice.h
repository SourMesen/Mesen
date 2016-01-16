#pragma once

#include "stdafx.h"

class IRenderingDevice
{
	public:
		virtual void UpdateFrame(void *frameBuffer, uint32_t width, uint32_t height) = 0;
		virtual void Render() = 0;
		virtual void Reset() = 0;
};