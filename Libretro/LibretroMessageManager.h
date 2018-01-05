#pragma once
#include "libretro.h"
#include "../Core/IMessageManager.h"
#include "../Core/MessageManager.h"

class LibretroMessageManager : public IMessageManager
{
private:
	retro_log_printf_t _log = nullptr;

public:
	LibretroMessageManager(retro_log_printf_t logCallback)
	{
		_log = logCallback;
		MessageManager::RegisterMessageManager(this);
	}

	~LibretroMessageManager()
	{
		MessageManager::RegisterMessageManager(nullptr);
	}

	// Inherited via IMessageManager
	virtual void DisplayMessage(string title, string message) override
	{
		if(_log) {
			_log(RETRO_LOG_INFO, message.c_str());
		}
	}
};