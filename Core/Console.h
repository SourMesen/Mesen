#include "stdafx.h"
#include "CPU.h"
#include "PPU.h"
#include "BaseMapper.h"
#include "MemoryManager.h"

class Console
{
	private:
		unique_ptr<CPU> _cpu;
		PPU _ppu;
		shared_ptr<BaseMapper> _mapper;
		MemoryManager _memoryManager;

	public:
		Console(char* filename);
		~Console();
		void Run();
		void Reset();
		static void RunBenchmark();
};
