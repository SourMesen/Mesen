#pragma once

#include "stdafx.h"

class IRenderingDevice
{
	public:
		virtual void UpdateFrame(void *frameBuffer) = 0;
		virtual void Render() = 0;
};