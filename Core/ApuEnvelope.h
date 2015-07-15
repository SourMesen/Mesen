#pragma once
#include "stdafx.h"
#include "ApuLengthCounter.h"

class ApuEnvelope : public ApuLengthCounter
{
private:
	bool _constantVolume = false;
	uint8_t _volume = 0;

	uint8_t _envelope = 0;
	uint8_t _envelopeCounter = 0;

	bool _start = false;
	int8_t _divider = 0;
	uint8_t _counter = 0;

protected:
	void InitializeEnvelope(uint8_t regValue)
	{
		_constantVolume = (regValue & 0x10) == 0x10;
		_volume = regValue & 0x0F;
	}

	void ResetEnvelope()
	{
		_start = true;
	}
	
	uint32_t GetVolume()
	{
		if(_lengthCounter > 0) {
			if(_constantVolume) {
				return _volume;
			} else {
				return _counter;
			}
		} else {
			return 0;
		}
	}

public:
	void TickEnvelope()
	{
		if(!_start) {
			_divider--;
			if(_divider < 0) {
				_divider = _volume;
				if(_counter > 0) {
					_counter--;
				} else if(_lengthCounterHalt) {
					_counter = 15;
				}
			}
		} else {
			_start = false;
			_counter = 15;
			_divider = _volume;
		}
	}
};
