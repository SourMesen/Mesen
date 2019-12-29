﻿#include "SdlRenderer.h"
#include "../Core/Console.h"
#include "../Core/Debugger.h"
#include "../Core/VideoRenderer.h"
#include "../Core/VideoDecoder.h"
#include "../Core/EmulationSettings.h"

SimpleLock SdlRenderer::_frameLock;

SdlRenderer::SdlRenderer(shared_ptr<Console> console, void* windowHandle, bool registerAsMessageManager) : BaseRenderer(console, registerAsMessageManager), _windowHandle(windowHandle)
{
	_frameBuffer = nullptr;
	_requiredWidth = 256;
	_requiredHeight = 240;
	
	shared_ptr<VideoRenderer> videoRenderer = _console->GetVideoRenderer();
	if(videoRenderer) {
		_console->GetVideoRenderer()->RegisterRenderingDevice(this);
	}
}

SdlRenderer::~SdlRenderer()
{
	shared_ptr<VideoRenderer> videoRenderer = _console->GetVideoRenderer();
	if(videoRenderer) {
		videoRenderer->UnregisterRenderingDevice(this);
	}
	
	auto lock = _frameLock.AcquireSafe();
	Cleanup();
	delete[] _frameBuffer;	
}

void SdlRenderer::SetFullscreenMode(bool fullscreen, void* windowHandle, uint32_t monitorWidth, uint32_t monitorHeight)
{
	//TODO: Implement exclusive fullscreen for Linux
}

bool SdlRenderer::Init()
{
	auto log = [](const char* msg) {
		MessageManager::Log(msg);
		MessageManager::Log(SDL_GetError());		
	};

	if(SDL_InitSubSystem(SDL_INIT_VIDEO) != 0) {
		log("[SDL] Failed to initialize video subsystem.");
		return false;
	};

	_sdlWindow = SDL_CreateWindowFrom(_windowHandle);
	if(!_sdlWindow) {
		log("[SDL] Failed to create window from handle.");
		return false;
	}

	//Hack to make this work properly - otherwise SDL_CreateRenderer never returns
	_sdlWindow->flags |= SDL_WINDOW_OPENGL;

	if(SDL_GL_LoadLibrary(NULL) != 0) {
		log("[SDL] Failed to initialize OpenGL, attempting to continue with initialization.");
	}

	uint32_t baseFlags = _vsyncEnabled ? SDL_RENDERER_PRESENTVSYNC : 0;

	_sdlRenderer = SDL_CreateRenderer(_sdlWindow, -1, baseFlags | SDL_RENDERER_ACCELERATED);
	if(!_sdlRenderer) {
		log("[SDL] Failed to create accelerated renderer.");

		MessageManager::Log("[SDL] Attempting to create software renderer...");
		_sdlRenderer = SDL_CreateRenderer(_sdlWindow, -1, baseFlags | SDL_RENDERER_SOFTWARE);
		if(!_sdlRenderer) {
			log("[SDL] Failed to create software renderer.");
			return false;
		}		
	}

	_sdlTexture = SDL_CreateTexture(_sdlRenderer, SDL_PIXELFORMAT_ARGB8888, SDL_TEXTUREACCESS_STREAMING, _nesFrameWidth, _nesFrameHeight);
	if(!_sdlTexture) {
		string msg = "[SDL] Failed to create texture: " + std::to_string(_nesFrameWidth) + "x" + std::to_string(_nesFrameHeight);
		log(msg.c_str());
		return false;
	}

	_spriteFont.reset(new SpriteFont(_sdlRenderer, "Resources/Font.24.spritefont"));
	_largeFont.reset(new SpriteFont(_sdlRenderer, "Resources/Font.64.spritefont"));

	SDL_SetWindowSize(_sdlWindow, _screenWidth, _screenHeight);

	return true;
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
}

void SdlRenderer::Reset()
{
	Cleanup();
	if(Init()) {
		_console->GetVideoRenderer()->RegisterRenderingDevice(this);
	} else {
		Cleanup();
	}
}

void SdlRenderer::SetScreenSize(uint32_t width, uint32_t height)
{
	ScreenSize screenSize;
	_console->GetVideoDecoder()->GetScreenSize(screenSize, false);

	if(_screenHeight != (uint32_t)screenSize.Height || _screenWidth != (uint32_t)screenSize.Width || _nesFrameHeight != height || _nesFrameWidth != width || _resizeFilter != _console->GetSettings()->GetVideoResizeFilter() || _vsyncEnabled != _console->GetSettings()->CheckFlag(EmulationFlags::VerticalSync)) {
		_vsyncEnabled = _console->GetSettings()->CheckFlag(EmulationFlags::VerticalSync);

		_nesFrameHeight = height;
		_nesFrameWidth = width;
		_newFrameBufferSize = width*height;

		_screenHeight = screenSize.Height;
		_screenWidth = screenSize.Width;

		_resizeFilter = _console->GetSettings()->GetVideoResizeFilter();

		SDL_SetHint(SDL_HINT_RENDER_SCALE_QUALITY, _resizeFilter == VideoResizeFilter::Bilinear ? "1" : "0");
		_screenBufferSize = _screenHeight*_screenWidth;

		Reset();
	}	
}

