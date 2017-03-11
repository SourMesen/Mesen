#include "stdafx.h"
#include <regex>
#include <unordered_map>
#include "../Utilities/HexUtilities.h"
#include "../Utilities/StringUtilities.h"
#include "Assembler.h"
#include "CPU.h"
#include "DisassemblyInfo.h"
#include "LabelManager.h"

static const std::regex instRegex = std::regex("^\\s*([a-z]{3})[*]{0,1}[\\s]*([#]{0,1})([(]{0,1})[\\s]*([$]{0,1})([^,)(;:]*)[\\s]*((,x\\)|\\),y|,x|,y|\\)){0,1})\\s*(;*)(.*)", std::regex_constants::icase);
static const std::regex isCommentOrBlank = std::regex("^\\s*([;]+.*$|\\s*$)", std::regex_constants::icase);
static const std::regex labelRegex = std::regex("^\\s*([@_a-z][@_a-z0-9]*):(.*)", std::regex_constants::icase);
static const std::regex byteRegex = std::regex("^\\s*[.]byte\\s+((\\$[a-f0-9]{1,2},)*)(\\$[a-f0-9]{1,2})+\\s*(;*)(.*)$", std::regex_constants::icase);

static string opName[256] = {
	//	0			1			2			3			4			5			6			7			8			9			A			B			C			D			E			F
	"BRK",	"ORA",	"STP",	"SLO",	"NOP",	"ORA",	"ASL",	"SLO",	"PHP",	"ORA",	"ASL",	"ANC",	"NOP",	"ORA",	"ASL",	"SLO", //0
	"BPL",	"ORA",	"STP",	"SLO",	"NOP",	"ORA",	"ASL",	"SLO",	"CLC",	"ORA",	"NOP",	"SLO",	"NOP",	"ORA",	"ASL",	"SLO", //1
	"JSR",	"AND",	"STP",	"RLA",	"BIT",	"AND",	"ROL",	"RLA",	"PLP",	"AND",	"ROL",	"ANC",	"BIT",	"AND",	"ROL",	"RLA", //2
	"BMI",	"AND",	"STP",	"RLA",	"NOP",	"AND",	"ROL",	"RLA",	"SEC",	"AND",	"NOP",	"RLA",	"NOP",	"AND",	"ROL",	"RLA", //3
	"RTI",	"EOR",	"STP",	"SRE",	"NOP",	"EOR",	"LSR",	"SRE",	"PHA",	"EOR",	"LSR",	"ALR",	"JMP",	"EOR",	"LSR",	"SRE", //4
	"BVC",	"EOR",	"STP",	"SRE",	"NOP",	"EOR",	"LSR",	"SRE",	"CLI",	"EOR",	"NOP",	"SRE",	"NOP",	"EOR",	"LSR",	"SRE", //5
	"RTS",	"ADC",	"STP",	"RRA",	"NOP",	"ADC",	"ROR",	"RRA",	"PLA",	"ADC",	"ROR",	"ARR",	"JMP",	"ADC",	"ROR",	"RRA", //6
	"BVS",	"ADC",	"STP",	"RRA",	"NOP",	"ADC",	"ROR",	"RRA",	"SEI",	"ADC",	"NOP",	"RRA",	"NOP",	"ADC",	"ROR",	"RRA", //7
	"NOP",	"STA",	"NOP",	"SAX",	"STY",	"STA",	"STX",	"SAX",	"DEY",	"NOP",	"TXA",	"XAA",	"STY",	"STA",	"STX",	"SAX", //8
	"BCC",	"STA",	"STP",	"AHX",	"STY",	"STA",	"STX",	"SAX",	"TYA",	"STA",	"TXS",	"TAS",	"SHY",	"STA",	"SHX",	"AXA", //9
	"LDY",	"LDA",	"LDX",	"LAX",	"LDY",	"LDA",	"LDX",	"LAX",	"TAY",	"LDA",	"TAX",	"LAX",	"LDY",	"LDA",	"LDX",	"LAX", //A
	"BCS",	"LDA",	"STP",	"LAX",	"LDY",	"LDA",	"LDX",	"LAX",	"CLV",	"LDA",	"TSX",	"LAS",	"LDY",	"LDA",	"LDX",	"LAX", //B
	"CPY",	"CMP",	"NOP",	"DCP",	"CPY",	"CMP",	"DEC",	"DCP",	"INY",	"CMP",	"DEX",	"AXS",	"CPY",	"CMP",	"DEC",	"DCP", //C
	"BNE",	"CMP",	"STP",	"DCP",	"NOP",	"CMP",	"DEC",	"DCP",	"CLD",	"CMP",	"NOP",	"DCP",	"NOP",	"CMP",	"DEC",	"DCP", //D
	"CPX",	"SBC",	"NOP",	"ISC",	"CPX",	"SBC",	"INC",	"ISC",	"INX",	"SBC",	"NOP",	"SBC",	"CPX",	"SBC",	"INC",	"ISC", //E
	"BEQ",	"SBC",	"STP",	"ISC",	"NOP",	"SBC",	"INC",	"ISC",	"SED",	"SBC",	"NOP",	"ISC",	"NOP",	"SBC",	"INC",	"ISC"  //F
};

