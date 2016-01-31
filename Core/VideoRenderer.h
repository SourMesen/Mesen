#pragma once
#include "stdafx.h"
#include <thread>
#include "../Utilities/AutoResetEvent.h"

class IRenderingDevice;

class VideoRenderer
{
private:
	static unique_ptr<VideoRenderer> Instance;

	AutoResetEvent _waitForRender;
	unique_ptr<std::thread> _renderThread;
	IRenderingDevice* _renderer = nullptr;
	atomic<bool> _stopFlag = false;

	void RenderThread();

public:
	static VideoRenderer* GetInstance();
	VideoRenderer();
	~VideoRenderer();

	void StartThread();
	void StopThread();

	void UpdateFrame(void *frameBuffer, uint32_t width, uint32_t height);
	void RegisterRenderingDevice(IRenderingDevice *renderer);
	void UnregisterRenderingDevice(IRenderingDevice *renderer);
};