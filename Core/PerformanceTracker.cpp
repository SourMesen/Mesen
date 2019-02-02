/*
This is code is heavily based on the Lua scripts written by upsilandre 
The original scripts can be found on his blog here: http://upsilandre.over-blog.com/2018/07/compte-rendu-framerate-nes.html
*/

#include "stdafx.h"
#include "PerformanceTracker.h"
#include "Console.h"
#include "PPU.h"
#include "DebugHud.h"
#include "IKeyManager.h"
#include "KeyManager.h"

enum Colors
{
	Red = 0x00FF0000,
	Yellow = 0x00FFD800,
	OpaqueWhite = 0x00FFFFFF,
	OpaqueBlack = 0x00000000,
	Black = 0x40000000,
	White = 0x40FFFFFF,
	Blue = 0xA00000FF,
	Transparent = 0xFF000000,
};

PerformanceTracker::PerformanceTracker(shared_ptr<Console> console)
{
	_console = console;
	_data = PerfTrackerData();
}

void PerformanceTracker::Initialize(int32_t address, AddressType type, PerfTrackerMode mode)
{
	_address = address;
	_type = type;
	_mode = mode;
	_needReset = true;
}

PerfTrackerMode PerformanceTracker::GetMode()
{
	return _mode;
}

void PerformanceTracker::ProcessMouseInput()
{
	bool leftButtonPressed = KeyManager::IsMouseButtonPressed(MouseButton::LeftButton);
	bool rightButtonPressed = KeyManager::IsMouseButtonPressed(MouseButton::RightButton);
	if(_leftButtonPressed && leftButtonPressed != _leftButtonPressed) {
		//Left button was clicked
		_mode = (PerfTrackerMode)((_mode + 1) % 4);
		if(_mode == PerfTrackerMode::Disabled) {
			_mode = PerfTrackerMode::Fullscreen;
		}
	}
	if(_rightButtonPressed && rightButtonPressed != _rightButtonPressed) {
		//Right button was clicked
		_updateSpeed = _updateSpeed == PerfTrackerSpeed::Fast ? PerfTrackerSpeed::Normal : PerfTrackerSpeed::Fast;
	}

	_leftButtonPressed = leftButtonPressed;
	_rightButtonPressed = rightButtonPressed;
}

void PerformanceTracker::ProcessEndOfFrame()
{
	if(_mode == PerfTrackerMode::Disabled) {
		return;
	}

	ProcessMouseInput();

	_data.frameCount++;
	_data.frameProcessed = false;

	shared_ptr<DebugHud> hud = _console->GetDebugHud();
	int frame = _console->GetPpu()->GetFrameCount();

	if(_mode == PerfTrackerMode::TextOnly) {
		hud->DrawRectangle(2, 2, 33, 22, Colors::Black, true, 1, frame);
		hud->DrawRectangle(2, 2, 33, 22, Colors::OpaqueWhite, false, 1, frame);
		hud->DrawString(4, 4, std::to_string(_data.fps) + "FPS", Colors::OpaqueWhite, Colors::Transparent, 1, frame);
		hud->DrawString(4, 14, std::to_string(_data.updateCpu) + "%", Colors::OpaqueWhite, Colors::Transparent, 1, frame);
	} else if(_mode == PerfTrackerMode::Fullscreen || _mode == PerfTrackerMode::Compact) {
		hud->DrawRectangle(0, 223, 256, 10, Colors::Black, true, 1, frame);
		hud->DrawLine(0, 222, 255, 222, Colors::White, 1, frame);
		hud->DrawLine(0, 233, 255, 233, Colors::White, 1, frame);
		hud->DrawLine(40, 223, 40, 232, Colors::White, 1, frame);
		hud->DrawLine(100, 223, 100, 232, Colors::White, 1, frame);
		hud->DrawLine(170, 223, 170, 232, Colors::White, 1, frame);
		hud->DrawString(6, 224, std::to_string(_data.fps) + "FPS", Colors::OpaqueWhite, Colors::Transparent, 1, frame);

		string cpuUsage = std::to_string(_data.updateCpu);
		cpuUsage = std::string(3 - cpuUsage.size(), ' ') + cpuUsage;
		hud->DrawString(46, 224, "CPU: " + cpuUsage + "%", Colors::OpaqueWhite, Colors::Transparent, 1, frame);

		string avgCpuUsage = std::to_string(_data.totalCpu / _data.cpuEntry);
		avgCpuUsage = std::string(3 - avgCpuUsage.size(), ' ') + avgCpuUsage;
		hud->DrawString(105, 224, "Avg FPS: " + std::to_string((_data.gameFrame * 60) / _data.totalFrame), Colors::OpaqueWhite, Colors::Transparent, 1, frame);
		hud->DrawString(176, 224, "Avg CPU: " + avgCpuUsage + "%", Colors::OpaqueWhite, Colors::Transparent, 1, frame);		

		int scale = _mode == PerfTrackerMode::Fullscreen ? 200 : 60;

		hud->DrawRectangle(0, 2, 256, scale + 1, (uint32_t)Colors::Blue, true, 1, frame);
		hud->DrawLine(0, 2, 255, 2, Colors::White, 1, frame);
		hud->DrawLine(0, (scale / 4) + 2, 255, (scale / 4) + 2, Colors::White, 1, frame);
		hud->DrawLine(0, (scale / 2) + 2, 255, (scale / 2) + 2, Colors::White, 1, frame);
		hud->DrawLine(0, (scale * 3 / 4) + 2, 255, (scale * 3 / 4) + 2, Colors::White, 1, frame);
		hud->DrawLine(0, scale + 2, 255, scale + 2, Colors::White, 1, frame);
		hud->DrawString(1, 4, "200%/60FPS", Colors::White, Colors::Transparent, 1, frame);
		hud->DrawString(0, scale / 2 + 4, "100%/30FPS", Colors::White, Colors::Transparent, 1, frame);

		DrawChart(_data.cpuChartDataPoints, _data.cpuChartPos, Colors::Red, scale, 200);
		DrawChart(_data.fpsChartDataPoints, _data.fpsChartPos, Colors::Yellow, scale, 60);
	}
}

