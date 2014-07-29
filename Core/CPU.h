#pragma once

#include "stdafx.h"
#include "MemoryManager.h"
#include "Snapshotable.h"

namespace PSFlags
{
	enum PSFlags : uint8_t
	{
		Carry = 0x01,
		Zero = 0x02,
		Interrupt = 0x04,
		Decimal = 0x08,
		Break = 0x10,
		Reserved = 0x20,
		Overflow = 0x40,
		Negative = 0x80
	};
}

enum AddrMode
{
	None,	Imp, Imm, Rel,
	Zero, ZeroX, ZeroY,
	IndX,	IndY, IndYW,
	Abs, AbsX, AbsXW, AbsY, AbsYW
};

enum class IRQSource
{
	External = 1,
	FrameCounter = 2,
	DMC = 4,
};

struct State
{
	uint16_t PC;
	uint8_t SP;
	uint8_t A;
	uint8_t X;
	uint8_t Y;
	uint8_t PS;
};

class CPU : public Snapshotable
{
private:
	const uint16_t NMIVector = 0xFFFA;
	const uint16_t ResetVector = 0xFFFC;
	const uint16_t IRQVector = 0xFFFE;

	typedef void(CPU::*Func)();

	static int32_t CycleCount;
	static int32_t RelativeCycleCount;
	static uint32_t CyclePenalty;

	Func _opTable[256];
	uint8_t _cycles[256];
	AddrMode _addrMode[256];
	AddrMode _instAddrMode;
	uint8_t _cyclesPageCrossed[256];
	bool _pageCrossed = false;

	State _state;
	MemoryManager *_memoryManager = nullptr;

	static bool NMIFlag;
	static uint32_t IRQFlag;
	bool _runNMI = false;
	bool _runIRQ = false;

	uint8_t ReadByte()
	{
		return MemoryRead(_state.PC++);
	}

	uint16_t ReadWord()
	{
		uint16_t value = MemoryReadWord(PC());
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
		_pageCrossed = ((valA + valB) & 0xFF00) != (valA & 0xFF00);
		return _pageCrossed;
	}

	bool CheckPageCrossed(uint16_t valA, uint8_t valB)
	{
		_pageCrossed = ((valA + valB) & 0xFF00) != (valA & 0xFF00);
		return _pageCrossed;
	}

	void MemoryWrite(uint16_t addr, uint8_t value)
	{
		_memoryManager->Write(addr, value);
	}

	uint8_t MemoryRead(uint16_t addr) {
		return _memoryManager->Read(addr);
	}

