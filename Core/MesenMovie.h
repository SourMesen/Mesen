#pragma once

#include "stdafx.h"
#include "CheatManager.h"
#include "MovieManager.h"

struct MovieData
{
	uint32_t SaveStateSize = 0;
	uint32_t DataSize[4];
	vector<uint16_t> PortData[4];
};

class MesenMovie : public IMovie
{
private:
	const uint32_t MovieFormatVersion = 5;
	bool _recording = false;
	bool _playing = false;
	uint8_t _counter[4];
	uint8_t _lastState[4];
	uint32_t _readPosition[4];
	ofstream _file;
	string _filename;
	stringstream _startState;
	MovieData _data;
	vector<CodeInfo> _cheatList;

private:
	void Reset();
	bool Save();
	void Stop();
	bool Load(std::stringstream &file, bool autoLoadRom);

protected:
	void PushState(uint8_t port);
	void Record(string filename, bool reset);
	void Play(stringstream &filestream, bool autoLoadRom, string filename = "");

	bool IsPlaying();
	bool IsRecording();

public:
	~MesenMovie();

	void RecordState(uint8_t port, uint8_t state);
	uint8_t GetState(uint8_t port);
};