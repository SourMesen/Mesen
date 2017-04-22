#pragma once
#include "stdafx.h"
#include "MessageManager.h"
#include "EmulationSettings.h"

class IMovie
{
protected:
	void EndMovie()
	{
		MessageManager::DisplayMessage("Movies", "MovieEnded");
		MessageManager::SendNotification(ConsoleNotificationType::MovieEnded);
		if(EmulationSettings::CheckFlag(EmulationFlags::PauseOnMovieEnd)) {
			EmulationSettings::SetFlags(EmulationFlags::Paused);
		}
	}

public:
	virtual void RecordState(uint8_t port, uint8_t value) = 0;
	virtual uint8_t GetState(uint8_t port) = 0;

	virtual void Record(string filename, bool reset) = 0;
	virtual bool Play(stringstream &filestream, bool autoLoadRom) = 0;

	virtual bool IsRecording() = 0;
	virtual bool IsPlaying() = 0;
};

class MovieManager
{
private:
	static shared_ptr<IMovie> _instance;

public:
	static void Record(string filename, bool reset);
	static void Play(string filename);
	static bool Play(std::stringstream &filestream, bool autoLoadRom);
	static void Stop();
	static bool Playing();
	static bool Recording();

	static void RecordState(uint8_t port, uint8_t value);
	static uint8_t GetState(uint8_t port);
};