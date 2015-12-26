#pragma once

#include "stdafx.h"
#include <deque>
#include "INotificationListener.h"
#include "../Utilities/AutoResetEvent.h"

class AutoRomTest : public INotificationListener
{
private:
	bool _recording;
	bool _runningTest;
	bool _testResult;

	uint8_t _previousHash[16];
	std::deque<uint8_t*> _screenshotHashes;
	std::deque<uint8_t> _repetitionCount;
	uint8_t _currentCount;

	string _filename;
	ofstream _file;

	AutoResetEvent _signal;

private:
	void Reset();
	void ValidateFrame(uint16_t* ppuFrameBuffer);
	void SaveFrame(uint16_t* ppuFrameBuffer);
	void Save();

public:
	AutoRomTest();
	~AutoRomTest();

	void ProcessNotification(ConsoleNotificationType type, void* parameter);
	void Record(string filename, bool reset);
	bool Run(string filename);
	void Stop();
};