#pragma once
#include "stdafx.h"
#include "../Utilities/SimpleLock.h"
#include "EmulationSettings.h"
#include "HdNesPack.h"

class VideoDecoder
{
private:
	static unique_ptr<VideoDecoder> Instance;

	OverscanDimensions _overscan;
	uint32_t* _frameBuffer = nullptr;
	SimpleLock _frameLock;

	HdNesPack* _hdNesPack = nullptr;
	uint32_t _hdScale = 1;

	uint32_t ProcessIntensifyBits(uint16_t ppuPixel);
	void UpdateBufferSize(uint8_t hdScale);

public:
	static VideoDecoder* GetInstance();
	~VideoDecoder();

	uint32_t* DecodeFrame(uint16_t* inputBuffer);
	void TakeScreenshot(string romFilename);

	uint32_t GetScale();
	uint32_t* DecodeHdFrame(uint16_t* inputBuffer, HdPpuPixelInfo *screenTiles);

	void DebugDecodeFrame(uint16_t* inputBuffer, uint32_t* outputBuffer, uint32_t length);
};