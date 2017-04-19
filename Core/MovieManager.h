#pragma once
#include "stdafx.h"

class IMovie
{
public:
	virtual void RecordState(uint8_t port, uint8_t value) = 0;
	virtual uint8_t GetState(uint8_t port) = 0;

	virtual void Record(string filename, bool reset) = 0;
	virtual void Play(stringstream &filestream, bool autoLoadRom, string filename = "") = 0;

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
	static void Play(std::stringstream &filestream, bool autoLoadRom);
	static void Stop();
	static bool Playing();
	static bool Recording();

	static void RecordState(uint8_t port, uint8_t value);
	static uint8_t GetState(uint8_t port);
};