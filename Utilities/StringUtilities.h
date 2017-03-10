#pragma once
#include "stdafx.h"

class StringUtilities
{
public:
	static vector<string> Split(string input, char delimiter)
	{
		vector<string> result;
		size_t index;
		while((index = input.find(delimiter)) != string::npos) {
			result.push_back(input.substr(0, index));
			input = input.substr(index + 1, input.size() - index - 1);
		}
		result.push_back(input);
		return result;
	}
};
