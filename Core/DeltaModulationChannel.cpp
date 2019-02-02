#include "stdafx.h"
#include "DeltaModulationChannel.h"
#include "APU.h"
#include "CPU.h"
#include "Console.h"
#include "MemoryManager.h"

DeltaModulationChannel::DeltaModulationChannel(AudioChannel channel, shared_ptr<Console> console, SoundMixer *mixer) : BaseApuChannel(channel, console, mixer)
{
}

void DeltaModulationChannel::Reset(bool softReset)
{
	BaseApuChannel::Reset(softReset);

	if(!softReset) {
		//At power on, the sample address is set to $C000 and the sample length is set to 1
		//Resetting does not reset their value
		_sampleAddr = 0xC000;
		_sampleLength = 1;
	}

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

	_lastValue4011 = 0;

	//Not sure if this is accurate, but it seems to make things better rather than worse (for dpcmletterbox)
	//"On the real thing, I think the power-on value is 428 (or the equivalent at least - it uses a linear feedback shift register), though only the even/oddness should matter for this test."
	_period = (GetNesModel() == NesModel::NTSC ? _dmcPeriodLookupTableNtsc : _dmcPeriodLookupTablePal)[0] - 1;
	
	//Make sure the DMC doesn't tick on the first cycle - this is part of what keeps Sprite/DMC DMA tests working while fixing dmc_pitch.
	_timer = _period;
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
		_console->GetCpu()->StartDmcTransfer();
	}
}

void DeltaModulationChannel::FillReadBuffer()
{
	if(_bytesRemaining > 0) {
		_readBuffer = _console->GetMemoryManager()->Read(_currentAddr, MemoryOperationType::DmcRead);
		_bufferEmpty = false;

		//"The address is incremented; if it exceeds $FFFF, it is wrapped around to $8000."
		_currentAddr++;
		if(_currentAddr == 0) {
			_currentAddr = 0x8000;
		}

		_bytesRemaining--;

		if(_bytesRemaining == 0) {
			_needToRun = false;
			if(_loopFlag) {
				//Looped sample should never set IRQ flag
				InitSample();
			} else if(_irqEnabled) {
				_console->GetCpu()->SetIrqSource(IRQSource::DMC);
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
	Stream(_sampleAddr, _sampleLength, _outputLevel, _irqEnabled, _loopFlag, _currentAddr, _bytesRemaining, _readBuffer, _bufferEmpty, _shiftRegister, _bitsRemaining, _silenceFlag, _needToRun);
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
	_console->GetApu()->Run();

	switch(addr & 0x03) {
		case 0:		//4010
			_irqEnabled = (value & 0x80) == 0x80;
			_loopFlag = (value & 0x40) == 0x40;

			//"The rate determines for how many CPU cycles happen between changes in the output level during automatic delta-encoded sample playback."
			//Because BaseApuChannel does not decrement when setting _timer, we need to actually set the value to 1 less than the lookup table
			_period = (GetNesModel() == NesModel::NTSC ? _dmcPeriodLookupTableNtsc : _dmcPeriodLookupTablePal)[value & 0x0F] - 1;

			if(!_irqEnabled) {
				_console->GetCpu()->ClearIrqSource(IRQSource::DMC);
			}
			break;

		case 1: {		//4011
			uint8_t newValue = value & 0x7F;
			uint8_t previousLevel = _outputLevel;
			_outputLevel = newValue;
			
			if(_console->GetSettings()->CheckFlag(EmulationFlags::ReduceDmcPopping) && abs(_outputLevel - previousLevel) > 50) {
				//Reduce popping sounds for 4011 writes
				_outputLevel -= (_outputLevel - previousLevel) / 2;
			}

			//4011 applies new output right away, not on the timer's reload.  This fixes bad DMC sound when playing through 4011.
			AddOutput(_outputLevel);
			
			if(_lastValue4011 != value && newValue > 0) {
				_console->SetNextFrameOverclockStatus(true);
			}

			_lastValue4011 = newValue;
			break;
		}

		case 2:		//4012
			_sampleAddr = 0xC000 | ((uint32_t)value << 6);
			if(value > 0) {
				_console->SetNextFrameOverclockStatus(false);
			}
			break;

		case 3:		//4013
			_sampleLength = (value << 4) | 0x0001;
			if(value > 0) {
				_console->SetNextFrameOverclockStatus(false);
			}
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

ApuDmcState DeltaModulationChannel::GetState()
{
	ApuDmcState state;
	state.BytesRemaining = _bytesRemaining;
	state.IrqEnabled = _irqEnabled;
	state.Loop = _loopFlag;
	state.OutputVolume = _lastOutput;
	state.Period = _period;
	state.Timer = _timer;
	state.SampleRate = (double)_console->GetCpu()->GetClockRate(GetNesModel()) / (_period + 1);
	state.SampleAddr = _sampleAddr;
	state.SampleLength = _sampleLength;
	return state;
}