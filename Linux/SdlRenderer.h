#pragma once
#include <SDL2/SDL.h>
#include "../Core/IRenderingDevice.h"
#include "../Utilities/SimpleLock.h"
#include "../Core/EmulationSettings.h"
#include "../Core/VideoRenderer.h"

struct SDL_Window
{
	const void *magic;
    Uint32 id;
    char *title;
    SDL_Surface *icon;
    int x, y;
    int w, h;
    int min_w, min_h;
    int max_w, max_h;
    Uint32 flags;
};
typedef struct SDL_Window SDL_Window;

class SdlRenderer : public IRenderingDevice
{
private:
	void* _windowHandle;
	SDL_Window* _sdlWindow = nullptr;
	SDL_Renderer *_sdlRenderer = nullptr;
	SDL_Texture *_sdlTexture = nullptr;

	VideoResizeFilter _resizeFilter = VideoResizeFilter::NearestNeighbor;

	SimpleLock _frameLock;
	uint8_t* _frameBuffer;

	const uint32_t _bytesPerPixel = 4;
	uint32_t _screenWidth = 0;
	uint32_t _screenHeight = 0;
	uint32_t _screenBufferSize = 0;
	double _scale = 0;

	uint32_t _nesFrameHeight = 0;
	uint32_t _nesFrameWidth = 0;
	uint32_t _newFrameBufferSize = 0;

	void Init();
	void Cleanup();
	void SetScreenSize(uint32_t width, uint32_t height);

public:
	SdlRenderer(void* windowHandle);
	virtual ~SdlRenderer();

	void UpdateFrame(void *frameBuffer, uint32_t width, uint32_t height);
	void Render();
	void Reset();
};