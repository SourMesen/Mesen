#pragma once
#include "stdafx.h"
#include <thread>
using std::thread;

#include "../Utilities/SimpleLock.h"
#include "../Utilities/AutoResetEvent.h"
#include "EmulationSettings.h"
#include "HdNesPack.h"


class IRenderingDevice;

class VideoDecoder
{
private:
	static unique_ptr<VideoDecoder> Instance;
	
	IRenderingDevice* _renderer;

	uint16_t *_ppuOutputBuffer = nullptr;
	HdPpuPixelInfo *_hdScreenTiles = nullptr;

	unique_ptr<thread> _decodeThread;
	unique_ptr<thread> _renderThread;

	OverscanDimensions _overscan;
	uint32_t* _frameBuffer = nullptr;
	SimpleLock _screenshotLock;
	AutoResetEvent _waitForFrame;
	AutoResetEvent _waitForRender;

	bool _isHD = false;
	atomic<bool> _frameChanged = false;
	atomic<bool> _stopFlag = false;
	uint32_t _frameCount = 0;

	unique_ptr<HdNesPack> _hdNesPack = nullptr;
	uint32_t _hdScale = 1;

	uint32_t ProcessIntensifyBits(uint16_t ppuPixel);
	void UpdateBufferSize();

	void DecodeThread();
	void RenderThread();

public:
	static VideoDecoder* GetInstance();
	VideoDecoder();
	~VideoDecoder();

	void DecodeFrame();
	void TakeScreenshot(string romFilename);

	uint32_t GetScale();
	uint32_t GetFrameCount();

	void DebugDecodeFrame(uint16_t* inputBuffer, uint32_t* outputBuffer, uint32_t length);

	bool UpdateFrame(void* frameBuffer, HdPpuPixelInfo *screenTiles = nullptr);

	void StartThread();
	void StopThread();

	void RegisterRenderingDevice(IRenderingDevice *renderer);
};