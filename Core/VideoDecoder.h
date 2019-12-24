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
class RotateFilter;
class IRenderingDevice;
class VideoHud;
class Console;
struct HdScreenInfo;

struct ScreenSize
{
	int32_t Width;
	int32_t Height;
	double Scale;
};

class VideoDecoder
{
private:
	shared_ptr<Console> _console;
	EmulationSettings* _settings;
	uint16_t *_ppuOutputBuffer = nullptr;
	HdScreenInfo *_hdScreenInfo = nullptr;
	bool _hdFilterEnabled = false;
	uint32_t _frameNumber = 0;

	unique_ptr<thread> _decodeThread;
	unique_ptr<VideoHud> _hud;

	AutoResetEvent _waitForFrame;
	
	atomic<bool> _frameChanged;
	atomic<bool> _stopFlag;
	uint32_t _frameCount = 0;

	ScreenSize _previousScreenSize = {};
	double _previousScale = 0;
	FrameInfo _lastFrameInfo;

	VideoFilterType _videoFilterType = VideoFilterType::None;
	unique_ptr<BaseVideoFilter> _videoFilter;
	shared_ptr<ScaleFilter> _scaleFilter;
	shared_ptr<RotateFilter> _rotateFilter;

	void UpdateVideoFilter();

	void DecodeThread();

public:
	VideoDecoder(shared_ptr<Console> console);
	~VideoDecoder();

	void DecodeFrame(bool synchronous = false);
	void TakeScreenshot();
	void TakeScreenshot(std::stringstream &stream);

	uint32_t GetFrameCount();

	FrameInfo GetFrameInfo();
	void GetScreenSize(ScreenSize &size, bool ignoreScale);

	void UpdateFrameSync(void* frameBuffer, HdScreenInfo *hdScreenInfo = nullptr);
	void UpdateFrame(void* frameBuffer, HdScreenInfo *hdScreenInfo = nullptr);

	bool IsRunning();
	void StartThread();
	void StopThread();
};