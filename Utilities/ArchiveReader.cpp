#include "stdafx.h"
#include "ArchiveReader.h"
#include <string.h>
#include <sstream>
#include <algorithm>
#include "FolderUtilities.h"

ArchiveReader::~ArchiveReader()
{
	if(_buffer) {
		delete[] _buffer;
		_buffer = nullptr;
	}
}

std::stringstream ArchiveReader::GetStream(string filename)
{
	std::stringstream ss;
	if(_initialized) {
		uint8_t* buffer = nullptr;
		size_t size = 0;

		ExtractFile(filename, &buffer, size);
		ss.write((char*)buffer, size);

		delete[] buffer;
	}

	return ss;
}

vector<string> ArchiveReader::GetFileList(std::initializer_list<string> extensions)
{
	vector<string> filenames;
	for(string filename : InternalGetFileList()) {
		string lcFilename = filename;
		std::transform(lcFilename.begin(), lcFilename.end(), lcFilename.begin(), ::tolower);
		if(filename.length() > 4) {
			for(string ext : extensions) {
				if(lcFilename.substr(lcFilename.length() - 4, 4).compare(ext) == 0) {
					filenames.push_back(filename);
				}
			}
		}
	}

	return filenames;
}

bool ArchiveReader::LoadArchive(void* buffer, size_t size)
{
	if(InternalLoadArchive(buffer, size)) {
		_initialized = true;
		return true;
	}
	return false;
}

bool ArchiveReader::LoadArchive(string filename)
{
	ifstream in(filename, std::ios::binary | std::ios::in);
	if(in) {
		in.seekg(0, std::ios::end);
		std::streampos filesize = in.tellg();
		in.seekg(0, std::ios::beg);

		if(_buffer) {
			delete[] _buffer;
			_buffer = nullptr;
		}

		_buffer = new uint8_t[(uint32_t)filesize];
		in.read((char*)_buffer, filesize);
		bool result = LoadArchive(_buffer, (size_t)filesize);
		return result;
	}
	return false;
}