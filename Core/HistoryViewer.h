#pragma once
#include "stdafx.h"
#include <deque>
#include "IInputProvider.h"
#include "RewindData.h"

class Console;

class HistoryViewer : public IInputProvider
{
private:
	static constexpr int32_t BufferSize = 30; //Number of frames between each save state

	shared_ptr<Console> _console;
	std::deque<RewindData> _history;
	uint32_t _position;
	uint32_t _pollCounter;

public:
	HistoryViewer(shared_ptr<Console> console);

	void SetHistoryData(std::deque<RewindData> &history);
	
	uint32_t GetHistoryLength();
	uint32_t GetPosition();
	void SeekTo(uint32_t seekPosition);
	
	void ProcessEndOfFrame();

	// Inherited via IInputProvider
	virtual bool SetInput(BaseControlDevice * device) override;
};