#pragma once

#include "stdafx.h"
#include <algorithm>
#include "../Utilities/FolderUtilities.h"
#include "../Utilities/ZipReader.h"
#include "../Utilities/CRC32.h"
#include "../Utilities/IpsPatcher.h"

enum class MirroringType
{
	Horizontal,
	Vertical,
	ScreenAOnly,
	ScreenBOnly,
	FourScreens,
	Custom
};

enum class RomHeaderVersion
{
	iNes = 0,
	Nes2_0 = 1,
	OldiNes = 2
};

struct NESHeader
{
/*
	Thing 	Archaic			 	iNES 									NES 2.0
	Byte 6	Mapper low nibble, Mirroring, Battery/Trainer flags
	Byte 7 	Unused 				Mapper high nibble, Vs. 		Mapper high nibble, NES 2.0 signature, PlayChoice, Vs.
	Byte 8 	Unused 				Total PRG RAM size (linear) 	Mapper highest nibble, mapper variant
	Byte 9 	Unused 				TV system 							Upper bits of ROM size
	Byte 10 	Unused 				Unused 								PRG RAM size (logarithmic; battery and non-battery)
	Byte 11 	Unused 				Unused 								VRAM size (logarithmic; battery and non-battery)
	Byte 12 	Unused 				Unused 								TV system
	Byte 13 	Unused 				Unused 								Vs. PPU variant
*/
	char NES[4];
	uint8_t PrgCount;
	uint8_t ChrCount;
	uint8_t Byte6;
	uint8_t Byte7;
	uint8_t Byte8;
	uint8_t Byte9;
	uint8_t Byte10;
	uint8_t Byte11;
	uint8_t Byte12;
	uint8_t Byte13;
	uint8_t Reserved[2];

	uint16_t GetMapperID()
	{
		switch(GetRomHeaderVersion()) {
			case RomHeaderVersion::Nes2_0:
				return (Byte8 & 0x0F << 4) | (Byte7 & 0xF0) | (Byte6 >> 4);
			case RomHeaderVersion::iNes:
				return (Byte7 & 0xF0) | (Byte6 >> 4);
			case RomHeaderVersion::OldiNes:
				return (Byte6 >> 4);
		}
	}

	bool HasBattery()
	{
		return (Byte6 & 0x02) == 0x02;
	}

	bool HasTrainer()
	{
		return (Byte6 & 0x04) == 0x04;
	}

	bool IsPalRom()
	{
		switch(GetRomHeaderVersion()) {
			case RomHeaderVersion::Nes2_0: return (Byte12 & 0x01) == 0x01;
			case RomHeaderVersion::iNes: return (Byte9 & 0x01) == 0x01;
			default: return false;
		}
	}

	RomHeaderVersion GetRomHeaderVersion()
	{
		if((Byte7 & 0x0C) == 0x08) {
			return RomHeaderVersion::Nes2_0;
		} else if((Byte7 & 0x0C) == 0x00) {
			return RomHeaderVersion::iNes;
		} else {
			return RomHeaderVersion::OldiNes;
		}
	}

	uint32_t GetPrgSize()
	{
		if(GetRomHeaderVersion() == RomHeaderVersion::Nes2_0) {
			return (((Byte9 & 0x0F) << 4) | PrgCount) * 0x4000;
		} else {
			return PrgCount * 0x4000;
		}
	}

	uint32_t GetChrSize()
	{
		if(GetRomHeaderVersion() == RomHeaderVersion::Nes2_0) {
			return (((Byte9 & 0xF0) << 4) | ChrCount) * 0x2000;
		} else {
			return ChrCount * 0x2000;
		}
	}

	uint32_t GetWorkRamSize()
	{
		uint8_t value = Byte10 & 0x0F;
		return value == 0 ? 0 : 128 * (uint32_t)std::pow(2, value);
	}

	uint32_t GetSaveRamSize()
	{
		uint8_t value = (Byte10 & 0xF0) >> 4;
		return value == 0 ? 0 : 128 * (uint32_t)std::pow(2, value);
	}

