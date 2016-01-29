#pragma once
#include "stdafx.h"
#include "RomLoader.h"
#include "iNesLoader.h"
#include "FdsLoader.h"

bool RomLoader::LoadFromZip(istream &zipFile)
{
	bool result = false;

	uint32_t fileSize;
	uint8_t* buffer = ReadFile(zipFile, fileSize);

	ZipReader reader;
	reader.LoadZipArchive(buffer, fileSize);

	vector<string> fileList = reader.GetFileList();
	for(string filename : fileList) {
		std::transform(filename.begin(), filename.end(), filename.begin(), ::tolower);
		if(filename.length() > 4) {
			if(filename.substr(filename.length() - 4, 4).compare(".nes") == 0 || filename.substr(filename.length() - 4, 4).compare(".fds") == 0) {
				uint8_t* fileBuffer = nullptr;
				size_t fileSize = 0;
				reader.ExtractFile(filename, &fileBuffer, fileSize);
				if(fileBuffer) {
					result = LoadFromMemory(fileBuffer, fileSize);
					delete[] fileBuffer;
					break;
				}
			}
		}

	}

	delete[] buffer;
	return result;
}

bool RomLoader::LoadFromStream(istream &romFile)
{
	uint32_t fileSize;
	uint8_t* buffer = ReadFile(romFile, fileSize);
	bool result = LoadFromMemory(buffer, fileSize);
	delete[] buffer;

	return result;
}

uint32_t RomLoader::GetFileSize(istream &file)
{
	file.seekg(0, ios::end);
	uint32_t fileSize = (uint32_t)file.tellg();
	file.seekg(0, ios::beg);

	return fileSize;
}

uint8_t* RomLoader::ReadFile(istream &file, uint32_t &fileSize)
{
	fileSize = GetFileSize(file);

	uint8_t* buffer = new uint8_t[fileSize];
	file.read((char*)buffer, fileSize);
	return buffer;
}

bool RomLoader::LoadFromMemory(uint8_t* buffer, size_t length)
{
	vector<uint8_t> fileData(buffer, buffer + length);

	if(!_ipsFilename.empty()) {
		//Apply IPS patch
		fileData = IpsPatcher::PatchBuffer(_ipsFilename, fileData);
	}

	if(memcmp(buffer, "NES\x1a", 4) == 0) {
		iNesLoader loader;
		_romData = loader.LoadRom(fileData);
	} else if(memcmp(buffer, "FDS\x1a", 4) == 0 || memcmp(buffer, "\x1*NINTENDO-HVC*", 15) == 0) {
		FdsLoader loader;
		_romData = loader.LoadRom(fileData, _filename);
	} else {
		_romData.Error = true;
	}

	_romData.RawData = fileData;
	_romData.Crc32 = CRC32::GetCRC(buffer, length);

	return !_romData.Error;
}

bool RomLoader::LoadFile(string filename, istream *filestream, string ipsFilename)
{
	_filename = filename;
	_ipsFilename = ipsFilename;

	ifstream file;
	istream* input = nullptr;
	if(!filestream) {
		file.open(filename, ios::in | ios::binary);
		if(file) {
			input = &file;
		}
	} else {
		input = filestream;
	}
	

	char header[15];
	input->seekg(0, ios::beg);
	input->read(header, 15);
	input->seekg(0, ios::beg);

	if(memcmp(header, "PK", 2) == 0) {
		return LoadFromZip(*input);
	} else if(memcmp(header, "NES\x1a", 4) == 0 || memcmp(header, "FDS\x1a", 4) == 0 || memcmp(header, "\x1*NINTENDO-HVC*", 15) == 0) {
		return LoadFromStream(*input);
	}
	return false;
}

RomData RomLoader::GetRomData()
{
	_romData.Filename = _filename;
	return _romData;
}

uint32_t RomLoader::GetCRC32(string filename)
{
	RomLoader loader;
	uint32_t crc = 0;
	if(loader.LoadFile(filename)) {
		crc = loader._romData.Crc32;
	}
	return crc;
}

string RomLoader::FindMatchingRomInFolder(string folder, string romFilename, uint32_t crc32Hash)
{
	std::transform(romFilename.begin(), romFilename.end(), romFilename.begin(), ::tolower);
	vector<string> validExtensions = { { "*.nes", "*.zip", "*.fds" } };
	vector<string> romFiles;

	for(string extension : validExtensions) {
		for(string file : FolderUtilities::GetFilesInFolder(folder, extension, true)) {
			romFiles.push_back(file);
		}
	}

	for(string romFile : romFiles) {
		//Quick search by filename
		string originalFilename = romFile;
		std::transform(romFile.begin(), romFile.end(), romFile.begin(), ::tolower);
		if(FolderUtilities::GetFilename(romFile, true).compare(romFilename) == 0) {
			if(RomLoader::GetCRC32(romFile) == crc32Hash) {
				return originalFilename;
			}
		}
	}

	for(string romFile : romFiles) {
		//Slower search by CRC value
		if(RomLoader::GetCRC32(romFile) == crc32Hash) {
			//Matching ROM found
			return romFile;
		}
	}

	return "";
}