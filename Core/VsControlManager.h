#pragma once
#include "stdafx.h"
#include "ControlManager.h"
#include "CPU.h"
#include "Console.h"
#include "VsZapper.h"
#include <assert.h>
#include "StandardController.h"

enum class VsInputType
{
	Default = 0,
	TypeA = 1,
	TypeB = 2,
	TypeC = 3,
	TypeD = 4,
	TypeE = 5
};

class VsControlManager : public ControlManager
{
private:
	static VsControlManager *_instance;
	uint8_t _prgChrSelectBit;
	uint8_t _dipSwitches = 0;
	bool _serviceButton = false;
	bool _coinInserted[2] = { };
	int32_t _coinInsertCycle[2] = { };
	VsInputType _inputType = VsInputType::Default;
	
	uint32_t _protectionCounter = 0;
	uint32_t _protectionData[3][32] = { 
		{
			0xFF, 0xBF, 0xB7, 0x97, 0x97, 0x17, 0x57, 0x4F,
			0x6F, 0x6B, 0xEB, 0xA9, 0xB1, 0x90, 0x94, 0x14,
			0x56, 0x4E, 0x6F, 0x6B, 0xEB, 0xA9, 0xB1, 0x90,
			0xD4, 0x5C, 0x3E, 0x26, 0x87, 0x83, 0x13, 0x00
		},
		{
			0x00, 0x00, 0x00, 0x00, 0xB4, 0x00, 0x00, 0x00,
			0x00, 0x6F, 0x00, 0x00, 0x00, 0x00, 0x94, 0x00,
			0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
			0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
		},
		{
			0x05, 0x01, 0x89, 0x37, 0x05, 0x00, 0xD1, 0x3E,
			0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
			0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
			0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
		}
	};

private:
	void UpdateCoinInsertedFlags()
	{
		int32_t cycle = CPU::GetCycleCount();
		for(int i = 0; i < 2; i++) {
			if(_coinInserted[i] && cycle - _coinInsertCycle[i] > 120000) {
				_coinInserted[i] = false;
			}
		}
	}

public:
	VsControlManager()
	{
		_instance = this;
	}

	virtual ~VsControlManager()
	{
		if(_instance == this) {
			_instance = nullptr;
		}
	}

	shared_ptr<BaseControlDevice> GetZapper(uint8_t port)
	{
		return shared_ptr<BaseControlDevice>(new VsZapper(port));
	}

	void Reset(bool softReset)
	{
		_protectionCounter = 0;
	}

	static VsControlManager* GetInstance()
	{
		return _instance;
	}

	void StreamState(bool saving)
	{
		ControlManager::StreamState(saving);
		Stream(_prgChrSelectBit, _protectionCounter);
	}

	void InsertCoin(uint8_t port)
	{
		assert(port < 2);

		_coinInsertCycle[port] = CPU::GetCycleCount();
		_coinInserted[port] = true;
	}

	void SetDipSwitches(uint8_t dipSwitches)
	{
		_dipSwitches = dipSwitches;
	}

	void SetInputType(VsInputType inputType)
	{
		_inputType = inputType;
	}

	void SetServiceButtonState(bool pushed)
	{
		_serviceButton = pushed;
	}

	void GetMemoryRanges(MemoryRanges &ranges)
	{
		ControlManager::GetMemoryRanges(ranges);
		ranges.AddHandler(MemoryOperation::Read, 0x4020, 0x5FFF);
		ranges.AddHandler(MemoryOperation::Write, 0x4020, 0x5FFF);
	}

	uint8_t GetPrgChrSelectBit()
	{
		return _prgChrSelectBit;
	}

