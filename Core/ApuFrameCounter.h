#pragma once
#include "stdafx.h"
#include "IMemoryHandler.h"
#include "CPU.h"
#include "EmulationSettings.h"

enum class FrameType
{
	None = 0,
	QuarterFrame = 1,
	HalfFrame = 2,
};

class ApuFrameCounter : public IMemoryHandler, public Snapshotable
{
private:
	const vector<vector<int32_t>> _stepCyclesNtsc = { { { 7457, 14913, 22371, 29828, 29829, 29830},
																		 { 7457, 14913, 22371, 29829, 37281, 37282} } };
	const vector<vector<int32_t>> _stepCyclesPal =  { { { 8313, 16627, 24939, 33252, 33253, 33254},
																		 { 8313, 16627, 24939, 33253, 41565, 41566} } };
	const vector<vector<FrameType>> _frameType = { { { FrameType::QuarterFrame, FrameType::HalfFrame, FrameType::QuarterFrame, FrameType::None, FrameType::HalfFrame, FrameType::None },
																	 { FrameType::QuarterFrame, FrameType::HalfFrame, FrameType::QuarterFrame, FrameType::None, FrameType::HalfFrame, FrameType::None } } };

	vector<vector<int32_t>> _stepCycles;
	NesModel _nesModel;
	int32_t _nextIrqCycle;
	int32_t _previousCycle;
	uint32_t _currentStep;
	uint32_t _stepMode; //0: 4-step mode, 1: 5-step mode
	bool _inhibitIRQ;

	void (*_callback)(FrameType);

public:
	ApuFrameCounter(void (*frameCounterTickCallback)(FrameType))
	{
		_callback = frameCounterTickCallback;
		Reset(false);
	}

	void Reset(bool softReset)
	{
		_nextIrqCycle = 29828;

		//"After reset or power-up, APU acts as if $4017 were written with $00 from 9 to 12 clocks before first instruction begins."
		//Because of the 3-4 sequence reset delay, 9-12 clocks turns into 6-7
		_previousCycle = 6;

		//"After reset: APU mode in $4017 was unchanged", so we need to keep whatever value _stepMode has for soft resets
		if(!softReset) {
			_stepMode = 0;
		}

		_currentStep = 0;
		_inhibitIRQ = false;
	}

	void StreamState(bool saving)
	{
		Stream<int32_t>(_nextIrqCycle);
		Stream<int32_t>(_previousCycle);
		Stream<uint32_t>(_currentStep);
		Stream<uint32_t>(_stepMode);
		Stream<bool>(_inhibitIRQ);
		Stream<NesModel>(_nesModel);

		if(!saving) {
			SetNesModel(_nesModel);
		}
	}

	void SetNesModel(NesModel model)
	{
		if(_nesModel != model || _stepCycles.size() == 0) {
			_nesModel = model;
			_stepCycles.clear();
			_stepCycles.push_back(model == NesModel::NTSC ? _stepCyclesNtsc[0] : _stepCyclesPal[0]);
			_stepCycles.push_back(model == NesModel::NTSC ? _stepCyclesNtsc[1] : _stepCyclesPal[1]);
		}
	}

	uint32_t Run(int32_t &cyclesToRun)
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
		ranges.AddHandler(MemoryOperation::Write, 0x4017);
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

		//Reset sequence after $4017 is written to
		if(CPU::GetCycleCount() & 0x01) {
			//"If the write occurs during an APU cycle, the effects occur 3 CPU cycles after the $4017 write cycle"
			_previousCycle = -3;
		} else {
			//"If the write occurs between APU cycles, the effects occur 4 CPU cycles after the write cycle. "
			_previousCycle = -4;
		}
		_currentStep = 0;

		if(_stepMode == 1) {
			//"Writing to $4017 with bit 7 set will immediately generate a clock for both the quarter frame and the half frame units, regardless of what the sequencer is doing."
			_callback(FrameType::HalfFrame);
		}
	}
};