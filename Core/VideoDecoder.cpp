#include "stdafx.h"
#include "IRenderingDevice.h"
#include "VideoDecoder.h"
#include "EmulationSettings.h"
#include "DefaultVideoFilter.h"
#include "RawVideoFilter.h"
#include "BisqwitNtscFilter.h"
#include "NtscFilter.h"
#include "HdVideoFilter.h"
#include "ScaleFilter.h"
#include "VideoRenderer.h"
#include "RewindManager.h"
#include "Console.h"
#include "PPU.h"
#include "HdData.h"
#include "HdNesPack.h"
#include "RotateFilter.h"
#include "DebugHud.h"
#include "NotificationManager.h"

VideoDecoder::VideoDecoder(shared_ptr<Console> console)
{
	_console = console;
	_frameChanged = false;
	_stopFlag = false;
	UpdateVideoFilter();
}

VideoDecoder::~VideoDecoder()
{
	StopThread();
}

FrameInfo VideoDecoder::GetFrameInfo()
{
	return _lastFrameInfo;
}

void VideoDecoder::GetScreenSize(ScreenSize &size, bool ignoreScale)
{
	if(_videoFilter) {
		OverscanDimensions overscan = ignoreScale ? _videoFilter->GetOverscan() : EmulationSettings::GetOverscanDimensions();
		FrameInfo frameInfo{ overscan.GetScreenWidth(), overscan.GetScreenHeight(), PPU::ScreenWidth, PPU::ScreenHeight, 4 };
		double aspectRatio = EmulationSettings::GetAspectRatio(_console);
		double scale = (ignoreScale ? 1 : EmulationSettings::GetVideoScale());
		size.Width = (int32_t)(frameInfo.Width * scale);
		size.Height = (int32_t)(frameInfo.Height * scale);
		if(aspectRatio != 0.0) {
			size.Width = (uint32_t)(frameInfo.OriginalHeight * scale * aspectRatio * ((double)frameInfo.Width / frameInfo.OriginalWidth));
		}

		if(EmulationSettings::GetScreenRotation() % 180) {
			std::swap(size.Width, size.Height);
		}

		size.Scale = scale;
	}
}

void VideoDecoder::UpdateVideoFilter()
{
	VideoFilterType newFilter = EmulationSettings::GetVideoFilterType();

	if(_videoFilterType != newFilter || _videoFilter == nullptr || (_hdScreenInfo && !_hdFilterEnabled) || (!_hdScreenInfo && _hdFilterEnabled)) {
		_videoFilterType = newFilter;
		_videoFilter.reset(new DefaultVideoFilter());
		_scaleFilter.reset();

		switch(_videoFilterType) {
			case VideoFilterType::None: break;
			case VideoFilterType::NTSC: _videoFilter.reset(new NtscFilter()); break;
			case VideoFilterType::BisqwitNtsc: _videoFilter.reset(new BisqwitNtscFilter(1)); break;
			case VideoFilterType::BisqwitNtscHalfRes: _videoFilter.reset(new BisqwitNtscFilter(2)); break;
			case VideoFilterType::BisqwitNtscQuarterRes: _videoFilter.reset(new BisqwitNtscFilter(4)); break;
			case VideoFilterType::Raw: _videoFilter.reset(new RawVideoFilter()); break;
			default: _scaleFilter = ScaleFilter::GetScaleFilter(_videoFilterType); break;
		}

		_hdFilterEnabled = false;
		if(_hdScreenInfo) {
			_videoFilter.reset(new HdVideoFilter(_console->GetHdData()));
			_hdFilterEnabled = true;
		}
	}

	if(EmulationSettings::GetScreenRotation() == 0 && _rotateFilter) {
		_rotateFilter.reset();
	} else if(EmulationSettings::GetScreenRotation() > 0) {
		if(!_rotateFilter || _rotateFilter->GetAngle() != EmulationSettings::GetScreenRotation()) {
			_rotateFilter.reset(new RotateFilter(EmulationSettings::GetScreenRotation()));
		}
	}
}

