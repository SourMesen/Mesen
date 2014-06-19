#pragma once

#include "stdafx.h"
#include "ROMLoader.h"
#include "IMemoryHandler.h"

class BaseMapper : public IMemoryHandler
{
	protected:
		NESHeader _header;
		vector<MemoryBank> _romBanks;
		vector<MemoryBank> _vromBanks;

		virtual void InitMapper() = 0;

	public:
		void Initialize(NESHeader header, vector<MemoryBank> romBanks, vector<MemoryBank> vromBanks)
		{
			_header = header;
			_romBanks = romBanks;
			_vromBanks = vromBanks;

			InitMapper();
		}

		const NESHeader GetHeader()
		{
			return _header;
		}
};

class DefaultMapper : public BaseMapper
{
	vector<MemoryBank*> _mappedRomBanks;
	MemoryBank *_mappedVromBank;
	private:
		void InitMapper()
		{
			if(_romBanks.size() == 1) {
				_mappedRomBanks = { &_romBanks[0], &_romBanks[0] };
			} else {
				_mappedRomBanks = { &_romBanks[0], &_romBanks[1] };
			}

			_mappedVromBank = &_vromBanks[0];
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

		uint8_t ReadRAM(uint16_t addr)
		{
			return (*_mappedRomBanks[(addr >> 14) & 0x01])[addr & 0x3FFF];
		}

		void WriteRAM(uint16_t addr, uint8_t value)
		{
			(*_mappedRomBanks[(addr >> 14) & 0x01])[addr & 0x3FFF] = value;
		}

		uint8_t ReadVRAM(uint16_t addr)
		{
			return (*_mappedVromBank)[addr & 0x1FFF];
		}

		void WriteVRAM(uint16_t addr, uint8_t value)
		{
			(*_mappedVromBank)[addr & 0x1FFF] = value;
		}
};

class MapperFactory
{
	public:
		static unique_ptr<BaseMapper> InitializeFromFile(string filename)
		{
			ROMLoader loader(filename);

			NESHeader header = loader.GetHeader();
			
			uint8_t mapperID = header.GetMapperID();
			unique_ptr<BaseMapper> mapper = nullptr;
			switch(mapperID) {
				case 0: mapper = unique_ptr<BaseMapper>(new DefaultMapper()); break;
			}			

			if(!mapper) {
				throw std::exception("Unsupported mapper");
			}

			mapper->Initialize(header, loader.GetROMBanks(), loader.GetVROMBanks());
			return mapper;
		}
};
