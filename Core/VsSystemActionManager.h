#pragma once
#include "stdafx.h"
#include "SystemActionManager.h"

class VsSystemActionManager : public SystemActionManager
{
private:
	static constexpr uint8_t InsertCoinFrameCount = 4;

	uint8_t _needInsertCoin[2] = { 0, 0 };
	bool _needServiceButton = false;

	void ProcessInsertCoin(uint8_t port)
	{
		if(_needInsertCoin[port] > 0) {
			_needInsertCoin[port]--;
			SetBit(VsSystemActionManager::VsButtons::InsertCoin1 + port);
		}
	}

	string GetKeyNames() override
	{
		return "RP12S";
	}

public:
	enum VsButtons { InsertCoin1 = 2, InsertCoin2, ServiceButton };

	VsSystemActionManager(std::shared_ptr<Console> console) : SystemActionManager(console)
	{
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

		if(_needServiceButton) {
			SetBit(VsSystemActionManager::VsButtons::ServiceButton);
		}
	}

	void InsertCoin(uint8_t port)
	{
		_needInsertCoin[port] = VsSystemActionManager::InsertCoinFrameCount;
		MessageManager::DisplayMessage("VS System", "CoinInsertedSlot", std::to_string(port + 1));
	}

	void SetServiceButtonState(bool pressed)
	{
		_needServiceButton = pressed;
	}
};