#pragma once
#include "stdafx.h"

class NavigationHistory
{
private:
	vector<uint32_t> _history;
	int _position;

public:
	NavigationHistory()
	{
		_position = -1;
	}

	void AddLocation(uint32_t addr)
	{
		//Erase forward history
		if(!_history.empty()) {
			_history.erase(_history.begin() + _position + 1, _history.end());
		}

		_history.push_back(addr);
		_position++;
	}

	int32_t GoBack()
	{
		if(_position == 0) {
			return -1;
		}

		return _history[--_position];
	}

	int32_t GoForward()
	{
		if(_position >= (int)_history.size() - 1) {
			return -1;
		}

		return _history[++_position];
	}

};