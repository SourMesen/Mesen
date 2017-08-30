#pragma once
#include "stdafx.h"
#include "DrawCommand.h"

class DrawRectangleCommand : public DrawCommand
{
private:
	int _x, _y, _width, _height, _color;
	bool _fill;

protected:
	void InternalDraw()
	{
		if(_fill) {
			for(int j = 0; j < _height; j++) {
				for(int i = 0; i < _width; i++) {
					DrawPixel(_x + i, _y + j, _color);
				}
			}
		} else {
			for(int i = 0; i < _width; i++) {
				DrawPixel(_x + i, _y, _color);
				DrawPixel(_x + i, _y + _height - 1, _color);
			}
			for(int i = 0; i < _height; i++) {
				DrawPixel(_x, _y + i, _color);
				DrawPixel(_x + _width - 1, _y + i, _color);
			}
		}
	}

public:
	DrawRectangleCommand(int x, int y, int width, int height, int color, bool fill, int frameCount) :
		DrawCommand(frameCount), _x(x), _y(y), _width(width), _height(height), _color(color), _fill(fill)
	{
		if(!(_color & 0xFF000000)) {
			_color |= 0xFF000000;
		}
	}
};