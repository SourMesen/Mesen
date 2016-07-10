#pragma once
#include "stdafx.h"
#include "BaseMapper.h"
#include "VsControlManager.h"

class VsSystem : public BaseMapper
{
private:
	uint8_t _prgChrSelectBit = false;

protected:
	virtual uint16_t GetPRGPageSize() { return 0x2000; }
	virtual uint16_t GetCHRPageSize() { return 0x2000; }

	virtual void InitMapper()
	{
		//"Note: unlike all other mappers, an undersize mapper 99 image implies open bus instead of mirroring."
		//However, it doesn't look like any game actually rely on this behavior?  So not implemented for now.
		SelectPRGPage(0, 0);
		SelectPRGPage(1, 1);
		SelectPRGPage(2, 2);
		SelectPRGPage(3, 3);

		SelectCHRPage(0, 0);

		//Force VS system if mapper 99 (since we assume VsControlManager exists below)
		_gameSystem = GameSystem::VsUniSystem;
	}

	void StreamState(bool saving)
	{
		BaseMapper::StreamState(saving);

		Stream(_prgChrSelectBit);
	}

	void ProcessCpuClock()
	{
		if(_prgChrSelectBit != VsControlManager::GetInstance()->GetPrgChrSelectBit()) {
			_prgChrSelectBit = VsControlManager::GetInstance()->GetPrgChrSelectBit();

			if(_prgSize > 0x8000) {
				//"Note: In case of games with 40KiB PRG - ROM(as found in VS Gumshoe), the above bit additionally changes 8KiB PRG - ROM at $8000 - $9FFF."
				//"Only Vs. Gumshoe uses the 40KiB PRG variant; in the iNES encapsulation, the 8KiB banks are arranged as 0, 1, 2, 3, 0alternate, empty"
				SelectPRGPage(0, _prgChrSelectBit << 2);
			}

			SelectCHRPage(0, _prgChrSelectBit);
		}
	}
};
