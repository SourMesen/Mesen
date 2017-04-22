#pragma once
#include "stdafx.h"
#include "RomData.h"
class ArchiveReader;

class RomLoader
{
	private:
		RomData _romData;
		string _filename;
		string _patchFilename;

		bool LoadFromArchive(istream &zipFile, ArchiveReader& reader, int32_t archiveFileIndex = -1);
		bool LoadFromStream(istream &romFile, string romName);
		uint32_t GetFileSize(istream &file);

		uint8_t* ReadFile(istream &file, uint32_t &fileSize);
		bool LoadFromMemory(uint8_t* buffer, size_t length, string romName);
		static int32_t FindMatchingRomInFile(string filename, HashInfo hashInfo);
		void ApplyPatch(string patchPath, vector<uint8_t> &data);

	public:
		bool LoadFile(string filename, istream *filestream = nullptr, string patchFilename = "", int32_t archiveFileIndex = -1);
		RomData GetRomData();
		static string FindMatchingRomInFolder(string folder, string romFilename, HashInfo hashInfo, bool useFastSearch, int32_t &archiveFileIndex);
		static vector<string> GetArchiveRomList(string filename);
};