void Assembler::ProcessLine(string code, uint16_t &instructionAddress, vector<int16_t>& output, std::unordered_map<string, uint16_t> &labels)
{
	//Remove extra spaces as part of processing
	size_t offset = code.find_first_of(',', 0);
	if(offset != string::npos) {
		code.erase(std::remove(code.begin() + offset + 1, code.end(), ' '), code.end());
	}
	offset = code.find_first_of(')', 0);
	if(offset != string::npos) {
		code.erase(std::remove(code.begin() + offset + 1, code.end(), ' '), code.end());
	}

	//Determine if the line is blank, a comment, a label or code
	std::smatch match;
	if(std::regex_search(code, match, byteRegex)) {
		vector<string> bytes = StringUtilities::Split(match.str(1) + match.str(3), ',');
		for(string &byte : bytes) {
			output.push_back((uint8_t)(HexUtilities::FromHex(byte.substr(1))));
			instructionAddress++;
		}
		output.push_back(AssemblerSpecialCodes::EndOfLine);
	} else if(std::regex_search(code, match, labelRegex)) {
		string label = match.str(1);
		string afterLabel = match.str(2);
		labels[match.str(1)] = instructionAddress;
		ProcessLine(afterLabel, instructionAddress, output, labels);
		return;
	} else if(std::regex_search(code, match, isCommentOrBlank)) {
		output.push_back(AssemblerSpecialCodes::EndOfLine);
		return;
	} else if(std::regex_search(code, match, instRegex) && match.size() > 1) {
		LineData lineData;
		AssemblerSpecialCodes result = GetLineData(match, lineData, labels);
		if(result == AssemblerSpecialCodes::OK) {
			AssembleInstruction(lineData, instructionAddress, output);
		} else {
			output.push_back(result);
		}
	} else {
		output.push_back(AssemblerSpecialCodes::ParsingError);
	}
}

AssemblerSpecialCodes Assembler::GetLineData(std::smatch match, LineData &lineData, std::unordered_map<string, uint16_t> &labels)
{
	lineData.OpCode = match.str(1);
	lineData.IsImmediate = !match.str(2).empty();
	lineData.IsHex = !match.str(4).empty();
	lineData.HasComment = !match.str(8).empty();
	lineData.OperandSuffix = match.str(6);
	lineData.HasOpeningParenthesis = !match.str(3).empty();

	std::transform(lineData.OperandSuffix.begin(), lineData.OperandSuffix.end(), lineData.OperandSuffix.begin(), ::toupper);
	std::transform(lineData.OpCode.begin(), lineData.OpCode.end(), lineData.OpCode.begin(), ::toupper);

	bool foundSpace = false;
	for(char c : match.str(5)) {
		if(c != ' ' && c != '\t') {
			if(foundSpace) {
				//can't have spaces in operands (except at the very end)
				return AssemblerSpecialCodes::InvalidSpaces;
			} else if(lineData.IsHex && !((c >= '0' && c <= '9') || (c >= 'A' && c <= 'F') || (c >= 'a' && c <= 'f'))) {
				//invalid hex
				return AssemblerSpecialCodes::InvalidHex;
			}

			lineData.Operand.push_back(c);
		} else {
			foundSpace = true;
		}
	}

	if(!lineData.HasComment && !match.str(9).empty()) {
		//something is trailing at the end of the line, and it's not a comment
		return AssemblerSpecialCodes::TrailingText;
	}

	if(!lineData.IsHex) {
		bool allNumeric = true;
		for(int i = 0; i < lineData.Operand.size(); i++) {
			if(lineData.Operand[i] == '-' && i == 0 && lineData.Operand.size() > 1) {
				//First char is a minus sign, and more characters follow, continue
				continue;
			}

			if((lineData.Operand[i] < '0' || lineData.Operand[i] > '9')) {
				allNumeric = false;
				break;
			}
		}

		if(allNumeric && !lineData.Operand.empty()) {
			//Operand is not empty, and it only contains decimal values
			lineData.IsDecimal = true;
		} else {
			lineData.IsDecimal = false;
		}
	}

	return GetAddrModeAndOperandSize(lineData, labels);
}

