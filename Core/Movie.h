#pragma once

#include "stdafx.h"

struct MovieData
{
	uint32_t SaveStateSize = 0;
	uint32_t DataSize[4];
	vector<uint16_t> PortData[4];

	bool Save(ofstream &file, stringstream &startState)
	{
		file.write("NMO", 3);
		SaveStateSize = (uint32_t)startState.tellp();
		file.write((char*)&SaveStateSize, sizeof(uint32_t));
		
		if(SaveStateSize > 0) {
			startState.seekg(0, ios::beg);
			uint8_t *stateBuffer = new uint8_t[SaveStateSize];
			startState.read((char*)stateBuffer, SaveStateSize);
			file.write((char*)stateBuffer, SaveStateSize);
			delete[] stateBuffer;
		}

		for(int i = 0; i < 4; i++) {
			DataSize[i] = PortData[i].size();
			file.write((char*)&DataSize[i], sizeof(uint32_t));
			if(DataSize[i] > 0) {
				file.write((char*)&PortData[i][0], DataSize[i] * sizeof(uint16_t));
			}
		}

		file.close();

		return true;
	}

	bool Load(wstring filename, stringstream &startState)
	{
		ifstream file(filename, ios::in | ios::binary);

		if(file) {
			char header[3];
			file.read((char*)&header, 3);

			if(memcmp((char*)&header, "NMO", 3) != 0) {
				//Invalid movie file
				return false;
			}

			file.read((char*)&SaveStateSize, sizeof(uint32_t));
			
			if(SaveStateSize > 0) {
				startState.clear();
				startState.seekg(0, ios::beg);
				uint8_t *stateBuffer = new uint8_t[SaveStateSize];
				file.read((char*)stateBuffer, SaveStateSize);
				startState.write((char*)stateBuffer, SaveStateSize);
				delete[] stateBuffer;
			}

			for(int i = 0; i < 4; i++) {
				file.read((char*)&DataSize[i], sizeof(uint32_t));

				uint16_t* readBuffer = new uint16_t[DataSize[i]];
				file.read((char*)readBuffer, DataSize[i] * sizeof(uint16_t));
				PortData[i] = vector<uint16_t>(readBuffer, readBuffer + DataSize[i]);
				delete[] readBuffer;
			}

			file.close();

			return true;
		}

		return false;
	}
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
		stringstream _startState;
		MovieData _data;

	private:
		void PushState(uint8_t port);
		void StartRecording(wstring filename, bool reset);
		void PlayMovie(wstring filename);
		void StopAll();

		void RecordState(uint8_t port, uint8_t state);
		uint8_t GetState(uint8_t port);
		
	public:
		static void Record(wstring filename, bool reset);
		static void Play(wstring filename);
		static void Stop();
		static bool Playing();
};