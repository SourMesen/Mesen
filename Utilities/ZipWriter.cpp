#include "stdafx.h"
#include <string.h>
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
