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
		while(!_stopThread) {
			bool showMessage = false;
			uint32_t autoSaveDelay = console->GetSettings()->GetAutoSaveDelay(showMessage) * 60 * 1000;
			if(autoSaveDelay > 0) {
				if(_timer.GetElapsedMS() > autoSaveDelay) {
					if(!console->IsDebuggerAttached()) {
						console->GetSaveStateManager()->SaveState(_autoSaveSlot, showMessage);
					}
					_timer.Reset();
				}
			} else {
				_timer.Reset();
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
