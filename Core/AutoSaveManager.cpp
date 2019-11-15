#include "stdafx.h"
#include "AutoSaveManager.h"
#include "Console.h"
#include "EmulationSettings.h"
#include "SaveStateManager.h"

AutoSaveManager::AutoSaveManager(shared_ptr<Console> console)
{
	_stopThread = false;
	_timer.Reset();
	_autoSaveThread = std::thread([=]() {
		bool showMessage = false;
		double targetTime = (double)console->GetSettings()->GetAutoSaveDelay(showMessage) * 60 * 1000;
		while(!_stopThread) {
			uint32_t autoSaveDelay = console->GetSettings()->GetAutoSaveDelay(showMessage) * 60 * 1000;
			if(autoSaveDelay > 0) {
				if(targetTime >= 0 && !console->IsExecutionStopped()) {
					targetTime -= _timer.GetElapsedMS();
					_timer.Reset();
					if(targetTime <= 0) {
						if(!console->IsDebuggerAttached()) {
							console->GetSaveStateManager()->SaveState(_autoSaveSlot, showMessage);
						}
						targetTime = (double)console->GetSettings()->GetAutoSaveDelay(showMessage) * 60 * 1000;
						_timer.Reset();
					}
				} else {
					_timer.Reset();
				}
			} else {
				_timer.Reset();
				targetTime = autoSaveDelay;
			}

			if(!_stopThread) {
				_signal.Wait(1000);
			}
		}
	});
}

AutoSaveManager::~AutoSaveManager()
{
	_stopThread = true;
	_signal.Signal();
	_autoSaveThread.join();
}
