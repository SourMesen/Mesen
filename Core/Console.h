#pragma once

#include "stdafx.h"
#include <atomic>
#include "../Utilities/SimpleLock.h"
#include "VirtualFile.h"

class BaseMapper;
class RewindManager;
class HistoryViewer;
class APU;
class CPU;
class PPU;
class MemoryManager;
class ControlManager;
class AutoSaveManager;
class HdPackBuilder;
class HdAudioDevice;
class SystemActionManager;
class Timer;
class CheatManager;
class SaveStateManager;
class VideoDecoder;
class VideoRenderer;
class DebugHud;
class SoundMixer;
class NotificationManager;
class Debugger;
class EmulationSettings;
class BatteryManager;

struct HdPackData;
struct HashInfo;
struct RomInfo;

enum class MemoryOperationType;
enum class NesModel;
enum class ScaleFilterType;
enum class ConsoleFeatures;
enum class DebugMemoryType;
enum class EventType;
enum class DebugEventType : uint8_t;
enum class RamPowerOnState;

class Console : public std::enable_shared_from_this<Console>
{
private:
	SimpleLock _runLock;
	SimpleLock _stopLock;
	SimpleLock _debuggerLock;
	atomic<uint32_t> _pauseCounter;

	shared_ptr<RewindManager> _rewindManager;
	shared_ptr<HistoryViewer> _historyViewer;

	shared_ptr<CPU> _cpu;
	shared_ptr<PPU> _ppu;
	shared_ptr<APU> _apu;
	shared_ptr<Debugger> _debugger;
	shared_ptr<BaseMapper> _mapper;
	shared_ptr<ControlManager> _controlManager;
	shared_ptr<MemoryManager> _memoryManager;
	
	//Used by VS-DualSystem
	shared_ptr<Console> _master;
	shared_ptr<Console> _slave;
	
	shared_ptr<BatteryManager> _batteryManager;
	shared_ptr<SystemActionManager> _systemActionManager;

	shared_ptr<VideoDecoder> _videoDecoder;
	shared_ptr<VideoRenderer> _videoRenderer;
	unique_ptr<AutoSaveManager> _autoSaveManager;
	shared_ptr<SaveStateManager> _saveStateManager;
	shared_ptr<CheatManager> _cheatManager;
	shared_ptr<DebugHud> _debugHud;
	shared_ptr<SoundMixer> _soundMixer;
	shared_ptr<NotificationManager> _notificationManager;
	shared_ptr<EmulationSettings> _settings;

	shared_ptr<HdPackBuilder> _hdPackBuilder;
	shared_ptr<HdPackData> _hdData;
	unique_ptr<HdAudioDevice> _hdAudioDevice;

	NesModel _model;

	string _romFilepath;
	string _patchFilename;

	bool _paused = false;
	bool _stop = false;
	bool _running = false;
	int32_t _stopCode = 0;

	bool _pauseOnNextFrameRequested = false;
	bool _resetRunTimers = false;

	bool _disableOcNextFrame = false;

	bool _initialized = false;
	std::thread::id _emulationThreadId;

	void LoadHdPack(VirtualFile &romFile, VirtualFile &patchFile);

	void UpdateNesModel(bool sendNotification);
	double GetFrameDelay();
	void DisplayDebugInformation(double lastFrame, double &lastFrameMin, double &lastFrameMax, double frameDurations[60]);

	void ExportStub();

public:
	Console(shared_ptr<Console> master = nullptr, EmulationSettings* initialSettings = nullptr);
	~Console();

	void Init();
	void Release(bool forShutdown);

	shared_ptr<BatteryManager> GetBatteryManager();
	shared_ptr<SaveStateManager> GetSaveStateManager();
	shared_ptr<VideoDecoder> GetVideoDecoder();
	shared_ptr<VideoRenderer> GetVideoRenderer();
	shared_ptr<DebugHud> GetDebugHud();
	shared_ptr<SoundMixer> GetSoundMixer();
	shared_ptr<NotificationManager> GetNotificationManager();
	EmulationSettings* GetSettings();
	
	bool IsDualSystem();
	shared_ptr<Console> GetDualConsole();
	bool IsMaster();

