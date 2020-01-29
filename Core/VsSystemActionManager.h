#pragma once
#include "stdafx.h"
#include "SystemActionManager.h"
#include "Console.h"

class VsSystemActionManager : public SystemActionManager
{
private:
	static constexpr uint8_t InsertCoinFrameCount = 4;

	bool _isDualSystem = false;
	uint8_t _needInsertCoin[4] = { 0, 0, 0, 0 };
	bool _needServiceButton[2] = { false, false };

	void ProcessInsertCoin(uint8_t port)
	{
		if(_needInsertCoin[port] > 0) {
			_needInsertCoin[port]--;

			switch(port) {
				case 0: SetBit(VsSystemActionManager::VsButtons::InsertCoin1); break;
				case 1: SetBit(VsSystemActionManager::VsButtons::InsertCoin2); break;
				case 2: SetBit(VsSystemActionManager::VsButtons::InsertCoin3); break;
				case 3: SetBit(VsSystemActionManager::VsButtons::InsertCoin4); break;
			}			
		}
	}

	string GetKeyNames() override
	{
		return _isDualSystem ? "RP12S34ST" : "RP12S";
	}

public:
	enum VsButtons { InsertCoin1 = 2, InsertCoin2, ServiceButton, InsertCoin3, InsertCoin4, ServiceButton2 };

	VsSystemActionManager(std::shared_ptr<Console> console) : SystemActionManager(console)
	{
		_isDualSystem = console->IsDualSystem();
	}

	uint8_t ReadRAM(uint16_t addr) override
	{
		uint8_t value = 0;
		if(addr == 0x4016) {
			if(IsPressed(VsSystemActionManager::VsButtons::InsertCoin1)) {
				value |= 0x20;
			}
			if(IsPressed(VsSystemActionManager::VsButtons::InsertCoin2)) {
				value |= 0x40;
			}
			if(IsPressed(VsSystemActionManager::VsButtons::ServiceButton)) {
				value |= 0x04;
			}
		}
		return value;
	}

	void OnAfterSetState() override
	{
		SystemActionManager::OnAfterSetState();

		ProcessInsertCoin(0);
		ProcessInsertCoin(1);
		ProcessInsertCoin(2);
		ProcessInsertCoin(3);

		if(_needServiceButton[0]) {
			SetBit(VsSystemActionManager::VsButtons::ServiceButton);
		}
		if(_needServiceButton[1]) {
			SetBit(VsSystemActionManager::VsButtons::ServiceButton2);
		}
	}

	void InsertCoin(uint8_t port)
	{
		if(port < 4) {
			_console->Pause();
			_needInsertCoin[port] = VsSystemActionManager::InsertCoinFrameCount;
			MessageManager::DisplayMessage("VS System", "CoinInsertedSlot", std::to_string(port + 1));
			_console->Resume();
		}
	}

	void SetServiceButtonState(int consoleId, bool pressed)
	{
		_console->Pause();
		_needServiceButton[consoleId] = pressed;
		_console->Resume();
	}
};