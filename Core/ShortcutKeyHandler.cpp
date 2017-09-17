#include "stdafx.h"
#include "ShortcutKeyHandler.h"
#include "EmulationSettings.h"
#include "ControlManager.h"
#include "VideoDecoder.h"
#include "VsControlManager.h"
#include "FDS.h"
#include "SaveStateManager.h"
#include "RewindManager.h"

ShortcutKeyHandler::ShortcutKeyHandler()
{
	_keySetIndex = 0;
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

	return ControlManager::IsKeyPressed(comb.Key1) &&
		(comb.Key2 == 0 || ControlManager::IsKeyPressed(comb.Key2)) &&
		(comb.Key3 == 0 || ControlManager::IsKeyPressed(comb.Key3));
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

	//Let the UI handle these shortcuts
	for(uint64_t i = (uint64_t)EmulatorShortcut::SwitchDiskSide; i <= (uint64_t)EmulatorShortcut::OpenTraceLogger; i++) {
		if(DetectKeyPress((EmulatorShortcut)i)) {
			void* param = (void*)i;
			MessageManager::SendNotification(ConsoleNotificationType::ExecuteShortcut, param);
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

	if(VsControlManager::GetInstance() && !isNetplayClient && !isMovieActive) {
		VsControlManager* manager = VsControlManager::GetInstance();
		if(DetectKeyPress(EmulatorShortcut::VsServiceButton)) {
			manager->SetServiceButtonState(true);
		}
		if(DetectKeyRelease(EmulatorShortcut::VsServiceButton)) {
			manager->SetServiceButtonState(false);
		}
	}

	if(DetectKeyPress(EmulatorShortcut::InsertNextDisk) && !isNetplayClient && !isMovieActive) {
		FDS::InsertNextDisk();
	}

	if(DetectKeyPress(EmulatorShortcut::MoveToNextStateSlot)) {
		SaveStateManager::MoveToNextSlot();
	}

	if(DetectKeyPress(EmulatorShortcut::MoveToPreviousStateSlot)) {
		SaveStateManager::MoveToPreviousSlot();
	}

	if(DetectKeyPress(EmulatorShortcut::SaveState)) {
		SaveStateManager::SaveState();
	}

	if(DetectKeyPress(EmulatorShortcut::LoadState) && !isNetplayClient) {
		SaveStateManager::LoadState();
	}

	if(DetectKeyPress(EmulatorShortcut::ToggleCheats) && !isNetplayClient && !isMovieActive) {
		MessageManager::SendNotification(ConsoleNotificationType::ExecuteShortcut, (void*)EmulatorShortcut::ToggleCheats);
	}

	if(DetectKeyPress(EmulatorShortcut::ToggleAudio)) {
		MessageManager::SendNotification(ConsoleNotificationType::ExecuteShortcut, (void*)EmulatorShortcut::ToggleAudio);
	}

	if(DetectKeyPress(EmulatorShortcut::RunSingleFrame)) {
		if(EmulationSettings::CheckFlag(EmulationFlags::Paused)) {
			EmulationSettings::ClearFlags(EmulationFlags::Paused);
			Console::Pause();
			std::this_thread::sleep_for(std::chrono::duration<int, std::milli>(50));
			EmulationSettings::SetFlags(EmulationFlags::Paused);
			Console::Resume();
		} else {
			EmulationSettings::SetFlags(EmulationFlags::Paused);
		}
	}

	if(!isNetplayClient && !isMovieActive && !EmulationSettings::CheckFlag(NsfPlayerEnabled)) {
		if(DetectKeyPress(EmulatorShortcut::ToggleRewind)) {
			if(RewindManager::IsRewinding()) {
				RewindManager::StopRewinding();
			} else {
				RewindManager::StartRewinding();
			}
		}

		if(DetectKeyPress(EmulatorShortcut::Rewind)) {
			RewindManager::StartRewinding();
		} else if(DetectKeyRelease(EmulatorShortcut::Rewind)) {
			RewindManager::StopRewinding();
		} else  if(DetectKeyPress(EmulatorShortcut::RewindTenSecs)) {
			RewindManager::RewindSeconds(10);
		} else if(DetectKeyPress(EmulatorShortcut::RewindOneMin)) {
			RewindManager::RewindSeconds(60);
		}
	}
}

void ShortcutKeyHandler::ProcessKeys()
{
	auto lock = _lock.AcquireSafe();
	ControlManager::RefreshKeyState();

	_pressedKeys = ControlManager::GetPressedKeys();
	_isKeyUp = _pressedKeys.size() < _lastPressedKeys.size();

	if(_pressedKeys.size() == _lastPressedKeys.size()) {
		bool noChange = true;
		for(int i = 0; i < _pressedKeys.size(); i++) {
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