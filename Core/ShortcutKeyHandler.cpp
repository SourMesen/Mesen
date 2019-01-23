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
	_repeatStarted = false;

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
	KeyCombination keyComb = _console->GetSettings()->GetShortcutKey(shortcut, _keySetIndex);
	vector<KeyCombination> supersets = _console->GetSettings()->GetShortcutSupersets(shortcut, _keySetIndex);
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

void ShortcutKeyHandler::ProcessRunSingleFrame()
{
	EmulationSettings* settings = _console->GetSettings();
	if(!_runSingleFrameRepeatTimer) {
		_runSingleFrameRepeatTimer.reset(new Timer());
	}
	_runSingleFrameRepeatTimer->Reset();

	if(settings->CheckFlag(EmulationFlags::DebuggerWindowEnabled)) {
		shared_ptr<Debugger> debugger = _console->GetDebugger(false);
		if(debugger) {
			debugger->BreakOnScanline(241);
		}
	} else {
		_console->PauseOnNextFrame();
		settings->ClearFlags(EmulationFlags::Paused);
	}
}

void ShortcutKeyHandler::CheckMappedKeys()
{
	EmulationSettings* settings = _console->GetSettings();
	bool isNetplayClient = GameClient::Connected();
	bool isMovieActive = MovieManager::Playing() || MovieManager::Recording();

	_keyboardMode = false;
	if(DetectKeyPress(EmulatorShortcut::ToggleKeyboardMode)) {
		if(settings->IsKeyboardMode()) {
			settings->DisableKeyboardMode();
		} else {
			ControlManager* controlManager = _console->GetControlManager();
			if(controlManager && controlManager->HasKeyboard()) {
				settings->EnableKeyboardMode();
			}
		}
	}
	_keyboardMode = settings->IsKeyboardMode();

	//Let the UI handle these shortcuts
	for(uint64_t i = (uint64_t)EmulatorShortcut::SwitchDiskSide; i < (uint64_t)EmulatorShortcut::ShortcutCount; i++) {
		if(DetectKeyPress((EmulatorShortcut)i)) {
			void* param = (void*)i;
			_console->GetNotificationManager()->SendNotification(ConsoleNotificationType::ExecuteShortcut, param);
		}
	}

	if(DetectKeyPress(EmulatorShortcut::FastForward)) {
		settings->SetFlags(EmulationFlags::Turbo);
	} else if(DetectKeyRelease(EmulatorShortcut::FastForward)) {
		settings->ClearFlags(EmulationFlags::Turbo);
	}

	if(DetectKeyPress(EmulatorShortcut::ToggleFastForward)) {
		if(settings->CheckFlag(EmulationFlags::Turbo)) {
			settings->ClearFlags(EmulationFlags::Turbo);
		} else {
			settings->SetFlags(EmulationFlags::Turbo);
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
		ProcessRunSingleFrame();
	}

	if(DetectKeyRelease(EmulatorShortcut::RunSingleFrame)) {
		_runSingleFrameRepeatTimer.reset();
		_repeatStarted = false;
	}

	if(!isNetplayClient && !MovieManager::Recording()) {
		shared_ptr<RewindManager> rewindManager = _console->GetRewindManager();
		if(rewindManager) {
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
}

void ShortcutKeyHandler::ProcessKeys()
{
	if(!_console->GetSettings()->InputEnabled()) {
		return;
	}

	auto lock = _lock.AcquireSafe();
	KeyManager::RefreshKeyState();

	_pressedKeys = KeyManager::GetPressedKeys();
	_isKeyUp = _pressedKeys.size() < _lastPressedKeys.size();

	bool noChange = false;
	if(_pressedKeys.size() == _lastPressedKeys.size()) {
		noChange = true;
		for(size_t i = 0; i < _pressedKeys.size(); i++) {
			if(_pressedKeys[i] != _lastPressedKeys[i]) {
				noChange = false;
				break;
			}
		}
	}

	if(!noChange) {
		//Only run this if the keys have changed
		for(int i = 0; i < 2; i++) {
			_keysDown[i].clear();
			_keySetIndex = i;
			CheckMappedKeys();
			_prevKeysDown[i] = _keysDown[i];
		}

		_lastPressedKeys = _pressedKeys;
	}

	if(_runSingleFrameRepeatTimer) {
		double elapsedMs = _runSingleFrameRepeatTimer->GetElapsedMS();
		if((_repeatStarted && elapsedMs >= 50) || (!_repeatStarted && elapsedMs >= 500)) {
			//Over 500ms has elapsed since the key was first pressed, or over 50ms since repeat mode started (20fps)
			//In this case, run another frame and pause again.
			_repeatStarted = true;
			ProcessRunSingleFrame();
		}
	}
}