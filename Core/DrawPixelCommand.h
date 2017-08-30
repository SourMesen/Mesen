#pragma once
#include "stdafx.h"
#include "DrawCommand.h"

class DrawPixelCommand : public DrawCommand
{
private:
	int _x, _y, _color;

protected:
	void InternalDraw()
	{
		DrawPixel(_x, _y, _color);
	}

public:
	DrawPixelCommand(int x, int y, int color, int frameCount) :
		DrawCommand(frameCount), _x(x), _y(y), _color(color) 
	{
		if(!(_color & 0xFF000000)) {
			_color |= 0xFF000000;
		}
	}
};
