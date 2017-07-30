#pragma once
#include "stdafx.h"
#include "../Utilities/VirtualFile.h"
#include "RomData.h"
class ArchiveReader;

class RomLoader
{
	private:
		RomData _romData;
		string _filename;

		static string FindMatchingRomInFile(string filePath, HashInfo hashInfo);

	public:
		bool LoadFile(VirtualFile romFile);
		bool LoadFile(string filename, vector<uint8_t> &fileData);

		RomData GetRomData();
		static string FindMatchingRomInFolder(string folder, string romFilename, HashInfo hashInfo, bool useFastSearch);
};
