#pragma once
#include "stdafx.h"
#include "VideoDecoder.h"
#include "EmulationSettings.h"
#include "MessageManager.h"
#include "../Utilities/PNGWriter.h"
#include "../Utilities/FolderUtilities.h"

const uint32_t PPU_PALETTE_ARGB[] = {
	0xFF666666, 0xFF002A88, 0xFF1412A7, 0xFF3B00A4, 0xFF5C007E,
	0xFF6E0040, 0xFF6C0600, 0xFF561D00, 0xFF333500, 0xFF0B4800,
	0xFF005200, 0xFF004F08, 0xFF00404D, 0xFF000000, 0xFF000000,
	0xFF000000, 0xFFADADAD, 0xFF155FD9, 0xFF4240FF, 0xFF7527FE,
	0xFFA01ACC, 0xFFB71E7B, 0xFFB53120, 0xFF994E00, 0xFF6B6D00,
	0xFF388700, 0xFF0C9300, 0xFF008F32, 0xFF007C8D, 0xFF000000,
	0xFF000000, 0xFF000000, 0xFFFFFEFF, 0xFF64B0FF, 0xFF9290FF,
	0xFFC676FF, 0xFFF36AFF, 0xFFFE6ECC, 0xFFFE8170, 0xFFEA9E22,
	0xFFBCBE00, 0xFF88D800, 0xFF5CE430, 0xFF45E082, 0xFF48CDDE,
	0xFF4F4F4F, 0xFF000000, 0xFF000000, 0xFFFFFEFF, 0xFFC0DFFF,
	0xFFD3D2FF, 0xFFE8C8FF, 0xFFFBC2FF, 0xFFFEC4EA, 0xFFFECCC5,
	0xFFF7D8A5, 0xFFE4E594, 0xFFCFEF96, 0xFFBDF4AB, 0xFFB3F3CC,
	0xFFB5EBF2, 0xFFB8B8B8, 0xFF000000, 0xFF000000,
};

unique_ptr<VideoDecoder> VideoDecoder::Instance;

VideoDecoder* VideoDecoder::GetInstance()
{
	if(!Instance) {
		Instance.reset(new VideoDecoder());
	}
	return Instance.get();
}

VideoDecoder::~VideoDecoder()
{
	if(_frameBuffer) {
		delete[] _frameBuffer;
	}
}

uint32_t VideoDecoder::ProcessIntensifyBits(uint16_t ppuPixel)
{
	uint32_t pixelOutput = PPU_PALETTE_ARGB[ppuPixel & 0x3F];

	//Incorrect emphasis bit implementation, but will do for now.
	float redChannel = (float)((pixelOutput & 0xFF0000) >> 16);
	float greenChannel = (float)((pixelOutput & 0xFF00) >> 8);
	float blueChannel = (float)(pixelOutput & 0xFF);

	if(ppuPixel & 0x40) {
		//Intensify red
		redChannel *= 1.1f;
		greenChannel *= 0.9f;
		blueChannel *= 0.9f;
	}
	if(ppuPixel & 0x80) {
		//Intensify green
		greenChannel *= 1.1f;
		redChannel *= 0.9f;
		blueChannel *= 0.9f;
	}
	if(ppuPixel & 0x100) {
		//Intensify blue
		blueChannel *= 1.1f;
		redChannel *= 0.9f;
		greenChannel *= 0.9f;
	}

	uint8_t r, g, b;
	r = (uint8_t)fmin(redChannel, 255);
	g = (uint8_t)fmin(greenChannel, 255);
	b = (uint8_t)fmin(blueChannel, 255);

	return 0xFF000000 | (r << 16) | (g << 8) | b;
}

void VideoDecoder::UpdateBufferSize()
{
	OverscanDimensions overscan = EmulationSettings::GetOverscanDimensions();

	if(!_frameBuffer || _overscan.GetPixelCount() != overscan.GetPixelCount()) {
		_overscan = overscan;
		if(_frameBuffer) {
			delete[] _frameBuffer;
		}
		_frameBuffer = new uint32_t[_overscan.GetPixelCount()];
	}
}

uint32_t* VideoDecoder::DecodeFrame(uint16_t* inputBuffer)
{
	_frameLock.Acquire();
	UpdateBufferSize();
	uint32_t* outputBuffer = _frameBuffer;
	for(uint32_t i = _overscan.Top, iMax = 240 - _overscan.Bottom; i < iMax; i++) {
		for(uint32_t j = _overscan.Left, jMax = 256 - _overscan.Right; j < jMax; j++) {
			*outputBuffer = ProcessIntensifyBits(inputBuffer[i*256+j]);
			outputBuffer++;
		}
	}
	_frameLock.Release();

	return _frameBuffer;
}

void VideoDecoder::TakeScreenshot(string romFilename)
{
	uint32_t bufferSize = _overscan.GetPixelCount() * 4;
	uint32_t* frameBuffer = new uint32_t[bufferSize];

	_frameLock.Acquire();
	memcpy(frameBuffer, _frameBuffer, bufferSize);
	_frameLock.Release();

	//ARGB -> ABGR
	for(uint32_t i = 0; i < bufferSize; i++) {
		frameBuffer[i] = (frameBuffer[i] & 0xFF00FF00) | ((frameBuffer[i] & 0xFF0000) >> 16) | ((frameBuffer[i] & 0xFF) << 16);
	}

	int counter = 0;
	string baseFilename = FolderUtilities::GetScreenshotFolder() + romFilename;
	string ssFilename;
	while(true) {
		string counterStr = std::to_string(counter);
		while(counterStr.length() < 3) {
			counterStr = "0" + counterStr;
		}
		ssFilename = baseFilename + "_" + counterStr + ".png";
		ifstream file(ssFilename, ios::in);
		if(file) {
			file.close();
		} else {
			break;
		}
		counter++;
	}

	PNGWriter::WritePNG(ssFilename, (uint8_t*)frameBuffer, _overscan.GetScreenWidth(), _overscan.GetScreenHeight());
	delete[] frameBuffer;

	MessageManager::DisplayMessage("Screenshot saved", FolderUtilities::GetFilename(ssFilename, true));
}