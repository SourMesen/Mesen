#include "stdafx.h"
#include "VsControlManager.h"

VsControlManager *VsControlManager::_instance = nullptr;

VsControlManager::VsControlManager(shared_ptr<BaseControlDevice> systemActionManager, shared_ptr<BaseControlDevice> mapperControlDevice) : ControlManager(systemActionManager, mapperControlDevice)
{
	_instance = this;
}

VsControlManager::~VsControlManager()
{
	if(_instance == this) {
		_instance = nullptr;
	}
}

ControllerType VsControlManager::GetControllerType(uint8_t port)
{
	ControllerType type = ControlManager::GetControllerType(port);
	if(type == ControllerType::Zapper) {
		type = ControllerType::VsZapper;
	}
	return type;
}

void VsControlManager::Reset(bool softReset)
{
	_protectionCounter = 0;
}

VsControlManager* VsControlManager::GetInstance()
{
	return _instance;
}

void VsControlManager::StreamState(bool saving)
{
	ControlManager::StreamState(saving);
	Stream(_prgChrSelectBit, _protectionCounter, _refreshState, _inputType);
}

void VsControlManager::SetInputType(VsInputType inputType)
{
	_inputType = inputType;
}

void VsControlManager::GetMemoryRanges(MemoryRanges &ranges)
{
	ControlManager::GetMemoryRanges(ranges);
	ranges.AddHandler(MemoryOperation::Read, 0x4020, 0x5FFF);
	ranges.AddHandler(MemoryOperation::Write, 0x4020, 0x5FFF);
}

uint8_t VsControlManager::GetPrgChrSelectBit()
{
	return _prgChrSelectBit;
}

void VsControlManager::RemapControllerButtons()
{
	shared_ptr<StandardController> controllers[2];
	controllers[0] = std::dynamic_pointer_cast<StandardController>(GetControlDevice(0));
	controllers[1] = std::dynamic_pointer_cast<StandardController>(GetControlDevice(1));

	if(!controllers[0] || !controllers[1]) {
		return;
	}

	if(_inputType == VsInputType::TypeA) {
		BaseControlDevice::SwapButtons(controllers[0], StandardController::Buttons::Select, controllers[0], StandardController::Buttons::Start);
		BaseControlDevice::SwapButtons(controllers[1], StandardController::Buttons::Select, controllers[1], StandardController::Buttons::Start);
	} else if(_inputType == VsInputType::TypeB) {
		std::swap(controllers[0], controllers[1]);
		BaseControlDevice::SwapButtons(controllers[1], StandardController::Buttons::Select, controllers[0], StandardController::Buttons::Start);
		BaseControlDevice::SwapButtons(controllers[0], StandardController::Buttons::Select, controllers[1], StandardController::Buttons::Start);
	} else if(_inputType == VsInputType::TypeC) {
		std::swap(controllers[0], controllers[1]);

		if(controllers[0]->IsPressed(StandardController::Buttons::Start)) {
			controllers[1]->SetBit(StandardController::Buttons::Select);
		} else {
			controllers[1]->ClearBit(StandardController::Buttons::Select);
		}

		controllers[0]->ClearBit(StandardController::Buttons::Start);
		controllers[0]->ClearBit(StandardController::Buttons::Select);
	} else if(_inputType == VsInputType::TypeD) {
		std::swap(controllers[0], controllers[1]);
		BaseControlDevice::SwapButtons(controllers[1], StandardController::Buttons::Select, controllers[0], StandardController::Buttons::Start);
		BaseControlDevice::SwapButtons(controllers[0], StandardController::Buttons::Select, controllers[1], StandardController::Buttons::Start);
		controllers[0]->InvertBit(StandardController::Buttons::Select);
		controllers[1]->InvertBit(StandardController::Buttons::Select);
	} else if(_inputType == VsInputType::TypeE) {
		BaseControlDevice::SwapButtons(controllers[0], StandardController::Buttons::B, controllers[1], StandardController::Buttons::A);
		BaseControlDevice::SwapButtons(controllers[0], StandardController::Buttons::Select, controllers[0], StandardController::Buttons::Start);
		BaseControlDevice::SwapButtons(controllers[1], StandardController::Buttons::Select, controllers[1], StandardController::Buttons::Start);
	}
}

uint8_t VsControlManager::ReadRAM(uint16_t addr)
{
	uint8_t value = 0;

	uint32_t crc = Console::GetHashInfo().PrgCrc32Hash;

	switch(addr) {
		case 0x4016: {
			uint32_t dipSwitches = EmulationSettings::GetDipSwitches();
			value = ControlManager::ReadRAM(addr);
			value |= ((dipSwitches & 0x01) ? 0x08 : 0x00);
			value |= ((dipSwitches & 0x02) ? 0x10 : 0x00);
			break;
		}

		case 0x4017: {
			value = ControlManager::ReadRAM(addr) & 0x01;

			uint32_t dipSwitches = EmulationSettings::GetDipSwitches();
			value |= ((dipSwitches & 0x04) ? 0x04 : 0x00);
			value |= ((dipSwitches & 0x08) ? 0x08 : 0x00);
			value |= ((dipSwitches & 0x10) ? 0x10 : 0x00);
			value |= ((dipSwitches & 0x20) ? 0x20 : 0x00);
			value |= ((dipSwitches & 0x40) ? 0x40 : 0x00);
			value |= ((dipSwitches & 0x80) ? 0x80 : 0x00);
			break;
		}

		case 0x5E00:
			_protectionCounter = 0;
			break;

		case 0x5E01:
			if(crc == 0xEB2DBA63 || crc == 0x98CFE016) {
				//TKO Boxing
				value = _protectionData[0][_protectionCounter++ & 0x1F];
			} else if(crc == 0x135ADF7C) {
				//RBI Baseball
				value = _protectionData[1][_protectionCounter++ & 0x1F];
			}
			break;

		default:
			if((crc == 0xF9D3B0A3 || crc == 0x66BB838F || crc == 0x9924980A) && addr >= 0x5400 && addr <= 0x57FF) {
				//Super devious
				return _protectionData[2][_protectionCounter++ & 0x1F];
			}
			break;
	}

	return value;
}

void VsControlManager::WriteRAM(uint16_t addr, uint8_t value)
{
	ControlManager::WriteRAM(addr, value);

	bool previousState = _refreshState;
	_refreshState = (value & 0x01) == 0x01;

	if(previousState && !_refreshState) {
		RemapControllerButtons();
	}

	if(addr == 0x4016) {
		_prgChrSelectBit = (value >> 2) & 0x01;
	}
}
