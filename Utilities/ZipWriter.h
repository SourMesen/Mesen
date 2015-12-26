#pragma once
#include "stdafx.h"
#include "miniz.h"

class ZipWriter
{
private:
	mz_zip_archive _zipArchive;
	string _zipFilename;

public:
	ZipWriter(string zipFilename);
	~ZipWriter();

	void AddFile(string filepath, string zipFilename);
};