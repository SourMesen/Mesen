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
	HdPack = 999
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
};

enum class ExpansionPortDevice
{
	None = 0,
	Zapper = 1,
	FourPlayerAdapter = 2,
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

class EmulationSettings
{
private:
	//Version 0.1.0
	static uint16_t _versionMajor;
	static uint8_t _versionMinor;
	static uint8_t _versionRevision;

	static uint32_t PpuPaletteArgb[64];
	static uint32_t _flags;

	static uint32_t _audioLatency;
	static double _channelVolume[11];
	static double _masterVolume;

	static uint32_t _sampleRate;
	
	static NesModel _model;

	static uint32_t _emulationSpeed;

	static OverscanDimensions _overscan;
	static VideoFilterType _videoFilterType;
	static double _videoScale;
	static VideoAspectRatio _aspectRatio;

	static ConsoleType _consoleType;
	static ExpansionPortDevice _expansionDevice;
	static ControllerType _controllerTypes[4];
	static KeyMappingSet _controllerKeys[4];
	static bool _needControllerUpdate;

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

	//0: No limit, Number: % of default speed (50/60fps)
	static void SetEmulationSpeed(uint32_t emulationSpeed)
	{
		_emulationSpeed = emulationSpeed;
	}

	static uint32_t GetEmulationSpeed()
	{
		return _emulationSpeed;
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
		return PpuPaletteArgb;
	}

	static void GetRgbPalette(uint32_t* paletteBuffer)
	{
		memcpy(paletteBuffer, PpuPaletteArgb, sizeof(PpuPaletteArgb));
	}

	static void SetRgbPalette(uint32_t* paletteBuffer)
	{
		memcpy(PpuPaletteArgb, paletteBuffer, sizeof(PpuPaletteArgb));
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
};
