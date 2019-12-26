#include "stdafx.h"
#include "BaseControlDevice.h"
#include "KeyManager.h"
#include "../Utilities/StringUtilities.h"
#include "Console.h"
#include "EmulationSettings.h"

BaseControlDevice::BaseControlDevice(shared_ptr<Console> console, uint8_t port, KeyMappingSet keyMappingSet)
{
	_console = console;
	_port = port;
	_strobe = false;
	_keyMappings = keyMappingSet.GetKeyMappingArray();
}

BaseControlDevice::~BaseControlDevice()
{
}

uint8_t BaseControlDevice::GetPort()
{
	return _port;
}

void BaseControlDevice::SetStateFromInput()
{
	ClearState();
	InternalSetStateFromInput();
}

void BaseControlDevice::InternalSetStateFromInput()
{
}

void BaseControlDevice::StreamState(bool saving)
{
	auto lock = _stateLock.AcquireSafe();
	VectorInfo<uint8_t> state{ &_state.State };
	Stream(_strobe, state);
}

bool BaseControlDevice::IsCurrentPort(uint16_t addr)
{
	return _port == (addr - 0x4016);
}

bool BaseControlDevice::IsExpansionDevice()
{
	return _port == BaseControlDevice::ExpDevicePort;
}

void BaseControlDevice::StrobeProcessRead()
{
	if(_strobe) {
		RefreshStateBuffer();
	}
}

void BaseControlDevice::StrobeProcessWrite(uint8_t value)
{
	bool prevStrobe = _strobe;
	_strobe = (value & 0x01) == 0x01;

	if(prevStrobe && !_strobe) {
		RefreshStateBuffer();
	}
}

void BaseControlDevice::ClearState()
{
	auto lock = _stateLock.AcquireSafe();
	_state = ControlDeviceState();
}

ControlDeviceState BaseControlDevice::GetRawState()
{
	auto lock = _stateLock.AcquireSafe();
	return _state;
}

void BaseControlDevice::SetRawState(ControlDeviceState state)
{
	auto lock = _stateLock.AcquireSafe();
	_state = state;
}

void BaseControlDevice::SetTextState(string textState)
{
	auto lock = _stateLock.AcquireSafe();
	ClearState();

	if(IsRawString()) {
		_state.State.insert(_state.State.end(), textState.begin(), textState.end());
	} else {
		if(HasCoordinates()) {
			vector<string> data = StringUtilities::Split(textState, ' ');
			if(data.size() >= 3) {
				MousePosition pos;
				try {
					pos.X = (int16_t)std::stol(data[0]);
					pos.Y = (int16_t)std::stol(data[1]);
				} catch(std::exception&) {
					pos.X = -1;
					pos.Y = -1;
				}
				SetCoordinates(pos);
				textState = data[2];
			}
		}

		int i = 0;
		for(char c : textState) {
			if(c != '.') {
				SetBit(i);
			}
			i++;
		}
	}
}

string BaseControlDevice::GetTextState()
{
	auto lock = _stateLock.AcquireSafe();
	if(IsRawString()) {
		return string((char*)_state.State.data(), _state.State.size());
	} else {
		string keyNames = GetKeyNames();
		string output = "";

		if(HasCoordinates()) {
			MousePosition pos = GetCoordinates();
			output += std::to_string(pos.X) + " " + std::to_string(pos.Y) + " ";
		}

		for(size_t i = 0; i < keyNames.size(); i++) {
			output += IsPressed((uint8_t)i) ? keyNames[i] : '.';
		}

		return output;
	}
}

void BaseControlDevice::EnsureCapacity(int32_t minBitCount)
{
	auto lock = _stateLock.AcquireSafe();
	uint32_t minByteCount = minBitCount / 8 + 1 + (HasCoordinates() ? 32 : 0);
	int32_t gap = minByteCount - (int32_t)_state.State.size();

	if(gap > 0) {
		_state.State.insert(_state.State.end(), gap, 0);
	}
}

