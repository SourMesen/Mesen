#include "stdafx.h"
#include "VideoHud.h"
#include "ControlManager.h"
#include "StandardController.h"
#include "MovieManager.h"

void VideoHud::DrawHud(uint8_t *outputBuffer, FrameInfo frameInfo, OverscanDimensions overscan)
{
	uint32_t displayCount = 0;
	InputDisplaySettings settings = EmulationSettings::GetInputDisplaySettings();
	for(int inputPort = 0; inputPort < 4; inputPort++) {
		if((settings.VisiblePorts >> inputPort) & 0x01) {
			if(DisplayControllerInput(inputPort, outputBuffer, frameInfo, overscan, displayCount)) {
				displayCount++;
			}
		}
	}

	DrawMovieIcons(outputBuffer, frameInfo, overscan);
}

bool VideoHud::DisplayControllerInput(int inputPort, uint8_t *outputBuffer, FrameInfo &frameInfo, OverscanDimensions &overscan, uint32_t displayIndex)
{
	int scale = frameInfo.Width / overscan.GetScreenWidth();
	uint32_t* rgbaBuffer = (uint32_t*)outputBuffer;

	InputDisplaySettings settings = EmulationSettings::GetInputDisplaySettings();
	uint32_t yStart, xStart; 
	switch(settings.DisplayPosition) {
		case InputDisplayPosition::TopLeft: 
			xStart = 3 * scale + (settings.DisplayHorizontally ? displayIndex * 40 * scale : 0);
			yStart = 5 * scale + (settings.DisplayHorizontally ? 0 : displayIndex * 14 * scale);
			break;
		case InputDisplayPosition::TopRight: 
			xStart = frameInfo.Width - 40 * scale - (settings.DisplayHorizontally ? displayIndex * 40 * scale : 0);
			yStart = 5 * scale + (settings.DisplayHorizontally ? 0 : displayIndex * 14 * scale);
			break;
		case InputDisplayPosition::BottomLeft: 
			xStart = 3 * scale + (settings.DisplayHorizontally ? displayIndex * 40 * scale : 0);
			yStart = frameInfo.Height - 15 * scale - (settings.DisplayHorizontally ? 0 : displayIndex * 14 * scale);
			break;
		default:
		case InputDisplayPosition::BottomRight: 
			xStart = frameInfo.Width - 40 * scale - (settings.DisplayHorizontally ? displayIndex * 40 * scale : 0);
			yStart = frameInfo.Height - 15 * scale - (settings.DisplayHorizontally ? 0 : displayIndex * 14 * scale);
			break;
	}

	int port = inputPort > 1 ? inputPort - 2 : inputPort;
	shared_ptr<StandardController> device = std::dynamic_pointer_cast<StandardController>(ControlManager::GetControlDevice(port));
	if(inputPort > 1 && device) {
		device = std::dynamic_pointer_cast<StandardController>(device->GetAdditionalController());
	}

	if(device) {
		uint8_t buttonState = device->GetLastButtonState();
		for(int y = 0; y < 13 * scale; y++) {
			for(int x = 0; x < 38 * scale; x++) {
				uint32_t bufferPos = (yStart + y)*frameInfo.Width + (xStart + x);
				uint32_t gridValue = _gamePads[inputPort][y / scale * 38 + x / scale];
				if(gridValue > 0) {
					if((buttonState >> (gridValue - 1)) & 0x01) {
						BlendColors(rgbaBuffer + bufferPos, 0xEFFFFFFF);
					} else {
						BlendColors(rgbaBuffer + bufferPos, 0xEF111111);
					}
				} else {
					BlendColors(rgbaBuffer + bufferPos, 0xBFAAAAAA);
				}
			}
		}
		return true;
	}
	return false;
}

