#include "stdafx.h"
#include "IRenderingDevice.h"
#include "VideoDecoder.h"
#include "EmulationSettings.h"
#include "DefaultVideoFilter.h"
#include "BisqwitNtscFilter.h"
#include "NtscFilter.h"
#include "HdVideoFilter.h"
#include "ScaleFilter.h"
#include "VideoRenderer.h"
#include "RewindManager.h"
#include "PPU.h"
#include "HdNesPack.h"

unique_ptr<VideoDecoder> VideoDecoder::Instance;

VideoDecoder* VideoDecoder::GetInstance()
{
	if(!Instance) {
		Instance.reset(new VideoDecoder());
	}
	return Instance.get();
}

void VideoDecoder::Release()
{
	if(Instance) {
		Instance.reset();
	}
}

VideoDecoder::VideoDecoder()
{
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
	return _videoFilter->GetFrameInfo();
}

void VideoDecoder::GetScreenSize(ScreenSize &size, bool ignoreScale)
{
	if(_videoFilter) {
		OverscanDimensions overscan = EmulationSettings::GetOverscanDimensions();
		FrameInfo frameInfo{ overscan.GetScreenWidth(), overscan.GetScreenHeight(), PPU::ScreenWidth, PPU::ScreenHeight, 4 };
		double aspectRatio = EmulationSettings::GetAspectRatio();
		double scale = (ignoreScale ? 1 : EmulationSettings::GetVideoScale());
		size.Width = (int32_t)(frameInfo.Width * scale);
		size.Height = (int32_t)(frameInfo.Height * scale);
		if(aspectRatio != 0.0) {
			size.Width = (uint32_t)(frameInfo.OriginalHeight * scale * aspectRatio * ((double)frameInfo.Width / frameInfo.OriginalWidth));
		}
		size.Scale = scale;
	}
}

void VideoDecoder::UpdateVideoFilter()
{
	VideoFilterType newFilter = EmulationSettings::GetVideoFilterType();
	if(_hdScreenTiles) {
		newFilter = VideoFilterType::HdPack;
	}

	if(_videoFilterType != newFilter || _videoFilter == nullptr) {
		_videoFilterType = newFilter;

		switch(_videoFilterType) {
			case VideoFilterType::None: _videoFilter.reset(new DefaultVideoFilter()); break;
			case VideoFilterType::NTSC: _videoFilter.reset(new NtscFilter()); break;
			case VideoFilterType::BisqwitNtsc: _videoFilter.reset(new BisqwitNtscFilter(1)); break;
			case VideoFilterType::BisqwitNtscHalfRes: _videoFilter.reset(new BisqwitNtscFilter(2)); break;
			case VideoFilterType::BisqwitNtscQuarterRes: _videoFilter.reset(new BisqwitNtscFilter(4)); break;
			case VideoFilterType::xBRZ2x: _videoFilter.reset(new ScaleFilter(ScaleFilterType::xBRZ, 2)); break;
			case VideoFilterType::xBRZ3x: _videoFilter.reset(new ScaleFilter(ScaleFilterType::xBRZ, 3)); break;
			case VideoFilterType::xBRZ4x: _videoFilter.reset(new ScaleFilter(ScaleFilterType::xBRZ, 4)); break;
			case VideoFilterType::xBRZ5x: _videoFilter.reset(new ScaleFilter(ScaleFilterType::xBRZ, 5)); break;
			case VideoFilterType::xBRZ6x: _videoFilter.reset(new ScaleFilter(ScaleFilterType::xBRZ, 6)); break;
			case VideoFilterType::HQ2x: _videoFilter.reset(new ScaleFilter(ScaleFilterType::HQX, 2)); break;
			case VideoFilterType::HQ3x: _videoFilter.reset(new ScaleFilter(ScaleFilterType::HQX, 3)); break;
			case VideoFilterType::HQ4x: _videoFilter.reset(new ScaleFilter(ScaleFilterType::HQX, 4)); break;
			case VideoFilterType::Scale2x: _videoFilter.reset(new ScaleFilter(ScaleFilterType::Scale2x, 2)); break;
			case VideoFilterType::Scale3x: _videoFilter.reset(new ScaleFilter(ScaleFilterType::Scale2x, 3)); break;
			case VideoFilterType::Scale4x: _videoFilter.reset(new ScaleFilter(ScaleFilterType::Scale2x, 4)); break;
			case VideoFilterType::_2xSai: _videoFilter.reset(new ScaleFilter(ScaleFilterType::_2xSai, 2)); break;
			case VideoFilterType::Super2xSai: _videoFilter.reset(new ScaleFilter(ScaleFilterType::Super2xSai, 2)); break;
			case VideoFilterType::SuperEagle: _videoFilter.reset(new ScaleFilter(ScaleFilterType::SuperEagle, 2)); break;

			case VideoFilterType::Prescale2x: _videoFilter.reset(new ScaleFilter(ScaleFilterType::Prescale, 2)); break;
			case VideoFilterType::Prescale3x: _videoFilter.reset(new ScaleFilter(ScaleFilterType::Prescale, 3)); break;
			case VideoFilterType::Prescale4x: _videoFilter.reset(new ScaleFilter(ScaleFilterType::Prescale, 4)); break;
			case VideoFilterType::Prescale6x: _videoFilter.reset(new ScaleFilter(ScaleFilterType::Prescale, 6)); break;
			case VideoFilterType::Prescale8x: _videoFilter.reset(new ScaleFilter(ScaleFilterType::Prescale, 8)); break;
			case VideoFilterType::Prescale10x: _videoFilter.reset(new ScaleFilter(ScaleFilterType::Prescale, 10)); break;

			case VideoFilterType::HdPack: _videoFilter.reset(new HdVideoFilter()); break;
		}
	}
}

