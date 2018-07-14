#pragma once
#include "stdafx.h"
#include "EmulationSettings.h"
#include "FrameInfo.h"

struct ControlDeviceState;
class Console;

class VideoHud
{
private:
	static const vector<uint32_t> _gamePads[4];
	static const vector<uint32_t> _playIcon;
	static const vector<uint32_t> _recordIcon;

	void BlendColors(uint32_t* output, uint32_t input);
	bool DisplayControllerInput(shared_ptr<Console> console, ControlDeviceState &state, int inputPort, uint32_t *outputBuffer, FrameInfo &frameInfo, OverscanDimensions &overscan, uint32_t displayIndex);
	void DrawMovieIcons(shared_ptr<Console> console, uint32_t *outputBuffer, FrameInfo &frameInfo, OverscanDimensions &overscan);

public:
	void DrawHud(shared_ptr<Console> console, uint32_t *outputBuffer, FrameInfo frameInfo, OverscanDimensions overscan);
};
