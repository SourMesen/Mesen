#include "stdafx.h"
#include "IpsPatcher.h"

class IpsRecord
{
public:
	uint32_t Address = 0;
	uint16_t Length = 0;
	uint8_t* Replacement = nullptr;

	//For RLE records (when length == 0)
	uint16_t RepeatCount = 0;
	uint8_t Value = 0;

	bool ReadRecord(ifstream &ipsFile)
	{
		uint8_t buffer[3];

		ipsFile.read((char*)buffer, 3);
		if(memcmp(buffer, "EOF", 3) == 0) {
			//EOF reached
			return false;
		} else {
			Address = buffer[2] | (buffer[1] << 8) | (buffer[0] << 16);

			ipsFile.read((char*)buffer, 2);
			Length = buffer[1] | (buffer[0] << 8);

			if(Length == 0) {
				//RLE record
				ipsFile.read((char*)buffer, 3);
				RepeatCount = buffer[1] | (buffer[0] << 8);
				Value = buffer[2];
			} else {
				Replacement = new uint8_t[Length];
				ipsFile.read((char*)Replacement, Length);
			}
			return true;
		}
	}

	~IpsRecord()
	{
		if(Replacement != nullptr) {
			delete[] Replacement;
		}
	}
};

bool IpsPatcher::PatchBuffer(string ipsFilepath, uint8_t* inputBuffer, size_t inputBufferSize, uint8_t** outputBuffer, size_t &outputBufferSize)
{
	ifstream ipsFile(ipsFilepath, std::ios::in | std::ios::binary);

	if(ipsFile) {
		char header[5];
		ipsFile.read((char*)&header, 5);
		if(memcmp((char*)&header, "PATCH", 5) != 0) {
			//Invalid ips file
			return false;
		}

		vector<IpsRecord*> records;
		int32_t truncateOffset = -1;
		size_t maxOutputSize = inputBufferSize;
		while(!ipsFile.eof()) {
			IpsRecord *record = new IpsRecord();
			if(record->ReadRecord(ipsFile)) {
				if(record->Address + record->Length + record->RepeatCount > maxOutputSize) {
					maxOutputSize = record->Address + record->Length + record->RepeatCount;
				}
				records.push_back(record);
			} else {
				//EOF, try to read truncate offset record if it exists
				uint8_t buffer[3];
				ipsFile.read((char*)buffer, 3);
				if(!ipsFile.eof()) {
					truncateOffset = buffer[2] | (buffer[1] << 8) | (buffer[0] << 16);
				}
				break;
			}
		}

		outputBufferSize = maxOutputSize;
		uint8_t *output = new uint8_t[outputBufferSize];
		memset(output, 0, outputBufferSize);
		memcpy(output, inputBuffer, inputBufferSize);

		for(IpsRecord *record : records) {
			if(record->Length == 0) {
				memset(output+record->Address, record->Value, record->RepeatCount);
			} else {
				memcpy(output+record->Address, record->Replacement, record->Length);
			}

			delete record;
		}

		if(truncateOffset != -1 && (int32_t)outputBufferSize > truncateOffset) {
			uint8_t* truncatedOutput = new uint8_t[truncateOffset];
			memcpy(truncatedOutput, output, truncateOffset);
			delete[] output;
			*outputBuffer = truncatedOutput;
		} else {
			*outputBuffer = output;
		}

		ipsFile.close();

		return true;
	}

	return false;
}