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

bool ShortcutKeyHandler::DetectKeyRelease(uint32_t keyCode)
{
	if(!ControlManager::IsKeyPressed(keyCode)) {
		if(_prevKeysDown.find(keyCode) != _prevKeysDown.end()) {
			return true;
		}
	}
	return false;
}

void ShortcutKeyHandler::CheckMappedKeys(EmulatorKeyMappings mappings)
{
	bool isNetplayClient = GameClient::Connected();
	bool isMovieActive = MovieManager::Playing() || MovieManager::Recording();

	if(DetectKeyPress(mappings.FastForward)) {
		EmulationSettings::SetFlags(EmulationFlags::Turbo);
	} else if(DetectKeyRelease(mappings.FastForward)) {
		EmulationSettings::ClearFlags(EmulationFlags::Turbo);
	}

	if(DetectKeyPress(mappings.IncreaseSpeed)) {
		EmulationSettings::IncreaseEmulationSpeed();
	}
	
	if(DetectKeyPress(mappings.DecreaseSpeed)) {
		EmulationSettings::DecreaseEmulationSpeed();
	}

	if(DetectKeyPress(mappings.TakeScreenshot)) {
		VideoDecoder::GetInstance()->TakeScreenshot();
	}

	if(VsControlManager::GetInstance() && !isNetplayClient && !isMovieActive) {
		VsControlManager* manager = VsControlManager::GetInstance();
		if(DetectKeyPress(mappings.InsertCoin1)) {
			manager->InsertCoin(0);
		}
		if(DetectKeyPress(mappings.InsertCoin2)) {
			manager->InsertCoin(1);
		}
		if(DetectKeyPress(mappings.VsServiceButton)) {
			manager->SetServiceButtonState(true);
		}
		if(DetectKeyRelease(mappings.VsServiceButton)) {
			manager->SetServiceButtonState(false);
		}
	}

	if(DetectKeyPress(mappings.SwitchDiskSide) && !isNetplayClient && !isMovieActive) {
		FDS::SwitchDiskSide();
	}

	if(DetectKeyPress(mappings.InsertNextDisk) && !isNetplayClient && !isMovieActive) {
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

	if(DetectKeyPress(mappings.LoadState) && !isNetplayClient) {
		SaveStateManager::LoadState();
	}

	if(DetectKeyPress(mappings.Reset) && !isNetplayClient && !isMovieActive) {
		Console::Reset(true);
	}

	if(DetectKeyPress(mappings.Pause) && !isNetplayClient) {
		if(EmulationSettings::CheckFlag(EmulationFlags::Paused)) {
			EmulationSettings::ClearFlags(EmulationFlags::Paused);
		} else {
			EmulationSettings::SetFlags(EmulationFlags::Paused);
		}
	}

	if(DetectKeyPress(mappings.Exit)) {
		MessageManager::SendNotification(ConsoleNotificationType::RequestExit);
	}

	if(DetectKeyPress(mappings.ToggleCheats) && !isNetplayClient && !isMovieActive) {
		MessageManager::SendNotification(ConsoleNotificationType::ToggleCheats);
	}

	if(DetectKeyPress(mappings.ToggleAudio)) {
		MessageManager::SendNotification(ConsoleNotificationType::ToggleAudio);
	}

	if(ControlManager::IsKeyPressed(mappings.RunSingleFrame)) {
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
		if(DetectKeyPress(mappings.Rewind)) {
			RewindManager::StartRewinding();
		} else if(DetectKeyRelease(mappings.Rewind)) {
			RewindManager::StopRewinding();
		} else  if(DetectKeyPress(mappings.RewindTenSecs)) {
			RewindManager::RewindSeconds(10);
		} else if(DetectKeyPress(mappings.RewindOneMin)) {
			RewindManager::RewindSeconds(60);
		}
	}
}

void ShortcutKeyHandler::ProcessKeys(EmulatorKeyMappingSet mappings)
{
	ControlManager::RefreshKeyState();

	_keysDown.clear();
	CheckMappedKeys(mappings.KeySet1);
	CheckMappedKeys(mappings.KeySet2);
	_prevKeysDown = _keysDown;
}