void SdlRenderer::UpdateFrame(void *frameBuffer, uint32_t width, uint32_t height)
{
	_frameLock.Acquire();
	if(_frameBuffer == nullptr || _requiredWidth != width || _requiredHeight != height) {
		_requiredWidth = width;
		_requiredHeight = height;
		
		delete[] _frameBuffer;
		_frameBuffer = new uint32_t[width*height];
		memset(_frameBuffer, 0, width*height*4);	
	}
	
	memcpy(_frameBuffer, frameBuffer, width*height*_bytesPerPixel);
	_frameChanged = true;	
	_frameLock.Release();
}

void SdlRenderer::Render()
{
	SetScreenSize(_requiredWidth, _requiredHeight);
	
	if(!_sdlRenderer || !_sdlTexture) {
		return;
	}
	
	bool paused = _console->IsPaused() && _console->IsRunning();
	bool disableOverlay = _console->GetSettings()->CheckFlag(EmulationFlags::HidePauseOverlay);
	shared_ptr<Debugger> debugger = _console->GetDebugger(false);
	if(debugger && debugger->IsExecutionStopped()) {
		paused = debugger->IsPauseIconShown();
		disableOverlay = true;
	}

	if(_noUpdateCount > 10 || _frameChanged || paused || IsMessageShown()) {	
		SDL_RenderClear(_sdlRenderer);

		uint8_t *textureBuffer;
		int rowPitch;
		SDL_LockTexture(_sdlTexture, nullptr, (void**)&textureBuffer, &rowPitch);
		{
			auto frameLock = _frameLock.AcquireSafe();
			if(_frameBuffer && _nesFrameWidth == _requiredWidth && _nesFrameHeight == _requiredHeight) {
				uint32_t* ppuFrameBuffer = _frameBuffer;
				for(uint32_t i = 0, iMax = _nesFrameHeight; i < iMax; i++) {
					memcpy(textureBuffer, ppuFrameBuffer, _nesFrameWidth*_bytesPerPixel);
					ppuFrameBuffer += _nesFrameWidth;
					textureBuffer += rowPitch;
				}
			}
		}
		SDL_UnlockTexture(_sdlTexture);

		if(_frameChanged) {
			_renderedFrameCount++;
			_frameChanged = false;
		}

		SDL_Rect source = {0, 0, (int)_nesFrameWidth, (int)_nesFrameHeight };
		SDL_Rect dest = {0, 0, (int)_screenWidth, (int)_screenHeight };
		SDL_RenderCopy(_sdlRenderer, _sdlTexture, &source, &dest);

		if(paused && !_console->GetSettings()->CheckFlag(EmulationFlags::HidePauseOverlay)) {
			DrawPauseScreen(disableOverlay);
		} else if(_console->GetVideoDecoder()->IsRunning()) {
			DrawCounters();
		}

		DrawToasts();

		SDL_RenderPresent(_sdlRenderer);
	} else {
		_noUpdateCount++;
	}
}

void SdlRenderer::DrawPauseScreen(bool disableOverlay)
{
	if(disableOverlay) {
		DrawString(L"I", 15, 15, 106, 90, 205, 168);
		DrawString(L"I", 23, 15, 106, 90, 205, 168);
	} else {
		uint32_t textureData = 0x222222AA;
		SDL_Surface* surf = SDL_CreateRGBSurfaceFrom((void*)&textureData, 1, 1, 32, 4, 0xFF000000, 0x00FF0000, 0x0000FF00, 0x000000FF);
		if(surf) {
			SDL_Texture* texture = SDL_CreateTextureFromSurface(_sdlRenderer, surf);
			if(texture) {
				SDL_Rect source = { 0, 0, 1, 1 };
				SDL_Rect dest = { 0, 0, (int)_screenWidth, (int)_screenHeight };
				SDL_RenderCopy(_sdlRenderer, texture, &source, &dest);
				SDL_DestroyTexture(texture);
			}
			SDL_FreeSurface(surf);
		}

		XMFLOAT2 size = _largeFont->MeasureString(L"PAUSE");
		_largeFont->DrawString(_sdlRenderer, L"PAUSE", (int)(_screenWidth / 2 - size.x / 2), (int)(_screenHeight / 2 - size.y / 2 - 8), 250, 235, 215);

		string utf8Message = _console->GetSettings()->GetPauseScreenMessage();
		if(utf8Message.size() > 0) {
			std::wstring message = utf8::utf8::decode(utf8Message);
			float width = MeasureString(message);
			DrawString(message, _screenWidth - width - 20, _screenHeight - 40);
		}
	}
}

void SdlRenderer::DrawString(std::wstring message, int x, int y, uint8_t r, uint8_t g, uint8_t b, uint8_t opacity)
{
	const wchar_t *text = message.c_str();
	_spriteFont->DrawString(_sdlRenderer, text, x, y, r, g, b);
}

float SdlRenderer::MeasureString(std::wstring text)
{
	return _spriteFont->MeasureString(text.c_str()).x;
}

bool SdlRenderer::ContainsCharacter(wchar_t character)
{
	return _spriteFont->ContainsCharacter(character);
}
