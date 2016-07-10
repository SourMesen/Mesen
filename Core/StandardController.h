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
	bool _hasArkanoidController;
	shared_ptr<BaseControlDevice> _additionalController;
	uint8_t GetButtonState();

protected:
	uint8_t RefreshState();
	virtual void StreamState(bool saving);

public:
	using BaseControlDevice::BaseControlDevice;

	uint32_t GetNetPlayState();

	uint8_t GetPortOutput();
	void RefreshStateBuffer();

	//Used for VS System button unscrambling
	uint32_t GetInternalState();
	void SetInternalState(uint32_t state);

	void AddAdditionalController(shared_ptr<BaseControlDevice> controller);
};