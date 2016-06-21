#pragma once

#include "stdafx.h"
#include "CheatManager.h"

struct MovieData
{
	uint32_t SaveStateSize = 0;
	uint32_t DataSize[4];
	vector<uint16_t> PortData[4];
};

class Movie
{
	private:
		static shared_ptr<Movie> _instance;
		const uint32_t MovieFormatVersion = 4;
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
		void PushState(uint8_t port);
		void StartRecording(string filename, bool reset);
		
		void PlayMovie(stringstream &filestream, bool autoLoadRom, string filename = "");
		void StopAll();
		void Reset();

		bool Save();
		bool Load(std::stringstream &file, bool autoLoadRom);
		
	public:
		~Movie();

		void RecordState(uint8_t port, uint8_t state);
		uint8_t GetState(uint8_t port);

		static shared_ptr<Movie> GetInstance();
		static void Record(string filename, bool reset);
		static void Play(string filename);
		static void Play(std::stringstream &filestream, bool autoLoadRom);
		static void Stop();
		static bool Playing();
		static bool Recording();
};