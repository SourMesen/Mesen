#pragma once
#include "stdafx.h"
#include "VirtualFile.h"
#include "RomData.h"
#include "BaseLoader.h"
class ArchiveReader;

class RomLoader : public BaseLoader
{
private:
	static constexpr int MaxFilesToCheck = 100;

	RomData _romData;
	string _filename;

	static string FindMatchingRomInFile(string filePath, HashInfo hashInfo, int &iterationCount);
	
public:
	using BaseLoader::BaseLoader;
	
	bool LoadFile(VirtualFile romFile);
	bool LoadFile(string filename, vector<uint8_t> &fileData);

	RomData GetRomData();
	static string FindMatchingRom(vector<string> romFiles, string romFilename, HashInfo hashInfo, bool useFastSearch);
};
