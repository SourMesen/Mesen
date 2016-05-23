#include "stdafx.h"
#include "IRenderingDevice.h"
#include "VideoDecoder.h"
#include "EmulationSettings.h"
#include "DefaultVideoFilter.h"
#include "NtscFilter.h"
#include "HdVideoFilter.h"
#include "ScaleFilter.h"
#include "VideoRenderer.h"

unique_ptr<VideoDecoder> VideoDecoder::Instance;

VideoDecoder* VideoDecoder::GetInstance()
{
	if(!Instance) {
		Instance.reset(new VideoDecoder());
	}
	return Instance.get();
}

VideoDecoder::VideoDecoder()
{
	UpdateVideoFilter();
}

VideoDecoder::~VideoDecoder()
{
	StopThread();
}

void VideoDecoder::GetScreenSize(ScreenSize &size, bool ignoreScale)
{
	if(_videoFilter) {
		double aspectRatio = EmulationSettings::GetAspectRatio();
		size.Width = (int32_t)(_videoFilter->GetFrameInfo().Width * (ignoreScale ? 1 : EmulationSettings::GetVideoScale()));
		size.Height = (int32_t)(_videoFilter->GetFrameInfo().Height * (ignoreScale ? 1 : EmulationSettings::GetVideoScale()));
		if(aspectRatio != 0.0) {
			size.Width = (uint32_t)(size.Height * aspectRatio);
		}
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

	_frameChanged = false;

	VideoRenderer::GetInstance()->UpdateFrame(_videoFilter->GetOutputBuffer(), _videoFilter->GetFrameInfo().Width, _videoFilter->GetFrameInfo().Height);
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
		if(!_frameChanged) {
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

		_decodeThread.release();

		if(_ppuOutputBuffer != nullptr) {
			//Clear whole screen
			for(int i = 0; i < PPU::PixelCount; i++) {
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

void VideoDecoder::TakeScreenshot(string romFilename)
{
	if(_videoFilter) {
		_videoFilter->TakeScreenshot(romFilename);
	}
}