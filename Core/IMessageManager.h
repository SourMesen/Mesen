#pragma once

#include "stdafx.h"
#include <chrono>

class ToastInfo;

class IMessageManager
{
public:
	virtual void DisplayMessage(wstring title, wstring message) = 0;
	virtual void DisplayToast(shared_ptr<ToastInfo> toast) = 0;
};

class ToastInfo
{
private:
	wstring _title;
	wstring _message;
	uint8_t* _icon;
	uint32_t _iconSize;
	uint64_t _endTime;
	uint64_t _startTime;

	uint8_t* ReadFile(wstring filename, uint32_t &fileSize)
	{
		ifstream file(filename, ios::in | ios::binary);
		if(file) {
			file.seekg(0, ios::end);
			fileSize = (uint32_t)file.tellg();
			file.seekg(0, ios::beg);

			uint8_t* buffer = new uint8_t[fileSize];
			file.read((char*)buffer, fileSize);
			return buffer;
		}
		return nullptr;
	}

	uint64_t GetCurrentTime()
	{
		return std::chrono::duration_cast<std::chrono::milliseconds>(std::chrono::high_resolution_clock::now().time_since_epoch()).count();
	}

public:
	ToastInfo(wstring title, wstring message, int displayDuration, wstring iconFile)
	{
		_title = title;
		_message = message;
		
		_icon = ReadFile(iconFile, _iconSize);

		_startTime = GetCurrentTime();
		_endTime = _startTime + displayDuration;
	}

	ToastInfo(wstring title, wstring message, int displayDuration, uint8_t* iconData, uint32_t iconSize)
	{
		_title = title;
		_message = message;
		
		_iconSize = iconSize;
		_icon = new uint8_t[iconSize];
		memcpy(_icon, iconData, iconSize);

		_startTime = GetCurrentTime();
		_endTime = _startTime + displayDuration;
	}

	~ToastInfo()
	{
		if(_icon) {
			delete _icon;
		}
	}

	wstring GetToastTitle()
	{
		return _title;
	}

	wstring GetToastMessage() 
	{
		return _message;
	}

	uint8_t* GetToastIcon()
	{
		return _icon;
	}

	uint32_t GetIconSize()
	{
		return _iconSize;
	}

	bool HasIcon() 
	{
		return _iconSize > 0;
	}

	float GetOpacity()
	{
		uint64_t currentTime = GetCurrentTime();
		if(currentTime - _startTime < 333) {
			return (currentTime - _startTime) * 3.0f / 1000.0f;
		} else if(_endTime - currentTime < 333) {
			return (_endTime - currentTime) * 3.0f / 1000.0f;
		} else {
			return 1.0f;
		}
	}

	bool IsToastExpired()
	{
		return _endTime < GetCurrentTime();
	}
};