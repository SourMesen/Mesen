#pragma once

#include "stdafx.h"
#include "CPU.h"
#include "PPU.h"
#include "APU.h"
#include "BaseMapper.h"
#include "MemoryManager.h"
#include "ControlManager.h"
#include "Debugger.h"
#include "../Utilities/SimpleLock.h"
#include "IMessageManager.h"
#include "INotificationListener.h"

enum EmulationFlags
{
	LimitFPS = 0x01,
	Paused = 0x02,
};

class Console
{
	private:
		static Console* Instance;
		static uint32_t Flags;
		static uint32_t CurrentFPS;
		static SimpleLock PauseLock;
		static SimpleLock RunningLock;
		static IMessageManager* MessageManager;
		static list<INotificationListener*> NotificationListeners;

		shared_ptr<CPU> _cpu;
		unique_ptr<PPU> _ppu;
		unique_ptr<APU> _apu;
		shared_ptr<BaseMapper> _mapper;
		unique_ptr<ControlManager> _controlManager;
		shared_ptr<MemoryManager> _memoryManager;

		wstring _romFilepath;

		bool _stop = false;
		bool _reset = false;

		void ResetComponents(bool softReset);
		void Initialize(wstring filename);

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

		shared_ptr<Debugger> Console::GetDebugger();

		static void SaveState(wstring filename);
		static void SaveState(ostream &saveStream);
		static bool LoadState(wstring filename);
		static void LoadState(istream &loadStream);
		static void LoadState(uint8_t *buffer, uint32_t bufferSize);

		static void LoadROM(wstring filename);		
		static bool AttemptLoadROM(wstring filename, uint32_t crc32Hash);
		static wstring GetROMPath();

		static bool CheckFlag(int flag);
		static void SetFlags(int flags);
		static void ClearFlags(int flags);
		static uint32_t GetFPS();

		static void RegisterMessageManager(IMessageManager* messageManager);
		static void DisplayMessage(wstring message);

		static void RegisterNotificationListener(INotificationListener* notificationListener);
		static void UnregisterNotificationListener(INotificationListener* notificationListener);
		static void SendNotification(ConsoleNotificationType type);

		static Console* GetInstance();
};
