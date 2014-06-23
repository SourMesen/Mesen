#include "stdafx.h"
#include "SoundManager.h"

SoundManager::SoundManager(HWND hwnd)
{
	APU::RegisterAudioDevice(this);

	_directSound = 0;
	_primaryBuffer = 0;
	_secondaryBuffer = 0;

	InitializeDirectSound(hwnd);
}

SoundManager::~SoundManager()
{
	Release();
}

bool SoundManager::InitializeDirectSound(HWND hwnd)
{
	HRESULT result;
	DSBUFFERDESC bufferDesc;
	WAVEFORMATEX waveFormat;


	// Initialize the direct sound interface pointer for the default sound device.
	result = DirectSoundCreate8(NULL, &_directSound, NULL);
	if(FAILED(result)) {
		return false;
	}

	// Set the cooperative level to priority so the format of the primary sound buffer can be modified.
	result = _directSound->SetCooperativeLevel(hwnd, DSSCL_PRIORITY);
	if(FAILED(result)) {
		return false;
	}

	// Setup the primary buffer description.
	bufferDesc.dwSize = sizeof(DSBUFFERDESC);
	bufferDesc.dwFlags = DSBCAPS_PRIMARYBUFFER | DSBCAPS_CTRLVOLUME;
	bufferDesc.dwBufferBytes = 0;
	bufferDesc.dwReserved = 0;
	bufferDesc.lpwfxFormat = NULL;
	bufferDesc.guid3DAlgorithm = GUID_NULL;

	// Get control of the primary sound buffer on the default sound device.
	result = _directSound->CreateSoundBuffer(&bufferDesc, &_primaryBuffer, NULL);
	if(FAILED(result)) {
		return false;
	}

	// Setup the format of the primary sound bufffer.
	waveFormat.wFormatTag = WAVE_FORMAT_PCM;
	waveFormat.nSamplesPerSec = 44100;
	waveFormat.wBitsPerSample = 16;
	waveFormat.nChannels = 1;
	waveFormat.nBlockAlign = (waveFormat.wBitsPerSample / 8) * waveFormat.nChannels;
	waveFormat.nAvgBytesPerSec = waveFormat.nSamplesPerSec * waveFormat.nBlockAlign;
	waveFormat.cbSize = 0;

	// Set the primary buffer to be the wave format specified.
	result = _primaryBuffer->SetFormat(&waveFormat);
	if(FAILED(result)) {
		return false;
	}

	// Set the buffer description of the secondary sound buffer that the wave file will be loaded onto.
	bufferDesc.dwSize = sizeof(DSBUFFERDESC);
	bufferDesc.dwFlags = DSBCAPS_CTRLPOSITIONNOTIFY | DSBCAPS_GETCURRENTPOSITION2 | DSBCAPS_GLOBALFOCUS | DSBCAPS_LOCSOFTWARE | DSBCAPS_CTRLVOLUME | DSBCAPS_CTRLFREQUENCY;
	bufferDesc.dwBufferBytes = 0xFFFF;
	bufferDesc.dwReserved = 0;
	bufferDesc.lpwfxFormat = &waveFormat;
	bufferDesc.guid3DAlgorithm = GUID_NULL;

	// Create a temporary sound buffer with the specific buffer settings.
	IDirectSoundBuffer* tempBuffer;
	result = _directSound->CreateSoundBuffer(&bufferDesc, &tempBuffer, NULL);
	if(FAILED(result)) {
		return false;
	}

	// Test the buffer format against the direct sound 8 interface and create the secondary buffer.
	result = tempBuffer->QueryInterface(IID_IDirectSoundBuffer8, (LPVOID*)&_secondaryBuffer);
	if(FAILED(result)) {
		return false;
	}

	// Set volume of the buffer to 100%.
	result = _secondaryBuffer->SetVolume(DSBVOLUME_MAX);
	if(FAILED(result)) {
		return false;
	}

	// Release the temporary buffer.
	tempBuffer->Release();

	return true;
}


