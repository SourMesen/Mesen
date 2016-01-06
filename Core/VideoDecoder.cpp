#include "stdafx.h"
#include "IRenderingDevice.h"
#include "VideoDecoder.h"
#include "EmulationSettings.h"
#include "DefaultVideoFilter.h"
#include "NtscFilter.h"
#include "HdVideoFilter.h"

const uint32_t PPU_PALETTE_ARGB[64] = {
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
		outputBuffer[i] = PPU_PALETTE_ARGB[inputBuffer[i] & 0x3F];
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
				break;
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

bool VideoDecoder::UpdateFrame(void *ppuOutputBuffer, HdPpuPixelInfo *hdPixelInfo)
{
	bool readyForNewFrame = _frameChanged.load() == false ? true : false;

	if(readyForNewFrame) {
		//The PPU sends us a new frame via this function when a full frame is done drawing
		_hdScreenTiles = hdPixelInfo;
		_ppuOutputBuffer = (uint16_t*)ppuOutputBuffer;
		_frameChanged = true;
		_waitForFrame.Signal();
	}
	_frameCount++;

	return readyForNewFrame;
}

void VideoDecoder::StartThread()
{
	if(!Instance->_decodeThread) {	
		_stopFlag = false;
		_frameChanged = false;
		_frameCount = 0;
		Instance->_decodeThread.reset(new thread(&VideoDecoder::DecodeThread, Instance.get()));
		Instance->_renderThread.reset(new thread(&VideoDecoder::RenderThread, Instance.get()));
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