void PerformanceTracker::DrawChart(int *dataPoints, int startPos, int color, int scale, int maxValue)
{
	shared_ptr<DebugHud> hud = _console->GetDebugHud();
	int frame = _console->GetPpu()->GetFrameCount();

	int dotX = 0;
	int i = startPos;
	do {
		int dotY1 = dataPoints[i] * scale / maxValue;
		int dotY2 = dataPoints[(i + 1) % 256] * scale / maxValue;
		hud->DrawLine(dotX, scale + 2 - dotY1, dotX + 1, scale + 2 - dotY2, color, 1, frame);
		dotX++;
		i = (i + 1) % 256;
	} while(i != startPos);
}

void PerformanceTracker::ProcessCpuExec(AddressTypeInfo & addressInfo)
{
	if(_mode == PerfTrackerMode::Disabled || addressInfo.Address != _address || addressInfo.Type != _type) {
		//Performance tracker isn't running, or address doesn't match, ignore it
		return;
	}

	if(_needReset) {
		//Options have been changed, reset everything
		_data = PerfTrackerData();
		_needReset = false;
	}

	if(_data.frameProcessed) {
		//We already hit this instruction once during this frame, don't count it again
		return;
	}

	_data.frameProcessed = true;

	//Count the number of frames since the last time we processed this instruction
	int lag = _data.frameCount - _data.prevFrameCount - 1;
	_data.prevFrameCount = _data.frameCount;

	if(lag >= 0 && lag < 3 && _data.frameCount > 3) {
		//Check what percentage of the current frame has been used (since the last NMI)
		int scanline = _console->GetPpu()->GetCurrentScanline();
		PPUDebugState state;
		_console->GetPpu()->GetState(state);
		int scanlineCount = state.ScanlineCount;
		_data.partialCpu = ((scanline + 20) % scanlineCount) * 100 / scanlineCount;
		if(scanline >= 241) {
			_data.partialCpu = 0;
		}

		//Store the current frame's CPU usage as a data point for the chart (each lag frame counts as +100% CPU)
		int finalCpu = _data.partialCpu + (lag * 100);

		//Update the average CPU statistic
		_data.totalCpu += finalCpu;
		_data.cpuEntry++;

		//Process the last X frames since NMI, and check which ones are lag frames
		for(int i = 0; i <= lag; i++) {
			_data.totalFrame++;
			_data.isLagFramePos = (_data.isLagFramePos + 1) % 60;
			_data.isLagFrame[_data.isLagFramePos] = true;
		}
		_data.isLagFrame[_data.isLagFramePos] = false;

		_data.gameFrame++;

		_data.updateTimer++;
		if(_updateSpeed == PerfTrackerSpeed::Fast || _data.updateTimer == 8) {
			_data.updateTimer = 0;
			_data.cpuChartDataPoints[_data.cpuChartPos] = finalCpu;
			_data.cpuChartPos = (_data.cpuChartPos + 1) % 256;

			//Update the onscreen display for the CPU %
			_data.updateCpu = finalCpu;

			//Calculate the average FPS for the last 60 frames (and add it as a data point for the chart)
			_data.fps = 0;
			int i = _data.isLagFramePos;
			do {
				_data.fps += _data.isLagFrame[i] ? 0 : 1;
				i = (i + 1) % 60;
			} while(i != _data.isLagFramePos);

			_data.fpsChartDataPoints[_data.fpsChartPos] = _data.fps;
			_data.fpsChartPos = (_data.fpsChartPos + 1) % 256;
		}
	}
}
