#include "stdafx.h"
#include "AviRecorder.h"
#include "MessageManager.h"

AviRecorder::AviRecorder()
{
	_recording = false;
	_stopFlag = false;
	_frameBuffer = nullptr;
	_frameBufferLength = 0;
	_sampleRate = 0;
}

AviRecorder::~AviRecorder()
{
	if(_recording) {
		StopRecording();
	}

	if(_frameBuffer) {
		delete[] _frameBuffer;
		_frameBuffer = nullptr;
	}
}

bool AviRecorder::StartRecording(string filename, VideoCodec codec, uint32_t width, uint32_t height, uint32_t bpp, uint32_t fps, uint32_t audioSampleRate, uint32_t compressionLevel)
{
	if(!_recording) {
		_outputFile = filename;
		_sampleRate = audioSampleRate;
		_frameBufferLength = height * width * bpp;
		_frameBuffer = new uint8_t[_frameBufferLength];

		_aviWriter.reset(new AviWriter());
		if(!_aviWriter->StartWrite(filename, codec, width, height, bpp, fps, audioSampleRate, compressionLevel)) {
			_aviWriter.reset();
			return false;
		}

		_aviWriterThread = std::thread([=]() {
			while(!_stopFlag) {
				_waitFrame.Wait();
				if(_stopFlag) {
					break;
				}

				auto lock = _lock.AcquireSafe();
				_aviWriter->AddFrame(_frameBuffer);
			}
		});

		MessageManager::DisplayMessage("VideoRecorder", "VideoRecorderStarted", _outputFile);
		_recording = true;
	}
	return true;
}

void AviRecorder::StopRecording()
{
	if(_recording) {
		_recording = false;

		_stopFlag = true;
		_waitFrame.Signal();
		_aviWriterThread.join();

		_aviWriter->EndWrite();
		_aviWriter.reset();

		MessageManager::DisplayMessage("VideoRecorder", "VideoRecorderStopped", _outputFile);
	}
}

void AviRecorder::AddFrame(void* frameBuffer)
{
	if(_recording) {
		auto lock = _lock.AcquireSafe();
		memcpy(_frameBuffer, frameBuffer, _frameBufferLength);
		_waitFrame.Signal();
	}
}

void AviRecorder::AddSound(int16_t* soundBuffer, uint32_t sampleCount, uint32_t sampleRate)
{
	if(_recording) {
		if(_sampleRate != sampleRate) {
			auto lock = _lock.AcquireSafe();
			StopRecording();
		} else {
			_aviWriter->AddSound(soundBuffer, sampleCount);
		}
	}
}

bool AviRecorder::IsRecording()
{
	return _recording;
}