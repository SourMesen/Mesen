#include "stdafx.h"
#include "CodeDataLogger.h"
#include "Debugger.h"
#include "LabelManager.h"

CodeDataLogger::CodeDataLogger(Debugger *debugger, uint32_t prgSize, uint32_t chrSize)
{
	_debugger = debugger;
	_prgSize = prgSize;
	_chrSize = chrSize;
	_cdlData = new uint8_t[prgSize+chrSize];
	Reset();
}

CodeDataLogger::~CodeDataLogger()
{
	delete[] _cdlData;
}

void CodeDataLogger::Reset()
{
	_codeSize = 0;
	_dataSize = 0;
	_usedChrSize = 0;
	_drawnChrSize = 0;
	_readChrSize = 0;
	memset(_cdlData, 0, _prgSize + _chrSize);
}

bool CodeDataLogger::LoadCdlFile(string cdlFilepath)
{
	ifstream cdlFile(cdlFilepath, ios::in | ios::binary);
	if(cdlFile) {
		cdlFile.seekg(0, std::ios::end);
		size_t fileSize = (size_t)cdlFile.tellg();
		cdlFile.seekg(0, std::ios::beg);

		if(fileSize == _prgSize + _chrSize) {
			Reset();

			cdlFile.read((char*)_cdlData, _prgSize + _chrSize);
			cdlFile.close();

			for(int i = 0, len = _prgSize; i < len; i++) {
				if(IsCode(i)) {
					_codeSize++;
				} else if(IsData(i)) {
					_dataSize++;
				}
			}

			for(int i = 0, len = _chrSize; i < len; i++) {
				if(IsDrawn(i) || IsRead(i)) {
					_usedChrSize++;
					if(IsDrawn(i)) {
						_drawnChrSize++;
					} else if(IsRead(i)) {
						_readChrSize++;
					}
				}
			}
			return true;
		}
	}
	return false;
}

bool CodeDataLogger::SaveCdlFile(string cdlFilepath)
{
	ofstream cdlFile(cdlFilepath, ios::out | ios::binary);
	if(cdlFile) {
		cdlFile.write((char*)_cdlData, _prgSize+_chrSize);
		cdlFile.close();
		return true;
	}
	return false;
}

void CodeDataLogger::SetFlag(int32_t absoluteAddr, CdlPrgFlags flag)
{
	if(absoluteAddr >= 0 && absoluteAddr < (int32_t)_prgSize) {
		if((_cdlData[absoluteAddr] & (uint8_t)flag) != (uint8_t)flag) {
			if(flag == CdlPrgFlags::Code) {
				if(IsData(absoluteAddr)) {
					//Remove the data flag from bytes that we are flagging as code
					_cdlData[absoluteAddr] &= ~(uint8_t)CdlPrgFlags::Data;
					_dataSize--;
				}
				_cdlData[absoluteAddr] |= (uint8_t)flag;
				_codeSize++;
			} else if(flag == CdlPrgFlags::Data) {
				if(!IsCode(absoluteAddr)) {
					_cdlData[absoluteAddr] |= (uint8_t)flag;
					_dataSize++;
				}
			} else {
				_cdlData[absoluteAddr] |= (uint8_t)flag;
			}
		}
	}
}

void CodeDataLogger::SetFlag(int32_t chrAbsoluteAddr, CdlChrFlags flag)
{
	if(chrAbsoluteAddr >= 0 && chrAbsoluteAddr < (int32_t)_chrSize) {
		if((_cdlData[_prgSize + chrAbsoluteAddr] & (uint8_t)flag) != (uint8_t)flag) {
			_usedChrSize++;
			if(flag == CdlChrFlags::Read) {
				_readChrSize++;
			} else if(flag == CdlChrFlags::Drawn) {
				_drawnChrSize++;
			}
			_cdlData[_prgSize + chrAbsoluteAddr] |= (uint8_t)flag;
		}
	}
}

CdlRatios CodeDataLogger::GetRatios()
{
	CdlRatios ratios;
	ratios.CodeRatio = (float)_codeSize / (float)_prgSize;
	ratios.DataRatio = (float)_dataSize / (float)_prgSize;
	ratios.PrgRatio = (float)(_codeSize + _dataSize) / (float)_prgSize;
	if(_chrSize > 0) {
		ratios.ChrRatio = (float)(_usedChrSize) / (float)_chrSize;
		ratios.ChrReadRatio = (float)(_readChrSize) / (float)_chrSize;
		ratios.ChrDrawnRatio = (float)(_drawnChrSize) / (float)_chrSize;
	} else {
		ratios.ChrRatio = -1;
		ratios.ChrReadRatio = -1;
		ratios.ChrDrawnRatio = -1;
	}

	return ratios;
}

bool CodeDataLogger::IsCode(uint32_t absoluteAddr)
{
	return (_cdlData[absoluteAddr] & (uint8_t)CdlPrgFlags::Code) == (uint8_t)CdlPrgFlags::Code;
}

bool CodeDataLogger::IsSubEntryPoint(uint32_t absoluteAddr)
{
	return (_cdlData[absoluteAddr] & (uint8_t)CdlPrgFlags::SubEntryPoint) == (uint8_t)CdlPrgFlags::SubEntryPoint;
}

bool CodeDataLogger::IsData(uint32_t absoluteAddr)
{
	return (_cdlData[absoluteAddr] & (uint8_t)CdlPrgFlags::Data) == (uint8_t)CdlPrgFlags::Data;
}

bool CodeDataLogger::IsRead(uint32_t absoluteAddr)
{
	return (_cdlData[absoluteAddr + _prgSize] & (uint8_t)CdlChrFlags::Read) == (uint8_t)CdlChrFlags::Read;
}

bool CodeDataLogger::IsDrawn(uint32_t absoluteAddr)
{
	return (_cdlData[absoluteAddr + _prgSize] & (uint8_t)CdlChrFlags::Drawn) == (uint8_t)CdlChrFlags::Drawn;
}

void CodeDataLogger::GetCdlData(uint32_t offset, uint32_t length, DebugMemoryType memoryType, uint8_t * cdlData)
{
	if(memoryType == DebugMemoryType::PrgRom) {
		for(uint32_t i = 0; i < length; i++) {
			cdlData[i] = _cdlData[offset + i];
			if(_debugger->GetLabelManager()->HasLabelOrComment(offset + i, AddressType::PrgRom)) {
				cdlData[i] |= 0x04; //Has label or comment
			}
		}
	} else if(memoryType == DebugMemoryType::ChrRom) {
		for(uint32_t i = 0; i < length; i++) {
			cdlData[i] = _cdlData[_prgSize + offset + i];
		}
	} else if(memoryType == DebugMemoryType::CpuMemory) {
		for(uint32_t i = 0; i < length; i++) {
			int32_t absoluteAddress = _debugger->GetAbsoluteAddress(offset + i);
			cdlData[i] = absoluteAddress >= 0 ? _cdlData[absoluteAddress] : 0;
			if(_debugger->GetLabelManager()->HasLabelOrComment(offset + i)) {
				cdlData[i] |= 0x04; //Has label or comment
			}
		}
	} else if(memoryType == DebugMemoryType::PpuMemory) {
		for(uint32_t i = 0; i < length; i++) {
			int32_t absoluteAddress = _debugger->GetAbsoluteChrAddress(offset + i);
			cdlData[i] = absoluteAddress >= 0 ? _cdlData[_prgSize + absoluteAddress] : 0;
		}
	}
}
