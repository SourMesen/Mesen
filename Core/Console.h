#include "stdafx.h"
#include "CPU.h"
#include "PPU.h"
#include "BaseMapper.h"
#include "MemoryManager.h"

class Console
{
	private:
		unique_ptr<CPU> _cpu;
		unique_ptr<PPU> _ppu;
		shared_ptr<BaseMapper> _mapper;
		MemoryManager _memoryManager;

	public:
		Console(string filename);
		~Console();
		void Run();
		void RunTest(bool callback(Console*));
		void Reset();
		static void RunTests();
};
