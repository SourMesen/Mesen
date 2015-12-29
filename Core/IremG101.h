#pragma once
#include "stdafx.h"
#include "BaseMapper.h"

class IremG101 : public BaseMapper
{
	protected:
		virtual uint16_t GetPRGPageSize() { return 0x2000; }
		virtual uint16_t GetCHRPageSize() {	return 0x0400; }

		uint8_t _prgRegs[2];
		uint8_t _prgMode;

		void InitMapper() 
		{
			_prgRegs[0] = _prgRegs[1] = 0;
			_prgMode = 0;

			SelectPRGPage(2, -2);
			SelectPRGPage(3, -1);
		}

		virtual void StreamState(bool saving)
		{
			StreamArray<uint8_t>(_prgRegs, 2);
			Stream<uint8_t>(_prgMode);
		}

		void UpdatePrgMode()
		{
			if(_prgMode == 0) {
				SelectPRGPage(0, _prgRegs[0]);
				SelectPRGPage(1, _prgRegs[1]);
				SelectPRGPage(2, -2);
				SelectPRGPage(3, -1);
			} else {
				SelectPRGPage(0, -2);
				SelectPRGPage(1, _prgRegs[1]);
				SelectPRGPage(2, _prgRegs[0]);
				SelectPRGPage(3, -1);
			}
		}

		void WriteRegister(uint16_t addr, uint8_t value)
		{
			switch(addr & 0xF000) {
				case 0x8000:
					_prgRegs[0] = value & 0x1F;
					SelectPRGPage(_prgMode == 0 ? 0 : 2, _prgRegs[0]);
					break;
				case 0x9000:
					_prgMode = (value & 0x02) >> 1;
					UpdatePrgMode();
					SetMirroringType((value & 0x01) == 0x01 ? MirroringType::Horizontal : MirroringType::Vertical);
					break;
				case 0xA000:
					_prgRegs[1] = value & 0x1F;
					SelectPRGPage(1, _prgRegs[1]);
					break;
				case 0xB000:
					SelectCHRPage(addr & 0x07, value);
					break;
			}
		}
};