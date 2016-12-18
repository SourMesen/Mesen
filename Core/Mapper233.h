#pragma once
#include "stdafx.h"
#include "Mapper226.h"

class Mapper233 : public Mapper226
{
private:
	uint8_t _reset;

protected:
	void Reset(bool softReset) override
	{
		Mapper226::Reset(softReset);

		if(softReset) {
			_reset = _reset ^ 0x01;
			UpdatePrg();
		} else {
			_reset = 0;
		}
	}

	void StreamState(bool saving) override
	{
		Mapper226::StreamState(saving);
		Stream(_reset);
	}

	uint8_t GetPrgPage() override
	{
		return (_registers[0] & 0x1F) | (_reset << 5) | ((_registers[1] & 0x01) << 6);
	}

};
