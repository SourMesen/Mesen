#pragma once
#include "stdafx.h"
#include "DrawCommand.h"

class DrawLineCommand : public DrawCommand
{
private:
	int _x, _y, _x2, _y2, _color;

protected:
	void InternalDraw()
	{
		int xDiff = _x2 - _x;
		int yDiff = _y2 - _y;
		if(xDiff == 0) {
			for(int i = _y; i <= _y2; i++) {
				DrawPixel(_x, i, _color);
			}
		} else {
			double deltaErr = std::abs((double)yDiff / (double)xDiff);
			double error = deltaErr - 0.5;
			int y = _y;
			for(int x = _x; x <= _x2; x++) {
				DrawPixel(x, y, _color);
				error += deltaErr;
				if(error >= 0.5) {
					y++;
					error -= 1.0;
				}
			}
		}
	}

public:
	DrawLineCommand(int x, int y, int x2, int y2, int color, int frameCount) :
		DrawCommand(frameCount), _x(x), _y(y), _x2(x2), _y2(y2), _color(color)
	{
		if(!(_color & 0xFF000000)) {
			_color |= 0xFF000000;
		}
	}
};
