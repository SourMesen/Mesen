#pragma once
#include "stdafx.h"
#include "IMemoryHandler.h"
#include "CPU.h"

enum class FrameType
{
	None = 0,
	QuarterFrame = 1,
	HalfFrame = 2,
};

class ApuFrameCounter : public IMemoryHandler, public Snapshotable
{
private:
	const vector<vector<uint32_t>> _stepCycles = { { { 7457, 14913, 22371, 29828, 29829, 29830},
																	 { 7457, 14913, 22371, 29829, 37281, 37282} } };
	const vector<vector<FrameType>> _frameType = { { { FrameType::QuarterFrame, FrameType::HalfFrame, FrameType::QuarterFrame, FrameType::None, FrameType::HalfFrame, FrameType::None },
																	 { FrameType::QuarterFrame, FrameType::HalfFrame, FrameType::QuarterFrame, FrameType::None, FrameType::HalfFrame, FrameType::None } } };

	int32_t _nextIrqCycle = 29828;
	uint32_t _previousCycle = 0;
	uint32_t _currentStep = 0;
	uint32_t _stepMode = 0; //0: 4-step mode, 1: 5-step mode
	bool _inhibitIRQ = false;

	void (*_callback)(FrameType);

public:
	ApuFrameCounter(void (*frameCounterTickCallback)(FrameType))
	{
		_callback = frameCounterTickCallback;
	}

	void Reset()
	{
		_nextIrqCycle = 29828;
		_previousCycle = 0;
		_currentStep = 0;
		_stepMode = 0;
		_inhibitIRQ = false;
	}

	void StreamState(bool saving)
	{
		Stream<int32_t>(_nextIrqCycle);
		Stream<uint32_t>(_previousCycle);
		Stream<uint32_t>(_currentStep);
		Stream<uint32_t>(_stepMode);
		Stream<bool>(_inhibitIRQ);
	}	

	uint32_t Run(uint32_t &cyclesToRun)
	{
		uint32_t cyclesRan;

		if(_previousCycle + cyclesToRun >= _stepCycles[_stepMode][_currentStep]) {
			if(!_inhibitIRQ && _stepMode == 0 && _currentStep >= 3) {
				//Set irq on the last 3 cycles for 4-step mode
				CPU::SetIRQSource(IRQSource::FrameCounter);
				_nextIrqCycle++;
			}

			FrameType type = _frameType[_stepMode][_currentStep];
			if(type != FrameType::None) {
				_callback(type);
			}

			cyclesRan = _stepCycles[_stepMode][_currentStep] - _previousCycle;
			cyclesToRun -= cyclesRan;

			_currentStep++;
			if(_currentStep == 6) {
				_currentStep = 0;
				_previousCycle = 0;
				if(_stepMode == 0 && !_inhibitIRQ) {
					_nextIrqCycle = 29828;
				}
			} else {
				_previousCycle += cyclesRan;
			}
		} else {
			cyclesRan = cyclesToRun;
			cyclesToRun = 0;
			_previousCycle += cyclesRan;
		}
		return cyclesRan;
	}

	bool IrqPending(uint32_t cyclesToRun)
	{
		if(_nextIrqCycle != -1) {
			if(_previousCycle + cyclesToRun >= (uint32_t)_nextIrqCycle) {
				return true;
			}
		}
		return false;
	}

	void GetMemoryRanges(MemoryRanges &ranges)
	{
		ranges.AddHandler(MemoryType::RAM, MemoryOperation::Write, 0x4017);
	}

	uint8_t ReadRAM(uint16_t addr)
	{
		return 0;
	}

	void WriteRAM(uint16_t addr, uint8_t value)
	{
		APU::StaticRun();
		_stepMode = ((value & 0x80) == 0x80) ? 1 : 0;
		_inhibitIRQ = (value & 0x40) == 0x40;
		_nextIrqCycle = -1;

		if(_inhibitIRQ) {
			CPU::ClearIRQSource(IRQSource::FrameCounter);
		} else if(_stepMode == 0) {
			_nextIrqCycle = 29828;
		}

		//Reset sequence when $4017 is written to
		_previousCycle = 0;
		_currentStep = 0;

		if(_stepMode == 1) {
			//Writing to $4017 with bit 7 set will immediately generate a clock for both the quarter frame and the half frame units, regardless of what the sequencer is doing.
			_callback(FrameType::HalfFrame);
		}
	}
};