void VideoDecoder::DecodeFrame()
{
	UpdateVideoFilter();

	if(_videoFilterType == VideoFilterType::HdPack) {
		((HdVideoFilter*)_videoFilter.get())->SetHdScreenTiles(_hdScreenTiles);
	}
	_videoFilter->SendFrame(_ppuOutputBuffer);

	ScreenSize screenSize;
	GetScreenSize(screenSize, true);
	if(_previousScale != EmulationSettings::GetVideoScale() || screenSize.Height != _previousScreenSize.Height || screenSize.Width != _previousScreenSize.Width) {
		MessageManager::SendNotification(ConsoleNotificationType::ResolutionChanged);
	}
	_previousScale = EmulationSettings::GetVideoScale();
	_previousScreenSize = screenSize;
	
	FrameInfo frameInfo = _videoFilter->GetFrameInfo();

	_frameChanged = false;
	
	//Rewind manager will take care of sending the correct frame to the video renderer
	RewindManager::SendFrame(_videoFilter->GetOutputBuffer(), frameInfo.Width, frameInfo.Height);
}

void VideoDecoder::DebugDecodeFrame(uint16_t* inputBuffer, uint32_t* outputBuffer, uint32_t length)
{
	for(uint32_t i = 0; i < length; i++) {
		outputBuffer[i] = EmulationSettings::GetRgbPalette()[inputBuffer[i] & 0x3F];
	}
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

void VideoDecoder::UpdateFrameSync(void *ppuOutputBuffer, HdPpuPixelInfo *hdPixelInfo)
{
	_hdScreenTiles = hdPixelInfo;
	_ppuOutputBuffer = (uint16_t*)ppuOutputBuffer;
	DecodeFrame();
	_frameCount++;
}

void VideoDecoder::UpdateFrame(void *ppuOutputBuffer, HdPpuPixelInfo *hdPixelInfo)
{
	if(_frameChanged) {
		//Last frame isn't done decoding yet - sometimes Signal() introduces a 25-30ms delay
		while(_frameChanged) {
			//Spin until decode is done
		}
		//At this point, we are sure that the decode thread is no longer busy
	}

	_hdScreenTiles = hdPixelInfo;
	_ppuOutputBuffer = (uint16_t*)ppuOutputBuffer;
	_frameChanged = true;
	_waitForFrame.Signal();

	_frameCount++;
}

void VideoDecoder::StartThread()
{
	if(!_decodeThread) {	
		_stopFlag = false;
		_frameChanged = false;
		_frameCount = 0;
		_waitForFrame.Reset();

		_decodeThread.reset(new thread(&VideoDecoder::DecodeThread, this));
	}
}

void VideoDecoder::StopThread()
{
	_stopFlag = true;
	if(_decodeThread) {
		_waitForFrame.Signal();
		_decodeThread->join();

		_decodeThread.reset();

		_hdScreenTiles = nullptr;
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
}

bool VideoDecoder::IsRunning()
{
	return _decodeThread != nullptr;
}

void VideoDecoder::TakeScreenshot()
{
	if(_videoFilter) {
		_videoFilter->TakeScreenshot();
	}
}

void VideoDecoder::TakeScreenshot(std::stringstream &stream)
{
	if(_videoFilter) {
		_videoFilter->TakeScreenshot("", &stream);
	}
}
