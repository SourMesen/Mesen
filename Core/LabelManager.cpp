#include "stdafx.h"
#include "LabelManager.h"
#include "Debugger.h"

LabelManager::LabelManager(shared_ptr<BaseMapper> mapper)
{
	_mapper = mapper;
}

void LabelManager::SetLabel(uint32_t address, AddressType addressType, string label, string comment)
{
	switch(addressType) {
		case AddressType::InternalRam: address |= 0x70000000; break;
		case AddressType::PrgRom: address |= 0x60000000; break;
		case AddressType::WorkRam: address |= 0x50000000; break;
		case AddressType::SaveRam: address |= 0x40000000; break;
		case AddressType::Register: address |= 0x30000000; break;
	}

	auto existingLabel = _codeLabels.find(address);
	if(existingLabel != _codeLabels.end()) {
		_codeLabelReverseLookup.erase(existingLabel->second);
	}

	_codeLabels.erase(address);
	if(!label.empty()) {
		if(label.size() > 400) {
			//Restrict labels to 400 bytes
			label = label.substr(0, 400);
		}
		_codeLabels.emplace(address, label);
		_codeLabelReverseLookup.emplace(label, address);
	}

	_codeComments.erase(address);
	if(!comment.empty()) {
		_codeComments.emplace(address, comment);
	}
}

int32_t LabelManager::GetLabelAddress(uint16_t relativeAddr, bool checkRegisters)
{
	if(relativeAddr < 0x2000) {
		return relativeAddr | 0x70000000;
	} else {
		int32_t addr = _mapper->ToAbsoluteAddress(relativeAddr);
		if(addr >= 0) {
			//PRG ROM
			return addr | 0x60000000;
		}

		addr = _mapper->ToAbsoluteWorkRamAddress(relativeAddr);
		if(addr >= 0) {
			//Work RAM
			return addr | 0x50000000;
		}

		addr = _mapper->ToAbsoluteSaveRamAddress(relativeAddr);
		if(addr >= 0) {
			//Save RAM
			return addr | 0x40000000;
		}

		//Register
		if(checkRegisters) {
			return relativeAddr | 0x30000000;
		}
	}

	return -1;
}

string LabelManager::GetLabel(uint16_t relativeAddr, bool checkRegisters)
{
	int32_t labelAddr = GetLabelAddress(relativeAddr, checkRegisters);

	if(labelAddr >= 0) {
		auto result = _codeLabels.find(labelAddr);
		if(result != _codeLabels.end()) {
			return result->second;
		}
	}

	return "";
}

string LabelManager::GetComment(uint16_t relativeAddr)
{
	int32_t labelAddr = GetLabelAddress(relativeAddr, false);

	if(labelAddr >= 0) {
		auto result = _codeComments.find(labelAddr);
		if(result != _codeComments.end()) {
			return result->second;
		}
	}

	return "";
}

void LabelManager::GetLabelAndComment(uint16_t relativeAddr, string &label, string &comment)
{
	int32_t labelAddr = GetLabelAddress(relativeAddr, false);

	if(labelAddr >= 0) {
		auto result = _codeLabels.find(labelAddr);
		if(result != _codeLabels.end()) {
			label = result->second;
		} else {
			label.clear();
		}

		auto commentResult = _codeComments.find(labelAddr);
		if(commentResult != _codeComments.end()) {
			comment = commentResult->second;
		} else {
			comment.clear();
		}
	}
}

int32_t LabelManager::GetLabelRelativeAddress(string label)
{
	auto result = _codeLabelReverseLookup.find(label);
	if(result != _codeLabelReverseLookup.end()) {
		uint32_t address = result->second;
		AddressType type = AddressType::InternalRam;
		if((address & 0x70000000) == 0x70000000) {
			type = AddressType::InternalRam;
		} else if((address & 0x60000000) == 0x60000000) {
			type = AddressType::PrgRom;
		} else if((address & 0x50000000) == 0x50000000) {
			type = AddressType::WorkRam;
		} else if((address & 0x40000000) == 0x40000000) {
			type = AddressType::SaveRam;
		} else if((address & 0x30000000) == 0x30000000) {
			type = AddressType::Register;
		} else {
			return -1;
		}
		return _mapper->FromAbsoluteAddress(address & 0x0FFFFFFF, type);
	}
	return -1;
}