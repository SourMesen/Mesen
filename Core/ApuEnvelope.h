#pragma once
#include "stdafx.h"
#include "ApuLengthCounter.h"

class ApuEnvelope : public ApuLengthCounter
{
private:
	bool _constantVolume = false;
	uint8_t _volume = 0;

	uint8_t _envelopeCounter = 0;

	bool _start = false;
	int8_t _divider = 0;
	uint8_t _counter = 0;

protected:
	ApuEnvelope(AudioChannel channel, Blip_Buffer* buffer) : ApuLengthCounter(channel, buffer)
	{
	}

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
	virtual void Reset(bool softReset)
	{
		ApuLengthCounter::Reset(softReset);

		_constantVolume = false;
		_volume = 0;
		_envelopeCounter = 0;
		_start = false;
		_divider = 0;
		_counter = 0;
	}

	virtual void StreamState(bool saving)
	{
		ApuLengthCounter::StreamState(saving);

		Stream<bool>(_constantVolume);
		Stream<uint8_t>(_volume);
		Stream<uint8_t>(_envelopeCounter);
		Stream<bool>(_start);
		Stream<int8_t>(_divider);
		Stream<uint8_t>(_counter);
	}

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
