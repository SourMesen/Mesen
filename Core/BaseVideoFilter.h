#pragma once
#include "stdafx.h"
#include "../Utilities/SimpleLock.h"
#include "EmulationSettings.h"
#include "FrameInfo.h"
#include "VideoHud.h"

class BaseVideoFilter
{
private:
	uint32_t* _outputBuffer = nullptr;
	uint32_t _bufferSize = 0;
	SimpleLock _frameLock;
	OverscanDimensions _overscan;
	bool _isOddFrame;

	void UpdateBufferSize();

protected:
	virtual void ApplyFilter(uint16_t *ppuOutputBuffer) = 0;
	virtual void OnBeforeApplyFilter();
	bool IsOddFrame();

public:
	BaseVideoFilter();
	virtual ~BaseVideoFilter();

	uint32_t* GetOutputBuffer();
	void SendFrame(uint16_t *ppuOutputBuffer, uint32_t frameNumber);
	void TakeScreenshot(VideoFilterType filterType);
	void TakeScreenshot(VideoFilterType filterType, string filename, std::stringstream *stream = nullptr);

	virtual OverscanDimensions GetOverscan();
	virtual FrameInfo GetFrameInfo() = 0;
};