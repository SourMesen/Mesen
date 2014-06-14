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
};

class DefaultMapper : public BaseMapper
{
	vector<MemoryBank*> _mappedRomBanks;
	private:
		void InitMapper()
		{
			if(_romBanks.size() == 1) {
				_mappedRomBanks = { &_romBanks[0], &_romBanks[0] };
			} else {
				_mappedRomBanks = { &_romBanks[0], &_romBanks[1] };
			}
		}

	public:
		std::array<int, 2> GetIOAddresses()
		{
			return std::array<int, 2> {{ 0x8000, 0xFFFF }};
		}

		uint8_t MemoryRead(uint16_t addr)
		{
			return (*_mappedRomBanks[(addr >> 14) & 0x01])[addr & 0x3FFF];
		}

		void MemoryWrite(uint16_t addr, uint8_t value)
		{
			(*_mappedRomBanks[(addr >> 14) & 0x01])[addr & 0x3FFF] = value;
		}
};

class MapperFactory
{
	public:
		static shared_ptr<BaseMapper> InitializeFromFile(string filename)
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
