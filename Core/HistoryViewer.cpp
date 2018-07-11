#include "stdafx.h"
#include "HistoryViewer.h"
#include "RewindData.h"
#include "Console.h"
#include "BaseControlDevice.h"
#include "SoundMixer.h"
#include "NotificationManager.h"

HistoryViewer::HistoryViewer(shared_ptr<Console> console)
{
	_console = console;
	_position = 0;
	_pollCounter = 0;
}

void HistoryViewer::SetHistoryData(std::deque<RewindData> &history)
{
	_history = history;

	_console->GetControlManager()->UnregisterInputProvider(this);
	_console->GetControlManager()->RegisterInputProvider(this);
	
	SeekTo(0);
}

uint32_t HistoryViewer::GetHistoryLength()
{
	//Returns history length in number of frames
	return (uint32_t)(_history.size() * HistoryViewer::BufferSize);
}

uint32_t HistoryViewer::GetPosition()
{
	return _position;
}

void HistoryViewer::SeekTo(uint32_t seekPosition)
{
	//Seek to the specified position, in seconds
	uint32_t index = (uint32_t)(seekPosition * 60 / HistoryViewer::BufferSize);
	if(index < _history.size()) {
		_console->Pause();
		
		bool wasPaused = _console->GetPauseStatus();
		_console->SetPauseStatus(false);
		_position = index;
		RewindData rewindData = _history[_position];
		rewindData.LoadState(_console);

		_console->GetSoundMixer()->StopAudio(true);
		_pollCounter = 0;
		_console->SetPauseStatus(wasPaused);

		_console->Resume();
	}
}

bool HistoryViewer::SetInput(BaseControlDevice *device)
{
	uint8_t port = device->GetPort();
	std::deque<ControlDeviceState> &stateData = _history[_position].InputLogs[port];
	if(_pollCounter < stateData.size()) {
		ControlDeviceState state = stateData[_pollCounter];
		device->SetRawState(state);
	}
	if(port == 0 && _pollCounter < 30) {
		_pollCounter++;
	}
	return true;
}

void HistoryViewer::ProcessEndOfFrame()
{
	if(_pollCounter == HistoryViewer::BufferSize) {
		_pollCounter = 0;
		_position++;

		if(_position >= _history.size()) {
			//Reached the end of history data
			_console->SetPauseStatus(true);
			return;
		}

		RewindData rewindData = _history[_position];
		rewindData.LoadState(_console);
	}
}
