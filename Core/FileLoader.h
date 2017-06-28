#pragma once
#include "stdafx.h"
#include "../Utilities/ArchiveReader.h"

class FileLoader
{
private:
	static void ReadFile(istream &file, vector<uint8_t> &fileData);
	static bool LoadFromArchive(istream &zipFile, ArchiveReader& reader, int32_t archiveFileIndex, vector<uint8_t> &fileData);

public:
	static bool LoadFile(string filename, istream *filestream, int32_t archiveFileIndex, vector<uint8_t> &fileData);
	static void ApplyPatch(string patchPath, vector<uint8_t> &fileData);
	static vector<string> GetArchiveRomList(string filename);
};