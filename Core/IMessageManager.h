#pragma once

#include "stdafx.h"

class IMessageManager
{
public:
	virtual void DisplayMessage(wstring message) = 0;
};