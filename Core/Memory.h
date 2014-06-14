#include "stdafx.h"
#include "EventHandler.h"
#include "PPU.h"

using std::vector;
using std::shared_ptr;
using std::unique_ptr;
using std::ios;
using std::ifstream;

struct NESHeader
{
	char NES[4];
	uint8_t ROMCount;
	uint8_t VROMCount;
	uint8_t Flags1;
	uint8_t Flags2;
	uint8_t RAMCount;
	uint8_t CartType;
	uint8_t Reserved[6];

	uint8_t GetMapperID()
	{
		return (Flags2 & 0xF0) | (Flags1 >> 4);
	}
};

typedef vector<uint8_t> MemoryBank;

class ROMLoader
{
	private:
		const int ROMBankSize = 0x4000;
		const int VROMBankSize = 0x2000;
		NESHeader _header;
		vector<MemoryBank> _romBanks;
		vector<MemoryBank> _vromBanks;

	public:
		ROMLoader(const char* filename)
		{
			_romBanks.clear();
			_vromBanks.clear();

			ifstream romFile(filename, ios::in | ios::binary);

			if(!romFile) {
				return;
			}

			romFile.read((char*)&_header, sizeof(NESHeader));

			uint8_t *buffer = new uint8_t[max(ROMBankSize, VROMBankSize)];
			for(int i = 0; i < _header.ROMCount; i++) {
				romFile.read((char*)buffer, ROMBankSize);
				_romBanks.push_back(MemoryBank(buffer, buffer + ROMBankSize));
			}

			for(int i = 0; i < _header.VROMCount; i++) {
				romFile.read((char*)buffer, VROMBankSize);
				_vromBanks.push_back(MemoryBank(buffer, buffer + VROMBankSize));
			}
			
			delete[] buffer;

			romFile.close();
		}

		vector<MemoryBank> GetROMBanks()
		{
			return _romBanks;
		}

		vector<MemoryBank> GetVROMBanks()
		{
			return _vromBanks;
		}

		NESHeader GetHeader()
		{
			return _header;
		}
};

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
		uint8_t MemoryRead(uint16_t addr)
		{
			return _romBanks[addr <= 0xC000 ? 0 : 1][addr & 0x3FFF];
		}

		void MemoryWrite(uint16_t addr, uint8_t value)
		{
			_romBanks[addr <= 0xC000 ? 0 : 1][addr & 0x3FFF] = value;
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

class MemoryManager
{
	private:
		const int InternalRAMSize = 0x800;
		const int SRAMSize = 0x800;

		/*EventHandler<IMemoryHandler, uint16_t> _readRegisterHandler;
		EventHandler<IMemoryHandler, uint16_t> _writeRegisterHandler;*/

		shared_ptr<BaseMapper> _mapper;
		uint8_t *_internalRAM;
		uint8_t *_SRAM;

		PPU _ppu;

		vector<IMemoryHandler*> _registerHandlers;
			
		void RegisterIODevice(IMemoryHandler *handler, uint16_t startAddr, uint16_t endAddr)
		{
			for(int i = startAddr; i < endAddr; i++) {
				_registerHandlers[i] = handler;
			}
		}

		uint8_t ReadRegister(uint16_t addr)
		{
			if(_registerHandlers[addr] != nullptr) {
				return _registerHandlers[addr]->MemoryRead(addr);
			} else {
				return 0;
			}
		}

		void WriteRegister(uint16_t addr, uint8_t value)
		{
			if(_registerHandlers[addr] != nullptr) {
				_registerHandlers[addr]->MemoryWrite(addr, value);
			}
		}

	public:
		MemoryManager(shared_ptr<BaseMapper> mapper) : _mapper(mapper) 
		{
			_internalRAM = new uint8_t[InternalRAMSize];
			_SRAM = new uint8_t[SRAMSize];
			ZeroMemory(_SRAM, SRAMSize);
			ZeroMemory(_internalRAM, InternalRAMSize);

			for(int i = 0; i < 0xFFFF; i++) {
				_registerHandlers.push_back(nullptr);
			}

			RegisterIODevice(&_ppu, 0x2000, 0x3FFF);
		}

		~MemoryManager()
		{
			delete[] _internalRAM;
			delete[] _SRAM;
		}

		uint8_t Read(uint16_t addr) {
			if(addr <= 0x1FFF) {
				return _internalRAM[addr & 0x07FF];
			} else if(addr <= 0x401F) {
				return ReadRegister(addr);
			} else if(addr <= 0x5FFF) {
				throw std::exception("Not implemented yet");
				//return ReadExpansionROM();
			} else if(addr <= 0x7FFF) {
				return _SRAM[addr];
			} else {
				return _mapper->MemoryRead(addr);
			}
		}

		void Write(uint16_t addr, uint8_t value) {
			//_writeHandler(addr);
			if(addr <= 0x1FFFF) {
				_internalRAM[addr & 0x07FF] = value;
			} else if(addr <= 0x401F) {
				WriteRegister(addr, value);
			} else if(addr <= 0x5FFF) {
				throw std::exception("Not implemented yet");
				//return ReadExpansionROM();
			} else if(addr <= 0x7FFF) {
				_SRAM[addr] = value;
			} else {
				_mapper->MemoryWrite(addr, value);
			}
		}

		uint16_t ReadWord(uint16_t addr) {
			uint8_t lo = Read(addr);
			uint8_t hi = Read(addr+1);
			return lo | hi << 8;
		}
};

