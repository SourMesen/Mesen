#pragma once
#include "stdafx.h"
#include "IMemoryHandler.h"
#include "SoundMixer.h"
#include "OggMixer.h"

struct HdPackData;

class HdAudioDevice : public IMemoryHandler
{
private:
	HdPackData *_hdData;
	uint8_t _album;
	uint8_t _flags;
	bool _trackError;
	OggMixer* _oggMixer;
	
	bool PlayBgmTrack(uint8_t track);
	bool PlaySfx(uint8_t sfxNumber);
	void ProcessControlFlags(uint8_t flags);

public:
	HdAudioDevice(HdPackData *hdData);

	void GetMemoryRanges(MemoryRanges &ranges) override;
	void WriteRAM(uint16_t addr, uint8_t value) override;
	uint8_t ReadRAM(uint16_t addr) override;
};