#include "stdafx.h"
#include "../Utilities/FolderUtilities.h"
#include "../Utilities/ArchiveReader.h"
#include "../Utilities/CRC32.h"
#include "../Utilities/sha1.h"
#include "../Utilities/BpsPatcher.h"
#include "../Utilities/IpsPatcher.h"
#include "../Utilities/UpsPatcher.h"
#include "../Utilities/ZipReader.h"
#include "../Utilities/SZReader.h"
#include "RomLoader.h"
#include "iNesLoader.h"
#include "FdsLoader.h"
#include "NsfLoader.h"
#include "NsfeLoader.h"
#include "UnifLoader.h"

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
			return reader.GetFileList({ ".nes", ".fds", ".nsf", ".nsfe", "*.unf" });
		} else if(memcmp(header, "7z", 2) == 0) {
			SZReader reader;
			reader.LoadArchive(filename);
			return reader.GetFileList({ ".nes", ".fds", ".nsf", ".nsfe", "*.unf" });
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

	vector<string> fileList = reader.GetFileList({ ".nes", ".fds", ".nsf", ".nsfe", ".unf" });
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

void RomLoader::ApplyPatch(string patchPath, vector<uint8_t> &data)
{
	//Apply patch file
	MessageManager::DisplayMessage("Patch", "ApplyingPatch", FolderUtilities::GetFilename(patchPath, true));
	ifstream patchFile(patchPath, ios::binary | ios::in);
	if(patchFile.good()) {
		char buffer[5] = {};
		patchFile.read(buffer, 5);
		patchFile.close();
		if(memcmp(buffer, "PATCH", 5) == 0) {
			data = IpsPatcher::PatchBuffer(patchPath, data);
		} else if(memcmp(buffer, "UPS1", 4) == 0) {
			data = UpsPatcher::PatchBuffer(patchPath, data);
		} else if(memcmp(buffer, "BPS1", 4) == 0) {
			data = BpsPatcher::PatchBuffer(patchPath, data);
		}
	}
}

bool RomLoader::LoadFromMemory(uint8_t* buffer, size_t length, string romName)
{
	vector<uint8_t> fileData(buffer, buffer + length);
	
	if(!_patchFilename.empty()) {
		ApplyPatch(_patchFilename, fileData);
	}

	uint32_t crc = CRC32::GetCRC(buffer, length);
	MessageManager::Log("");
	MessageManager::Log("Loading rom: " + romName);
	stringstream crcHex;
	crcHex << std::hex << std::uppercase << std::setfill('0') << std::setw(8) << crc;
	MessageManager::Log("File CRC32: 0x" + crcHex.str());

	if(memcmp(buffer, "NES\x1a", 4) == 0) {
		iNesLoader loader;
		_romData = loader.LoadRom(fileData, nullptr);
	} else if(memcmp(buffer, "FDS\x1a", 4) == 0 || memcmp(buffer, "\x1*NINTENDO-HVC*", 15) == 0) {
		FdsLoader loader;
		_romData = loader.LoadRom(fileData, _filename);
	} else if(memcmp(buffer, "NESM\x1a", 5) == 0) {
		NsfLoader loader;
		_romData = loader.LoadRom(fileData);
	} else if(memcmp(buffer, "NSFE", 4) == 0) {
		NsfeLoader loader;
		_romData = loader.LoadRom(fileData);
	} else if(memcmp(buffer, "UNIF", 4) == 0) {
		UnifLoader loader;
		_romData = loader.LoadRom(fileData);
	} else {
		NESHeader header = { };
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
		if(_filename.find("(e)") != string::npos || _filename.find("(E)") != string::npos) {
			_romData.System = GameSystem::NesPal;
		} else if(_filename.find("(VS)") != string::npos || _filename.find("(vs)") != string::npos || _filename.find("(Vs)") != string::npos || _filename.find("(vS)") != string::npos) {
			_romData.System = GameSystem::VsUniSystem;
		}
	}

	return !_romData.Error;
}

bool RomLoader::LoadFile(string filename, istream *filestream, string patchFilename, int32_t archiveFileIndex)
{
	_filename = filename;
	_patchFilename = patchFilename;

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
	
	if(input) {
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
		} else {
			if(archiveFileIndex > 0) {
				return false;
			}

			return LoadFromStream(*input, FolderUtilities::GetFilename(filename, true));
		}
	}
	return false;
}

RomData RomLoader::GetRomData()
{
	return _romData;
}

int32_t RomLoader::FindMatchingRomInFile(string filename, HashInfo hashInfo)
{
	RomLoader loader;
	int32_t fileIndex = 0;
	while(loader.LoadFile(filename, nullptr, "", fileIndex)) {
		if(hashInfo.Crc32Hash == loader._romData.Crc32 || hashInfo.Sha1Hash.compare(loader._romData.Sha1) == 0) {
			return fileIndex;
		}
		fileIndex++;
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