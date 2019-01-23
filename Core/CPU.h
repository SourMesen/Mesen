#if (defined(DUMMYCPU) && !defined(__DUMMYCPU__H)) || (!defined(DUMMYCPU) && !defined(__CPU__H))
#ifdef DUMMYCPU
#define __DUMMYCPU__H
#else
#define __CPU__H
#endif

#include "stdafx.h"
#include "Snapshotable.h"
#include "Types.h"

enum class NesModel;
class Console;
class MemoryManager;
class DummyCpu;

class CPU : public Snapshotable
{
#ifndef DUMMYCPU
	friend DummyCpu;
#endif

public:
	static constexpr uint16_t NMIVector = 0xFFFA;
	static constexpr uint16_t ResetVector = 0xFFFC;
	static constexpr uint16_t IRQVector = 0xFFFE;
	static constexpr uint32_t ClockRateNtsc = 1789773;
	static constexpr uint32_t ClockRatePal = 1662607;
	static constexpr uint32_t ClockRateDendy = 1773448;

private:
	typedef void(CPU::*Func)();

	int32_t _cycleCount;
	uint16_t _operand;

	Func _opTable[256];
	AddrMode _addrMode[256];
	AddrMode _instAddrMode;

	uint16_t _spriteDmaCounter;
	bool _spriteDmaTransfer;

	int8_t _dmcCounter;
	bool _dmcDmaRunning;
	bool _cpuWrite = false;
	uint16_t _writeAddr = 0;

	uint8_t _irqMask;

	State _state;
	shared_ptr<Console> _console;
	MemoryManager* _memoryManager;

	bool _prevRunIrq = false;
	bool _runIrq = false;

	bool _warnOnCrash = true;

#ifdef DUMMYCPU
	uint32_t _writeCounter = 0;
	uint16_t _writeAddresses[10];
	uint8_t _writeValue[10];
	bool _isDummyWrite[10];

	uint32_t _readCounter = 0;
	uint16_t _readAddresses[10];
	uint8_t _readValue[10];
	bool _isDummyRead[10];
#endif

	void IncCycleCount();
	uint16_t FetchOperand();
	void IRQ();

	uint8_t GetOPCode()
	{
		uint8_t opCode = MemoryRead(_state.PC, MemoryOperationType::ExecOpCode);
		_state.PC++;
		return opCode;
	}

	void DummyRead()
	{
		MemoryRead(_state.PC, MemoryOperationType::DummyRead);
	}
	
	uint8_t ReadByte()
	{
		uint8_t value = MemoryRead(_state.PC, MemoryOperationType::ExecOperand);
		_state.PC++;
		return value;
	}

	uint16_t ReadWord()
	{
		uint16_t value = MemoryReadWord(_state.PC, MemoryOperationType::ExecOperand);
		_state.PC += 2;
		return value;
	}

	void ClearFlags(uint8_t flags)
	{
		_state.PS &= ~flags;
	}

	void SetFlags(uint8_t flags)
	{
		_state.PS |= flags;
	}

	bool CheckFlag(uint8_t flag)
	{
		return (_state.PS & flag) == flag;
	}

	void SetZeroNegativeFlags(uint8_t value)
	{
		if(value == 0) {
			SetFlags(PSFlags::Zero);
		} else if(value & 0x80) {
			SetFlags(PSFlags::Negative);
		}
	}

	bool CheckPageCrossed(uint16_t valA, int8_t valB)
	{
		return ((valA + valB) & 0xFF00) != (valA & 0xFF00);
	}

	bool CheckPageCrossed(uint16_t valA, uint8_t valB)
	{
		return ((valA + valB) & 0xFF00) != (valA & 0xFF00);
	}

	void MemoryWrite(uint16_t addr, uint8_t value, MemoryOperationType operationType = MemoryOperationType::Write);
	uint8_t MemoryRead(uint16_t addr, MemoryOperationType operationType = MemoryOperationType::Read);

