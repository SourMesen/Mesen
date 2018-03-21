#pragma once

#include <algorithm>
#include "stdafx.h"
#include "MessageManager.h"
#include "GameClient.h"
#include "KeyManager.h"
#include "../Utilities/SimpleLock.h"

enum EmulationFlags : uint64_t
{
	Paused = 0x01,
	ShowFPS = 0x02,
	VerticalSync = 0x04,
	AllowInvalidInput = 0x08,
	RemoveSpriteLimit = 0x10,
	UseHdPacks = 0x20,
	HasFourScore = 0x40,

	PauseOnMovieEnd = 0x0100,
	PauseWhenInBackground = 0x0200,
	AllowBackgroundInput = 0x0400,
	ReduceSoundInBackground = 0x0800,
	MuteSoundInBackground = 0x1000,

	FdsFastForwardOnLoad = 0x2000,
	FdsAutoLoadDisk = 0x4000,
	Mmc3IrqAltBehavior = 0x8000,
	
	SwapDutyCycles = 0x10000,

	DisableGameDatabase = 0x20000,
	AutoConfigureInput = 0x40000,

	ShowLagCounter = 0x80000,

	SilenceTriangleHighFreq = 0x100000,
	ReduceDmcPopping = 0x200000,

	DisableBackground = 0x400000,
	DisableSprites = 0x800000,
	ForceBackgroundFirstColumn = 0x1000000,
	ForceSpritesFirstColumn = 0x2000000,
	DisablePpu2004Reads = 0x4000000,
	DisableNoiseModeFlag = 0x8000000,
	DisablePaletteRead = 0x10000000,
	DisableOamAddrBug = 0x20000000,
	DisablePpuReset = 0x40000000,
	EnableOamDecay = 0x80000000,

	UseNes101Hvc101Behavior = 0x100000000,
	ShowFrameCounter = 0x200000000,
	ShowGameTimer = 0x400000000,

	FdsAutoInsertDisk = 0x800000000,

	Turbo = 0x2000000000,
	InBackground = 0x4000000000,
	NsfPlayerEnabled = 0x8000000000,
	
	DisplayMovieIcons = 0x10000000000,
	HidePauseOverlay = 0x20000000000,

	UseCustomVsPalette = 0x40000000000,

	AdaptiveSpriteLimit = 0x80000000000,

	DisableOsd = 0x100000000000,
	DisableGameSelectionScreen = 0x200000000000,

	ConfirmExitResetPower = 0x400000000000,

	NsfRepeat = 0x800000000000,
	NsfShuffle = 0x1000000000000,

	IntegerFpsMode = 0x2000000000000,

	DebuggerWindowEnabled = 0x4000000000000,
	BreakOnCrash = 0x8000000000000,
	
	AllowMismatchingSaveState = 0x10000000000000,

	ForceMaxSpeed = 0x4000000000000000,	
	ConsoleMode = 0x8000000000000000,
};

enum class AudioChannel
{
	Square1 = 0,
	Square2 = 1,
	Triangle = 2,
	Noise = 3,
	DMC = 4,
	FDS = 5,
	MMC5 = 6,
	VRC6 = 7,
	VRC7 = 8,
	Namco163 = 9,
	Sunsoft5B = 10
};

enum class EqualizerFilterType
{
	None = 0,
	Butterworth = 1,
	Chebyshev1 = 2,
	Chebyshev2 = 3
};

enum class ScaleFilterType
{
	xBRZ = 0,
	HQX = 1,
	Scale2x = 2,
	_2xSai = 3,
	Super2xSai = 4,
	SuperEagle = 5,
	Prescale = 6,
};

enum class VideoFilterType
{
	None = 0,
	NTSC = 1,
	BisqwitNtscQuarterRes = 2,
	BisqwitNtscHalfRes = 3,
	BisqwitNtsc = 4,
	xBRZ2x = 5,
	xBRZ3x = 6,
	xBRZ4x = 7,
	xBRZ5x = 8,
	xBRZ6x = 9,
	HQ2x = 10,
	HQ3x = 11,
	HQ4x = 12,
	Scale2x = 13,
	Scale3x = 14,
	Scale4x = 15,
	_2xSai = 16,
	Super2xSai = 17,
	SuperEagle = 18,
	Prescale2x = 19,
	Prescale3x = 20,
	Prescale4x = 21,
	Prescale6x = 22,
	Prescale8x = 23,
	Prescale10x = 24,
	HdPack = 999
};

enum class VideoResizeFilter
{
	NearestNeighbor = 0,
	Bilinear = 1
};

enum class VideoAspectRatio
{
	NoStretching = 0,
	Auto = 1,
	NTSC = 2,
	PAL = 3,
	Standard = 4,
	Widescreen = 5,
	Custom = 6
};

struct OverscanDimensions
{
	uint32_t Left = 0;
	uint32_t Right = 0;
	uint32_t Top = 0;
	uint32_t Bottom = 0;

	uint32_t GetPixelCount()
	{
		return GetScreenWidth() * GetScreenHeight();
	}

	uint32_t GetScreenWidth()
	{
		return 256 - Left - Right;
	}

	uint32_t GetScreenHeight()
	{
		return 240 - Top - Bottom;
	}
};

