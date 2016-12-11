#include "stdafx.h"
#include <string>
#include <cstring>
#include <sstream>
#include "ZipWriter.h"
#include "FolderUtilities.h"

ZipWriter::ZipWriter(string zipFilename)
{
	_zipFilename = zipFilename;
	memset(&_zipArchive, 0, sizeof(mz_zip_archive));
	mz_zip_writer_init_file(&_zipArchive, _zipFilename.c_str(), 0);
}

ZipWriter::~ZipWriter()
{
	mz_zip_writer_finalize_archive(&_zipArchive);
	mz_zip_writer_end(&_zipArchive);
}

void ZipWriter::AddFile(string filepath, string zipFilename)
{
	if(!mz_zip_writer_add_file(&_zipArchive, zipFilename.c_str(), filepath.c_str(), "", 0, MZ_BEST_COMPRESSION)) {
		std::cout << "mz_zip_writer_add_file() failed!" << std::endl;
	}
}

void ZipWriter::AddFile(std::stringstream &filestream, string zipFilename)
{
	filestream.seekg(0, std::ios::end);
	size_t bufferSize = (size_t)filestream.tellg();
	filestream.seekg(0, std::ios::beg);

	uint8_t* buffer = new uint8_t[bufferSize];
	filestream.read((char*)buffer, bufferSize);

	if(!	mz_zip_writer_add_mem(&_zipArchive, zipFilename.c_str(), buffer, bufferSize, MZ_BEST_COMPRESSION)) {
		std::cout << "mz_zip_writer_add_file() failed!" << std::endl;
	}
}
