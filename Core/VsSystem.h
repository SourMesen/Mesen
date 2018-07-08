#pragma once
#include "stdafx.h"
#include "BaseMapper.h"
#include "VsControlManager.h"

class VsSystem : public BaseMapper
{
private:
	uint8_t _prgChrSelectBit = 0;

protected:
	virtual uint16_t GetPRGPageSize() override { return 0x2000; }
	virtual uint16_t GetCHRPageSize() override { return 0x2000; }
	virtual uint32_t GetWorkRamSize() override { return 0x800; }

	virtual void InitMapper() override
	{
		if(!IsNes20()) {
			//Force VS system if mapper 99
			_romInfo.System = GameSystem::VsSystem;
			if(_prgSize >= 0x10000) {
				_romInfo.VsType = VsSystemType::VsDualSystem;
			} else {
				_romInfo.VsType = VsSystemType::Default;
			}
		}

		//"Note: unlike all other mappers, an undersize mapper 99 image implies open bus instead of mirroring."
		//However, it doesn't look like any game actually rely on this behavior?  So not implemented for now.
		uint8_t prgOuter = _console->IsMaster() ? 0 : 4;
		SelectPRGPage(0, 0 | prgOuter);
		SelectPRGPage(1, 1 | prgOuter);
		SelectPRGPage(2, 2 | prgOuter);
		SelectPRGPage(3, 3 | prgOuter);

		uint8_t chrOuter = _console->IsMaster() ? 0 : 2;
		SelectCHRPage(0, 0 | chrOuter);
	}

	void Reset(bool softReset) override
	{
		BaseMapper::Reset(softReset);
		UpdateMemoryAccess(0);
	}

	void StreamState(bool saving) override
	{
		BaseMapper::StreamState(saving);
		Stream(_prgChrSelectBit);
	}

	void ProcessCpuClock() override
	{
		VsControlManager* controlManager = dynamic_cast<VsControlManager*>(_console->GetControlManager());
		if(controlManager && _prgChrSelectBit != controlManager->GetPrgChrSelectBit()) {
			_prgChrSelectBit = controlManager->GetPrgChrSelectBit();

			if(_romInfo.VsType == VsSystemType::Default && _prgSize > 0x8000) {
				//"Note: In case of games with 40KiB PRG - ROM(as found in VS Gumshoe), the above bit additionally changes 8KiB PRG - ROM at $8000 - $9FFF."
				//"Only Vs. Gumshoe uses the 40KiB PRG variant; in the iNES encapsulation, the 8KiB banks are arranged as 0, 1, 2, 3, 0alternate, empty"
				SelectPRGPage(0, _prgChrSelectBit << 2);
			}

			uint8_t chrOuter = _console->IsMaster() ? 0 : 2;
			SelectCHRPage(0, _prgChrSelectBit | chrOuter);
		}
	}

public:
	void UpdateMemoryAccess(uint8_t slaveMasterBit)
	{
		shared_ptr<Console> dualConsole = _console->GetDualConsole();
		if(_console->IsMaster() && dualConsole) {
			VsSystem* otherMapper = dynamic_cast<VsSystem*>(dualConsole->GetMapper());

			//Give memory access to master CPU or slave CPU, based on "slaveMasterBit"
			for(int i = 0; i < 4; i++) {
				SetCpuMemoryMapping(0x6000 + i * 0x800, 0x67FF + i * 0x800, _workRam, slaveMasterBit ? MemoryAccessType::ReadWrite : MemoryAccessType::NoAccess);
				otherMapper->SetCpuMemoryMapping(0x6000 + i * 0x800, 0x67FF + i * 0x800, _workRam, slaveMasterBit ? MemoryAccessType::NoAccess : MemoryAccessType::ReadWrite);
			}
		}
	}
};
