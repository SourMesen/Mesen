#pragma once
#include "stdafx.h"
#include "miniz.h"
#include "ArchiveReader.h"

class ZipReader : public ArchiveReader
{
private:
	mz_zip_archive _zipArchive;

protected:
	bool InternalLoadArchive(void* buffer, size_t size);
	vector<string> InternalGetFileList();

public:
	ZipReader();
	virtual ~ZipReader();

	void ExtractFile(string filename, uint8_t **fileBuffer, size_t &fileSize);
};