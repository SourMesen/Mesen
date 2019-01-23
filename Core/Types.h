#pragma once
#include "stdafx.h"

namespace PSFlags
{
	enum PSFlags : uint8_t
	{
		Carry = 0x01,
		Zero = 0x02,
		Interrupt = 0x04,
		Decimal = 0x08,
		Break = 0x10,
		Reserved = 0x20,
		Overflow = 0x40,
		Negative = 0x80
	};
}

enum class AddrMode
{
	None, Acc, Imp, Imm, Rel,
	Zero, Abs, ZeroX, ZeroY,
	Ind, IndX, IndY, IndYW,
	AbsX, AbsXW, AbsY, AbsYW
};

enum class IRQSource
{
	External = 1,
	FrameCounter = 2,
	DMC = 4,
	FdsDisk = 8,
};

enum class MemoryOperation
{
	Read = 1,
	Write = 2,
	Any = 3
};

enum class MemoryOperationType
{
	Read = 0,
	Write = 1,
	ExecOpCode = 2,
	ExecOperand = 3,
	PpuRenderingRead = 4,
	DummyRead = 5,
	DmcRead = 6,
	DummyWrite = 7
};

struct State
{
	uint16_t PC = 0;
	uint8_t SP = 0;
	uint8_t A = 0;
	uint8_t X = 0;
	uint8_t Y = 0;
	uint8_t PS = 0;
	uint32_t IRQFlag = 0;
	int32_t CycleCount = 0;
	bool NMIFlag = false;

	//Used by debugger
	uint16_t DebugPC = 0;
	uint16_t PreviousDebugPC = 0;
};

enum class PrgMemoryType
{
	PrgRom,
	SaveRam,
	WorkRam,
};

enum class ChrMemoryType
{
	Default,
	ChrRom,
	ChrRam,
	NametableRam
};

enum MemoryAccessType
{
	Unspecified = -1,
	NoAccess = 0x00,
	Read = 0x01,
	Write = 0x02,
	ReadWrite = 0x03
};

enum class MirroringType
{
	Horizontal,
	Vertical,
	ScreenAOnly,
	ScreenBOnly,
	FourScreens
};

struct CartridgeState
{
	uint32_t PrgRomSize;
	uint32_t ChrRomSize;
	uint32_t ChrRamSize;
	
	uint32_t PrgPageCount;
	uint32_t PrgPageSize;
	int32_t PrgMemoryOffset[0x100];
	PrgMemoryType PrgType[0x100];
	MemoryAccessType PrgMemoryAccess[0x100];

	uint32_t ChrPageCount;
	uint32_t ChrPageSize;
	int32_t ChrMemoryOffset[0x40];
	ChrMemoryType ChrType[0x40];
	MemoryAccessType ChrMemoryAccess[0x40];

	uint32_t WorkRamPageSize;
	uint32_t SaveRamPageSize;

	MirroringType Mirroring;
	bool HasBattery;
};

struct PPUControlFlags
{
	bool VerticalWrite;
	uint16_t SpritePatternAddr;
	uint16_t BackgroundPatternAddr;
	bool LargeSprites;
	bool VBlank;

	bool Grayscale;
	bool BackgroundMask;
	bool SpriteMask;
	bool BackgroundEnabled;
	bool SpritesEnabled;
	bool IntensifyRed;
	bool IntensifyGreen;
	bool IntensifyBlue;
};

struct PPUStatusFlags
{
	bool SpriteOverflow;
	bool Sprite0Hit;
	bool VerticalBlank;
};

struct PPUState
{
	uint8_t Control;
	uint8_t Mask;
	uint8_t Status;
	uint32_t SpriteRamAddr;
	uint16_t VideoRamAddr;
	uint8_t XScroll;
	uint16_t TmpVideoRamAddr;
	bool WriteToggle;

	uint16_t HighBitShift;
	uint16_t LowBitShift;
};

struct TileInfo
{
	uint8_t LowByte;
	uint8_t HighByte;
	uint32_t PaletteOffset;
	uint16_t TileAddr;
	
	int32_t AbsoluteTileAddr; //used by HD ppu
	uint8_t OffsetY; //used by HD ppu
};

struct SpriteInfo : TileInfo
{
	bool HorizontalMirror;
	bool BackgroundPriority;
	uint8_t SpriteX;

	bool VerticalMirror; //used by HD ppu
};

struct ApuLengthCounterState
{
	bool Halt;
	uint8_t Counter;
	uint8_t ReloadValue;
};

struct ApuEnvelopeState
{
	bool StartFlag;
	bool Loop;
	bool ConstantVolume;
	uint8_t Divider;
	uint8_t Counter;
	uint8_t Volume;
};

struct ApuSquareState
{
	uint8_t Duty;
	uint8_t DutyPosition;
	uint16_t Period;
	uint16_t Timer;

	bool SweepEnabled;
	bool SweepNegate;
	uint8_t SweepPeriod;
	uint8_t SweepShift;

	bool Enabled;
	uint8_t OutputVolume;
	double Frequency;

	ApuLengthCounterState LengthCounter;
	ApuEnvelopeState Envelope;
};

struct ApuTriangleState
{
	uint16_t Period;
	uint16_t Timer;
	uint8_t SequencePosition;

	bool Enabled;
	double Frequency;
	uint8_t OutputVolume;

