#include "stdafx.h"
#include "CPU.h"

CPU* CPU::Instance = nullptr;

CPU::CPU(MemoryManager *memoryManager) : _memoryManager(memoryManager)
{
	CPU::Instance = this;

	Func opTable[] = { 
	//	0				1				2				3				4				5				6						7				8				9				A						B				C						D						E						F
		&CPU::BRK,	&CPU::ORA,	nullptr,		nullptr,		&CPU::NOP,	&CPU::ORA,	&CPU::ASL_Memory,	nullptr,		&CPU::PHP,	&CPU::ORA,	&CPU::ASL_Acc,		nullptr,		&CPU::NOP,			&CPU::ORA,	&CPU::ASL_Memory,		nullptr, //0
		&CPU::BPL,	&CPU::ORA,	nullptr,		nullptr,		&CPU::NOP,	&CPU::ORA,	&CPU::ASL_Memory,	nullptr,		&CPU::CLC,	&CPU::ORA,	nullptr,				nullptr,		&CPU::NOP,			&CPU::ORA,	&CPU::ASL_Memory,		nullptr, //1
		&CPU::JSR,	&CPU::AND,	nullptr,		nullptr,		&CPU::BIT,	&CPU::AND,	&CPU::ROL_Memory,	nullptr,		&CPU::PLP,	&CPU::AND,	&CPU::ROL_Acc,		nullptr,		&CPU::BIT,			&CPU::AND,	&CPU::ROL_Memory,		nullptr, //2
		&CPU::BMI,	&CPU::AND,	nullptr,		nullptr,		&CPU::NOP,	&CPU::AND,	&CPU::ROL_Memory,	nullptr,		&CPU::SEC,	&CPU::AND,	nullptr,				nullptr,		&CPU::NOP,			&CPU::AND,	&CPU::ROL_Memory,		nullptr, //3
		&CPU::RTI,	&CPU::EOR,	nullptr,		nullptr,		&CPU::NOP,	&CPU::EOR,	&CPU::LSR_Memory,	nullptr,		&CPU::PHA,	&CPU::EOR,	&CPU::LSR_Acc,		nullptr,		&CPU::JMP_Abs,		&CPU::EOR,	&CPU::LSR_Memory,		nullptr, //4
		&CPU::BVC,	&CPU::EOR,	nullptr,		nullptr,		&CPU::NOP,	&CPU::EOR,	&CPU::LSR_Memory,	nullptr,		&CPU::CLI,	&CPU::EOR,	nullptr,				nullptr,		&CPU::NOP,			&CPU::EOR,	&CPU::LSR_Memory,		nullptr, //5
		&CPU::RTS,	&CPU::ADC,	nullptr,		nullptr,		&CPU::NOP,	&CPU::ADC,	&CPU::ROR_Memory,	nullptr,		&CPU::PLA,	&CPU::ADC,	&CPU::ROR_Acc,		nullptr,		&CPU::JMP_Ind,		&CPU::ADC,	&CPU::ROR_Memory,		nullptr, //6
		&CPU::BVS,	&CPU::ADC,	nullptr,		nullptr,		&CPU::NOP,	&CPU::ADC,	&CPU::ROR_Memory,	nullptr,		&CPU::SEI,	&CPU::ADC,	nullptr,				nullptr,		&CPU::NOP,			&CPU::ADC,	&CPU::ROR_Memory,		nullptr, //7
		&CPU::NOP,	&CPU::STA,	&CPU::NOP,	nullptr,		&CPU::STY,	&CPU::STA,	&CPU::STX,			nullptr,		&CPU::DEY,	&CPU::NOP,	&CPU::TXA,			nullptr,		&CPU::STY,			&CPU::STA,	&CPU::STX,				nullptr, //8
		&CPU::BCC,	&CPU::STA,	nullptr,		nullptr,		&CPU::STY,	&CPU::STA,	&CPU::STX,			nullptr,		&CPU::TYA,	&CPU::STA,	&CPU::TXS,			nullptr,		nullptr,				&CPU::STA,	nullptr,					nullptr, //9
		&CPU::LDY,	&CPU::LDA,	&CPU::LDX,	nullptr,		&CPU::LDY,	&CPU::LDA,	&CPU::LDX,			nullptr,		&CPU::TAY,	&CPU::LDA,	&CPU::TAX,			nullptr,		&CPU::LDY,			&CPU::LDA,	&CPU::LDX,				nullptr, //A
		&CPU::BCS,	&CPU::LDA,	nullptr,		nullptr,		&CPU::LDY,	&CPU::LDA,	&CPU::LDX,			nullptr,		&CPU::CLV,	&CPU::LDA,	&CPU::TSX,			nullptr,		&CPU::LDY,			&CPU::LDA,	&CPU::LDX,				nullptr, //B
		&CPU::CPY,	&CPU::CPA,	&CPU::NOP,	nullptr,		&CPU::CPY,	&CPU::CPA,	&CPU::DEC,			nullptr,		&CPU::INY,	&CPU::CPA,	&CPU::DEX,			nullptr,		&CPU::CPY,			&CPU::CPA,	&CPU::DEC,				nullptr, //C
		&CPU::BNE,	&CPU::CPA,	nullptr,		nullptr,		&CPU::NOP,	&CPU::CPA,	&CPU::DEC,			nullptr,		&CPU::CLD,	&CPU::CPA,	nullptr,				nullptr,		&CPU::NOP,			&CPU::CPA,	&CPU::DEC,				nullptr, //D
		&CPU::CPX,	&CPU::SBC,	&CPU::NOP,	nullptr,		&CPU::CPX,	&CPU::SBC,	&CPU::INC,			nullptr,		&CPU::INX,	&CPU::SBC,	&CPU::NOP,			nullptr,		&CPU::CPX,			&CPU::SBC,	&CPU::INC,				nullptr, //E
		&CPU::BEQ,	&CPU::SBC,	nullptr,		nullptr,		&CPU::NOP,	&CPU::SBC,	&CPU::INC,			nullptr,		&CPU::SED,	&CPU::SBC,	nullptr,				nullptr,		&CPU::NOP,			&CPU::SBC,	&CPU::INC,				nullptr  //F
	};

	AddrMode addrMode[] = {
		Imm,  IndX,  None, IndX,  Zero,  Zero,  Zero,  Zero,  Imp, Imm,   Imp, Imm,   Abs,   Abs,   Abs,   Abs,
		Rel,  IndY,  None, IndYW, ZeroX, ZeroX, ZeroX, ZeroX, Imp, AbsY,  Imp, AbsYW, AbsX,  AbsX,  AbsXW, AbsXW,
		Abs, IndX,  None, IndX,  Zero,  Zero,  Zero,  Zero,  Imp, Imm,   Imp, Imm,   Abs,   Abs,   Abs,   Abs,
		Rel,  IndY,  None, IndYW, ZeroX, ZeroX, ZeroX, ZeroX, Imp, AbsY,  Imp, AbsYW, AbsX,  AbsX,  AbsXW, AbsXW,
		Imp,  IndX,  None, IndX,  Zero,  Zero,  Zero,  Zero,  Imp, Imm,   Imp, Imm,   Abs,   Abs,   Abs,   Abs,
		Rel,  IndY,  None, IndYW, ZeroX, ZeroX, ZeroX, ZeroX, Imp, AbsY,  Imp, AbsYW, AbsX,  AbsX,  AbsXW, AbsXW,
		Imp,  IndX,  None, IndX,  Zero,  Zero,  Zero,  Zero,  Imp, Imm,   Imp, Imm,   Abs,   Abs,   Abs,   Abs,
		Rel,  IndY,  None, IndYW, ZeroX, ZeroX, ZeroX, ZeroX, Imp, AbsY,  Imp, AbsYW, AbsX,  AbsX,  AbsXW, AbsXW,
		Imm,  IndX,  Imm,  IndX,  Zero,  Zero,  Zero,  Zero,  Imp, Imm,   Imp, Imm,   Abs,   Abs,   Abs,   Abs,
		Rel,  IndYW, None, IndY,  ZeroX, ZeroX, ZeroY, ZeroY, Imp, AbsYW, Imp, AbsY,  AbsXW, AbsXW, AbsYW, AbsY,
		Imm,  IndX,  Imm,  IndX,  Zero,  Zero,  Zero,  Zero,  Imp, Imm,   Imp, Imm,   Abs,   Abs,   Abs,   Abs,
		Rel,  IndY,  None, IndY,  ZeroX, ZeroX, ZeroY, ZeroY, Imp, AbsY,  Imp, AbsY,  AbsX,  AbsX,  AbsY,  AbsY,
		Imm,  IndX,  Imm,  IndX,  Zero,  Zero,  Zero,  Zero,  Imp, Imm,   Imp, Imm,   Abs,   Abs,   Abs,   Abs,
		Rel,  IndY,  None, IndYW, ZeroX, ZeroX, ZeroX, ZeroX, Imp, AbsY,  Imp, AbsYW, AbsX,  AbsX,  AbsXW, AbsXW,
		Imm,  IndX,  Imm,  IndX,  Zero,  Zero,  Zero,  Zero,  Imp, Imm,   Imp, Imm,   Abs,   Abs,   Abs,   Abs,
		Rel,  IndY,  None, IndYW, ZeroX, ZeroX, ZeroX, ZeroX, Imp, AbsY,  Imp, AbsYW, AbsX,  AbsX,  AbsXW, AbsXW,
	};

	uint8_t cycles[] {
		7, 6, 2, 8, 3, 3, 5, 5, 3, 2, 2, 2, 4, 4, 6, 6,
		2, 5, 2, 8, 4, 4, 6, 6, 2, 4, 2, 7, 4, 4, 7, 7,
		6, 6, 2, 8, 3, 3, 5, 5, 4, 2, 2, 2, 4, 4, 6, 6,
		2, 5, 2, 8, 4, 4, 6, 6, 2, 4, 2, 7, 4, 4, 7, 7,
		6, 6, 2, 8, 3, 3, 5, 5, 3, 2, 2, 2, 3, 4, 6, 6,
		2, 5, 2, 8, 4, 4, 6, 6, 2, 4, 2, 7, 4, 4, 7, 7,
		6, 6, 2, 8, 3, 3, 5, 5, 4, 2, 2, 2, 5, 4, 6, 6,
		2, 5, 2, 8, 4, 4, 6, 6, 2, 4, 2, 7, 4, 4, 7, 7,
		2, 6, 2, 6, 3, 3, 3, 3, 2, 2, 2, 2, 4, 4, 4, 4,
		2, 6, 2, 6, 4, 4, 4, 4, 2, 5, 2, 5, 5, 5, 5, 5,
		2, 6, 2, 6, 3, 3, 3, 3, 2, 2, 2, 2, 4, 4, 4, 4,
		2, 5, 2, 5, 4, 4, 4, 4, 2, 4, 2, 4, 4, 4, 4, 4,
		2, 6, 2, 8, 3, 3, 5, 5, 2, 2, 2, 2, 4, 4, 6, 6,
		2, 5, 2, 8, 4, 4, 6, 6, 2, 4, 2, 7, 4, 4, 7, 7,
		2, 6, 3, 8, 3, 3, 5, 5, 2, 2, 2, 2, 4, 4, 6, 6,
		2, 5, 2, 8, 4, 4, 6, 6, 2, 4, 2, 7, 4, 4, 7, 7,
	};

	uint8_t cyclesPageCrossed[] {
		7, 6, 2, 8, 3, 3, 5, 5, 3, 2, 2, 2, 4, 4, 6, 6,
		3, 6, 2, 8, 4, 4, 6, 6, 2, 5, 2, 7, 5, 5, 7, 7,
		6, 6, 2, 8, 3, 3, 5, 5, 4, 2, 2, 2, 4, 4, 6, 6,
		3, 6, 2, 8, 4, 4, 6, 6, 2, 5, 2, 7, 5, 5, 7, 7,
		6, 6, 2, 8, 3, 3, 5, 5, 3, 2, 2, 2, 3, 4, 6, 6,
		3, 6, 2, 8, 4, 4, 6, 6, 2, 5, 2, 7, 5, 5, 7, 7,
		6, 6, 2, 8, 3, 3, 5, 5, 4, 2, 2, 2, 5, 4, 6, 6,
		3, 6, 2, 8, 4, 4, 6, 6, 2, 5, 2, 7, 5, 5, 7, 7,
		2, 6, 2, 6, 3, 3, 3, 3, 2, 2, 2, 2, 4, 4, 4, 4,
		3, 6, 2, 6, 4, 4, 4, 4, 2, 5, 2, 5, 5, 5, 5, 5,
		2, 6, 2, 6, 3, 3, 3, 3, 2, 2, 2, 2, 4, 4, 4, 4,
		3, 6, 2, 5, 4, 4, 4, 4, 2, 5, 2, 5, 5, 5, 5, 5,
		2, 6, 2, 8, 3, 3, 5, 5, 2, 2, 2, 2, 4, 4, 6, 6,
		3, 6, 2, 8, 4, 4, 6, 6, 2, 5, 2, 7, 5, 5, 7, 7,
		2, 6, 3, 8, 3, 3, 5, 5, 2, 2, 2, 2, 4, 4, 6, 6,
		3, 6, 2, 8, 4, 4, 6, 6, 2, 5, 2, 7, 5, 5, 7, 7,
	};

	memcpy(_opTable, opTable, sizeof(opTable));
	memcpy(_cycles, cycles, sizeof(cycles));
	memcpy(_addrMode, addrMode, sizeof(addrMode));
	memcpy(_cyclesPageCrossed, cyclesPageCrossed, sizeof(cyclesPageCrossed));
}

