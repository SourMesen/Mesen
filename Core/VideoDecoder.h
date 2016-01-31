#pragma once
#include "stdafx.h"
#include <thread>
using std::thread;

#include "../Utilities/SimpleLock.h"
#include "../Utilities/AutoResetEvent.h"
#include "EmulationSettings.h"
#include "HdNesPack.h"

class BaseVideoFilter;
class IRenderingDevice;

struct ScreenSize
{
	int32_t Width;
	int32_t Height;
};

class VideoDecoder
{
private:
	static unique_ptr<VideoDecoder> Instance;

	uint16_t *_ppuOutputBuffer = nullptr;
	HdPpuPixelInfo *_hdScreenTiles = nullptr;

	unique_ptr<thread> _decodeThread;

	AutoResetEvent _waitForFrame;
	
	atomic<bool> _frameChanged = false;
	atomic<bool> _stopFlag = false;
	uint32_t _frameCount = 0;

	VideoFilterType _videoFilterType = VideoFilterType::None;
	unique_ptr<BaseVideoFilter> _videoFilter = nullptr;

	void UpdateVideoFilter();

	void DecodeThread();

public:
	static VideoDecoder* GetInstance();
	VideoDecoder();
	~VideoDecoder();

	void DecodeFrame();
	void TakeScreenshot(string romFilename);

	uint32_t GetFrameCount();

	void GetScreenSize(ScreenSize &size);

	void DebugDecodeFrame(uint16_t* inputBuffer, uint32_t* outputBuffer, uint32_t length);

	void UpdateFrame(void* frameBuffer, HdPpuPixelInfo *screenTiles = nullptr);

	bool IsRunning();
	void StartThread();
	void StopThread();
};