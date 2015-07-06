#pragma once

#include "stdafx.h"

struct MovieData
{
	uint32_t SaveStateSize = 0;
	uint32_t DataSize[4];
	vector<uint16_t> PortData[4];
};

class Movie
{
	friend class ControlManager;

	private:
		static Movie* Instance;
		bool _recording = false;
		bool _playing = false;
		uint8_t _counter[4];
		uint8_t _lastState[4];
		uint32_t _readPosition[4];
		ofstream _file;
		wstring _filename;
		stringstream _startState;
		MovieData _data;

	private:
		void PushState(uint8_t port);
		void StartRecording(wstring filename, bool reset);
		void PlayMovie(wstring filename);
		void StopAll();
		void Reset();

		void RecordState(uint8_t port, uint8_t state);
		uint8_t GetState(uint8_t port);

		bool Save();
		bool Load(wstring filename);
		
	public:
		static void Record(wstring filename, bool reset);
		static void Play(wstring filename);
		static void Stop();
		static bool Playing();
		static bool Recording();
};