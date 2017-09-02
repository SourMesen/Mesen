#pragma once
#include "stdafx.h"
#include "BaseMapper.h"
#include "VrcIrq.h"

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
	VRC6a,
	VRC6b
};

class VRC2_4 : public BaseMapper
{
	private:
		VrcIrq _irq;
		VRCVariant _variant;
		bool _useHeuristics;

		uint8_t _prgReg0;
		uint8_t _prgReg1;
		uint8_t _prgMode;

		uint8_t _hiCHRRegs[8];
		uint8_t _loCHRRegs[8];

		bool _hasIRQ;

		void DetectVariant()
		{
			switch(_mapperID) {
				default:
				case 21:
					//Conflicts: VRC4c
					switch(_subMapperID) {
						default:
						case 0: _variant = VRCVariant::VRC4a; break;
						case 1: _variant = VRCVariant::VRC4a; break;
						case 2: _variant = VRCVariant::VRC4c; break;
					}
					break;

				case 22: _variant = VRCVariant::VRC2a; break;

				case 23:
					//Conflicts: VRC4e
					switch(_subMapperID) {
						default:
						case 0: _variant = VRCVariant::VRC2b; break;
						case 2: _variant = VRCVariant::VRC4e; break;
						case 3: _variant = VRCVariant::VRC2b; break;
					}
					break;

				case 25:
					//Conflicts: VRC2c, VRC4d
					switch(_subMapperID) {
						default:
						case 0: _variant = VRCVariant::VRC4b; break;
						case 1: _variant = VRCVariant::VRC4b; break;
						case 2: _variant = VRCVariant::VRC4d; break;
						case 3: _variant = VRCVariant::VRC2c; break;
					}
					break;

				case 27: _variant = VRCVariant::VRC4_27; break; //Untested
			}

			_useHeuristics = (_subMapperID == 0) && _mapperID != 22 && _mapperID != 27;
		}

	protected:
		virtual uint16_t GetPRGPageSize() override { return 0x2000; }
		virtual uint16_t GetCHRPageSize() override {	return 0x0400; }

		void InitMapper() override 
		{
			DetectVariant();

			_prgMode = 0;
			_prgReg0 = 0;
			_prgReg1 = 0;
			_hasIRQ = false;
			memset(_loCHRRegs, 0, sizeof(_loCHRRegs));
			memset(_hiCHRRegs, 0, sizeof(_hiCHRRegs));

			UpdateState();
		}

		void ProcessCpuClock() override
		{
			if((_useHeuristics && _mapperID != 22) || _variant >= VRCVariant::VRC4a) {
				//Only VRC4 supports IRQs
				_irq.ProcessCpuClock();
			}
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

		void WriteRegister(uint16_t addr, uint8_t value) override
		{
			addr = TranslateAddress(addr) & 0xF00F;

			if(addr >= 0x8000 && addr <= 0x8006) {
				_prgReg0 = value & 0x1F;
			} else if((_variant <= VRCVariant::VRC2c && addr >= 0x9000 && addr <= 0x9003) || (_variant >= VRCVariant::VRC4a && addr >= 0x9000 && addr <= 0x9001)) {
				uint8_t mask = 0x03;
				if(!_useHeuristics && (_variant >= VRCVariant::VRC2a && _variant <= VRCVariant::VRC2c)) {
					//When we are certain this is a VRC2 game, only use the first bit for mirroring selection
					mask = 0x01;
				}

				switch(value & mask) {
					case 0: SetMirroringType(MirroringType::Vertical); break;
					case 1: SetMirroringType(MirroringType::Horizontal); break;
					case 2: SetMirroringType(MirroringType::ScreenAOnly); break;
					case 3: SetMirroringType(MirroringType::ScreenBOnly); break;
				}
			} else if(_variant >= VRCVariant::VRC4a && addr >= 0x9002 && addr <= 0x9003) {
				_prgMode = (value >> 1) & 0x01;
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
			} else if(addr == 0xF000) {
				_irq.SetReloadValueNibble(value, false);
			} else if(addr == 0xF001) {
				_irq.SetReloadValueNibble(value, true);
			} else if(addr == 0xF002) {
				_irq.SetControlValue(value);
			} else if(addr == 0xF003) {
				_irq.AcknowledgeIrq();
			}

			UpdateState();
		}

	public:		
		uint16_t TranslateAddress(uint16_t addr)
		{
			uint32_t A0, A1;

			if(_useHeuristics) {
				switch(_variant) {
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
						throw std::runtime_error("not supported");
						break;
				}
			} else {
				switch(_variant) {
					case VRCVariant::VRC2a:
						//Mapper 22
						A0 = (addr >> 1) & 0x01;
						A1 = (addr & 0x01);
						break;

					case VRCVariant::VRC4_27:
						//Mapper 27
						A0 = addr & 0x01;
						A1 = (addr >> 1) & 0x01;
						break;

					case VRCVariant::VRC2c:
					case VRCVariant::VRC4b:
						//Mapper 25
						A0 = (addr >> 1) & 0x01;
						A1 = (addr & 0x01);
						break;

					case VRCVariant::VRC4d:
						//Mapper 25
						A0 = (addr >> 3) & 0x01;
						A1 = (addr >> 2) & 0x01;
						break;

					case VRCVariant::VRC4a:
						//Mapper 21
						A0 = (addr >> 1) & 0x01;
						A1 = (addr >> 2) & 0x01;
						break;

					case VRCVariant::VRC4c:
						//Mapper 21
						A0 = (addr >> 6) & 0x01;
						A1 = (addr >> 7) & 0x01;
						break;

					case VRCVariant::VRC2b:
						//Mapper 23
						A0 = addr & 0x01;
						A1 = (addr >> 1) & 0x01;
						break;

					case VRCVariant::VRC4e:
						//Mapper 23
						A0 = (addr >> 2) & 0x01;
						A1 = (addr >> 3) & 0x01;
						break;

					default:
						throw std::runtime_error("not supported");
						break;
				}

			}

			return (addr & 0xFF00) | (A1 << 1) | A0;
		}

		void StreamState(bool saving) override
		{
			BaseMapper::StreamState(saving);
			ArrayInfo<uint8_t> loChrRegs = { _loCHRRegs, 8 };
			ArrayInfo<uint8_t> hiChrRegs = { _hiCHRRegs, 8 };
			SnapshotInfo irq{ &_irq };
			Stream(_prgReg0, _prgReg1, _prgMode, loChrRegs, hiChrRegs, _hasIRQ, irq);
		}
};