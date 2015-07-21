#pragma once

#include "stdafx.h"

class IVideoDevice
{
	public:
		virtual void UpdateFrame(void *outputBuffer) = 0;
};