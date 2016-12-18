#pragma once
#include "stdafx.h"
#include "BaseMapper.h"

class MMC2 : public BaseMapper
{
	private:
		enum class MMC2Registers
		{
			RegA000 = 0xA,
			RegB000 = 0xB,
			RegC000 = 0xC,
			RegD000 = 0xD,
			RegE000 = 0xE,
			RegF000 = 0xF
		};

	protected:
		uint8_t _leftLatch;
		uint8_t _rightLatch;
		uint8_t _leftChrPage[2];
		uint8_t _rightChrPage[2];
		bool _needChrUpdate;

		virtual uint16_t GetPRGPageSize() override { return 0x2000; }
		virtual uint16_t GetCHRPageSize() override {	return 0x1000; }

		virtual void InitMapper() override 
		{
			_leftLatch = 1;
			_rightLatch = 1;
			_leftChrPage[0] = 0;
			_leftChrPage[1] = 0;
			_rightChrPage[0] = 0;
			_rightChrPage[1] = 0;
			_needChrUpdate = false;

			SelectPRGPage(0, 0);
			SelectPRGPage(1, -3);
			SelectPRGPage(2, -2);
			SelectPRGPage(3, -1);
			SelectCHRPage(0, 0);
			SelectCHRPage(0, 1);
		}

		void StreamState(bool saving) override
		{
			BaseMapper::StreamState(saving);
			Stream(_leftLatch, _rightLatch, _needChrUpdate, _leftChrPage[0], _leftChrPage[1], _rightChrPage[0], _rightChrPage[1]);			
		}

		void WriteRegister(uint16_t addr, uint8_t value) override
		{
			switch((MMC2Registers)(addr >> 12)) {
				case MMC2Registers::RegA000:
					SelectPRGPage(0, value & 0x0F);
					break;

				case MMC2Registers::RegB000:
					_leftChrPage[0] = value & 0x1F;
					SelectCHRPage(0, _leftChrPage[_leftLatch]);
					break;

				case MMC2Registers::RegC000:
					_leftChrPage[1] = value & 0x1F;
					SelectCHRPage(0, _leftChrPage[_leftLatch]);
					break;

				case MMC2Registers::RegD000:
					_rightChrPage[0] = value & 0x1F;
					SelectCHRPage(1, _rightChrPage[_rightLatch]);
					break;

				case MMC2Registers::RegE000:
					_rightChrPage[1] = value & 0x1F;
					SelectCHRPage(1, _rightChrPage[_rightLatch]);
					break;

				case MMC2Registers::RegF000:
					SetMirroringType(((value & 0x01) == 0x01) ? MirroringType::Horizontal : MirroringType::Vertical);
					break;
			}
		}

	public:
		virtual void NotifyVRAMAddressChange(uint16_t addr) override
		{
			if(_needChrUpdate) {
				SelectCHRPage(0, _leftChrPage[_leftLatch]);
				SelectCHRPage(1, _rightChrPage[_rightLatch]);
				_needChrUpdate = false;
			}

			if(addr == 0x0FD8) {
				_leftLatch = 0;
				_needChrUpdate = true;
			} else if(addr == 0x0FE8) {
				_leftLatch = 1;
				_needChrUpdate = true;
			} else if(addr >= 0x1FD8 && addr <= 0x1FDF) {
				_rightLatch = 0;
				_needChrUpdate = true;
			} else if(addr >= 0x1FE8 && addr <= 0x1FEF) {
				_rightLatch = 1;
				_needChrUpdate = true;
			}
		}
};