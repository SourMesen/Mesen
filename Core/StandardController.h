#pragma once

#include "stdafx.h"

#include "IControlDevice.h"

struct KeyMapping
{
	uint32_t A;
	uint32_t B;
	uint32_t Up;
	uint32_t Down;
	uint32_t Left;
	uint32_t Right;
	uint32_t Start;
	uint32_t Select;
	uint32_t TurboA;
	uint32_t TurboB;
	uint32_t TurboStart;
	uint32_t TurboSelect;
	uint32_t TurboSpeed;
};

class StandardController : public IControlDevice
{
private:
	vector<KeyMapping> _keyMappings;

public:
	StandardController(uint8_t port);
	~StandardController();

	void AddKeyMappings(KeyMapping keyMapping);
	void ClearKeyMappings();

	ButtonState GetButtonState();
};