#include "stdafx.h"
#include "MessageManager.h"
#include "Movie.h"
#include "Console.h"
#include "../Utilities/FolderUtilities.h"
#include "ROMLoader.h"

Movie* Movie::Instance = new Movie();

void Movie::PushState(uint8_t port)
{
	if(_counter[port] > 0) {
		uint16_t data = _lastState[port] << 8 | _counter[port];
		_data.PortData[port].push_back(data);

		_lastState[port] = 0;
		_counter[port] = 0;
	}
}

void Movie::RecordState(uint8_t port, uint8_t state)
{
	if(_recording) {
		if(_lastState[port] != state || _counter[port] == 0) {
			if(_counter[port] != 0) {
				PushState(port);
			}
			_lastState[port] = state;
			_counter[port] = 1;
		} else {
			_counter[port]++;

			if(_counter[port] == 255) {
				PushState(port);
			}
		}
	}
}

uint8_t Movie::GetState(uint8_t port)
{
	uint16_t data = --_data.PortData[port][_readPosition[port]];
	if((data & 0xFF) == 0) {
		_readPosition[port]++;
	}

	if(_readPosition[port] >= _data.DataSize[port]) {
		//End of movie file
		MessageManager::DisplayMessage("Movies", "Movie ended.");
		_playing = false;
	}

	return (data >> 8);
}

void Movie::Reset()
{
	_startState.clear();
	_startState.seekg(0, ios::beg);
	_startState.seekp(0, ios::beg);

	memset(_readPosition, 0, 4 * sizeof(uint32_t));
	memset(_counter, 0, 4);
	memset(_lastState, 0, 4);
	_data = MovieData();

	_recording = false;
	_playing = false;
}

void Movie::StartRecording(string filename, bool reset)
{
	_filename = filename;
	_file.open(filename, ios::out | ios::binary);

	if(_file) {
		Console::Pause();

		Reset();

		if(reset) {
			Console::Reset(false);
		} else {
			Console::SaveState(_startState);
		}

		_recording = true;

		Console::Resume();

		MessageManager::DisplayMessage("Movies", "Recording to: " + FolderUtilities::GetFilename(filename, true));
	}
}

void Movie::StopAll()
{
	if(_recording) {
		_recording = false;
		for(int i = 0; i < 4; i++) {
			PushState(i);
		}
		Save();
	}
	if(_playing) {
		MessageManager::DisplayMessage("Movies", "Movie stopped.");
		_playing = false;
	}
}

void Movie::PlayMovie(string filename)
{
	StopAll();

	Reset();
	
	if(Load(filename)) {
		Console::Pause();
		if(_startState.tellp() > 0) {
			//Restore state if one was present in the movie
			Console::LoadState(_startState);
		} else {
			Console::Reset(false);
		}
		_playing = true;
		Console::Resume();
		MessageManager::DisplayMessage("Movies", "Playing movie: " + FolderUtilities::GetFilename(filename, true));
	}
}

void Movie::Record(string filename, bool reset)
{
	Instance->StartRecording(filename, reset);
}

void Movie::Play(string filename)
{
	Instance->PlayMovie(filename);
}

void Movie::Stop()
{
	Instance->StopAll();
}

bool Movie::Playing()
{
	return Instance->_playing;
}

bool Movie::Recording()
{
	return Instance->_recording;
}

bool Movie::Save()
{
	_file.write("MMO", 3);
	_data.SaveStateSize = (uint32_t)_startState.tellp();
		
	string romFilepath = Console::GetROMPath();
	string romFilename = FolderUtilities::GetFilename(romFilepath, true);

	uint32_t romCrc32 = ROMLoader::GetCRC32(romFilepath);
	_file.write((char*)&romCrc32, sizeof(romCrc32));

	uint32_t romNameSize = (uint32_t)romFilename.size();
	_file.write((char*)&romNameSize, sizeof(uint32_t));
	_file.write((char*)romFilename.c_str(), romNameSize);

	_file.write((char*)&_data.SaveStateSize, sizeof(uint32_t));
		
	if(_data.SaveStateSize > 0) {
		_startState.seekg(0, ios::beg);
		uint8_t *stateBuffer = new uint8_t[_data.SaveStateSize];
		_startState.read((char*)stateBuffer, _data.SaveStateSize);
		_file.write((char*)stateBuffer, _data.SaveStateSize);
		delete[] stateBuffer;
	}

	for(int i = 0; i < 4; i++) {
		_data.DataSize[i] = (uint32_t)_data.PortData[i].size();
		_file.write((char*)&_data.DataSize[i], sizeof(uint32_t));
		if(_data.DataSize[i] > 0) {
			_file.write((char*)&_data.PortData[i][0], _data.DataSize[i] * sizeof(uint16_t));
		}
	}

	_file.close();

	MessageManager::DisplayMessage("Movies", "Movie saved to file: " + FolderUtilities::GetFilename(_filename, true));

	return true;
}

bool Movie::Load(string filename)
{
	ifstream file(filename, ios::in | ios::binary);

	if(file) {
		char header[3];
		file.read((char*)&header, 3);

		if(memcmp((char*)&header, "MMO", 3) != 0) {
			//Invalid movie file
			return false;
		}

		uint32_t romCrc32;
		file.read((char*)&romCrc32, sizeof(romCrc32));

		uint32_t romNameSize;
		file.read((char*)&romNameSize, sizeof(uint32_t));
			
		char* romFilename = new char[romNameSize + 1];
		memset(romFilename, 0, (romNameSize+1));
		file.read((char*)romFilename, romNameSize);

		string currentRom = Console::GetROMPath();
		bool loadedGame = true;
		if(currentRom.empty() || romCrc32 != ROMLoader::GetCRC32(currentRom)) {
			//Loaded game isn't the same as the game used for the movie, attempt to load the correct game
			loadedGame = Console::LoadROM(romFilename, romCrc32);
		}

		if(loadedGame) {
			file.read((char*)&_data.SaveStateSize, sizeof(uint32_t));

			if(_data.SaveStateSize > 0) {
				uint8_t *stateBuffer = new uint8_t[_data.SaveStateSize];
				file.read((char*)stateBuffer, _data.SaveStateSize);
				_startState.write((char*)stateBuffer, _data.SaveStateSize);
				delete[] stateBuffer;
			}

			for(int i = 0; i < 4; i++) {
				file.read((char*)&_data.DataSize[i], sizeof(uint32_t));

				uint16_t* readBuffer = new uint16_t[_data.DataSize[i]];
				file.read((char*)readBuffer, _data.DataSize[i] * sizeof(uint16_t));
				_data.PortData[i] = vector<uint16_t>(readBuffer, readBuffer + _data.DataSize[i]);
				delete[] readBuffer;
			}
		} else {
			MessageManager::DisplayMessage("Movies", "Missing ROM required (" + string(romFilename) + ") to play movie.");
		}
		file.close();

		return loadedGame;
	}

	return false;
}
