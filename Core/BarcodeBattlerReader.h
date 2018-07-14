#pragma once
#include "stdafx.h"
#include "BaseControlDevice.h"
#include "IBarcodeReader.h"
#include "MemoryManager.h"

class BarcodeBattlerReader : public BaseControlDevice, public IBarcodeReader
{
private:
	static constexpr int StreamSize = 200;
	uint64_t _newBarcode = 0;
	uint32_t _newBarcodeDigitCount = 0;

	uint8_t _barcodeStream[BarcodeBattlerReader::StreamSize];
	int32_t _insertCycle = 0;

protected:
	void StreamState(bool saving) override
	{
		BaseControlDevice::StreamState(saving);

		ArrayInfo<uint8_t> bitStream{ _barcodeStream, BarcodeBattlerReader::StreamSize };
		Stream(_newBarcode, _newBarcodeDigitCount, _insertCycle, bitStream);
	}

	bool IsRawString() override
	{
		return true;
	}

	void InitBarcodeStream()
	{
		vector<uint8_t> state = GetRawState().State;
		string barcodeText(state.begin(), state.end());

		//Signature at the end, needed for code to be recognized
		barcodeText += "EPOCH\xD\xA";
		//Pad to 20 characters with spaces
		barcodeText.insert(0, 20 - barcodeText.size(), ' ');

		int pos = 0;
		vector<uint8_t> bits;
		for(int i = 0; i < 20; i++) {
			_barcodeStream[pos++] = 1;
			for(int j = 0; j < 8; j++) {
				_barcodeStream[pos++] = ~((barcodeText[i] >> j) & 0x01);
			}
			_barcodeStream[pos++] = 0;
		}
	}

public:
	BarcodeBattlerReader(shared_ptr<Console> console) : BaseControlDevice(console, BaseControlDevice::ExpDevicePort)
	{
	}

	void InternalSetStateFromInput() override
	{
		ClearState();

		if(_newBarcodeDigitCount > 0) {
			string barcodeText = std::to_string(_newBarcode);
			//Pad 8 or 13 character barcode with 0s at start
			barcodeText.insert(0, _newBarcodeDigitCount - barcodeText.size(), '0');
			SetTextState(barcodeText);

			_newBarcode = 0;
			_newBarcodeDigitCount = 0;
		}
	}

	void OnAfterSetState() override
	{
		if(GetRawState().State.size() > 0) {
			InitBarcodeStream();
			if(_console) {
				_insertCycle = _console->GetCpu()->GetCycleCount();
			}
		}
	}

	void InputBarcode(uint64_t barcode, uint32_t digitCount) override
	{
		_newBarcode = barcode;
		_newBarcodeDigitCount = digitCount;
	}

	uint8_t ReadRAM(uint16_t addr) override
	{
		if(addr == 0x4017) {
			int32_t elapsedCycles = _console->GetCpu()->GetElapsedCycles(_insertCycle);
			constexpr uint32_t cyclesPerBit = CPU::ClockRateNtsc / 1200;

			uint32_t streamPosition = elapsedCycles / cyclesPerBit;
			if(streamPosition < BarcodeBattlerReader::StreamSize) {
				return _barcodeStream[streamPosition] << 2;
			}
		}
		return 0;
	}

	void WriteRAM(uint16_t addr, uint8_t value) override
	{
	}
};