#pragma once

#include "stdafx.h"
#include "IControlDevice.h"
#include "IMemoryHandler.h"
#include "Movie.h"
#include "IGameBroadcaster.h"
#include "Snapshotable.h"

class ControlManager : public Snapshotable, public IMemoryHandler
{
	private:
		static IControlDevice* ControlDevices[4];
		static IControlDevice* OriginalControlDevices[4];
		static IGameBroadcaster* GameBroadcaster;

		bool _refreshState = false;
		uint8_t _stateBuffer[4];

		void RefreshAllPorts();
		void RefreshStateBuffer(uint8_t port);
		uint8_t GetPortValue(uint8_t port);

	protected:
		virtual void StreamState(bool saving);

	public:
		ControlManager();

		static void RegisterBroadcaster(IGameBroadcaster* gameBroadcaster);
		static void UnregisterBroadcaster(IGameBroadcaster* gameBroadcaster);

		static void BackupControlDevices();
		static void RestoreControlDevices();
		static IControlDevice* GetControlDevice(uint8_t port);
		static void RegisterControlDevice(IControlDevice* controlDevice, uint8_t port);
		static void UnregisterControlDevice(IControlDevice* controlDevice);

		void GetMemoryRanges(MemoryRanges &ranges)
		{
			ranges.AddHandler(MemoryType::RAM, MemoryOperation::Read, 0x4016, 0x4017);
			ranges.AddHandler(MemoryType::RAM, MemoryOperation::Write, 0x4016);
		}
		
		uint8_t ReadRAM(uint16_t addr);
		void WriteRAM(uint16_t addr, uint8_t value);
};