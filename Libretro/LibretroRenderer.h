#pragma once
#include "stdafx.h"
#include "../Core/IRenderingDevice.h"
#include "../Core/VideoRenderer.h"
#include "libretro.h"

class LibretroRenderer : public IRenderingDevice
{
private:
	retro_video_refresh_t _sendFrame = nullptr;
	bool _skipMode = false;

public:
	LibretroRenderer()
	{
		VideoRenderer::GetInstance()->RegisterRenderingDevice(this);
	}

	~LibretroRenderer()
	{
		VideoRenderer::GetInstance()->UnregisterRenderingDevice(this);
	}

	// Inherited via IRenderingDevice
	virtual void UpdateFrame(void *frameBuffer, uint32_t width, uint32_t height) override
	{
		if(!_skipMode && _sendFrame) {
			_sendFrame(frameBuffer, width, height, sizeof(uint32_t) * width);
		}
	}

	void SetSendFrame(retro_video_refresh_t sendFrame)
	{
		_sendFrame = sendFrame;
	}	

	void SetSkipMode(bool skip)
	{
		_skipMode = skip;
	}
	
	virtual void Render() override
	{
	}

	virtual void Reset() override
	{
	}

	virtual void SetFullscreenMode(bool fullscreen, void *windowHandle, uint32_t monitorWidth, uint32_t monitorHeight) override
	{
	}
};