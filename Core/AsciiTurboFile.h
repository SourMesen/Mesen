#pragma once
#include "stdafx.h"
#include "../Utilities/FolderUtilities.h"
#include "Console.h"
#include "BaseControlDevice.h"
#include "IBattery.h"
#include "BatteryManager.h"

class AsciiTurboFile : public BaseControlDevice, public IBattery
{
private:
	static constexpr int FileSize = 0x2000;
	static constexpr int BitCount = FileSize * 8;
	uint8_t _lastWrite = 0;
	uint16_t _position = 0;
	uint8_t _data[AsciiTurboFile::FileSize];

protected:
	void StreamState(bool saving) override
	{
		BaseControlDevice::StreamState(saving);
		ArrayInfo<uint8_t> data{ _data, AsciiTurboFile::FileSize };
		Stream(_position, _lastWrite, data);
	}

public:
	AsciiTurboFile(shared_ptr<Console> console) : BaseControlDevice(console, BaseControlDevice::ExpDevicePort)
	{
		BatteryManager::LoadBattery(".tf", _data, AsciiTurboFile::FileSize);
	}

	~AsciiTurboFile()
	{
		SaveBattery();
	}

	void SaveBattery() override
	{
		BatteryManager::SaveBattery(".tf", _data, AsciiTurboFile::FileSize);
	}

	uint8_t ReadRAM(uint16_t addr) override
	{
		if(addr == 0x4017) {
			return ((_data[_position / 8] >> (_position % 8)) & 0x01) << 2;
		}
		return 0;
	}

	void WriteRAM(uint16_t addr, uint8_t value) override
	{
		if(!(value & 0x02)) {
			_position = 0;
		}

		if(!(value & 0x04) && (_lastWrite & 0x04)) {
			//Clock, perform write, increase position
			_data[_position / 8] &= ~(1 << (_position % 8));
			_data[_position / 8] |= (value & 0x01) << (_position % 8);
			_position = (_position + 1) & (AsciiTurboFile::BitCount - 1);
		}

		_lastWrite = value;
	}
};