#pragma once

#include "stdafx.h"
#include <atomic>
#include "../Utilities/SimpleLock.h"
#include "../Utilities/VirtualFile.h"
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
class HdPackBuilder;
class HdAudioDevice;
struct HdPackData;
enum class NesModel;
enum class ScaleFilterType;

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

		shared_ptr<HdPackBuilder> _hdPackBuilder;
		unique_ptr<HdPackData> _hdData;
		unique_ptr<HdAudioDevice> _hdAudioDevice;

		NesModel _model;

		string _romFilepath;
		string _patchFilename;

		bool _stop = false;

		bool _disableOcNextFrame = false;

		atomic<bool> _resetRequested;
		atomic<uint32_t> _lagCounter;
		
		bool _initialized = false;

		void LoadHdPack(VirtualFile &romFile, VirtualFile &patchFile);

		void ResetComponents(bool softReset);
		bool Initialize(VirtualFile &romFile, VirtualFile &patchFile);
		void UpdateNesModel(bool sendNotification);
		double GetFrameDelay();

	public:
		Console();
		~Console();
		void Run();
		void Stop();
		static void RequestReset();
		static void Reset(bool softReset = true);
		static void PowerCycle();

		//Used to pause the emu loop to perform thread-safe operations
		static void Pause();

		//Used to resume the emu loop after calling Pause()
		static void Resume();

		std::shared_ptr<Debugger> GetDebugger(bool autoStart = true);
		void StopDebugger();

		static void SaveState(ostream &saveStream);
		static void LoadState(istream &loadStream);
		static void LoadState(uint8_t *buffer, uint32_t bufferSize);

		static bool LoadROM(VirtualFile romFile, VirtualFile patchFile = {});
		static bool LoadROM(string romName, HashInfo hashInfo);
		static VirtualFile GetRomPath();
		static string GetRomName();
		static bool IsChrRam();
		static RomFormat GetRomFormat();
		static HashInfo GetHashInfo();
		static NesModel GetModel();

		static uint32_t GetLagCounter();
		static void ResetLagCounter();

		static bool IsRunning();

		static void SetNextFrameOverclockStatus(bool disabled);

		static bool IsDebuggerAttached();

		static HdPackData* GetHdData();
		static bool IsHdPpu();

		static void StartRecordingHdPack(string saveFolder, ScaleFilterType filterType, uint32_t scale, uint32_t flags, uint32_t chrRamBankSize);
		static void StopRecordingHdPack();

		static shared_ptr<Console> GetInstance();
		static void Release();
};
