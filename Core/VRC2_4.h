#pragma once
#include "stdafx.h"
#include "BaseMapper.h"

enum class VRCVariant
{
	VRC2a,	//Mapper 22
	VRC2b,	//23
	VRC2c,	//25
	VRC4a,	//21
	VRC4b,	//25
	VRC4c,	//21
	VRC4d,	//25
	VRC4e,	//23
	VRC4_27, //27
};

class VRC2_4 : public BaseMapper
{
	private:
		VRCVariant _variant;

		uint8_t _prgReg0;
		uint8_t _prgReg1;
		uint8_t _prgMode;

		uint8_t _hiCHRRegs[8];
		uint8_t _loCHRRegs[8];

		bool _hasIRQ;

	protected:
		virtual uint32_t GetPRGPageSize() { return 0x2000; }
		virtual uint32_t GetCHRPageSize() {	return 0x0400; }

		void InitMapper() 
		{
			_prgMode = 0;
			_prgReg0 = 0;
			_prgReg1 = 0;
			_hasIRQ = 0;
			memset(_loCHRRegs, 0, sizeof(_loCHRRegs));
			memset(_hiCHRRegs, 0, sizeof(_hiCHRRegs));

			UpdateState();
		}

		void UpdateState()
		{
			for(int i = 0; i < 8; i++) {
				uint32_t page = _loCHRRegs[i] | (_hiCHRRegs[i] << 4);
				if(_variant == VRCVariant::VRC2a) {
					//"On VRC2a (mapper 022) only the high 7 bits of the CHR regs are used -- the low bit is ignored.  Therefore, you effectively have to right-shift the CHR page by 1 to get the actual page number."
					page >>= 1;
				}
				SelectCHRPage(i, page);
			}

			if(_prgMode == 0) {
				SelectPRGPage(0, _prgReg0);
				SelectPRGPage(1, _prgReg1);
				SelectPRGPage(2, -2);
				SelectPRGPage(3, -1);
			} else {
				SelectPRGPage(0, -2);
				SelectPRGPage(1, _prgReg1);
				SelectPRGPage(2, _prgReg0);
				SelectPRGPage(3, -1);
			}
		}

		void WriteRegister(uint16_t addr, uint8_t value)
		{
			addr = TranslateAddress(addr);
			if(addr >= 0x8000 && addr <= 0x8006) {
				_prgReg0 = value & 0x1F;
			} else if(addr == 0x9000 || addr == 0x9002) {
				switch(value & 0x03) {
					case 0: _mirroringType = MirroringType::Vertical; break;
					case 1: _mirroringType = MirroringType::Horizontal; break;
					case 2: _mirroringType = MirroringType::ScreenAOnly; break;
					case 3: _mirroringType = MirroringType::ScreenBOnly; break;
				}
			} else if(addr == 0x9004 || addr == 0x9006) {
				if(_variant >= VRCVariant::VRC4a) {
					_prgMode = (value >> 1) & 0x01;
				}
			} else if(addr >= 0xA000 && addr <= 0xA006) {
				_prgReg1 = value & 0x1F;
			} else if(addr >= 0xB000 && addr <= 0xE006) {
				uint8_t regNumber = ((((addr >> 12) & 0x07) - 3) << 1) + ((addr >> 1) & 0x01);
				bool lowBits = (addr & 0x01) == 0x00;
				if(lowBits) {
					//The other reg contains the low 4 bits
					_loCHRRegs[regNumber] = value & 0x0F;
				} else {
					//One reg contains the high 5 bits 
					_hiCHRRegs[regNumber] = value & 0x1F;
				}
			} else if(addr == 0xF000 || addr == 0xF001) {
				//IRQ Reload Value
				_hasIRQ = true;
			} else if(addr == 0xF002) {
				//IRQ Control
				_hasIRQ = true;
			} else if(addr == 0xF003) {
				//IRQ Acknowledge
				_hasIRQ = true;
			}

			UpdateState();
		}

	public:		
		VRC2_4(VRCVariant variant)
		{
			_variant = variant;
		}

		uint16_t TranslateAddress(uint16_t addr)
		{
			uint32_t A0 = addr & 0x01;
			uint32_t A1 = (addr >> 1) & 0x01;

			switch(_variant) {
				case VRCVariant::VRC2a:
					//Mapper 22
					A0 = (addr >> 1) & 0x01;
					A1 = (addr & 0x01);
					break;

				case VRCVariant::VRC2c:
				case VRCVariant::VRC4b:
				case VRCVariant::VRC4d:
					//Mapper 25
					//ORing both values should make most games work.
					//VRC2c & VRC4b (Both uses the same bits)
					A0 = (addr >> 1) & 0x01;
					A1 = (addr & 0x01);
					
					//VRC4d
					A0 |= (addr >> 3) & 0x01;
					A1 |= (addr >> 2) & 0x01;
					break;
				case VRCVariant::VRC4a:  
				case VRCVariant::VRC4c: 
					//Mapper 21
					//VRC4a
					A0 = (addr >> 1) & 0x01;
					A1 = (addr >> 2) & 0x01;

					//VRC4c
					A0 |= (addr >> 6) & 0x01;
					A1 |= (addr >> 7) & 0x01;
					break;
				
				case VRCVariant::VRC2b:
				case VRCVariant::VRC4e:
					//Mapper 23
					//VRC2b
					A0 = addr & 0x01;
					A1 = (addr >> 1) & 0x01;

					//VRC4e
					A0 |= (addr >> 2) & 0x01;
					A1 |= (addr >> 3) & 0x01;
					break;
				default:
					throw exception("not supported");
					break;
			}

			return (addr & 0xFF00) | (A1 << 1) | A0;
		}

		void StreamState(bool saving)
		{
			Stream<uint8_t>(_prgReg0);
			Stream<uint8_t>(_prgReg1);
			Stream<uint8_t>(_prgMode);

			StreamArray<uint8_t>(_loCHRRegs, 8);
			StreamArray<uint8_t>(_hiCHRRegs, 8);

			Stream<bool>(_hasIRQ);
		}
};