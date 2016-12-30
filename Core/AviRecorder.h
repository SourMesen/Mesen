#pragma once
#include "stdafx.h"
#include <thread>
#include "../Utilities/AutoResetEvent.h"
#include "../Utilities/AviWriter.h"
#include "../Utilities/SimpleLock.h"

class AviRecorder
{
private:
	std::thread _aviWriterThread;
	
	unique_ptr<AviWriter> _aviWriter;

	string _outputFile;
	SimpleLock _lock;
	AutoResetEvent _waitFrame;

	atomic<bool> _stopFlag;	
	bool _recording;
	uint8_t* _frameBuffer;
	uint32_t _frameBufferLength;
	uint32_t _sampleRate;

public:
	AviRecorder();
	virtual ~AviRecorder();

	bool StartRecording(string filename, VideoCodec codec, uint32_t width, uint32_t height, uint32_t bpp, uint32_t fps, uint32_t audioSampleRate);
	void StopRecording();

	void AddFrame(void* frameBuffer);
	void AddSound(int16_t* soundBuffer, uint32_t sampleCount, uint32_t sampleRate);
	void SendAudio();

	bool IsRecording();
};