	uint16_t MemoryReadWord(uint16_t addr, MemoryOperationType operationType = MemoryOperationType::Read) {
		uint8_t lo = MemoryRead(addr, operationType);
		uint8_t hi = MemoryRead(addr + 1, operationType);
		return lo | hi << 8;
	}

	void SetRegister(uint8_t &reg, uint8_t value) {
		ClearFlags(PSFlags::Zero | PSFlags::Negative);
		SetZeroNegativeFlags(value);
		reg = value;
	}

	void Push(uint8_t value) {
		MemoryWrite(SP() + 0x100, value);
		SetSP(SP() - 1);
	}

	void Push(uint16_t value) {
		Push((uint8_t)(value >> 8));
		Push((uint8_t)value);
	}

	uint8_t Pop() {
		SetSP(SP() + 1);
		return MemoryRead(0x100 + SP());
	}

	uint16_t PopWord() {
		uint8_t lo = Pop();
		uint8_t hi = Pop();
		
		return lo | hi << 8;
	}

	uint8_t A() { return _state.A; }
	void SetA(uint8_t value) { SetRegister(_state.A, value); }
	uint8_t X() { return _state.X; }
	void SetX(uint8_t value) { SetRegister(_state.X, value); }
	uint8_t Y() { return _state.Y; }
	void SetY(uint8_t value) { SetRegister(_state.Y, value); }
	uint8_t SP() { return _state.SP; }
	void SetSP(uint8_t value) { _state.SP = value; }
	uint8_t PS() { return _state.PS; }
	void SetPS(uint8_t value) { _state.PS = value & 0xCF; }
	uint16_t PC() { return _state.PC; }
	void SetPC(uint16_t value) { _state.PC = value; }

	uint16_t GetOperand()
	{
		return _operand;
	}

	uint8_t GetOperandValue()
	{
		if(_instAddrMode >= AddrMode::Zero) {
			return MemoryRead(GetOperand());
		} else {
			return (uint8_t)GetOperand();
		}
	}

	uint16_t GetIndAddr() { return ReadWord(); }
	uint8_t GetImmediate() { return ReadByte(); }
	uint8_t GetZeroAddr() { return ReadByte(); }
	uint8_t GetZeroXAddr() { 
		uint8_t value = ReadByte();
		MemoryRead(value, MemoryOperationType::DummyRead); //Dummy read
		return value + X();
	}
	uint8_t GetZeroYAddr() { 
		uint8_t value = ReadByte();
		MemoryRead(value, MemoryOperationType::DummyRead); //Dummy read
		return value + Y();
	}
	uint16_t GetAbsAddr() { return ReadWord(); }

	uint16_t GetAbsXAddr(bool dummyRead = true) { 
		uint16_t baseAddr = ReadWord();
		bool pageCrossed = CheckPageCrossed(baseAddr, X());

		if(pageCrossed || dummyRead) {
			//Dummy read done by the processor (only when page is crossed for READ instructions)
			MemoryRead(baseAddr + X() - (pageCrossed ? 0x100 : 0), MemoryOperationType::DummyRead);
		}
		return baseAddr + X(); 
	}

	uint16_t GetAbsYAddr(bool dummyRead = true) { 
		uint16_t baseAddr = ReadWord();
		bool pageCrossed = CheckPageCrossed(baseAddr, Y());
		
		if(pageCrossed || dummyRead) {
			//Dummy read done by the processor (only when page is crossed for READ instructions)
			MemoryRead(baseAddr + Y() - (pageCrossed ? 0x100 : 0), MemoryOperationType::DummyRead);
		}

		return baseAddr + Y(); 
	}

	uint16_t GetInd() { 
		uint16_t addr = GetOperand();
		if((addr & 0xFF) == 0xFF) {
			auto lo = MemoryRead(addr);
			auto hi = MemoryRead(addr - 0xFF);
			return (lo | hi << 8);
		} else {
			return MemoryReadWord(addr);
		}
	}

