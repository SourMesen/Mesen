#pragma once

#include "stdafx.h"
#include "MessageManager.h"
#include "GameClient.h"

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

	ForceMaxSpeed = 0x40000000000,
	
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

enum class NesModel
{
	Auto = 0,
	NTSC = 1,
	PAL = 2,
	Dendy = 3,
};

enum class RamPowerOnState
{
	AllZeros = 0,
	AllOnes = 1,
	Random = 2
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
	Auto = 0,
	NTSC = 1,
	PAL = 2,
	Standard = 3,
	Widescreen = 4,
	Custom = 5
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

	double YFilterLength = 0;
	double IFilterLength = 0;
	double QFilterLength = 0;
};

enum class ConsoleType
{
	Nes = 0,
	Famicom = 1
};

enum class ControllerType
{
	None = 0,
	StandardController = 1,
	Zapper = 2,
	ArkanoidController = 3,
};

enum class ExpansionPortDevice
{
	None = 0,
	Zapper = 1,
	FourPlayerAdapter = 2,
	ArkanoidController = 3,
	OekaKidsTablet = 4,
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

	bool HasKeySet()
	{
		return A || B || Up || Down || Left || Right || Start || Select || TurboA || TurboB || TurboStart || TurboSelect;
	}
};

struct KeyMappingSet
{
	KeyMapping Mapping1;
	KeyMapping Mapping2;
	KeyMapping Mapping3;
	KeyMapping Mapping4;
	uint32_t TurboSpeed;
};

struct EmulatorKeyMappings
{
	uint32_t FastForward;
	uint32_t Rewind;
	uint32_t RewindTenSecs;
	uint32_t RewindOneMin;

	uint32_t Pause;
	uint32_t Reset;
	uint32_t Exit;

	uint32_t MoveToNextStateSlot;
	uint32_t MoveToPreviousStateSlot;
	uint32_t SaveState;
	uint32_t LoadState;

	uint32_t SwitchDiskSide;
	uint32_t InsertNextDisk;

	uint32_t InsertCoin1;
	uint32_t InsertCoin2;
	uint32_t VsServiceButton;

	uint32_t TakeScreenshot;
	uint32_t IncreaseSpeed;
	uint32_t DecreaseSpeed;

	uint32_t ToggleCheats;

	uint32_t RunSingleFrame;
};

struct EmulatorKeyMappingSet
{
	EmulatorKeyMappings KeySet1 = {};
	EmulatorKeyMappings KeySet2 = {};
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
};

enum class StereoFilter
{
	None = 0,
	Delay = 1,
	Panning = 2,
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

	static uint32_t _ppuPaletteArgb[11][64];
	static uint32_t _defaultPpuPalette[64];
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
	static uint32_t _ppuScanlineCount;
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

	static ConsoleType _consoleType;
	static ExpansionPortDevice _expansionDevice;
	static ControllerType _controllerTypes[4];
	static KeyMappingSet _controllerKeys[4];
	static bool _needControllerUpdate;

	static int32_t _nsfAutoDetectSilenceDelay;
	static int32_t _nsfMoveToNextTrackTime;
	static bool _nsfDisableApuIrqs;

	static InputDisplaySettings _inputDisplaySettings;

	static uint32_t _autoSaveDelay;
	static bool _autoSaveNotify;

	static EmulatorKeyMappingSet _emulatorKeys;

	static RamPowerOnState _ramPowerOnState;

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
		_flags |= flags;

