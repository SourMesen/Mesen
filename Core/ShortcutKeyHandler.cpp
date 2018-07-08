#include "stdafx.h"
#include "ShortcutKeyHandler.h"
#include "EmulationSettings.h"
#include "KeyManager.h"
#include "VideoDecoder.h"
#include "FDS.h"
#include "SaveStateManager.h"
#include "RewindManager.h"
#include "SystemActionManager.h"
#include "FdsSystemActionManager.h"
#include "VsSystemActionManager.h"
#include "MovieManager.h"
#include "ControlManager.h"
#include "Console.h"
#include "NotificationManager.h"

ShortcutKeyHandler::ShortcutKeyHandler(shared_ptr<Console> console)
{
	_console = console;
	_keySetIndex = 0;
	_isKeyUp = false;
	_keyboardMode = false;

	_stopThread = false;
	_thread = std::thread([=]() {
		while(!_stopThread) {
			ProcessKeys();
			std::this_thread::sleep_for(std::chrono::duration<int, std::milli>(50));
		}
	});
}

ShortcutKeyHandler::~ShortcutKeyHandler()
{
	_stopThread = true;
	_thread.join();
}

bool ShortcutKeyHandler::IsKeyPressed(EmulatorShortcut shortcut)
{
	KeyCombination keyComb = EmulationSettings::GetShortcutKey(shortcut, _keySetIndex);
	vector<KeyCombination> supersets = EmulationSettings::GetShortcutSupersets(shortcut, _keySetIndex);
	for(KeyCombination &superset : supersets) {
		if(IsKeyPressed(superset)) {
			//A superset is pressed, ignore this subset
			return false;
		}
	}

	//No supersets are pressed, check if all matching keys are pressed
	return IsKeyPressed(keyComb);
}

bool ShortcutKeyHandler::IsKeyPressed(KeyCombination comb)
{
	int keyCount = (comb.Key1 ? 1 : 0) + (comb.Key2 ? 1 : 0) + (comb.Key3 ? 1 : 0);

	if(keyCount == 0 || _pressedKeys.empty()) {
		return false;
	}

	return IsKeyPressed(comb.Key1) &&
		(comb.Key2 == 0 || IsKeyPressed(comb.Key2)) &&
		(comb.Key3 == 0 || IsKeyPressed(comb.Key3));
}

bool ShortcutKeyHandler::IsKeyPressed(uint32_t keyCode)
{
	if(keyCode >= 0x200 || !_keyboardMode) {
		return KeyManager::IsKeyPressed(keyCode);
	} else {
		return false;
	}
}

bool ShortcutKeyHandler::DetectKeyPress(EmulatorShortcut shortcut)
{
	if(IsKeyPressed(shortcut)) {
		bool newlyPressed = _prevKeysDown[_keySetIndex].find((uint32_t)shortcut) == _prevKeysDown[_keySetIndex].end();
		_keysDown[_keySetIndex].emplace((uint32_t)shortcut);

		if(newlyPressed && !_isKeyUp) {
			return true;
		}
	}
	return false;
}

bool ShortcutKeyHandler::DetectKeyRelease(EmulatorShortcut shortcut)
{
	if(!IsKeyPressed(shortcut)) {
		if(_prevKeysDown[_keySetIndex].find((uint32_t)shortcut) != _prevKeysDown[_keySetIndex].end()) {
			return true;
		}
	}
	return false;
}

