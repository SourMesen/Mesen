#pragma once
#include "stdafx.h"
#include "../Utilities/SimpleLock.h"
#include "EmulationSettings.h"

struct FrameInfo
{
	uint32_t Width;
	uint32_t Height;
	uint32_t BitsPerPixel;
};

class BaseVideoFilter
{
private:
	uint8_t* _outputBuffer = nullptr;
	uint32_t _bufferSize = 0;
	SimpleLock _frameLock;
	OverscanDimensions _overscan;

	void UpdateBufferSize();

protected:
	OverscanDimensions GetOverscan();
	virtual void ApplyFilter(uint16_t *ppuOutputBuffer) = 0;
	virtual void OnBeforeApplyFilter();

public:
	BaseVideoFilter();
	virtual ~BaseVideoFilter();

	uint8_t* GetOutputBuffer();
	void SendFrame(uint16_t *ppuOutputBuffer);
	void TakeScreenshot();

	virtual FrameInfo GetFrameInfo() = 0;
};