#include "stdafx.h"
#include "VideoHud.h"
#include "ControlManager.h"
#include "BaseControlDevice.h"
#include "ControlDeviceState.h"
#include "StandardController.h"
#include "FourScore.h"
#include "Zapper.h"
#include "MovieManager.h"
#include "Console.h"

void VideoHud::DrawHud(shared_ptr<Console> console, uint32_t *outputBuffer, FrameInfo frameInfo, OverscanDimensions overscan)
{
	uint32_t displayCount = 0;
	InputDisplaySettings settings = EmulationSettings::GetInputDisplaySettings();
	
	vector<ControlDeviceState> states = console->GetControlManager()->GetPortStates();
	for(int inputPort = 0; inputPort < 4; inputPort++) {
		if((settings.VisiblePorts >> inputPort) & 0x01) {
			if(DisplayControllerInput(states[inputPort], inputPort, outputBuffer, frameInfo, overscan, displayCount)) {
				displayCount++;
			}
		}
	}

	DrawMovieIcons(outputBuffer, frameInfo, overscan);
}

bool VideoHud::DisplayControllerInput(ControlDeviceState &state, int inputPort, uint32_t *outputBuffer, FrameInfo &frameInfo, OverscanDimensions &overscan, uint32_t displayIndex)
{
	bool axisInverted = (EmulationSettings::GetScreenRotation() % 180) != 0;
	int scale = frameInfo.Width / (axisInverted ? overscan.GetScreenHeight() : overscan.GetScreenWidth());
	uint32_t* rgbaBuffer = outputBuffer;

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

	int32_t buttonState = -1;

	shared_ptr<BaseControlDevice> device = ControlManager::CreateControllerDevice(EmulationSettings::GetControllerType(inputPort), 0, nullptr);
	if(!device) {
		return false;
	}

	device->SetRawState(state);

	shared_ptr<StandardController> controller = std::dynamic_pointer_cast<StandardController>(device);
	if(controller) {
		buttonState = controller->ToByte();
	}

	if(buttonState >= 0) {
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

	shared_ptr<Zapper> zapper = std::dynamic_pointer_cast<Zapper>(device);
	if(zapper) {
		MousePosition pos = zapper->GetCoordinates();
		if(pos.X != -1 && pos.Y != -1) {
			for(int i = -1; i <= 1; i++) {
				int y = (pos.Y - overscan.Top) * scale + i;
				if(y < 0 || y >(int)frameInfo.Height) continue;

				for(int j = -1; j <= 1; j++) {
					int x = (pos.X - overscan.Left) * scale + j;
					if(x < 0 || x > (int)frameInfo.Width) continue;

					uint32_t bufferPos = y*frameInfo.Width + x;
					BlendColors(rgbaBuffer + bufferPos, 0xFFFF0000);
				}
			}
		}		
	}

	return false;
}

void VideoHud::DrawMovieIcons(uint32_t *outputBuffer, FrameInfo &frameInfo, OverscanDimensions &overscan)
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

