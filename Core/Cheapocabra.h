#pragma once
#include "stdafx.h"
#include "BaseMapper.h"

//Missing Flash rom support, and only tested via a test rom
class Cheapocabra : public BaseMapper
{
private:
	uint8_t _reg;
	uint8_t* _extraNametables[4];

protected:
	virtual uint16_t GetPRGPageSize() override { return 0x8000; }
	virtual uint16_t GetCHRPageSize() override { return 0x2000; }
	virtual uint16_t RegisterStartAddress() override { return 0x5000; }
	virtual uint16_t RegisterEndAddress() override { return 0x5FFF; }
	virtual uint32_t GetChrRamSize() override { return 0x8000; }

	void InitMapper() override
	{
		AddRegisterRange(0x7000, 0x7FFF, MemoryOperation::Write);
		for(int i = 0; i < 4; i++) {
			_extraNametables[i] = new uint8_t[0x400];
			BaseMapper::InitializeRam(_extraNametables[i], 0x400);
			AddNametable(4 + i, _extraNametables[i]);
		}
		_reg = GetPowerOnByte();
		UpdateState();
	}

	virtual ~Cheapocabra()
	{
		delete[] _extraNametables[0];
		delete[] _extraNametables[1];
		delete[] _extraNametables[2];
		delete[] _extraNametables[3];
	}

	void StreamState(bool saving) override
	{
		BaseMapper::StreamState(saving);
		ArrayInfo<uint8_t> extraNametable0{ _extraNametables[0], 0x400 };
		ArrayInfo<uint8_t> extraNametable1{ _extraNametables[1], 0x400 };
		ArrayInfo<uint8_t> extraNametable2{ _extraNametables[2], 0x400 };
		ArrayInfo<uint8_t> extraNametable3{ _extraNametables[3], 0x400 };
		Stream(_reg, extraNametable0, extraNametable1, extraNametable2, extraNametable3);
	}

	void UpdateState()
	{
		SelectPRGPage(0, _reg & 0x0F);
		SelectCHRPage(0, (_reg >> 4) & 0x01);
		if(_reg & 0x20) {
			SetNametables(4, 5, 6, 7);
		} else {
			SetNametables(0, 1, 2, 3);
		}
	}

	void WriteRegister(uint16_t addr, uint8_t value) override
	{
		_reg = value;
		UpdateState();
	}
};