struct PictureSettings
{
	double Brightness = 0;
	double Contrast = 0;
	double Saturation = 0;
	double Hue = 0;
	double ScanlineIntensity = 0;
};

struct NtscFilterSettings
{
	double Sharpness = 0;
	double Gamma = 0;
	double Resolution = 0;
	double Artifacts = 0;
	double Fringing = 0;
	double Bleed = 0;
	bool MergeFields = false;
	bool VerticalBlend = false;
	bool KeepVerticalResolution = false;

	double YFilterLength = 0;
	double IFilterLength = 0;
	double QFilterLength = 0;
};

enum class RamPowerOnState
{
	AllZeros = 0,
	AllOnes = 1,
	Random = 2
};

extern const vector<string> NesModelNames;
enum class NesModel
{
	Auto = 0,
	NTSC = 1,
	PAL = 2,
	Dendy = 3,
};

extern const vector<string> ConsoleTypeNames;
enum class ConsoleType
{
	Nes = 0,
	Famicom = 1
};

extern const vector<string> ControllerTypeNames;
enum class ControllerType
{
	None = 0,
	StandardController = 1,
	Zapper = 2,
	ArkanoidController = 3,
	SnesController = 4,
	PowerPad = 5,
	SnesMouse = 6,
	SuborMouse = 7,
	VsZapper = 8
};

extern const vector<string> ExpansionPortDeviceNames;
enum class ExpansionPortDevice
{
	None = 0,
	Zapper = 1,
	FourPlayerAdapter = 2,
	ArkanoidController = 3,
	OekaKidsTablet = 4,
	FamilyTrainerMat = 5,
	KonamiHyperShot = 6,
	FamilyBasicKeyboard = 7,
	PartyTap = 8,
	Pachinko = 9,
	ExcitingBoxing = 10,
	JissenMahjong = 11,
	SuborKeyboard = 12,
	BarcodeBattler = 13,
	HoriTrack = 14,
	BandaiHyperShot = 15,
	AsciiTurboFile = 16,
	BattleBox = 17,
};

extern const vector<string> PpuModelNames;
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

enum class VsInputType
{
	Default = 0,
	TypeA = 1,
	TypeB = 2,
	TypeC = 3,
	TypeD = 4,
	TypeE = 5
};

struct KeyMapping
{
	uint32_t A = 0;
	uint32_t B = 0;
	uint32_t Up = 0;
	uint32_t Down = 0;
	uint32_t Left = 0;
	uint32_t Right = 0;
	uint32_t Start = 0;
	uint32_t Select = 0;
	uint32_t TurboA = 0;
	uint32_t TurboB = 0;
	uint32_t TurboStart = 0;
	uint32_t TurboSelect = 0;
	uint32_t Microphone = 0;
	uint32_t LButton = 0;
	uint32_t RButton = 0;

	uint32_t PowerPadButtons[12] = {};
	uint32_t FamilyBasicKeyboardButtons[72] = {};
	uint32_t PartyTapButtons[6] = {};
	uint32_t PachinkoButtons[2] = {};
	uint32_t ExcitingBoxingButtons[8] = {};
	uint32_t JissenMahjongButtons[21] = {};
	uint32_t SuborKeyboardButtons[99] = {};
	uint32_t BandaiMicrophoneButtons[3] = {};

	bool HasKeySet()
	{
		if(A || B || Up || Down || Left || Right || Start || Select || TurboA || TurboB || TurboStart || TurboSelect || Microphone || LButton || RButton) {
			return true;
		}

		bool hasKeyBinding = false;
		hasKeyBinding |= HasKeyBinding(PowerPadButtons, sizeof(PowerPadButtons) / sizeof(PowerPadButtons[0]));
		hasKeyBinding |= HasKeyBinding(FamilyBasicKeyboardButtons, sizeof(FamilyBasicKeyboardButtons) / sizeof(FamilyBasicKeyboardButtons[0]));
		hasKeyBinding |= HasKeyBinding(PartyTapButtons, sizeof(PartyTapButtons) / sizeof(PartyTapButtons[0]));
		hasKeyBinding |= HasKeyBinding(PachinkoButtons, sizeof(PachinkoButtons) / sizeof(PachinkoButtons[0]));
		hasKeyBinding |= HasKeyBinding(ExcitingBoxingButtons, sizeof(ExcitingBoxingButtons) / sizeof(ExcitingBoxingButtons[0]));
		hasKeyBinding |= HasKeyBinding(JissenMahjongButtons, sizeof(JissenMahjongButtons) / sizeof(JissenMahjongButtons[0]));
		hasKeyBinding |= HasKeyBinding(SuborKeyboardButtons, sizeof(SuborKeyboardButtons) / sizeof(SuborKeyboardButtons[0]));
		hasKeyBinding |= HasKeyBinding(BandaiMicrophoneButtons, sizeof(BandaiMicrophoneButtons) / sizeof(BandaiMicrophoneButtons[0]));
		return hasKeyBinding;
	}

private:
	bool HasKeyBinding(uint32_t* buttons, uint32_t count)
	{
		for(uint32_t i = 0; i < count; i++) {
			if(buttons[i] != 0) {
				return true;
			}
		}
		return false;
	}
};

