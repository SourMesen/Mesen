#include "stdafx.h"
#include "DebugHud.h"
#include "DrawCommand.h"
#include "DrawLineCommand.h"
#include "DrawPixelCommand.h"
#include "DrawRectangleCommand.h"
#include "DrawStringCommand.h"

DebugHud* DebugHud::_instance = nullptr;

DebugHud::DebugHud()
{
	_instance = this;
}

DebugHud::~DebugHud()
{
	_commandLock.Acquire();
	if(_instance == this) {
		_instance = nullptr;
	}
	_commandLock.Release();
}

DebugHud* DebugHud::GetInstance()
{
	return _instance;
}

void DebugHud::ClearScreen()
{
	auto lock = _commandLock.AcquireSafe();
	_commands.clear();
}

void DebugHud::Draw(uint32_t* argbBuffer, OverscanDimensions &overscan)
{
	auto lock = _commandLock.AcquireSafe();
	for(shared_ptr<DrawCommand> &command : _commands) {
		command->Draw(argbBuffer, overscan);
	}
	_commands.erase(std::remove_if(_commands.begin(), _commands.end(), [](const shared_ptr<DrawCommand>& c) { return c->Expired(); }), _commands.end());
}

void DebugHud::DrawPixel(int x, int y, int color, int frameCount)
{
	auto lock = _commandLock.AcquireSafe();
	_commands.push_back(shared_ptr<DrawPixelCommand>(new DrawPixelCommand(x, y, color, frameCount)));
}

void DebugHud::DrawLine(int x, int y, int x2, int y2, int color, int frameCount)
{
	auto lock = _commandLock.AcquireSafe();
	_commands.push_back(shared_ptr<DrawLineCommand>(new DrawLineCommand(x, y, x2, y2, color, frameCount)));
}

void DebugHud::DrawRectangle(int x, int y, int width, int height, int color, bool fill, int frameCount)
{
	auto lock = _commandLock.AcquireSafe();
	_commands.push_back(shared_ptr<DrawRectangleCommand>(new DrawRectangleCommand(x, y, width, height, color, fill, frameCount)));
}

void DebugHud::DrawString(int x, int y, string text, int color, int backColor, int frameCount)
{
	auto lock = _commandLock.AcquireSafe();
	_commands.push_back(shared_ptr<DrawStringCommand>(new DrawStringCommand(x, y, text, color, backColor, frameCount)));
}
