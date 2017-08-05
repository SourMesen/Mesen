#pragma once
#include "stdafx.h"
#include "IMemoryHandler.h"

class Debugger;
class DisassemblyInfo;

class CodeRunner : public IMemoryHandler
{
private:
	vector<uint8_t> _byteCode;
	Debugger *_debugger;
	bool _running;

public:
	static const uint16_t BaseAddress = 0x3000;

	CodeRunner(vector<uint8_t> byteCode, Debugger *debugger);

	bool IsRunning();
	DisassemblyInfo GetDisassemblyInfo(uint16_t cpuAddress);

	void GetMemoryRanges(MemoryRanges &ranges) override;
	uint8_t ReadRAM(uint16_t addr) override;
	void WriteRAM(uint16_t addr, uint8_t value) override;
};