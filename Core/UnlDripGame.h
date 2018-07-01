#pragma once
#include "stdafx.h"
#include "BaseMapper.h"
#include "UnlDripGameAudio.h"

class UnlDripGame : public BaseMapper
{
private:
	unique_ptr<UnlDripGameAudio> _audioChannels[2];
	uint8_t _extendedAttributes[2][0x400];
	uint8_t _lowByteIrqCounter;
	uint16_t _irqCounter;
	uint16_t _lastNametableFetchAddr;
	bool _irqEnabled;
	bool _extAttributesEnabled;
	bool _wramWriteEnabled;
	bool _dipSwitch;

protected:
	uint16_t GetPRGPageSize() override { return 0x4000; }
	uint16_t GetCHRPageSize() override { return 0x800; }
	bool AllowRegisterRead() override { return true; }
	uint16_t RegisterStartAddress() override { return 0x8000; }
	uint16_t RegisterEndAddress() override { return 0xFFFF; }

	void InitMapper() override
	{
		_audioChannels[0].reset(new UnlDripGameAudio(_console));
		_audioChannels[1].reset(new UnlDripGameAudio(_console));

		_lowByteIrqCounter = 0;
		_irqCounter = 0;
		_irqEnabled = false;
		_extAttributesEnabled = false;
		_wramWriteEnabled = false;
		_dipSwitch = false;
		_lastNametableFetchAddr = 0;

		InitializeRam(_extendedAttributes[0], 0x400);
		InitializeRam(_extendedAttributes[1], 0x400);

		SelectPRGPage(1, -1);

		AddRegisterRange(0x4800, 0x5FFF, MemoryOperation::Read);
		RemoveRegisterRange(0x8000, 0xFFFF, MemoryOperation::Read);
	}

	void StreamState(bool saving) override
	{
		BaseMapper::StreamState(saving);

		ArrayInfo<uint8_t> extAttributes1 { _extendedAttributes[0], 0x400 };
		ArrayInfo<uint8_t> extAttributes2 { _extendedAttributes[1], 0x400 };
		SnapshotInfo audioChannel1 { _audioChannels[0].get() };
		SnapshotInfo audioChannel2 { _audioChannels[1].get() };

		Stream(extAttributes1, extAttributes2, audioChannel1, audioChannel2, _lowByteIrqCounter, _irqCounter, _irqEnabled,
				 _extAttributesEnabled, _wramWriteEnabled, _dipSwitch);
		
		if(!saving) {
			UpdateWorkRamState();
		}
	}

	void ProcessCpuClock() override
	{
		if(_irqEnabled) {
			if(_irqCounter > 0) {
				_irqCounter--;
				if(_irqCounter == 0) {
					//While the IRQ counter is enabled, the timer is decremented once per CPU
					//cycle.Once the timer reaches zero, the /IRQ line is set to logic 0 and the
					//timer stops decrementing
					_irqEnabled = false;
					_console->GetCpu()->SetIrqSource(IRQSource::External);
				}
			}
		}

		_audioChannels[0]->Clock();
		_audioChannels[1]->Clock();
	}

	void UpdateWorkRamState()
	{
		SetCpuMemoryMapping(0x6000, 0x7FFF, 0, PrgMemoryType::WorkRam, _wramWriteEnabled ? MemoryAccessType::ReadWrite : MemoryAccessType::Read);
	}
	
	virtual uint8_t MapperReadVRAM(uint16_t addr, MemoryOperationType memoryOperationType) override
	{
		if(_extAttributesEnabled && memoryOperationType == MemoryOperationType::PpuRenderingRead) {
			if(addr >= 0x2000 && (addr & 0x3FF) < 0x3C0) {
				//Nametable fetches
				_lastNametableFetchAddr = addr & 0x03FF;
			} else if(addr >= 0x2000 && (addr & 0x3FF) >= 0x3C0) {
				//Attribute fetches
				uint8_t bank;
				switch(GetMirroringType()) {
					default:
					case MirroringType::ScreenAOnly: bank = 0; break;
					case MirroringType::ScreenBOnly: bank = 1; break;
					case MirroringType::Horizontal: bank = (addr & 0x800) ? 1 : 0; break;
					case MirroringType::Vertical: bank = (addr & 0x400) ? 1 : 0; break;
				}
				
				//Return a byte containing the same palette 4 times - this allows the PPU to select the right palette no matter the shift value
				uint8_t value = _extendedAttributes[bank][_lastNametableFetchAddr & 0x3FF] & 0x03;
				return (value << 6) | (value << 4) | (value << 2) | value;
			}
		}
		return BaseMapper::MapperReadVRAM(addr, memoryOperationType);
	}

	uint8_t ReadRegister(uint16_t addr) override
	{
		switch(addr & 0x5800) {
			case 0x4800: return (_dipSwitch ? 0x80 : 0) | 0x64;
			case 0x5000: return _audioChannels[0]->ReadRegister();
			case 0x5800: return _audioChannels[1]->ReadRegister();
		}
		return 0;
	}

	void WriteRegister(uint16_t addr, uint8_t value) override
	{
		if(addr <= 0xBFFF) {
			switch(addr & 0x800F) {
				case 0x8000: case 0x8001: case 0x8002: case 0x8003:
					_audioChannels[0]->WriteRegister(addr, value);
					break;

				case 0x8004: case 0x8005: case 0x8006: case 0x8007:
					_audioChannels[1]->WriteRegister(addr, value);
					break;

				case 0x8008:
					_lowByteIrqCounter = value;
					break;

				case 0x8009:
					//Data written to the IRQ Counter Low register is buffered until writing to IRQ
					//Counter High, at which point the composite data is written directly to the IRQ	timer.
					_irqCounter = ((value & 0x7F) << 8) | _lowByteIrqCounter;
					_irqEnabled = (value & 0x80) != 0;

					//Writing to the IRQ Enable register will acknowledge the interrupt and return the /IRQ signal to logic 1.
					_console->GetCpu()->ClearIrqSource(IRQSource::External);
					break;

				case 0x800A:
					switch(value & 0x03) {
						case 0: SetMirroringType(MirroringType::Vertical); break;
						case 1: SetMirroringType(MirroringType::Horizontal); break;
						case 2: SetMirroringType(MirroringType::ScreenAOnly); break;
						case 3: SetMirroringType(MirroringType::ScreenBOnly); break;
					}
					_extAttributesEnabled = (value & 0x04) != 0;
					_wramWriteEnabled = (value & 0x08) != 0;
					UpdateWorkRamState();
					break;

				case 0x800B: SelectPRGPage(0, value & 0x0F); break;
				case 0x800C: SelectCHRPage(0, value & 0x0F); break;
				case 0x800D: SelectCHRPage(1, value & 0x0F); break;
				case 0x800E: SelectCHRPage(2, value & 0x0F); break;
				case 0x800F: SelectCHRPage(3, value & 0x0F); break;
			}
		} else {
			//Attribute expansion memory at $C000-$C7FF is mirrored throughout $C000-$FFFF.
			_extendedAttributes[(addr & 0x400) ? 1 : 0][addr & 0x3FF] = value;
		}
	}
};