	uint16_t GetIndXAddr() {
		uint8_t zero = ReadByte();
		
		//Dummy read
		MemoryRead(zero, MemoryOperationType::DummyRead);

		zero += X();
		
		uint16_t addr;
		if(zero == 0xFF) {
			addr = MemoryRead(0xFF) | MemoryRead(0x00) << 8;
		} else {
			addr = MemoryReadWord(zero);
		}
		return addr;
	}

	uint16_t GetIndYAddr(bool dummyRead = true) {
		uint8_t zero = ReadByte();
		
		uint16_t addr;
		if(zero == 0xFF) {
			addr = MemoryRead(0xFF) | MemoryRead(0x00) << 8;
		} else {
			addr = MemoryReadWord(zero);
		}

		bool pageCrossed = CheckPageCrossed(addr, Y());			
		if(pageCrossed || dummyRead) {
			//Dummy read done by the processor (only when page is crossed for READ instructions)
			MemoryRead(addr + Y() - (pageCrossed ? 0x100 : 0), MemoryOperationType::DummyRead);
		}
		return addr + Y();
	}

	void AND() { SetA(A() & GetOperandValue()); }
	void EOR() { SetA(A() ^ GetOperandValue()); }
	void ORA() { SetA(A() | GetOperandValue()); }

	void ADD(uint8_t value)
	{
		uint16_t result = (uint16_t)A() + (uint16_t)value + (CheckFlag(PSFlags::Carry) ? PSFlags::Carry : 0x00);
		
		ClearFlags(PSFlags::Carry | PSFlags::Negative | PSFlags::Overflow | PSFlags::Zero);
		SetZeroNegativeFlags((uint8_t)result);
		if(~(A() ^ value) & (A() ^ result) & 0x80) {
			SetFlags(PSFlags::Overflow);
		}
		if(result > 0xFF) {
			SetFlags(PSFlags::Carry);
		}
		SetA((uint8_t)result);
	}

	void ADC() { ADD(GetOperandValue()); }
	void SBC() { ADD(GetOperandValue() ^ 0xFF); }

	void CMP(uint8_t reg, uint8_t value) 
	{
		ClearFlags(PSFlags::Carry | PSFlags::Negative | PSFlags::Zero);

		auto result = reg - value;

		if(reg >= value) {
			SetFlags(PSFlags::Carry);
		}
		if(reg == value) {
			SetFlags(PSFlags::Zero);
		}
		if((result & 0x80) == 0x80) {
			SetFlags(PSFlags::Negative);
		}
	}

	void CPA() { CMP(A(), GetOperandValue()); }
	void CPX() { CMP(X(), GetOperandValue()); }
	void CPY() { CMP(Y(), GetOperandValue()); }

	void INC() 
	{
		uint16_t addr = GetOperand();
		ClearFlags(PSFlags::Negative | PSFlags::Zero);
		uint8_t value = MemoryRead(addr);		
		
		MemoryWrite(addr, value, MemoryOperationType::DummyWrite); //Dummy write
		
		value++;
		SetZeroNegativeFlags(value);
		MemoryWrite(addr, value);
	}

	void DEC() 
	{
		uint16_t addr = GetOperand();
		ClearFlags(PSFlags::Negative | PSFlags::Zero);
		uint8_t value = MemoryRead(addr);
		MemoryWrite(addr, value, MemoryOperationType::DummyWrite); //Dummy write
		
		value--;
		SetZeroNegativeFlags(value);
		MemoryWrite(addr, value);
	}

	uint8_t ASL(uint8_t value)
	{
		ClearFlags(PSFlags::Carry | PSFlags::Negative | PSFlags::Zero);
		if(value & 0x80) {
			SetFlags(PSFlags::Carry);
		}

		uint8_t result = value << 1;
		SetZeroNegativeFlags(result);
		return result;
	}

	uint8_t LSR(uint8_t value) {
		ClearFlags(PSFlags::Carry | PSFlags::Negative | PSFlags::Zero);
		if(value & 0x01) {
			SetFlags(PSFlags::Carry);
		}

		uint8_t result = value >> 1;
		SetZeroNegativeFlags(result);
		return result;
	}

