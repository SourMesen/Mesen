#pragma once
#include "stdafx.h"
#include "BaseMapper.h"
#include "ControlManager.h"
#include "StandardController.h"
#include "BandaiMicrophone.h"

class BandaiKaraoke : public BaseMapper
{
protected:
	uint16_t GetPRGPageSize() override { return 0x4000; }
	uint16_t GetCHRPageSize() override { return 0x2000; }
	bool AllowRegisterRead() override { return true; }
	bool HasBusConflicts() override { return true; }
	ConsoleFeatures GetAvailableFeatures() { return ConsoleFeatures::BandaiMicrophone; }

	void InitMapper() override
	{
		AddRegisterRange(0x6000, 0x7FFF, MemoryOperation::Read);
		RemoveRegisterRange(0x8000, 0xFFFF, MemoryOperation::Read);

		SelectPRGPage(0, 0);
		SelectPRGPage(1, 0x07);
		SelectCHRPage(0, 0);

		_mapperControlDevice.reset(new BandaiMicrophone(EmulationSettings::GetControllerKeys(0)));
	}

	uint8_t ReadRegister(uint16_t addr) override
	{
		return _mapperControlDevice->ReadRAM(addr) | MemoryManager::GetOpenBus(0xF8);
	}

	void WriteRegister(uint16_t addr, uint8_t value) override
	{
		if(value & 0x10) {
			//Select internal rom
			SelectPRGPage(0, value & 0x07);
		} else {
			//Select expansion rom
			if(_prgSize >= 0x40000) {
				SelectPRGPage(0, (value & 0x07) | 0x08);
			} else {
				//Open bus for roms that don't contain the expansion rom
				RemoveCpuMemoryMapping(0x8000, 0xBFFF);
			}
		}

		SetMirroringType(value & 0x20 ? MirroringType::Horizontal : MirroringType::Vertical);
	}
};