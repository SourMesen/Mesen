#pragma once
#include "Snapshotable.h"
#include "CPU.h"

class VrcIrq : Snapshotable
{
private:
	uint8_t _irqReloadValue;
	uint8_t _irqCounter;
	int16_t _irqPrescalerCounter;
	bool _irqEnabled;
	bool _irqEnabledAfterAck;
	bool _irqCycleMode;

protected:
	void StreamState(bool saving)
	{
		Stream<uint8_t>(_irqReloadValue);
		Stream<uint8_t>(_irqCounter);
		Stream<int16_t>(_irqPrescalerCounter);
		Stream<bool>(_irqEnabled);
		Stream<bool>(_irqEnabledAfterAck);
		Stream<bool>(_irqCycleMode);
	}

public:
	void Reset()
	{
		_irqPrescalerCounter = 0;
		_irqReloadValue = 0;
		_irqCounter = 0;
		_irqEnabled = false;
		_irqEnabledAfterAck = false;
		_irqCycleMode = false;
	}

	void ProcessCpuClock()
	{
		if(_irqEnabled) {
			_irqPrescalerCounter -= 3;

			if(_irqCycleMode || (_irqPrescalerCounter <= 0 && !_irqCycleMode)) {
				if(_irqCounter == 0xFF) {
					_irqCounter = _irqReloadValue;
					CPU::SetIRQSource(IRQSource::External);
				} else {
					_irqCounter++;
				}
				_irqPrescalerCounter += 341;
			}
		}
	}

	void SetReloadValue(uint8_t value)
	{
		_irqReloadValue = value;
	}

	void SetControlValue(uint8_t value)
	{
		_irqEnabledAfterAck = (value & 0x01) == 0x01;
		_irqEnabled = (value & 0x02) == 0x02;
		_irqCycleMode = (value & 0x04) == 0x04;

		if(_irqEnabled) {
			_irqCounter = _irqReloadValue;
			_irqPrescalerCounter = 341;
		}
	}

	void AcknowledgeIrq()
	{
		_irqEnabled = _irqEnabledAfterAck;
		CPU::ClearIRQSource(IRQSource::External);
	}

};