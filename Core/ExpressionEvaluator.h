#pragma once
#include "stdafx.h"
#include <stack>
#include <deque>
#include <unordered_map>
#include "../Utilities/SimpleLock.h"
#include "DebuggerTypes.h"

class Debugger;

enum EvalOperators
{
	//Binary operators
	Multiplication = 2000000000,
	Division = 2000000001,
	Modulo = 2000000002,
	Addition = 2000000003,
	Substration = 2000000004,
	ShiftLeft = 2000000005,
	ShiftRight = 2000000006,
	SmallerThan = 2000000007,
	SmallerOrEqual = 2000000008,
	GreaterThan = 2000000009,
	GreaterOrEqual = 2000000010,
	Equal = 2000000011,
	NotEqual = 2000000012,
	BinaryAnd = 2000000013,
	BinaryXor = 2000000014,
	BinaryOr = 2000000015,
	LogicalAnd = 2000000016,
	LogicalOr = 2000000017,

	//Unary operators
	Plus = 2000000050,
	Minus = 2000000051,
	BinaryNot = 2000000052,
	LogicalNot = 2000000053,

	//Used to read ram address
	Bracket = 2000000054, //Read byte
	Braces = 2000000055, //Read word

	//Special value, not used as an operator
	Parenthesis = 2000000100,
};

enum EvalValues
{
	RegA = 2000000100,
	RegX = 2000000101,
	RegY = 2000000102,
	RegSP = 2000000103,
	RegPS = 2000000104,
	RegPC = 2000000105,
	RegOpPC = 2000000106,
	PpuFrameCount = 2000000107,
	PpuCycle = 2000000108,
	PpuScanline = 2000000109,
	Nmi = 2000000110,
	Irq = 2000000111,
	Value = 2000000112,
	Address = 2000000113,
	AbsoluteAddress = 2000000114,
	IsWrite = 2000000115,
	IsRead = 2000000116,
};

enum EvalResultType
{
	Numeric = 0,
	Boolean = 1,
	Invalid = 2,
	DivideBy0 = 3
};

class StringHasher
{
public:
	size_t operator()(const std::string& t) const 
	{
		//Quick hash for expressions - most are likely to have different lengths, and not expecting dozens of breakpoints, either, so this should be fine.
		return t.size();
	}
};

class ExpressionEvaluator
{
private:
	static const vector<string> _binaryOperators;
	static const vector<int> _binaryPrecedence;
	static const vector<string> _unaryOperators;
	static const vector<int> _unaryPrecedence;

	static std::unordered_map<string, std::vector<int>, StringHasher> _outputCache;
	static SimpleLock _cacheLock;

	int operandStack[1000];
	Debugger* _debugger;
	bool _containsCustomLabels = false;

	bool IsOperator(string token, int &precedence, bool unaryOperator);
	EvalOperators GetOperator(string token, bool unaryOperator);
	bool CheckSpecialTokens(string expression, size_t &pos, string &output);
	string GetNextToken(string expression, size_t &pos);	
	bool ProcessSpecialOperator(EvalOperators evalOp, std::stack<EvalOperators> &opStack, vector<int> &outputQueue);
	bool ToRpn(string expression, vector<int> &outputQueue);
	int32_t PrivateEvaluate(string expression, DebugState &state, EvalResultType &resultType, OperationInfo &operationInfo, bool &success);
	vector<int>* GetRpnList(string expression, vector<int> &output, bool& success);

public:
	ExpressionEvaluator(Debugger* debugger);

	int32_t Evaluate(vector<int> &outputQueue, DebugState &state, EvalResultType &resultType, OperationInfo &operationInfo);
	int32_t Evaluate(string expression, DebugState &state, EvalResultType &resultType, OperationInfo &operationInfo);
	vector<int>* GetRpnList(string expression);

	bool Validate(string expression);
};