	ApuLengthCounterState LengthCounter;
};

struct ApuNoiseState
{
	uint16_t Period;
	uint16_t Timer;
	uint16_t ShiftRegister;
	bool ModeFlag;

	bool Enabled;
	double Frequency;
	uint8_t OutputVolume;

	ApuLengthCounterState LengthCounter;
	ApuEnvelopeState Envelope;
};

struct ApuDmcState
{
	double SampleRate;
	uint16_t SampleAddr;
	uint16_t SampleLength;

	bool Loop;
	bool IrqEnabled;
	uint16_t Period;
	uint16_t Timer;
	uint16_t BytesRemaining;

	uint8_t OutputVolume;
};

struct ApuFrameCounterState
{
	bool FiveStepMode;
	uint8_t SequencePosition;
	bool IrqEnabled;
};

struct ApuState
{
	ApuSquareState Square1;
	ApuSquareState Square2;
	ApuTriangleState Triangle;
	ApuNoiseState Noise;
	ApuDmcState Dmc;
	ApuFrameCounterState FrameCounter;
};

struct MousePosition
{
	int16_t X;
	int16_t Y;
};

struct MouseMovement
{
	int16_t dx;
	int16_t dy;
};

enum class ConsoleFeatures
{
	None = 0,
	Fds = 1,
	Nsf = 2,
	VsSystem = 4,
	BarcodeReader = 8,
	TapeRecorder = 16,
	BandaiMicrophone = 32,
	DatachBarcodeReader = 64
};

enum class RecordMovieFrom
{
	StartWithoutSaveData = 0,
	StartWithSaveData,
	CurrentState
};

struct RecordMovieOptions
{
public:
	char Filename[2000] = {};
	char Author[250] = {};
	char Description[10000] = {};

	RecordMovieFrom RecordFrom = RecordMovieFrom::StartWithoutSaveData;
};

enum class GameSystem
{
	NesNtsc,
	NesPal,
	Famicom,
	Dendy,
	VsSystem,
	Playchoice,
	FDS,
	Unknown,
};

enum class BusConflictType
{
	Default = 0,
	Yes,
	No
};

struct HashInfo
{
	uint32_t Crc32 = 0;
	uint32_t PrgCrc32 = 0;
	uint32_t PrgChrCrc32 = 0;
	string Sha1;
	string PrgChrMd5;
};

enum class RomFormat
{
	Unknown = 0,
	iNes = 1,
	Unif = 2,
	Fds = 3,
	Nsf = 4,
};

enum class VsSystemType
{
	Default = 0,
	RbiBaseballProtection = 1,
	TkoBoxingProtection = 2,
	SuperXeviousProtection = 3,
	IceClimberProtection = 4,
	VsDualSystem = 5,
	RaidOnBungelingBayProtection = 6,
};

enum class GameInputType
{
	Unspecified = 0,
	StandardControllers = 1,
	FourScore = 2,
	FourPlayerAdapter = 3,
	VsSystem = 4,
	VsSystemSwapped = 5,
	VsSystemSwapAB = 6,
	VsZapper = 7,
	Zapper = 8,
	TwoZappers = 9,
	BandaiHypershot = 0x0A,
	PowerPadSideA = 0x0B,
	PowerPadSideB = 0x0C,
	FamilyTrainerSideA = 0x0D,
	FamilyTrainerSideB = 0x0E,
	ArkanoidControllerNes = 0x0F,
	ArkanoidControllerFamicom = 0x10,
	DoubleArkanoidController = 0x11,
	KonamiHyperShot = 0x12,
	PachinkoController = 0x13,
	ExcitingBoxing = 0x14,
	JissenMahjong = 0x15,
	PartyTap = 0x16,
	OekaKidsTablet = 0x17,
	BarcodeBattler = 0x18,
	MiraclePiano = 0x19, //not supported yet
	PokkunMoguraa = 0x1A, //not supported yet
	TopRider = 0x1B, //not supported yet
	DoubleFisted = 0x1C, //not supported yet
	Famicom3dSystem = 0x1D, //not supported yet
	DoremikkoKeyboard = 0x1E, //not supported yet
	ROB = 0x1F, //not supported yet
	FamicomDataRecorder = 0x20,
	TurboFile = 0x21,
	BattleBox = 0x22,
	FamilyBasicKeyboard = 0x23,
	Pec586Keyboard = 0x24, //not supported yet
	Bit79Keyboard = 0x25, //not supported yet
	SuborKeyboard = 0x26,
	SuborKeyboardMouse1 = 0x27,
	SuborKeyboardMouse2 = 0x28,
	SnesMouse = 0x29,
	GenericMulticart = 0x2A, //not supported yet
	SnesControllers = 0x2B,
	RacermateBicycle = 0x2C, //not supported yet
	UForce = 0x2D, //not supported yet
	LastEntry
};

enum class PpuModel
{
	Ppu2C02 = 0,
	Ppu2C03 = 1,
	Ppu2C04A = 2,
	Ppu2C04B = 3,
	Ppu2C04C = 4,
	Ppu2C04D = 5,
	Ppu2C05A = 6,
	Ppu2C05B = 7,
	Ppu2C05C = 8,
	Ppu2C05D = 9,
	Ppu2C05E = 10
};

enum class ConsoleId
{
	Master = 0,
	Slave = 1,
	HistoryViewer = 2
};