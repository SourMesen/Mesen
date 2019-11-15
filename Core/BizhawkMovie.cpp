#include "stdafx.h"
#include "ControlManager.h"
#include "SystemActionManager.h"
#include "FdsSystemActionManager.h"
#include "VsSystemActionManager.h"
#include "BizhawkMovie.h"
#include "VsControlManager.h"
#include "Console.h"
#include "BatteryManager.h"
#include "NotificationManager.h"

BizhawkMovie::BizhawkMovie(shared_ptr<Console> console)
{
	_console = console;
	_originalPowerOnState = _console->GetSettings()->GetRamPowerOnState();
}

BizhawkMovie::~BizhawkMovie()
{
	Stop();
}

void BizhawkMovie::Stop()
{
	if(_isPlaying) {
		MessageManager::DisplayMessage("Movies", "MovieEnded");

		_console->GetNotificationManager()->SendNotification(ConsoleNotificationType::MovieEnded);
		if(_console->GetSettings()->CheckFlag(EmulationFlags::PauseOnMovieEnd)) {
			_console->GetSettings()->SetFlags(EmulationFlags::Paused);
		}

		_console->GetSettings()->SetRamPowerOnState(_originalPowerOnState);
		_isPlaying = false;
	}
	_console->GetControlManager()->UnregisterInputProvider(this);
}

bool BizhawkMovie::SetInput(BaseControlDevice *device)
{
	SystemActionManager* actionManager = dynamic_cast<SystemActionManager*>(device);
	int32_t pollCounter = _console->GetControlManager()->GetPollCounter();
	if(actionManager) {
		if(pollCounter < (int32_t)_systemActionByFrame.size()) {
			uint32_t systemAction = _systemActionByFrame[pollCounter];
			if(systemAction & 0x01) {
				actionManager->SetBit(SystemActionManager::Buttons::PowerButton);
			}
			if(systemAction & 0x02) {
				actionManager->SetBit(SystemActionManager::Buttons::ResetButton);
			}

			VsSystemActionManager* vsActionManager = dynamic_cast<VsSystemActionManager*>(device);
			if(vsActionManager) {
				if(systemAction & 0x04) {
					actionManager->SetBit(VsSystemActionManager::VsButtons::InsertCoin1);
				}
				if(systemAction & 0x08) {
					actionManager->SetBit(VsSystemActionManager::VsButtons::InsertCoin2);
				}
				if(systemAction & 0x10) {
					actionManager->SetBit(VsSystemActionManager::VsButtons::ServiceButton);
				}
			}

			FdsSystemActionManager* fdsActionManager = dynamic_cast<FdsSystemActionManager*>(device);
			if(fdsActionManager) {
				//FDS timings between NesHawk & Mesen are currently significantly different
				//So FDS games will always go out of sync
				if(systemAction & 0x04) {
					fdsActionManager->SetBit(FdsSystemActionManager::FdsButtons::EjectDiskButton);
				}
				
				if(systemAction >= 8) {
					systemAction >>= 3;
					uint32_t diskNumber = 0;
					while(!(systemAction & 0x01)) {
						systemAction >>= 1;
						diskNumber++;
					}

					fdsActionManager->SetBit(FdsSystemActionManager::FdsButtons::InsertDisk1 + diskNumber);
				}
			}
		}
	} else {
		int port = device->GetPort();
		StandardController* controller = dynamic_cast<StandardController*>(device);
		if(controller) {
			if(pollCounter < (int32_t)_dataByFrame[port].size()) {
				controller->SetTextState(_dataByFrame[port][pollCounter]);
			} else {
				Stop();
			}
		}
	}

	return true;
}

bool BizhawkMovie::InitializeGameData(ZipReader &reader)
{
	stringstream fileData;
	if(!reader.GetStream("Header.txt", fileData)) {
		return false;
	}

	_console->GetControlManager()->SetPollCounter(0);

	while(!fileData.eof()) {
		string line;
		std::getline(fileData, line);
		if(line.compare(0, 4, "SHA1", 4) == 0) {
			if(line.size() >= 45) {
				HashInfo hashInfo;
				hashInfo.Sha1 = line.substr(5, 40);
				if(_console->LoadMatchingRom("", hashInfo)) {
					return true;
				}
			}
		} else if(line.compare(0, 3, "MD5", 3) == 0) {
			if(line.size() >= 36) {
				HashInfo hashInfo;
				hashInfo.PrgChrMd5 = line.substr(4, 32);
				std::transform(hashInfo.PrgChrMd5.begin(), hashInfo.PrgChrMd5.end(), hashInfo.PrgChrMd5.begin(), ::toupper);
				if(_console->LoadMatchingRom("", hashInfo)) {
					return true;
				}
			}
		}
	}
	return false;
}

bool BizhawkMovie::InitializeInputData(ZipReader &reader)
{
	stringstream inputData;
	if(!reader.GetStream("Input Log.txt", inputData)) {
		return false;
	}

	int systemActionCount = 2;
	shared_ptr<FdsSystemActionManager> fdsActionManager = _console->GetSystemActionManager<FdsSystemActionManager>();
	if(fdsActionManager) {
		//Eject disk + Insert Disk #XX
		systemActionCount += fdsActionManager->GetSideCount() + 1;
	} else {
		shared_ptr<VsSystemActionManager> vsActionManager = _console->GetSystemActionManager<VsSystemActionManager>();
		if(vsActionManager) {
			//Insert coin 1, 2 + service button
			systemActionCount += 3;
		}
	}
	
	while(!inputData.eof()) {
		string line;
		std::getline(inputData, line);

		if(line.size() > 0 && line[0] == '|') {
			line.erase(std::remove(line.begin(), line.end(), '|'), line.end());
			line = line.substr(0, line.size() - 1);

			//Read power/reset/FDS/VS/etc. commands
			uint32_t systemAction = 0;
			for(int i = 0; i < systemActionCount; i++) {
				if(line[i] != '.') {
					systemAction |= (1 << i);
				}
			}
			_systemActionByFrame.push_back(systemAction);

			line = line.substr(systemActionCount);
			int port = 0;
			while(line.size() >= 8) {
				_dataByFrame[port].push_back(line.substr(0, 8));
				line = line.substr(8);
				port++;
			}
			while(port < 4) {
				_dataByFrame[port].push_back("........");
				port++;
			}
		}
	}

	return _dataByFrame[0].size() > 0;
}

bool BizhawkMovie::Play(VirtualFile &file)
{
	_console->Pause();
	ZipReader reader;

	std::stringstream ss;
	file.ReadFile(ss);

	reader.LoadArchive(ss);
	
	_console->GetNotificationManager()->RegisterNotificationListener(shared_from_this());
	_console->GetSettings()->SetRamPowerOnState(RamPowerOnState::AllOnes);
	_console->GetBatteryManager()->SetBatteryProvider(shared_from_this());
	if(InitializeInputData(reader) && InitializeGameData(reader)) {
		//NesHawk initializes memory to 1s
		_isPlaying = true;
	}
	_console->Resume();
	return _isPlaying;
}

bool BizhawkMovie::IsPlaying()
{
	return _isPlaying;
}

void BizhawkMovie::ProcessNotification(ConsoleNotificationType type, void* parameter)
{
	if(type == ConsoleNotificationType::GameLoaded) {
		_console->GetControlManager()->RegisterInputProvider(this);
	}
}

vector<uint8_t> BizhawkMovie::LoadBattery(string extension)
{
	return vector<uint8_t>();
}
