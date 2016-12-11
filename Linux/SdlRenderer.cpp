#include "SdlRenderer.h"
#include "../Core/VideoRenderer.h"
#include "../Core/VideoDecoder.h"
#include "../Core/EmulationSettings.h"

SdlRenderer::SdlRenderer(void* windowHandle) : _windowHandle(windowHandle)
{
	_frameBuffer = nullptr;
	SetScreenSize(256,240);
	VideoRenderer::GetInstance()->RegisterRenderingDevice(this);	
}

SdlRenderer::~SdlRenderer()
{
	VideoRenderer::GetInstance()->UnregisterRenderingDevice(this);
	Cleanup();
}

void SdlRenderer::Init()
{
	SDL_InitSubSystem(SDL_INIT_VIDEO);
	_sdlWindow = SDL_CreateWindowFrom(_windowHandle);

	//Hack to make this work properly - otherwise SDL_CreateRenderer never returns
	_sdlWindow->flags |= SDL_WINDOW_OPENGL;
	SDL_GL_LoadLibrary(NULL);

	_sdlRenderer = SDL_CreateRenderer(_sdlWindow, -1, SDL_RENDERER_ACCELERATED);
	_sdlTexture = SDL_CreateTexture(_sdlRenderer, SDL_PIXELFORMAT_ARGB8888, SDL_TEXTUREACCESS_STREAMING, _nesFrameWidth, _nesFrameHeight);

	_frameBuffer = new uint8_t[_nesFrameHeight*_nesFrameWidth*4];
	memset(_frameBuffer, 0, _nesFrameHeight*_nesFrameWidth*4);
}

void SdlRenderer::Cleanup()
{
	if(_sdlTexture) {
		SDL_DestroyTexture(_sdlTexture);
		_sdlTexture = nullptr;		
	}
	if(_sdlRenderer) {
		SDL_DestroyRenderer(_sdlRenderer);
		_sdlRenderer = nullptr;
	}
	if(_frameBuffer) {
		delete[] _frameBuffer; 
		_frameBuffer = nullptr;
	}
}

void SdlRenderer::Reset()
{
	_frameLock.Acquire();
	Cleanup();
	Init();
	_frameLock.Release();
}

void SdlRenderer::SetScreenSize(uint32_t width, uint32_t height)
{
	ScreenSize screenSize;
	VideoDecoder::GetInstance()->GetScreenSize(screenSize, true);

	double scale = EmulationSettings::GetVideoScale();
	if(_scale != scale || _screenHeight != (uint32_t)screenSize.Height || _screenWidth != (uint32_t)screenSize.Width || _nesFrameHeight != height || _nesFrameWidth != width || _resizeFilter != EmulationSettings::GetVideoResizeFilter()) {
		_frameLock.Acquire();

		_nesFrameHeight = height;
		_nesFrameWidth = width;
		_newFrameBufferSize = width*height;

		_screenHeight = screenSize.Height;
		_screenWidth = screenSize.Width;

		_resizeFilter = EmulationSettings::GetVideoResizeFilter();
		_scale = scale;
		SDL_SetHint(SDL_HINT_RENDER_SCALE_QUALITY, _resizeFilter == VideoResizeFilter::Bilinear ? "1" : "0");
		SDL_RenderSetLogicalSize(_sdlRenderer, _nesFrameWidth, _nesFrameHeight);		

		_screenBufferSize = _screenHeight*_screenWidth;

		Reset();
		_frameLock.Release();
	}
}

void SdlRenderer::UpdateFrame(void *frameBuffer, uint32_t width, uint32_t height)
{
	_frameLock.Acquire();
	SetScreenSize(width, height);
	memcpy(_frameBuffer, frameBuffer, width*height*4);
	_frameLock.Release();
}

void SdlRenderer::Render()
{
	auto lock = _frameLock.AcquireSafe();

	SDL_RenderClear(_sdlRenderer);

	uint8_t *textureBuffer;
	int rowPitch;
	SDL_LockTexture(_sdlTexture, nullptr, (void**)&textureBuffer, &rowPitch);
	uint32_t* ppuFrameBuffer = (uint32_t*)_frameBuffer;
	for(uint32_t i = 0, iMax = _nesFrameHeight; i < iMax; i++) {
		memcpy(textureBuffer, ppuFrameBuffer, _nesFrameWidth*4);
		ppuFrameBuffer += _nesFrameWidth;
		textureBuffer += rowPitch;
	}
	SDL_UnlockTexture(_sdlTexture);

	SDL_RenderCopy(_sdlRenderer, _sdlTexture, nullptr, nullptr);
	SDL_RenderPresent(_sdlRenderer);
}
