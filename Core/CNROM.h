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
		SelectCHRPage(0, 0);
	}
	
	bool HasBusConflicts() override { return (_mapperID == 3 && _subMapperID == 2) || _mapperID == 185; }

	void WriteRegister(uint16_t addr, uint8_t value) override
	{
		if(_enableCopyProtection) {
			//"if C AND $0F is nonzero, and if C does not equal $13: CHR is enabled"
			//Seicross (mapper 185 version) is assigned to submapper 16 (not a real submapper) to make it work properly
			if((_subMapperID == 16 && !(value & 0x01)) || (_subMapperID == 0 && (value & 0x0F) != 0 && value != 0x13)) {
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