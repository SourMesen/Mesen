#pragma once

#include "stdafx.h"

struct ButtonState
{
	bool Up = false;
	bool Down = false;
	bool Left = false;
	bool Right = false;

	bool A = false;
	bool B = false;

	bool Select = false;
	bool Start = false;

	uint8_t ToByte()
	{
		//"Button status for each controller is returned as an 8-bit report in the following order: A, B, Select, Start, Up, Down, Left, Right."
		return (uint8_t)A | ((uint8_t)B << 1) | ((uint8_t)Select << 2) | ((uint8_t)Start << 3) |
				((uint8_t)Up << 4) | ((uint8_t)Down << 5) | ((uint8_t)Left << 6) | ((uint8_t)Right << 7);
	}

	void FromByte(uint8_t stateData)
	{
		A = (stateData & 0x01) == 0x01;
		B = (stateData & 0x02) == 0x02;
		Select = (stateData & 0x04) == 0x04;
		Start = (stateData & 0x08) == 0x08;
		Up = (stateData & 0x10) == 0x10;
		Down = (stateData & 0x20) == 0x20;
		Left = (stateData & 0x40) == 0x40;
		Right = (stateData & 0x80) == 0x80;
	}
};

class IControlDevice
{
	public:
		virtual ButtonState GetButtonState() = 0;
};