	uint8_t ROL(uint8_t value) {
		bool carryFlag = CheckFlag(PSFlags::Carry);
		ClearFlags(PSFlags::Carry | PSFlags::Negative | PSFlags::Zero);

		if(value & 0x80) {
			SetFlags(PSFlags::Carry);
		}

		uint8_t result = (value << 1 | (carryFlag ? 0x01 : 0x00));
		SetZeroNegativeFlags(result);
		return result;
	}

	uint8_t ROR(uint8_t value) {
		bool carryFlag = CheckFlag(PSFlags::Carry);
		ClearFlags(PSFlags::Carry | PSFlags::Negative | PSFlags::Zero);
		if(value & 0x01) {
			SetFlags(PSFlags::Carry);
		}

		uint8_t result = (value >> 1 | (carryFlag ? 0x80 : 0x00));
		SetZeroNegativeFlags(result);
		return result;
	}

	void ASLAddr() {
		uint16_t addr = GetOperand();
		uint8_t value = MemoryRead(addr);
		MemoryWrite(addr, value, MemoryOperationType::DummyWrite); //Dummy write
		MemoryWrite(addr, ASL(value));
	}

	void LSRAddr() {
		uint16_t addr = GetOperand();
		uint8_t value = MemoryRead(addr);
		MemoryWrite(addr, value, MemoryOperationType::DummyWrite); //Dummy write
		MemoryWrite(addr, LSR(value));
	}

	void ROLAddr() {
		uint16_t addr = GetOperand();
		uint8_t value = MemoryRead(addr);
		MemoryWrite(addr, value, MemoryOperationType::DummyWrite); //Dummy write
		MemoryWrite(addr, ROL(value));
	}

	void RORAddr() {
		uint16_t addr = GetOperand();
		uint8_t value = MemoryRead(addr);
		MemoryWrite(addr, value, MemoryOperationType::DummyWrite); //Dummy write
		MemoryWrite(addr, ROR(value));
	}

	void JMP(uint16_t addr) {
		SetPC(addr);
	}

	void BranchRelative(bool branch) {
		int8_t offset = (int8_t)GetOperand();
		if(branch) {
			//"a taken non-page-crossing branch ignores IRQ/NMI during its last clock, so that next instruction executes before the IRQ"
			//Fixes "branch_delays_irq" test
			if(_runIrq && !_prevRunIrq) {
				_runIrq = false;
			}
			DummyRead();

			if(CheckPageCrossed(PC(), offset)) {
				DummyRead();
			}

			SetPC(PC() + offset);
		}
	}

	void BIT() {
		uint8_t value = GetOperandValue();
		ClearFlags(PSFlags::Zero | PSFlags::Overflow | PSFlags::Negative);
		if((A() & value) == 0) {
			SetFlags(PSFlags::Zero);
		}
		if(value & 0x40) {
			SetFlags(PSFlags::Overflow);
		}
		if(value & 0x80) {
			SetFlags(PSFlags::Negative);
		}
	}

	//OP Codes
	void LDA() { SetA(GetOperandValue()); }
	void LDX() { SetX(GetOperandValue()); }
	void LDY() { SetY(GetOperandValue()); }

	void STA() { MemoryWrite(GetOperand(), A()); }
	void STX() { MemoryWrite(GetOperand(), X()); }
	void STY() { MemoryWrite(GetOperand(), Y()); }

	void TAX() { SetX(A()); }
	void TAY() { SetY(A()); }
	void TSX() { SetX(SP()); }
	void TXA() { SetA(X()); }
	void TXS() { SetSP(X()); }
	void TYA() { SetA(Y()); }

	void PHA() { Push(A()); }
	void PHP() {
		uint8_t flags = PS() | PSFlags::Break | PSFlags::Reserved;
		Push((uint8_t)flags);
	}
	void PLA() { 
		DummyRead();
		SetA(Pop()); 
	}
	void PLP() { 
		DummyRead();
		SetPS(Pop()); 
	}

