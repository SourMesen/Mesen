#include "stdafx.h"
#include "../Utilities/FolderUtilities.h"
#include "MovieManager.h"
#include "MesenMovie.h"
#include "BizhawkMovie.h"

shared_ptr<IMovie> MovieManager::_instance;

void MovieManager::Record(string filename, bool reset)
{
	shared_ptr<IMovie> movie(new MesenMovie());
	movie->Record(filename, reset);
	_instance = movie;
}

void MovieManager::Play(string filename)
{
	ifstream file(filename, ios::in | ios::binary);
	if(file.good()) {
		std::stringstream ss;
		ss << file.rdbuf();
		file.close();
		if(MovieManager::Play(ss, true)) {
			MessageManager::DisplayMessage("Movies", "MoviePlaying", FolderUtilities::GetFilename(filename, true));
		}
	}
}

bool MovieManager::Play(std::stringstream &filestream, bool autoLoadRom)
{
	char header[3] = { };
	filestream.read(header, 3);
	filestream.seekg(0, ios::beg);

	if(memcmp(header, "MMO", 3) == 0) {
		shared_ptr<IMovie> movie(new MesenMovie());
		if(movie->Play(filestream, autoLoadRom)) {
			_instance = movie;
			return true;
		}
	} else if(memcmp(header, "PK", 2) == 0) {
		shared_ptr<IMovie> movie(new BizhawkMovie());
		if(movie->Play(filestream, autoLoadRom)) {
			_instance = movie;
			return true;
		}
	}

	return false;
}

void MovieManager::Stop()
{
	if(_instance && _instance->IsPlaying()) {
		MessageManager::DisplayMessage("Movies", "MovieEnded");
	}
	_instance.reset();
}

bool MovieManager::Playing()
{
	if(_instance) {
		return _instance->IsPlaying();
	} else {
		return false;
	}
}

bool MovieManager::Recording()
{
	if(_instance) {
		return _instance->IsRecording();
	} else {
		return false;
	}
}

void MovieManager::RecordState(uint8_t port, uint8_t value)
{
	if(_instance) {
		_instance->RecordState(port, value);
	}
}

uint8_t MovieManager::GetState(uint8_t port)
{
	if(_instance) {
		return _instance->GetState(port);
	}
	return 0;
}