void CPU::Reset(bool softReset)
{
	_state.NMIFlag = false;
	_state.IRQFlag = 0;
	_cycleCount = 0;
	_relativeCycleCount = 0;
	_cyclePenalty = 0;

	_state.PC = MemoryReadWord(CPU::ResetVector);
	if(softReset) {
		SetFlags(PSFlags::Interrupt);
		_state.SP -= 0x03;
	} else {
		_state.A = 0;
		_state.SP = 0xFD;
		_state.X = 0;
		_state.Y = 0;
		_state.PS = PSFlags::Reserved | PSFlags::Interrupt;

		_runIRQ = false;
		_runNMI = false;
	}
}

uint32_t CPU::Exec()
{
	//static ofstream log("log.txt", ios::out | ios::binary);
	uint32_t executedCycles = 0;
	if(!_runNMI && !_runIRQ) {
		uint8_t opCode = GetOPCode();

		if(_state.NMIFlag) {
			_runNMI = true;
		} else if(opCode != 0x40 && _state.IRQFlag > 0 && !CheckFlag(PSFlags::Interrupt)) {
			_runIRQ = true;
		}

		_instAddrMode = _addrMode[opCode];
		if(_opTable[opCode] != nullptr) {
			//std::cout << std::hex << (_state.PC - 1) << ": " << (short)opCode << std::endl;
			(this->*_opTable[opCode])();
			executedCycles = (IsPageCrossed() ? _cyclesPageCrossed[opCode] : _cycles[opCode]);
		} else {
			GetOperandAddr();
			std::cout << "Invalid opcode: " << std::hex << (short)opCode;
			//throw exception("Invalid opcode");
		}

		if(!_runIRQ && opCode == 0x40 && _state.IRQFlag > 0 && !CheckFlag(PSFlags::Interrupt)) {
			//"If an IRQ is pending and an RTI is executed that clears the I flag, the CPU will invoke the IRQ handler immediately after RTI finishes executing."
			_runIRQ = true;
		}
	} else {
		if(_runNMI) {
			NMI();
			_runNMI = false;
			_state.NMIFlag = false;
		} else if(_runIRQ) {
			IRQ();
		}
		_runIRQ = false;

		executedCycles = 7;
	}
	
	_cycleCount += executedCycles;
	return executedCycles + GetCyclePenalty();
}

void CPU::EndFrame()
{
	_relativeCycleCount += _cycleCount;
	_cycleCount = 0;
}

void CPU::StreamState(bool saving)
{
	Stream<uint16_t>(_state.PC);
	Stream<uint8_t>(_state.SP);
	Stream<uint8_t>(_state.PS);
	Stream<uint8_t>(_state.A);
	Stream<uint8_t>(_state.X);
	Stream<uint8_t>(_state.Y);
		
	Stream<int32_t>(_cycleCount);
	Stream<bool>(_state.NMIFlag);
	Stream<uint32_t>(_state.IRQFlag);

	Stream<bool>(_runNMI);
	Stream<bool>(_runIRQ);

	Stream<int32_t>(_relativeCycleCount);
}