struct KeyMappingSet
{
	KeyMapping Mapping1;
	KeyMapping Mapping2;
	KeyMapping Mapping3;
	KeyMapping Mapping4;
	uint32_t TurboSpeed = 0;
	bool PowerpadUseSideA = false;

	vector<KeyMapping> GetKeyMappingArray()
	{
		vector<KeyMapping> keyMappings;
		if(Mapping1.HasKeySet()) {
			keyMappings.push_back(Mapping1);
		}
		if(Mapping2.HasKeySet()) {
			keyMappings.push_back(Mapping2);
		}
		if(Mapping3.HasKeySet()) {
			keyMappings.push_back(Mapping3);
		}
		if(Mapping4.HasKeySet()) {
			keyMappings.push_back(Mapping4);
		}
		return keyMappings;
	}
};

enum class EmulatorShortcut
{
	FastForward,
	Rewind,
	RewindTenSecs,
	RewindOneMin,

	MoveToNextStateSlot,
	MoveToPreviousStateSlot,
	SaveState,
	LoadState,

	InsertNextDisk,
	VsServiceButton,

	ToggleCheats,
	ToggleAudio,
	ToggleFastForward,
	ToggleRewind,
	ToggleKeyboardMode,

	RunSingleFrame,

	// Everything below this is handled UI-side
	SwitchDiskSide,
	EjectDisk,

	InsertCoin1,
	InsertCoin2,
	
	InputBarcode,

	TakeScreenshot,

	IncreaseSpeed,
	DecreaseSpeed,
	MaxSpeed,

	Pause,
	Reset,
	PowerCycle,
	PowerOff,
	Exit,

	SetScale1x,
	SetScale2x,
	SetScale3x,
	SetScale4x,
	SetScale5x,
	SetScale6x,
	ToggleFullscreen,
	ToggleFps,
	ToggleGameTimer,
	ToggleFrameCounter,
	ToggleLagCounter,
	ToggleOsd,
	ToggleAlwaysOnTop,
	ToggleSprites,
	ToggleBackground,

	LoadRandomGame,
	SaveStateSlot1,
	SaveStateSlot2,
	SaveStateSlot3,
	SaveStateSlot4,
	SaveStateSlot5,
	SaveStateSlot6,
	SaveStateSlot7,
	SaveStateToFile,

	LoadStateSlot1,
	LoadStateSlot2,
	LoadStateSlot3,
	LoadStateSlot4,
	LoadStateSlot5,
	LoadStateSlot6,
	LoadStateSlot7,
	LoadStateSlot8,
	LoadStateFromFile,

	OpenFile,
	ShortcutCount
};

struct KeyCombination
{
	uint32_t Key1 = 0;
	uint32_t Key2 = 0;
	uint32_t Key3 = 0;

	vector<uint32_t> GetKeys()
	{
		vector<uint32_t> result;
		if(Key1) {
			result.push_back(Key1);
		}
		if(Key2) {
			result.push_back(Key2);
		}
		if(Key3) {
			result.push_back(Key3);
		}
		return result;
	}

	bool IsSubsetOf(KeyCombination keyCombination)
	{
		vector<uint32_t> myKeys = GetKeys();
		vector<uint32_t> otherKeys = keyCombination.GetKeys();

		if(otherKeys.size() > myKeys.size()) {
			for(size_t i = 0; i < myKeys.size(); i++) {
				if(std::find(otherKeys.begin(), otherKeys.end(), myKeys[i]) == otherKeys.end()) {
					//Current key combination contains a key not found in the other combination, so it's not a subset
					return false;
				}
			}
			return true;
		}
		return false;
	}
};

enum class Language
{
	SystemDefault = 0,
	English = 1,
	French = 2,
	Japanese = 3,
	Russian = 4,
	Spanish = 5,
	Ukrainian = 6,
	Portuguese = 7,
	Catalan = 8
};

enum class MouseDevice
{
	Unknown = 0,
	SnesMouse,
	SuborMouse,
	ArkanoidController,
	HoriTrack
};

enum class StereoFilter
{
	None = 0,
	Delay = 1,
	Panning = 2,
};

enum class InputDisplayPosition
{
	TopLeft = 0,
	TopRight = 1,
	BottomLeft = 2,
	BottomRight = 3
};

struct InputDisplaySettings
{
	uint8_t VisiblePorts;
	InputDisplayPosition DisplayPosition;
	bool DisplayHorizontally;
};

class EmulationSettings
{
private:
	static uint16_t _versionMajor;
	static uint8_t _versionMinor;
	static uint8_t _versionRevision;

	static const vector<uint32_t> _speedValues;

	static uint32_t _ppuPaletteArgb[11][64];
	static uint32_t _defaultPpuPalette[64];
	static uint32_t _currentPalette[64];
	static uint8_t _paletteLut[11][64];

	static uint64_t _flags;

	static Language _displayLanguage;

