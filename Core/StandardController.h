#pragma once
#include "stdafx.h"
#include "BaseControlDevice.h"
#include "Zapper.h"

class StandardController : public BaseControlDevice
{
private:
	uint32_t _stateBuffer;
	uint32_t _stateBufferFamicom;

	bool _hasZapper;
	shared_ptr<BaseControlDevice> _additionalController;

protected:
	uint8_t RefreshState();
	virtual void StreamState(bool saving);

public:
	using BaseControlDevice::BaseControlDevice;

	virtual uint8_t GetButtonState();
	uint8_t GetPortOutput();
	void RefreshStateBuffer();

	shared_ptr<Zapper> GetZapper();

	void AddAdditionalController(shared_ptr<BaseControlDevice> controller);
};