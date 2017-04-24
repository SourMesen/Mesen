#include "stdafx.h"
#include "BizhawkMovie.h"
#include "VsControlManager.h"
#include "FDS.h"

BizhawkMovie::BizhawkMovie()
{
	_originalPowerOnState = EmulationSettings::GetRamPowerOnState();
	MessageManager::RegisterNotificationListener(this);
}

BizhawkMovie::~BizhawkMovie()
{
	MessageManager::UnregisterNotificationListener(this);
	EmulationSettings::SetRamPowerOnState(_originalPowerOnState);
}

void BizhawkMovie::ProcessNotification(ConsoleNotificationType type, void* parameter)
{
	if(type == ConsoleNotificationType::PpuFrameDone) {
		int32_t frameNumber = PPU::GetFrameCount() - 1;
		if(frameNumber < _systemActionByFrame.size()) {
			uint32_t systemAction = _systemActionByFrame[frameNumber];
			if(systemAction & 0x01) {
				//Power, not implemented yet
			}
			if(systemAction & 0x02) {
				//Reset, not implemented yet
			}
			
			if(FDS::GetSideCount()) {
				//FDS timings between NesHawk & Mesen are currently significantly different
				//So FDS games will always go out of sync
				if(systemAction & 0x04) {
					FDS::EjectDisk();
				} else if(systemAction >= 8) {
					systemAction >>= 3;
					uint32_t diskNumber = 0;
					while(!(systemAction & 0x01)) {
						systemAction >>= 1;
						diskNumber++;
					}
					FDS::InsertDisk(diskNumber);
				}
			} else if(VsControlManager::GetInstance()) {
				if(VsControlManager::GetInstance()) {
					if(systemAction & 0x04) {
						VsControlManager::GetInstance()->InsertCoin(0);
					}
					if(systemAction & 0x08) {
						VsControlManager::GetInstance()->InsertCoin(1);
					}
					VsControlManager::GetInstance()->SetServiceButtonState(systemAction & 0x10 ? true : false);
				}
			}
		}
	}
}

uint8_t BizhawkMovie::GetState(uint8_t port)
{
	int32_t frameNumber = PPU::GetFrameCount() - 1;
	if(frameNumber < _dataByFrame[0].size()) {
		return _dataByFrame[port][frameNumber];
	} else {
		EndMovie();
		EmulationSettings::SetRamPowerOnState(_originalPowerOnState);
		_isPlaying = false;
		return 0;
	}
}

bool BizhawkMovie::InitializeGameData(ZipReader & reader)
{
	std::stringstream ss = reader.GetStream("Header.txt");

	bool result = false;
	while(!ss.eof()) {
		string line;
		std::getline(ss, line);
		if(line.compare(0, 4, "SHA1", 4) == 0) {
			if(line.size() >= 45) {
				string sha1 = line.substr(5, 40);
				if(Console::LoadROM("", sha1)) {
					result = true;
				}
			}
		}
	}
	return result;
}

bool BizhawkMovie::InitializeInputData(ZipReader & reader)
{
	const uint8_t orValues[8] = { 0x10, 0x20, 0x40, 0x80, 0x08, 0x04, 0x02, 0x01 };
	std::stringstream ss = reader.GetStream("Input Log.txt");

	int systemActionCount = 2;
	if(FDS::GetSideCount() > 0) {
		//Eject disk + Insert Disk #XX
		systemActionCount += FDS::GetSideCount() + 1;
	} else if(VsControlManager::GetInstance()) {
		//Insert coin 1, 2 + service button
		systemActionCount += 3;
	}

	while(!ss.eof()) {
		string line;
		std::getline(ss, line);

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

			//Only supports regular controllers (up to 4 of them)
			for(int i = 0; i < 8*4; i++) {
				uint8_t port = i / 8;

				if(port <= 3) {
					uint8_t portValue = 0;
					for(int j = 0; j < 8 && i + j + systemActionCount < line.size(); j++) {
						if(line[i+j+systemActionCount] != '.') {
							portValue |= orValues[j];
						}
					}
					i += 7;
					_dataByFrame[port].push_back(portValue);
				}
			}
		}
	}

	return _dataByFrame[0].size() > 0;
}

bool BizhawkMovie::Play(stringstream & filestream, bool autoLoadRom)
{
	Console::Pause();
	ZipReader reader;
	reader.LoadArchive(filestream);
	if(InitializeGameData(reader)) {
		if(InitializeInputData(reader)) {
			//NesHawk initializes memory to 1s
			EmulationSettings::SetRamPowerOnState(RamPowerOnState::AllOnes);
			Console::Reset(false);
			_isPlaying = true;
		}
	}
	Console::Resume();
	return _isPlaying;
}

bool BizhawkMovie::IsRecording()
{
	return false;
}

bool BizhawkMovie::IsPlaying()
{
	return _isPlaying;
}

void BizhawkMovie::RecordState(uint8_t port, uint8_t value)
{
	//Not implemented
}

void BizhawkMovie::Record(string filename, bool reset)
{
	//Not implemented
}
