#include "stdafx.h"
#include "ZIPReader.h"

ZIPReader::ZIPReader()
{
	memset(&_zipArchive, 0, sizeof(mz_zip_archive));
}

ZIPReader::~ZIPReader()
{
	if(_initialized) {
		mz_zip_reader_end(&_zipArchive);
	}
}

void ZIPReader::LoadZIPArchive(void* buffer, size_t size)
{
	if(mz_zip_reader_init_mem(&_zipArchive, buffer, size, 0)) {
		_initialized = true;
	}
}

vector<string> ZIPReader::GetFileList()
{
	vector<string> fileList;
	if(_initialized) {
		for(int i = 0, len = (int)mz_zip_reader_get_num_files(&_zipArchive); i < len; i++) {
			mz_zip_archive_file_stat file_stat;
			if(!mz_zip_reader_file_stat(&_zipArchive, i, &file_stat)) {
				std::cout << "mz_zip_reader_file_stat() failed!" << std::endl;
			}

			fileList.push_back(file_stat.m_filename);
		}
	}
	return fileList;
}

void ZIPReader::ExtractFile(string filename, uint8_t **fileBuffer, size_t &fileSize)
{
	if(_initialized) {
		size_t uncompSize;
		void *p = mz_zip_reader_extract_file_to_heap(&_zipArchive, filename.c_str(), &uncompSize, 0);
		if(!p) {
			std::cout << "mz_zip_reader_extract_file_to_heap() failed!" << std::endl;
		}

		*fileBuffer = new uint8_t[uncompSize];
		memcpy(*fileBuffer, p, uncompSize);

		// We're done.
		mz_free(p);

		fileSize = uncompSize;
	}
}
