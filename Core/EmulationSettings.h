#pragma once

#include "stdafx.h"
#include "MessageManager.h"

enum EmulationFlags
{
	Paused = 0x01,
	ShowFPS = 0x02,
	VerticalSync = 0x04,

	Mmc3IrqAltBehavior = 0x8000,
};

enum class AudioChannel
{
	Square1 = 0,
	Square2 = 1,
	Triangle = 2,
	Noise = 3,
	DMC = 4
};

enum class NesModel
{
	Auto = 0,
	NTSC = 1,
	PAL = 2,
};

enum class VideoFilterType
{
	None = 0,
	NTSC = 1,
	HdPack = 999
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

class EmulationSettings
{
private:
	static uint32_t _flags;

	static bool _audioEnabled;
	static uint32_t _audioLatency;
	static double _channelVolume[5];
	
	static NesModel _model;

	static uint32_t _emulationSpeed;

	static OverscanDimensions _overscan;
	static VideoFilterType _videoFilterType;
	static uint32_t _videoScale;

public:
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

	static void SetNesModel(NesModel model)
	{
		_model = model;
	}

	static NesModel GetNesModel()
	{
		return _model;
	}

	static void SetAudioState(bool enabled)
	{
		_audioEnabled = enabled;
	}

	//0: Muted, 0.5: Default, 1.0: Max volume
	static void SetChannelVolume(AudioChannel channel, double volume)
	{
		_channelVolume[(int)channel] = volume;
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
		_overscan.Left = left;
		_overscan.Right = right;
		_overscan.Top = top;
		_overscan.Bottom = bottom;
	}

	static OverscanDimensions GetOverscanDimensions()
	{
		return _overscan;
	}

	static double GetChannelVolume(AudioChannel channel)
	{
		return _audioEnabled ? _channelVolume[(int)channel] : 0;
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

	static void SetVideoScale(uint32_t scale)
	{
		_videoScale = scale;
		MessageManager::SendNotification(ConsoleNotificationType::ResolutionChanged);
	}

	static uint32_t GetVideoScale()
	{
		return _videoScale;
	}
};
