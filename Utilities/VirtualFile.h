#pragma once
#include "stdafx.h"
#include <algorithm>
#include <iterator>
#include <sstream>
#include "sha1.h"
#include "ArchiveReader.h"
#include "StringUtilities.h"
#include "FolderUtilities.h"
#include "BpsPatcher.h"
#include "IpsPatcher.h"
#include "UpsPatcher.h"

class VirtualFile
{
private:
	string _path = "";
	string _innerFile = "";
	vector<uint8_t> _data;

	void FromStream(std::istream &input, vector<uint8_t> &output)
	{
		input.seekg(0, std::ios::end);
		uint32_t fileSize = (uint32_t)input.tellg();
		input.seekg(0, std::ios::beg);

		output.resize(fileSize, 0);
		input.read((char*)output.data(), fileSize);
	}

	void LoadFile()
	{
		if(_data.size() == 0) {
			if(!_innerFile.empty()) {
				shared_ptr<ArchiveReader> reader = ArchiveReader::GetReader(_path);
				if(reader) {
					reader->ExtractFile(_innerFile, _data);
				}
			} else {
				ifstream input(_path, std::ios::in | std::ios::binary);
				if(input.good()) {
					FromStream(input, _data);
				}
			}
		}
	}

public:
	VirtualFile()
	{
	}

	VirtualFile(const string &archivePath, const string innerFile)
	{
		_path = archivePath;
		_innerFile = innerFile;
	}

	VirtualFile(const string &file)
	{
		vector<string> tokens = StringUtilities::Split(file, '\x1');
		_path = tokens[0];
		if(tokens.size() > 1) {
			_innerFile = tokens[1];
		}
	}

	VirtualFile(std::istream &input, string filePath)
	{
		_path = filePath;
		FromStream(input, _data);
	}

	operator std::string() const 
	{ 
		if(_innerFile.empty()) {
			return _path;
		} else if(_path.empty()) {
			throw std::runtime_error("Cannot convert to string");
		} else {
			return _path + "\x1" + _innerFile;
		}
	}
	
	bool IsValid()
	{
		LoadFile();
		return _data.size() > 0;
	}

	string GetFilePath()
	{
		return _path;
	}

	string GetFolderPath()
	{
		return FolderUtilities::GetFolderName(_path);
	}

	string GetFileName()
	{
		return _innerFile.empty() ? FolderUtilities::GetFilename(_path, true) : _innerFile;
	}

	string GetSha1Hash()
	{
		string hash = SHA1::GetHash(_data);
		std::transform(hash.begin(), hash.end(), hash.begin(), ::tolower);
		return hash;
	}

	bool ReadFile(vector<uint8_t> &out)
	{
		LoadFile();
		if(_data.size() > 0) {
			out.resize(_data.size(), 0);
			std::copy(_data.begin(), _data.end(), out.begin());
			return true;
		}
		return false;
	}

	bool ReadFile(std::stringstream &out)
	{
		LoadFile();
		if(_data.size() > 0) {
			out.write((char*)_data.data(), _data.size());
			return true;
		}
		return false;
	}

	bool ApplyPatch(VirtualFile &patch)
	{
		//Apply patch file
		bool result = false;
		if(patch.IsValid() && patch._data.size() >= 5 ) {
			vector<uint8_t> patchedData;
			std::stringstream ss;
			patch.ReadFile(ss);
			
			if(memcmp(patch._data.data(), "PATCH", 5) == 0) {
				result = IpsPatcher::PatchBuffer(ss, _data, patchedData);
			} else if(memcmp(patch._data.data(), "UPS1", 4) == 0) {
				result = UpsPatcher::PatchBuffer(ss, _data, patchedData);
			} else if(memcmp(patch._data.data(), "BPS1", 4) == 0) {
				result = BpsPatcher::PatchBuffer(ss, _data, patchedData);
			}
			if(result) {
				_data = patchedData;
			}
		}
		return result;
	}
};