		_backgroundEnabled = !CheckFlag(EmulationFlags::DisableBackground);
		_spritesEnabled = !CheckFlag(EmulationFlags::DisableSprites);
	}

	static void SetFlagState(EmulationFlags flag, bool enabled)
	{
		if(enabled) {
			_flags |= flag;
		} else {
			_flags &= ~flag;
		}
		
		_backgroundEnabled = !CheckFlag(EmulationFlags::DisableBackground);
		_spritesEnabled = !CheckFlag(EmulationFlags::DisableSprites);
	}

	static void ClearFlags(uint64_t flags)
	{
		_flags &= ~flags;
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
		return CheckFlag(EmulationFlags::Paused) || (CheckFlag(EmulationFlags::InBackground) && CheckFlag(EmulationFlags::PauseWhenInBackground) && !GameClient::Connected());
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
	}

	static PpuModel GetPpuModel()
	{
		return _ppuModel;
	}

	//0: Muted, 0.5: Default, 1.0: Max volume
	static void SetChannelVolume(AudioChannel channel, double volume)
	{
		_channelVolume[(int)channel] = volume;
	}

	static void SetMasterVolume(double volume)
	{
		_masterVolume = volume;
	}

	static void SetChannelPanning(AudioChannel channel, double panning)
	{
		_channelPanning[(int)channel] = panning;
		_audioSettingsChanged = true;
	}

	static double GetBandGain(int band)
	{
		if(band < (int)_bandGains.size()) {
			return _bandGains[band];
		} else {
			return 0;
		}
	}

	static void SetBandGain(int band, double gain)
	{
		if(band < (int)_bandGains.size()) {
			_bandGains[band] = gain;
		}
		_audioSettingsChanged = true;
	}
	
	static vector<double> GetEqualizerBands()
	{
		return _bands;
	}

	static void SetEqualizerBands(double *bands, uint32_t bandCount);

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
		if(_emulationSpeed > 0 && _emulationSpeed < 450) {
			EmulationSettings::SetEmulationSpeed(_emulationSpeed + (_emulationSpeed < 100 ? 25 : 50), true);
		} else {
			EmulationSettings::SetEmulationSpeed(0, true);
		}
	}

	static void DecreaseEmulationSpeed()
	{
		if(_emulationSpeed == 0) {
			EmulationSettings::SetEmulationSpeed(450, true);
		} else if(_emulationSpeed > 25) {
			EmulationSettings::SetEmulationSpeed(_emulationSpeed - (_emulationSpeed <= 100 ? 25 : 50), true);
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

	static void SetRewindBufferSize(uint32_t seconds);

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

	static void SetPpuScanlineCount(uint32_t scanlineCount)
	{
		_ppuScanlineCount = scanlineCount;		
		UpdateEffectiveOverclockRate();
	}

	static void DisableOverclocking(bool disabled)
	{
		_disableOverclocking = disabled;
		UpdateEffectiveOverclockRate();
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

	static void SetNtscFilterSettings(double artifacts, double bleed, double fringing, double gamma, double resolution, double sharpness, bool mergeFields, double yFilterLength, double iFilterLength, double qFilterLength)
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
	}

	static NtscFilterSettings GetNtscFilterSettings()
	{
		return _ntscFilterSettings;
	}

	static double GetAspectRatio()
	{
		switch(_aspectRatio) {
			case VideoAspectRatio::NTSC: return 128.0 / 105.0;
			case VideoAspectRatio::PAL: return 9440000.0 / 6384411.0;
			case VideoAspectRatio::Standard: return 4.0 / 3.0;
			case VideoAspectRatio::Widescreen: return 16.0 / 9.0;
			case VideoAspectRatio::Custom: return _customAspectRatio;
		}
		return 0.0;
	}

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
	
	static uint32_t* GetRgbPalette()
	{
		return _ppuPaletteArgb[(int)_ppuModel];
	}

	static void GetRgbPalette(uint32_t* paletteBuffer)
	{
		memcpy(paletteBuffer, _ppuPaletteArgb[0], sizeof(_ppuPaletteArgb[0]));
	}

	static void SetRgbPalette(uint32_t* paletteBuffer)
	{
		memcpy(_ppuPaletteArgb[0], paletteBuffer, sizeof(_ppuPaletteArgb[0]));
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

	static void SetEmulatorKeys(EmulatorKeyMappingSet keyMappings)
	{
		_emulatorKeys = keyMappings;
	}

	static EmulatorKeyMappingSet GetEmulatorKeys()
	{
		return _emulatorKeys;
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
};
