#pragma once

#include "stdafx.h"

class IKeyManager
{
public:
	virtual void RefreshState() = 0;
	virtual bool IsKeyPressed(uint32_t keyCode) = 0;
	virtual uint32_t GetPressedKey() = 0;
	virtual wchar_t* GetKeyName(uint32_t keyCode) = 0;
	virtual uint32_t GetKeyCode(wchar_t* keyName) = 0;
};