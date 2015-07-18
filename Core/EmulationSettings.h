#pragma once

#include "stdafx.h"

enum EmulationFlags
{
	Paused = 0x01,
	LimitFPS = 0x02,
	ShowFPS = 0x04,
};

enum class AudioChannel
{
	Square1 = 0,
	Square2 = 1,
	Triangle = 2,
	Noise = 3,
	DMC = 4
};

class EmulationSettings
{
private:
	static uint32_t Flags;
	static uint32_t AudioLatency;
	static double ChannelVolume[5];

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

	//0: Muted, 0.5: Default, 1.0: Max volume
	static void SetChannelVolume(AudioChannel channel, double volume)
	{
		ChannelVolume[(int)channel] = volume;
	}

	static void SetAudioLatency(uint32_t msLatency)
	{
		AudioLatency = msLatency;
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
