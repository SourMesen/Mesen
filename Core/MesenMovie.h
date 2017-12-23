#pragma once

#include "stdafx.h"
#include "CheatManager.h"
#include "MovieManager.h"
#include "ControlManager.h"
#include "BatteryManager.h"
#include "VirtualFile.h"

class ZipReader;

class MesenMovie : public IMovie, public IBatteryProvider, public std::enable_shared_from_this<MesenMovie>
{
private:
	VirtualFile _movieFile;
	shared_ptr<ZipReader> _reader;
	bool _playing = false;
	size_t _deviceIndex = 0;
	vector<vector<string>> _inputData;
	vector<string> _cheats;
	std::unordered_map<string, string> _settings;
	string _filename;

private:
	void ParseSettings(stringstream &data);
	void ApplySettings();
	bool LoadGame();
	void Stop();

	uint32_t LoadInt(std::unordered_map<string, string> &settings, string name);
	bool LoadBool(std::unordered_map<string, string> &settings, string name);
	string LoadString(std::unordered_map<string, string> &settings, string name);
	void LoadCheats();
	bool LoadCheat(string cheatData, CodeInfo &code);

public:
	MesenMovie();
	~MesenMovie();

	bool Play(VirtualFile &file) override;
	bool SetInput(BaseControlDevice* device) override;
	bool IsPlaying() override;

	// Inherited via IBatteryProvider
	virtual vector<uint8_t> LoadBattery(string extension) override;
};