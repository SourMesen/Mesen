#pragma once
#include "stdafx.h"
#include "../Utilities/SimpleLock.h"
#include "EmulationSettings.h"
#include "FrameInfo.h"
#include "VideoHud.h"

class BaseVideoFilter
{
private:
	VideoHud _videoHud;
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