void ShortcutKeyHandler::CheckMappedKeys()
{
	bool isNetplayClient = GameClient::Connected();
	bool isMovieActive = MovieManager::Playing() || MovieManager::Recording();

	_keyboardMode = false;
	if(DetectKeyPress(EmulatorShortcut::ToggleKeyboardMode)) {
		if(EmulationSettings::IsKeyboardMode()) {
			EmulationSettings::DisableKeyboardMode();
		} else {
			ControlManager* controlManager = _console->GetControlManager();
			if(controlManager && controlManager->HasKeyboard()) {
				EmulationSettings::EnableKeyboardMode();
			}
		}
	}
	_keyboardMode = EmulationSettings::IsKeyboardMode();

	//Let the UI handle these shortcuts
	for(uint64_t i = (uint64_t)EmulatorShortcut::SwitchDiskSide; i < (uint64_t)EmulatorShortcut::ShortcutCount; i++) {
		if(DetectKeyPress((EmulatorShortcut)i)) {
			void* param = (void*)i;
			_console->GetNotificationManager()->SendNotification(ConsoleNotificationType::ExecuteShortcut, param);
		}
	}

	if(DetectKeyPress(EmulatorShortcut::FastForward)) {
		EmulationSettings::SetFlags(EmulationFlags::Turbo);
	} else if(DetectKeyRelease(EmulatorShortcut::FastForward)) {
		EmulationSettings::ClearFlags(EmulationFlags::Turbo);
	}

	if(DetectKeyPress(EmulatorShortcut::ToggleFastForward)) {
		if(EmulationSettings::CheckFlag(EmulationFlags::Turbo)) {
			EmulationSettings::ClearFlags(EmulationFlags::Turbo);
		} else {
			EmulationSettings::SetFlags(EmulationFlags::Turbo);
		}
	}

	shared_ptr<VsSystemActionManager> vsSam = _console->GetSystemActionManager<VsSystemActionManager>();
	if(vsSam && !isNetplayClient && !MovieManager::Playing()) {
		if(DetectKeyPress(EmulatorShortcut::VsServiceButton)) {
			vsSam->SetServiceButtonState(0, true);
		}
		if(DetectKeyRelease(EmulatorShortcut::VsServiceButton)) {
			vsSam->SetServiceButtonState(0, false);
		}
		if(DetectKeyPress(EmulatorShortcut::VsServiceButton2)) {
			vsSam->SetServiceButtonState(1, true);
		}
		if(DetectKeyRelease(EmulatorShortcut::VsServiceButton2)) {
			vsSam->SetServiceButtonState(1, false);
		}
	}

	if(DetectKeyPress(EmulatorShortcut::InsertNextDisk) && !isNetplayClient && !MovieManager::Playing()) {
		shared_ptr<FdsSystemActionManager> sam = _console->GetSystemActionManager<FdsSystemActionManager>();
		if(sam) {
			sam->InsertNextDisk();
		}
	}

	if(DetectKeyPress(EmulatorShortcut::MoveToNextStateSlot)) {
		_console->GetSaveStateManager()->MoveToNextSlot();
	}

	if(DetectKeyPress(EmulatorShortcut::MoveToPreviousStateSlot)) {
		_console->GetSaveStateManager()->MoveToPreviousSlot();
	}

	if(DetectKeyPress(EmulatorShortcut::SaveState)) {
		_console->GetSaveStateManager()->SaveState();
	}

	if(DetectKeyPress(EmulatorShortcut::LoadState) && !isNetplayClient) {
		_console->GetSaveStateManager()->LoadState();
	}

	if(DetectKeyPress(EmulatorShortcut::ToggleCheats) && !isNetplayClient && !isMovieActive) {
		_console->GetNotificationManager()->SendNotification(ConsoleNotificationType::ExecuteShortcut, (void*)EmulatorShortcut::ToggleCheats);
	}

	if(DetectKeyPress(EmulatorShortcut::ToggleAudio)) {
		_console->GetNotificationManager()->SendNotification(ConsoleNotificationType::ExecuteShortcut, (void*)EmulatorShortcut::ToggleAudio);
	}

	if(DetectKeyPress(EmulatorShortcut::RunSingleFrame)) {
		if(EmulationSettings::CheckFlag(EmulationFlags::DebuggerWindowEnabled)) {
			shared_ptr<Debugger> debugger = _console->GetDebugger(false);
			if(debugger) {
				debugger->BreakOnScanline(241);
			}
		} else {
			if(EmulationSettings::CheckFlag(EmulationFlags::Paused)) {
				EmulationSettings::ClearFlags(EmulationFlags::Paused);
				_console->Pause();
				std::this_thread::sleep_for(std::chrono::duration<int, std::milli>(50));
				EmulationSettings::SetFlags(EmulationFlags::Paused);
				_console->Resume();
			} else {
				EmulationSettings::SetFlags(EmulationFlags::Paused);
			}
		}
	}

	if(!isNetplayClient && !MovieManager::Recording() && !EmulationSettings::CheckFlag(NsfPlayerEnabled)) {
		RewindManager* rewindManager = _console->GetRewindManager();
		if(DetectKeyPress(EmulatorShortcut::ToggleRewind)) {
			if(rewindManager->IsRewinding()) {
				rewindManager->StopRewinding();
			} else {
				rewindManager->StartRewinding();
			}
		}

		if(DetectKeyPress(EmulatorShortcut::Rewind)) {
			rewindManager->StartRewinding();
		} else if(DetectKeyRelease(EmulatorShortcut::Rewind)) {
			rewindManager->StopRewinding();
		} else  if(DetectKeyPress(EmulatorShortcut::RewindTenSecs)) {
			rewindManager->RewindSeconds(10);
		} else if(DetectKeyPress(EmulatorShortcut::RewindOneMin)) {
			rewindManager->RewindSeconds(60);
		}
	}
}

void ShortcutKeyHandler::ProcessKeys()
{
	if(!EmulationSettings::InputEnabled()) {
		return;
	}

	auto lock = _lock.AcquireSafe();
	KeyManager::RefreshKeyState();

	_pressedKeys = KeyManager::GetPressedKeys();
	_isKeyUp = _pressedKeys.size() < _lastPressedKeys.size();

	if(_pressedKeys.size() == _lastPressedKeys.size()) {
		bool noChange = true;
		for(size_t i = 0; i < _pressedKeys.size(); i++) {
			if(_pressedKeys[i] != _lastPressedKeys[i]) {
				noChange = false;
				break;
			}
		}
		if(noChange) {
			//Same keys as before, nothing to do
			return;
		}
	}

	for(int i = 0; i < 2; i++) {
		_keysDown[i].clear();
		_keySetIndex = i;
		CheckMappedKeys();
		_prevKeysDown[i] = _keysDown[i];
	}

	_lastPressedKeys = _pressedKeys;
}