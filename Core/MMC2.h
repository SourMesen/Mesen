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

		uint8_t _leftLatch;
		uint8_t _rightLatch;
		uint8_t _leftChrPage[2];
		uint8_t _rightChrPage[2];

	protected:
		virtual uint16_t GetPRGPageSize() { return 0x2000; }
		virtual uint16_t GetCHRPageSize() {	return 0x1000; }

		void InitMapper() 
		{
			_leftLatch = 1;
			_rightLatch = 1;
			_leftChrPage[0] = 0;
			_leftChrPage[1] = 0;
			_rightChrPage[0] = 0;
			_rightChrPage[1] = 0;

			SelectPRGPage(0, 0);
			SelectPRGPage(1, -3);
			SelectPRGPage(2, -2);
			SelectPRGPage(3, -1);
			SelectCHRPage(0, 0);
			SelectCHRPage(0, 1);
		}

		void StreamState(bool saving)
		{
			Stream<uint8_t>(_leftLatch);
			Stream<uint8_t>(_rightLatch);
			StreamArray<uint8_t>(_leftChrPage, 2);
			StreamArray<uint8_t>(_rightChrPage, 2);

			BaseMapper::StreamState(saving);
		}

		void WriteRegister(uint16_t addr, uint8_t value)
		{
			switch((MMC2Registers)(addr >> 12)) {
				case MMC2Registers::RegA000:
					SelectPRGPage(0, value);
					break;

				case MMC2Registers::RegB000:
					_leftChrPage[0] = value;
					SelectCHRPage(0, _leftChrPage[_leftLatch]);
					break;

				case MMC2Registers::RegC000:
					_leftChrPage[1] = value;
					SelectCHRPage(0, _leftChrPage[_leftLatch]);
					break;

				case MMC2Registers::RegD000:
					_rightChrPage[0] = value;
					SelectCHRPage(1, _rightChrPage[_rightLatch]);
					break;

				case MMC2Registers::RegE000:
					_rightChrPage[1] = value;
					SelectCHRPage(1, _rightChrPage[_rightLatch]);
					break;

				case MMC2Registers::RegF000:
					SetMirroringType(((value & 0x01) == 0x01) ? MirroringType::Horizontal : MirroringType::Vertical);
					break;
			}
		}

	public:
		virtual void NotifyVRAMAddressChange(uint16_t addr)
		{
			SelectCHRPage(0, _leftChrPage[_leftLatch]);
			SelectCHRPage(1, _rightChrPage[_rightLatch]);

			switch(addr & 0xFFF8) {
				case 0x0FD8: _leftLatch = 0; break;
				case 0x0FE8: _leftLatch = 1; break;
				case 0x1FD8: _rightLatch = 0; break;
				case 0x1FE8: _rightLatch = 1; break;
			}
		}
};