#pragma once

#include "stdafx.h"
#include <algorithm>
#include "../Utilities/FolderUtilities.h"
#include "../Utilities/ZipReader.h"
#include "../Utilities/CRC32.h"
#include "../Utilities/IpsPatcher.h"
#include "RomData.h"

class RomLoader
{
	private:
		RomData _romData;
		string _filename;
		string _ipsFilename;

		bool LoadFromZip(istream &zipFile);
		bool LoadFromStream(istream &romFile);
		uint32_t GetFileSize(istream &file);

		uint8_t* ReadFile(istream &file, uint32_t &fileSize);
		bool LoadFromMemory(uint8_t* buffer, size_t length);

	public:
		bool LoadFile(string filename, istream *filestream = nullptr, string ipsFilename = "");
		RomData GetRomData();
		static uint32_t GetCRC32(string filename);
		static string FindMatchingRomInFolder(string folder, string romFilename, uint32_t crc32Hash);
};