	static bool _audioSettingsChanged;
	static uint32_t _audioLatency;
	static double _channelVolume[11];
	static double _channelPanning[11];
	static EqualizerFilterType _equalizerFilterType;
	static vector<double> _bandGains;
	static vector<double> _bands;
	static double _masterVolume;
	static uint32_t _sampleRate;
	static StereoFilter _stereoFilter;
	static int32_t _stereoDelay;
	static double _stereoAngle;
	static double _reverbStrength;
	static double _reverbDelay;
	static uint32_t _crossFeedRatio;
		
	static NesModel _model;
	static PpuModel _ppuModel;

	static uint32_t _emulationSpeed;
	static uint32_t _turboSpeed;
	static uint32_t _rewindSpeed;

	static uint32_t _rewindBufferSize;

	static bool _hasOverclock;
	static uint32_t _overclockRate;
	static bool _overclockAdjustApu;
	static bool _disableOverclocking;
	static uint32_t _extraScanlinesBeforeNmi;
	static uint32_t _extraScanlinesAfterNmi;
	static double _effectiveOverclockRate;
	static double _effectiveOverclockRateSound;

	static OverscanDimensions _overscan;
	static VideoFilterType _videoFilterType;
	static double _videoScale;
	static VideoAspectRatio _aspectRatio;
	static double _customAspectRatio;
	static VideoResizeFilter _resizeFilter;
	static PictureSettings _pictureSettings;
	static NtscFilterSettings _ntscFilterSettings;
	static bool _backgroundEnabled;
	static bool _spritesEnabled;
	static uint32_t _screenRotation;
	static uint32_t _exclusiveRefreshRate;

	static ConsoleType _consoleType;
	static ExpansionPortDevice _expansionDevice;
	static ControllerType _controllerTypes[4];
	static KeyMappingSet _controllerKeys[4];
	static bool _needControllerUpdate;
	static uint32_t _zapperDetectionRadius;
	static std::unordered_map<int, double> _mouseSensitivity;
	static int32_t _inputPollScanline;

	static int32_t _nsfAutoDetectSilenceDelay;
	static int32_t _nsfMoveToNextTrackTime;
	static bool _nsfDisableApuIrqs;

	static InputDisplaySettings _inputDisplaySettings;

	static uint32_t _autoSaveDelay;
	static bool _autoSaveNotify;

	static bool _keyboardModeEnabled;

	static std::unordered_map<uint32_t, KeyCombination> _emulatorKeys[3];
	static std::unordered_map<uint32_t, vector<KeyCombination>> _shortcutSupersets[3];

	static RamPowerOnState _ramPowerOnState;
	static uint32_t _dipSwitches;
	static VsInputType _vsInputType;
	
	static SimpleLock _shortcutLock;
	static SimpleLock _equalizerLock;
	static SimpleLock _lock;

public:
	static uint32_t GetMesenVersion()
	{
		return (_versionMajor << 16) | (_versionMinor << 8) | _versionRevision;
	}

	static string GetMesenVersionString()
	{
		return std::to_string(_versionMajor) + "." + std::to_string(_versionMinor) + "." + std::to_string(_versionRevision);
	}

	static void SetFlags(uint64_t flags)
	{
		if((_flags & flags) != flags) {
			//Need a lock to prevent flag changes from being ignored due to multithreaded access
			LockHandler lock = _lock.AcquireSafe();
			_flags |= flags;

			_backgroundEnabled = !CheckFlag(EmulationFlags::DisableBackground);
			_spritesEnabled = !CheckFlag(EmulationFlags::DisableSprites);
			if(flags & EmulationFlags::UseCustomVsPalette) {
				UpdateCurrentPalette();
			}
		}
	}

	static void SetFlagState(EmulationFlags flag, bool enabled)
	{
		if(enabled) {
			SetFlags(flag);
		} else {
			ClearFlags(flag);
		}
	}

	static void ClearFlags(uint64_t flags)
	{
		if((_flags & flags) != 0) {
			//Need a lock to prevent flag changes from being ignored due to multithreaded access
			LockHandler lock = _lock.AcquireSafe();
			_flags &= ~flags;

			_backgroundEnabled = !CheckFlag(EmulationFlags::DisableBackground);
			_spritesEnabled = !CheckFlag(EmulationFlags::DisableSprites);
			if(flags & EmulationFlags::UseCustomVsPalette) {
				UpdateCurrentPalette();
			}
		}
	}

	static bool CheckFlag(EmulationFlags flag)
	{
		return (_flags & flag) == flag;
	}

	static void SetDisplayLanguage(Language lang)
	{
		_displayLanguage = lang;
	}

	static Language GetDisplayLanguage()
	{
		return _displayLanguage;
	}

	static bool IsPaused()
	{
		return (CheckFlag(EmulationFlags::Paused) || (CheckFlag(EmulationFlags::InBackground) && CheckFlag(EmulationFlags::PauseWhenInBackground) && !GameClient::Connected())) && !CheckFlag(EmulationFlags::DebuggerWindowEnabled);
	}

	static bool InputEnabled()
	{
		return !EmulationSettings::CheckFlag(EmulationFlags::InBackground) || EmulationSettings::CheckFlag(EmulationFlags::AllowBackgroundInput);
	}

	static void SetNesModel(NesModel model)
	{
		_model = model;
	}

	static NesModel GetNesModel()
	{
		return _model;
	}

