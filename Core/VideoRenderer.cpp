#include "stdafx.h"
#include "IRenderingDevice.h"
#include "VideoRenderer.h"

unique_ptr<VideoRenderer> VideoRenderer::Instance;

VideoRenderer* VideoRenderer::GetInstance()
{
	if(!Instance) {
		Instance.reset(new VideoRenderer());
	}
	return Instance.get();
}

VideoRenderer::VideoRenderer()
{
	StartThread();
}

VideoRenderer::~VideoRenderer()
{
	_stopFlag = true;
	StopThread();
}

void VideoRenderer::StartThread()
{
	if(!_renderThread) {
		_stopFlag = false;
		_waitForRender.Reset();

		_renderThread.reset(new std::thread(&VideoRenderer::RenderThread, this));
	}
}

void VideoRenderer::StopThread()
{
	_stopFlag = true;
	if(_renderThread) {
		_renderThread->join();
		_renderThread.release();
	}
}

void VideoRenderer::RenderThread()
{
	if(_renderer) {
		_renderer->Reset();
	}

	while(!_stopFlag.load()) {
		//Wait until a frame is ready, or until 16ms have passed (to allow UI to run at a minimum of 60fps)
		_waitForRender.Wait(16);
		if(_renderer) {
			_renderer->Render();
		}
	}
}

void VideoRenderer::UpdateFrame(void *frameBuffer, uint32_t width, uint32_t height)
{
	if(_renderer) {
		_renderer->UpdateFrame(frameBuffer, width, height);
		_waitForRender.Signal();
	}
}

void VideoRenderer::RegisterRenderingDevice(IRenderingDevice *renderer)
{
	_renderer = renderer;
	StartThread();
}

void VideoRenderer::UnregisterRenderingDevice(IRenderingDevice *renderer)
{
	if(_renderer == renderer) {
		StopThread();
		_renderer = nullptr;
	}
}