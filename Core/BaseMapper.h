#pragma once

#include "stdafx.h"
#include "Snapshotable.h"
#include "IMemoryHandler.h"
#include "ROMLoader.h"
#include <assert.h>

class BaseMapper : public IMemoryHandler, public Snapshotable
{
	protected:
		uint8_t* _prgRAM;
		uint8_t* _chrRAM;
		uint32_t _prgSize;
		uint32_t _chrSize;
		
		bool _hasCHRRAM;
		bool _hasBattery;
		wstring _romFilename;

		MirroringType _mirroringType;

		vector<uint8_t*> _prgPages;
		vector<uint8_t*> _chrPages;

		uint32_t* _prgSlotPages;
		uint32_t* _chrSlotPages;

		uint32_t _chrShift = -1;
		uint32_t _prgShift = -1;

		virtual void InitMapper() = 0;

	public:
		const static int PRGSize = 0x8000;
		const static int CHRSize = 0x2000;
		const static int PRGRAMSize = 0x2000;

	protected:
		virtual uint32_t GetPRGPageSize() = 0;
		virtual uint32_t GetCHRPageSize() = 0;

		void SelectPRGPage(uint32_t slot, uint32_t page)
		{
			//std::cout << std::dec << "PRG Slot " << (short)slot << ": " << (short)(page & (GetPRGPageCount() - 1)) << std::endl;
			_prgPages[slot] = &_prgRAM[(page & (GetPRGPageCount() - 1))  * GetPRGPageSize()];
			_prgSlotPages[slot] = page;
		}

		void SelectCHRPage(uint32_t slot, uint32_t page)
		{
			//std::cout << std::dec << "CHR Slot " << (short)slot << ": " << (short)page << std::endl;
			_chrPages[slot] = &_chrRAM[(page & (GetCHRPageCount() - 1)) * GetCHRPageSize()];
			_chrSlotPages[slot] = page;
		}

		uint32_t GetPRGSlotCount()
		{
			return BaseMapper::PRGSize / GetPRGPageSize();
		}

		uint32_t GetCHRSlotCount()
		{
			return BaseMapper::CHRSize / GetCHRPageSize();
		}

		uint32_t GetPRGPageCount()
		{
			return _prgSize / GetPRGPageSize();
		}

		uint32_t GetCHRPageCount()
		{
			return _chrSize / GetCHRPageSize();
		}

		uint32_t log2(uint32_t value)
		{
			uint32_t counter = 0;
			while(value >>= 1) {
				counter++;
			}
			return counter;
		}

		uint32_t AddrToPRGSlot(uint16_t addr)
		{
			if(_prgShift == -1) {
				_prgShift = this->log2(GetPRGSlotCount());
			}

			return (addr >> (15 - _prgShift)) & (GetPRGSlotCount() - 1);
		}

		uint32_t AddrToCHRSlot(uint16_t addr)
		{
			if(_chrShift == -1) {
				_chrShift = this->log2(GetCHRSlotCount());
			}

			return (addr >> (13 - _chrShift)) & (GetCHRSlotCount() - 1);
		}

		wstring GetBatteryFilename()
		{
			wstring filename = _romFilename;
			std::transform(filename.begin(), filename.end(), filename.begin(), std::tolower);
			if(filename.compare(filename.length() - 4, 4, L".nes") == 0) {
				//filename ends in .nes, remove it
				filename = _romFilename.substr(0, _romFilename.length() - 4);
			} else {
				filename = _romFilename;
			}
			filename += L".sav";
			return filename;
		}

	protected:
		virtual void StreamState(bool saving)
		{
			StreamArray<uint32_t>(_prgSlotPages, GetPRGSlotCount());
			StreamArray<uint32_t>(_chrSlotPages, GetCHRSlotCount());
			
			Stream<bool>(_hasCHRRAM);
			if(_hasCHRRAM) {
				StreamArray<uint8_t>(_chrRAM, BaseMapper::CHRSize);
			}

			Stream<MirroringType>(_mirroringType);

			if(!saving) {
				for(int i = GetPRGSlotCount() - 1; i >= 0; i--) {
					SelectPRGPage(i, _prgSlotPages[i]);
				}

				for(int i = GetCHRSlotCount() - 1; i >= 0; i--) {
					SelectCHRPage(i, _chrSlotPages[i]);
				}
			}
		}

