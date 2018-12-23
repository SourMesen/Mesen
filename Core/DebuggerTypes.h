#pragma once
#include "Types.h"
#include "EmulationSettings.h"

enum BreakpointType
{
	Global = 0,
	Execute = 1,
	ReadRam = 2,
	WriteRam = 3,
	ReadVram = 4,
	WriteVram = 5,
};

enum class DebuggerFlags
{
	None = 0x00,
	PpuPartialDraw = 0x01,
	PpuShowPreviousFrame = 0x02,

	ShowEffectiveAddresses = 0x04,
	DisplayOpCodesInLowerCase = 0x08,
	BreakOnBrk = 0x10,
	BreakOnUnofficialOpCode = 0x20,
	BreakOnUninitMemoryRead = 0x40,

	DisassembleVerifiedData = 0x80,
	DisassembleUnidentifiedData = 0x100,
	ShowVerifiedData = 0x200,
	ShowUnidentifiedData = 0x400,

	IgnoreRedundantWrites = 0x800,

	HidePauseIcon = 0x1000,

	BreakOnDecayedOamRead = 0x2000,
	BreakOnInit = 0x4000,
	BreakOnPlay = 0x8000,
};

enum class BreakSource
{
	Unspecified = -1,
	Breakpoint = 0,
	CpuStep = 1,
	PpuStep = 2,
	BreakOnBrk = 3,
	BreakOnUnofficialOpCode = 4,
	BreakOnReset = 5,
	BreakOnFocus = 6,
	BreakOnUninitMemoryRead = 7,
	BreakOnDecayedOamRead = 8,
	BreakOnCpuCrash = 9,
	Pause = 10,
	BreakAfterSuspend = 11,
};

enum class AddressType
{
	InternalRam = 0,
	PrgRom = 1,
	WorkRam = 2,
	SaveRam = 3,
	Register = 4
};

enum class PpuAddressType
{
	None = 0,
	ChrRom = 1,
	ChrRam = 2,
	PaletteRam = 3
};

struct AddressTypeInfo
{
	int32_t Address;
	AddressType Type;
};

struct PpuAddressTypeInfo
{
	int32_t Address;
	PpuAddressType Type;
};

enum class DebugMemoryType
{
	CpuMemory = 0,
	PpuMemory = 1,
	PaletteMemory = 2,
	SpriteMemory = 3,
	SecondarySpriteMemory = 4,
	PrgRom = 5,
	ChrRom = 6,
	ChrRam = 7,
	WorkRam = 8,
	SaveRam = 9,
	InternalRam = 10
};

enum class CdlHighlightType
{
	None = 0,
	HighlightUsed = 1,
	HighlightUnused = 2,
};

struct PPUDebugState
{
	PPUControlFlags ControlFlags;
	PPUStatusFlags StatusFlags;
	PPUState State;
	int32_t Scanline;
	uint32_t Cycle;
	uint32_t FrameCount;
	uint32_t NmiScanline;
	uint32_t ScanlineCount;
	uint32_t SafeOamScanline;
};

struct DebugState
{
	State CPU;
	PPUDebugState PPU;
	CartridgeState Cartridge;
	ApuState APU;
	NesModel Model;
	uint32_t ClockRate;
};

struct InstructionProgress
{
	uint8_t OpCode;
	uint32_t OpCycle;
	MemoryOperationType OpMemoryOperationType;
};

struct OperationInfo
{
	uint16_t Address;
	int16_t Value;
	MemoryOperationType OperationType;
};

enum class DebugEventType : uint8_t
{
	None = 0,
	PpuRegisterWrite,
	PpuRegisterRead,
	MapperRegisterWrite,
	MapperRegisterRead,
	Nmi,
	Irq,
	SpriteZeroHit,
	Breakpoint,
};

struct DebugEventInfo
{
	uint16_t Cycle;
	int16_t Scanline;
	uint16_t ProgramCounter;
	uint16_t Address;
	int16_t BreakpointId;
	DebugEventType Type;
	uint8_t Value;
	int8_t PpuLatch;
};

enum class EventType
{
	Reset = 0,
	Nmi = 1,
	Irq = 2,
	StartFrame = 3,
	EndFrame = 4,
	CodeBreak = 5,
	StateLoaded = 6,
	StateSaved = 7,
	InputPolled = 8,
	SpriteZeroHit = 9,
	ScriptEnded = 10,
	EventTypeSize
};

enum class StackFrameFlags : uint8_t
{
	None = 0,
	Nmi = 1,
	Irq = 2
};

struct StackFrameInfo
{
	int32_t JumpSourceAbsolute;
	int32_t JumpTargetAbsolute;
	uint16_t JumpSource;
	uint16_t JumpTarget;
	StackFrameFlags Flags;
};