	void ProcessCpuClock();
	CPU* GetCpu();
	PPU* GetPpu();
	APU* GetApu();
	BaseMapper* GetMapper();
	ControlManager* GetControlManager();
	MemoryManager* GetMemoryManager();
	CheatManager* GetCheatManager();
	shared_ptr<RewindManager> GetRewindManager();
	HistoryViewer* GetHistoryViewer();

	bool LoadMatchingRom(string romName, HashInfo hashInfo);
	string FindMatchingRom(string romName, HashInfo hashInfo);

	bool Initialize(string romFile, string patchFile = "");
	bool Initialize(VirtualFile &romFile);
	bool Initialize(VirtualFile &romFile, VirtualFile &patchFile, bool forPowerCycle = false);

	void SaveBatteries();

	void Run();
	void ResetRunTimers();
	void Stop(int stopCode = 0);
		
	int32_t GetStopCode();
		
	void RunSingleFrame();
	void RunSlaveCpu();
	bool UpdateHdPackMode();

	shared_ptr<SystemActionManager> GetSystemActionManager();

	template<typename T>
	shared_ptr<T> GetSystemActionManager()
	{
		return std::dynamic_pointer_cast<T>(_systemActionManager);
	}

	uint32_t GetDipSwitchCount();
	ConsoleFeatures GetAvailableFeatures();
	void InputBarcode(uint64_t barcode, uint32_t digitCount);

	void LoadTapeFile(string filepath);
	void StartRecordingTapeFile(string filepath);
	void StopRecordingTapeFile();
	bool IsRecordingTapeFile();
	bool IsNsf();
		
	std::thread::id GetEmulationThreadId();

	void Reset(bool softReset = true);
	void PowerCycle();
	void ReloadRom(bool forPowerCycle = false);
	void ResetComponents(bool softReset);

	//Used to pause the emu loop to perform thread-safe operations
	void Pause();

	//Used to resume the emu loop after calling Pause()
	void Resume();

	shared_ptr<Debugger> GetDebugger(bool autoStart = true);
	void StopDebugger();

	void SaveState(ostream &saveStream);
	void LoadState(istream &loadStream);
	void LoadState(istream &loadStream, uint32_t stateVersion);
	void LoadState(uint8_t *buffer, uint32_t bufferSize);

	VirtualFile GetRomPath();
	VirtualFile GetPatchFile();
	RomInfo GetRomInfo();
	uint32_t GetFrameCount();
	NesModel GetModel();

	uint32_t GetLagCounter();
	void ResetLagCounter();

	bool IsRunning();
	bool IsExecutionStopped();

	bool IsPaused();
	void PauseOnNextFrame();

	void SetNextFrameOverclockStatus(bool disabled);

	bool IsDebuggerAttached();

	void InitializeRam(void* data, uint32_t length);
	static void InitializeRam(RamPowerOnState powerOnState, void* data, uint32_t length);

	shared_ptr<HdPackData> GetHdData();
	bool IsHdPpu();

	void StartRecordingHdPack(string saveFolder, ScaleFilterType filterType, uint32_t scale, uint32_t flags, uint32_t chrRamBankSize);
	void StopRecordingHdPack();
	
	void CopyRewindData(shared_ptr<Console> sourceConsole);

	uint8_t* GetRamBuffer(DebugMemoryType memoryType, uint32_t &size, int32_t &startAddr);
		
	void DebugAddTrace(const char *log);
	void DebugProcessPpuCycle();
	void DebugProcessEvent(EventType type);
	void DebugProcessInterrupt(uint16_t cpuAddr, uint16_t destCpuAddr, bool forNmi);
	void DebugSetLastFramePpuScroll(uint16_t addr, uint8_t xScroll, bool updateHorizontalScrollOnly);
	void DebugAddDebugEvent(DebugEventType type);
	bool DebugProcessRamOperation(MemoryOperationType type, uint16_t &addr, uint8_t &value);
	void DebugProcessVramReadOperation(MemoryOperationType type, uint16_t addr, uint8_t &value);
	void DebugProcessVramWriteOperation(uint16_t addr, uint8_t &value);
};
