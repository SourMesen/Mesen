#pragma once
#include "stdafx.h"
#include <thread>
using std::thread;

#include "../Utilities/SimpleLock.h"
#include "../Utilities/AutoResetEvent.h"
#include "EmulationSettings.h"
#include "FrameInfo.h"

class BaseVideoFilter;
class ScaleFilter;
class IRenderingDevice;
struct HdPpuPixelInfo;

struct ScreenSize
{
	int32_t Width;
	int32_t Height;
	double Scale;
};

class VideoDecoder
{
private:
	static unique_ptr<VideoDecoder> Instance;

	uint16_t *_ppuOutputBuffer = nullptr;
	HdPpuPixelInfo *_hdScreenTiles = nullptr;
	bool _hdFilterEnabled = false;

	unique_ptr<thread> _decodeThread;

	AutoResetEvent _waitForFrame;
	
	atomic<bool> _frameChanged;
	atomic<bool> _stopFlag;
	uint32_t _frameCount = 0;

	ScreenSize _previousScreenSize = {};
	double _previousScale = 0;

	VideoFilterType _videoFilterType = VideoFilterType::None;
	unique_ptr<BaseVideoFilter> _videoFilter;
	unique_ptr<ScaleFilter> _scaleFilter;

	void UpdateVideoFilter();

	void DecodeThread();

public:
	static VideoDecoder* GetInstance();
	VideoDecoder();
	~VideoDecoder();

	static void Release();

	void DecodeFrame();
	void TakeScreenshot();
	void TakeScreenshot(std::stringstream &stream);

	uint32_t GetFrameCount();

	FrameInfo GetFrameInfo();
	void GetScreenSize(ScreenSize &size, bool ignoreScale);

	void DebugDecodeFrame(uint16_t* inputBuffer, uint32_t* outputBuffer, uint32_t length);

	void UpdateFrameSync(void* frameBuffer, HdPpuPixelInfo *screenTiles = nullptr);
	void UpdateFrame(void* frameBuffer, HdPpuPixelInfo *screenTiles = nullptr);

	bool IsRunning();
	void StartThread();
	void StopThread();
};