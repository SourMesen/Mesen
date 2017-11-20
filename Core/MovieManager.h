#pragma once
#include "stdafx.h"
#include "MessageManager.h"
#include "EmulationSettings.h"
#include "IInputProvider.h"

class MovieRecorder;
class VirtualFile;

class IMovie : public IInputProvider
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
	virtual bool Play(VirtualFile &file) = 0;
	virtual bool IsPlaying() = 0;
};

class MovieManager
{
private:
	static shared_ptr<IMovie> _player;
	static shared_ptr<MovieRecorder> _recorder;

public:
	static void Record(string filename, bool reset);
	static void Play(VirtualFile file);
	static void Stop();
	static bool Playing();
	static bool Recording();
};