	void INX() { SetX(X() + 1); }
	void INY() { SetY(Y() + 1); }

	void DEX() { SetX(X() - 1); }
	void DEY() { SetY(Y() - 1); }

	void ASL_Acc() { SetA(ASL(A())); }
	void ASL_Memory() { ASLAddr(); }

	void LSR_Acc() { SetA(LSR(A())); }
	void LSR_Memory() { LSRAddr(); }

	void ROL_Acc() { SetA(ROL(A())); }
	void ROL_Memory() { ROLAddr(); }

	void ROR_Acc() { SetA(ROR(A())); }
	void ROR_Memory() { RORAddr(); }

	void JMP_Abs() {
		JMP(GetOperand());
	}
	void JMP_Ind() { JMP(GetInd()); }
	void JSR() {
		uint16_t addr = GetOperand();
		DummyRead();
		Push((uint16_t)(PC() - 1));
		JMP(addr);
	}
	void RTS() {
		uint16_t addr = PopWord();
		DummyRead();
		DummyRead();
		SetPC(addr + 1);
	}

	void BCC() {
		BranchRelative(!CheckFlag(PSFlags::Carry));
	}

	void BCS() {
		BranchRelative(CheckFlag(PSFlags::Carry));
	}

	void BEQ() {
		BranchRelative(CheckFlag(PSFlags::Zero));
	}

	void BMI() {
		BranchRelative(CheckFlag(PSFlags::Negative));
	}

	void BNE() {
		BranchRelative(!CheckFlag(PSFlags::Zero));
	}

	void BPL() {
		BranchRelative(!CheckFlag(PSFlags::Negative));
	}

	void BVC() {
		BranchRelative(!CheckFlag(PSFlags::Overflow));
	}

	void BVS() {
		BranchRelative(CheckFlag(PSFlags::Overflow));
	}

	void CLC() { ClearFlags(PSFlags::Carry); }
	void CLD() { ClearFlags(PSFlags::Decimal); }
	void CLI() { ClearFlags(PSFlags::Interrupt); }
	void CLV() { ClearFlags(PSFlags::Overflow); }
	void SEC() { SetFlags(PSFlags::Carry); }
	void SED() { SetFlags(PSFlags::Decimal); }
	void SEI() { SetFlags(PSFlags::Interrupt); }

	void BRK();
	
	void RTI() {
		DummyRead();
		SetPS(Pop());
		SetPC(PopWord());
	}

	void NOP() {
		//Make sure the nop operation takes as many cycles as meant to
		GetOperandValue();
	}

	
	//Unofficial OpCodes
	void SLO()
	{
		//ASL & ORA
		uint8_t value = GetOperandValue();
		MemoryWrite(GetOperand(), value, MemoryOperationType::DummyWrite); //Dummy write
		uint8_t shiftedValue = ASL(value);
		SetA(A() | shiftedValue);
		MemoryWrite(GetOperand(), shiftedValue);
	}
	
	void SRE()
	{
		//ROL & AND
		uint8_t value = GetOperandValue();
		MemoryWrite(GetOperand(), value, MemoryOperationType::DummyWrite); //Dummy write
		uint8_t shiftedValue = LSR(value);
		SetA(A() ^ shiftedValue);
		MemoryWrite(GetOperand(), shiftedValue);
	}
	
	void RLA()
	{
		//LSR & EOR
		uint8_t value = GetOperandValue();
		MemoryWrite(GetOperand(), value, MemoryOperationType::DummyWrite); //Dummy write
		uint8_t shiftedValue = ROL(value);
		SetA(A() & shiftedValue);
		MemoryWrite(GetOperand(), shiftedValue);
	}

	void RRA()
	{
		//ROR & ADC
		uint8_t value = GetOperandValue();
		MemoryWrite(GetOperand(), value, MemoryOperationType::DummyWrite); //Dummy write
		uint8_t shiftedValue = ROR(value);
		ADD(shiftedValue);
		MemoryWrite(GetOperand(), shiftedValue);
	}

