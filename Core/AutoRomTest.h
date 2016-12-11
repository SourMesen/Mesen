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
	int _badFrameCount;
	bool _recordingFromMovie;

	uint8_t _previousHash[16];
	std::deque<uint8_t*> _screenshotHashes;
	std::deque<uint8_t> _repetitionCount;
	uint8_t _currentCount;
	
	//Used when make a test out of an existing movie/test
	stringstream _movieStream;
	stringstream _romStream;

	string _filename;
	ofstream _file;

	AutoResetEvent _signal;

private:
	void Reset();
	void ValidateFrame(uint16_t* ppuFrameBuffer);
	void SaveFrame(uint16_t* ppuFrameBuffer);
	void Save();
	void RecordFromMovie(string testFilename, stringstream &movieStream, bool autoLoadRom);

public:
	AutoRomTest();
	virtual ~AutoRomTest();

	void ProcessNotification(ConsoleNotificationType type, void* parameter);
	void Record(string filename, bool reset);
	void RecordFromMovie(string testFilename, string movieFilename);
	void RecordFromTest(string newTestFilename, string existingTestFilename);
	int32_t Run(string filename);
	void Stop();
};