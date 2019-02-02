#pragma once
#include "stdafx.h"
#include "Snapshotable.h"

class BaseEeprom24C0X : public Snapshotable
{
protected:
	enum class Mode
	{
		Idle = 0,
		Address = 1,
		Read = 2,
		Write = 3,
		SendAck = 4,
		WaitAck = 5,
		ChipAddress = 6
	};

	shared_ptr<Console> _console;

	Mode _mode = Mode::Idle;
	Mode _nextMode = Mode::Idle;
	uint8_t _chipAddress = 0;
	uint8_t _address = 0;
	uint8_t _data = 0;
	uint8_t _counter = 0;
	uint8_t _output = 0;
	uint8_t _prevScl = 0;
	uint8_t _prevSda = 0;
	uint8_t _romData[256];

	void StreamState(bool saving) override
	{
		ArrayInfo<uint8_t> romData { _romData, 256 };
		Stream(_mode, _nextMode, _chipAddress, _address, _data, _counter, _output, _prevScl, _prevSda, romData);
	}

public:
	virtual void Write(uint8_t scl, uint8_t sda) = 0;
	virtual void SaveBattery() = 0;
	
	uint8_t Read()
	{
		return _output;
	}

	void WriteScl(uint8_t scl)
	{
		Write(scl, _prevSda);
	}

	void WriteSda(uint8_t sda)
	{
		Write(_prevScl, sda);
	}
};

