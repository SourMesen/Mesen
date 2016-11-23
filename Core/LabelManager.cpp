#include "stdafx.h"
#include "LabelManager.h"
#include "Debugger.h"

LabelManager::LabelManager(shared_ptr<BaseMapper> mapper)
{
	_mapper = mapper;
}

void LabelManager::SetLabel(uint32_t address, AddressType addressType, string label, string comment)
{
	ExpressionEvaluator::ResetCustomCache();

	switch(addressType) {
		case AddressType::InternalRam: address |= 0x40000000; break;
		case AddressType::PrgRom: address |= 0x20000000; break;
		case AddressType::WorkRam: address |= 0x10000000; break;
		case AddressType::SaveRam: address |= 0x08000000; break;
	}

	auto existingLabel = _codeLabels.find(address);
	if(existingLabel != _codeLabels.end()) {
		_codeLabelReverseLookup.erase(existingLabel->second);
	}

	_codeLabels.erase(address);
	if(!label.empty()) {
		_codeLabels.emplace(address, label);
		_codeLabelReverseLookup.emplace(label, address);
	}

	_codeComments.erase(address);
	if(!comment.empty()) {
		_codeComments.emplace(address, comment);
	}
}

int32_t LabelManager::GetLabelAddress(uint16_t relativeAddr)
{
	if(relativeAddr < 0x2000) {
		return relativeAddr | 0x40000000;
	} else {
		int32_t addr = _mapper->ToAbsoluteAddress(relativeAddr);
		if(addr >= 0) {
			//PRG ROM
			return addr | 0x20000000;
		}

		addr = _mapper->ToAbsoluteWorkRamAddress(relativeAddr);
		if(addr >= 0) {
			//Work RAM
			return addr | 0x10000000;
		}

		addr = _mapper->ToAbsoluteSaveRamAddress(relativeAddr);
		if(addr >= 0) {
			//Save RAM
			return addr | 0x08000000;
		}
	}

	return -1;
}

string LabelManager::GetLabel(uint16_t relativeAddr)
{
	uint32_t labelAddr = GetLabelAddress(relativeAddr);

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
	uint32_t labelAddr = GetLabelAddress(relativeAddr);

	if(labelAddr >= 0) {
		auto result = _codeComments.find(labelAddr);
		if(result != _codeComments.end()) {
			return result->second;
		}
	}

	return "";
}

int32_t LabelManager::GetLabelRelativeAddress(string label)
{
	auto result = _codeLabelReverseLookup.find(label);
	if(result != _codeLabelReverseLookup.end()) {
		return _mapper->FromAbsoluteAddress(result->second);
	}
	return -1;
}