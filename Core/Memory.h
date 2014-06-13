#include "stdafx.h"
#include "EventHandler.h"

class IMemoryHandler
{
	public:
		virtual void MemoryRead(uint16_t aa) = 0;
		virtual void MemoryWrite(uint16_t aa) = 0;
};

class MemoryManager
{
	private:
		EventHandler<IMemoryHandler, uint16_t> _readHandler;
		EventHandler<IMemoryHandler, uint16_t> _writeHandler;
		uint8_t *_memory = nullptr;
	
	public:
		MemoryManager(uint8_t *memory) : _memory(memory) { }
		EventHandler<IMemoryHandler, uint16_t> *OnMemoryRead() { return &_readHandler; }
		EventHandler<IMemoryHandler, uint16_t> *OnMemoryWrite() { return &_writeHandler; }

		uint8_t Read(uint16_t addr) {
			//_readHandler(addr);
			return _memory[addr];
		}

		uint16_t ReadWord(uint16_t addr) {
			uint8_t lo = Read(addr);
			uint8_t hi = Read(addr+1);
			return lo | hi << 8;
		}

		void Write(uint16_t addr, uint8_t value) {
			//_writeHandler(addr);
			_memory[addr] = value;
		}
};

