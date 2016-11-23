#pragma once

enum class DebuggerFlags
{
	PpuPartialDraw = 1,
	ShowEffectiveAddresses = 2,
	ShowOnlyDisassembledCode = 4
};

enum class AddressType
{
	InternalRam = 0,
	PrgRom = 1,
	WorkRam = 2,
	SaveRam = 3,
};

struct AddressTypeInfo
{
	int32_t Address;
	AddressType Type;
};