	uint16_t MemoryReadWord(uint16_t addr) {
		return _memoryManager->ReadWord(addr);
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
	void SetPS(uint8_t value) { _state.PS = (value & 0xCF) | PSFlags::Reserved; }
	uint16_t PC() { return _state.PC; }
	void SetPC(uint16_t value) { _state.PC = value; }

	uint16_t GetOperandAddr()
	{
		switch(_instAddrMode) {
			case AddrMode::Abs: return GetAbsAddr();
			case AddrMode::AbsX: return GetAbsXAddr(false); 
			case AddrMode::AbsXW: return GetAbsXAddr(true);
			case AddrMode::AbsY: return GetAbsYAddr(false);
			case AddrMode::AbsYW: return GetAbsYAddr(true);
			case AddrMode::Imm: return GetImmediate();
			case AddrMode::IndX: return GetIndXAddr();
			case AddrMode::IndY: return GetIndYAddr(false);
			case AddrMode::IndYW: return GetIndYAddr(true);
			case AddrMode::Zero: return GetZeroAddr();
			case AddrMode::ZeroX: return GetZeroXAddr();
			case AddrMode::ZeroY: return GetZeroYAddr();
		}
		return 0;
	}

	uint8_t GetOperand()
	{
		uint16_t addr = GetOperandAddr();

		if(_instAddrMode != AddrMode::Imm) {
			return MemoryRead(addr);
		} else {
			return (uint8_t)addr;
		}
	}

	uint8_t GetImmediate() { return ReadByte(); }
	uint8_t GetZero() { return MemoryRead(GetZeroAddr()); }
	uint8_t GetZeroAddr() { return ReadByte(); }

	uint8_t GetZeroX() { return MemoryRead(GetZeroXAddr()); }
	uint8_t GetZeroXAddr() { return ReadByte() + X(); }

	uint8_t GetZeroY() { return MemoryRead(GetZeroYAddr()); }
	uint8_t GetZeroYAddr() { return ReadByte() + Y(); }
	
	uint8_t GetAbs() { return MemoryRead(GetAbsAddr()); }
	uint16_t GetAbsAddr() { return ReadWord(); }

	uint8_t GetAbsX() { return MemoryRead(GetAbsXAddr(false)); }
	uint16_t GetAbsXAddr(bool dummyRead = true) { 
		uint16_t baseAddr = ReadWord();
		bool pageCrossed = CheckPageCrossed(baseAddr, X());

		if(pageCrossed || dummyRead) {
			//Dummy read done by the processor (only when page is crossed for READ instructions)
			MemoryRead(baseAddr + X() - (pageCrossed ? 0x100 : 0));
		}
		return baseAddr + X(); 
	}

	uint8_t GetAbsY() { return MemoryRead(GetAbsYAddr(false)); }
	uint16_t GetAbsYAddr(bool dummyRead = true) { 
		uint16_t baseAddr = ReadWord();
		bool pageCrossed = CheckPageCrossed(baseAddr, Y());
		
		if(pageCrossed || dummyRead) {
			//Dummy read done by the processor (only when page is crossed for READ instructions)
			MemoryRead(baseAddr + Y() - (pageCrossed ? 0x100 : 0));
		}

		return baseAddr + Y(); 
	}

	uint16_t GetInd() { 
		uint16_t addr = ReadWord();
		if((addr & 0xFF) == 0xFF) {
			auto lo = MemoryRead(addr);
			auto hi = MemoryRead(addr - 0xFF);
			return (lo | hi << 8);
		} else {
			return MemoryReadWord(addr);
		}
	}

	uint8_t GetIndX() { return MemoryRead(GetIndXAddr()); }
	uint16_t GetIndXAddr() {
		uint8_t zero = ReadByte();
		
		//Dummy read
		MemoryRead(zero);

		zero += X();
		
		uint16_t addr;
		if(zero == 0xFF) {
			addr = MemoryRead(0xFF) | MemoryRead(0x00) << 8;
		} else {
			addr = MemoryReadWord(zero);
		}
		return addr;
	}

	uint8_t GetIndY() { return MemoryRead(GetIndYAddr(false)); }
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
			MemoryRead(addr + Y() - (pageCrossed ? 0x100 : 0));
		}
		return addr + Y();
	}

	void AND() { SetA(A() & GetOperand()); }
	void EOR() { SetA(A() ^ GetOperand()); }
	void ORA() { SetA(A() | GetOperand()); }

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

	void ADC() { ADD(GetOperand()); }
	void SBC() { ADD(GetOperand() ^ 0xFF); }

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

	void CPA() { CMP(A(), GetOperand()); }
	void CPX() { CMP(X(), GetOperand()); }
	void CPY() { CMP(Y(), GetOperand()); }

	void INC() 
	{
		uint16_t addr = GetOperandAddr();
		ClearFlags(PSFlags::Negative | PSFlags::Zero);
		uint8_t value = MemoryRead(addr);		
		
		MemoryWrite(addr, value); //Dummy write
		
		value++;
		SetZeroNegativeFlags(value);
		MemoryWrite(addr, value);
	}

	void DEC() 
	{
		uint16_t addr = GetOperandAddr();
		ClearFlags(PSFlags::Negative | PSFlags::Zero);
		uint8_t value = MemoryRead(addr);
		MemoryWrite(addr, value); //Dummy write
		
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
		return value >> 1;
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
		uint16_t addr = GetOperandAddr();
		uint8_t value = MemoryRead(addr);
		MemoryWrite(addr, value); //Dummy write
		MemoryWrite(addr, ASL(value));
	}

	void LSRAddr() {
		uint16_t addr = GetOperandAddr();
		uint8_t value = MemoryRead(addr);
		MemoryWrite(addr, value); //Dummy write
		MemoryWrite(addr, LSR(value));
	}

	void ROLAddr() {
		uint16_t addr = GetOperandAddr();
		uint8_t value = MemoryRead(addr);
		MemoryWrite(addr, value); //Dummy write
		MemoryWrite(addr, ROL(value));
	}

	void RORAddr() {
		uint16_t addr = GetOperandAddr();
		uint8_t value = MemoryRead(addr);
		MemoryWrite(addr, value); //Dummy write
		MemoryWrite(addr, ROR(value));
	}

	void JMP(uint16_t addr) {
		SetPC(addr);
	}

	void BranchRelative(bool branch) {
		int8_t offset = GetImmediate();
		if(branch) {
			CheckPageCrossed(PC(), offset);
			IncCycleCount(1);

			SetPC(PC() + offset);
		}
	}

	void BIT() {
		uint8_t value = GetOperand();
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

	bool IsPageCrossed() {
		bool pageCrossed = _pageCrossed;
		_pageCrossed = false;
		return pageCrossed;
	}

	uint32_t GetCyclePenalty() {
		uint32_t cyclePenalty = CPU::CyclePenalty;
		CPU::CyclePenalty = 0;
		return cyclePenalty;
	}

	#pragma region OP Codes
	void LDA() { SetA(GetOperand()); }
	void LDX() { SetX(GetOperand()); }
	void LDY() { SetY(GetOperand()); }

	void STA() { MemoryWrite(GetOperandAddr(), A()); }
	void STX() { MemoryWrite(GetOperandAddr(), X()); }
	void STY() { MemoryWrite(GetOperandAddr(), Y()); }

	void TAX() { SetX(A()); }
	void TAY() { SetY(A()); }
	void TSX() { SetX(SP()); }
	void TXA() { SetA(X()); }
	void TXS() { SetSP(X()); }
	void TYA() { SetA(Y()); }

	void PHA() { Push(A()); }
	void PHP() {
		uint8_t flags = PS() | PSFlags::Break;
		Push((uint8_t)flags);
	}
	void PLA() { SetA(Pop()); }
	void PLP() { SetPS(Pop()); }

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
		JMP(GetAbsAddr());
	}
	void JMP_Ind() { JMP(GetInd()); }
	void JSR() {
		uint16_t addr = GetAbsAddr();
		Push((uint16_t)(PC() - 1));
		JMP(addr);
	}
	void RTS() {
		uint16_t addr = PopWord();
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

	void BRK() {
		Push((uint16_t)(PC() + 1));

		uint8_t flags = PS() | PSFlags::Break;
		if(CPU::NMIFlag) {
			Push((uint8_t)flags);
			SetFlags(PSFlags::Interrupt);

			SetPC(MemoryReadWord(CPU::NMIVector));
		} else {

			Push((uint8_t)flags);
			SetFlags(PSFlags::Interrupt);

			SetPC(MemoryReadWord(CPU::IRQVector));
		}
	}

	void NMI() {
		Push((uint16_t)(PC()));
		Push((uint8_t)PS());
		SetFlags(PSFlags::Interrupt);
		SetPC(MemoryReadWord(CPU::NMIVector));
	}

	void IRQ() {
		Push((uint16_t)(PC()));

		if(CPU::NMIFlag) {
			Push((uint8_t)PS());
			SetFlags(PSFlags::Interrupt);

			SetPC(MemoryReadWord(CPU::NMIVector));
		} else {
			Push((uint8_t)PS());
			SetFlags(PSFlags::Interrupt);
			SetPC(MemoryReadWord(CPU::IRQVector));
		}
	}
	
	void RTI() {
		SetPS(Pop());
		SetPC(PopWord());
	}

	void NOP() {
		GetOperand();
	}
	
	#pragma endregion

protected:
	void StreamState(bool saving);

public:
	static const uint32_t ClockRate = 1789773;

	CPU(MemoryManager *memoryManager);
	static int32_t GetCycleCount() { return CPU::CycleCount; }
	static int32_t GetRelativeCycleCount() { return CPU::RelativeCycleCount + CPU::CycleCount; }
	static void IncCycleCount(uint32_t cycles) { 
		CPU::CyclePenalty += cycles;
		CPU::CycleCount += cycles;
	}
	static void SetNMIFlag() { CPU::NMIFlag = true; }
	static void ClearNMIFlag() { CPU::NMIFlag = false; }
	static void SetIRQSource(IRQSource source) 
	{ 
		CPU::IRQFlag |= (int)source; 
	}
	static void ClearIRQSource(IRQSource source)
	{
		CPU::IRQFlag &= ~(int)source;
	}

	void Reset(bool softReset);
	uint32_t Exec();
	void EndFrame();

	State GetState() { return _state; }

};