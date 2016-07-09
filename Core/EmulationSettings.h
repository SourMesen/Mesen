#pragma once

#include "stdafx.h"
#include "MessageManager.h"
#include "GameClient.h"

enum EmulationFlags
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

	InBackground = 0x40000000,
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

enum class NesModel
{
	Auto = 0,
	NTSC = 1,
	PAL = 2,
	Dendy = 3,
};

enum class VideoFilterType
{
	None = 0,
	NTSC = 1,
	xBRZ2x = 2,
	xBRZ3x = 3,
	xBRZ4x = 4,
	xBRZ5x = 5,
	xBRZ6x = 6,
	HQ2x = 7,
	HQ3x = 8,
	HQ4x = 9,
	Scale2x = 10,
	Scale3x = 11,
	Scale4x = 12,
	_2xSai = 13,
	Super2xSai = 14,
	SuperEagle = 15,
	Prescale2x = 16,
	Prescale3x = 17,
	Prescale4x = 18,
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
	Widescreen = 4
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
};

enum class ConsoleType
{
	Nes = 0,
	Famicom = 1,
	VsSystem = 2,
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

enum class Language
{
	SystemDefault = 0,
	English = 1,
	French = 2,
	Japanese = 3
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

class EmulationSettings
{
private:
	static uint16_t _versionMajor;
	static uint8_t _versionMinor;
	static uint8_t _versionRevision;

	static uint32_t _ppuPaletteArgb[11][64];
	static uint32_t _defaultPpuPalette[64];
	static uint32_t _flags;

	static Language _displayLanguage;

	static uint32_t _audioLatency;
	static double _channelVolume[11];
	static double _masterVolume;
	static uint32_t _sampleRate;
	static StereoFilter _stereoFilter;
	static int32_t _stereoDelay;
	static double _stereoAngle;
	static double _reverbStrength;
	static double _reverbDelay;
		
	static NesModel _model;
	static PpuModel _ppuModel;

	static uint32_t _emulationSpeed;
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
	static VideoResizeFilter _resizeFilter;
	static PictureSettings _pictureSettings;
	static NtscFilterSettings _ntscFilterSettings;

	static ConsoleType _consoleType;
	static ExpansionPortDevice _expansionDevice;
	static ControllerType _controllerTypes[4];
	static KeyMappingSet _controllerKeys[4];
	static bool _needControllerUpdate;

	static int32_t _nsfAutoDetectSilenceDelay;
	static int32_t _nsfMoveToNextTrackTime;
	static bool _nsfDisableApuIrqs;

public:
	static uint32_t GetMesenVersion()
	{
		return (_versionMajor << 16) | (_versionMinor << 8) | _versionRevision;
	}

	static string GetMesenVersionString()
	{
		return std::to_string(_versionMajor) + "." + std::to_string(_versionMinor) + "." + std::to_string(_versionRevision);
	}

	static void SetFlags(EmulationFlags flags)
	{
		_flags |= flags;
	}

	static void ClearFlags(EmulationFlags flags)
	{
		_flags &= ~flags;
	}

	static bool CheckFlag(uint32_t flag)
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

	static void SetSampleRate(uint32_t sampleRate)
	{
		_sampleRate = sampleRate;
	}

	static uint32_t GetSampleRate()
	{
		return _sampleRate;
	}

	static void SetAudioLatency(uint32_t msLatency)
	{
		_audioLatency = msLatency;
	}

	static void SetStereoFilter(StereoFilter stereoFilter)
	{
		_stereoFilter = stereoFilter;
	}

	static void SetStereoDelay(int32_t delay)
	{
		_stereoDelay = delay;
	}

	static void SetStereoPanningAngle(double angle)
	{
		_stereoAngle = angle;
	}

	static void SetReverbParameters(double strength, double delay)
	{
		_reverbStrength = strength;
		_reverbDelay = delay;
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

	//0: No limit, Number: % of default speed (50/60fps)
	static void SetEmulationSpeed(uint32_t emulationSpeed)
	{
		_emulationSpeed = emulationSpeed;
	}

	static uint32_t GetEmulationSpeed()
	{
		return _emulationSpeed;
	}

	static void UpdateEffectiveOverclockRate()
	{
		if(_disableOverclocking) {
			_effectiveOverclockRateSound = 100;
			_effectiveOverclockRate = 100;
		} else {
			_effectiveOverclockRateSound = _overclockRate * (double)(1 + (double)(_extraScanlinesBeforeNmi + _extraScanlinesAfterNmi) / _ppuScanlineCount);
			_effectiveOverclockRate = _overclockRate;
		}
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

	static double GetOverclockRate(bool forApu = false, bool forSoundMixer = false)
	{
		if(forApu && _overclockAdjustApu || forSoundMixer) {
			return _effectiveOverclockRateSound;
		} else if(!forApu) {
			return _effectiveOverclockRate;
		} else {
			return 100;
		}
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
			MessageManager::SendNotification(ConsoleNotificationType::ResolutionChanged);
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

	static void SetVideoAspectRatio(VideoAspectRatio aspectRatio)
	{
		if(_aspectRatio != aspectRatio) {
			_aspectRatio = aspectRatio;
			MessageManager::SendNotification(ConsoleNotificationType::ResolutionChanged);
		}
	}

	static VideoAspectRatio GetVideoAspectRatio()
	{
		return _aspectRatio;
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

	static void SetNtscFilterSettings(double artifacts, double bleed, double fringing, double gamma, double resolution, double sharpness, bool mergeFields)
	{
		_ntscFilterSettings.Artifacts = artifacts;
		_ntscFilterSettings.Bleed = bleed;
		_ntscFilterSettings.Fringing = fringing;
		_ntscFilterSettings.Gamma = gamma;
		_ntscFilterSettings.Resolution = resolution;
		_ntscFilterSettings.Sharpness = sharpness;

		_ntscFilterSettings.MergeFields = mergeFields;
	}

	static NtscFilterSettings GetNtscFilterSettings()
	{
		return _ntscFilterSettings;
	}

	static double GetAspectRatio()
	{
		switch(_aspectRatio) {
			case VideoAspectRatio::NTSC: return 8.0 / 7.0;
			case VideoAspectRatio::PAL: return 18.0 / 13.0;
			case VideoAspectRatio::Standard: return 4.0 / 3.0;
			case VideoAspectRatio::Widescreen: return 16.0 / 9.0;
		}
		return 0.0;
	}

	static void SetVideoScale(double scale)
	{
		if(_videoScale != scale) {
			_videoScale = scale;
			MessageManager::SendNotification(ConsoleNotificationType::ResolutionChanged);
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
};
