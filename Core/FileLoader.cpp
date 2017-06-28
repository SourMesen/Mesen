#include "stdafx.h"
#include "FileLoader.h"
#include "../Utilities/ZipReader.h"
#include "../Utilities/SZReader.h"
#include "../Utilities/BpsPatcher.h"
#include "../Utilities/IpsPatcher.h"
#include "../Utilities/UpsPatcher.h"
#include "../Utilities/FolderUtilities.h"
#include "MessageManager.h"

void FileLoader::ReadFile(istream &file, vector<uint8_t> &fileData)
{
	file.seekg(0, ios::end);
	uint32_t fileSize = (uint32_t)file.tellg();
	file.seekg(0, ios::beg);

	fileData = vector<uint8_t>(fileSize, 0);
	file.read((char*)fileData.data(), fileSize);
}

bool FileLoader::LoadFromArchive(istream &zipFile, ArchiveReader &reader, int32_t archiveFileIndex, vector<uint8_t> &fileData)
{
	ReadFile(zipFile, fileData);
	reader.LoadArchive(fileData.data(), fileData.size());

	vector<string> fileList = reader.GetFileList({ ".nes", ".fds", ".nsf", ".nsfe", ".unf" });
	if(fileList.empty() || archiveFileIndex > (int32_t)fileList.size()) {
		return false;
	}

	if(archiveFileIndex == -1) {
		archiveFileIndex = 0;
	}
	reader.ExtractFile(fileList[archiveFileIndex], fileData);
	return true;
}

bool FileLoader::LoadFile(string filename, istream *filestream, int32_t archiveFileIndex, vector<uint8_t> &fileData)
{
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
			return LoadFromArchive(*input, reader, archiveFileIndex, fileData);
		} else if(memcmp(header, "7z", 2) == 0) {
			SZReader reader;
			return LoadFromArchive(*input, reader, archiveFileIndex, fileData);
		} else {
			if(archiveFileIndex > 0) {
				return false;
			}

			ReadFile(*input, fileData);
			return true;
		}
	}
	return false;
}

void FileLoader::ApplyPatch(string patchPath, vector<uint8_t> &fileData)
{
	//Apply patch file
	MessageManager::DisplayMessage("Patch", "ApplyingPatch", FolderUtilities::GetFilename(patchPath, true));
	ifstream patchFile(patchPath, ios::binary | ios::in);
	if(patchFile.good()) {
		char buffer[5] = {};
		patchFile.read(buffer, 5);
		patchFile.close();
		if(memcmp(buffer, "PATCH", 5) == 0) {
			fileData = IpsPatcher::PatchBuffer(patchPath, fileData);
		} else if(memcmp(buffer, "UPS1", 4) == 0) {
			fileData = UpsPatcher::PatchBuffer(patchPath, fileData);
		} else if(memcmp(buffer, "BPS1", 4) == 0) {
			fileData = BpsPatcher::PatchBuffer(patchPath, fileData);
		}
	}
}

vector<string> FileLoader::GetArchiveRomList(string filename)
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
