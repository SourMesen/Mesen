#include "stdafx.h"
#include "CPU.h"
#include "PPU.h"
#include "APU.h"
#include "DeltaModulationChannel.h"

CPU* CPU::Instance = nullptr;

CPU::CPU(MemoryManager *memoryManager) : _memoryManager(memoryManager)
{
	CPU::Instance = this;

	Func opTable[] = { 
	//	0				1				2				3				4				5				6						7				8				9				A						B				C						D				E						F
		&CPU::BRK,	&CPU::ORA,	&CPU::HLT,	&CPU::SLO,	&CPU::NOP,	&CPU::ORA,	&CPU::ASL_Memory,	&CPU::SLO,	&CPU::PHP,	&CPU::ORA,	&CPU::ASL_Acc,		&CPU::AAC,	&CPU::NOP,			&CPU::ORA,	&CPU::ASL_Memory,	&CPU::SLO, //0
		&CPU::BPL,	&CPU::ORA,	&CPU::HLT,	&CPU::SLO,	&CPU::NOP,	&CPU::ORA,	&CPU::ASL_Memory,	&CPU::SLO,	&CPU::CLC,	&CPU::ORA,	&CPU::NOP,			&CPU::SLO,	&CPU::NOP,			&CPU::ORA,	&CPU::ASL_Memory,	&CPU::SLO, //1
		&CPU::JSR,	&CPU::AND,	&CPU::HLT,	&CPU::RLA,	&CPU::BIT,	&CPU::AND,	&CPU::ROL_Memory,	&CPU::RLA,	&CPU::PLP,	&CPU::AND,	&CPU::ROL_Acc,		&CPU::AAC,	&CPU::BIT,			&CPU::AND,	&CPU::ROL_Memory,	&CPU::RLA, //2
		&CPU::BMI,	&CPU::AND,	&CPU::HLT,	&CPU::RLA,	&CPU::NOP,	&CPU::AND,	&CPU::ROL_Memory,	&CPU::RLA,	&CPU::SEC,	&CPU::AND,	&CPU::NOP,			&CPU::RLA,	&CPU::NOP,			&CPU::AND,	&CPU::ROL_Memory,	&CPU::RLA, //3
		&CPU::RTI,	&CPU::EOR,	&CPU::HLT,	&CPU::SRE,	&CPU::NOP,	&CPU::EOR,	&CPU::LSR_Memory,	&CPU::SRE,	&CPU::PHA,	&CPU::EOR,	&CPU::LSR_Acc,		&CPU::ASR,	&CPU::JMP_Abs,		&CPU::EOR,	&CPU::LSR_Memory,	&CPU::SRE, //4
		&CPU::BVC,	&CPU::EOR,	&CPU::HLT,	&CPU::SRE,	&CPU::NOP,	&CPU::EOR,	&CPU::LSR_Memory,	&CPU::SRE,	&CPU::CLI,	&CPU::EOR,	&CPU::NOP,			&CPU::SRE,	&CPU::NOP,			&CPU::EOR,	&CPU::LSR_Memory,	&CPU::SRE, //5
		&CPU::RTS,	&CPU::ADC,	&CPU::HLT,	&CPU::RRA,	&CPU::NOP,	&CPU::ADC,	&CPU::ROR_Memory,	&CPU::RRA,	&CPU::PLA,	&CPU::ADC,	&CPU::ROR_Acc,		&CPU::ARR,	&CPU::JMP_Ind,		&CPU::ADC,	&CPU::ROR_Memory,	&CPU::RRA, //6
		&CPU::BVS,	&CPU::ADC,	&CPU::HLT,	&CPU::RRA,	&CPU::NOP,	&CPU::ADC,	&CPU::ROR_Memory,	&CPU::RRA,	&CPU::SEI,	&CPU::ADC,	&CPU::NOP,			&CPU::RRA,	&CPU::NOP,			&CPU::ADC,	&CPU::ROR_Memory,	&CPU::RRA, //7
		&CPU::NOP,	&CPU::STA,	&CPU::NOP,	&CPU::SAX,	&CPU::STY,	&CPU::STA,	&CPU::STX,			&CPU::SAX,	&CPU::DEY,	&CPU::NOP,	&CPU::TXA,			&CPU::UNK,	&CPU::STY,			&CPU::STA,	&CPU::STX,			&CPU::SAX, //8
		&CPU::BCC,	&CPU::STA,	&CPU::HLT,	&CPU::AXA,	&CPU::STY,	&CPU::STA,	&CPU::STX,			&CPU::SAX,	&CPU::TYA,	&CPU::STA,	&CPU::TXS,			&CPU::TAS,	&CPU::NOP,			&CPU::STA,	&CPU::NOP,			&CPU::AXA, //9
		&CPU::LDY,	&CPU::LDA,	&CPU::LDX,	&CPU::LAX,	&CPU::LDY,	&CPU::LDA,	&CPU::LDX,			&CPU::LAX,	&CPU::TAY,	&CPU::LDA,	&CPU::TAX,			&CPU::ATX,	&CPU::LDY,			&CPU::LDA,	&CPU::LDX,			&CPU::LAX, //A
		&CPU::BCS,	&CPU::LDA,	&CPU::HLT,	&CPU::LAX,	&CPU::LDY,	&CPU::LDA,	&CPU::LDX,			&CPU::LAX,	&CPU::CLV,	&CPU::LDA,	&CPU::TSX,			&CPU::LAS,	&CPU::LDY,			&CPU::LDA,	&CPU::LDX,			&CPU::LAX, //B
		&CPU::CPY,	&CPU::CPA,	&CPU::NOP,	&CPU::DCP,	&CPU::CPY,	&CPU::CPA,	&CPU::DEC,			&CPU::DCP,	&CPU::INY,	&CPU::CPA,	&CPU::DEX,			&CPU::AXS,	&CPU::CPY,			&CPU::CPA,	&CPU::DEC,			&CPU::DCP, //C
		&CPU::BNE,	&CPU::CPA,	&CPU::HLT,	&CPU::DCP,	&CPU::NOP,	&CPU::CPA,	&CPU::DEC,			&CPU::DCP,	&CPU::CLD,	&CPU::CPA,	&CPU::NOP,			&CPU::DCP,	&CPU::NOP,			&CPU::CPA,	&CPU::DEC,			&CPU::DCP, //D
		&CPU::CPX,	&CPU::SBC,	&CPU::NOP,	&CPU::ISB,	&CPU::CPX,	&CPU::SBC,	&CPU::INC,			&CPU::ISB,	&CPU::INX,	&CPU::SBC,	&CPU::NOP,			&CPU::SBC,	&CPU::CPX,			&CPU::SBC,	&CPU::INC,			&CPU::ISB, //E
		&CPU::BEQ,	&CPU::SBC,	&CPU::HLT,	&CPU::ISB,	&CPU::NOP,	&CPU::SBC,	&CPU::INC,			&CPU::ISB,	&CPU::SED,	&CPU::SBC,	&CPU::NOP,			&CPU::ISB,	&CPU::NOP,			&CPU::SBC,	&CPU::INC,			&CPU::ISB  //F
	};

	AddrMode addrMode[] = {
	//	0		1		 2		 3		  4		5		 6		  7		8	  9		A	  B		C		 D		  E		F
		Imp,  IndX,  None, IndX,  Zero,  Zero,  Zero,  Zero,  Imp, Imm,   Imp, Imm,   Abs,   Abs,   Abs,   Abs,   //0
		Rel,  IndY,  None, IndYW, ZeroX, ZeroX, ZeroX, ZeroX, Imp, AbsY,  Imp, AbsYW, AbsX,  AbsX,  AbsXW, AbsXW, //1
		Abs,	IndX,  None, IndX,  Zero,  Zero,  Zero,  Zero,  Imp, Imm,   Imp, Imm,   Abs,   Abs,   Abs,   Abs,   //2
		Rel,  IndY,  None, IndYW, ZeroX, ZeroX, ZeroX, ZeroX, Imp, AbsY,  Imp, AbsYW, AbsX,  AbsX,  AbsXW, AbsXW, //3
		Imp,  IndX,  None, IndX,  Zero,  Zero,  Zero,  Zero,  Imp, Imm,   Imp, Imm,   Abs,   Abs,   Abs,   Abs,   //4
		Rel,  IndY,  None, IndYW, ZeroX, ZeroX, ZeroX, ZeroX, Imp, AbsY,  Imp, AbsYW, AbsX,  AbsX,  AbsXW, AbsXW, //5
		Imp,  IndX,  None, IndX,  Zero,  Zero,  Zero,  Zero,  Imp, Imm,   Imp, Imm,   Abs,   Abs,   Abs,   Abs,   //6
		Rel,  IndY,  None, IndYW, ZeroX, ZeroX, ZeroX, ZeroX, Imp, AbsY,  Imp, AbsYW, AbsX,  AbsX,  AbsXW, AbsXW, //7
		Imm,  IndX,  Imm,  IndX,  Zero,  Zero,  Zero,  Zero,  Imp, Imm,   Imp, Imm,   Abs,   Abs,   Abs,   Abs,   //8
		Rel,  IndYW, None, IndYW, ZeroX, ZeroX, ZeroY, ZeroY, Imp, AbsYW, Imp, AbsYW, AbsXW, AbsXW, AbsYW, AbsYW, //9
		Imm,  IndX,  Imm,  IndX,  Zero,  Zero,  Zero,  Zero,  Imp, Imm,   Imp, Imm,   Abs,   Abs,   Abs,   Abs,   //A
		Rel,  IndY,  None, IndY,  ZeroX, ZeroX, ZeroY, ZeroY, Imp, AbsY,  Imp, AbsY,  AbsX,  AbsX,  AbsY,  AbsY,  //B
		Imm,  IndX,  Imm,  IndX,  Zero,  Zero,  Zero,  Zero,  Imp, Imm,   Imp, Imm,   Abs,   Abs,   Abs,   Abs,   //C
		Rel,  IndY,  None, IndYW, ZeroX, ZeroX, ZeroX, ZeroX, Imp, AbsY,  Imp, AbsYW, AbsX,  AbsX,  AbsXW, AbsXW, //D
		Imm,  IndX,  Imm,  IndX,  Zero,  Zero,  Zero,  Zero,  Imp, Imm,   Imp, Imm,   Abs,   Abs,   Abs,   Abs,   //E
		Rel,  IndY,  None, IndYW, ZeroX, ZeroX, ZeroX, ZeroX, Imp, AbsY,  Imp, AbsYW, AbsX,  AbsX,  AbsXW, AbsXW, //F
	};
	
	memcpy(_opTable, opTable, sizeof(opTable));
	memcpy(_addrMode, addrMode, sizeof(addrMode));
}

