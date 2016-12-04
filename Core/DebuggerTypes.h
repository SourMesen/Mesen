#pragma once

enum class DebuggerFlags
{
	PpuPartialDraw = 1,
	ShowEffectiveAddresses = 2,
	ShowOnlyDisassembledCode = 4,
	DisplayOpCodesInLowerCase = 8
};

enum class AddressType
{
	InternalRam = 0,
	PrgRom = 1,
	WorkRam = 2,
	SaveRam = 3,
	Register = 4
};

struct AddressTypeInfo
{
	int32_t Address;
	AddressType Type;
};