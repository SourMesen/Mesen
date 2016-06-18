#pragma once
#include "stdafx.h"
#include "../Utilities/ZipReader.h"
#include "../Utilities/SZReader.h"
#include "RomLoader.h"
#include "iNesLoader.h"
#include "FdsLoader.h"

vector<string> RomLoader::GetArchiveRomList(string filename)
{
	ifstream in(filename, ios::in | ios::binary);
	if(in) {
		uint8_t header[2];
		in.read((char*)header, 2);
		in.close();

		if(memcmp(header, "PK", 2) == 0) {
			ZipReader reader;
			reader.LoadArchive(filename);
			return reader.GetFileList({ ".nes", ".fds" });
		} else if(memcmp(header, "7z", 2) == 0) {
			SZReader reader;
			reader.LoadArchive(filename);
			return reader.GetFileList({ ".nes", ".fds" });
		}
	}
	return{};
}

bool RomLoader::LoadFromArchive(istream &zipFile, ArchiveReader& reader, int32_t archiveFileIndex)
{
	bool result = false;

	uint32_t fileSize;
	uint8_t* buffer = ReadFile(zipFile, fileSize);

	reader.LoadArchive(buffer, fileSize);

	vector<string> fileList = reader.GetFileList({ ".nes", ".fds" });
	int32_t currentIndex = 0;
	if(archiveFileIndex > (int32_t)fileList.size()) {
		return false;
	}

	for(string filename : fileList) {
		if(archiveFileIndex == -1 || archiveFileIndex == currentIndex) {
			uint8_t* fileBuffer = nullptr;
			size_t size = 0;
			reader.ExtractFile(filename, &fileBuffer, size);
			if(fileBuffer) {
				result = LoadFromMemory(fileBuffer, size, FolderUtilities::GetFilename(filename, true));
				delete[] fileBuffer;
				break;
			}
		}
		currentIndex++;
	}

	delete[] buffer;
	return result;
}

bool RomLoader::LoadFromStream(istream &romFile, string romName)
{
	uint32_t fileSize;
	uint8_t* buffer = ReadFile(romFile, fileSize);
	bool result = LoadFromMemory(buffer, fileSize, romName);
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

bool RomLoader::LoadFromMemory(uint8_t* buffer, size_t length, string romName)
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
	_romData.RomName = romName;
	_romData.Filename = _filename;

	if(_romData.System == GameSystem::Unknown) {
		//Use filename to detect PAL/VS system games
		if(_filename.find("(e)") != string::npos || _filename.find("(E)") != string::npos) {
			_romData.System = GameSystem::NesPal;
		} else if(_filename.find("(VS)") != string::npos || _filename.find("(vs)") != string::npos || _filename.find("(Vs)") != string::npos || _filename.find("(vS)") != string::npos) {
			_romData.System = GameSystem::VsUniSystem;
		}
	}

	return !_romData.Error;
}

bool RomLoader::LoadFile(string filename, istream *filestream, string ipsFilename, int32_t archiveFileIndex)
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
		ZipReader reader;
		return LoadFromArchive(*input, reader, archiveFileIndex);
	} else if(memcmp(header, "7z", 2) == 0) {
		SZReader reader;
		return LoadFromArchive(*input, reader, archiveFileIndex);
	} else if(memcmp(header, "NES\x1a", 4) == 0 || memcmp(header, "FDS\x1a", 4) == 0 || memcmp(header, "\x1*NINTENDO-HVC*", 15) == 0) {
		if(archiveFileIndex > 0) {
			return false;
		}

		return LoadFromStream(*input, FolderUtilities::GetFilename(filename, true));
	}
	return false;
}

RomData RomLoader::GetRomData()
{
	return _romData;
}

int32_t RomLoader::FindMatchingRomInFile(string filename, uint32_t crc32Hash)
{
	RomLoader loader;
	int32_t fileIndex = 0;
	while(loader.LoadFile(filename, nullptr, "", fileIndex)) {
		if(crc32Hash == loader._romData.Crc32) {
			return fileIndex;
		}
		fileIndex++;
	}
	return -1;
}

string RomLoader::FindMatchingRomInFolder(string folder, string romFilename, uint32_t crc32Hash, bool useFastSearch, int32_t &archiveFileIndex)
{
	std::transform(romFilename.begin(), romFilename.end(), romFilename.begin(), ::tolower);
	vector<string> validExtensions = { { "*.nes", "*.zip", "*.7z", "*.fds" } };
	vector<string> romFiles;

	for(string extension : validExtensions) {
		for(string file : FolderUtilities::GetFilesInFolder(folder, extension, true)) {
			romFiles.push_back(file);
		}
	}

	if(useFastSearch) {
		for(string romFile : romFiles) {
			//Quick search by filename
			string originalFilename = romFile;
			std::transform(romFile.begin(), romFile.end(), romFile.begin(), ::tolower);
			if(FolderUtilities::GetFilename(romFile, true).compare(romFilename) == 0) {
				archiveFileIndex = RomLoader::FindMatchingRomInFile(romFile, crc32Hash);
				if(archiveFileIndex >= 0) {
					return originalFilename;
				}
			}
		}
	} else {
		for(string romFile : romFiles) {
			//Slower search by CRC value
			archiveFileIndex = RomLoader::FindMatchingRomInFile(romFile, crc32Hash);
			if(archiveFileIndex >= 0) {
				return romFile;
			}
		}
	}

	archiveFileIndex = -1;
	return "";
}