void CPU::Reset(bool softReset)
{
	_state.NMIFlag = false;
	_state.IRQFlag = 0;
	_cycleCount = 0;

	_spriteDmaTransfer = false;
	_spriteDmaCounter = 0;

	_dmcCounter = -1;
	_dmcDmaRunning = false;

	//Use _memoryManager->Read() directly to prevent clocking the PPU/APU when setting PC at reset
	_state.PC = _memoryManager->Read(CPU::ResetVector) | _memoryManager->Read(CPU::ResetVector+1) << 8;

	if(softReset) {
		SetFlags(PSFlags::Interrupt);
		_state.SP -= 0x03;
	} else {
		_state.A = 0;
		_state.SP = 0xFD;
		_state.X = 0;
		_state.Y = 0;
		_state.PS = PSFlags::Reserved | PSFlags::Interrupt;

		_runIrq = false;
	}
}

void CPU::Exec()
{
	uint8_t opCode = GetOPCode();
	_instAddrMode = _addrMode[opCode];
	_operand = FetchOperand();
	(this->*_opTable[opCode])();
	
	if(_prevRunIrq) {
		IRQ();
	}
}

void CPU::IncCycleCount()
{
	if(_dmcDmaRunning) {
		//CPU is being stalled by the DMC's DMA transfer
		_dmcCounter--;
		if(_dmcCounter == 0) {
			//Update the DMC buffer when the stall period is completed
			_dmcDmaRunning = false;
			DeltaModulationChannel::SetReadBuffer();
		}
	}

	PPU::ExecStatic();
	APU::ExecStatic();
	
	if(!_spriteDmaTransfer) {
		//IRQ flags are ignored during Sprite DMA - fixes irq_and_dma

		//"it's really the status of the interrupt lines at the end of the second-to-last cycle that matters."
		//Keep the irq lines values from the previous cycle.  The before-to-last cycle's values will be used
		_prevRunIrq = _runIrq;
		_runIrq = _state.NMIFlag || (_state.IRQFlag > 0 && !CheckFlag(PSFlags::Interrupt));
	}

	_cycleCount++;
}