	uint32_t GetChrRamSize()
	{
		uint8_t value = Byte11 & 0x0F;
		return value == 0 ? 0 : 128 * (uint32_t)std::pow(2, value);
	}

	uint32_t GetSavedChrRamSize()
	{
		uint8_t value = (Byte10 & 0xF0) >> 4;
		return value == 0 ? 0 : 128 * (uint32_t)std::pow(2, value);
	}

	uint8_t GetSubMapper()
	{
		if(GetRomHeaderVersion() == RomHeaderVersion::Nes2_0) {
			return (Byte8 & 0xF0) >> 4;
		} else {
			return 0;
		}
	}

	MirroringType GetMirroringType()
	{
		if(Byte6 & 0x08) {
			return MirroringType::FourScreens;
		} else {
			return Byte6 & 0x01 ? MirroringType::Vertical : MirroringType::Horizontal;
		}
	}

	void SanitizeHeader(size_t romLength)
	{
		size_t calculatedLength = sizeof(NESHeader) + 0x4000 * PrgCount;
		while(calculatedLength > romLength) {
			PrgCount--;
			calculatedLength = sizeof(NESHeader) + 0x4000 * PrgCount;
		}

		calculatedLength = sizeof(NESHeader) + 0x4000 * PrgCount + 0x2000 * ChrCount;
		while(calculatedLength > romLength) {
			ChrCount--;
			calculatedLength = sizeof(NESHeader) + 0x4000 * PrgCount + 0x2000 * ChrCount;
		}
	}
};

class ROMLoader
{
	private:
		NESHeader _header;
		string _filename;
		uint8_t* _prgRAM = nullptr;
		uint8_t* _chrRAM = nullptr;
		uint32_t _crc32;
		string _ipsFilename;

		bool LoadFromZip(stringstream &zipFile)
		{
			bool result = false;

			uint32_t fileSize;
			uint8_t* buffer = ReadFile(zipFile, fileSize);

			ZipReader reader;
			reader.LoadZipArchive(buffer, fileSize);
			
			vector<string> fileList = reader.GetFileList();
			for(string filename : fileList) {
				std::transform(filename.begin(), filename.end(), filename.begin(), ::tolower);
				if(filename.length() > 4) {
					if(filename.substr(filename.length() - 4, 4).compare(".nes") == 0) {
						uint8_t* fileBuffer = nullptr;
						size_t fileSize = 0;
						reader.ExtractFile(filename, &fileBuffer, fileSize);
						if(fileBuffer) {
							result = LoadFromMemory(fileBuffer, fileSize);
							delete[] fileBuffer;
							break;
						}
					}
				}

			}

			delete[] buffer;
			return result;
		}

		bool LoadFromFile(stringstream &romFile)
		{
			uint32_t fileSize;
			uint8_t* buffer = ReadFile(romFile, fileSize);
			bool result = LoadFromMemory(buffer, fileSize);
			delete[] buffer;

			return result;
		}

		uint32_t GetFileSize(stringstream &file)
		{
			file.seekg(0, ios::end);
			uint32_t fileSize = (uint32_t)file.tellg();
			file.seekg(0, ios::beg);

			return fileSize;
		}

		uint8_t* ReadFile(stringstream &file, uint32_t &fileSize)
		{
			fileSize = GetFileSize(file);
			
			uint8_t* buffer = new uint8_t[fileSize];
			file.read((char*)buffer, fileSize);
			return buffer;
		}

		bool LoadFromMemory(uint8_t* buffer, size_t length)
		{
			if(!_ipsFilename.empty()) {
				//Apply IPS patch
				uint8_t* patchedFile = nullptr;
				size_t patchedSize = 0;
				if(IpsPatcher::PatchBuffer(_ipsFilename, buffer, length, &patchedFile, patchedSize)) {
					buffer = patchedFile;
					length = patchedSize;
				}
			}

			_crc32 = CRC32::GetCRC(buffer, length);
			if(memcmp(buffer, "NES", 3) == 0 && length >= sizeof(NESHeader)) {
				memcpy((char*)&_header, buffer, sizeof(NESHeader));
				buffer += sizeof(NESHeader);

				_header.SanitizeHeader(length);

				_prgRAM = new uint8_t[_header.GetPrgSize()];
				_chrRAM = new uint8_t[_header.GetChrSize()];

				memcpy(_prgRAM, buffer, _header.GetPrgSize());
				buffer += _header.GetPrgSize();

				memcpy(_chrRAM, buffer, _header.GetChrSize());

				return true;
			}
			return false;			
		}