	static void SetPpuModel(PpuModel ppuModel)
	{
		_ppuModel = ppuModel;
		UpdateCurrentPalette();
	}

	static void UpdateCurrentPalette()
	{
		if(CheckFlag(EmulationFlags::UseCustomVsPalette)) {
			for(int i = 0; i < 64; i++) {
				_currentPalette[i] = _ppuPaletteArgb[0][_paletteLut[(int)_ppuModel][i]];
			}
		} else {
			memcpy(_currentPalette, _ppuPaletteArgb[(int)_ppuModel], sizeof(_currentPalette));
		}
	}

	static PpuModel GetPpuModel()
	{
		return _ppuModel;
	}

	//0: Muted, 0.5: Default, 1.0: Max volume
	static void SetChannelVolume(AudioChannel channel, double volume)
	{
		_channelVolume[(int)channel] = volume;
		_audioSettingsChanged = true;
	}

	static void SetMasterVolume(double volume)
	{
		_masterVolume = volume;
		_audioSettingsChanged = true;
	}

	static void SetChannelPanning(AudioChannel channel, double panning)
	{
		_channelPanning[(int)channel] = panning;
		_audioSettingsChanged = true;
	}

	static vector<double> GetBandGains()
	{
		auto lock = _equalizerLock.AcquireSafe();
		return _bandGains;
	}

	static void SetBandGain(int band, double gain)
	{
		auto lock = _equalizerLock.AcquireSafe();
		if(band < (int)_bandGains.size()) {
			_bandGains[band] = gain;
			_audioSettingsChanged = true;
		}
	}

	static vector<double> GetEqualizerBands()
	{
		auto lock = _equalizerLock.AcquireSafe();
		return _bands;
	}

	static void SetEqualizerBands(double *bands, uint32_t bandCount)
	{
		auto lock = _equalizerLock.AcquireSafe();
		_bands.clear();
		_bandGains.clear();
		for(uint32_t i = 0; i < bandCount; i++) {
			_bands.push_back(bands[i]);
			_bandGains.push_back(0);
		}
	}

	static EqualizerFilterType GetEqualizerFilterType()
	{
		return _equalizerFilterType;
	}

	static void SetEqualizerFilterType(EqualizerFilterType filter)
	{
		_equalizerFilterType = filter;
		_audioSettingsChanged = true;
	}

	static void SetSampleRate(uint32_t sampleRate)
	{
		_sampleRate = sampleRate;
		_audioSettingsChanged = true;
	}

	static uint32_t GetSampleRate()
	{
		return _sampleRate;
	}

	static void SetAudioLatency(uint32_t msLatency)
	{
		_audioLatency = msLatency;
		_audioSettingsChanged = true;
	}

	static void SetStereoFilter(StereoFilter stereoFilter)
	{
		_stereoFilter = stereoFilter;
		_audioSettingsChanged = true;
	}

	static void SetStereoDelay(int32_t delay)
	{
		_stereoDelay = delay;
		_audioSettingsChanged = true;
	}

	static void SetStereoPanningAngle(double angle)
	{
		_stereoAngle = angle;
		_audioSettingsChanged = true;
	}

	static void SetReverbParameters(double strength, double delay)
	{
		_reverbStrength = strength;
		_reverbDelay = delay;
		_audioSettingsChanged = true;
	}

	static StereoFilter GetStereoFilter()
	{
		return _stereoFilter;
	}

	static int32_t GetStereoDelay()
	{
		return _stereoDelay;
	}

	static double GetStereoPanningAngle()
	{
		return _stereoAngle;
	}

	static double GetReverbStrength()
	{
		return _reverbStrength;
	}

	static double GetReverbDelay()
	{
		return _reverbDelay;
	}

	static void SetCrossFeedRatio(uint32_t ratio)
	{
		_crossFeedRatio = ratio;
		_audioSettingsChanged = true;
	}

	static bool NeedAudioSettingsUpdate()
	{
		bool value = _audioSettingsChanged;
		if(value) {
			_audioSettingsChanged = false;
		}
		return value;
	}

	static uint32_t GetCrossFeedRatio()
	{
		return _crossFeedRatio;
	}

	//0: No limit, Number: % of default speed (50/60fps)
	static void SetEmulationSpeed(uint32_t emulationSpeed, bool displaySpeed = false)
	{
		if(_emulationSpeed != emulationSpeed) {
			_emulationSpeed = emulationSpeed;
			if(displaySpeed) {
				MessageManager::DisplayMessage("EmulationSpeed", _emulationSpeed == 0 ? "EmulationMaximumSpeed" : "EmulationSpeedPercent", std::to_string(_emulationSpeed));
			}
		}
	}

	static void IncreaseEmulationSpeed()
	{
		if(_emulationSpeed == _speedValues.back()) {
			EmulationSettings::SetEmulationSpeed(0, true);
		} else if(_emulationSpeed != 0) {
			for(size_t i = 0; i < _speedValues.size(); i++) {
				if(_speedValues[i] > _emulationSpeed) {
					EmulationSettings::SetEmulationSpeed(_speedValues[i], true);
					break;
				}
			}
		}
	}

