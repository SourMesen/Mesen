#pragma once
#include "stdafx.h"
#include "../Utilities/SimpleLock.h"
#include "EmulationSettings.h"
class DrawCommand;

class DebugHud
{
private:
	static DebugHud* _instance;
	vector<shared_ptr<DrawCommand>> _commands;
	SimpleLock _commandLock;

public:
	static DebugHud* GetInstance();
	DebugHud();
	~DebugHud();

	void Draw(uint32_t* argbBuffer, OverscanDimensions &overscan);
	void ClearScreen();

	void DrawPixel(int x, int y, int color, int frameCount);
	void DrawLine(int x, int y, int x2, int y2, int color, int frameCount);
	void DrawRectangle(int x, int y, int width, int height, int color, bool fill, int frameCount);
	void DrawScreenBuffer(uint32_t* screenBuffer);
	void DrawString(int x, int y, string text, int color, int backColor, int frameCount);
};
