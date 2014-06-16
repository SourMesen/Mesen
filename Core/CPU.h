#pragma once

#include "stdafx.h"
#include "MemoryManager.h"

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

struct State
{
	uint16_t PC;
	uint8_t SP;
	uint8_t A;
	uint8_t X;
	uint8_t Y;
	uint8_t PS;
};

class CPU
{
private:
	typedef void(CPU::*Func)();

	Func _opTable[256];
	uint8_t _cycles[256];

	State _state;

	MemoryManager *_memoryManager = nullptr;

	static uint64_t CycleCount;
	static bool NMIFlag;

	uint16_t _currentPC = 0;
	uint8_t _cyclePenalty = 0;

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

	bool IsPageCrossed(uint16_t valA, uint8_t valB)
	{
		return (uint8_t)valA + valB >= 0x100;
	}

	void MemoryWrite(uint16_t addr, uint8_t value)
	{
		_memoryManager->Write(addr, value);
		/*if(addr == 0x200) {
			std::cout << "------------------" << std::endl;
			std::cout << "(0x" << std::hex << (short)_currentPC << ") TEST NUMBER: " << std::dec << (int)value << std::endl;
			std::cout << "------------------" << std::endl;
		} else {
			//std::cout << "(0x" << std::hex << (short)_currentPC << ") W: 0x" << std::hex << (short)addr << " = 0x" << std::hex << (short)value << std::endl;
		}*/
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
	void SetPS(uint8_t value) { _state.PS = (value & 0xDF) | PSFlags::Reserved; }
	uint16_t PC() { return _state.PC; }
	void SetPC(uint16_t value) { _state.PC = value; }

	uint8_t GetImmediate() { return ReadByte(); }
	uint8_t GetZero() { return MemoryRead(GetZeroAddr()); }
	uint8_t GetZeroAddr() { return ReadByte(); }

	uint8_t GetZeroX() { return MemoryRead(GetZeroXAddr()); }
	uint8_t GetZeroXAddr() { return ReadByte() + X(); }

	uint8_t GetZeroY() { return MemoryRead(GetZeroYAddr()); }
	uint8_t GetZeroYAddr() { return ReadByte() + Y(); }
	
	uint8_t GetAbs() { return MemoryRead(GetAbsAddr()); }
	uint16_t GetAbsAddr() { return ReadWord(); }

	uint8_t GetAbsX() { return MemoryRead(GetAbsXAddr()); }
	uint16_t GetAbsXAddr() { 
		uint16_t baseAddr = ReadWord();
		if(IsPageCrossed(baseAddr, X())) {
			SetCyclePenalty(1);
		}
		return baseAddr + X(); 
	}

	uint8_t GetAbsY() { return MemoryRead(GetAbsYAddr()); }
	uint16_t GetAbsYAddr() { 
		uint16_t baseAddr = ReadWord();
		if(IsPageCrossed(baseAddr, Y())) {
			SetCyclePenalty(1);
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
		uint8_t zero = ReadByte() + X();
		uint16_t addr;
		if(zero == 0xFF) {
			addr = MemoryRead(0xFF) | MemoryReadWord(0x00) << 8;
		} else {
			addr = MemoryReadWord(zero);
		}
		return addr;
	}

	uint8_t GetIndY() { return MemoryRead(GetIndYAddr()); }
	uint16_t GetIndYAddr() {
		uint8_t zero = ReadByte();
		uint16_t addr;
		if(zero == 0xFF) {
			addr = MemoryRead(0xFF) | MemoryReadWord(0x00) << 8;
		} else {
			addr = MemoryReadWord(zero);
		}
		if(IsPageCrossed(addr, Y())) {
			SetCyclePenalty(1);
		}
		return addr + Y();
	}

	void AND(uint8_t value) { SetA(A() & value); }
	void XOR(uint8_t value) { SetA(A() ^ value); }
	void OR(uint8_t value) { SetA(A() | value); }

	void ADC(uint8_t value) {
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

	void SBC(uint8_t value) { ADC(value ^ 0xFF); }

	void CMP(uint8_t reg, uint8_t value) {
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

	void CPA(uint8_t value) { CMP(A(), value); }
	void CPX(uint8_t value) { CMP(X(), value); }
	void CPY(uint8_t value) { CMP(Y(), value); }

	void INC(uint16_t addr) {
		ClearFlags(PSFlags::Negative | PSFlags::Zero);
		uint8_t memory = MemoryRead(addr) + 1;
		SetZeroNegativeFlags(memory);
		MemoryWrite(addr, memory);
	}

	void DEC(uint16_t addr) {
		ClearFlags(PSFlags::Negative | PSFlags::Zero);
		uint8_t memory = MemoryRead(addr) - 1;
		SetZeroNegativeFlags(memory);
		MemoryWrite(addr, memory);
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

	void ASLAddr(uint16_t addr) {
		uint8_t value = MemoryRead(addr);
		MemoryWrite(addr, ASL(value));
	}

	void LSRAddr(uint16_t addr) {
		uint8_t value = MemoryRead(addr);
		MemoryWrite(addr, LSR(value));
	}

	void ROLAddr(uint16_t addr) {
		uint8_t value = MemoryRead(addr);
		MemoryWrite(addr, ROL(value));
	}

	void RORAddr(uint16_t addr) {
		uint8_t value = MemoryRead(addr);
		MemoryWrite(addr, ROR(value));
	}

	void JMP(uint16_t addr) {
		//std::cout << "JMP from 0x" << std::hex << _currentPC << " to " << addr << std::endl;
		SetPC(addr);
	}

	void BranchRelative(bool branch) {
		int8_t offset = GetImmediate();
		if(branch) {
			if(IsPageCrossed(PC(), offset)) {
				SetCyclePenalty(2);
			} else {
				SetCyclePenalty(1);
			}
			SetPC(PC() + offset);

			if(_currentPC == PC()) {
				if(_currentPC != 0x33a7) {
					std::cout << "Infinite loop at: 0x" << std::hex << (short)_currentPC;
					std::cout << std::endl;
				} else {
					Reset();
				}
			}
		}
	}

	void BIT(uint8_t value) {
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

	void SetCyclePenalty(uint8_t penalty) {
		_cyclePenalty = penalty;
	}

	uint8_t GetCyclePenalty() {
		uint8_t penalty = _cyclePenalty;
		_cyclePenalty = 0;
		return penalty;
	}

	#pragma region OP Codes
	void LDA_Imm() { SetA(GetImmediate()); }
	void LDA_Zero() { SetA(GetZero()); }
	void LDA_ZeroX() { SetA(GetZeroX()); }
	void LDA_Abs() { SetA(GetAbs()); }
	void LDA_AbsX() { SetA(GetAbsX()); }
	void LDA_AbsY() { SetA(GetAbsY()); }
	void LDA_IndX() { SetA(GetIndX()); }
	void LDA_IndY() { SetA(GetIndY()); }

	void LDX_Imm() { SetX(GetImmediate()); }
	void LDX_Zero() { SetX(GetZero()); }
	void LDX_ZeroY() { SetX(GetZeroY()); }
	void LDX_Abs() { SetX(GetAbs()); }
	void LDX_AbsY() { SetX(GetAbsY()); }

	void LDY_Imm() { SetY(GetImmediate()); }
	void LDY_Zero() { SetY(GetZero()); }
	void LDY_ZeroX() { SetY(GetZeroX()); }
	void LDY_Abs() { SetY(GetAbs()); }
	void LDY_AbsX() { SetY(GetAbsX()); }

	void STA_Zero() { MemoryWrite(GetZeroAddr(), A()); }
	void STA_ZeroX() { MemoryWrite(GetZeroXAddr(), A()); }
	void STA_Abs() { MemoryWrite(GetAbsAddr(), A()); }
	void STA_AbsX() { MemoryWrite(GetAbsXAddr(), A()); }
	void STA_AbsY() { MemoryWrite(GetAbsYAddr(), A()); }
	void STA_IndX() { MemoryWrite(GetIndXAddr(), A()); }
	void STA_IndY() { MemoryWrite(GetIndYAddr(), A()); }

	void STX_Zero() { MemoryWrite(GetZeroAddr(), X()); }
	void STX_ZeroY() { MemoryWrite(GetZeroYAddr(), X()); }
	void STX_Abs() { MemoryWrite(GetAbsAddr(), X()); }

	void STY_Zero() { MemoryWrite(GetZeroAddr(), Y()); }
	void STY_ZeroX() { MemoryWrite(GetZeroXAddr(), Y()); }
	void STY_Abs() { MemoryWrite(GetAbsAddr(), Y()); }

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

	void AND_Imm() { AND(GetImmediate()); }
	void AND_Zero() { AND(GetZero()); }
	void AND_ZeroX() { AND(GetZeroX()); }
	void AND_Abs() { AND(GetAbs()); }
	void AND_AbsX() { AND(GetAbsX()); }
	void AND_AbsY() { AND(GetAbsY()); }
	void AND_IndX() { AND(GetIndX()); }
	void AND_IndY() { AND(GetIndY()); }

	void EOR_Imm() { XOR(GetImmediate()); }
	void EOR_Zero() { XOR(GetZero()); }
	void EOR_ZeroX() { XOR(GetZeroX()); }
	void EOR_Abs() { XOR(GetAbs()); }
	void EOR_AbsX() { XOR(GetAbsX()); }
	void EOR_AbsY() { XOR(GetAbsY()); }
	void EOR_IndX() { XOR(GetIndX()); }
	void EOR_IndY() { XOR(GetIndY()); }

	void ORA_Imm() { OR(GetImmediate()); }
	void ORA_Zero() { OR(GetZero()); }
	void ORA_ZeroX() { OR(GetZeroX()); }
	void ORA_Abs() { OR(GetAbs()); }
	void ORA_AbsX() { OR(GetAbsX()); }
	void ORA_AbsY() { OR(GetAbsY()); }
	void ORA_IndX() { OR(GetIndX()); }
	void ORA_IndY() { OR(GetIndY()); }

	void BIT_Zero() {
		BIT(GetZero());
	}
	void BIT_Abs() {
		BIT(GetAbs());
	}

	void ADC_Imm() { ADC(GetImmediate()); }
	void ADC_Zero() { ADC(GetZero()); }
	void ADC_ZeroX() { ADC(GetZeroX()); }
	void ADC_Abs() { ADC(GetAbs()); }
	void ADC_AbsX() { ADC(GetAbsX()); }
	void ADC_AbsY() { ADC(GetAbsY()); }
	void ADC_IndX() { ADC(GetIndX()); }
	void ADC_IndY() { ADC(GetIndY()); }

	void SBC_Imm() { SBC(GetImmediate()); }
	void SBC_Zero() { SBC(GetZero()); }
	void SBC_ZeroX() { SBC(GetZeroX()); }
	void SBC_Abs() { SBC(GetAbs()); }
	void SBC_AbsX() { SBC(GetAbsX()); }
	void SBC_AbsY() { SBC(GetAbsY()); }
	void SBC_IndX() { SBC(GetIndX()); }
	void SBC_IndY() { SBC(GetIndY()); }

	void CMP_Imm() { CPA(GetImmediate()); }
	void CMP_Zero() { CPA(GetZero()); }
	void CMP_ZeroX() { CPA(GetZeroX()); }
	void CMP_Abs() { CPA(GetAbs()); }
	void CMP_AbsX() { CPA(GetAbsX()); }
	void CMP_AbsY() { CPA(GetAbsY()); }
	void CMP_IndX() { CPA(GetIndX()); }
	void CMP_IndY() { CPA(GetIndY()); }

	void CPX_Imm() { CPX(GetImmediate()); }
	void CPX_Zero() { CPX(GetZero()); }
	void CPX_Abs() { CPX(GetAbs()); }

	void CPY_Imm() { CPY(GetImmediate()); }
	void CPY_Zero() { CPY(GetZero()); }
	void CPY_Abs() { CPY(GetAbs()); }

	void INC_Zero() { INC(GetZeroAddr()); }
	void INC_ZeroX() { INC(GetZeroXAddr()); }
	void INC_Abs() { INC(GetAbsAddr()); }
	void INC_AbsX() { INC(GetAbsXAddr()); }
	void INX() { SetX(X() + 1); }
	void INY() { SetY(Y() + 1); }

	void DEC_Zero() { DEC(GetZeroAddr()); }
	void DEC_ZeroX() { DEC(GetZeroXAddr()); }
	void DEC_Abs() { DEC(GetAbsAddr()); }
	void DEC_AbsX() { DEC(GetAbsXAddr()); }
	void DEX() { SetX(X() - 1); }
	void DEY() { SetY(Y() - 1); }

	void ASL_Acc() { SetA(ASL(A())); }
	void ASL_Zero() { ASLAddr(GetZeroAddr()); }
	void ASL_ZeroX() { ASLAddr(GetZeroXAddr()); }
	void ASL_Abs() { ASLAddr(GetAbsAddr()); }
	void ASL_AbsX() { ASLAddr(GetAbsXAddr()); }

	void LSR_Acc() { SetA(LSR(A())); }
	void LSR_Zero() { LSRAddr(GetZeroAddr()); }
	void LSR_ZeroX() { LSRAddr(GetZeroXAddr()); }
	void LSR_Abs() { LSRAddr(GetAbsAddr()); }
	void LSR_AbsX() { LSRAddr(GetAbsXAddr()); }

	void ROL_Acc() { SetA(ROL(A())); }
	void ROL_Zero() { ROLAddr(GetZeroAddr()); }
	void ROL_ZeroX() { ROLAddr(GetZeroXAddr()); }
	void ROL_Abs() { ROLAddr(GetAbsAddr()); }
	void ROL_AbsX() { ROLAddr(GetAbsXAddr()); }

	void ROR_Acc() { SetA(ROR(A())); }
	void ROR_Zero() { RORAddr(GetZeroAddr()); }
	void ROR_ZeroX() { RORAddr(GetZeroXAddr()); }
	void ROR_Abs() { RORAddr(GetAbsAddr()); }
	void ROR_AbsX() { RORAddr(GetAbsXAddr()); }

	void JMP_Abs() {
		JMP(GetAbsAddr());
	}
	void JMP_Ind() { JMP(GetInd()); }
	void JSR() {
		uint16_t addr = GetAbsAddr();
		//std::cout << "JSR from 0x" << std::hex << _currentPC << " to " << addr;
		Push((uint16_t)(PC() - 1));
		JMP(addr);
	}
	void RTS() {
		uint16_t addr = PopWord();
		//std::cout << "RTS from 0x" << std::hex << _currentPC << " to " << addr;
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
		Push((uint8_t)flags);
		SetFlags(PSFlags::Interrupt);

		SetPC(MemoryReadWord(0xFFFE));
	}

	void NMI() {
		Push((uint16_t)(PC() + 1));
		Push((uint8_t)PS());
		SetFlags(PSFlags::Interrupt);
		SetPC(MemoryReadWord(0xFFFA));
	}


	void NOP() {}
	void RTI() {
		SetPS(Pop());
		SetPC(PopWord());
	}
	#pragma endregion

public:
	CPU(MemoryManager *memoryManager);
	static uint64_t GetCycleCount() { return CPU::CycleCount; }
	static void SetNMIFlag() { CPU::NMIFlag = true; }
	void Reset();
	void Exec();
	State GetState() { return _state; }

};