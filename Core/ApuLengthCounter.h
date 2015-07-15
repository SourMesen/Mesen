#pragma once
#include "stdafx.h"
#include "BaseApuChannel.h"

class ApuLengthCounter : public BaseApuChannel<15>
{
private:
	const vector<uint8_t> _lcLookupTable = { { 10, 254, 20, 2, 40, 4, 80, 6, 160, 8, 60, 10, 14, 12, 26, 14, 12, 16, 24, 18, 48, 20, 96, 22, 192, 24, 72, 26, 16, 28, 32, 30 } };
	bool _enabled = false;

protected:
	bool _lengthCounterHalt = false;
	uint8_t _lengthCounter = 0;
	
	void InitializeLengthCounter(bool haltFlag)
	{
		_lengthCounterHalt = haltFlag;
	}

	void LoadLengthCounter(uint8_t value)
	{
		if(_enabled) {
			_lengthCounter = _lcLookupTable[value];
		}
	}

public:
	ApuLengthCounter(Blip_Buffer* buffer) : BaseApuChannel(buffer)
	{
	}

	virtual void Reset()
	{
		BaseApuChannel::Reset();

		_enabled = false;
		_lengthCounterHalt = false;
		_lengthCounter = 0;
	}

	virtual void StreamState(bool saving)
	{
		BaseApuChannel::StreamState(saving);

		Stream<bool>(_enabled);
		Stream<bool>(_lengthCounterHalt);
		Stream<uint8_t>(_lengthCounter);
	}

	bool GetStatus()
	{
		return _lengthCounter > 0;
	}

	void TickLengthCounter()
	{
		if(_lengthCounter > 0 && !_lengthCounterHalt) {
			_lengthCounter--;
		}
	}

	void SetEnabled(bool enabled)
	{
		if(!enabled) {
			_lengthCounter = 0;
		}
		_enabled = enabled;
	}
};
