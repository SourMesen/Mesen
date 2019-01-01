#include "stdafx.h"
#include <climits>
#include <algorithm>
#include "ExpressionEvaluator.h"
#include "Console.h"
#include "Debugger.h"
#include "MemoryDumper.h"
#include "LabelManager.h"
#include "../Utilities/HexUtilities.h"

const vector<string> ExpressionEvaluator::_binaryOperators = { { "*", "/", "%", "+", "-", "<<", ">>", "<", "<=", ">", ">=", "==", "!=", "&", "^", "|", "&&", "||" } };
const vector<int> ExpressionEvaluator::_binaryPrecedence = { { 10,  10,  10,   9,   9,    8,    8,   7,   7,    7,    7,    6,    6,   5,   4,   3,    2,    1 } };
const vector<string> ExpressionEvaluator::_unaryOperators = { { "+", "-", "~", "!" } };
const vector<int> ExpressionEvaluator::_unaryPrecedence = { { 11,  11,  11,  11 } };

bool ExpressionEvaluator::IsOperator(string token, int &precedence, bool unaryOperator)
{
	if(unaryOperator) {
		for(size_t i = 0, len = _unaryOperators.size(); i < len; i++) {
			if(token.compare(_unaryOperators[i]) == 0) {
				precedence = _unaryPrecedence[i];
				return true;
			}
		}
	} else {
		for(size_t i = 0, len = _binaryOperators.size(); i < len; i++) {
			if(token.compare(_binaryOperators[i]) == 0) {
				precedence = _binaryPrecedence[i];
				return true;
			}
		}
	}
	return false;
}

EvalOperators ExpressionEvaluator::GetOperator(string token, bool unaryOperator)
{
	if(unaryOperator) {
		for(size_t i = 0, len = _unaryOperators.size(); i < len; i++) {
			if(token.compare(_unaryOperators[i]) == 0) {
				return (EvalOperators)(EvalOperators::Plus + i);
			}
		}
	} else {
		for(size_t i = 0, len = _binaryOperators.size(); i < len; i++) {
			if(token.compare(_binaryOperators[i]) == 0) {
				return (EvalOperators)(EvalOperators::Multiplication + i);
			}
		}
	}
	return EvalOperators::Addition;
}

bool ExpressionEvaluator::CheckSpecialTokens(string expression, size_t &pos, string &output, ExpressionData &data)
{
	string token;
	size_t initialPos = pos;
	size_t len = expression.size();
	do {
		char c = std::tolower(expression[pos]);
		if((c >= 'a' && c <= 'z') || (c >= '0' && c <= '9') || c == '_' || c == '@') {
			//Only letters, numbers and underscore are allowed in code labels
			token += c;
			pos++;
		} else {
			break;
		}
	} while(pos < len);

	if(token == "a") {
		output += std::to_string((int64_t)EvalValues::RegA);
	} else if(token == "x") {
		output += std::to_string((int64_t)EvalValues::RegX);
	} else if(token == "y") {
		output += std::to_string((int64_t)EvalValues::RegY);
	} else if(token == "ps") {
		output += std::to_string((int64_t)EvalValues::RegPS);
	} else if(token == "sp") {
		output += std::to_string((int64_t)EvalValues::RegSP);
	} else if(token == "pc") {
		output += std::to_string((int64_t)EvalValues::RegPC);
	} else if(token == "oppc") {
		output += std::to_string((int64_t)EvalValues::RegOpPC);
	} else if(token == "frame") {
		output += std::to_string((int64_t)EvalValues::PpuFrameCount);
	} else if(token == "cycle") {
		output += std::to_string((int64_t)EvalValues::PpuCycle);
	} else if(token == "scanline") {
		output += std::to_string((int64_t)EvalValues::PpuScanline);
	} else if(token == "irq") {
		output += std::to_string((int64_t)EvalValues::Irq);
	} else if(token == "nmi") {
		output += std::to_string((int64_t)EvalValues::Nmi);
	} else if(token == "value") {
		output += std::to_string((int64_t)EvalValues::Value);
	} else if(token == "address") {
		output += std::to_string((int64_t)EvalValues::Address);
	} else if(token == "romaddress") {
		output += std::to_string((int64_t)EvalValues::AbsoluteAddress);
	} else if(token == "iswrite") {
		output += std::to_string((int64_t)EvalValues::IsWrite);
	} else if(token == "isread") {
		output += std::to_string((int64_t)EvalValues::IsRead);
	} else {
		string originalExpression = expression.substr(initialPos, pos - initialPos);
		bool validLabel = _debugger->GetLabelManager()->ContainsLabel(originalExpression);
		if(validLabel) {
			data.Labels.push_back(originalExpression);
			output += std::to_string(EvalValues::FirstLabelIndex + data.Labels.size() - 1);
		} else {
			return false;
		}
	}

	return true;
}

