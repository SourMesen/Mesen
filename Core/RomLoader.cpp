#include "stdafx.h"
#include <algorithm>
#include "../Utilities/FolderUtilities.h"
#include "../Utilities/CRC32.h"
#include "../Utilities/sha1.h"
#include "RomLoader.h"
#include "FileLoader.h"
#include "iNesLoader.h"
#include "FdsLoader.h"
#include "NsfLoader.h"
#include "NsfeLoader.h"
#include "UnifLoader.h"

bool RomLoader::LoadFile(string filename, int32_t archiveFileIndex)
{
	vector<uint8_t> fileData;
	if(FileLoader::LoadFile(filename, nullptr, archiveFileIndex, fileData)) {
		return LoadFile(filename, fileData);
	} else {
		return false;
	}
}

bool RomLoader::LoadFile(string filename, vector<uint8_t> &fileData)
{
	_filename = filename;

	string romName = FolderUtilities::GetFilename(filename, true);

	uint32_t crc = CRC32::GetCRC(fileData.data(), fileData.size());
	MessageManager::Log("");
	MessageManager::Log("Loading rom: " + romName);
	stringstream crcHex;
	crcHex << std::hex << std::uppercase << std::setfill('0') << std::setw(8) << crc;
	MessageManager::Log("File CRC32: 0x" + crcHex.str());

	if(memcmp(fileData.data(), "NES\x1a", 4) == 0) {
		iNesLoader loader;
		_romData = loader.LoadRom(fileData, nullptr);
	} else if(memcmp(fileData.data(), "FDS\x1a", 4) == 0 || memcmp(fileData.data(), "\x1*NINTENDO-HVC*", 15) == 0) {
		FdsLoader loader;
		_romData = loader.LoadRom(fileData, _filename);
	} else if(memcmp(fileData.data(), "NESM\x1a", 5) == 0) {
		NsfLoader loader;
		_romData = loader.LoadRom(fileData);
	} else if(memcmp(fileData.data(), "NSFE", 4) == 0) {
		NsfeLoader loader;
		_romData = loader.LoadRom(fileData);
	} else if(memcmp(fileData.data(), "UNIF", 4) == 0) {
		UnifLoader loader;
		_romData = loader.LoadRom(fileData);
	} else {
		NESHeader header = {};
		if(GameDatabase::GetiNesHeader(crc, header)) {
			MessageManager::Log("[DB] Headerless ROM file found - using game database data.");
			iNesLoader loader;
			_romData = loader.LoadRom(fileData, &header);
		} else {
			MessageManager::Log("Invalid rom file.");
			_romData.Error = true;
		}
	}

	_romData.Crc32 = crc;
	_romData.Sha1 = SHA1::GetHash(fileData);
	_romData.RawData = fileData;
	_romData.RomName = romName;
	_romData.Filename = _filename;

	if(_romData.System == GameSystem::Unknown) {
		//Use filename to detect PAL/VS system games
		string name = _romData.Filename;
		std::transform(name.begin(), name.end(), name.begin(), ::tolower);

		if(name.find("(e)") != string::npos || name.find("(australia)") != string::npos || name.find("(europe)") != string::npos ||
			name.find("(germany)") != string::npos || name.find("(spain)") != string::npos) {
			_romData.System = GameSystem::NesPal;
		} else if(name.find("(vs)") != string::npos) {
			_romData.System = GameSystem::VsUniSystem;
		}
	}

	return !_romData.Error;
}

RomData RomLoader::GetRomData()
{
	return _romData;
}

int32_t RomLoader::FindMatchingRomInFile(string filename, HashInfo hashInfo)
{
	vector<uint8_t> fileData;
	int fileIndex = 0;
	while(FileLoader::LoadFile(filename, nullptr, fileIndex, fileData)) {
		RomLoader loader;
		if(loader.LoadFile(filename, fileData)) {
			if(hashInfo.Crc32Hash == loader._romData.Crc32 || hashInfo.Sha1Hash.compare(loader._romData.Sha1) == 0) {
				return fileIndex;
			}
			fileIndex++;
		}
	}
	return -1;
}

string RomLoader::FindMatchingRomInFolder(string folder, string romFilename, HashInfo hashInfo, bool useFastSearch, int32_t &archiveFileIndex)
{
	std::transform(romFilename.begin(), romFilename.end(), romFilename.begin(), ::tolower);
	vector<string> validExtensions = { { ".nes", ".zip", ".7z", ".fds" } };
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
				archiveFileIndex = RomLoader::FindMatchingRomInFile(romFile, hashInfo);
				if(archiveFileIndex >= 0) {
					return originalFilename;
				}
			}
		}
	} else {
		for(string romFile : romFiles) {
			//Slower search by CRC value
			archiveFileIndex = RomLoader::FindMatchingRomInFile(romFile, hashInfo);
			if(archiveFileIndex >= 0) {
				return romFile;
			}
		}
	}

	archiveFileIndex = -1;
	return "";
}