AssemblerSpecialCodes Assembler::GetAddrModeAndOperandSize(LineData &lineData, std::unordered_map<string, uint16_t> &labels)
{
	int opSize = 0;
	bool invalid = false;
	string operand = lineData.Operand;

	if(lineData.IsHex) {
		if(operand.size() == 0) {
			return AssemblerSpecialCodes::MissingOperand;
		} else if(operand.size() <= 2) {
			opSize = 1;
		} else if(operand.size() <= 4) {
			opSize = 2;
		} else {
			return AssemblerSpecialCodes::OperandOutOfRange;
		}
	} else if(lineData.IsDecimal) {
		int value = std::stoi(operand.c_str());
		if(value < -32768) {
			//< -32768 is invalid
			return AssemblerSpecialCodes::OperandOutOfRange;
		} else if(value < -128) {
			//-32768 to -129 is 2 bytes
			opSize = 2;
		} else if(value <= 255) {
			//-128 to 255 is 2 bytes
			opSize = 1;
		} else if(value <= 65535) {
			//256 to 65535 is 2 bytes
			opSize = 2;
		} else {
			//> 65535 is invalid
			return AssemblerSpecialCodes::OperandOutOfRange;
		}
	} else if(!operand.empty()) {
		//Check if the operand is a known label
		auto findResult = labels.find(operand);
		if(findResult != labels.end()) {
			lineData.Operand = HexUtilities::ToHex((uint16_t)findResult->second);
			lineData.IsHex = true;
			opSize = 2;
		} else if(operand.size() == 1 && (operand[0] == 'A' || operand[0] == 'a') && lineData.OperandSuffix.empty() && !lineData.IsHex && !lineData.IsImmediate && !lineData.HasOpeningParenthesis) {
			//Allow optional "A" after AddrMode == Accumulator instructions
			lineData.Mode = AddrMode::Acc;
			opSize = 0;
		} else {
			int32_t addr = _labelManager->GetLabelRelativeAddress(operand);
			if(addr >= 256) {
				lineData.Operand = HexUtilities::ToHex((uint16_t)addr);
				lineData.IsHex = true;
				opSize = 2;
			} else if(addr >= 0) {
				lineData.Operand = HexUtilities::ToHex((uint8_t)addr);
				lineData.IsHex = true;
				opSize = 1;
			} else {
				return AssemblerSpecialCodes::UnknownLabel;
			}
		}
	} else {
		//No operand
		opSize = 0;
	}

	if(lineData.Mode == AddrMode::None) {
		if(lineData.IsImmediate) {
			if(lineData.HasOpeningParenthesis || opSize == 0) {
				invalid = true;
			}
			lineData.Mode = AddrMode::Imm; //or Rel
		} else if(lineData.HasOpeningParenthesis) {
			if(lineData.OperandSuffix.compare(")") == 0) {
				opSize = 2;
				lineData.Mode = AddrMode::Ind;
			} else if(lineData.OperandSuffix.compare(",X)") == 0) {
				opSize = 1;
				lineData.Mode = AddrMode::IndX;
			} else if(lineData.OperandSuffix.compare("),Y") == 0) {
				opSize = 1;
				lineData.Mode = AddrMode::IndY;
			} else {
				invalid = true;
			}
		} else {
			if(lineData.OperandSuffix.compare(",X") == 0) {
				if(opSize == 2) {
					lineData.Mode = AddrMode::AbsX;
				} else if(opSize == 1) {
					lineData.Mode = AddrMode::ZeroX;
				} else {
					invalid = true;
				}
			} else if(lineData.OperandSuffix.compare(",Y") == 0) {
				if(opSize == 2) {
					lineData.Mode = AddrMode::AbsY;
				} else if(opSize == 1) {
					lineData.Mode = AddrMode::ZeroY;
				} else {
					invalid = true;
				}
			} else if(lineData.OperandSuffix.empty()) {
				if(opSize == 0) {
					lineData.Mode = AddrMode::Imp; //or Acc
				} else if(opSize == 2) {
					lineData.Mode = AddrMode::Abs;
				} else if(opSize == 1) {
					lineData.Mode = AddrMode::Zero;
				} else {
					invalid = true;
				}
			} else {
				invalid = true;
			}
		}
	}

	if(lineData.Mode == AddrMode::None) {
		invalid = true;
	}

	lineData.OperandSize = opSize;

	return invalid ? AssemblerSpecialCodes::ParsingError : AssemblerSpecialCodes::OK;
}