	static void DecreaseEmulationSpeed()
	{
		if(_emulationSpeed == 0) {
			EmulationSettings::SetEmulationSpeed(_speedValues.back(), true);
		} else if(_emulationSpeed > _speedValues.front()) {
			for(int i = (int)_speedValues.size() - 1; i >= 0; i--) {
				if(_speedValues[i] < _emulationSpeed) {
					EmulationSettings::SetEmulationSpeed(_speedValues[i], true);
					break;
				}
			}
		}
	}

	static void SetTurboRewindSpeed(uint32_t turboSpeed, uint32_t rewindSpeed)
	{
		_turboSpeed = turboSpeed;
		_rewindSpeed = rewindSpeed;
	}

	static uint32_t GetRewindSpeed()
	{
		return _rewindSpeed;
	}

	static void SetRewindBufferSize(uint32_t seconds)
	{
		_rewindBufferSize = seconds;
	}

	static uint32_t GetRewindBufferSize()
	{
		return _rewindBufferSize;
	}

	static uint32_t GetEmulationSpeed(bool ignoreTurbo = false);

	static void UpdateEffectiveOverclockRate()
	{
		if(_disableOverclocking) {
			_effectiveOverclockRate = 100;
		} else {
			_effectiveOverclockRate = _overclockRate;
		}
		_hasOverclock = _effectiveOverclockRate != 100;
		_audioSettingsChanged = true;
	}

	static void DisableOverclocking(bool disabled)
	{
		if(_disableOverclocking != disabled) {
			_disableOverclocking = disabled;
			UpdateEffectiveOverclockRate();
		}
	}

	static uint32_t GetOverclockRateSetting()
	{
		return _overclockRate;
	}

	static bool HasOverclock()
	{
		return _hasOverclock;
	}

	static double GetOverclockRate()
	{
		return _effectiveOverclockRate;
	}

	static bool GetOverclockAdjustApu()
	{
		return _overclockAdjustApu;
	}

	static void SetOverclockRate(uint32_t overclockRate, bool adjustApu)
	{
		if(_overclockRate != overclockRate || _overclockAdjustApu != adjustApu) {
			_overclockRate = overclockRate;
			_overclockAdjustApu = adjustApu;

			UpdateEffectiveOverclockRate();

			MessageManager::SendNotification(ConsoleNotificationType::ConfigChanged);

			MessageManager::DisplayMessage("ClockRate", std::to_string((uint32_t)EmulationSettings::GetOverclockRate()) + "%");
		}
	}

	static uint32_t GetPpuExtraScanlinesBeforeNmi()
	{
		return _disableOverclocking ? 0 : _extraScanlinesBeforeNmi;
	}

	static uint32_t GetPpuExtraScanlinesAfterNmi()
	{
		return _disableOverclocking ? 0 : _extraScanlinesAfterNmi;
	}

	static void SetPpuNmiConfig(uint32_t extraScanlinesBeforeNmi, uint32_t extraScanlinesAfterNmi)
	{
		if(_extraScanlinesBeforeNmi != extraScanlinesBeforeNmi || _extraScanlinesAfterNmi != extraScanlinesAfterNmi) {
			if(extraScanlinesBeforeNmi > 0 || extraScanlinesAfterNmi > 0) {
				MessageManager::DisplayMessage("PPU", "ScanlineTimingWarning");
			}

			_extraScanlinesBeforeNmi = extraScanlinesBeforeNmi;
			_extraScanlinesAfterNmi = extraScanlinesAfterNmi;

			UpdateEffectiveOverclockRate();
		}
	}

	static void SetOverscanDimensions(uint8_t left, uint8_t right, uint8_t top, uint8_t bottom)
	{
		if(_overscan.Left != left || _overscan.Right != right || _overscan.Top != top || _overscan.Bottom != bottom) {
			_overscan.Left = left;
			_overscan.Right = right;
			_overscan.Top = top;
			_overscan.Bottom = bottom;
		}
	}

	static OverscanDimensions GetOverscanDimensions()
	{
		return _overscan;
	}

	static double GetChannelVolume(AudioChannel channel)
	{
		return _channelVolume[(int)channel];
	}

	static double GetMasterVolume()
	{
		return _masterVolume;
	}

	static double GetChannelPanning(AudioChannel channel)
	{
		return _channelPanning[(int)channel];
	}

	static uint32_t GetAudioLatency()
	{
		return _audioLatency;
	}

	static void SetVideoFilterType(VideoFilterType videoFilterType)
	{
		_videoFilterType = videoFilterType;
	}

	static VideoFilterType GetVideoFilterType()
	{
		return _videoFilterType;
	}

	static void SetVideoResizeFilter(VideoResizeFilter videoResizeFilter)
	{
		_resizeFilter = videoResizeFilter;
	}

	static VideoResizeFilter GetVideoResizeFilter()
	{
		return _resizeFilter;
	}

	static void SetVideoAspectRatio(VideoAspectRatio aspectRatio, double customRatio)
	{
		_aspectRatio = aspectRatio;
		_customAspectRatio = customRatio;
	}

	static bool GetBackgroundEnabled()
	{
		return _backgroundEnabled;
	}

	static bool GetSpritesEnabled()
	{
		return _spritesEnabled;
	}

