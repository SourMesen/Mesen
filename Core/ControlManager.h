#pragma once

#include "stdafx.h"
#include "IControlDevice.h"
#include "IMemoryHandler.h"
#include "Movie.h"
#include "IGameBroadcaster.h"
#include "Snapshotable.h"
#include "../Utilities/SimpleLock.h"
#include "IKeyManager.h"

class ControlManager : public Snapshotable, public IMemoryHandler
{
	private:
		static unique_ptr<IKeyManager> _keyManager;
		static IControlDevice* ControlDevices[4];
		static IControlDevice* OriginalControlDevices[4];
		static IGameBroadcaster* GameBroadcaster;
		static SimpleLock ControllerLock[4];

		bool _refreshState = false;
		uint32_t _stateBuffer[2];

		void RefreshAllPorts();
		uint8_t GetControllerState(uint8_t port);
		bool HasFourScoreAdapter();
		void RefreshStateBuffer(uint8_t port);
		uint8_t GetPortValue(uint8_t port);

	protected:
		virtual void StreamState(bool saving);

	public:
		ControlManager();

		static void RegisterBroadcaster(IGameBroadcaster* gameBroadcaster);
		static void UnregisterBroadcaster(IGameBroadcaster* gameBroadcaster);

		static void RegisterKeyManager(IKeyManager* keyManager);
		static bool IsKeyPressed(uint32_t keyCode);
		static uint32_t GetPressedKey();
		static string GetKeyName(uint32_t keyCode);
		static uint32_t GetKeyCode(string keyName);

		static void BackupControlDevices();
		static void RestoreControlDevices();
		static IControlDevice* GetControlDevice(uint8_t port);
		static void RegisterControlDevice(IControlDevice* controlDevice, uint8_t port);
		static void UnregisterControlDevice(IControlDevice* controlDevice);

		void GetMemoryRanges(MemoryRanges &ranges)
		{
			ranges.AddHandler(MemoryOperation::Read, 0x4016, 0x4017);
			ranges.AddHandler(MemoryOperation::Write, 0x4016);
		}
		
		uint8_t ReadRAM(uint16_t addr);
		void WriteRAM(uint16_t addr, uint8_t value);
};