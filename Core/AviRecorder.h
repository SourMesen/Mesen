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

	uint32_t _fps;
	uint32_t _width;
	uint32_t _height;

	uint32_t GetFps();

public:
	AviRecorder();
	virtual ~AviRecorder();

	bool StartRecording(string filename, VideoCodec codec, uint32_t width, uint32_t height, uint32_t bpp, uint32_t audioSampleRate, uint32_t compressionLevel);
	void StopRecording();

	void AddFrame(void* frameBuffer, uint32_t width, uint32_t height);
	void AddSound(int16_t* soundBuffer, uint32_t sampleCount, uint32_t sampleRate);

	bool IsRecording();
};