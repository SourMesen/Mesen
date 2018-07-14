#include "stdafx.h"
#include "OggReader.h"
#include "OggMixer.h"

enum class OggPlaybackOptions
{
	None = 0x00,
	Loop = 0x01
};

OggMixer::OggMixer()
{
}

void OggMixer::Reset(uint32_t sampleRate)
{
	_bgm.reset();
	_sfx.clear();
	_sfxVolume = 128;
	_bgmVolume = 128;
	_options = 0;
	_sampleRate = sampleRate;
	_paused = false;
}

void OggMixer::SetPlaybackOptions(uint8_t options)
{
	_options = options;
	
	bool loop = (options & (int)OggPlaybackOptions::Loop) != 0;
	if(_bgm) {
		_bgm->SetLoopFlag(loop);
	}
}

void OggMixer::SetPausedFlag(bool paused)
{
	_paused = paused;
}

void OggMixer::StopBgm()
{
	_bgm.reset();
}

void OggMixer::StopSfx()
{
	_sfx.clear();
}

void OggMixer::SetBgmVolume(uint8_t volume)
{
	_bgmVolume = volume;
}

void OggMixer::SetSfxVolume(uint8_t volume)
{
	_sfxVolume = volume;
}

bool OggMixer::IsBgmPlaying()
{
	return !_paused && _bgm;
}

bool OggMixer::IsSfxPlaying()
{
	return _sfx.size() > 0;
}

void OggMixer::SetSampleRate(int sampleRate)
{
	_sampleRate = sampleRate;
	if(_bgm) {
		_bgm->SetSampleRate(sampleRate);
	}
	for(shared_ptr<OggReader> &sfx : _sfx) {
		sfx->SetSampleRate(sampleRate);
	}
}

bool OggMixer::Play(string filename, bool isSfx, uint32_t startOffset)
{
	shared_ptr<OggReader> reader(new OggReader());
	bool loop = !isSfx && (_options & (int)OggPlaybackOptions::Loop) != 0;
	if(reader->Init(filename, loop, _sampleRate, startOffset)) {
		if(isSfx) {
			_sfx.push_back(reader);
		} else {
			_bgm = reader;
		}
		return true;
	}
	return false;
}

void OggMixer::ApplySamples(int16_t * buffer, size_t sampleCount, double masterVolumne)
{
	if(_bgm && !_paused) {
		_bgm->ApplySamples(buffer, sampleCount, _bgmVolume, masterVolumne);
		if(_bgm->IsPlaybackOver()) {
			_bgm.reset();
		}
	}
	for(shared_ptr<OggReader> &sfx : _sfx) {
		sfx->ApplySamples(buffer, sampleCount, _sfxVolume, masterVolumne);
	}
	_sfx.erase(std::remove_if(_sfx.begin(), _sfx.end(), [](const shared_ptr<OggReader>& o) { return o->IsPlaybackOver(); }), _sfx.end());
}

int OggMixer::GetBgmOffset()
{
	if(_bgm) {
		return _bgm->GetOffset();
	} else {
		return -1;
	}
}