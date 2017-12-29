#pragma once
#include "stdafx.h"
#include "BaseControlDevice.h"
#include "Console.h"

class SystemActionManager : public BaseControlDevice
{
private:
	std::weak_ptr<Console> _console;
	bool _needReset = false;
	bool _needPowerCycle = false;

protected:

	string GetKeyNames() override
	{
		return "RP";
	}

	void StreamState(bool saving) override
	{
		BaseControlDevice::StreamState(saving);
	}

public:
	enum Buttons { ResetButton = 0, PowerButton = 1 };

	SystemActionManager(std::shared_ptr<Console> console) : BaseControlDevice(BaseControlDevice::ConsoleInputPort)
	{
		_console = console;
	}

	uint8_t ReadRAM(uint16_t addr) override
	{
		return 0;
	}

	void WriteRAM(uint16_t addr, uint8_t value) override
	{
	}

	void OnAfterSetState() override
	{
		if(_needReset) {
			SetBit(SystemActionManager::Buttons::ResetButton);
			_needReset = false;
		}
		if(_needPowerCycle) {
			SetBit(SystemActionManager::Buttons::PowerButton);
			_needPowerCycle = false;
		}		
	}

	void Reset()
	{
		_needReset = true;
	}

	void PowerCycle()
	{
		_needPowerCycle = true;
	}

	void ProcessSystemActions()
	{
		shared_ptr<Console> console = _console.lock();
		if(console) {
			if(IsPressed(SystemActionManager::Buttons::ResetButton)) {
				console->ResetComponents(true);
			}
			if(IsPressed(SystemActionManager::Buttons::PowerButton)) {
				console->PowerCycle();
				//Calling PowerCycle() causes this object to be deleted - no code must be written below this line
			}
		}
	}
};