	public:
		ROMLoader()
		{
		}

		~ROMLoader()
		{
			if(_prgRAM) {
				delete[] _prgRAM;
				_prgRAM = nullptr;
			}
			if(_chrRAM) {
				delete[] _chrRAM;
				_chrRAM = nullptr;
			}
		}
		
		bool LoadFile(string filename, stringstream *filestream = nullptr, string ipsFilename = "") 
		{
			_ipsFilename = ipsFilename;

			stringstream ss;
			if(!filestream) {
				ifstream file(filename, ios::in | ios::binary);
				if(file) {
					ss << file.rdbuf();
					file.close();
					filestream = &ss;
				}
			}

			filestream->seekg(0, ios::beg);

			bool result = false;
			char header[3];
			filestream->read(header, 3);
			if(memcmp(header, "NES", 3) == 0) {
				_filename = FolderUtilities::GetFilename(filename, false);
				filestream->seekg(0, ios::beg);
				result = LoadFromFile(*filestream);
			} else if(memcmp(header, "PK", 2) == 0) {
				_filename = FolderUtilities::GetFilename(filename, false);
				filestream->seekg(0, ios::beg);
				result = LoadFromZip(*filestream);
			}
			return result;
		}

		void GetPrgRom(uint8_t** buffer)
		{
			*buffer = new uint8_t[GetPrgSize()];
			memcpy(*buffer, _prgRAM, GetPrgSize());
		}

		void GetChrRom(uint8_t** buffer)
		{
			*buffer = new uint8_t[GetChrSize()];
			memcpy(*buffer, _chrRAM, GetChrSize());
		}

		uint32_t GetPrgSize()
		{
			return _header.GetPrgSize();
		}

		uint32_t GetChrSize()
		{
			return _header.GetChrSize();
		}

		MirroringType GetMirroringType()
		{
			return _header.GetMirroringType();
		}

		uint16_t GetMapperID()
		{
			return _header.GetMapperID();
		}

		uint8_t GetSubMapper()
		{
			return _header.GetSubMapper();
		}

		bool HasBattery()
		{
			return _header.HasBattery();
		}

		bool IsPalRom()
		{
			return _header.IsPalRom() || _filename.find("(e)") != string::npos || _filename.find("(E)") != string::npos;
		}

		string GetFilename()
		{
			return _filename;
		}

		static uint32_t GetCRC32(string filename)
		{
			ROMLoader loader;
			uint32_t crc = 0;
			if(loader.LoadFile(filename)) {
				crc = loader._crc32;
			}
			return crc;
		}

		static string FindMatchingRomInFolder(string folder, string romFilename, uint32_t crc32Hash)
		{
			std::transform(romFilename.begin(), romFilename.end(), romFilename.begin(), ::tolower);
			vector<string> romFiles = FolderUtilities::GetFilesInFolder(folder, "*.nes", true);
			for(string zipFile : FolderUtilities::GetFilesInFolder(folder, "*.zip", true)) {
				romFiles.push_back(zipFile);
			}
			for(string romFile : romFiles) {
				//Quick search by filename
				std::transform(romFile.begin(), romFile.end(), romFile.begin(), ::tolower);
				if(FolderUtilities::GetFilename(romFile, true).compare(romFilename) == 0) {
					if(ROMLoader::GetCRC32(romFile) == crc32Hash) {
						return romFile;
					}
				}
			}

			for(string romFile : romFiles) {
				//Slower search by CRC value
				if(ROMLoader::GetCRC32(romFile) == crc32Hash) {
					//Matching ROM found
					return romFile;
				}
			}

			return "";
		}
};

