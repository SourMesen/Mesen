#pragma once
#include "stdafx.h"
#include "../BlipBuffer/Blip_Buffer.h"
#include "APU.h"
#include "IMemoryHandler.h"
#include "ApuEnvelope.h"

class DeltaModulationChannel : public BaseApuChannel<127>
{
private:	
	const vector<uint16_t> _dmcPeriodLookupTableNtsc = { { 428, 380, 340, 320, 286, 254, 226, 214, 190, 160, 142, 128, 106,  84,  72,  54 } };
	const vector<uint16_t> _dmcPeriodLookupTablePal = { { 398, 354, 316, 298, 276, 236, 210, 198, 176, 148, 132, 118,  98,  78,  66,  50 } };

	MemoryManager *_memoryManager = nullptr;

	uint16_t _sampleAddr = 0;
	uint16_t _sampleLength = 0;
	uint8_t _outputLevel = 0;
	bool _irqEnabled = false;
	bool _loopFlag = false;

	uint16_t _currentAddr = 0;
	uint16_t _bytesRemaining = 0;
	uint8_t _readBuffer = 0;
	bool _bufferEmpty = true;

	uint8_t _shiftRegister = 0;
	uint8_t _bitsRemaining = 0;
	bool _silenceFlag = true;

	void InitSample()
	{
		_currentAddr = _sampleAddr;
		_bytesRemaining = _sampleLength;
	}

	void FillReadBuffer()
	{
		if(_bufferEmpty && _bytesRemaining > 0) {
			_readBuffer = _memoryManager->Read(_currentAddr);
			_bufferEmpty = false;

			_currentAddr++;
			_bytesRemaining--;

			if(_bytesRemaining == 0) {
				if(_loopFlag) {
					//Looped sample should never set IRQ flag
					InitSample();
				} else if(_irqEnabled) {
					CPU::SetIRQSource(IRQSource::DMC);
				}
			}
		}
	}
	
protected:
	void Clock()
	{
		if(!_silenceFlag) {
			if(_shiftRegister & 0x01) {
				if(_outputLevel <= 125) {
					_outputLevel += 2;
				}
			} else {
				if(_outputLevel >= 2) {
					_outputLevel -= 2;
				}
			}
			_shiftRegister >>= 1;
		}

		_bitsRemaining--;
		if(_bitsRemaining == 0) {
			_bitsRemaining = 8;
			if(_bufferEmpty) {
				_silenceFlag = true;
			} else {
				_silenceFlag = false;
				_shiftRegister = _readBuffer;
				_bufferEmpty = true;
				FillReadBuffer();
			}
		}

		AddOutput(_outputLevel);
	}

public:
	DeltaModulationChannel(AudioChannel channel, Blip_Buffer *buffer, MemoryManager* memoryManager) : BaseApuChannel(channel, buffer)
	{
		_memoryManager = memoryManager;
		_clockDivider = 1;
		SetVolume(0.42545);
	}

	virtual void Reset(bool softReset)
	{
		BaseApuChannel::Reset(softReset);
		
		_sampleAddr = 0;
		_sampleLength = 0;
		_outputLevel = 0;
		_irqEnabled = false;
		_loopFlag = false;

		_currentAddr = 0;
		_bytesRemaining = 0;
		_readBuffer = 0;
		_bufferEmpty = true;

		_shiftRegister = 0;
		_bitsRemaining = 0;
		_silenceFlag = true;
	}

	virtual void StreamState(bool saving)
	{
		BaseApuChannel::StreamState(saving);

		Stream<uint16_t>(_sampleAddr);
		Stream<uint16_t>(_sampleLength);
		Stream<uint8_t>(_outputLevel);
		Stream<bool>(_irqEnabled);
		Stream<bool>(_loopFlag);

		Stream<uint16_t>(_currentAddr);
		Stream<uint16_t>(_bytesRemaining);
		Stream<uint8_t>(_readBuffer);
		Stream<bool>(_bufferEmpty);

		Stream<uint8_t>(_shiftRegister);
		Stream<uint8_t>(_bitsRemaining);
		Stream<bool>(_silenceFlag);
	}

	bool IrqPending(uint32_t cyclesToRun)
	{
		if(_irqEnabled && _bytesRemaining > 0) {
			uint32_t cyclesToEmptyBuffer = (_bitsRemaining + (_bytesRemaining-1)* 8) * _period;
			if(cyclesToRun >= cyclesToEmptyBuffer) {
				return true;
			}
		}
		return false;
	}

	bool GetStatus()
	{
		return _bytesRemaining > 0;
	}

	void GetMemoryRanges(MemoryRanges &ranges)
	{
		ranges.AddHandler(MemoryType::RAM, MemoryOperation::Write, 0x4010, 0x4013);
	}

	void WriteRAM(uint16_t addr, uint8_t value)
	{
		APU::StaticRun();
		switch(addr & 0x03) {
			case 0:		//4010
				_irqEnabled = (value & 0x80) == 0x80;
				_loopFlag = (value & 0x40) == 0x40;

				//"The rate determines for how many CPU cycles happen between changes in the output level during automatic delta-encoded sample playback."
				//Because BaseApuChannel does not decrement when setting _timer, we need to actually set the value to 1 less than the lookup table
				_period = (GetNesModel() == NesModel::NTSC ? _dmcPeriodLookupTableNtsc : _dmcPeriodLookupTablePal)[value & 0x0F] - 1;

				if(!_irqEnabled) {
					CPU::ClearIRQSource(IRQSource::DMC);
				}
				break;

			case 1:		//4011
				_outputLevel = value & 0x7F;
				_shiftRegister = value & 0x7F;
				break;

			case 2:		//4012
				_sampleAddr = 0xC000 | ((uint32_t)value << 6);
				break;

			case 3:		//4013
				_sampleLength = (value << 4) | 0x0001;
				break;
		}
	}

	void SetEnabled(bool enabled)
	{
		if(!enabled) {
			_bytesRemaining = 0;
		} else if(_bytesRemaining == 0) {
			InitSample();
			FillReadBuffer();
		}
	}
};