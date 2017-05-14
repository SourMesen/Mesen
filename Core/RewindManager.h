#pragma once
#include "stdafx.h"
#include <deque>
#include "INotificationListener.h"
#include "RewindData.h"

enum class RewindState
{
	Stopped = 0,
	Stopping = 1,
	Starting = 2,
	Started = 3
};

class RewindManager : public INotificationListener
{
private:
	static const uint32_t BufferSize = 30; //Number of frames between each save state
	static RewindManager* _instance;
	std::deque<RewindData> _history;
	std::deque<RewindData> _historyBackup;
	RewindData _currentHistory;

	RewindState _rewindState;
	int32_t _framesToFastForward;

	std::deque<vector<uint32_t>> _videoHistory;
	vector<vector<uint32_t>> _videoHistoryBuilder;
	std::deque<int16_t> _audioHistory;
	vector<int16_t> _audioHistoryBuilder;

	void AddHistoryBlock();
	void PopHistory();

	void Start();
	void Stop();

	void ProcessFrame(void *frameBuffer, uint32_t width, uint32_t height);
	bool ProcessAudio(int16_t *soundBuffer, uint32_t sampleCount, uint32_t sampleRate);

public:
	RewindManager();
	~RewindManager();

	void ProcessNotification(ConsoleNotificationType type, void* parameter) override;
	void ProcessEndOfFrame();

	static void ClearBuffer();

	static void RecordInput(uint8_t port, uint8_t input);
	static uint8_t GetInput(uint8_t port);

	static void StartRewinding();
	static void StopRewinding();
	static bool IsRewinding();
	static void RewindSeconds(uint32_t seconds);

	static void SendFrame(void *frameBuffer, uint32_t width, uint32_t height);
	static bool SendAudio(int16_t *soundBuffer, uint32_t sampleCount, uint32_t sampleRate);
};