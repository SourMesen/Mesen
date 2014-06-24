#pragma once

#include "stdafx.h"
#include "IMemoryHandler.h"
#include "ROMLoader.h"

class BaseMapper : public IMemoryHandler
{
	private:
		MirroringType _mirroringType;

	protected:
		uint8_t* _prgRAM;
		uint8_t* _chrRAM;
		uint32_t _prgSize;
		uint32_t _chrSize;

		virtual void InitMapper() = 0;

	public:
		const static int PRGSize = 0x8000;
		const static int CHRSize = 0x2000;

	public:
		void Initialize(MirroringType mirroringType, ROMLoader &romLoader)
		{
			_mirroringType = mirroringType;
			_prgRAM = romLoader.GetPRGRam();
			_chrRAM = romLoader.GetCHRRam();
			_prgSize = romLoader.GetPRGSize();
			_chrSize = romLoader.GetCHRSize();

			if(_chrSize == 0) {
				_chrRAM = new uint8_t[BaseMapper::CHRSize];
				_chrSize = BaseMapper::CHRSize;
			}

			InitMapper();
		}

		~BaseMapper()
		{
			delete[] _prgRAM;
			delete[] _chrRAM;
		}

		virtual MirroringType GetMirroringType()
		{
			return _mirroringType;
		}
};

class DefaultMapper : public BaseMapper
{
	protected:
		vector<uint8_t*> _mappedRomBanks;
		vector<uint8_t*> _mappedVromBanks;

		virtual void InitMapper()
		{
			if(_prgSize == 0x4000) {
				_mappedRomBanks = { _prgRAM, _prgRAM };
			} else {
				_mappedRomBanks = { _prgRAM, &_prgRAM[0x4000] };
			}

			_mappedVromBanks.push_back(_chrRAM);
		}

	public:
		vector<std::array<uint16_t, 2>> GetRAMAddresses()
		{
			return { { { 0x8000, 0xFFFF } } };
		}
		
		vector<std::array<uint16_t, 2>> GetVRAMAddresses()
		{
			return { { { 0x0000, 0x1FFF } } };
		}

		virtual uint8_t ReadRAM(uint16_t addr)
		{
			return _mappedRomBanks[(addr >> 14) & 0x01][addr & 0x3FFF];
		}

		virtual void WriteRAM(uint16_t addr, uint8_t value)
		{
			_mappedRomBanks[(addr >> 14) & 0x01][addr & 0x3FFF] = value;
		}

		virtual uint8_t ReadVRAM(uint16_t addr)
		{
			return _mappedVromBanks[0][addr & 0x1FFF];
		}

		virtual void WriteVRAM(uint16_t addr, uint8_t value)
		{
			_mappedVromBanks[0][addr & 0x1FFF] = value;
		}
};

class Mapper2 : public DefaultMapper
{
	private:
		void InitMapper() 
		{
			DefaultMapper::InitMapper();

			uint8_t numberOfBanks = _prgSize / 0x4000;
			_mappedRomBanks[1] = &_prgRAM[(numberOfBanks - 1) * 0x4000];
		}

	public:		
		void WriteRAM(uint16_t addr, uint8_t value)
		{
			_mappedRomBanks[0] = &_prgRAM[value * 0x4000];
		}
};