#pragma once

#include "stdafx.h"
#include <atomic>
#include "../Utilities/SimpleLock.h"
#include "RomData.h"

class Debugger;
class BaseMapper;
class RewindManager;
class APU;
class CPU;
class PPU;
class MemoryManager;
class ControlManager;
class AutoSaveManager;
enum class NesModel;

class Console
{
	private:
		static shared_ptr<Console> Instance;
		SimpleLock _pauseLock;
		SimpleLock _runLock;
		SimpleLock _stopLock;

		shared_ptr<RewindManager> _rewindManager;
		shared_ptr<CPU> _cpu;
		shared_ptr<PPU> _ppu;
		unique_ptr<APU> _apu;
		shared_ptr<Debugger> _debugger;
		SimpleLock _debuggerLock;
		shared_ptr<BaseMapper> _mapper;
		unique_ptr<ControlManager> _controlManager;
		shared_ptr<MemoryManager> _memoryManager;

		unique_ptr<AutoSaveManager> _autoSaveManager;

		NesModel _model;

		string _romFilepath;

		bool _stop = false;

		bool _disableOcNextFrame = false;

		atomic<bool> _resetRequested;
		atomic<uint32_t> _lagCounter;
		
		bool _initialized = false;

		void ResetComponents(bool softReset);
		bool Initialize(string filename, stringstream *filestream = nullptr, string patchFilename = "", int32_t archiveFileIndex = -1);
		void UpdateNesModel(bool sendNotification);
		double GetFrameDelay();

	public:
		Console();
		~Console();
		void Run();
		void Stop();
		static void RequestReset();
		static void Reset(bool softReset = true);

		//Used to pause the emu loop to perform thread-safe operations
		static void Pause();

		//Used to resume the emu loop after calling Pause()
		static void Resume();

		std::shared_ptr<Debugger> GetDebugger(bool autoStart = true);
		void StopDebugger();

		static NesModel GetNesModel();
		static void SaveState(ostream &saveStream);
		static void LoadState(istream &loadStream);
		static void LoadState(uint8_t *buffer, uint32_t bufferSize);

		static bool LoadROM(string filepath, stringstream *filestream = nullptr, int32_t archiveFileIndex = -1, string patchFilepath = "");
		static bool LoadROM(string romName, HashInfo hashInfo);
		static bool LoadROM(string romName, uint32_t crc32Hash);
		static bool LoadROM(string romName, string sha1Hash);
		static string GetROMPath();
		static string GetRomName();
		static RomFormat GetRomFormat();
		static uint32_t GetCrc32();
		static uint32_t GetPrgCrc32();
		static NesModel GetModel();

		static uint32_t GetLagCounter();
		static void ResetLagCounter();

		static bool IsRunning();

		static void DisableOcNextFrame();

		static bool IsDebuggerAttached();

		static shared_ptr<Console> GetInstance();
		static void Release();
};
