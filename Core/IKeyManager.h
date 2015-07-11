#pragma once

#include "stdafx.h"

class IKeyManager
{
public:
	virtual void RefreshState() = 0;
	virtual bool IsKeyPressed(uint32_t keyCode) = 0;
	virtual uint32_t GetPressedKey() = 0;
	virtual string GetKeyName(uint32_t keyCode) = 0;
	virtual uint32_t GetKeyCode(string keyName) = 0;
};