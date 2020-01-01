#include "stdafx.h"
#include "WavReader.h"

shared_ptr<WavReader> WavReader::Create(uint8_t* wavData, uint32_t length)
{
	//Basic WAV file read checks (should be using FourCC logic instead, but this will do for now)
	if(!wavData || length < 100) {
		return nullptr;
	}
	if(memcmp(wavData, "RIFF", 4) || memcmp(wavData + 8, "WAVE", 4) || memcmp(wavData + 12, "fmt ", 4)) {
		return nullptr;
	}

	uint32_t channelCount = wavData[22] | (wavData[23] << 8);
	if(channelCount != 1) {
		//Only mono files are supported at the moment
		return nullptr;
	}

	uint32_t fmtSize = wavData[16] | (wavData[17] << 8) | (wavData[18] << 16) | (wavData[19] << 24);
	if(memcmp(wavData + 20 + fmtSize, "data", 4)) {
		//Couldn't find data chunk
		return nullptr;
	}

	uint32_t bitsPerSample = wavData[34] | (wavData[35] << 8);
	if(bitsPerSample != 16) {
		//Only support 16-bit samples for now
		return nullptr;
	}

	uint32_t sampleRate = wavData[24] | (wavData[25] << 8) | (wavData[26] << 16) | (wavData[27] << 24);

	shared_ptr<WavReader> r(new WavReader());
	r->_fileData = wavData;
	r->_fileSize = length;
	r->_fileSampleRate = sampleRate;
	r->_dataStartOffset = 28 + fmtSize;
	return r;
}

WavReader::WavReader()
{
	_done = true;
	_prevSample = 0;
	_sampleRate = 0;
	_blip = blip_new(10000);
	_outputBuffer = new int16_t[20000];
}

WavReader::~WavReader()
{
	blip_delete(_blip);
	delete[] _outputBuffer;
}

void WavReader::Play(uint32_t startSample)
{
	_prevSample = 0;
	_done = false;
	_fileOffset = _dataStartOffset + startSample * 2;
	
	blip_clear(_blip);
	blip_set_rates(_blip, _fileSampleRate, _sampleRate);
}

bool WavReader::IsPlaybackOver()
{
	return _done;
}

void WavReader::SetSampleRate(uint32_t sampleRate)
{
	_sampleRate = sampleRate;
}

void WavReader::LoadSamples(uint32_t samplesToLoad)
{
	uint32_t samplesRead = 0;

	for(uint32_t i = _fileOffset; i < _fileSize && samplesRead < samplesToLoad; i+=2) {
		int16_t sample = _fileData[i] | (_fileData[i + 1] << 8);

		blip_add_delta(_blip, samplesRead, sample - _prevSample);

		_prevSample = sample;

		_fileOffset += 2;
		samplesRead++;

		if(samplesRead < samplesToLoad && i + 2 >= _fileSize) {
			_done = true;
			break;
		}
	}

	blip_end_frame(_blip, samplesRead);
}

void WavReader::ApplySamples(int16_t *buffer, size_t sampleCount, double volume)
{
	if(_done) {
		return;
	}

	//Volume is set to 10 when volume is set to 100% in the UI
	volume /= 10.0;

	LoadSamples((uint32_t)sampleCount * _fileSampleRate / _sampleRate + 1 - blip_samples_avail(_blip));

	int samplesRead = blip_read_samples(_blip, _outputBuffer, (int)sampleCount, 0);
	for(size_t i = 0, len = samplesRead; i < len; i++) {
		int16_t sample = (int16_t)((int32_t)_outputBuffer[i] * volume);

		//Apply to both left and right channels
		buffer[i*2] += sample;
		buffer[i*2+1] += sample;
	}
}

int32_t WavReader::GetPosition()
{
	return _done ? -1 : (_fileOffset - _dataStartOffset) / 2;
}