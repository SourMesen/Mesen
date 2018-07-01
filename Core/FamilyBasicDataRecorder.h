#pragma once
#include "stdafx.h"
#include "../Utilities/Base64.h"
#include "BaseControlDevice.h"
#include "Console.h"
#include "CPU.h"

class FamilyBasicDataRecorder : public BaseControlDevice
{
private:
	static constexpr int32_t SamplingRate = 88;
	shared_ptr<Console> _console;
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
	FamilyBasicDataRecorder(shared_ptr<Console> console) : BaseControlDevice(BaseControlDevice::ExpDevicePort2)
	{
		_console = console;
	}

	~FamilyBasicDataRecorder()
	{
		if(_isRecording) {
			StopRecording();
		}
	}

	void OnAfterSetState() override
	{
		if(GetRawState().State.size() > 0) {
			_data = Base64::Decode(GetTextState());
			_cycle = _console->GetCpu()->GetCycleCount();
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
		_cycle = _console->GetCpu()->GetCycleCount();
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
			int32_t readPos = _console->GetCpu()->GetElapsedCycles(_cycle) / FamilyBasicDataRecorder::SamplingRate;

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
			while(_console->GetCpu()->GetElapsedCycles(_cycle) > FamilyBasicDataRecorder::SamplingRate) {
				_data.push_back(value & 0x01);
				_cycle += 88;
			}
		}
	}
};