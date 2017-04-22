#pragma once
#include "stdafx.h"

class ArchiveReader
{
protected:
	bool _initialized = false;
	uint8_t* _buffer = nullptr;
	virtual bool InternalLoadArchive(void* buffer, size_t size) = 0;
	virtual vector<string> InternalGetFileList() = 0;
public:
	~ArchiveReader();

	bool LoadArchive(void* buffer, size_t size);
	bool LoadArchive(string filename);
	bool LoadArchive(std::istream &in);

	std::stringstream GetStream(string filename);

	vector<string> GetFileList(std::initializer_list<string> extensions);

	virtual void ExtractFile(string filename, uint8_t **fileBuffer, size_t &fileSize) = 0;
};