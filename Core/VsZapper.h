#pragma once
#include "stdafx.h"
#include "Zapper.h"

class VsZapper : public Zapper
{
private:
	uint32_t _stateBuffer;

protected:
	uint8_t RefreshState();
	void RefreshStateBuffer();

public:
	using Zapper::Zapper;

	uint8_t GetPortOutput();
};