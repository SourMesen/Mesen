#pragma once

#include "stdafx.h"
#include "IMemoryHandler.h"
#include "ROMLoader.h"

class BaseMapper : public IMemoryHandler
{
	protected:
		uint8_t* _prgRAM;
		uint8_t* _chrRAM;
		uint32_t _prgSize;
		uint32_t _chrSize;
		
		MirroringType _mirroringType;

		vector<uint8_t*> _prgPages;
		vector<uint8_t*> _chrPages;

		virtual void InitMapper() = 0;

	public:
		const static int PRGSize = 0x8000;
		const static int CHRSize = 0x2000;

	protected:
		virtual uint32_t GetPRGPageSize() = 0;
		virtual uint32_t GetCHRPageSize() = 0;

		void SelectPRGPage(uint32_t slot, uint32_t page)
		{
			//std::cout << std::dec << "PRG Slot " << (short)slot << ": " << (short)page << std::endl;
			_prgPages[slot] = &_prgRAM[(page & (GetPRGPageCount() - 1))  * GetPRGPageSize()];
		}

		void SelectCHRPage(uint32_t slot, uint32_t page)
		{
			//std::cout << std::dec << "CHR Slot " << (short)slot << ": " << (short)page << std::endl;
			_chrPages[slot] = &_chrRAM[(page & (GetCHRPageCount() - 1)) * GetCHRPageSize()];
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
			static uint32_t prgShift = -1;
			if(prgShift == -1) {
				prgShift = this->log2(GetPRGSlotCount());
			}

			return (addr >> (15 - prgShift)) & (GetPRGSlotCount() - 1);
		}

		uint32_t AddrToCHRSlot(uint16_t addr)
		{
			static uint32_t chrShift = -1;
			if(chrShift == -1) {
				chrShift = this->log2(GetCHRSlotCount());
			}

			return (addr >> (13 - chrShift)) & (GetCHRSlotCount() - 1);
		}

	public:
		void Initialize(ROMLoader &romLoader)
		{
			_mirroringType = romLoader.GetMirroringType();
			_prgRAM = romLoader.GetPRGRam();
			_chrRAM = romLoader.GetCHRRam();
			_prgSize = romLoader.GetPRGSize();
			_chrSize = romLoader.GetCHRSize();

			if(_chrSize == 0) {
				_chrRAM = new uint8_t[BaseMapper::CHRSize];
				_chrSize = BaseMapper::CHRSize;
			}

			for(int i = GetPRGSlotCount(); i > 0; i--) {
				_prgPages.push_back(nullptr);
			}

			for(int i = GetCHRSlotCount(); i > 0; i--) {
				_chrPages.push_back(nullptr);
			}

			InitMapper();
		}

		~BaseMapper()
		{
			delete[] _prgRAM;
			delete[] _chrRAM;
		}

		vector<std::array<uint16_t, 2>> GetRAMAddresses()
		{
			return { { { 0x8000, 0xFFFF } } };
		}
		
		vector<std::array<uint16_t, 2>> GetVRAMAddresses()
		{
			return { { { 0x0000, 0x1FFF } } };
		}

		virtual MirroringType GetMirroringType()
		{
			return _mirroringType;
		}
		
		uint8_t ReadRAM(uint16_t addr)
		{
			return _prgPages[AddrToPRGSlot(addr)][addr & (GetPRGPageSize() - 1)];
		}

		void WriteRAM(uint16_t addr, uint8_t value)
		{
			_prgPages[AddrToPRGSlot(addr)][addr & (GetPRGPageSize() - 1)] = value;
		}
		
		virtual uint8_t ReadVRAM(uint16_t addr)
		{
			return _chrPages[AddrToCHRSlot(addr)][addr & (GetCHRPageSize() - 1)];
		}

		virtual void WriteVRAM(uint16_t addr, uint8_t value)
		{
			_chrPages[AddrToCHRSlot(addr)][addr & (GetCHRPageSize() - 1)] = value;
		}
};