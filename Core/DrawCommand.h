#pragma once
#include "stdafx.h"
#include "EmulationSettings.h"

class DrawCommand
{
private:
	int _frameCount;
	uint32_t* _argbBuffer;
	OverscanDimensions _overscan;

protected:
	virtual void InternalDraw() = 0;
	__forceinline void DrawPixel(uint32_t x, uint32_t y, int color)
	{
		if(x < _overscan.Left || x >= _overscan.Left + _overscan.GetScreenWidth() ||
			y < _overscan.Top || y >= _overscan.Top + _overscan.GetScreenHeight()) {
			//In overscan (out of bounds), skip drawing
			return;
		}

		if((color & 0xFF000000) != 0xFF000000) {
			BlendColors((uint8_t*)&_argbBuffer[(y - _overscan.Top)*_overscan.GetScreenWidth() + (x - _overscan.Left)], (uint8_t*)&color);
		} else {
			_argbBuffer[(y - _overscan.Top)*_overscan.GetScreenWidth() + (x - _overscan.Left)] = color;
		}
	}

	__forceinline void BlendColors(uint8_t output[4], uint8_t input[4])
	{
		uint8_t alpha = input[3] + 1;
		uint8_t invertedAlpha = 256 - input[3];
		output[0] = (uint8_t)((alpha * input[0] + invertedAlpha * output[0]) >> 8);
		output[1] = (uint8_t)((alpha * input[1] + invertedAlpha * output[1]) >> 8);
		output[2] = (uint8_t)((alpha * input[2] + invertedAlpha * output[2]) >> 8);
		output[3] = 0xFF;
	}

public:
	DrawCommand(int frameCount)
	{ 
		_frameCount = frameCount > 0 ? frameCount : -1;
	}

	void Draw(uint32_t* argbBuffer, OverscanDimensions &overscan)
	{
		_argbBuffer = argbBuffer;
		_overscan = overscan;

		InternalDraw();

		_frameCount--;
	}

	bool Expired()
	{
		return _frameCount == 0;
	}
};
