#include "SdlRenderer.h"
#include "../Core/VideoRenderer.h"
#include "../Core/VideoDecoder.h"
#include "../Core/EmulationSettings.h"

SdlRenderer::SdlRenderer(void* windowHandle) : _windowHandle(windowHandle)
{
	_frameBuffer = nullptr;
	SetScreenSize(256,240);
	VideoRenderer::GetInstance()->RegisterRenderingDevice(this);
	MessageManager::RegisterMessageManager(this);	
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

	_spriteFont.reset(new SpriteFont(_sdlRenderer, "Resources/Font.24.spritefont"));
	_largeFont.reset(new SpriteFont(_sdlRenderer, "Resources/Font.64.spritefont"));

	SDL_SetWindowSize(_sdlWindow, _screenWidth, _screenHeight);

	_frameBuffer = new uint8_t[_nesFrameHeight*_nesFrameWidth*_bytesPerPixel];
	memset(_frameBuffer, 0, _nesFrameHeight*_nesFrameWidth*_bytesPerPixel);
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
	VideoDecoder::GetInstance()->GetScreenSize(screenSize, false);

	if(_screenHeight != (uint32_t)screenSize.Height || _screenWidth != (uint32_t)screenSize.Width || _nesFrameHeight != height || _nesFrameWidth != width || _resizeFilter != EmulationSettings::GetVideoResizeFilter()) {
		_frameLock.Acquire();

		_nesFrameHeight = height;
		_nesFrameWidth = width;
		_newFrameBufferSize = width*height;

		_screenHeight = screenSize.Height;
		_screenWidth = screenSize.Width;

		_resizeFilter = EmulationSettings::GetVideoResizeFilter();

		SDL_SetHint(SDL_HINT_RENDER_SCALE_QUALITY, _resizeFilter == VideoResizeFilter::Bilinear ? "1" : "0");
		_screenBufferSize = _screenHeight*_screenWidth;

		Reset();
		_frameLock.Release();
	}	
}

void SdlRenderer::UpdateFrame(void *frameBuffer, uint32_t width, uint32_t height)
{
	_frameLock.Acquire();
	SetScreenSize(width, height);
	memcpy(_frameBuffer, frameBuffer, width*height*_bytesPerPixel);
	_frameChanged = true;	
	_frameLock.Release();
}

void SdlRenderer::Render()
{
	bool paused = EmulationSettings::IsPaused();
	if(_noUpdateCount > 10 || _frameChanged || paused || IsMessageShown()) {	
		auto lock = _frameLock.AcquireSafe();

		SDL_RenderClear(_sdlRenderer);

		uint8_t *textureBuffer;
		int rowPitch;
		SDL_LockTexture(_sdlTexture, nullptr, (void**)&textureBuffer, &rowPitch);
		uint32_t* ppuFrameBuffer = (uint32_t*)_frameBuffer;
		for(uint32_t i = 0, iMax = _nesFrameHeight; i < iMax; i++) {
			memcpy(textureBuffer, ppuFrameBuffer, _nesFrameWidth*_bytesPerPixel);
			ppuFrameBuffer += _nesFrameWidth;
			textureBuffer += rowPitch;
		}
		SDL_UnlockTexture(_sdlTexture);

		if(_frameChanged) {
			_renderedFrameCount++;
			_frameChanged = false;
		}

		SDL_Rect source = {0, 0, (int)_nesFrameWidth, (int)_nesFrameHeight };
		SDL_Rect dest = {0, 0, (int)_screenWidth, (int)_screenHeight };
		SDL_RenderCopy(_sdlRenderer, _sdlTexture, &source, &dest);

		if(paused && !EmulationSettings::CheckFlag(EmulationFlags::HidePauseOverlay)) {
			DrawPauseScreen();
		} else if(VideoDecoder::GetInstance()->IsRunning()) {
			DrawCounters();
		}

		DrawToasts();

		SDL_RenderPresent(_sdlRenderer);
	} else {
		_noUpdateCount++;
	}
}

void SdlRenderer::DrawPauseScreen()
{
	uint32_t textureData = 0x222222AA;
	SDL_Surface* surf = SDL_CreateRGBSurfaceFrom((void*)&textureData, 1, 1, 32, 4, 0xFF000000, 0x00FF0000, 0x0000FF00, 0x000000FF);
	SDL_Texture* texture = SDL_CreateTextureFromSurface(_sdlRenderer, surf);
	SDL_Rect source = {0, 0, 1, 1 };
	SDL_Rect dest = {0, 0, (int)_screenWidth, (int)_screenHeight };
	SDL_RenderCopy(_sdlRenderer, texture, &source, &dest);
	SDL_DestroyTexture(texture);
	SDL_FreeSurface(surf);

	XMVECTOR stringDimensions = _largeFont->MeasureString(L"PAUSE");
	float* measureF = (float*)&stringDimensions;
	_largeFont->DrawString(_sdlRenderer, L"PAUSE", (int)(_screenWidth / 2 - measureF[0] / 2), (int)(_screenHeight / 2 - measureF[1] / 2 - 8), 250, 235, 215);
}

void SdlRenderer::DrawString(std::wstring message, int x, int y, uint8_t r, uint8_t g, uint8_t b, uint8_t opacity)
{
	const wchar_t *text = message.c_str();
	_spriteFont->DrawString(_sdlRenderer, text, x, y, r, g, b);
}

float SdlRenderer::MeasureString(std::wstring text)
{
	XMVECTOR measure = _spriteFont->MeasureString(text.c_str());
	float* measureF = (float*)&measure;
	return measureF[0];
}

bool SdlRenderer::ContainsCharacter(wchar_t character)
{
	return _spriteFont->ContainsCharacter(character);
}