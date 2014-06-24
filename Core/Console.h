#pragma once

#include "stdafx.h"
#include "CPU.h"
#include "PPU.h"
#include "APU.h"
#include "BaseMapper.h"
#include "MemoryManager.h"
#include "ControlManager.h"

enum EmulationFlags
{
	LimitFPS = 0x01,
};

class Console
{
	private:
		static uint32_t Flags;
		static uint32_t CurrentFPS;

		unique_ptr<CPU> _cpu;
		unique_ptr<PPU> _ppu;
		unique_ptr<APU> _apu;
		shared_ptr<BaseMapper> _mapper;
		unique_ptr<ControlManager> _controlManager;
		unique_ptr<MemoryManager> _memoryManager;

		wstring _romFilename;

		bool _stop = false;
		bool _reset = false;

		void ResetComponents();

	public:
		Console(wstring filename);
		~Console();
		void Run();
		void Stop();
		void Reset();

		bool RunTest(uint8_t* expectedResult);
		void SaveTestResult();

		static bool CheckFlag(int flag);
		static void SetFlags(int flags);
		static void ClearFlags(int flags);
		static uint32_t GetFPS();
};
