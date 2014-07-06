#pragma once

#include "stdafx.h"

class IGameBroadcaster
{
public:
	virtual void BroadcastInput(uint8_t inputData, uint8_t port) = 0;
};