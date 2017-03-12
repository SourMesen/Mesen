#include "stdafx.h"
#include "CodeRunner.h"
#include "Debugger.h"
#include "DisassemblyInfo.h"

CodeRunner::CodeRunner(vector<uint8_t> byteCode, Debugger *debugger)
{
	_byteCode = byteCode;
	_debugger = debugger;
	_running = true;

	if(_byteCode.size() < 0x1000) {
		//Fill the entire $3000-$3FFF range
		_byteCode.insert(_byteCode.end(), 0x1000 - _byteCode.size(), 0xEA); //0xEA = NOP
	}
}

bool CodeRunner::IsRunning()
{
	return _running;
}

void CodeRunner::GetMemoryRanges(MemoryRanges & ranges)
{
	ranges.SetAllowOverride();
	ranges.AddHandler(MemoryOperation::Any, CodeRunner::BaseAddress, CodeRunner::BaseAddress + 0xFFF);
}

uint8_t CodeRunner::ReadRAM(uint16_t addr)
{
	return _byteCode[addr - CodeRunner::BaseAddress];
}

void CodeRunner::WriteRAM(uint16_t addr, uint8_t value)
{
	_byteCode[addr - CodeRunner::BaseAddress] = value;

	if(addr == CodeRunner::BaseAddress) {
		//Writing to $3000 stops the code runner and resumes normal execution
		_debugger->StopCodeRunner();
		_running = false;
	}
}

shared_ptr<DisassemblyInfo> CodeRunner::GetDisassemblyInfo(uint16_t cpuAddress)
{
	return shared_ptr<DisassemblyInfo>(new DisassemblyInfo(_byteCode.data() + cpuAddress - CodeRunner::BaseAddress, false));
}