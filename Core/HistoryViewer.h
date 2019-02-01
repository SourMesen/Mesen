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
	virtual ~HistoryViewer();

	void SetHistoryData(std::deque<RewindData> &history);
	
	uint32_t GetHistoryLength();
	void GetHistorySegments(uint32_t * segmentBuffer, uint32_t &bufferSize);
	uint32_t GetPosition();
	void SeekTo(uint32_t seekPosition);

	bool CreateSaveState(string outputFile, uint32_t position);
	bool SaveMovie(string movieFile, uint32_t startPosition, uint32_t endPosition);

	void ResumeGameplay(shared_ptr<Console> console, uint32_t resumePosition);
	
	void ProcessEndOfFrame();

	// Inherited via IInputProvider
	bool SetInput(BaseControlDevice * device) override;
};