string ExpressionEvaluator::GetNextToken(string expression, size_t &pos, ExpressionData &data)
{
	string output;
	bool isOperator = false;
	bool isNumber = false;
	bool isHex = false;
	size_t initialPos = pos;
	for(size_t len = expression.size(); pos < len; pos++) {
		char c = std::tolower(expression[pos]);

		if(c == '$' && pos == initialPos) {
			isHex = true;
		} else if((c >= '0' && c <= '9') || (isHex && c >= 'a' && c <= 'f')) {
			if(isNumber || output.empty()) {
				output += c;
				isNumber = true;
			} else {
				//Just hit the start of a number, done reading current token
				break;
			}
		} else if(isNumber) {
			//First non-numeric character, done
			break;
		} else if(c == '(' || c == ')' || c == '[' || c == ']' || c == '{' || c == '}' || c == '-' || c == '+' || c == '~') {
			if(output.empty()) {
				output += c;
				pos++;
			}
			break;
		} else if(c == '!') {
			//Figure out if it's ! or !=
			if(pos < len - 1) {
				if(expression[pos + 1] == '=') {
					output += "!=";
					pos+=2;
				} else {
					output += "!";
					pos++;
				}
			}
			break;
		} else {
			if(c == '$') {
				break;
			} else if((c < 'a' || c > 'z') && c != '_' && c != '@') {
				//Not a number, not a letter, this is an operator
				isOperator = true;
				output += c;
			} else {
				if(isOperator) {
					break;
				} else {
					if(output.empty()) {
						if(CheckSpecialTokens(expression, pos, output, data)) {
							break;
						}
					}
					output += c;
				}
			}
		}
	}

	if(isHex) {
		output = std::to_string(HexUtilities::FromHex(output));
	}

	return output;
}
	
bool ExpressionEvaluator::ProcessSpecialOperator(EvalOperators evalOp, std::stack<EvalOperators> &opStack, std::stack<int> &precedenceStack, vector<int64_t> &outputQueue)
{
	if(opStack.empty()) {
		return false;
	}
	while(opStack.top() != evalOp) {
		outputQueue.push_back(opStack.top());
		opStack.pop();
		precedenceStack.pop();

		if(opStack.empty()) {
			return false;
		}
	}
	if(evalOp != EvalOperators::Parenthesis) {
		outputQueue.push_back(opStack.top());
	}
	opStack.pop();
	precedenceStack.pop();

	return true;
}

bool ExpressionEvaluator::ToRpn(string expression, ExpressionData &data)
{
	std::stack<EvalOperators> opStack = std::stack<EvalOperators>();	
	std::stack<int> precedenceStack;

	size_t position = 0;
	int parenthesisCount = 0;
	int bracketCount = 0;
	int braceCount = 0;

	bool previousTokenIsOp = true;
	while(true) {
		string token = GetNextToken(expression, position, data);
		if(token.empty()) {
			break;
		}

		bool unaryOperator = previousTokenIsOp;
		previousTokenIsOp = false;

		int precedence = 0;
		if(IsOperator(token, precedence, unaryOperator)) {
			EvalOperators op = GetOperator(token, unaryOperator);
			if(!opStack.empty()) {
				EvalOperators topOp = opStack.top();
				if((unaryOperator && precedence < precedenceStack.top()) || (!unaryOperator && precedence <= precedenceStack.top())) {
					opStack.pop();
					precedenceStack.pop();
					data.RpnQueue.push_back(topOp);
				}
			}
			opStack.push(op);
			precedenceStack.push(precedence);
			
			previousTokenIsOp = true;
		} else if(token[0] == '(') {
			parenthesisCount++;
			opStack.push(EvalOperators::Parenthesis);
			precedenceStack.push(0);
			previousTokenIsOp = true;
		} else if(token[0] == ')') {
			parenthesisCount--;
			if(!ProcessSpecialOperator(EvalOperators::Parenthesis, opStack, precedenceStack, data.RpnQueue)) {
				return false;
			}
		} else if(token[0] == '[') {
			bracketCount++;
			opStack.push(EvalOperators::Bracket);
			precedenceStack.push(0);
		} else if(token[0] == ']') {
			bracketCount--;
			if(!ProcessSpecialOperator(EvalOperators::Bracket, opStack, precedenceStack, data.RpnQueue)) {
				return false;
			}
		} else if(token[0] == '{') {
			braceCount++;
			opStack.push(EvalOperators::Braces);
			precedenceStack.push(0);
		} else if(token[0] == '}') {
			braceCount--;
			if(!ProcessSpecialOperator(EvalOperators::Braces, opStack, precedenceStack, data.RpnQueue)){
				return false;
			}
		} else {
			if(token[0] < '0' || token[0] > '9') {
				return false;
			} else {
				data.RpnQueue.push_back(std::stoll(token));
			}
		}
	}

	if(braceCount || bracketCount || parenthesisCount) {
		//Mismatching number of brackets/braces/parenthesis
		return false;
	}

	while(!opStack.empty()) {
		data.RpnQueue.push_back(opStack.top());
		opStack.pop();
	}

	return true;
}