	void SAX()
	{
		//STA & STX
		MemoryWrite(GetOperand(), A() & X());
	}

	void LAX()
	{
		//LDA & LDX
		uint8_t value = GetOperandValue();
		SetX(value);
		SetA(value);
	}

	void DCP()
	{
		//DEC & CMP
		uint8_t value = GetOperandValue();
		MemoryWrite(GetOperand(), value, MemoryOperationType::DummyWrite); //Dummy write
		value--;
		CMP(A(), value);
		MemoryWrite(GetOperand(), value);
	}

	void ISB()
	{
		//INC & SBC
		uint8_t value = GetOperandValue();
		MemoryWrite(GetOperand(), value, MemoryOperationType::DummyWrite); //Dummy write
		value++;
		ADD(value ^ 0xFF);
		MemoryWrite(GetOperand(), value);
	}

	void AAC()
	{
		SetA(A() & GetOperandValue());

		ClearFlags(PSFlags::Carry);
		if(CheckFlag(PSFlags::Negative)) {
			SetFlags(PSFlags::Carry);
		}
	}

	void ASR()
	{
		ClearFlags(PSFlags::Carry);
		SetA(A() & GetOperandValue());
		if(A() & 0x01) {
			SetFlags(PSFlags::Carry);
		}
		SetA(A() >> 1);
	}

	void ARR()
	{
		SetA(((A() & GetOperandValue()) >> 1) | (CheckFlag(PSFlags::Carry) ? 0x80 : 0x00));
		ClearFlags(PSFlags::Carry | PSFlags::Overflow);
		if(A() & 0x40) {
			SetFlags(PSFlags::Carry);
		}
		if((CheckFlag(PSFlags::Carry) ? 0x01 : 0x00) ^ ((A() >> 5) & 0x01)) {
			SetFlags(PSFlags::Overflow);
		}
	}

	void ATX()
	{
		//LDA & TAX
		uint8_t value = GetOperandValue();
		SetA(value); //LDA
		SetX(A()); //TAX
		SetA(A()); //Update flags based on A
	}

	void AXS()
	{
		//CMP & DEX
		uint8_t opValue = GetOperandValue();
		uint8_t value = (A() & X()) - opValue;
		
		ClearFlags(PSFlags::Carry);
		if((A() & X()) >= opValue) {
			SetFlags(PSFlags::Carry);
		}

		SetX(value);
	}

	void SYA()
	{
		uint8_t addrHigh = GetOperand() >> 8;
		uint8_t addrLow = GetOperand() & 0xFF;
		uint8_t value = Y() & (addrHigh + 1);
		
		//From here: http://forums.nesdev.com/viewtopic.php?f=3&t=3831&start=30
		//Unsure if this is accurate or not
		//"the target address for e.g. SYA becomes ((y & (addr_high + 1)) << 8) | addr_low instead of the normal ((addr_high + 1) << 8) | addr_low"
		MemoryWrite(((Y() & (addrHigh + 1)) << 8) | addrLow, value);
	}

	void SXA()
	{
		uint8_t addrHigh = GetOperand() >> 8;
		uint8_t addrLow = GetOperand() & 0xFF;
		uint8_t value = X() & (addrHigh + 1);
		MemoryWrite(((X() & (addrHigh + 1)) << 8) | addrLow, value);
	}
	
	//Unimplemented/Incorrect Unofficial OP codes
	void HLT()
	{
		//normally freezes the cpu, we can probably assume nothing will ever call this
		GetOperandValue();
	}

	void UNK()
	{
		//Make sure we take the right amount of cycles (not reliable for operations that write to memory, etc.)
		GetOperandValue();
	}

	void AXA()
	{
		uint16_t addr = GetOperand();
		
		//"This opcode stores the result of A AND X AND the high byte of the target address of the operand +1 in memory."	
		//This may not be the actual behavior, but the read/write operations are needed for proper cycle counting
		MemoryWrite(GetOperand(), ((addr >> 8) + 1) & A() & X());
	}

