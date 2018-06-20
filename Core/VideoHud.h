#pragma once
#include "stdafx.h"
#include "EmulationSettings.h"
#include "FrameInfo.h"

struct ControlDeviceState;

class VideoHud
{
private:
	static const vector<uint32_t> _gamePads[4];
	static const vector<uint32_t> _playIcon;
	static const vector<uint32_t> _recordIcon;

	void BlendColors(uint32_t* output, uint32_t input);
	bool DisplayControllerInput(ControlDeviceState &state, int inputPort, uint32_t *outputBuffer, FrameInfo &frameInfo, OverscanDimensions &overscan, uint32_t displayIndex);
	void DrawMovieIcons(uint32_t *outputBuffer, FrameInfo &frameInfo, OverscanDimensions &overscan);

public:
	void DrawHud(uint32_t *outputBuffer, FrameInfo frameInfo, OverscanDimensions overscan);
};
