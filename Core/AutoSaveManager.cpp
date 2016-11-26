#include "stdafx.h"
#include "AutoSaveManager.h"
#include "Console.h"
#include "EmulationSettings.h"
#include "SaveStateManager.h"

AutoSaveManager::AutoSaveManager()
{
	_stopThread = false;
	_timer.Reset();
	_autoSaveThread = std::thread([=]() {
		while(!_stopThread) {
			bool showMessage = false;
			uint32_t autoSaveDelay = EmulationSettings::GetAutoSaveDelay(showMessage) * 60 * 1000;
			if(autoSaveDelay > 0) {
				if(_timer.GetElapsedMS() > autoSaveDelay) {
					if(!Console::IsDebuggerAttached()) {
						SaveStateManager::SaveState(_autoSaveSlot, showMessage);
					}
					_timer.Reset();
				}
			} else {
				_timer.Reset();
			}
			std::this_thread::sleep_for(std::chrono::duration<int, std::milli>(1000));
		}
	});
}

AutoSaveManager::~AutoSaveManager()
{
	_stopThread = true;
	_autoSaveThread.join();
}
