#pragma once
#include "stdafx.h"
#include "BaseMapper.h"

class CNROM : public BaseMapper
{
private:
	bool _enableCopyProtection;

protected:
	virtual uint16_t GetPRGPageSize() override { return 0x8000; }
	virtual uint16_t GetCHRPageSize() override { return 0x2000; }

	void InitMapper() override
	{
		SelectPRGPage(0, 0);
		SelectCHRPage(0, GetPowerOnByte());

		//Needed for mighty bomb jack (j)
		_vramOpenBusValue = 0xFF;
	}
	
	bool HasBusConflicts() override { return (_romInfo.MapperID == 3 && _romInfo.SubMapperID == 2) || _romInfo.MapperID == 185; }

	void WriteRegister(uint16_t addr, uint8_t value) override
	{
		if(_enableCopyProtection) {
			//"if C AND $0F is nonzero, and if C does not equal $13: CHR is enabled"
			//Seicross (mapper 185 version) is assigned to submapper 16 (not a real submapper) to make it work properly
			if((_romInfo.SubMapperID == 16 && !(value & 0x01)) || (_romInfo.SubMapperID == 0 && (value & 0x0F) != 0 && value != 0x13)) {
				SelectCHRPage(0, 0);
			} else {
				RemovePpuMemoryMapping(0x0000, 0x1FFF);
			}
		} else {
			SelectCHRPage(0, value);
		}
	}

public:
	CNROM(bool enableCopyProtection) : _enableCopyProtection(enableCopyProtection)
	{
	}
};