void VideoHud::DrawMovieIcons(uint8_t *outputBuffer, FrameInfo &frameInfo, OverscanDimensions &overscan)
{
	if(EmulationSettings::CheckFlag(EmulationFlags::DisplayMovieIcons) && (MovieManager::Playing() || MovieManager::Recording())) {
		InputDisplaySettings settings = EmulationSettings::GetInputDisplaySettings();
		uint32_t xOffset = settings.VisiblePorts > 0 && settings.DisplayPosition == InputDisplayPosition::TopRight ? 50 : 27;
		uint32_t* rgbaBuffer = (uint32_t*)outputBuffer;
		int scale = frameInfo.Width / overscan.GetScreenWidth();
		uint32_t yStart = 15 * scale;
		uint32_t xStart = (frameInfo.Width - xOffset) * scale;
		if(MovieManager::Playing()) {
			for(int y = 0; y < 12 * scale; y++) {
				for(int x = 0; x < 12 * scale; x++) {
					uint32_t bufferPos = (yStart + y)*frameInfo.Width + (xStart + x);
					uint32_t gridValue = _playIcon[y / scale * 12 + x / scale];
					if(gridValue == 1) {
						BlendColors(rgbaBuffer + bufferPos, 0xEF00CF00);
					} else if(gridValue == 2) {
						BlendColors(rgbaBuffer + bufferPos, 0xEF009F00);
					} else if(gridValue == 3) {
						BlendColors(rgbaBuffer + bufferPos, 0xEF000000);
					}
				}
			}
		} else if(MovieManager::Recording()) {
			for(int y = 0; y < 12 * scale; y++) {
				for(int x = 0; x < 12 * scale; x++) {
					uint32_t bufferPos = (yStart + y)*frameInfo.Width + (xStart + x);
					uint32_t gridValue = _recordIcon[y / scale * 12 + x / scale];
					if(gridValue == 1) {
						BlendColors(rgbaBuffer + bufferPos, 0xEFCF0000);
					} else if(gridValue == 2) {
						BlendColors(rgbaBuffer + bufferPos, 0xEF9F0000);
					} else if(gridValue == 3) {
						BlendColors(rgbaBuffer + bufferPos, 0xEF000000);
					}
				}
			}
		}
	}
}

void VideoHud::BlendColors(uint32_t* output, uint32_t input)
{
	uint8_t inA = (input >> 24) & 0xFF;
	uint8_t inR = (input >> 16) & 0xFF;
	uint8_t inG = (input >> 8) & 0xFF;
	uint8_t inB = input & 0xFF;

	uint8_t invertedAlpha = 255 - inA;
	uint8_t outB = (uint8_t)((inA * inB + invertedAlpha * (*output & 0xFF)) >> 8);
	uint8_t outG = (uint8_t)((inA * inG + invertedAlpha * ((*output >> 8) & 0xFF)) >> 8);
	uint8_t outR = (uint8_t)((inA * inR + invertedAlpha * ((*output >> 16) & 0xFF)) >> 8);
	*output = 0xFF000000 | (outR << 16) | (outG << 8) | outB;
}

const vector<uint32_t> VideoHud::_playIcon = {
	3,3,3,0,0,0,0,0,0,0,0,0,
	3,1,1,3,3,0,0,0,0,0,0,0,
	3,1,2,1,1,3,3,0,0,0,0,0,
	3,1,2,2,2,1,1,3,3,0,0,0,
	3,1,2,2,2,2,2,1,1,3,0,0,
	3,1,2,2,2,2,2,2,2,1,3,0,
	3,1,2,2,2,2,2,2,2,1,3,0,
	3,1,2,2,2,2,2,1,1,3,0,0,
	3,1,2,2,2,1,1,3,3,0,0,0,
	3,1,2,1,1,3,3,0,0,0,0,0,
	3,1,1,3,3,0,0,0,0,0,0,0,
	3,3,3,0,0,0,0,0,0,0,0,0,
};

const vector<uint32_t> VideoHud::_recordIcon = {
	0,0,0,0,0,0,0,0,0,0,0,0,
	0,0,0,0,0,3,3,0,0,0,0,0,
	0,0,0,3,3,1,1,3,3,0,0,0,
	0,0,3,1,1,2,2,1,1,3,0,0,
	0,0,3,1,2,2,2,2,1,3,0,0,
	0,3,1,2,2,2,2,2,2,1,3,0,
	0,3,1,2,2,2,2,2,2,1,3,0,
	0,0,3,1,2,2,2,2,1,3,0,0,
	0,0,3,1,1,2,2,1,1,3,0,0,
	0,0,0,3,3,1,1,3,3,0,0,0,
	0,0,0,0,0,3,3,0,0,0,0,0,
	0,0,0,0,0,0,0,0,0,0,0,0,
};