void CPU::RunDMATransfer(uint8_t* spriteRAM, uint8_t offsetValue)
{
	Instance->_spriteDmaTransfer = true;
	
	//"The CPU is suspended during the transfer, which will take 513 or 514 cycles after the $4014 write tick."
	//"(1 dummy read cycle while waiting for writes to complete, +1 if on an odd CPU cycle, then 256 alternating read/write cycles.)"
	if(Instance->_cycleCount % 2 != 0) {
		Instance->DummyRead();
	}
	Instance->DummyRead();

	Instance->_spriteDmaCounter = 256;

	//DMA transfer starts at SpriteRamAddr and wraps around
	for(int i = 0; i < 0x100; i++) {
		//Read value
		uint8_t readValue = Instance->MemoryRead(offsetValue * 0x100 + i);
		
		//Write to sprite ram via $2004 ("DMA is implemented in the 2A03/7 chip and works by repeatedly writing to OAMDATA")
		Instance->MemoryWrite(0x2004, readValue);

		Instance->_spriteDmaCounter--;
	}
	
	Instance->_spriteDmaTransfer = false;
}

void CPU::StartDmcTransfer()
{
	//"DMC DMA adds 4 cycles normally, 2 if it lands on the $4014 write or during OAM DMA"
	//3 cycles if it lands on the last write cycle of any instruction
	Instance->_dmcDmaRunning = true;
	if(Instance->_spriteDmaTransfer) {
		if(Instance->_spriteDmaCounter == 2) {
			Instance->_dmcCounter = 1;
		} else if(Instance->_spriteDmaCounter == 1) {
			Instance->_dmcCounter = 3;
		} else {
			Instance->_dmcCounter = 2;
		}
	} else {
		if(Instance->_cpuWrite) {
			if(Instance->_writeAddr == 0x4014) {
				Instance->_dmcCounter = 2;
			} else {
				Instance->_dmcCounter = 3;
			}
		} else {
			Instance->_dmcCounter = 4;
		}
	}
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

	Stream<int8_t>(_dmcCounter);
	Stream<bool>(_dmcDmaRunning);

	Stream<uint16_t>(_spriteDmaCounter);
	Stream<bool>(_spriteDmaTransfer);
}