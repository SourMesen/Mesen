#pragma once

#include "stdafx.h"
#include "CPU.h"
#include "PPU.h"
#include "APU.h"
#include "MemoryManager.h"
#include "ControlManager.h"
#include "../Utilities/SimpleLock.h"

class Debugger;
class BaseMapper;

class Console
{
	private:
		static shared_ptr<Console> Instance;
		SimpleLock _pauseLock;
		SimpleLock _runLock;
		SimpleLock _stopLock;

		shared_ptr<CPU> _cpu;
		shared_ptr<PPU> _ppu;
		unique_ptr<APU> _apu;
		shared_ptr<Debugger> _debugger;
		shared_ptr<BaseMapper> _mapper;
		unique_ptr<ControlManager> _controlManager;
		shared_ptr<MemoryManager> _memoryManager;

		string _romFilepath;

		bool _stop = false;
		bool _reset = false;
		
		bool _initialized = false;

		void ResetComponents(bool softReset);
		void Initialize(string filename, stringstream *filestream = nullptr, string ipsFilename = "");
		double UpdateNesModel();

	public:
		Console();
		~Console();
		void Run();
		void Stop();
		static void Reset(bool softReset = true);

		//Used to pause the emu loop to perform thread-safe operations
		static void Pause();

		//Used to resume the emu loop after calling Pause()
		static void Resume();

		std::weak_ptr<Debugger> GetDebugger();
		void StopDebugger();

		static void SaveState(ostream &saveStream);
		static void LoadState(istream &loadStream);
		static void LoadState(uint8_t *buffer, uint32_t bufferSize);

		static void LoadROM(string filepath, stringstream *filestream = nullptr);
		static bool LoadROM(string romName, uint32_t crc32Hash);
		static void ApplyIpsPatch(string ipsFilename);
		static string GetROMPath();
		static uint32_t GetCrc32();

		static shared_ptr<Console> GetInstance();
		static void Release();
};
