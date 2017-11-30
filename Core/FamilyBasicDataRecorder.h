#pragma once
#include "stdafx.h"
#include "../Utilities/Base64.h"
#include "BaseControlDevice.h"
#include "CPU.h"

class FamilyBasicDataRecorder : public BaseControlDevice
{
private:
	static const uint32_t SamplingRate = 88;
	vector<uint8_t> _data;
	vector<uint8_t> _fileData;
	bool _enabled = false;
	bool _isPlaying = false;
	int32_t _cycle = -1;

	bool _isRecording = false;
	string _recordFilePath;

protected:
	void StreamState(bool saving) override
	{
		BaseControlDevice::StreamState(saving);

		VectorInfo<uint8_t> data{ &_data };
		Stream(_enabled, _isPlaying, _cycle, data);

		if(!saving && _isRecording) {
			StopRecording();
		}
	}

	bool IsRawString() override
	{
		return true;
	}

	void InternalSetStateFromInput() override
	{
		if(_fileData.size() > 0) {
			SetTextState(Base64::Encode(_fileData));
			_fileData.clear();
		}
	}	

public:
	FamilyBasicDataRecorder() : BaseControlDevice(BaseControlDevice::ExpDevicePort2)
	{
	}

	~FamilyBasicDataRecorder()
	{
		if(_isRecording) {
			StopRecording();
		}
	}

	void OnAfterSetState() override
	{
		if(_state.State.size() > 0) {
			_data = Base64::Decode(GetTextState());
			_cycle = CPU::GetCycleCount();
			_isPlaying = true;
			_isRecording = false;
		}
	}

	void LoadFromFile(VirtualFile file)
	{
		if(file.IsValid()) {
			vector<uint8_t> fileData;
			file.ReadFile(fileData);
			_fileData = fileData;
		}
	}

	bool IsRecording()
	{
		return _isRecording;
	}

	void StartRecording(string filePath)
	{
		_isPlaying = false;
		_recordFilePath = filePath;
		_data.clear();
		_cycle = CPU::GetCycleCount();
		_isRecording = true;
	}

	void StopRecording()
	{
		_isRecording = false;

		vector<uint8_t> fileData;
		
		int bitPos = 0;
		uint8_t currentByte = 0;
		for(uint8_t bitValue : _data) {
			currentByte |= (bitValue & 0x01) << bitPos;
			bitPos = (bitPos + 1) % 8;
			if(bitPos == 0) {
				fileData.push_back(currentByte);
				currentByte = 0;
			}
		}

		ofstream out(_recordFilePath, ios::binary);
		if(out) {
			out.write((char*)fileData.data(), fileData.size());
		}
	}

	uint8_t ReadRAM(uint16_t addr) override
	{
		if(addr == 0x4016 && _isPlaying) {
			int32_t readPos = CPU::GetElapsedCycles(_cycle) / FamilyBasicDataRecorder::SamplingRate;

			if((int32_t)_data.size() > readPos / 8) {
				uint8_t value = ((_data[readPos / 8] >> (readPos % 8)) & 0x01) << 1;
				return _enabled ? value : 0;
			} else {
				_isPlaying = false;
			}
		}

		return 0;
	}

	void WriteRAM(uint16_t addr, uint8_t value) override
	{
		_enabled = (value & 0x04) != 0;

		if(_isRecording) {
			while(CPU::GetElapsedCycles(_cycle) > FamilyBasicDataRecorder::SamplingRate) {
				_data.push_back(value & 0x01);
				_cycle += 88;
			}
		}
	}
};