int32_t ExpressionEvaluator::Evaluate(ExpressionData &data, DebugState &state, EvalResultType &resultType, OperationInfo &operationInfo)
{
	if(data.RpnQueue.empty()) {
		resultType = EvalResultType::Invalid;
		return 0;
	}

	int pos = 0;
	int64_t right = 0;
	int64_t left = 0;
	resultType = EvalResultType::Numeric;

	for(size_t i = 0, len = data.RpnQueue.size(); i < len; i++) {
		int64_t token = data.RpnQueue[i];

		if(token >= EvalValues::RegA) {
			//Replace value with a special value
			if(token >= EvalValues::FirstLabelIndex) {
				int64_t labelIndex = token - EvalValues::FirstLabelIndex;
				if((size_t)labelIndex < data.Labels.size()) {
					token = _debugger->GetLabelManager()->GetLabelRelativeAddress(data.Labels[labelIndex]);
				} else {
					token = -1;
				}
				if(token < 0) {
					//Label is no longer valid
					resultType = EvalResultType::Invalid;
					return 0;
				}
			} else {
				switch(token) {
					case EvalValues::RegA: token = state.CPU.A; break;
					case EvalValues::RegX: token = state.CPU.X; break;
					case EvalValues::RegY: token = state.CPU.Y; break;
					case EvalValues::RegSP: token = state.CPU.SP; break;
					case EvalValues::RegPS: token = state.CPU.PS; break;
					case EvalValues::RegPC: token = state.CPU.PC; break;
					case EvalValues::RegOpPC: token = state.CPU.DebugPC; break;
					case EvalValues::PpuFrameCount: token = state.PPU.FrameCount; break;
					case EvalValues::PpuCycle: token = state.PPU.Cycle; break;
					case EvalValues::PpuScanline: token = state.PPU.Scanline; break;
					case EvalValues::Nmi: token = state.CPU.NMIFlag; break;
					case EvalValues::Irq: token = state.CPU.IRQFlag; break;
					case EvalValues::Value: token = operationInfo.Value; break;
					case EvalValues::Address: token = operationInfo.Address; break;
					case EvalValues::AbsoluteAddress: token = _debugger->GetAbsoluteAddress(operationInfo.Address); break;
					case EvalValues::IsWrite: token = operationInfo.OperationType == MemoryOperationType::Write || operationInfo.OperationType == MemoryOperationType::DummyWrite; break;
					case EvalValues::IsRead: token = operationInfo.OperationType == MemoryOperationType::Read || operationInfo.OperationType == MemoryOperationType::DummyRead; break;
				}
			}
		} else if(token >= EvalOperators::Multiplication) {
			right = operandStack[--pos];
			if(pos > 0 && token <= EvalOperators::LogicalOr) {
				//Only do this for binary operators
				left = operandStack[--pos];
			}

			resultType = EvalResultType::Numeric;
			switch(token) {
				case EvalOperators::Multiplication: token = left * right; break;
				case EvalOperators::Division: 
					if(right == 0) {
						resultType = EvalResultType::DivideBy0;
						return 0;
					}
					token = left / right; break;
				case EvalOperators::Modulo:
					if(right == 0) {
						resultType = EvalResultType::DivideBy0;
						return 0;
					}
					token = left % right;
					break;
				case EvalOperators::Addition: token = left + right; break;
				case EvalOperators::Substration: token = left - right; break;
				case EvalOperators::ShiftLeft: token = left << right; break;
				case EvalOperators::ShiftRight: token = left >> right; break;
				case EvalOperators::SmallerThan: token = left < right; resultType = EvalResultType::Boolean; break;
				case EvalOperators::SmallerOrEqual: token = left <= right; resultType = EvalResultType::Boolean; break;
				case EvalOperators::GreaterThan: token = left > right; resultType = EvalResultType::Boolean; break;
				case EvalOperators::GreaterOrEqual: token = left >= right; resultType = EvalResultType::Boolean; break;
				case EvalOperators::Equal: token = left == right; resultType = EvalResultType::Boolean; break;
				case EvalOperators::NotEqual: token = left != right; resultType = EvalResultType::Boolean; break;
				case EvalOperators::BinaryAnd: token = left & right; break;
				case EvalOperators::BinaryXor: token = left ^ right; break;
				case EvalOperators::BinaryOr: token = left | right; break;
				case EvalOperators::LogicalAnd: token = left && right; resultType = EvalResultType::Boolean; break;
				case EvalOperators::LogicalOr: token = left || right; resultType = EvalResultType::Boolean; break;

				//Unary operators
				case EvalOperators::Plus: token = right; break;
				case EvalOperators::Minus: token = -right; break;
				case EvalOperators::BinaryNot: token = ~right; break;
				case EvalOperators::LogicalNot: token = !right; break;
				case EvalOperators::Bracket: token = _debugger->GetMemoryDumper()->GetMemoryValue(DebugMemoryType::CpuMemory, (uint32_t)right); break;
				case EvalOperators::Braces: token = _debugger->GetMemoryDumper()->GetMemoryValueWord(DebugMemoryType::CpuMemory, (uint32_t)right); break;
				default: throw std::runtime_error("Invalid operator");
			}
		}
		operandStack[pos++] = token;
	}
	return (int32_t)operandStack[0];
}

