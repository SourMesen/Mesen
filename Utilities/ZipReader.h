#pragma once
#include "stdafx.h"
#include "miniz.h"

class ZipReader
{
private:
	mz_zip_archive _zipArchive;
	bool _initialized = false;

public:
	ZipReader();
	~ZipReader();

	void LoadZipArchive(void* buffer, size_t size);
	void LoadZipArchive(string filepath);

	vector<string> GetFileList();
	void ExtractFile(string filename, uint8_t **fileBuffer, size_t &fileSize);
	std::stringstream ExtractFile(string filename);
};