#pragma once

#include "stdafx.h"
#include "MemoryManager.h"
#include "PPU.h"
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
	None,	Acc, Imp, Imm, Rel,
	Zero, ZeroX, ZeroY,
	Ind, IndX, IndY, IndYW,
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
	uint16_t PC = 0;
	uint8_t SP = 0;
	uint8_t A = 0;
	uint8_t X = 0;
	uint8_t Y = 0;
	uint8_t PS = 0;
	uint32_t IRQFlag = 0;
	bool NMIFlag = false;
};

class CPU : public Snapshotable
{
private:
	const uint16_t NMIVector = 0xFFFA;
	const uint16_t ResetVector = 0xFFFC;
	const uint16_t IRQVector = 0xFFFE;

	static CPU* Instance;

	typedef void(CPU::*Func)();

	int32_t _cycleCount;
	int32_t _relativeCycleCount;
	uint16_t _operand;

	Func _opTable[256];
	uint8_t _cycles[256];
	AddrMode _addrMode[256];
	AddrMode _instAddrMode;
	uint8_t _cyclesPageCrossed[256];
	bool _pageCrossed = false;

	State _state;
	MemoryManager *_memoryManager = nullptr;

	bool _runNMI = false;
	bool _runIRQ = false;

	void IncCycleCount();

	uint8_t GetOPCode()
	{
		return MemoryRead(_state.PC++, true);
	}

	void DummyRead()
	{
		MemoryRead(_state.PC);
	}

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
		IncCycleCount();
	}

	uint8_t MemoryRead(uint16_t addr, bool forExecute = false) {
		uint8_t value = _memoryManager->Read(addr, forExecute);
		IncCycleCount();
		return value;
	}

	uint16_t MemoryReadWord(uint16_t addr) {
		uint8_t lo = MemoryRead(addr);
		uint8_t hi = MemoryRead(addr + 1);
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
	void SetPS(uint8_t value) { _state.PS = (value & 0xCF) | PSFlags::Reserved; }
	uint16_t PC() { return _state.PC; }
	void SetPC(uint16_t value) { _state.PC = value; }

	uint16_t FetchOperand()
	{
		switch(_instAddrMode) {
			case AddrMode::Acc: 
			case AddrMode::Imp: DummyRead(); return 0;
			case AddrMode::Imm: 
			case AddrMode::Rel: return GetImmediate();
			case AddrMode::Zero: return GetZeroAddr();
			case AddrMode::ZeroX: return GetZeroXAddr();
			case AddrMode::ZeroY: return GetZeroYAddr();
			case AddrMode::Ind: return GetIndAddr();
			case AddrMode::IndX: return GetIndXAddr();
			case AddrMode::IndY: return GetIndYAddr(false);
			case AddrMode::IndYW: return GetIndYAddr(true);
			case AddrMode::Abs: return GetAbsAddr();
			case AddrMode::AbsX: return GetAbsXAddr(false); 
			case AddrMode::AbsXW: return GetAbsXAddr(true);
			case AddrMode::AbsY: return GetAbsYAddr(false);
			case AddrMode::AbsYW: return GetAbsYAddr(true);
		}
		throw std::runtime_error("invalid addressing mode");
	}

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
		MemoryRead(value); //Dummy read
		return value + X();
	}
	uint8_t GetZeroYAddr() { 
		uint8_t value = ReadByte();
		MemoryRead(value); //Dummy read
		return value + Y();
	}
	uint16_t GetAbsAddr() { return ReadWord(); }

	uint16_t GetAbsXAddr(bool dummyRead = true) { 
		uint16_t baseAddr = ReadWord();
		bool pageCrossed = CheckPageCrossed(baseAddr, X());

		if(pageCrossed || dummyRead) {
			//Dummy read done by the processor (only when page is crossed for READ instructions)
			MemoryRead(baseAddr + X() - (pageCrossed ? 0x100 : 0));
		}
		return baseAddr + X(); 
	}

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
		
		MemoryWrite(addr, value); //Dummy write
		
		value++;
		SetZeroNegativeFlags(value);
		MemoryWrite(addr, value);
	}

	void DEC() 
	{
		uint16_t addr = GetOperand();
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
		uint16_t addr = GetOperand();
		uint8_t value = MemoryRead(addr);
		MemoryWrite(addr, value); //Dummy write
		MemoryWrite(addr, ASL(value));
	}

	void LSRAddr() {
		uint16_t addr = GetOperand();
		uint8_t value = MemoryRead(addr);
		MemoryWrite(addr, value); //Dummy write
		MemoryWrite(addr, LSR(value));
	}

	void ROLAddr() {
		uint16_t addr = GetOperand();
		uint8_t value = MemoryRead(addr);
		MemoryWrite(addr, value); //Dummy write
		MemoryWrite(addr, ROL(value));
	}

	void RORAddr() {
		uint16_t addr = GetOperand();
		uint8_t value = MemoryRead(addr);
		MemoryWrite(addr, value); //Dummy write
		MemoryWrite(addr, ROR(value));
	}

	void JMP(uint16_t addr) {
		SetPC(addr);
	}

	void BranchRelative(bool branch) {
		int8_t offset = (int8_t)GetOperand();
		if(branch) {
			if(CheckPageCrossed(PC(), offset)) {
				DummyRead();
			}
			DummyRead();

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

	bool IsPageCrossed() {
		bool pageCrossed = _pageCrossed;
		_pageCrossed = false;
		return pageCrossed;
	}

	#pragma region OP Codes
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
		uint8_t flags = PS() | PSFlags::Break;
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

	void BRK() {
		Push((uint16_t)(PC() + 1));

		uint8_t flags = PS() | PSFlags::Break;
		if(_state.NMIFlag) {
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
		DummyRead();  //fetch opcode (and discard it - $00 (BRK) is forced into the opcode register instead)
		DummyRead();  //read next instruction byte (actually the same as above, since PC increment is suppressed. Also discarded.)
		Push((uint16_t)(PC()));
		Push((uint8_t)PS());
		SetFlags(PSFlags::Interrupt);
		SetPC(MemoryReadWord(CPU::NMIVector));
	}

	void IRQ() {
		DummyRead();  //fetch opcode (and discard it - $00 (BRK) is forced into the opcode register instead)
		DummyRead();  //read next instruction byte (actually the same as above, since PC increment is suppressed. Also discarded.)
		Push((uint16_t)(PC()));

		if(_state.NMIFlag) {
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
		DummyRead();
		SetPS(Pop());
		SetPC(PopWord());
	}

	void NOP() {
	}
		
	#pragma endregion

protected:
	void StreamState(bool saving);

public:
	static const uint32_t ClockRate = 1789773;

	CPU(MemoryManager *memoryManager);
	static int32_t GetCycleCount() { return CPU::Instance->_cycleCount; }
	static int32_t GetRelativeCycleCount() { return CPU::Instance->_relativeCycleCount + CPU::Instance->_cycleCount; }
	static void SetNMIFlag() { CPU::Instance->_state.NMIFlag = true; }
	static void ClearNMIFlag() { CPU::Instance->_state.NMIFlag = false; }
	static void SetIRQSource(IRQSource source) { CPU::Instance->_state.IRQFlag |= (int)source; }
	static void ClearIRQSource(IRQSource source) { CPU::Instance->_state.IRQFlag &= ~(int)source; }
	static void RunDMATransfer(uint8_t* spriteRAM, uint32_t &spriteRamAddr, uint8_t offsetValue);

	void Reset(bool softReset);
	uint32_t Exec();
	void EndFrame();

	State GetState() { return _state; }
};