void VideoDecoder::DecodeFrame(bool synchronous)
{
	UpdateVideoFilter();

	if(_hdFilterEnabled) {
		((HdVideoFilter*)_videoFilter.get())->SetHdScreenTiles(_hdScreenInfo);
	}
	_videoFilter->SendFrame(_ppuOutputBuffer, _frameNumber);

	uint32_t* outputBuffer = _videoFilter->GetOutputBuffer();
	FrameInfo frameInfo = _videoFilter->GetFrameInfo();
	_console->GetDebugHud()->Draw(outputBuffer, _videoFilter->GetOverscan(), frameInfo.Width, _frameNumber);

	if(_rotateFilter) {
		outputBuffer = _rotateFilter->ApplyFilter(outputBuffer, frameInfo.Width, frameInfo.Height);
		frameInfo = _rotateFilter->GetFrameInfo(frameInfo);
	}

	if(_scaleFilter) {
		outputBuffer = _scaleFilter->ApplyFilter(outputBuffer, frameInfo.Width, frameInfo.Height);
		frameInfo = _scaleFilter->GetFrameInfo(frameInfo);
	}

	if(_hud) {
		_hud->DrawHud(_console, outputBuffer, frameInfo, _videoFilter->GetOverscan());
	}

	ScreenSize screenSize;
	GetScreenSize(screenSize, true);
	if(_previousScale != EmulationSettings::GetVideoScale() || screenSize.Height != _previousScreenSize.Height || screenSize.Width != _previousScreenSize.Width) {
		_console->GetNotificationManager()->SendNotification(ConsoleNotificationType::ResolutionChanged);
	}
	_previousScale = EmulationSettings::GetVideoScale();
	_previousScreenSize = screenSize;
	
	_lastFrameInfo = frameInfo;

	_frameChanged = false;
	
	//Rewind manager will take care of sending the correct frame to the video renderer
	_console->GetRewindManager()->SendFrame(outputBuffer, frameInfo.Width, frameInfo.Height, synchronous);
}

void VideoDecoder::DecodeThread()
{
	//This thread will decode the PPU's output (color ID to RGB, intensify r/g/b and produce a HD version of the frame if needed)
	while(!_stopFlag.load()) {
		//DecodeFrame returns the final ARGB frame we want to display in the emulator window
		while(!_frameChanged) {
			_waitForFrame.Wait();
			if(_stopFlag.load()) {
				return;
			}
		}

		DecodeFrame();
	}
}

uint32_t VideoDecoder::GetFrameCount()
{
	return _frameCount;
}

void VideoDecoder::UpdateFrameSync(void *ppuOutputBuffer, HdScreenInfo *hdScreenInfo)
{
	_frameNumber = _console->GetFrameCount();
	_hdScreenInfo = hdScreenInfo;
	_ppuOutputBuffer = (uint16_t*)ppuOutputBuffer;
	DecodeFrame(true);
	_frameCount++;
}

void VideoDecoder::UpdateFrame(void *ppuOutputBuffer, HdScreenInfo *hdScreenInfo)
{
	if(_frameChanged) {
		//Last frame isn't done decoding yet - sometimes Signal() introduces a 25-30ms delay
		while(_frameChanged) {
			//Spin until decode is done
		}
		//At this point, we are sure that the decode thread is no longer busy
	}
	
	_frameNumber = _console->GetFrameCount();
	_hdScreenInfo = hdScreenInfo;
	_ppuOutputBuffer = (uint16_t*)ppuOutputBuffer;
	_frameChanged = true;
	_waitForFrame.Signal();

	_frameCount++;
}

void VideoDecoder::StartThread()
{
#ifndef LIBRETRO
	if(!_decodeThread) {	
		_stopFlag = false;
		_frameChanged = false;
		_frameCount = 0;
		_waitForFrame.Reset();
		_hud.reset(new VideoHud());
		_decodeThread.reset(new thread(&VideoDecoder::DecodeThread, this));
	}
#endif
}

void VideoDecoder::StopThread()
{
#ifndef LIBRETRO
	_stopFlag = true;
	if(_decodeThread) {
		_waitForFrame.Signal();
		_decodeThread->join();

		_decodeThread.reset();

		_hud.reset();
		_hdScreenInfo = nullptr;
		EmulationSettings::SetPpuModel(PpuModel::Ppu2C02);
		UpdateVideoFilter();
		if(_ppuOutputBuffer != nullptr) {
			//Clear whole screen
			for(uint32_t i = 0; i < PPU::PixelCount; i++) {
				_ppuOutputBuffer[i] = 14; //Black
			}
			DecodeFrame();
		}
		_ppuOutputBuffer = nullptr;
	}
#endif
}

bool VideoDecoder::IsRunning()
{
	return _decodeThread != nullptr;
}

void VideoDecoder::TakeScreenshot()
{
	if(_videoFilter) {
		_videoFilter->TakeScreenshot(_console, _console->GetRomPath(), _videoFilterType);
	}
}

void VideoDecoder::TakeScreenshot(std::stringstream &stream)
{
	if(_videoFilter) {
		_videoFilter->TakeScreenshot(_console, _videoFilterType, "", &stream);
	}
}
