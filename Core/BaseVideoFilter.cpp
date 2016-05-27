#include "stdafx.h"
#include "BaseVideoFilter.h"
#include "MessageManager.h"
#include "../Utilities/PNGHelper.h"
#include "../Utilities/FolderUtilities.h"

BaseVideoFilter::BaseVideoFilter()
{
	_overscan = EmulationSettings::GetOverscanDimensions();
}

BaseVideoFilter::~BaseVideoFilter()
{
	if(_outputBuffer) {
		delete[] _outputBuffer;
	}
}

void BaseVideoFilter::UpdateBufferSize()
{
	uint32_t newBufferSize = GetFrameInfo().Width*GetFrameInfo().Height*GetFrameInfo().BitsPerPixel;
	if(_bufferSize != newBufferSize) {
		if(_outputBuffer) {
			delete[] _outputBuffer;
		}

		_bufferSize = newBufferSize;
		_outputBuffer = new uint8_t[newBufferSize];

		MessageManager::SendNotification(ConsoleNotificationType::ResolutionChanged);
	}
}

OverscanDimensions BaseVideoFilter::GetOverscan()
{
	return _overscan;
}

void BaseVideoFilter::OnBeforeApplyFilter()
{
}

void BaseVideoFilter::SendFrame(uint16_t *ppuOutputBuffer)
{
	_frameLock.Acquire();
	_overscan = EmulationSettings::GetOverscanDimensions();
	UpdateBufferSize();
	OnBeforeApplyFilter();
	ApplyFilter(ppuOutputBuffer);
	_frameLock.Release();
}

uint8_t* BaseVideoFilter::GetOutputBuffer()
{
	return _outputBuffer;
}

void BaseVideoFilter::TakeScreenshot(string romFilename)
{
	uint32_t* frameBuffer = (uint32_t*)new uint8_t[_bufferSize];

	_frameLock.Acquire();
	memcpy(frameBuffer, GetOutputBuffer(), _bufferSize);
	_frameLock.Release();

	//ARGB -> ABGR
	for(uint32_t i = 0; i < _bufferSize/GetFrameInfo().BitsPerPixel; i++) {
		frameBuffer[i] = 0xFF000000 | (frameBuffer[i] & 0xFF00) | ((frameBuffer[i] & 0xFF0000) >> 16) | ((frameBuffer[i] & 0xFF) << 16);
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

	PNGHelper::WritePNG(ssFilename, (uint8_t*)frameBuffer, GetFrameInfo().Width, GetFrameInfo().Height);
	delete[] frameBuffer;

	MessageManager::DisplayMessage("ScreenshotSaved", FolderUtilities::GetFilename(ssFilename, true));
}

