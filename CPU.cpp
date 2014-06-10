#include "stdafx.h"
#include "CPU.h"
#include "Timer.h"

void Core::Reset()
{
	_state.A = 0;
	_state.PC = 0x0400;
	_state.SP = 0xFF;
	_state.X = 0;
	_state.Y = 0;
	_state.PS = PSFlags::Zero | PSFlags::Reserved;// | PSFlags::Interrupt;
}

void Core::Exec()
{
	uint16_t lastPC = 65535;
	int executedCount = 0;
	std::list<int> pcList;

	Timer timer;
	while(true) {
		_currentPC = _state.PC;
		uint8_t opCode = ReadByte();
		if(_opTable[opCode] != 0) {
			(this->*_opTable[opCode])();
		} else {
			std::cout << "Invalid opcode: PC:" << _currentPC << std::endl;
		}
		lastPC = _currentPC;
		executedCount++;

		if(executedCount >= 200000000) {
			break;
		}
	}

	std::cout << "Executed:" << executedCount << " in " << timer.GetElapsedMS() << " ms";
}

