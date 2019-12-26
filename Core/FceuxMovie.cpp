#include "stdafx.h"
#include <algorithm>
#include "../Utilities/StringUtilities.h"
#include "../Utilities/HexUtilities.h"
#include "../Utilities/Base64.h"
#include "ControlManager.h"
#include "FceuxMovie.h"
#include "Console.h"
#include "NotificationManager.h"

bool FceuxMovie::InitializeData(stringstream &filestream)
{
	bool result = false;

	_dataByFrame[0].push_back("");
	_dataByFrame[1].push_back("");
	_dataByFrame[2].push_back("");
	_dataByFrame[3].push_back("");

	_console->GetControlManager()->SetPollCounter(0);

	while(!filestream.eof()) {
		string line;
		std::getline(filestream, line);
		if(line.compare(0, 19, "romChecksum base64:", 19) == 0) {
			vector<uint8_t> md5array = Base64::Decode(line.substr(19, line.size() - 20));
			HashInfo hashInfo;
			hashInfo.PrgChrMd5 = HexUtilities::ToHex(md5array);
			_console->GetSettings()->SetRamPowerOnState(RamPowerOnState::AllZeros);
			if(_console->LoadMatchingRom("", hashInfo)) {
				result = true;
			} else {
				return false;
			}
		} else if(line.size() > 0 && line[0] == '|') {
			vector<string> lineData = StringUtilities::Split(line.substr(1), '|');
			
			if(lineData.size() == 0) {
				continue;
			}

			//Read power/reset/FDS/VS/etc. commands
			uint32_t systemAction = 0;
			try {
				systemAction = (uint32_t)std::atol(lineData[0].c_str());
			} catch(std::exception&) {
			}
			_systemActionByFrame.push_back(systemAction);

			//Only supports regular controllers (up to 4 of them)
			for(size_t i = 1; i < lineData.size() && i < 5; i++) {
				if(lineData[i].size() >= 8) {
					string data = lineData[i].substr(3, 1) + lineData[i].substr(2, 1) + lineData[i].substr(1, 1) + lineData[i].substr(0, 1);
					_dataByFrame[i - 1].push_back(data + lineData[i].substr(4, 4));
				} else {
					_dataByFrame[i - 1].push_back("");
				}
			}
		}
	}
	return result;
}

bool FceuxMovie::Play(VirtualFile &file)
{
	_console->Pause();
	
	std::stringstream ss;
	file.ReadFile(ss);
	_console->GetNotificationManager()->RegisterNotificationListener(shared_from_this());
	_console->GetBatteryManager()->SetBatteryProvider(shared_from_this());
	if(InitializeData(ss)) {
		_console->Reset(false);
		_isPlaying = true;
	}

	_console->Resume();
	return _isPlaying;
}