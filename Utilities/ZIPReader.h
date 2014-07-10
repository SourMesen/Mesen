#pragma once
#include "stdafx.h"
#include "miniz.h"

class ZIPReader
{
private:
	mz_zip_archive _zipArchive;
	bool _initialized = false;

public:
	ZIPReader();
	~ZIPReader();

	void LoadZIPArchive(void* buffer, size_t size);

	vector<string> GetFileList();
	void ExtractFile(string filename, uint8_t **fileBuffer, size_t &fileSize);
};