void SoundManager::Release()
{
	if(_secondaryBuffer) {
		_secondaryBuffer->Release();
		_secondaryBuffer = nullptr;
	}

	if(_primaryBuffer) {
		_primaryBuffer->Release();
		_primaryBuffer = nullptr;
	}
	
	if(_directSound) {
		_directSound->Release();
		_directSound = nullptr;
	}
}

void SoundManager::ClearSecondaryBuffer()
{
	unsigned char* bufferPtr;
	DWORD bufferSize;
	_secondaryBuffer->Lock(0, 0, (void**)&bufferPtr, (DWORD*)&bufferSize, nullptr, 0, DSBLOCK_ENTIREBUFFER);
	memset(bufferPtr, 0, bufferSize);
	_secondaryBuffer->Unlock((void*)bufferPtr, bufferSize, nullptr, 0);

	_secondaryBuffer->SetCurrentPosition(0);
	_lastWriteOffset = 0;
}

void SoundManager::CopyToSecondaryBuffer(uint8_t *data, uint32_t size)
{
	uint8_t* bufferPtrA;
	uint8_t* bufferPtrB;
	DWORD bufferASize;
	DWORD bufferBSize;

	_secondaryBuffer->Lock(_lastWriteOffset, size, (void**)&bufferPtrA, (DWORD*)&bufferASize, (void**)&bufferPtrB, (DWORD*)&bufferBSize, 0);
	_lastWriteOffset += size;

	memcpy(bufferPtrA, data, bufferASize);
	if(bufferPtrB && bufferBSize > 0) {
		memcpy(bufferPtrB, data + bufferASize, bufferBSize);
	}

	_secondaryBuffer->Unlock((void*)bufferPtrA, bufferASize, (void*)bufferPtrB, bufferBSize);
}

void SoundManager::Pause()
{
	_secondaryBuffer->Stop();
}

void SoundManager::Play()
{
	_secondaryBuffer->Play(0, 0, DSBPLAY_LOOPING);
}

void SoundManager::Reset()
{
	_secondaryBuffer->Stop();
	ClearSecondaryBuffer();
}

void SoundManager::PlayBuffer(int16_t *soundBuffer, uint32_t soundBufferSize)
{
	static const int32_t byteLatency = _latency * (APU::BitsPerSample / 8);
	DWORD status;
	_secondaryBuffer->GetStatus(&status);

	if(!(status & DSBSTATUS_PLAYING)) {
		CopyToSecondaryBuffer((uint8_t*)soundBuffer, soundBufferSize);
		if(_lastWriteOffset >= byteLatency) {
			Play();
		}
	} else {
		CopyToSecondaryBuffer((uint8_t*)soundBuffer, soundBufferSize);
		DWORD currentPlayCursor;
		_secondaryBuffer->GetCurrentPosition(&currentPlayCursor, nullptr);
		
		int32_t playWriteByteLatency = (_lastWriteOffset - currentPlayCursor);
		if(playWriteByteLatency < -byteLatency * 2) {
			playWriteByteLatency = 0xFFFF - currentPlayCursor + _lastWriteOffset;
		}

		int32_t latencyGap = playWriteByteLatency - byteLatency;
		if(abs(latencyGap) > 3000) {
			//Out of sync, move back to where we should be (start of the latency buffer)
			_secondaryBuffer->SetFrequency(44100);
			_secondaryBuffer->SetCurrentPosition(_lastWriteOffset - byteLatency);
		} else if(latencyGap < -200) {
			//Playing too fast, slow down playing
			_secondaryBuffer->SetFrequency(43900);
		} else if(latencyGap > 200) {
			//Playing too slow, speed up
			_secondaryBuffer->SetFrequency(44300);
		} else {
			//Normal playback
			_secondaryBuffer->SetFrequency(44100);
		}
	}
}