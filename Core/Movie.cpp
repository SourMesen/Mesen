#include "stdafx.h"
#include "Movie.h"
#include "Console.h"

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
		_playing = false;
	}

	return (data >> 8);
}

void Movie::StartRecording(wstring filename, bool reset)
{
	_file = ofstream(filename, ios::out | ios::binary);

	if(_file) {
		if(reset) {
			_startState.clear();
			Console::Reset();
		} else {
			Console::SaveState(_startState);
		}

		memset(_counter, 0, 4);
		memset(_lastState, 0, 4);
		_data = MovieData();
		_recording = true;
	}
}

void Movie::StopAll()
{
	if(_recording) {
		for(int i = 0; i < 4; i++) {
			PushState(i);
		}
		_data.Save(_file, _startState);
		_recording = false;
	}
	_playing = false;
}

void Movie::PlayMovie(wstring filename)
{
	if(!_recording) {
		memset(_readPosition, 0, 4 * sizeof(uint32_t));
		_data = MovieData();
		if(_data.Load(filename, _startState)) {
			if(_startState.tellp() > 0) {
				//Restore state if one was present in the movie
				Console::LoadState(_startState);
			} else {
				Console::Reset();
			}
			_playing = true;
		}
	}
}

void Movie::Record(wstring filename, bool reset)
{
	Instance->StartRecording(filename, reset);
}

void Movie::Play(wstring filename)
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