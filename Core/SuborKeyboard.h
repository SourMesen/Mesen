#pragma once
#include "stdafx.h"
#include "BaseControlDevice.h"

class SuborKeyboard : public BaseControlDevice
{
private:
	uint8_t _row = 0;
	uint8_t _column = 0;
	bool _enabled = false;

protected:
	enum Buttons
	{
		Num4, G, F, C, F2, E, Num5, V,
		Num2, D, S, End, F1, W, Num3, X,
		Ins, Backspace, PageDown, Right, F8, PageUp, Delete, Home,
		Num9, I, L, Comma, F5, O, Num0, Dot,
		RightBracket, Enter, Up, Left, F7, LeftBracket, Backslash, Down,
		Q, CapsLock, Z, Tab, Esc, A, Num1, Ctrl,
		Num7, Y, K, M, F4, U, Num8, J,
		Minus, SemiColon, Apostrophe, Slash, F6, P, Equal, Shift,
		T, H, N, Space, F3, R, Num6, B,
		NumpadEnter, Unknown1, Unknown2, Unknown3,
		LeftMenu, Numpad4, Numpad7, F11, F12, Numpad1, Numpad2, Numpad8,
		NumpadMinus, NumpadPlus, NumpadMultiply, Numpad9, F10, Numpad5, NumpadDivide, NumLock,
		Grave, Numpad6, Pause, F9, Numpad3, NumpadDot, Numpad0
	};

	Buttons _keyboardMatrix[104] = {
		Num4, G, F, C, F2, E, Num5, V,
		Num2, D, S, End, F1, W, Num3, X,
		Ins, Backspace, PageDown, Right, F8, PageUp, Delete, Home,
		Num9, I, L, Comma, F5, O, Num0, Dot,
		RightBracket, Enter, Up, Left, F7, LeftBracket, Backslash, Down,
		Q, CapsLock, Z, Tab, Esc, A, Num1, Ctrl,
		Num7, Y, K, M, F4, U, Num8, J,
		Minus, SemiColon, Apostrophe, Slash, F6, P, Equal, Shift,
		T, H, N, Space, F3, R, Num6, B,
		Numpad6, NumpadEnter, Numpad4, Numpad8, Numpad2, Unknown1, Unknown2, Unknown3,
		LeftMenu, Numpad4, Numpad7, F11, F12, Numpad1, Numpad2, Numpad8,
		NumpadMinus, NumpadPlus, NumpadMultiply, Numpad9, F10, Numpad5, NumpadDivide, NumLock,
		Grave, Numpad6, Pause, Space, F9, Numpad3, NumpadDot, Numpad0
	};

	void InternalSetStateFromInput() override
	{
		for(KeyMapping keyMapping : _keyMappings) {
			for(int i = 0; i < 99; i++) {
				SetPressedState(i, keyMapping.SuborKeyboardButtons[i]);
			}
		}
	}

	uint8_t GetActiveKeys(uint8_t row, uint8_t column)
	{
		uint8_t result = 0;
		uint32_t baseIndex = row * 8 + (column ? 4 : 0);
		for(int i = 0; i < 4; i++) {
			if(IsPressed(_keyboardMatrix[baseIndex + i])) {
				result |= 0x10;
			}
			result >>= 1;
		}
		return result;
	}

	void StreamState(bool saving) override
	{
		BaseControlDevice::StreamState(saving);
		Stream(_row, _column, _enabled);
	}

public:
	SuborKeyboard(KeyMappingSet keyMappings) : BaseControlDevice(BaseControlDevice::ExpDevicePort, keyMappings)
	{
	}

	uint8_t ReadRAM(uint16_t addr) override
	{
		if(addr == 0x4017) {
			if(_enabled) {
				uint8_t value = ((~GetActiveKeys(_row, _column)) << 1) & 0x1E;
				MessageManager::Log(std::to_string(value));
				return value;
			} else {
				return 0x1E;
			}
		}
		return 0;
	}

	void RefreshStateBuffer()
	{
		_row = 0;
		_column = 0;
	}

	void WriteRAM(uint16_t addr, uint8_t value) override
	{
		StrobeProcessWrite(value);

		uint8_t prevColumn = _column;
		_column = (value & 0x02) >> 1;
		_enabled = (value & 0x04) != 0;

		if(_enabled) {
			if(!_column && prevColumn) {
				_row = (_row + 1) % 13;
			}
		}
	}
};