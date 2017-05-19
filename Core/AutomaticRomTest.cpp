#include "stdafx.h"
#include "../Utilities/FolderUtilities.h"
#include "AutomaticRomTest.h"
#include "EmulationSettings.h"
#include "Console.h"
#include "PPU.h"
#include "VideoDecoder.h"
#include "StandardController.h"

bool AutomaticRomTest::_running = false;

AutomaticRomTest::AutomaticRomTest()
{
	_running = true;
	_errorCode = 0;
	MessageManager::RegisterNotificationListener(this);
}

AutomaticRomTest::~AutomaticRomTest()
{
	_running = false;
	MessageManager::UnregisterNotificationListener(this);
}

void AutomaticRomTest::ProcessNotification(ConsoleNotificationType type, void* parameter)
{
	if(type == ConsoleNotificationType::PpuFrameDone) {
		uint16_t *frameBuffer = (uint16_t*)parameter;
		if(PPU::GetFrameCount() == 5) {
			memcpy(_prevFrameBuffer, frameBuffer, sizeof(_prevFrameBuffer));
		} else if(PPU::GetFrameCount() == 300) {
			if(memcmp(_prevFrameBuffer, frameBuffer, sizeof(_prevFrameBuffer)) == 0) {
				//No change
				_errorCode |= 0x20;
			}
			memcpy(_prevFrameBuffer, frameBuffer, sizeof(_prevFrameBuffer));
			VideoDecoder::GetInstance()->TakeScreenshot();
		} else if(PPU::GetFrameCount() == 900) {
			if(memcmp(_prevFrameBuffer, frameBuffer, sizeof(_prevFrameBuffer)) == 0) {
				//No change
				_errorCode |= 0x01;
			}

			bool allZeros = true;
			for(int i = 0; i < 256 * 240; i++) {
				if(frameBuffer[i] != 0) {
					allZeros = false;
					break;
				}
			}

			if(allZeros) {
				_errorCode |= 0x04;
			}

			memcpy(_prevFrameBuffer, frameBuffer, sizeof(_prevFrameBuffer));
			VideoDecoder::GetInstance()->TakeScreenshot();
		} else if(PPU::GetFrameCount() == 1800) {
			bool continueTest = false;
			if(memcmp(_prevFrameBuffer, frameBuffer, sizeof(_prevFrameBuffer)) == 0) {
				//No change, change input pattern and keep trying
				continueTest = true;
			}

			bool allZeros = true;
			for(int i = 0; i < 256 * 240; i++) {
				if(frameBuffer[i] != 0) {
					allZeros = false;
					break;
				}
			}

			if(allZeros) {
				_errorCode |= 0x08;
			}

			VideoDecoder::GetInstance()->TakeScreenshot();

			if(!continueTest) {
				//Stop test
				_signal.Signal();
			}
		} else if(PPU::GetFrameCount() == 3600) {
			if(memcmp(_prevFrameBuffer, frameBuffer, sizeof(_prevFrameBuffer)) == 0) {
				//No change
				_errorCode |= 0x02;
			}

			bool allZeros = true;
			for(int i = 0; i < 256 * 240; i++) {
				if(frameBuffer[i] != 0) {
					allZeros = false;
					break;
				}
			}

			if(allZeros) {
				_errorCode |= 0x40;
			}

			VideoDecoder::GetInstance()->TakeScreenshot();

			//Stop test
			_signal.Signal();
		}
	}
}

int32_t AutomaticRomTest::Run(string filename)
{
	EmulationSettings::SetFlags(EmulationFlags::ForceMaxSpeed);
	EmulationSettings::SetMasterVolume(0);
	Console::Pause();
	if(Console::LoadROM(filename)) {
		Console::Resume();
		EmulationSettings::ClearFlags(EmulationFlags::Paused);
		_signal.Wait();

		EmulationSettings::SetFlags(EmulationFlags::Paused);

		if(PPU::GetFrameCount() < 1800) {
			//Finished early
			_errorCode |= 0x10;
		}

		EmulationSettings::ClearFlags(EmulationFlags::ForceMaxSpeed);
		EmulationSettings::SetMasterVolume(1.0);

		Console::GetInstance()->Stop();

		return _errorCode;
	}

	return -1;
}

bool AutomaticRomTest::Running()
{
	return _running;
}

uint8_t AutomaticRomTest::GetControllerState(uint8_t port)
{
	if(port == 0) {
		uint32_t frameNumber = PPU::GetFrameCount();

		if(frameNumber <= 1800) {
			if(frameNumber % 30 < 10) {
				//Press 1 button for 10 frames every second
				if((frameNumber / 30) % 8 != 1) {
					return 1 << ((frameNumber / 60) % 8);
				}
			}
		} else {
			if(frameNumber % 30 < 10) {
				if((frameNumber / 30) % 2) {
					return 0x01;
				} else {
					return 0x08;
				}
			}
		}
		return 0;
	} else {
		return 0;
	}
}
