#pragma once
#include "stdafx.h"
#include "BaseFdsChannel.h"

class ModChannel : public BaseFdsChannel
{
private:
	const int32_t ModReset = 0xFF;
	const int32_t _modLut[8] = { 0,1,2,4,ModReset,-4,-2,-1 };

	int8_t _counter = 0;
	bool _modulationDisabled = false;

	uint8_t _modTable[64];
	uint8_t _modTablePosition = 0;
	uint16_t _overflowCounter = 0;

protected:
	void StreamState(bool saving)
	{
		BaseFdsChannel::StreamState(saving);

		Stream<int8_t>(_counter);
		Stream<bool>(_modulationDisabled);
		Stream<uint8_t>(_modTablePosition);
		Stream<uint16_t>(_overflowCounter);

		StreamArray<uint8_t>(_modTable, 64);
	}

public:
	virtual void WriteReg(uint16_t addr, uint8_t value)
	{
		switch(addr & 0x03) {
			case 1: UpdateCounter(value & 0x7F); break;
			case 3: _modulationDisabled = (value & 0x80) == 0x80; break;
		}
		BaseFdsChannel::WriteReg(addr, value);
	}

	void WriteModTable(uint8_t value)
	{
		//"This register has no effect unless the mod unit is disabled via the high bit of $4087."
		if(_modulationDisabled) {
			_modTable[_modTablePosition & 0x3F] = value & 0x07;
			_modTable[(_modTablePosition + 1) & 0x3F] = value & 0x07;
			_modTablePosition = (_modTablePosition + 2) & 0x3F;
		}
	}

	void UpdateCounter(int8_t value)
	{
		_counter = value;
		if(_counter >= 64) {
			_counter -= 128;
		} else if(_counter < -64) {
			_counter += 128;
		}
	}

	bool IsEnabled()
	{
		return !_modulationDisabled && _frequency > 0;
	}

	bool TickModulator()
	{
		_overflowCounter += _frequency;

		if(_overflowCounter < _frequency) {
			//Overflowed, tick the modulator
			int32_t offset = _modLut[_modTable[_modTablePosition]];
			UpdateCounter(offset == ModReset ? 0 : _counter + offset);

			_modTablePosition = (_modTablePosition + 1) & 0x3F;

			return true;
		}

		return false;
	}

	int32_t GetWavePitch(uint16_t volumePitch)
	{
		//Code from NesDev Wiki

		// pitch   = $4082/4083 (12-bit unsigned pitch value)
		// counter = $4085 (7-bit signed mod counter)
		// gain    = $4084 (6-bit unsigned mod gain)

		// 1. multiply counter by gain, lose lowest 4 bits of result but "round" in a strange way
		int32_t temp = _counter * _gain;
		int32_t remainder = temp & 0xF;
		temp >>= 4;
		if(remainder > 0 && (temp & 0x80) == 0) {
			temp += _counter < 0 ? -1 : 2;
		}

		// 2. wrap if a certain range is exceeded
		if(temp >= 192) {
			temp -= 256;
		} else if(temp < -64) {
			temp += 256;
		}

		// 3. multiply result by pitch, then round to nearest while dropping 6 bits
		temp = volumePitch * temp;
		remainder = temp & 0x3F;
		temp >>= 6;
		if(remainder >= 32) {
			temp += 1;
		}

		// final mod result is in temp
		return temp;
	}
};
