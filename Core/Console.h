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
		static Console* Instance;
		static uint32_t Flags;
		static uint32_t CurrentFPS;
		static atomic_flag PauseFlag;
		static atomic_flag RunningFlag;

		unique_ptr<CPU> _cpu;
		unique_ptr<PPU> _ppu;
		unique_ptr<APU> _apu;
		shared_ptr<BaseMapper> _mapper;
		unique_ptr<ControlManager> _controlManager;
		unique_ptr<MemoryManager> _memoryManager;

		wstring _romFilename;

		bool _stop = false;
		bool _reset = false;

		void ResetComponents(bool softReset);

	public:
		Console(wstring filename);
		~Console();
		void Run();
		void Stop();
		static void Reset();

		//Used to pause the emu loop to perform thread-safe operations
		static void Pause();

		//Used to resume the emu loop after calling Pause()
		static void Resume();

		bool RunTest(uint8_t* expectedResult);
		void SaveTestResult();

		static void SaveState(wstring filename);
		static void SaveState(ostream &saveStream);
		static bool LoadState(wstring filename);
		static void LoadState(istream &loadStream);

		static bool CheckFlag(int flag);
		static void SetFlags(int flags);
		static void ClearFlags(int flags);
		static uint32_t GetFPS();
};
