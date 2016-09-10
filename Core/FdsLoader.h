#pragma once
#include "stdafx.h"
#include <algorithm>
#include "../Utilities/FolderUtilities.h"
#include "../Utilities/IpsPatcher.h"
#include "RomData.h"
#include "MessageManager.h"
#include "MapperFactory.h"
#include "GameDatabase.h"

class FdsLoader
{
private:
	const size_t FdsDiskSideCapacity = 65500;

private:
	void AddGaps(vector<uint8_t>& diskSide, uint8_t* readBuffer)
	{
		//Start image with 28300 bits of gap
		diskSide.insert(diskSide.end(), 28300 / 8, 0);

		for(size_t j = 0; j < FdsDiskSideCapacity;) {
			uint8_t blockType = readBuffer[j];
			uint32_t blockLength = 1;
			switch(blockType) {
				case 1: blockLength = 56; break; //Disk header
				case 2: blockLength = 2; break; //File count
				case 3: blockLength = 16; break; //File header
				case 4: blockLength = 1 + readBuffer[j - 3] + readBuffer[j - 2] * 0x100; break;
				default: return; //End parsing when we encounter an invalid block type (This is what Nestopia apppears to do)
			}

			if(blockType == 0) {
				diskSide.push_back(blockType);
			} else {
				diskSide.push_back(0x80);
				diskSide.insert(diskSide.end(), &readBuffer[j], &readBuffer[j] + blockLength);

				//Fake CRC value
				diskSide.push_back(0x4D);
				diskSide.push_back(0x62);

				//Insert 976 bits of gap after a block
				diskSide.insert(diskSide.end(), 976 / 8, 0);
			}

			j += blockLength;
		}
	}

	vector<uint8_t> RebuildFdsFile(vector<vector<uint8_t>> diskData, bool needHeader)
	{
		vector<uint8_t> output;
		output.reserve(diskData.size() * FdsDiskSideCapacity + 16);

		if(needHeader) {
			uint8_t header[16] = { 'F', 'D', 'S', '\x1a', (uint8_t)diskData.size(), '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0' };
			output.insert(output.end(), header, header + sizeof(header));
		}

		for(vector<uint8_t> &diskSide : diskData) {
			bool inGap = true;
			size_t i = 0, len = diskSide.size();
			size_t gapNeeded = FdsDiskSideCapacity;
			uint32_t fileSize = 0;
			while(i < len) {
				if(inGap) {
					if(diskSide[i] == 0x80) {
						inGap = false;
					}
					i++;
				} else {
					uint32_t blockLength = 1;
					switch(diskSide[i]) {
						case 1: blockLength = 56; break; //Disk header
						case 2: blockLength = 2; break; //File count
						case 3: blockLength = 16; fileSize = diskSide[i + 13] + diskSide[i + 14] * 0x100;  break; //File header
						case 4: blockLength = 1 + fileSize; break;
					}
					output.insert(output.end(), &diskSide[i], &diskSide[i] + blockLength);
					gapNeeded -= blockLength;
					i += blockLength;
					i += 2; //Skip CRC after block

					inGap = true;
				}
			}
			output.insert(output.end(), gapNeeded, 0);
		}

		return output;
	}

	void LoadDiskData(vector<uint8_t>& romFile, RomData &romData)
	{
		uint8_t numberOfSides = 0;
		size_t fileOffset = 0;
		bool hasHeader = memcmp(romFile.data(), "FDS\x1a", 4) == 0;
		if(hasHeader) {
			numberOfSides = romFile[4];
			fileOffset = 16;
		} else {
			numberOfSides = (uint8_t)(romFile.size() / 65500);
		}

		for(uint32_t i = 0; i < numberOfSides; i++) {
			romData.FdsDiskData.push_back(vector<uint8_t>());
			vector<uint8_t> &fdsDiskImage = romData.FdsDiskData.back();

			AddGaps(fdsDiskImage, &romFile[fileOffset]);			
			fileOffset += FdsDiskSideCapacity;

			//Ensure the image is 65500 bytes
			if(fdsDiskImage.size() < FdsDiskSideCapacity) {
				fdsDiskImage.resize(FdsDiskSideCapacity);
			}
		}
	}

	vector<uint8_t> LoadBios()
	{
		//For FDS, the PRG ROM is the FDS BIOS (8k)
		vector<uint8_t> biosData;

		ifstream biosFile("FdsBios.bin", ios::in | ios::binary);
		if(biosFile) {
			return vector<uint8_t>(std::istreambuf_iterator<char>(biosFile), {});
		} else {
			MessageManager::SendNotification(ConsoleNotificationType::FdsBiosNotFound);
		}
		return {};
	}

public:
	void SaveIpsFile(string filename, vector<uint8_t> &originalDiskData, vector<vector<uint8_t>> &currentDiskData)
	{
		bool needHeader = (memcmp(originalDiskData.data(), "FDS\x1a", 4) == 0);
		vector<uint8_t> newData = RebuildFdsFile(currentDiskData, needHeader);
		vector<uint8_t> ipsData = IpsPatcher::CreatePatch(originalDiskData, newData);
			
		string fdsSaveFilepath = FolderUtilities::CombinePath(FolderUtilities::GetSaveFolder(), FolderUtilities::GetFilename(filename, false) + ".ips");
		ofstream outputIps(fdsSaveFilepath, ios::binary);
		if(outputIps) {
			outputIps.write((char*)ipsData.data(), ipsData.size());
			outputIps.close();
		}
	}

	RomData LoadRom(vector<uint8_t>& romFile, string filename)
	{
		//Apply save data (saved as an IPS file), if found
		string fdsSaveFilepath = FolderUtilities::CombinePath(FolderUtilities::GetSaveFolder(), FolderUtilities::GetFilename(filename, false) + ".ips");
		romFile = IpsPatcher::PatchBuffer(fdsSaveFilepath, romFile);

		RomData romData;

		romData.MapperID = MapperFactory::FdsMapperID;
		romData.MirroringType = MirroringType::Vertical;
		romData.PrgRom = LoadBios();
		romData.System = GameSystem::FDS;

		if(romData.PrgRom.size() != 0x2000) {
			romData.Error = true;
		} else {
			LoadDiskData(romFile, romData);
		}

		//Setup default controllers
		GameDatabase::InitializeInputDevices("", GameSystem::FDS);

		return romData;
	}
};