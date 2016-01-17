#include "stdafx.h"
#include "IRenderingDevice.h"
#include "VideoDecoder.h"
#include "EmulationSettings.h"
#include "DefaultVideoFilter.h"
#include "NtscFilter.h"
#include "HdVideoFilter.h"

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

void VideoDecoder::GetScreenSize(ScreenSize &size)
{
	if(_videoFilter) {
		size.Width = _videoFilter->GetFrameInfo().Width * EmulationSettings::GetVideoScale();
		size.Height = _videoFilter->GetFrameInfo().Height * EmulationSettings::GetVideoScale();
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

	if(_renderer) {
		_renderer->UpdateFrame(_videoFilter->GetOutputBuffer(), _videoFilter->GetFrameInfo().Width, _videoFilter->GetFrameInfo().Height);
	}
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
		_waitForRender.Signal();
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
		_waitForRender.Reset();

		_decodeThread.reset(new thread(&VideoDecoder::DecodeThread, this));
		_renderThread.reset(new thread(&VideoDecoder::RenderThread, this));
	}
}

void VideoDecoder::StopThread()
{
	_stopFlag = true;
	if(_decodeThread) {
		_waitForFrame.Signal();
		_decodeThread->join();
	}
	if(_renderThread) {
		_waitForRender.Signal();
		_renderThread->join();
	}

	_decodeThread.release();
	_renderThread.release();

	if(_renderer && _ppuOutputBuffer != nullptr) {
		memset(_ppuOutputBuffer, 0, PPU::PixelCount * sizeof(uint16_t));
		DecodeFrame();
		_renderer->Render();
	}
}

void VideoDecoder::RenderThread()
{
	if(_renderer) {
		_renderer->Reset();
	}
	while(!_stopFlag.load()) {
		//Wait until a frame is ready, or until 16ms have passed (to allow UI to run at a minimum of 60fps)
		_waitForRender.Wait(16);
		if(_renderer) {
			_renderer->Render();
		}
	}
}

void VideoDecoder::RegisterRenderingDevice(IRenderingDevice *renderer)
{
	_renderer = renderer;
}

void VideoDecoder::UnregisterRenderingDevice(IRenderingDevice *renderer)
{
	if(_renderer == renderer) {
		_renderer = nullptr;
	}
}

void VideoDecoder::TakeScreenshot(string romFilename)
{
	if(_videoFilter) {
		_videoFilter->TakeScreenshot(romFilename);
	}
}