#include "stdafx.h"
#include "BaseVideoFilter.h"
#include "MessageManager.h"
#include "../Utilities/PNGHelper.h"
#include "../Utilities/FolderUtilities.h"
#include "Console.h"
#include "StandardController.h"
#include "ScaleFilter.h"

BaseVideoFilter::BaseVideoFilter()
{
	_overscan = EmulationSettings::GetOverscanDimensions();
}

BaseVideoFilter::~BaseVideoFilter()
{
	auto lock = _frameLock.AcquireSafe();
	if(_outputBuffer) {
		delete[] _outputBuffer;
		_outputBuffer = nullptr;
	}
}

void BaseVideoFilter::UpdateBufferSize()
{
	uint32_t newBufferSize = GetFrameInfo().Width*GetFrameInfo().Height*GetFrameInfo().BitsPerPixel;
	if(_bufferSize != newBufferSize) {
		_frameLock.Acquire();
		if(_outputBuffer) {
			delete[] _outputBuffer;
		}

		_bufferSize = newBufferSize;
		_outputBuffer = new uint8_t[newBufferSize];
		_frameLock.Release();
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
	_videoHud.DrawHud(GetOutputBuffer(), GetFrameInfo(), GetOverscan());
	_frameLock.Release();
}

uint8_t* BaseVideoFilter::GetOutputBuffer()
{
	return _outputBuffer;
}

void BaseVideoFilter::TakeScreenshot(VideoFilterType filterType, string filename, std::stringstream *stream)
{
	uint32_t* pngBuffer;
	FrameInfo frameInfo;
	uint32_t* frameBuffer = nullptr;
	{
		auto lock = _frameLock.AcquireSafe();
		if(_bufferSize == 0 || !GetOutputBuffer()) {
			return;
		}

		frameBuffer = (uint32_t*)new uint8_t[_bufferSize];
		memcpy(frameBuffer, GetOutputBuffer(), _bufferSize);
		frameInfo = GetFrameInfo();
	}

	shared_ptr<ScaleFilter> scaleFilter = ScaleFilter::GetScaleFilter(filterType);
	if(scaleFilter) {
		pngBuffer = scaleFilter->ApplyFilter(frameBuffer, frameInfo.Width, frameInfo.Height);
		frameInfo = scaleFilter->GetFrameInfo(frameInfo);
	} else {
		pngBuffer = frameBuffer;
	}

	if(!filename.empty()) {
		PNGHelper::WritePNG(filename, pngBuffer, frameInfo.Width, frameInfo.Height);
	} else {
		PNGHelper::WritePNG(*stream, pngBuffer, frameInfo.Width, frameInfo.Height);
	}

	delete[] frameBuffer;
}

void BaseVideoFilter::TakeScreenshot(VideoFilterType filterType)
{
	string romFilename = FolderUtilities::GetFilename(Console::GetRomName(), false);

	int counter = 0;
	string baseFilename = FolderUtilities::CombinePath(FolderUtilities::GetScreenshotFolder(), romFilename);
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

	TakeScreenshot(filterType, ssFilename);

	MessageManager::DisplayMessage("ScreenshotSaved", FolderUtilities::GetFilename(ssFilename, true));
}

