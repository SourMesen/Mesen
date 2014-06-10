#include "stdafx.h"

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

class Core
{
private:
	typedef void(Core::*Func)();

	Func _opTable[256];
	State _state;
	int8_t* _memory;
	uint16_t _currentPC;

	inline uint8_t Core::ReadByte()
	{
		return _memory[_state.PC++];
	}

	uint16_t Core::ReadWord()
	{
		uint16_t value = (uint16_t)(((uint8_t)_memory[_state.PC] | (((uint8_t)_memory[_state.PC + 1]) << 8)));
		_state.PC += 2;
		return value;
	}

	void ClearFlags(uint8_t flags)
	{
		_state.PS &= ~flags;
	}

	uint16_t lastNegative = 0;
	void SetFlags(uint8_t flags)
	{
		if(flags & PSFlags::Negative) {
			lastNegative = _state.PC;
		}
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

	void MemoryWrite(uint16_t addr, uint8_t value)
	{
		//SetZeroNegativeFlags(value);
		_memory[addr] = value;
		if(addr == 0x200) {
			std::cout << "------------------" << std::endl;
			std::cout << "TEST NUMBER: " << std::dec << (int)value << std::endl;
			std::cout << "------------------" << std::endl;
		} else {
			//std::cout << "(0x" << std::hex << (short)_currentPC << ") W: 0x" << std::hex << (short)addr << " = 0x" << std::hex << (short)value << std::endl;
		}
	}

	uint8_t MemoryRead(uint16_t addr) {
		//std::cout << "\t\t\t\t(0x" << std::hex << (short)_currentPC << ") R: 0x" << std::hex << (short)addr << " = 0x" << std::hex << (short)_memory[addr] << std::endl;
		return _memory[addr];
	}

	uint16_t MemoryReadWord(uint16_t addr) {
		return (_memory[addr] | (_memory[addr + 1] << 8));
	}

	void SetRegister(uint8_t &reg, int8_t value) {
		ClearFlags(PSFlags::Zero | PSFlags::Negative);
		SetZeroNegativeFlags(value);
		reg = value;
	}

	void Push(uint8_t value) {
		_memory[SP() + 0x100] = value;
		SetSP(SP() - 1);
	}

	void Push(uint16_t value) {
		Push((uint8_t)(value >> 8));
		Push((uint8_t)value);
	}

	uint8_t Pop() {
		SetSP(SP() + 1);
		int8_t value = _memory[0x100 + SP()];
		return value;
	}

	uint16_t PopWord() {
		return Pop() | (Pop() << 8);
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
	void SetPS(uint8_t value) { _state.PS = value | PSFlags::Reserved; }
	uint16_t PC() { return _state.PC; }
	void SetPC(uint16_t value) { _state.PC = value; }

	uint8_t GetImmediate() { return ReadByte(); }
	uint8_t GetZero() { return MemoryRead(GetZeroAddr()); }
	uint8_t GetZeroAddr() { return ReadByte(); }

	uint8_t GetZeroX() { return MemoryRead(GetZeroXAddr()); }
	uint8_t GetZeroXAddr() { return ReadByte() + X(); }

	int8_t GetZeroY() { return MemoryRead(GetZeroYAddr()); }
	int8_t GetZeroYAddr() { return ReadByte() + Y(); }


	int8_t GetAbs() { return MemoryRead(GetAbsAddr()); }
	uint16_t GetAbsAddr() { return ReadWord(); }

	int8_t GetAbsX() { return MemoryRead(GetAbsXAddr()); }
	uint16_t GetAbsXAddr() {
		return ReadWord() + X();
	}

	int8_t GetAbsY() { return MemoryRead(GetAbsYAddr()); }
	uint16_t GetAbsYAddr() {
		return ReadWord() + Y();
	}

	uint16_t GetInd() { return MemoryReadWord(ReadByte() | (ReadByte() << 8)); }

	int8_t GetIndX() { return MemoryRead(GetIndXAddr()); }
	uint16_t GetIndXAddr() {
		uint8_t zero = ReadByte() + X();
		//std::cout << (int)zero << std::endl;
		uint16_t addr = MemoryRead(zero) | (MemoryRead(zero + 1) << 8);
		return addr;
	}

	int8_t GetIndY() { return MemoryRead(GetIndYAddr()); }
	uint16_t GetIndYAddr() {
		uint8_t zero = ReadByte();
		//std::cout << (int)zero << std::endl;
		uint16_t addr = MemoryRead(zero) | (MemoryRead(zero + 1) << 8);
		return addr + Y();
	}


	void AND(int8_t value) { SetA(A() & value); }
	void XOR(int8_t value) {
		SetA(A() ^ value);
	}
	void OR(int8_t value) { SetA(A() | value); }

	void ADC(uint8_t value) {
		uint16_t result = (uint16_t)A() + (uint16_t)value + (CheckFlag(PSFlags::Carry) ? PSFlags::Carry : 0x00);
		if(result == 0x100) {
			//std::cout << std::hex << (short)A() << " + " << (short)value << (CheckFlag(PSFlags::Carry) ? "(+1) " : "") << " = " << (short)result << std::endl;
		}

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

	void SBC(int8_t value) {
		ADC(value ^ 0xFF);
		/*uint8_t result = A() - (value - ~(CheckFlag(PSFlags::Carry) ? PSFlags::Carry : 0x00));

		ClearFlags(PSFlags::Carry | PSFlags::Negative | PSFlags::Overflow | PSFlags::Zero);

		if (result & 0x80) {
		SetFlags(PSFlags::Overflow);
		}

		SetA(result);*/
	}

	void CMP(uint8_t reg, uint8_t value) {
		ClearFlags(PSFlags::Carry | PSFlags::Negative | PSFlags::Zero);
		if(reg >= value) {
			SetFlags(PSFlags::Carry);
		}
		if(reg == value) {
			SetFlags(PSFlags::Zero);
		}
		if(reg < value) {
			SetFlags(PSFlags::Negative);
		}
	}

	void CPA(int8_t value) { CMP(A(), value); }
	void CPX(int8_t value) { CMP(X(), value); }
	void CPY(int8_t value) { CMP(Y(), value); }

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
		int8_t value = MemoryRead(addr);
		MemoryWrite(addr, ASL(value));
	}

	void LSRAddr(uint16_t addr) {
		int8_t value = MemoryRead(addr);
		MemoryWrite(addr, LSR(value));
	}

	void ROLAddr(uint16_t addr) {
		int8_t value = MemoryRead(addr);
		MemoryWrite(addr, ROL(value));
	}

	void RORAddr(uint16_t addr) {
		int8_t value = MemoryRead(addr);
		MemoryWrite(addr, ROR(value));
	}

	void JMP(uint16_t addr) {
		_state.PC = addr;
	}

	void BranchRelative(bool branch) {
		int8_t offset = GetImmediate();
		if(branch) {
			SetPC(PC() + offset);
			if(_currentPC == PC()) {
				std::cout << "Infinite loop at: 0x" << std::hex << (short)_currentPC;
				std::cout << std::endl;
				Reset();
			}
		}
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

	void STA_Zero() { MemoryWrite(ReadByte(), A()); }
	void STA_ZeroX() { MemoryWrite((uint8_t)(ReadByte() + X()), A()); }
	void STA_Abs() { MemoryWrite(ReadWord(), A()); }
	void STA_AbsX() { MemoryWrite(GetAbsXAddr(), A()); }
	void STA_AbsY() { MemoryWrite(GetAbsYAddr(), A()); }
	void STA_IndX() { MemoryWrite(GetIndXAddr(), A()); }
	void STA_IndY() { MemoryWrite(GetIndYAddr(), A()); }

	void STX_Zero() { MemoryWrite(ReadByte(), X()); }
	void STX_ZeroY() { MemoryWrite((uint8_t)(ReadByte() + Y()), X()); }
	void STX_Abs() { MemoryWrite(ReadWord(), X()); }

	void STY_Zero() { MemoryWrite(ReadByte(), Y()); }
	void STY_ZeroX() { MemoryWrite((uint8_t)(ReadByte() + X()), Y()); }
	void STY_Abs() { MemoryWrite(ReadWord(), Y()); }

	void TAX() { SetX(A()); }
	void TAY() { SetY(A()); }
	void TSX() { SetX(SP()); }
	void TXA() { SetA(X()); }
	void TXS() { SetSP(X()); }
	void TYA() { SetA(Y()); }

	void PHA() { Push(A()); }
	void PHP() {
		SetFlags(PSFlags::Break);
		Push((uint8_t)PS());
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
		auto currentPC = _state.PC - 1;
		auto newPC = ReadWord();
		if(currentPC == newPC) {
			std::cout << "Infinite loop at: " << std::hex << (short)newPC << std::endl;
		}
		JMP(newPC);
	}
	void JMP_Ind() { JMP(GetInd()); }
	void JSR() {
		Push((uint16_t)(PC() + 1));
		JMP(GetAbsAddr());
	}
	void RTS() {
		SetPC(PopWord() + 1);
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
		SetFlags(PSFlags::Break);
		Push((uint16_t)(PC() + 1));
		Push((uint8_t)PS());
		SetFlags(PSFlags::Interrupt);
		ClearFlags(PSFlags::Break);
		SetPC((uint8_t)_memory[0xFFFE] | ((uint8_t)_memory[0xFFFF] << 8));
	}

	void NOP() {}
	void RTI() {
		SetPS(Pop());
		SetPC(PopWord());
	}
#pragma endregion
public:
	Core(int8_t *memory) : _memory(memory) {
		Reset();
		memset(_opTable, 0, 256 * sizeof(void*));
		_opTable[0x00] = &Core::BRK;
		_opTable[0x01] = &Core::ORA_IndX;
		_opTable[0x05] = &Core::ORA_Zero;
		_opTable[0x06] = &Core::ASL_Zero;
		_opTable[0x08] = &Core::PHP;
		_opTable[0x09] = &Core::ORA_Imm;
		_opTable[0x0A] = &Core::ASL_Acc;
		_opTable[0x0D] = &Core::ORA_Abs;
		_opTable[0x0E] = &Core::ASL_Abs;
		_opTable[0x10] = &Core::BPL;
		_opTable[0x11] = &Core::ORA_IndY;
		_opTable[0x15] = &Core::ORA_ZeroX;
		_opTable[0x16] = &Core::ASL_ZeroX;
		_opTable[0x18] = &Core::CLC;
		_opTable[0x19] = &Core::ORA_AbsY;
		_opTable[0x1D] = &Core::ORA_AbsX;
		_opTable[0x1E] = &Core::ASL_AbsX;
		_opTable[0x20] = &Core::JSR;
		_opTable[0x21] = &Core::AND_IndX;
		_opTable[0x24] = &Core::BIT_Zero;
		_opTable[0x25] = &Core::AND_Zero;
		_opTable[0x26] = &Core::ROL_Zero;
		_opTable[0x28] = &Core::PLP;
		_opTable[0x29] = &Core::AND_Imm;
		_opTable[0x2A] = &Core::ROL_Acc;
		_opTable[0x2C] = &Core::BIT_Abs;
		_opTable[0x2D] = &Core::AND_Abs;
		_opTable[0x2E] = &Core::ROL_Abs;
		_opTable[0x30] = &Core::BMI;
		_opTable[0x31] = &Core::AND_IndY;
		_opTable[0x35] = &Core::AND_ZeroX;
		_opTable[0x36] = &Core::ROL_ZeroX;
		_opTable[0x38] = &Core::SEC;
		_opTable[0x39] = &Core::AND_AbsY;
		_opTable[0x3D] = &Core::AND_AbsX;
		_opTable[0x3E] = &Core::ROL_AbsX;
		_opTable[0x40] = &Core::RTI;
		_opTable[0x41] = &Core::EOR_IndX;
		_opTable[0x45] = &Core::EOR_Zero;
		_opTable[0x46] = &Core::LSR_Zero;
		_opTable[0x48] = &Core::PHA;
		_opTable[0x49] = &Core::EOR_Imm;
		_opTable[0x4A] = &Core::LSR_Acc;
		_opTable[0x4C] = &Core::JMP_Abs;
		_opTable[0x4D] = &Core::EOR_Abs;
		_opTable[0x4E] = &Core::LSR_Abs;
		_opTable[0x50] = &Core::BVC;
		_opTable[0x51] = &Core::EOR_IndY;
		_opTable[0x55] = &Core::EOR_ZeroX;
		_opTable[0x56] = &Core::LSR_ZeroX;
		_opTable[0x58] = &Core::CLI;
		_opTable[0x59] = &Core::EOR_AbsY;
		_opTable[0x5D] = &Core::EOR_AbsX;
		_opTable[0x5E] = &Core::LSR_AbsX;
		_opTable[0x60] = &Core::RTS;
		_opTable[0x61] = &Core::ADC_IndX;
		_opTable[0x65] = &Core::ADC_Zero;
		_opTable[0x66] = &Core::ROR_Zero;
		_opTable[0x68] = &Core::PLA;
		_opTable[0x69] = &Core::ADC_Imm;
		_opTable[0x6A] = &Core::ROR_Acc;
		_opTable[0x6C] = &Core::JMP_Ind;
		_opTable[0x6D] = &Core::ADC_Abs;
		_opTable[0x6E] = &Core::ROR_Abs;
		_opTable[0x70] = &Core::BVS;
		_opTable[0x71] = &Core::ADC_IndY;
		_opTable[0x75] = &Core::ADC_ZeroX;
		_opTable[0x76] = &Core::ROR_ZeroX;
		_opTable[0x78] = &Core::SEI;
		_opTable[0x79] = &Core::ADC_AbsY;
		_opTable[0x7D] = &Core::ADC_AbsX;
		_opTable[0x7E] = &Core::ROR_AbsX;
		_opTable[0x81] = &Core::STA_IndX;
		_opTable[0x84] = &Core::STY_Zero;
		_opTable[0x85] = &Core::STA_Zero;
		_opTable[0x86] = &Core::STX_Zero;
		_opTable[0x88] = &Core::DEY;
		_opTable[0x8A] = &Core::TXA;
		_opTable[0x8C] = &Core::STY_Abs;
		_opTable[0x8D] = &Core::STA_Abs;
		_opTable[0x8E] = &Core::STX_Abs;
		_opTable[0x90] = &Core::BCC;
		_opTable[0x91] = &Core::STA_IndY;
		_opTable[0x94] = &Core::STY_ZeroX;
		_opTable[0x95] = &Core::STA_ZeroX;
		_opTable[0x96] = &Core::STX_ZeroY;
		_opTable[0x98] = &Core::TYA;
		_opTable[0x99] = &Core::STA_AbsY;
		_opTable[0x9A] = &Core::TXS;
		_opTable[0x9D] = &Core::STA_AbsX;
		_opTable[0xA0] = &Core::LDY_Imm;
		_opTable[0xA1] = &Core::LDA_IndX;
		_opTable[0xA2] = &Core::LDX_Imm;
		_opTable[0xA4] = &Core::LDY_Zero;
		_opTable[0xA5] = &Core::LDA_Zero;
		_opTable[0xA6] = &Core::LDX_Zero;
		_opTable[0xA8] = &Core::TAY;
		_opTable[0xA9] = &Core::LDA_Imm;
		_opTable[0xAA] = &Core::TAX;
		_opTable[0xAC] = &Core::LDY_Abs;
		_opTable[0xAD] = &Core::LDA_Abs;
		_opTable[0xAE] = &Core::LDX_Abs;
		_opTable[0xB0] = &Core::BCS;
		_opTable[0xB1] = &Core::LDA_IndY;
		_opTable[0xB4] = &Core::LDY_ZeroX;
		_opTable[0xB5] = &Core::LDA_ZeroX;
		_opTable[0xB6] = &Core::LDX_ZeroY;
		_opTable[0xB8] = &Core::CLV;
		_opTable[0xB9] = &Core::LDA_AbsY;
		_opTable[0xBA] = &Core::TSX;
		_opTable[0xBC] = &Core::LDY_AbsX;
		_opTable[0xBD] = &Core::LDA_AbsX;
		_opTable[0xBE] = &Core::LDX_AbsY;
		_opTable[0xC0] = &Core::CPY_Imm;
		_opTable[0xC1] = &Core::CMP_IndX;
		_opTable[0xC4] = &Core::CPY_Zero;
		_opTable[0xC5] = &Core::CMP_Zero;
		_opTable[0xC6] = &Core::DEC_Zero;
		_opTable[0xC8] = &Core::INY;
		_opTable[0xC9] = &Core::CMP_Imm;
		_opTable[0xCA] = &Core::DEX;
		_opTable[0xCC] = &Core::CPY_Abs;
		_opTable[0xCD] = &Core::CMP_Abs;
		_opTable[0xCE] = &Core::DEC_Abs;
		_opTable[0xD0] = &Core::BNE;
		_opTable[0xD1] = &Core::CMP_IndY;
		_opTable[0xD5] = &Core::CMP_ZeroX;
		_opTable[0xD6] = &Core::DEC_ZeroX;
		_opTable[0xD8] = &Core::CLD;
		_opTable[0xD9] = &Core::CMP_AbsY;
		_opTable[0xDD] = &Core::CMP_AbsX;
		_opTable[0xDE] = &Core::DEC_AbsX;
		_opTable[0xE0] = &Core::CPX_Imm;
		_opTable[0xE1] = &Core::SBC_IndX;
		_opTable[0xE4] = &Core::CPX_Zero;
		_opTable[0xE5] = &Core::SBC_Zero;
		_opTable[0xE6] = &Core::INC_Zero;
		_opTable[0xE8] = &Core::INX;
		_opTable[0xE9] = &Core::SBC_Imm;
		_opTable[0xEA] = &Core::NOP;
		_opTable[0xEC] = &Core::CPX_Abs;
		_opTable[0xED] = &Core::SBC_Abs;
		_opTable[0xEE] = &Core::INC_Abs;
		_opTable[0xF0] = &Core::BEQ;
		_opTable[0xF1] = &Core::SBC_IndY;
		_opTable[0xF5] = &Core::SBC_ZeroX;
		_opTable[0xF6] = &Core::INC_ZeroX;
		_opTable[0xF8] = &Core::SED;
		_opTable[0xF9] = &Core::SBC_AbsY;
		_opTable[0xFD] = &Core::SBC_AbsX;
		_opTable[0xFE] = &Core::INC_AbsX;
	}
	void Reset();
	void Exec();
};
