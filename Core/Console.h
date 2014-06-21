#pragma once

#include "stdafx.h"
#include "CPU.h"
#include "PPU.h"
#include "BaseMapper.h"
#include "MemoryManager.h"
#include "ControlManager.h"

class Console
{
	private:
		unique_ptr<CPU> _cpu;
		unique_ptr<PPU> _ppu;
		unique_ptr<BaseMapper> _mapper;
		unique_ptr<ControlManager> _controlManager;
		unique_ptr<MemoryManager> _memoryManager;

		bool _stop = false;
		bool _stopped = false;

	public:
		Console(wstring filename);
		~Console();
		void Run();
		void Stop();
		void RunTest(bool callback(Console*));
		void Reset();
		static void RunTests();
		static void Load(wstring filename);
};
