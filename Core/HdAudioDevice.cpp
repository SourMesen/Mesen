#include "stdafx.h"
#include "HdAudioDevice.h"
#include "HdData.h"

HdAudioDevice::HdAudioDevice(HdPackData * hdData)
{
	_hdData = hdData;
	_album = 0;
	_flags = 0;
	_trackError = false;
	_oggMixer = SoundMixer::GetOggMixer();
}

bool HdAudioDevice::PlayBgmTrack(uint8_t track)
{
	auto result = _hdData->BgmFilesById.find(_album * 256 + track);
	if(result != _hdData->BgmFilesById.end()) {
		return !_oggMixer->Play(result->second, false);
	} else {
		MessageManager::Log("[HDPack] Invalid album+track combination: " + std::to_string(_album) + ":" + std::to_string(track));
		return false;
	}
}

bool HdAudioDevice::PlaySfx(uint8_t sfxNumber)
{
	auto result = _hdData->SfxFilesById.find(_album * 256 + sfxNumber);
	if(result != _hdData->SfxFilesById.end()) {
		return !_oggMixer->Play(result->second, true);
	} else {
		MessageManager::Log("[HDPack] Invalid album+sfx number combination: " + std::to_string(_album) + ":" + std::to_string(sfxNumber));
		return false;
	}
}

void HdAudioDevice::ProcessControlFlags(uint8_t flags)
{
	_oggMixer->SetPausedFlag((flags & 0x01) == 0x01);
	if(flags & 0x02) {
		_oggMixer->StopBgm();
	}
	if(flags & 0x04) {
		_oggMixer->StopSfx();
	}
}

void HdAudioDevice::GetMemoryRanges(MemoryRanges & ranges)
{
	uint16_t baseRegisterAddr = (_hdData->OptionFlags & (int)HdPackOptions::AlternateRegisterRange) ? 0x5000 : 0x4000;
	ranges.SetAllowOverride();
	ranges.AddHandler(MemoryOperation::Any, baseRegisterAddr + 0xFF9, baseRegisterAddr + 0xFFF);
}

void HdAudioDevice::WriteRAM(uint16_t addr, uint8_t value)
{
	switch(addr & 0xFFF) {
		//Playback Options
		//Bit 0: Loop BGM
		//Bit 1-7: Unused, reserved - must be 0
		case 0xFF9: _oggMixer->SetPlaybackOptions(value); break;

		//Playback Control
		//Bit 0: Toggle Pause/Resume (only affects BGM)
		//Bit 1: Stop BGM
		//Bit 2: Stop all SFX
		//Bit 3-7: Unused, reserved - must be 0
		case 0xFFA: ProcessControlFlags(value); break;

		//BGM Volume: 0 = mute, 255 = max
		//Also has an immediate effect on currently playing BGM
		case 0xFFB: _oggMixer->SetBgmVolume(value); break;

		//SFX Volume: 0 = mute, 255 = max
		//Also has an immediate effect on all currently playing SFX
		case 0xFFC: _oggMixer->SetSfxVolume(value); break;

		//Album number: 0-255 (Allows for up to 64k BGM and SFX tracks)
		//No immediate effect - only affects subsequent $4FFE/$4FFF writes
		case 0xFFD: _album = value; break;

		//Play BGM track (0-255 = track number)
		//Stop the current BGM and starts a new track
		case 0xFFE: _trackError = PlayBgmTrack(value); break;

		//Play sound effect (0-255 = sfx number)
		//Plays a new sound effect (no limit to the number of simultaneous sound effects)
		case 0xFFF: _trackError = PlaySfx(value); break;
	}
}

uint8_t HdAudioDevice::ReadRAM(uint16_t addr)
{
	switch(addr & 0xFFF) {
		case 0xFFA:
			//Status
			return (
				(_oggMixer->IsBgmPlaying() ? 1 : 0) |
				(_oggMixer->IsSfxPlaying() ? 2 : 0) |
				(_trackError ? 4 : 0)
			);

		case 0xFFC: return 'N'; //NES
		case 0xFFD: return 'E'; //Enhanced
		case 0xFFE: return 'A'; //Audio
		case 0xFFF: return 1; //Revision
	}

	return 0;
}
