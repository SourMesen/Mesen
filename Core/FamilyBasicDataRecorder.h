#pragma once
#include "stdafx.h"
#include <deque>
#include "BaseControlDevice.h"
#include "CPU.h"

//TODO: Integration with UI
class FamilyBasicDataRecorder : public BaseControlDevice
{
private:
	static const uint32_t SamplingRate = 88;
	vector<uint8_t> _saveData;
	bool _enabled = false;
	int32_t _lastCycle = -1;
	int32_t _readStartCycle = -1;

protected:
	void StreamState(bool saving) override
	{
		BaseControlDevice::StreamState(saving);
		Stream(_enabled);
	}

public:
	FamilyBasicDataRecorder() : BaseControlDevice(BaseControlDevice::ExpDevicePort)
	{
	}

	uint8_t ReadRAM(uint16_t addr) override
	{
		if(addr == 0x4016) {
			if(EmulationSettings::CheckFlag(EmulationFlags::ShowFrameCounter)) {
				if(_readStartCycle == -1) {
					_readStartCycle = CPU::GetCycleCount();
				}

				int readPos = (CPU::GetCycleCount() - _readStartCycle) / FamilyBasicDataRecorder::SamplingRate;

				if((int32_t)_saveData.size() > readPos) {
					uint8_t value = (_saveData[readPos] & 0x01) << 1;
					return _enabled ? value : 0;
				}
			} else {
				if(!EmulationSettings::CheckFlag(EmulationFlags::ShowFPS)) {
					_lastCycle = -1;
					_readStartCycle = -1;
				}
			}
		}

		return 0;
	}

	void WriteRAM(uint16_t addr, uint8_t value) override
	{
		_enabled = (value & 0x04) != 0;

		if(EmulationSettings::CheckFlag(EmulationFlags::ShowFPS)) {
			if(_lastCycle == -1) {
				_saveData.clear();
				_lastCycle = CPU::GetCycleCount() - 88;
			}
			while(_lastCycle < CPU::GetCycleCount()) {
				_saveData.push_back(value & 0x01);
				_lastCycle += 88;
			}
		} else {
			if(!EmulationSettings::CheckFlag(EmulationFlags::ShowFrameCounter)) {
				_lastCycle = -1;
				_readStartCycle = -1;
			}
		}
	}
};