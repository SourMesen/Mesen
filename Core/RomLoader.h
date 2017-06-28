#pragma once
#include "stdafx.h"
#include "RomData.h"
class ArchiveReader;

class RomLoader
{
	private:
		RomData _romData;
		string _filename;

		static int32_t FindMatchingRomInFile(string filename, HashInfo hashInfo);

	public:
		bool LoadFile(string filename, int32_t archiveFileIndex);
		bool LoadFile(string filename, vector<uint8_t> &fileData);

		RomData GetRomData();
		static string FindMatchingRomInFolder(string folder, string romFilename, HashInfo hashInfo, bool useFastSearch, int32_t &archiveFileIndex);
};
