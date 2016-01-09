#pragma once
#include "stdafx.h"
#include <stack>
#include <deque>
#include <unordered_map>
#include "../Utilities/SimpleLock.h"

struct DebugState;

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

	//Special value, not used as an operator
	Parenthesis = 2000000100
};

enum EvalValues
{
	RegA = 2000000100,
	RegX = 2000000101,
	RegY = 2000000102,
	RegSP = 2000000103,
	RegPS = 2000000104,
	PpuCycle = 2000000105,
	PpuScanline = 2000000106,
	Nmi = 2000000107,
	Irq = 2000000108,
	Value = 2000000109,
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
	const vector<string> _binaryOperators = { { "*", "/", "%", "+", "-", "<<", ">>", "<", "<=", ">", ">=", "==", "!=", "&", "^", "|", "&&", "||" } };
	const vector<int> _binaryPrecedence = { {    10,  10,  10,   9,   9,    8,    8,   7,   7,    7,    7,    6,    6,   5,   4,   3,    2,    1 } };
	const vector<string> _unaryOperators = { { "+", "-", "!", "~" } };
	const vector<int> _unaryPrecedence = { {    11,  11,  11,  11 } };

	static std::unordered_map<string, std::vector<int>, StringHasher> _outputCache;
	static SimpleLock _cacheLock;

	bool IsOperator(string token, int &precedence, bool unaryOperator);
	EvalOperators GetOperator(string token, bool unaryOperator);
	int GetOperatorPrecendence(string token);
	bool CheckSpecialTokens(string expression, size_t &pos, string &output);
	string GetNextToken(string expression, size_t &pos);	
	void ToRpn(string expression, vector<int> &outputQueue);
	bool EvaluateExpression(vector<int> *outputQueue, DebugState &state, int16_t memoryValue);
	bool PrivateEvaluate(string expression, DebugState &state, int16_t memoryValue);

public:
	ExpressionEvaluator();

	bool Evaluate(string expression, DebugState &state, int16_t memoryValue = 0);
	bool Validate(string expression);
};