#include "stdafx.h"
#include "BaseVideoFilter.h"
#include "MessageManager.h"
#include "../Utilities/PNGHelper.h"
#include "../Utilities/FolderUtilities.h"
#include "StandardController.h"
#include "ScaleFilter.h"
#include "RotateFilter.h"
#include "Console.h"

BaseVideoFilter::BaseVideoFilter(shared_ptr<Console> console)
{
	_console = console;
	_overscan = _console->GetSettings()->GetOverscanDimensions();
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
	uint32_t newBufferSize = GetFrameInfo().Width*GetFrameInfo().Height;
	if(_bufferSize != newBufferSize) {
		_frameLock.Acquire();
		if(_outputBuffer) {
			delete[] _outputBuffer;
		}

		_bufferSize = newBufferSize;
		_outputBuffer = new uint32_t[newBufferSize];
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

bool BaseVideoFilter::IsOddFrame()
{
	return _isOddFrame;
}

void BaseVideoFilter::SendFrame(uint16_t *ppuOutputBuffer, uint32_t frameNumber)
{
	_frameLock.Acquire();
	_overscan = _console->GetSettings()->GetOverscanDimensions();
	_isOddFrame = frameNumber % 2;
	UpdateBufferSize();
	OnBeforeApplyFilter();
	ApplyFilter(ppuOutputBuffer);

	_frameLock.Release();
}

uint32_t* BaseVideoFilter::GetOutputBuffer()
{
	return _outputBuffer;
}

void BaseVideoFilter::TakeScreenshot(VideoFilterType filterType, string filename, std::stringstream *stream, bool rawScreenshot)
{
	uint32_t* pngBuffer;
	FrameInfo frameInfo;
	uint32_t* frameBuffer = nullptr;
	{
		auto lock = _frameLock.AcquireSafe();
		if(_bufferSize == 0 || !GetOutputBuffer()) {
			return;
		}

		frameBuffer = new uint32_t[_bufferSize];
		memcpy(frameBuffer, GetOutputBuffer(), _bufferSize * sizeof(frameBuffer[0]));
		frameInfo = GetFrameInfo();
	}

	pngBuffer = frameBuffer;

	if(!rawScreenshot) {
		uint32_t rotationAngle = _console->GetSettings()->GetScreenRotation();
		shared_ptr<RotateFilter> rotateFilter;
		if(rotationAngle > 0) {
			rotateFilter.reset(new RotateFilter(rotationAngle));
			pngBuffer = rotateFilter->ApplyFilter(pngBuffer, frameInfo.Width, frameInfo.Height);
			frameInfo = rotateFilter->GetFrameInfo(frameInfo);
		}

		shared_ptr<ScaleFilter> scaleFilter = ScaleFilter::GetScaleFilter(filterType);
		if(scaleFilter) {
			pngBuffer = scaleFilter->ApplyFilter(pngBuffer, frameInfo.Width, frameInfo.Height, _console->GetSettings()->GetPictureSettings().ScanlineIntensity);
			frameInfo = scaleFilter->GetFrameInfo(frameInfo);
		}

		VideoHud hud;
		hud.DrawHud(_console, pngBuffer, frameInfo, _console->GetSettings()->GetOverscanDimensions());
	}

	if(!filename.empty()) {
		PNGHelper::WritePNG(filename, pngBuffer, frameInfo.Width, frameInfo.Height);
	} else {
		PNGHelper::WritePNG(*stream, pngBuffer, frameInfo.Width, frameInfo.Height);
	}

	delete[] frameBuffer;
}

void BaseVideoFilter::TakeScreenshot(string romName, VideoFilterType filterType)
{
	string romFilename = FolderUtilities::GetFilename(romName, false);

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

