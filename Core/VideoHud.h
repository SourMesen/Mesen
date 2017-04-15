#pragma once
#include "stdafx.h"
#include "EmulationSettings.h"
#include "FrameInfo.h"

class VideoHud
{
private:
	static const vector<uint32_t> _gamePads[4];
	static const vector<uint32_t> _playIcon;
	static const vector<uint32_t> _recordIcon;

	void BlendColors(uint32_t* output, uint32_t input);
	bool DisplayControllerInput(int inputPort, uint8_t *outputBuffer, FrameInfo &frameInfo, OverscanDimensions &overscan, uint32_t displayIndex);
	void DrawMovieIcons(uint8_t *outputBuffer, FrameInfo &frameInfo, OverscanDimensions &overscan);

public:
	void DrawHud(uint8_t *outputBuffer, FrameInfo frameInfo, OverscanDimensions overscan);
};
