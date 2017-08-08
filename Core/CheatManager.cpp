#include "stdafx.h"

#include "CheatManager.h"
#include "Console.h"
#include "MessageManager.h"

CheatManager* CheatManager::Instance = new CheatManager();

CheatManager::CheatManager()
{
	for(int i = 0; i <= 0xFFFF; i++) {
		_relativeCheatCodes.push_back(nullptr);
	}
}

uint32_t CheatManager::DecodeValue(uint32_t code, uint32_t* bitIndexes, uint32_t bitCount)
{
	uint32_t result = 0;
	for(uint32_t i = 0; i < bitCount; i++) {
		result <<= 1;
		result |= (code >> bitIndexes[i]) & 0x01;
	}
	return result;
}

CodeInfo CheatManager::GetGGCodeInfo(string ggCode)
{
	string ggLetters = "APZLGITYEOXUKSVN";
	
	uint32_t rawCode = 0;
	for(size_t i = 0, len = ggCode.size(); i < len; i++) {
		rawCode |= ggLetters.find(ggCode[i]) << (i * 4);
	}

	CodeInfo code = { 0 };
	code.IsRelativeAddress = true;
	code.CompareValue = -1;
	uint32_t addressBits[15] = { 14, 13, 12, 19, 22, 21, 20, 7, 10, 9, 8, 15, 18, 17, 16 };
	uint32_t valueBits[8] = { 3, 6, 5, 4, 23, 2, 1, 0 };
	if(ggCode.size() == 8) {
		//Bit 5 of the value is stored in a different location for 8-character codes
		valueBits[4] = 31;

		uint32_t compareValueBits[8] = { 27, 30, 29, 28, 23, 26, 25, 24 };
		code.CompareValue = DecodeValue(rawCode, compareValueBits, 8);
	}
	code.Address = DecodeValue(rawCode, addressBits, 15) + 0x8000;
	code.Value = DecodeValue(rawCode, valueBits, 8);

	return code;
}

CodeInfo CheatManager::GetPARCodeInfo(uint32_t parCode)
{
	uint32_t shiftValues[31] = {
		3, 13, 14, 1, 6, 9, 5, 0, 12, 7, 2, 8, 10, 11, 4,	//address
		19, 21, 23, 22, 20, 17, 16, 18,							//compare
		29, 31, 24, 26, 25, 30, 27, 28							//value
	};
	uint32_t key = 0x7E5EE93A;
	uint32_t xorValue = 0x5C184B91;

	//Throw away bit 0, not used.
	parCode >>= 1;

	uint32_t result = 0;
	for(int32_t i = 30; i >= 0; i--) {
		if(((key ^ parCode) >> 30) & 0x01) {
			result |= 0x01 << shiftValues[i];
			key ^= xorValue;
		}
		parCode <<= 1;
		key <<= 1;
	}

	CodeInfo code = { 0 };
	code.IsRelativeAddress = true;
	code.Address = (result & 0x7fff) + 0x8000;
	code.Value = (result >> 24) & 0xFF;
	code.CompareValue = (result >> 16) & 0xFF;
	
	return code;
}

void CheatManager::AddCode(CodeInfo &code)
{
	if(code.IsRelativeAddress) {
		if(code.Address > 0xFFFF) {
			//Invalid cheat, ignore it
			return;
		}

		if(_relativeCheatCodes[code.Address] == nullptr) {
			_relativeCheatCodes[code.Address].reset(new vector<CodeInfo>());
		}
		_relativeCheatCodes[code.Address]->push_back(code);
	} else {
		_absoluteCheatCodes.push_back(code);
	}
	MessageManager::SendNotification(ConsoleNotificationType::CheatAdded);
}

void CheatManager::AddGameGenieCode(string code)
{
	CodeInfo info = GetGGCodeInfo(code);
	AddCode(info);
}

void CheatManager::AddProActionRockyCode(uint32_t code)
{
	CodeInfo info = GetPARCodeInfo(code);
	AddCode(info);
}

void CheatManager::AddCustomCode(uint32_t address, uint8_t value, int32_t compareValue, bool isRelativeAddress)
{
	CodeInfo code;
	code.Address = address;
	code.Value = value;
	code.CompareValue = compareValue;
	code.IsRelativeAddress = isRelativeAddress;

	AddCode(code);
}

void CheatManager::ClearCodes()
{
	bool cheatRemoved = false;

	for(int i = 0; i <= 0xFFFF; i++) {
		if(!_relativeCheatCodes[i]) {
			cheatRemoved = true;
		}
		_relativeCheatCodes[i].reset();
	}

	cheatRemoved |= _absoluteCheatCodes.size() > 0;
	_absoluteCheatCodes.clear();
	
	if(cheatRemoved) {
		MessageManager::SendNotification(ConsoleNotificationType::CheatRemoved);
	}
}

void CheatManager::ApplyRamCodes(uint16_t addr, uint8_t &value)
{
	if(Instance->_relativeCheatCodes[addr] != nullptr) {
		for(uint32_t i = 0, len = i < Instance->_relativeCheatCodes[addr]->size(); i < len; i++) {
			CodeInfo code = Instance->_relativeCheatCodes[addr]->at(i);
			if(code.CompareValue == -1 || code.CompareValue == value) {
				value = code.Value;
				return;
			}
		}
	}
}

void CheatManager::ApplyPrgCodes(uint8_t *prgRam, uint32_t prgSize)
{
	Console::Pause();
	for(uint32_t i = 0, len = i < Instance->_absoluteCheatCodes.size(); i < len; i++) {
		CodeInfo code = Instance->_absoluteCheatCodes[i];
		if(code.Address < prgSize) {
			if(code.CompareValue == -1 || code.CompareValue == prgRam[code.Address]) {
				prgRam[code.Address] = code.Value;
			}
		}
	}
	Console::Resume();
}

vector<CodeInfo> CheatManager::GetCheats()
{
	//Used by NetPlay
	vector<CodeInfo> cheats;
	for(unique_ptr<vector<CodeInfo>> &codes : Instance->_relativeCheatCodes) {
		if(codes) {
			std::copy(codes.get()->begin(), codes.get()->end(), std::back_inserter(cheats));
		}
	}
	std::copy(Instance->_absoluteCheatCodes.begin(), Instance->_absoluteCheatCodes.end(), std::back_inserter(cheats));
	return cheats;
}

void CheatManager::SetCheats(CheatInfo cheats[], uint32_t length)
{
	Console::Pause();

	Instance->ClearCodes();

	for(uint32_t i = 0; i < length; i++) {
		CheatInfo &cheat = cheats[i];
		switch(cheat.Type) {
			case CheatType::Custom: Instance->AddCustomCode(cheat.Address, cheat.Value, cheat.UseCompareValue ? cheat.CompareValue : -1, cheat.IsRelativeAddress); break;
			case CheatType::GameGenie: Instance->AddGameGenieCode(cheat.GameGenieCode);	break;
			case CheatType::ProActionRocky: Instance->AddProActionRockyCode(cheat.ProActionRockyCode); break;
		}
	}

	Console::Resume();
}

void CheatManager::SetCheats(vector<CodeInfo> &cheats)
{
	//Used by NetPlay
	Instance->ClearCodes();

	if(cheats.size() > 0) {
		MessageManager::DisplayMessage("Cheats", cheats.size() > 1 ? "CheatsApplied" : "CheatApplied", std::to_string(cheats.size()));
		for(CodeInfo &cheat : cheats) {
			Instance->AddCode(cheat);
		}
	}
}