	static void SetPictureSettings(double brightness, double contrast, double saturation, double hue, double scanlineIntensity)
	{
		_pictureSettings.Brightness = brightness;
		_pictureSettings.Contrast = contrast;
		_pictureSettings.Saturation = saturation;
		_pictureSettings.Hue = hue;
		_pictureSettings.ScanlineIntensity = scanlineIntensity;
	}

	static PictureSettings GetPictureSettings()
	{
		return _pictureSettings;
	}

	static void SetNtscFilterSettings(double artifacts, double bleed, double fringing, double gamma, double resolution, double sharpness, bool mergeFields, double yFilterLength, double iFilterLength, double qFilterLength, bool verticalBlend, bool keepVerticalResolution)
	{
		_ntscFilterSettings.Artifacts = artifacts;
		_ntscFilterSettings.Bleed = bleed;
		_ntscFilterSettings.Fringing = fringing;
		_ntscFilterSettings.Gamma = gamma;
		_ntscFilterSettings.Resolution = resolution;
		_ntscFilterSettings.Sharpness = sharpness;

		_ntscFilterSettings.MergeFields = mergeFields;

		_ntscFilterSettings.YFilterLength = yFilterLength;
		_ntscFilterSettings.IFilterLength = iFilterLength;
		_ntscFilterSettings.QFilterLength = qFilterLength;

		_ntscFilterSettings.VerticalBlend = verticalBlend;
		_ntscFilterSettings.KeepVerticalResolution = keepVerticalResolution;
	}

	static NtscFilterSettings GetNtscFilterSettings()
	{
		return _ntscFilterSettings;
	}

	static double GetAspectRatio();

	static void SetVideoScale(double scale)
	{
		if(_videoScale != scale) {
			_videoScale = scale;
		}
	}

	static double GetVideoScale()
	{
		return _videoScale;
	}
	
	static void SetScreenRotation(uint32_t angle)
	{
		_screenRotation = angle;
	}

	static uint32_t GetScreenRotation()
	{
		return _screenRotation;
	}

	static void SetExclusiveRefreshRate(uint32_t refreshRate)
	{
		_exclusiveRefreshRate = refreshRate;
	}

	static uint32_t GetExclusiveRefreshRate()
	{
		return _exclusiveRefreshRate;
	}

	static uint32_t* GetRgbPalette()
	{
		return _currentPalette;
	}

	static void GetRgbPalette(uint32_t* paletteBuffer)
	{
		memcpy(paletteBuffer, _ppuPaletteArgb[0], sizeof(_ppuPaletteArgb[0]));
	}

	static void SetRgbPalette(uint32_t* paletteBuffer)
	{
		memcpy(_ppuPaletteArgb[0], paletteBuffer, sizeof(_ppuPaletteArgb[0]));
		UpdateCurrentPalette();
	}

	static bool IsDefaultPalette()
	{
		return memcmp(_defaultPpuPalette, GetRgbPalette(), sizeof(_defaultPpuPalette)) == 0;
	}

	static void SetExpansionDevice(ExpansionPortDevice expansionDevice)
	{
		_expansionDevice = expansionDevice;
		_needControllerUpdate = true;
	}
	
	static ExpansionPortDevice GetExpansionDevice()
	{
		return _expansionDevice;
	}

	static void SetConsoleType(ConsoleType type)
	{
		_consoleType = type;
		_needControllerUpdate = true;
	}

	static ConsoleType GetConsoleType()
	{
		return _consoleType;
	}

	static void SetControllerType(uint8_t port, ControllerType type)
	{
		_controllerTypes[port] = type;
		_needControllerUpdate = true;
	}

	static ControllerType GetControllerType(uint8_t port)
	{
		return _controllerTypes[port];
	}

	static void SetControllerKeys(uint8_t port, KeyMappingSet keyMappings)
	{
		_controllerKeys[port] = keyMappings;
		_needControllerUpdate = true;
	}

	static KeyMappingSet GetControllerKeys(uint8_t port)
	{
		return _controllerKeys[port];
	}

	static void ClearShortcutKeys()
	{
		auto lock = _shortcutLock.AcquireSafe();
		_emulatorKeys[0].clear();
		_emulatorKeys[1].clear();
		_emulatorKeys[2].clear();
		_shortcutSupersets[0].clear();
		_shortcutSupersets[1].clear();
		_shortcutSupersets[2].clear();
		
		//Add Alt-F4 as a fake shortcut to prevent Alt-F4 from triggering Alt or F4 key bindings. (e.g load save state 4)
		KeyCombination keyComb;
		keyComb.Key1 = KeyManager::GetKeyCode("Alt");
		keyComb.Key2 = KeyManager::GetKeyCode("F4");
		SetShortcutKey(EmulatorShortcut::Exit, keyComb, 2);
	}

