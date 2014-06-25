#pragma once

#include "stdafx.h"
#include "IControlDevice.h"
#include "IMemoryHandler.h"

class ControlManager : public IMemoryHandler
{
	private:
		static IControlDevice* ControlDevices[4];
		bool _refreshState = false;
		uint8_t _stateBuffer[4];

		void RefreshAllPorts();
		void RefreshStateBuffer(uint8_t port);
		uint8_t GetPortValue(uint8_t port);

	public:
		ControlManager();

		static void RegisterControlDevice(IControlDevice* controlDevice, uint8_t port);

		void GetMemoryRanges(MemoryRanges &ranges)
		{
			ranges.AddHandler(MemoryType::RAM, MemoryOperation::Read, 0x4016, 0x4017);
			ranges.AddHandler(MemoryType::RAM, MemoryOperation::Write, 0x4016);
		}
		
		uint8_t ReadRAM(uint16_t addr);
		void WriteRAM(uint16_t addr, uint8_t value);
};