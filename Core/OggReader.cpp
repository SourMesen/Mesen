#include "stdafx.h"
#include "OggReader.h"

OggReader::OggReader()
{
	_done = false;
	_blipLeft = blip_new(10000);
	_blipRight = blip_new(10000);
	_oggBuffer = new int16_t[OggReader::SamplesToRead * 2];
	_outputBuffer = new int16_t[2000];
}

OggReader::~OggReader()
{
	blip_delete(_blipLeft);
	blip_delete(_blipRight);
	delete[] _oggBuffer;
	delete[] _outputBuffer;

	if(_vorbis) {
		stb_vorbis_close(_vorbis);
	}
}

bool OggReader::Init(string filename, bool loop, uint32_t sampleRate, uint32_t startOffset)
{
	int error;
	VirtualFile file = filename;
	_fileData = vector<uint8_t>(100000);
	if(file.ReadFile(_fileData)) {
		_vorbis = stb_vorbis_open_memory(_fileData.data(), (int)_fileData.size(), &error, nullptr);
		if(_vorbis) {
			_loop = loop;
			_oggSampleRate = stb_vorbis_get_info(_vorbis).sample_rate;
			if(startOffset > 0) {
				stb_vorbis_seek(_vorbis, startOffset);
			}
			blip_set_rates(_blipLeft, _oggSampleRate, sampleRate);
			blip_set_rates(_blipRight, _oggSampleRate, sampleRate);
			return true;
		}
	}
	return false;
}

bool OggReader::IsPlaybackOver()
{
	return _done && blip_samples_avail(_blipLeft) == 0;
}

void OggReader::SetSampleRate(int sampleRate)
{
	if(sampleRate != _sampleRate) {
		blip_clear(_blipLeft);
		blip_clear(_blipRight);

		_sampleRate = sampleRate;
		blip_set_rates(_blipLeft, _oggSampleRate, _sampleRate);
		blip_set_rates(_blipRight, _oggSampleRate, _sampleRate);
	}
}

void OggReader::SetLoopFlag(bool loop)
{
	_loop = loop;
}

bool OggReader::LoadSamples()
{
	int samplesReturned = stb_vorbis_get_samples_short_interleaved(_vorbis, 2, _oggBuffer, OggReader::SamplesToRead * 2);

	for(int i = 0; i < samplesReturned; i++) {
		blip_add_delta(_blipLeft, i, i == 0 ? 0 : (_oggBuffer[i * 2] - _oggBuffer[i * 2 - 2]));
		blip_add_delta(_blipRight, i, i == 0 ? 0 : (_oggBuffer[i * 2 + 1] - _oggBuffer[i * 2 - 1]));
	}
	blip_end_frame(_blipLeft, samplesReturned);
	blip_end_frame(_blipRight, samplesReturned);

	if(samplesReturned < OggReader::SamplesToRead) {
		if(_loop) {
			stb_vorbis_seek_start(_vorbis);
			LoadSamples();
		} else {
			_done = true;
		}
	}

	return samplesReturned > 0;
}

void OggReader::ApplySamples(int16_t * buffer, size_t sampleCount, uint8_t volume, double masterVolume)
{
	while(blip_samples_avail(_blipLeft) < (int)sampleCount) {
		if(!LoadSamples()) {
			break;
		}
	}

	int samplesRead = blip_read_samples(_blipLeft, _outputBuffer, (int)sampleCount, 1);
	blip_read_samples(_blipRight, _outputBuffer + 1, (int)sampleCount, 1);

	for(size_t i = 0, len = samplesRead * 2; i < len; i++) {
		buffer[i] += (int16_t)(_outputBuffer[i] * (masterVolume * volume / 255 / 10));
	}
}

uint32_t OggReader::GetOffset()
{
	return stb_vorbis_get_file_offset(_vorbis);
}