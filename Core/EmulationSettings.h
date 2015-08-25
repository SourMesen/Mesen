#pragma once

#include "stdafx.h"

enum EmulationFlags
{
	Paused = 0x01,
	ShowFPS = 0x02,
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
	static uint32_t Flags;
	static uint32_t AudioLatency;
	static double ChannelVolume[5];
	static NesModel Model;
	static uint32_t EmulationSpeed;
	static OverscanDimensions Overscan;

public:
	static void SetFlags(uint32_t flags)
	{
		Flags |= flags;
	}

	static void ClearFlags(uint32_t flags)
	{
		Flags &= ~flags;
	}

	static bool CheckFlag(uint32_t flag)
	{
		return (Flags & flag) == flag;
	}

	static void SetNesModel(NesModel model)
	{
		Model = model;
	}

	static NesModel GetNesModel()
	{
		return Model;
	}

	//0: Muted, 0.5: Default, 1.0: Max volume
	static void SetChannelVolume(AudioChannel channel, double volume)
	{
		ChannelVolume[(int)channel] = volume;
	}

	static void SetAudioLatency(uint32_t msLatency)
	{
		AudioLatency = msLatency;
	}

	//0: No limit, Number: % of default speed (50/60fps)
	static void SetEmulationSpeed(uint32_t emulationSpeed)
	{
		EmulationSpeed = emulationSpeed;
	}

	static uint32_t GetEmulationSpeed()
	{
		return EmulationSpeed;
	}

	static void SetOverscanDimensions(uint8_t left, uint8_t right, uint8_t top, uint8_t bottom)
	{
		Overscan.Left = left;
		Overscan.Right = right;
		Overscan.Top = top;
		Overscan.Bottom = bottom;
	}

	static OverscanDimensions GetOverscanDimensions()
	{
		return Overscan;
	}

	static double GetChannelVolume(AudioChannel channel)
	{
		return ChannelVolume[(int)channel];
	}

	static uint32_t GetAudioLatency()
	{
		return AudioLatency;
	}
};
