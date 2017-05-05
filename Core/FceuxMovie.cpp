#include "stdafx.h"
#include <algorithm>
#include "../Utilities/HexUtilities.h"
#include "FceuxMovie.h"
#include "Console.h"

vector<uint8_t> FceuxMovie::Base64Decode(string in)
{
	vector<uint8_t> out;

	vector<int> T(256, -1);
	for(int i = 0; i<64; i++) T["ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/"[i]] = i;

	int val = 0, valb = -8;
	for(uint8_t c : in) {
		if(T[c] == -1) break;
		val = (val << 6) + T[c];
		valb += 6;
		if(valb >= 0) {
			out.push_back(val >> valb);
			valb -= 8;
		}
	}
	return out;
}

bool FceuxMovie::InitializeData(stringstream &filestream)
{
	const uint8_t orValues[8] = { 0x80, 0x40, 0x20, 0x10, 0x08, 0x04, 0x02, 0x01 };
	uint32_t systemActionCount = 0;

	bool result = false;
	while(!filestream.eof()) {
		string line;
		std::getline(filestream, line);
		if(line.compare(0, 19, "romChecksum base64:", 19) == 0) {
			vector<uint8_t> md5array = Base64Decode(line.substr(19, line.size() - 20));
			HashInfo hashInfo;
			hashInfo.PrgChrMd5Hash = HexUtilities::ToHex(md5array);
			if(Console::LoadROM("", hashInfo)) {
				result = true;
			} else {
				return false;
			}
		} else if(line.size() > 0 && line[0] == '|') {
			line.erase(std::remove(line.begin(), line.end(), '|'), line.end());
			line = line.substr(1, line.size() - 2);

			//Read power/reset/FDS/VS/etc. commands
			/*uint32_t systemAction = 0;
			for(int i = 0; i < systemActionCount; i++) {
				if(line[i] != '.') {
					systemAction |= (1 << i);
				}
			}
			_systemActionByFrame.push_back(systemAction);*/

			//Only supports regular controllers (up to 4 of them)
			for(int i = 0; i < 8 * 4; i++) {
				uint8_t port = i / 8;

				if(port <= 3) {
					uint8_t portValue = 0;
					for(int j = 0; j < 8 && i + j + systemActionCount < line.size(); j++) {
						if(line[i + j + systemActionCount] != '.') {
							portValue |= orValues[j];
						}
					}
					i += 7;
					_dataByFrame[port].push_back(portValue);
				}
			}
		}
	}
	return result;
}

bool FceuxMovie::Play(stringstream & filestream, bool autoLoadRom)
{
	Console::Pause();
	if(InitializeData(filestream)) {
		EmulationSettings::SetRamPowerOnState(RamPowerOnState::AllZeros);
		Console::Reset(false);
		_isPlaying = true;
	}
	Console::Resume();
	return _isPlaying;
}