ExpressionEvaluator::ExpressionEvaluator(Debugger* debugger)
{
	_debugger = debugger;
}

ExpressionData ExpressionEvaluator::GetRpnList(string expression, bool &success)
{
	ExpressionData* cachedData = PrivateGetRpnList(expression, success);
	if(cachedData) {
		return *cachedData;
	} else {
		return ExpressionData();
	}
}

ExpressionData* ExpressionEvaluator::PrivateGetRpnList(string expression, bool& success)
{
	ExpressionData *cachedData = nullptr;
	{
		LockHandler lock = _cacheLock.AcquireSafe();

		auto result = _cache.find(expression);
		if(result != _cache.end()) {
			cachedData = &(result->second);
		}
	}

	if(cachedData == nullptr) {
		string fixedExp = expression;
		fixedExp.erase(std::remove(fixedExp.begin(), fixedExp.end(), ' '), fixedExp.end());
		ExpressionData data;
		success = ToRpn(fixedExp, data);
		if(success) {
			LockHandler lock = _cacheLock.AcquireSafe();
			_cache[expression] = data;
			cachedData = &_cache[expression];
		}
	} else {
		success = true;
	}

	return cachedData;
}

int32_t ExpressionEvaluator::PrivateEvaluate(string expression, DebugState &state, EvalResultType &resultType, OperationInfo &operationInfo, bool& success)
{
	success = true;
	ExpressionData *cachedData = PrivateGetRpnList(expression, success);

	if(!success) {
		return 0;
	}

	return Evaluate(*cachedData, state, resultType, operationInfo);	
}

int32_t ExpressionEvaluator::Evaluate(string expression, DebugState &state, EvalResultType &resultType, OperationInfo &operationInfo)
{
	try {
		bool success;
		int32_t result = PrivateEvaluate(expression, state, resultType, operationInfo, success);
		if(success) {
			return result;
		}
	} catch(std::exception e) {
	}
	resultType = EvalResultType::Invalid;
	return 0;
}

bool ExpressionEvaluator::Validate(string expression)
{
	try {
		DebugState state;
		EvalResultType type;
		OperationInfo operationInfo;
		bool success;
		PrivateEvaluate(expression, state, type, operationInfo, success);
		return success;
	} catch(std::exception e) {
		return false;
	}
}