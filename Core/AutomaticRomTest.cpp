#include "stdafx.h"
#include "../Utilities/FolderUtilities.h"
#include "AutomaticRomTest.h"
#include "EmulationSettings.h"
#include "Console.h"
#include "PPU.h"
#include "VideoDecoder.h"
#include "StandardController.h"
#include "NotificationManager.h"

AutomaticRomTest::AutomaticRomTest()
{
	_errorCode = 0;
}

AutomaticRomTest::~AutomaticRomTest()
{
}

void AutomaticRomTest::ProcessNotification(ConsoleNotificationType type, void* parameter)
{
	if(type == ConsoleNotificationType::PpuFrameDone) {
		uint16_t *frameBuffer = (uint16_t*)parameter;
		if(_console->GetFrameCount() == 5) {
			memcpy(_prevFrameBuffer, frameBuffer, sizeof(_prevFrameBuffer));
		} else if(_console->GetFrameCount() == 300) {
			if(memcmp(_prevFrameBuffer, frameBuffer, sizeof(_prevFrameBuffer)) == 0) {
				//No change
				_errorCode |= 0x20;
			}
			memcpy(_prevFrameBuffer, frameBuffer, sizeof(_prevFrameBuffer));
			_console->GetVideoDecoder()->TakeScreenshot();
		} else if(_console->GetFrameCount() == 900) {
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
			_console->GetVideoDecoder()->TakeScreenshot();
		} else if(_console->GetFrameCount() == 1800) {
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

			_console->GetVideoDecoder()->TakeScreenshot();

			if(!continueTest) {
				//Stop test
				_signal.Signal();
			}
		} else if(_console->GetFrameCount() == 3600) {
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

			_console->GetVideoDecoder()->TakeScreenshot();

			//Stop test
			_signal.Signal();
		}
	}
}

int32_t AutomaticRomTest::Run(string filename)
{
	_console.reset(new Console());
	EmulationSettings* settings = _console->GetSettings();
	settings->SetMasterVolume(0);
	_console->GetNotificationManager()->RegisterNotificationListener(shared_from_this());
	if(_console->Initialize(filename)) {
		_console->GetControlManager()->RegisterInputProvider(this);

		settings->SetFlags(EmulationFlags::ForceMaxSpeed);
		settings->ClearFlags(EmulationFlags::Paused);
		_signal.Wait();

		settings->SetFlags(EmulationFlags::Paused);

		if(_console->GetFrameCount() < 1800) {
			//Finished early
			_errorCode |= 0x10;
		}

		settings->ClearFlags(EmulationFlags::ForceMaxSpeed);
		settings->SetMasterVolume(1.0);

		_console->GetControlManager()->UnregisterInputProvider(this);
		_console->Stop();

		return _errorCode;
	}

	return -1;
}

bool AutomaticRomTest::SetInput(BaseControlDevice* device)
{
	if(device->GetPort() == 0) {
		uint32_t frameNumber = _console->GetFrameCount();
		ControlDeviceState state;

		if(frameNumber <= 1800) {
			if(frameNumber % 30 < 10) {
				//Press 1 button for 10 frames every second
				if((frameNumber / 30) % 8 != 1) {
					state.State.push_back(1 << ((frameNumber / 60) % 8));
				}
			}
		} else {
			if(frameNumber % 30 < 10) {
				if((frameNumber / 30) % 2) {
					state.State.push_back(0x01);
				} else {
					state.State.push_back(0x08);
				}
			}
		}
		if(state.State.empty()) {
			state.State.push_back(0);
		}
		device->SetRawState(state);
	}
	return true;
}