	void RemapControllerButtons()
	{
		ButtonState ports[2];
		shared_ptr<StandardController> controllers[2];
		controllers[0] = std::dynamic_pointer_cast<StandardController>(GetControlDevice(0));
		controllers[1] = std::dynamic_pointer_cast<StandardController>(GetControlDevice(1));
		if(controllers[0]) {
			ports[0].FromByte(controllers[0]->GetInternalState());
		}
		if(controllers[1]) {
			ports[1].FromByte(controllers[1]->GetInternalState());
		}

		if(_inputType == VsInputType::TypeA) {
			std::swap(ports[0], ports[1]);

			std::swap(ports[0].Select, ports[0].Start);
			std::swap(ports[1].Select, ports[1].Start);
		} else if(_inputType == VsInputType::TypeB) {
			std::swap(ports[1].Select, ports[0].Start);
			std::swap(ports[1].Start, ports[0].Select);
		} else if(_inputType == VsInputType::TypeC) {
			ports[1].Select = ports[0].Start;
			ports[0].Select = false;
			ports[0].Start = false;
		} else if(_inputType == VsInputType::TypeD) {
			std::swap(ports[1].Select, ports[0].Start);
			std::swap(ports[1].Start, ports[0].Select);
			ports[0].Select = !ports[0].Select;
			ports[1].Select = !ports[1].Select;
		} else if(_inputType == VsInputType::TypeE) {
			std::swap(ports[0], ports[1]);

			std::swap(ports[0].B, ports[1].A);
			std::swap(ports[1].Select, ports[1].Start);
			std::swap(ports[0].Select, ports[0].Start);
		}

		if(controllers[0]) {
			controllers[0]->SetInternalState((controllers[0]->GetInternalState() & ~0xFF) | ports[0].ToByte());
		}
		if(controllers[1]) {
			controllers[1]->SetInternalState((controllers[1]->GetInternalState() & ~0xFF) | ports[1].ToByte());
		}
	}

	void RefreshAllPorts()
	{
		ControlManager::RefreshAllPorts();
		if(_inputType != VsInputType::Default) {
			RemapControllerButtons();
		}
	}

	uint8_t ReadRAM(uint16_t addr)
	{
		UpdateCoinInsertedFlags();

		uint8_t value = 0;
		
		uint32_t crc = Console::GetCrc32();
		
		switch(addr) {
			case 0x4016:
				value = GetPortValue(1) & 0x01;
				if(_coinInserted[0]) {
					value |= 0x20;
				}
				if(_coinInserted[1]) {
					value |= 0x40;
				}
				if(_serviceButton) {
					value |= 0x04;
				}

				value |= ((_dipSwitches & 0x01) ? 0x08 : 0x00);
				value |= ((_dipSwitches & 0x02) ? 0x10 : 0x00);
				break;
			
			case 0x4017:
				value = GetPortValue(0) & 0x01;

				value |= ((_dipSwitches & 0x04) ? 0x04 : 0x00);
				value |= ((_dipSwitches & 0x08) ? 0x08 : 0x00);
				value |= ((_dipSwitches & 0x10) ? 0x10 : 0x00);
				value |= ((_dipSwitches & 0x20) ? 0x20 : 0x00);
				value |= ((_dipSwitches & 0x40) ? 0x40 : 0x00);
				value |= ((_dipSwitches & 0x80) ? 0x80 : 0x00);
				break;

			case 0x5E00:
				_protectionCounter = 0;
				break;

			case 0x5E01:
				if(crc == 0x4A5FEE2B) {
					//TKO Boxing
					value = _protectionData[0][_protectionCounter++ & 0x1F];
				} else if(crc == 0x90584067) {
					//RBI Baseball
					value = _protectionData[1][_protectionCounter++ & 0x1F];
				}
				break;

			default:
				if(crc == 0x5B0433F3 && addr >= 0x5400 && addr <= 0x57FF) {
					//Super devious
					return _protectionData[2][_protectionCounter++ & 0x1F];
				}
				break;
		}

		return value;
	}
	
	void WriteRAM(uint16_t addr, uint8_t value)
	{
		ControlManager::WriteRAM(addr, value);

		if(addr == 0x4016) {
			_prgChrSelectBit = (value >> 2) & 0x01;
		}
	}
};