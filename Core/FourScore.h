#pragma once
#include "BaseControlDevice.h"

class FourScore : public BaseControlDevice
{
private:
	uint32_t _signature4016 = 0;
	uint32_t _signature4017 = 0;

protected:
	void StreamState(bool saving) override
	{
		BaseControlDevice::StreamState(saving);
		Stream(_signature4016, _signature4017);
	}

	void RefreshStateBuffer() override
	{
		//Signature for port 0 = 0x10, reversed bit order => 0x08
		//Signature for port 1 = 0x20, reversed bit order => 0x04
		_signature4016 = (0x08 << 16);
		_signature4017 = (0x04 << 16);
	}

public:
	FourScore(shared_ptr<Console> console) : BaseControlDevice(console, BaseControlDevice::ExpDevicePort)
	{
	}
	
	uint8_t ReadRAM(uint16_t addr) override
	{
		StrobeProcessRead();
		uint8_t output = 0;
		if(addr == 0x4016) {
			output = _signature4016 & 0x01;
			_signature4016 >>= 1;
		} else if(addr == 0x4017) {
			output = _signature4017 & 0x01;
			_signature4017 >>= 1;
		}
		return output;
	}

	void WriteRAM(uint16_t addr, uint8_t value) override
	{
		StrobeProcessWrite(value);
	}
};
