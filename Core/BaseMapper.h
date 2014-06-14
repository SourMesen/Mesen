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

	public:
		void Initialize(NESHeader header, vector<MemoryBank> romBanks, vector<MemoryBank> vromBanks)
		{
			_header = header;
			_romBanks = romBanks;
			_vromBanks = vromBanks;
		}
};

class DefaultMapper : public BaseMapper
{
	private:

	public:
		std::array<int, 2> GetIOAddresses()
		{
			return std::array<int, 2> {{ 0x8000, 0xFFFF }};
		}

		uint8_t MemoryRead(uint16_t addr)
		{
			return _romBanks[(addr >> 14) & 0x01][addr & 0x3FFF];
		}

		void MemoryWrite(uint16_t addr, uint8_t value)
		{
			_romBanks[(addr >> 14) & 0x01][addr & 0x3FFF] = value;
		}
};

class MapperFactory
{
	public:
		static shared_ptr<BaseMapper> InitializeFromFile(char *filename)
		{
			ROMLoader loader(filename);

			NESHeader header = loader.GetHeader();
			
			uint8_t mapperID = header.GetMapperID();
			shared_ptr<BaseMapper> mapper = nullptr;
			switch(mapperID) {
				case 0: mapper = shared_ptr<BaseMapper>(new DefaultMapper()); break;
			}			

			if(!mapper) {
				throw std::exception("Unsupported mapper");
			}

			mapper->Initialize(header, loader.GetROMBanks(), loader.GetVROMBanks());
			return mapper;
		}
};
