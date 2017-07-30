#include "stdafx.h"
#include "ArchiveReader.h"
#include <string.h>
#include <sstream>
#include <algorithm>
#include "FolderUtilities.h"
#include "ZipReader.h"
#include "SZReader.h"

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
		vector<uint8_t> fileData;
		ExtractFile(filename, fileData);
		ss.write((char*)fileData.data(), fileData.size());
	}
	return ss;
}

vector<string> ArchiveReader::GetFileList(std::initializer_list<string> extensions)
{
	if(extensions.size() == 0) {
		return InternalGetFileList();
	}

	vector<string> filenames;
	for(string filename : InternalGetFileList()) {
		string lcFilename = filename;
		std::transform(lcFilename.begin(), lcFilename.end(), lcFilename.begin(), ::tolower);
		for(string ext : extensions) {
			if(lcFilename.size() >= ext.size()) {
				if(lcFilename.substr(lcFilename.length() - ext.size(), ext.size()).compare(ext) == 0) {
					filenames.push_back(filename);
				}
			}
		}
	}

	return filenames;
}

bool ArchiveReader::LoadArchive(std::istream &in)
{
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
	if(in.good()) {
		LoadArchive(in);
		in.close();
	}
	return false;
}

shared_ptr<ArchiveReader> ArchiveReader::GetReader(string filepath)
{
	ifstream in(filepath, std::ios::in | std::ios::binary);
	if(in) {
		uint8_t header[2] = { 0,0 };
		in.read((char*)header, 2);
		in.close();

		shared_ptr<ArchiveReader> reader;
		if(memcmp(header, "PK", 2) == 0) {
			reader.reset(new ZipReader());
		} else if(memcmp(header, "7z", 2) == 0) {
			reader.reset(new SZReader());
		}

		if(reader) {
			reader->LoadArchive(filepath);
			return reader;
		}
	}
	return nullptr;
}