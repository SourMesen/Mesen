#pragma once

#include "stdafx.h"
#include "../Utilities/FolderUtilities.h"
#include "../Utilities/ZIPReader.h"
#include "../Utilities/CRC32.h"

enum class MirroringType
{
	Horizontal,
	Vertical,
	ScreenAOnly,
	ScreenBOnly,
	FourScreens,
};

struct NESHeader
{
	char NES[4];
	uint8_t ROMCount;
	uint8_t VROMCount;
	uint8_t Flags1;
	uint8_t Flags2;
	uint8_t RAMCount;
	uint8_t CartType;
	uint8_t Reserved[6];

	uint8_t GetMapperID()
	{
		return (Flags2 & 0xF0) | (Flags1 >> 4);
	}

	bool HasBattery()
	{
		return (Flags1 & 0x02) == 0x02;
	}

	bool HasTrainer()
	{
		return (Flags1 & 0x04) == 0x04;
	}

	MirroringType GetMirroringType()
	{
		if(Flags1 & 0x08) {
			return MirroringType::FourScreens;
		} else {
			return Flags1 & 0x01 ? MirroringType::Vertical : MirroringType::Horizontal;
		}
	}
};

class ROMLoader
{
	private:
		NESHeader _header;
		wstring _filename;
		uint8_t* _prgRAM = nullptr;
		uint8_t* _chrRAM = nullptr;
		uint32_t _crc32;

		bool LoadFromZIP(ifstream &zipFile)
		{
			bool result = false;

			uint32_t fileSize;
			uint8_t* buffer = ReadFile(zipFile, fileSize);

			ZIPReader reader;
			reader.LoadZIPArchive(buffer, fileSize);
			
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

		bool LoadFromFile(ifstream &romFile)
		{
			uint32_t fileSize;
			uint8_t* buffer = ReadFile(romFile, fileSize);
			bool result = LoadFromMemory(buffer, fileSize);
			delete[] buffer;

			return result;
		}

		uint32_t GetFileSize(ifstream &file)
		{
			file.seekg(0, ios::end);
			uint32_t fileSize = (uint32_t)file.tellg();
			file.seekg(0, ios::beg);

			return fileSize;
		}

		uint8_t* ReadFile(ifstream &file, uint32_t &fileSize)
		{
			fileSize = GetFileSize(file);
			
			uint8_t* buffer = new uint8_t[fileSize];
			file.read((char*)buffer, fileSize);
			return buffer;
		}

		bool LoadFromMemory(uint8_t* buffer, uint32_t length)
		{
			_crc32 = CRC32::GetCRC(buffer, length);
			if(memcmp(buffer, "NES", 3) == 0) {
				memcpy((char*)&_header, buffer, sizeof(NESHeader));

				_prgRAM = new uint8_t[0x4000 * _header.ROMCount];
				_chrRAM = new uint8_t[0x2000 * _header.VROMCount];

				buffer += sizeof(NESHeader);
				memcpy(_prgRAM, buffer, 0x4000 * _header.ROMCount);

				buffer += 0x4000 * _header.ROMCount;
				memcpy(_chrRAM, buffer, 0x2000 * _header.VROMCount);

				return true;
			}
			return false;			
		}

	public:
		ROMLoader()
		{
		}

		bool LoadFile(wstring filename) 
		{
			bool result = false;
			ifstream file(filename, ios::in | ios::binary);
			if(file) {
				char header[3];
				file.read(header, 3);
				if(memcmp(header, "NES", 3) == 0) {
					_filename = FolderUtilities::GetFilename(filename, false);
					file.seekg(0, ios::beg);
					result = LoadFromFile(file);
					file.close();
				} else if(memcmp(header, "PK", 2) == 0) {
					_filename = FolderUtilities::GetFilename(filename, false);
					file.seekg(0, ios::beg);
					result = LoadFromZIP(file);
				} else {
					//Unsupported file format
					file.close();
				}
				file.close();
			}
			return result;
		}

		void FreeMemory()
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

		uint8_t* GetPRGRam()
		{
			return _prgRAM;
		}

		uint8_t* GetCHRRam()
		{
			return _chrRAM;
		}

		uint32_t GetPRGSize()
		{
			return _header.ROMCount * 0x4000;
		}

		uint32_t GetCHRSize()
		{
			return _header.VROMCount * 0x2000;
		}

		MirroringType GetMirroringType()
		{
			return _header.GetMirroringType();
		}

		uint8_t GetMapperID()
		{
			return _header.GetMapperID();
		}

		bool HasBattery()
		{
			return _header.HasBattery();
		}

		wstring GetFilename()
		{
			return _filename;
		}

		static uint32_t GetCRC32(wstring filename)
		{
			ROMLoader loader;
			uint32_t crc = 0;
			if(loader.LoadFile(filename)) {
				crc = loader._crc32;
				loader.FreeMemory();
			}
			return crc;
		}
};