	void TAS()
	{
		//"AND X register with accumulator and store result in stack
		//pointer, then AND stack pointer with the high byte of the
		//target address of the argument + 1. Store result in memory."
		uint16_t addr = GetOperand();
		SetSP(X() & A());
		MemoryWrite(addr, SP() & ((addr >> 8) + 1));
	}

	void LAS()
	{
		//"AND memory with stack pointer, transfer result to accumulator, X register and stack pointer."
		uint8_t value = GetOperandValue();
		SetA(value & SP());
		SetX(A());
		SetSP(A());
	}

protected:
	void StreamState(bool saving) override;

public:
	CPU(shared_ptr<Console> console);
	
	int32_t GetCycleCount() { return _cycleCount; }
	
	int32_t GetElapsedCycles(int32_t prevCycleCount)
	{
		if(prevCycleCount > _cycleCount) {
			return 0xFFFFFFFF - prevCycleCount + _cycleCount + 1;
		} else {
			return _cycleCount - prevCycleCount;
		}
	}

	void SetNmiFlag() { _state.NMIFlag = true; }
	void ClearNmiFlag() { _state.NMIFlag = false; }
	void SetIrqMask(uint8_t mask) { _irqMask = mask; }
	void SetIrqSource(IRQSource source) { _state.IRQFlag |= (int)source; }
	bool HasIrqSource(IRQSource source) { return (_state.IRQFlag & (int)source) != 0; }
	void ClearIrqSource(IRQSource source) { _state.IRQFlag &= ~(int)source; }

	void RunDMATransfer(uint8_t offsetValue);
	void StartDmcTransfer();

	uint32_t GetClockRate(NesModel model);
	bool IsCpuWrite() { return _cpuWrite; }
		
	//Used by debugger for "Set Next Statement"
	void SetDebugPC(uint16_t value) {
		SetPC(value);
		_state.PreviousDebugPC = _state.DebugPC;
		_state.DebugPC = value;
	}

	void Reset(bool softReset, NesModel model);
	void Exec();

	void GetState(State &state) 
	{ 
		state = _state;
		state.CycleCount = _cycleCount;
	}

	uint16_t GetDebugPC() { return _state.DebugPC; }
	uint16_t GetPC() { return _state.PC; }

	void SetState(State state)
	{
		uint16_t originalPc = state.PC;
		uint16_t originalDebugPc = state.DebugPC;
		_state = state;
		_cycleCount = state.CycleCount;
		state.PC = originalPc;
		state.DebugPC = originalDebugPc;
	}

#ifdef DUMMYCPU
#undef CPU
	void SetDummyState(CPU *c)
	{
#define CPU DummyCpu
		_writeCounter = 0;
		_readCounter = 0;

		_state = c->_state;

		_cycleCount = c->_cycleCount;
		_operand = c->_operand;
		_spriteDmaCounter = c->_spriteDmaCounter;
		_spriteDmaTransfer = c->_spriteDmaTransfer;
		_dmcCounter = c->_dmcCounter;
		_dmcDmaRunning = c->_dmcDmaRunning;
		_cpuWrite = c->_cpuWrite;
		_irqMask = c->_irqMask;
		_prevRunIrq = c->_prevRunIrq;
		_runIrq = c->_runIrq;
		_cycleCount = c->_cycleCount;
	}

	uint32_t GetWriteCount()
	{
		return _writeCounter;
	}

	uint32_t GetReadCount()
	{
		return _readCounter;
	}

	void GetWriteAddrValue(uint32_t index, uint16_t &addr, uint8_t &value, bool &isDummyWrite)
	{
		addr = _writeAddresses[index];
		value = _writeValue[index];
		isDummyWrite = _isDummyWrite[index];
	}

	void GetReadAddr(uint32_t index, uint16_t &addr, uint8_t &value, bool &isDummyRead)
	{
		addr = _readAddresses[index];
		value = _readValue[index];
		isDummyRead = _isDummyRead[index];
	}
#endif
};

#endif