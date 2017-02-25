#pragma once
#include "stdafx.h"
#include "BaseControlDevice.h"
#include "Zapper.h"

class StandardController : public BaseControlDevice
{
private:
	bool _isEmptyPort = false;
	uint32_t _stateBuffer = 0;
	uint32_t _stateBufferFamicom = 0;
	uint8_t _lastButtonState = 0;

	shared_ptr<BaseControlDevice> _additionalController;
	uint8_t GetButtonState();

protected:
	uint8_t RefreshState() override;
	virtual void StreamState(bool saving) override;

public:
	StandardController(uint8_t port, bool emptyPort = false);

	uint32_t GetNetPlayState() override;

	uint8_t GetPortOutput() override;
	void RefreshStateBuffer() override;

	//Used for VS System button unscrambling
	uint32_t GetInternalState();
	void SetInternalState(uint32_t state);

	void AddAdditionalController(shared_ptr<BaseControlDevice> controller);
	shared_ptr<BaseControlDevice> GetAdditionalController();

	uint8_t GetLastButtonState();
};