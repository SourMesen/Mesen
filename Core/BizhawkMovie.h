#pragma once
#include "stdafx.h"
#include "MovieManager.h"
#include "../Utilities/ZipReader.h"

class VirtualFile;

class BizhawkMovie : public IMovie
{
private:
	bool InitializeGameData(ZipReader &reader);
	bool InitializeInputData(ZipReader &reader);
	void Stop();

protected:
	vector<uint32_t> _systemActionByFrame;
	vector<string> _dataByFrame[4];
	bool _isPlaying = false;
	RamPowerOnState _originalPowerOnState;

public:
	BizhawkMovie();
	virtual ~BizhawkMovie();

	bool SetInput(BaseControlDevice *device) override;
	bool Play(VirtualFile &file) override;
	bool IsPlaying() override;
};