	public:
		void Initialize(ROMLoader &romLoader)
		{
			_mirroringType = romLoader.GetMirroringType();
			_prgRAM = romLoader.GetPRGRam();
			_chrRAM = romLoader.GetCHRRam();
			_prgSize = romLoader.GetPRGSize();
			_chrSize = romLoader.GetCHRSize();
			_hasBattery = romLoader.HasBattery();
			_romFilename = romLoader.GetFilename();

			if(_chrSize == 0) {
				_hasCHRRAM = true;
				_chrRAM = new uint8_t[BaseMapper::CHRSize];
				_chrSize = BaseMapper::CHRSize;
			}

			for(int i = GetPRGSlotCount(); i > 0; i--) {
				_prgPages.push_back(nullptr);
			}

			for(int i = GetCHRSlotCount(); i > 0; i--) {
				_chrPages.push_back(nullptr);
			}

			_prgSlotPages = new uint32_t[GetPRGSlotCount()];
			_chrSlotPages = new uint32_t[GetCHRSlotCount()];

			InitMapper();
		}

		virtual ~BaseMapper()
		{
			delete[] _prgRAM;
			delete[] _chrRAM;
			delete[] _prgSlotPages;
			delete[] _chrSlotPages;
		}

		void GetMemoryRanges(MemoryRanges &ranges)
		{
			ranges.AddHandler(MemoryType::RAM, MemoryOperation::Read, 0x8000, 0xFFFF);
			ranges.AddHandler(MemoryType::RAM, MemoryOperation::Write, 0x8000, 0xFFFF);

			ranges.AddHandler(MemoryType::VRAM, MemoryOperation::Read, 0x0000, 0x1FFF);
			ranges.AddHandler(MemoryType::VRAM, MemoryOperation::Write, 0x0000, 0x1FFF);
		}

		bool HasBattery()
		{
			return _hasBattery;
		}

		MirroringType GetMirroringType()
		{
			return _mirroringType;
		}

		void LoadBattery(uint8_t *sramBuffer)
		{
			ifstream batteryFile(GetBatteryFilename(), ios::in | ios::binary);

			if(batteryFile) {
				batteryFile.read((char*)sramBuffer, BaseMapper::PRGRAMSize);

				batteryFile.close();
			}
		}

		void SaveBattery(uint8_t *sramBuffer)
		{
			ofstream batteryFile(GetBatteryFilename(), ios::out | ios::binary);

			if(batteryFile) {
				batteryFile.write((char*)sramBuffer, BaseMapper::PRGRAMSize);

				batteryFile.close();
			}
		}
		
		uint8_t ReadRAM(uint16_t addr)
		{
			return _prgPages[AddrToPRGSlot(addr)][addr & (GetPRGPageSize() - 1)];
		}

		void WriteRAM(uint16_t addr, uint8_t value)
		{
			assert(false);
			//_prgPages[AddrToPRGSlot(addr)][addr & (GetPRGPageSize() - 1)] = value;
		}
		
		virtual uint8_t ReadVRAM(uint16_t addr)
		{
			return _chrPages[AddrToCHRSlot(addr)][addr & (GetCHRPageSize() - 1)];
		}

		virtual void WriteVRAM(uint16_t addr, uint8_t value)
		{
			if(_hasCHRRAM) {
				_chrPages[AddrToCHRSlot(addr)][addr & (GetCHRPageSize() - 1)] = value;
			} else {
				assert(false);
			}
		}

		virtual void NotifyVRAMAddressChange(uint16_t addr)
		{
			//Used for MMC3 IRQ counter
		}
};