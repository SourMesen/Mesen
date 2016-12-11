#include "SdlSoundManager.h"
#include "../Core/EmulationSettings.h"
#include "../Core/SoundMixer.h"

SdlSoundManager::SdlSoundManager()
{
	if(InitializeAudio(44100, false)) {
		SoundMixer::RegisterAudioDevice(this);

		_buffer = new uint8_t[0xFFFF];
	}
}

SdlSoundManager::~SdlSoundManager()
{
	if(_buffer) {
		delete[] _buffer;
	}
}

void SdlSoundManager::FillAudioBuffer(void *userData, uint8_t *stream, int len)
{
	SdlSoundManager* soundManager = (SdlSoundManager*)userData;

	soundManager->ReadFromBuffer(stream, len);
}

bool SdlSoundManager::InitializeAudio(uint32_t sampleRate, bool isStereo)
{
	if(SDL_InitSubSystem(SDL_INIT_AUDIO) != 0) {
		return false;
	}

	int isCapture = 0;

	_sampleRate = sampleRate;
	_isStereo = isStereo;

	SDL_AudioSpec audioSpec;
	SDL_memset(&audioSpec, 0, sizeof(audioSpec));
	audioSpec.freq = sampleRate;
	audioSpec.format = AUDIO_S16SYS; //16-bit samples
	audioSpec.channels = isStereo ? 2 : 1;
	audioSpec.samples = 1024;
	audioSpec.callback = &SdlSoundManager::FillAudioBuffer;
	audioSpec.userdata = this;

	SDL_AudioSpec obtainedSpec;

	_audioDeviceID = SDL_OpenAudioDevice(_deviceName.empty() ? nullptr : _deviceName.c_str(), isCapture, &audioSpec, &obtainedSpec, 0);

	_writePosition = 0;
	_readPosition = 0;

	_needReset = false;

	return _audioDeviceID != 0;
}

string SdlSoundManager::GetAvailableDevices()
{
	string deviceString;
	for(string device : GetAvailableDeviceInfo()) {
		deviceString += device + std::string("||");
	}
	return deviceString;
}

vector<string> SdlSoundManager::GetAvailableDeviceInfo()
{
	vector<string> deviceList;
	int isCapture = 0;
	int deviceCount = SDL_GetNumAudioDevices(isCapture);

	if(deviceCount == -1) {
		//No devices found
	} else {
		for(int i = 0; i < deviceCount; i++) {
			deviceList.push_back(SDL_GetAudioDeviceName(i, isCapture));
		}
	}

	return deviceList;
}

void SdlSoundManager::SetAudioDevice(string deviceName)
{
	if(deviceName.compare(_deviceName) != 0) {
		_deviceName = deviceName;
		_needReset = true;
	}
}

void SdlSoundManager::ReadFromBuffer(uint8_t* output, uint32_t len)
{
	if(_readPosition + len < 65536) {
		memcpy(output, _buffer+_readPosition, len);
		_readPosition += len;
	} else {
		int remainingBytes = (65536 - _readPosition);
		memcpy(output, _buffer+_readPosition, remainingBytes);
		memcpy(output+remainingBytes, _buffer, len - remainingBytes);
		_readPosition = len - remainingBytes;
	}
}

void SdlSoundManager::WriteToBuffer(uint8_t* input, uint32_t len)
{
	if(_writePosition + len < 65536) {
		memcpy(_buffer+_writePosition, input, len);
		_writePosition += len;
	} else {
		int remainingBytes = 65536 - _writePosition;
		memcpy(_buffer+_writePosition, input, remainingBytes);
		memcpy(_buffer, ((uint8_t*)input)+remainingBytes, len - remainingBytes);
		_writePosition = len - remainingBytes;
	}
}

void SdlSoundManager::PlayBuffer(int16_t *soundBuffer, uint32_t sampleCount, uint32_t sampleRate, bool isStereo)
{
	uint32_t bytesPerSample = (SoundMixer::BitsPerSample / 8);
	if(_sampleRate != sampleRate || _isStereo != isStereo || _needReset) {
		Stop();
		InitializeAudio(sampleRate, isStereo);
	}

	if(isStereo) {
		bytesPerSample *= 2;
	}

	int32_t byteLatency = (int32_t)((float)(sampleRate * EmulationSettings::GetAudioLatency()) / 1000.0f * bytesPerSample);
	if(byteLatency != _previousLatency) {
		Stop();
		_previousLatency = byteLatency;
	}

	WriteToBuffer((uint8_t*)soundBuffer, sampleCount * bytesPerSample);

	int32_t playWriteByteLatency = _writePosition - _readPosition;
	if(playWriteByteLatency < 0) {
		playWriteByteLatency = 0xFFFF - _readPosition + _writePosition;
	}

	if(playWriteByteLatency > byteLatency * 3) {
		//Out of sync, resync
		Stop();
		WriteToBuffer((uint8_t*)soundBuffer, sampleCount * bytesPerSample);
		playWriteByteLatency = _writePosition - _readPosition;
	}
	
	if(playWriteByteLatency > byteLatency) {
		//Start playing
		SDL_PauseAudioDevice(_audioDeviceID, 0);
	}
}

void SdlSoundManager::Pause()
{
	SDL_PauseAudioDevice(_audioDeviceID, 1);
}

void SdlSoundManager::Stop()
{
	Pause();
	_readPosition = 0;
	_writePosition = 0;
}
