#pragma once
#include "stdafx.h"
#include "MovieManager.h"
#include "../Utilities/ZipReader.h"

class BizhawkMovie : public IMovie, public INotificationListener
{
private:
	bool InitializeGameData(ZipReader &reader);
	bool InitializeInputData(ZipReader &reader);

protected:
	vector<uint32_t> _systemActionByFrame;
	vector<uint8_t> _dataByFrame[4];
	bool _isPlaying = false;
	RamPowerOnState _originalPowerOnState;

public:
	BizhawkMovie();
	virtual ~BizhawkMovie();

	void RecordState(uint8_t port, uint8_t value) override;
	void Record(string filename, bool reset) override;

	uint8_t GetState(uint8_t port) override;

	virtual bool Play(stringstream &filestream, bool autoLoadRom) override;

	bool IsRecording() override;
	bool IsPlaying() override;

	void ProcessNotification(ConsoleNotificationType type, void* parameter) override;
};