const vector<uint32_t> VideoHud::_gamePads[4] = {
 { 9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,
	9,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,9,
	9,0,0,0,0,5,5,5,0,0,0,0,0,0,0,0,0,0,9,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,9,
	9,0,0,0,0,5,5,5,0,0,0,0,0,0,0,0,0,9,9,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,9,
	9,0,0,0,0,5,5,5,0,0,0,0,0,0,0,0,0,0,9,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,9,
	9,0,7,7,7,9,9,9,8,8,8,0,0,0,0,0,0,0,9,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,9,
	9,0,7,7,7,9,9,9,8,8,8,0,0,0,0,0,0,9,9,9,0,0,0,0,0,0,0,2,2,0,0,0,0,1,1,0,0,9,
	9,0,7,7,7,9,9,9,8,8,8,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,2,2,0,0,1,1,1,1,0,9,
	9,0,0,0,0,6,6,6,0,0,0,0,0,0,3,3,3,3,0,4,4,4,4,0,0,0,2,2,2,2,0,0,1,1,1,1,0,9,
	9,0,0,0,0,6,6,6,0,0,0,0,0,0,3,3,3,3,0,4,4,4,4,0,0,0,0,2,2,0,0,0,0,1,1,0,0,9,
	9,0,0,0,0,6,6,6,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,9,
	9,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,9,
	9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9 },

 { 9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,
	9,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,9,
	9,0,0,0,0,5,5,5,0,0,0,0,0,0,0,0,0,9,9,9,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,9,
	9,0,0,0,0,5,5,5,0,0,0,0,0,0,0,0,0,0,0,9,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,9,
	9,0,0,0,0,5,5,5,0,0,0,0,0,0,0,0,0,0,9,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,9,
	9,0,7,7,7,9,9,9,8,8,8,0,0,0,0,0,0,9,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,9,
	9,0,7,7,7,9,9,9,8,8,8,0,0,0,0,0,0,9,9,9,0,0,0,0,0,0,0,2,2,0,0,0,0,1,1,0,0,9,
	9,0,7,7,7,9,9,9,8,8,8,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,2,2,0,0,1,1,1,1,0,9,
	9,0,0,0,0,6,6,6,0,0,0,0,0,0,3,3,3,3,0,4,4,4,4,0,0,0,2,2,2,2,0,0,1,1,1,1,0,9,
	9,0,0,0,0,6,6,6,0,0,0,0,0,0,3,3,3,3,0,4,4,4,4,0,0,0,0,2,2,0,0,0,0,1,1,0,0,9,
	9,0,0,0,0,6,6,6,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,9,
	9,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,9,
	9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9 },

 { 9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,
	9,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,9,
	9,0,0,0,0,5,5,5,0,0,0,0,0,0,0,0,0,9,9,9,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,9,
	9,0,0,0,0,5,5,5,0,0,0,0,0,0,0,0,0,0,0,9,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,9,
	9,0,0,0,0,5,5,5,0,0,0,0,0,0,0,0,0,0,9,9,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,9,
	9,0,7,7,7,9,9,9,8,8,8,0,0,0,0,0,0,0,0,9,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,9,
	9,0,7,7,7,9,9,9,8,8,8,0,0,0,0,0,0,9,9,9,0,0,0,0,0,0,0,2,2,0,0,0,0,1,1,0,0,9,
	9,0,7,7,7,9,9,9,8,8,8,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,2,2,0,0,1,1,1,1,0,9,
	9,0,0,0,0,6,6,6,0,0,0,0,0,0,3,3,3,3,0,4,4,4,4,0,0,0,2,2,2,2,0,0,1,1,1,1,0,9,
	9,0,0,0,0,6,6,6,0,0,0,0,0,0,3,3,3,3,0,4,4,4,4,0,0,0,0,2,2,0,0,0,0,1,1,0,0,9,
	9,0,0,0,0,6,6,6,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,9,
	9,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,9,
	9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9 },

 { 9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,
	9,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,9,
	9,0,0,0,0,5,5,5,0,0,0,0,0,0,0,0,0,9,0,9,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,9,
	9,0,0,0,0,5,5,5,0,0,0,0,0,0,0,0,0,9,0,9,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,9,
	9,0,0,0,0,5,5,5,0,0,0,0,0,0,0,0,0,9,9,9,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,9,
	9,0,7,7,7,9,9,9,8,8,8,0,0,0,0,0,0,0,0,9,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,9,
	9,0,7,7,7,9,9,9,8,8,8,0,0,0,0,0,0,0,0,9,0,0,0,0,0,0,0,2,2,0,0,0,0,1,1,0,0,9,
	9,0,7,7,7,9,9,9,8,8,8,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,2,2,0,0,1,1,1,1,0,9,
	9,0,0,0,0,6,6,6,0,0,0,0,0,0,3,3,3,3,0,4,4,4,4,0,0,0,2,2,2,2,0,0,1,1,1,1,0,9,
	9,0,0,0,0,6,6,6,0,0,0,0,0,0,3,3,3,3,0,4,4,4,4,0,0,0,0,2,2,0,0,0,0,1,1,0,0,9,
	9,0,0,0,0,6,6,6,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,9,
	9,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,9,
	9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9 }
};

