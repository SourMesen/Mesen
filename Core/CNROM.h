#pragma once
#include "stdafx.h"
#include "BaseMapper.h"

class CNROM : public BaseMapper
{
private:
	bool _enableCopyProtection;

protected:
	virtual uint16_t GetPRGPageSize() { return 0x8000; }
	virtual uint16_t GetCHRPageSize() { return 0x2000; }

	void InitMapper()
	{
		SelectPRGPage(0, 0);
		SelectCHRPage(0, 0);
	}
	
	bool HasBusConflicts() { return _mapperID == 3 && _subMapperID == 2; }

	void WriteRegister(uint16_t addr, uint8_t value)
	{
		if(_enableCopyProtection) {
			//"if C AND $0F is nonzero, and if C does not equal $13: CHR is enabled"
			//This means Seicross (mapper 185 version) will not work (to allow Spy vs Spy (J) to work
			if((value & 0x0F) != 0 && value != 0x13) {
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