void Assembler::AssembleInstruction(LineData &lineData, uint16_t &instructionAddress, vector<int16_t>& output)
{
	bool foundMatch = false;
	if(lineData.Mode == AddrMode::Imp && lineData.OpCode.compare("NOP") == 0) {
		//NOP has multiple name+addressing type collisions, the "official" NOP is 0xEA
		output.push_back(0xEA);
		instructionAddress++;
		foundMatch = true;
	} else {
		for(uint8_t i = 0; i < 255; i++) {
			AddrMode opMode = DisassemblyInfo::OPMode[i];
			if(lineData.OpCode.compare(opName[i]) == 0) {
				bool modeMatch = opMode == lineData.Mode;
				if(!modeMatch) {
					if(lineData.Mode == AddrMode::Imp && opMode == AddrMode::Acc ||
						lineData.Mode == AddrMode::IndY && opMode == AddrMode::IndYW ||
						lineData.Mode == AddrMode::AbsY && opMode == AddrMode::AbsYW ||
						lineData.Mode == AddrMode::AbsX && opMode == AddrMode::AbsXW) {
						modeMatch = true;
					} else if(lineData.Mode == AddrMode::Abs && opMode == AddrMode::Rel ||
						lineData.Mode == AddrMode::Imm && opMode == AddrMode::Rel) {

						if(lineData.OperandSize == 2) {
							if(lineData.Mode == AddrMode::Imm) {
								//Hardcoded jump values must be 1-byte
								output.push_back(AssemblerSpecialCodes::OutOfRangeJump);
								return;
							} else {
								modeMatch = true;

								//Convert "absolute" jump to a relative jump
								int value = lineData.IsHex ? HexUtilities::FromHex(lineData.Operand) : std::stoi(lineData.Operand);

								int16_t addressGap = value - (instructionAddress + 2);
								if(addressGap > 127 || addressGap < -128) {
									//Gap too long, can't jump that far
									output.push_back(AssemblerSpecialCodes::OutOfRangeJump);
									return;
								}

								//Update data to match relative jump
								lineData.OperandSize = 1;
								lineData.IsHex = true;
								lineData.Operand = HexUtilities::ToHex((uint8_t)addressGap);
							}
						} else {
							//Accept 1-byte relative jumps
							modeMatch = true;
						}
					}
				}

				if(modeMatch) {
					output.push_back(i);
					instructionAddress += (lineData.OperandSize + 1);

					if(lineData.OperandSize == 1) {
						int value = lineData.IsHex ? HexUtilities::FromHex(lineData.Operand) : std::stoi(lineData.Operand);
						output.push_back(value & 0xFF);
					} else if(lineData.OperandSize == 2) {
						int value = lineData.IsHex ? HexUtilities::FromHex(lineData.Operand) : std::stoi(lineData.Operand);
						output.push_back(value & 0xFF);
						output.push_back((value >> 8) & 0xFF);
					}

					foundMatch = true;
					break;
				}
			}
		}
	}

	if(!foundMatch) {
		output.push_back(AssemblerSpecialCodes::InvalidInstruction);
	} else {
		output.push_back(AssemblerSpecialCodes::EndOfLine);
	}
}

Assembler::Assembler(shared_ptr<LabelManager> labelManager)
{
	_labelManager = labelManager;
}

uint32_t Assembler::AssembleCode(string code, uint16_t startAddress, int16_t * assembledCode)
{
	std::unordered_map<string, uint16_t> temporaryLabels;

	size_t i = 0;
	vector<int16_t> output;
	output.reserve(1000);

	while(i < code.size()) {
		size_t offset = code.find_first_of('\n', i);
		string line;
		if(offset != string::npos) {
			line = code.substr(i, offset - i);
			i = offset + 1;
		} else {
			line = code.substr(i);
			i = code.size();
		}

		ProcessLine(line, startAddress, output, temporaryLabels);
	}

	memcpy(assembledCode, output.data(), output.size() * sizeof(uint16_t));
	return (uint32_t)output.size();
}
