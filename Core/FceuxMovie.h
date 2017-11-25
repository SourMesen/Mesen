#pragma once
#include "stdafx.h"
#include "../Utilities/ZipReader.h"
#include "MovieManager.h"
#include "BizhawkMovie.h"

class FceuxMovie : public BizhawkMovie
{
private:
	bool InitializeData(stringstream &filestream);

public:
	bool Play(VirtualFile &file) override;
};