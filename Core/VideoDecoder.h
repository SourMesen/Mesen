#pragma once
#include "stdafx.h"
#include "../Utilities/SimpleLock.h"
#include "EmulationSettings.h"

class VideoDecoder
{
private:
	static unique_ptr<VideoDecoder> Instance;

	OverscanDimensions _overscan;
	uint32_t* _frameBuffer = nullptr;
	SimpleLock _frameLock;

	uint32_t ProcessIntensifyBits(uint16_t ppuPixel);
	void UpdateBufferSize();

public:
	static VideoDecoder* GetInstance();
	~VideoDecoder();

	uint32_t* DecodeFrame(uint16_t* inputBuffer);
	void TakeScreenshot(string romFilename);

	void DebugDecodeFrame(uint16_t* inputBuffer, uint32_t* outputBuffer, uint32_t length);
};