bool BaseControlDevice::IsKeyboard()
{
	return false;
}

bool BaseControlDevice::HasCoordinates()
{
	return false;
}

bool BaseControlDevice::IsRawString()
{
	return false;
}

uint32_t BaseControlDevice::GetByteIndex(uint8_t bit)
{
	return bit / 8 + (HasCoordinates() ? 4 : 0);
}

bool BaseControlDevice::IsPressed(uint8_t bit)
{
	auto lock = _stateLock.AcquireSafe();
	EnsureCapacity(bit);
	uint8_t bitMask = 1 << (bit % 8);
	return (_state.State[GetByteIndex(bit)] & bitMask) != 0;
}

void BaseControlDevice::SetBitValue(uint8_t bit, bool set)
{
	if(set) {
		SetBit(bit);
	} else {
		ClearBit(bit);
	}
}

void BaseControlDevice::SetBit(uint8_t bit)
{
	auto lock = _stateLock.AcquireSafe();
	EnsureCapacity(bit);
	uint8_t bitMask = 1 << (bit % 8);
	_state.State[GetByteIndex(bit)] |= bitMask;
}

void BaseControlDevice::ClearBit(uint8_t bit)
{
	auto lock = _stateLock.AcquireSafe();
	EnsureCapacity(bit);
	uint8_t bitMask = 1 << (bit % 8);
	_state.State[GetByteIndex(bit)] &= ~bitMask;
}

void BaseControlDevice::InvertBit(uint8_t bit)
{
	if(IsPressed(bit)) {
		ClearBit(bit);
	} else {
		SetBit(bit);
	}
}

void BaseControlDevice::SetPressedState(uint8_t bit, uint32_t keyCode)
{
	if(IsKeyboard() && keyCode < 0x200 && !_console->GetSettings()->IsKeyboardMode()) {
		//Prevent keyboard device input when keyboard mode is off
		return;
	}

	if(_console->GetSettings()->InputEnabled() && (!_console->GetSettings()->IsKeyboardMode() || keyCode >= 0x200 || IsKeyboard()) && KeyManager::IsKeyPressed(keyCode)) {
		SetBit(bit);
	}
}

void BaseControlDevice::SetPressedState(uint8_t bit, bool enabled)
{
	if(enabled) {
		SetBit(bit);
	}
}

void BaseControlDevice::SetCoordinates(MousePosition pos)
{
	auto lock = _stateLock.AcquireSafe();
	EnsureCapacity(-1);

	_state.State[0] = pos.X & 0xFF;
	_state.State[1] = (pos.X >> 8) & 0xFF;
	_state.State[2] = pos.Y & 0xFF;
	_state.State[3] = (pos.Y >> 8) & 0xFF;
}

MousePosition BaseControlDevice::GetCoordinates()
{
	auto lock = _stateLock.AcquireSafe();
	EnsureCapacity(-1);

	MousePosition pos;
	pos.X = _state.State[0] | (_state.State[1] << 8);
	pos.Y = _state.State[2] | (_state.State[3] << 8);
	return pos;
}

void BaseControlDevice::SetMovement(MouseMovement mov)
{
	MouseMovement prev = GetMovement();
	mov.dx += prev.dx;
	mov.dy += prev.dy;
	SetCoordinates({ mov.dx, mov.dy });
}

MouseMovement BaseControlDevice::GetMovement()
{
	MousePosition pos = GetCoordinates();
	SetCoordinates({ 0, 0 });
	return { pos.X, pos.Y };
}

void BaseControlDevice::SwapButtons(shared_ptr<BaseControlDevice> state1, uint8_t button1, shared_ptr<BaseControlDevice> state2, uint8_t button2)
{
	bool pressed1 = state1->IsPressed(button1);
	bool pressed2 = state2->IsPressed(button2);

	state1->ClearBit(button1);
	state2->ClearBit(button2);

	if(pressed1) {
		state2->SetBit(button2);
	}
	if(pressed2) {
		state1->SetBit(button1);
	}
}
