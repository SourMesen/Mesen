#include "stdafx.h"
#include "ShortcutKeyHandler.h"
#include "EmulationSettings.h"
#include "ControlManager.h"
#include "VideoDecoder.h"
#include "VsControlManager.h"
#include "FDS.h"
#include "SaveStateManager.h"

ShortcutKeyHandler::ShortcutKeyHandler()
{
	_stopThread = false;
	_thread = std::thread([=]() {
		while(!_stopThread) {
			ProcessKeys(EmulationSettings::GetEmulatorKeys());
			std::this_thread::sleep_for(std::chrono::duration<int, std::milli>(50));
		}
	});
}

ShortcutKeyHandler::~ShortcutKeyHandler()
{
	_stopThread = true;
	_thread.join();
}

bool ShortcutKeyHandler::DetectKeyPress(uint32_t keyCode)
{
	if(ControlManager::IsKeyPressed(keyCode)) {
		_keysDown.emplace(keyCode);

		if(_prevKeysDown.find(keyCode) == _prevKeysDown.end()) {
			return true;
		}
	}
	return false;
}

void ShortcutKeyHandler::CheckMappedKeys(EmulatorKeyMappings mappings)
{
	if(ControlManager::IsKeyPressed(mappings.FastForward)) {
		_turboEnabled = true;
	}

	if(DetectKeyPress(mappings.TakeScreenshot)) {
		VideoDecoder::GetInstance()->TakeScreenshot();
	}

	if(DetectKeyPress(mappings.InsertCoin1)) {
		if(VsControlManager::GetInstance()) {
			VsControlManager::GetInstance()->InsertCoin(0);
		}
	}

	if(DetectKeyPress(mappings.InsertCoin2)) {
		if(VsControlManager::GetInstance()) {
			VsControlManager::GetInstance()->InsertCoin(1);
		}
	}

	if(DetectKeyPress(mappings.SwitchDiskSide)) {
		FDS::SwitchDiskSide();
	}

	if(DetectKeyPress(mappings.InsertNextDisk)) {
		FDS::InsertNextDisk();
	}

	if(DetectKeyPress(mappings.MoveToNextStateSlot)) {
		SaveStateManager::MoveToNextSlot();
	}

	if(DetectKeyPress(mappings.MoveToPreviousStateSlot)) {
		SaveStateManager::MoveToPreviousSlot();
	}

	if(DetectKeyPress(mappings.SaveState)) {
		SaveStateManager::SaveState();
	}

	if(DetectKeyPress(mappings.LoadState)) {
		SaveStateManager::LoadState();
	}

	if(DetectKeyPress(mappings.Reset)) {
		Console::Reset(true);
	}

	if(DetectKeyPress(mappings.Pause)) {
		if(EmulationSettings::CheckFlag(EmulationFlags::Paused)) {
			EmulationSettings::ClearFlags(EmulationFlags::Paused);
		} else {
			EmulationSettings::SetFlags(EmulationFlags::Paused);
		}
	}
}

void ShortcutKeyHandler::ProcessKeys(EmulatorKeyMappingSet mappings)
{
	ControlManager::RefreshKeyState();

	_keysDown.clear();

	_turboEnabled = false;
	CheckMappedKeys(mappings.KeySet1);
	CheckMappedKeys(mappings.KeySet2);
	if(_turboEnabled) {
		EmulationSettings::SetFlags(EmulationFlags::Turbo);
	} else {
		EmulationSettings::ClearFlags(EmulationFlags::Turbo);
	}

	_prevKeysDown = _keysDown;
}