#include "stdafx.h"
#include "DeltaModulationChannel.h"
#include "APU.h"
#include "CPU.h"

DeltaModulationChannel *DeltaModulationChannel::Instance = nullptr;

DeltaModulationChannel::DeltaModulationChannel(AudioChannel channel, SoundMixer *mixer, MemoryManager* memoryManager) : BaseApuChannel(channel, mixer)
{
	Instance = this;
	_memoryManager = memoryManager;
}

void DeltaModulationChannel::Reset(bool softReset)
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
	_bitsRemaining = 8;
	_silenceFlag = true;
	_needToRun = false;

	//Not sure if this is accurate, but it seems to make things better rather than worse (for dpcmletterbox)
	//"On the real thing, I think the power-on value is 428 (or the equivalent at least - it uses a linear feedback shift register), though only the even/oddness should matter for this test."
	_period = (GetNesModel() == NesModel::NTSC ? _dmcPeriodLookupTableNtsc : _dmcPeriodLookupTablePal)[0] - 1;
}

void DeltaModulationChannel::InitSample()
{
	_currentAddr = _sampleAddr;
	_bytesRemaining = _sampleLength;
	_needToRun = _bytesRemaining > 0;
}

void DeltaModulationChannel::StartDmcTransfer()
{
	if(_bufferEmpty && _bytesRemaining > 0) {
		CPU::StartDmcTransfer();
	}
}

void DeltaModulationChannel::FillReadBuffer()
{
	if(_bytesRemaining > 0) {
		_readBuffer = _memoryManager->Read(_currentAddr);
		_bufferEmpty = false;

		_currentAddr++;
		_bytesRemaining--;

		if(_bytesRemaining == 0) {
			_needToRun = false;
			if(_loopFlag) {
				//Looped sample should never set IRQ flag
				InitSample();
			} else if(_irqEnabled) {
				CPU::SetIRQSource(IRQSource::DMC);
			}
		}
	}
}

void DeltaModulationChannel::Clock()
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
			StartDmcTransfer();
		}
	}

	AddOutput(_outputLevel);
}

void DeltaModulationChannel::StreamState(bool saving)
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
	Stream<bool>(_needToRun);
}

bool DeltaModulationChannel::IrqPending(uint32_t cyclesToRun)
{
	if(_irqEnabled && _bytesRemaining > 0) {
		uint32_t cyclesToEmptyBuffer = (_bitsRemaining + (_bytesRemaining-1)* 8) * _period;
		if(cyclesToRun >= cyclesToEmptyBuffer) {
			return true;
		}
	}
	return false;
}

bool DeltaModulationChannel::GetStatus()
{
	return _bytesRemaining > 0;
}

void DeltaModulationChannel::GetMemoryRanges(MemoryRanges &ranges)
{
	ranges.AddHandler(MemoryOperation::Write, 0x4010, 0x4013);
}

void DeltaModulationChannel::WriteRAM(uint16_t addr, uint8_t value)
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

			//4011 applies new output right away, not on the timer's reload.  This fixes bad DMC sound when playing through 4011.
			AddOutput(_outputLevel);
			break;

		case 2:		//4012
			_sampleAddr = 0xC000 | ((uint32_t)value << 6);
			break;

		case 3:		//4013
			_sampleLength = (value << 4) | 0x0001;
			break;
	}
}

void DeltaModulationChannel::SetEnabled(bool enabled)
{
	if(!enabled) {
		_bytesRemaining = 0;
		_needToRun = false;
	} else if(_bytesRemaining == 0) {
		InitSample();
		StartDmcTransfer();
	}
}

bool DeltaModulationChannel::NeedToRun()
{
	return _needToRun;
}

void DeltaModulationChannel::SetReadBuffer()
{
	Instance->FillReadBuffer();
}