	static void SetShortcutKey(EmulatorShortcut shortcut, KeyCombination keyCombination, int keySetIndex)
	{
		auto lock = _shortcutLock.AcquireSafe();
		_emulatorKeys[keySetIndex][(uint32_t)shortcut] = keyCombination;
		
		for(int i = 0; i < 3; i++) {
			for(std::pair<const uint32_t, KeyCombination> &kvp : _emulatorKeys[i]) {
				if(keyCombination.IsSubsetOf(kvp.second)) {
					_shortcutSupersets[keySetIndex][(uint32_t)shortcut].push_back(kvp.second);
				} else if(kvp.second.IsSubsetOf(keyCombination)) {
					_shortcutSupersets[i][kvp.first].push_back(keyCombination);
				}
			}
		}
	}

	static KeyCombination GetShortcutKey(EmulatorShortcut shortcut, int keySetIndex)
	{
		auto lock = _shortcutLock.AcquireSafe();
		auto result = _emulatorKeys[keySetIndex].find((int)shortcut);
		if(result != _emulatorKeys[keySetIndex].end()) {
			return result->second;
		}
		return {};
	}
	
	static vector<KeyCombination> GetShortcutSupersets(EmulatorShortcut shortcut, int keySetIndex)
	{
		auto lock = _shortcutLock.AcquireSafe();
		return _shortcutSupersets[keySetIndex][(uint32_t)shortcut];
	}

	static bool NeedControllerUpdate()
	{
		if(_needControllerUpdate) {
			_needControllerUpdate = false;
			return true;
		} else {
			return false;
		}
	}

	static void SetMouseSensitivity(MouseDevice device, double sensitivity)
	{
		_mouseSensitivity[(int)device] = sensitivity;
	}

	static double GetMouseSensitivity(MouseDevice device)
	{
		auto result = _mouseSensitivity.find((int)device);
		if(result != _mouseSensitivity.end()) {
			return result->second;
		} else {
			return 1.0;
		}
	}

	static void SetZapperDetectionRadius(uint32_t detectionRadius)
	{
		_zapperDetectionRadius = detectionRadius;
	}

	static uint32_t GetZapperDetectionRadius()
	{
		return _zapperDetectionRadius;
	}

	static void SetInputPollScanline(int32_t scanline)
	{
		_inputPollScanline = scanline;
	}

	static int32_t GetInputPollScanline()
	{
		return _inputPollScanline;
	}

	static bool HasZapper()
	{
		return _controllerTypes[0] == ControllerType::Zapper || _controllerTypes[1] == ControllerType::Zapper || (_consoleType == ConsoleType::Famicom && _expansionDevice == ExpansionPortDevice::Zapper);
	}

	static bool HasArkanoidPaddle()
	{
		return _controllerTypes[0] == ControllerType::ArkanoidController || _controllerTypes[1] == ControllerType::ArkanoidController || (_consoleType == ConsoleType::Famicom && _expansionDevice == ExpansionPortDevice::ArkanoidController);
	}

	static void SetNsfConfig(int32_t autoDetectSilence, int32_t moveToNextTrackTime, bool disableApuIrqs)
	{
		_nsfAutoDetectSilenceDelay = autoDetectSilence;
		_nsfMoveToNextTrackTime = moveToNextTrackTime;
		_nsfDisableApuIrqs = disableApuIrqs;
	}

	static int32_t GetNsfAutoDetectSilenceDelay()
	{
		return _nsfAutoDetectSilenceDelay;
	}

	static int32_t GetNsfMoveToNextTrackTime()
	{
		return _nsfMoveToNextTrackTime;
	}

	static bool GetNsfDisableApuIrqs()
	{
		return _nsfDisableApuIrqs;
	}

	static void SetInputDisplaySettings(uint8_t visiblePorts, InputDisplayPosition displayPosition, bool displayHorizontally)
	{
		_inputDisplaySettings = { visiblePorts, displayPosition, displayHorizontally };
	}

	static InputDisplaySettings GetInputDisplaySettings()
	{
		return _inputDisplaySettings;
	}

	static void SetRamPowerOnState(RamPowerOnState state)
	{
		_ramPowerOnState = state;
	}

	static RamPowerOnState GetRamPowerOnState()
	{
		return _ramPowerOnState;
	}

	static void SetAutoSaveOptions(uint32_t delayInMinutes, bool showMessage)
	{
		_autoSaveDelay = delayInMinutes;
		_autoSaveNotify = showMessage;
	}

	static uint32_t GetAutoSaveDelay(bool &showMessage)
	{
		showMessage = _autoSaveNotify;
		return _autoSaveDelay;
	}

	static void SetDipSwitches(uint32_t dipSwitches)
	{
		_dipSwitches = dipSwitches;
	}

	static uint32_t GetDipSwitches()
	{
		return _dipSwitches;
	}

	static void SetVsInputType(VsInputType inputType)
	{
		_vsInputType = inputType;
	}

	static VsInputType GetVsInputType()
	{
		return _vsInputType;
	}

	static bool IsKeyboardMode()
	{
		return _keyboardModeEnabled;
	}

	static void EnableKeyboardMode()
	{
		if(!_keyboardModeEnabled) {
			_keyboardModeEnabled = true;
			MessageManager::DisplayMessage("Input", "KeyboardModeEnabled");
		}
	}

	static void DisableKeyboardMode()
	{
		if(_keyboardModeEnabled) {
			_keyboardModeEnabled = false;
			MessageManager::DisplayMessage("Input", "KeyboardModeDisabled");
		}
	}
};
