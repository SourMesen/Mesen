#pragma once
#include "stdafx.h"
#include "IMemoryHandler.h"
#include "../BlipBuffer/Blip_Buffer.h"
#include "EmulationSettings.h"
#include "Snapshotable.h"

template<int range>
class BaseApuChannel : public IMemoryHandler, public Snapshotable
{
private:
	unique_ptr<Blip_Synth<blip_good_quality, range>> _synth;
	Blip_Buffer *_buffer;

	uint16_t _lastOutput;
	uint32_t _previousCycle;
	AudioChannel _channel;
	double _baseVolume;
	NesModel _nesModel;

protected:
	uint16_t _timer = 0;
	uint16_t _period = 0;
	uint32_t _clockDivider = 2; //All channels except triangle clock overy other cpu clock

	void SetVolume(double volume)
	{
		_baseVolume = volume;
		UpdateSynthVolume();
	}

	void UpdateSynthVolume()
	{
		_synth->volume(_baseVolume * EmulationSettings::GetChannelVolume(_channel) * 2);
	}

	AudioChannel GetChannel()
	{
		return _channel;
	}

public:
	virtual void Clock() = 0;
	virtual bool GetStatus() = 0;

	BaseApuChannel(AudioChannel channel, Blip_Buffer *buffer)
	{
		_channel = channel;
		_buffer = buffer;
		_synth.reset(new Blip_Synth<blip_good_quality, range>());
		
		Reset(false);
	}

	virtual void Reset(bool softReset)
	{
		_timer = 0;
		_period = 0;
		_lastOutput = 0;
		_previousCycle = 0;
		_buffer->clear();
	}

	virtual void StreamState(bool saving)
	{
		Stream<uint16_t>(_lastOutput);
		Stream<uint32_t>(_previousCycle);
		Stream<uint16_t>(_timer);
		Stream<uint16_t>(_period);
		Stream<NesModel>(_nesModel);

		if(!saving) {
			_buffer->clear();
		}
	}

	void SetNesModel(NesModel model)
	{
		_nesModel = model;
	}

	NesModel GetNesModel()
	{
		return _nesModel;
	}

	virtual void Run(uint32_t targetCycle)
	{
		while(_previousCycle < targetCycle) {
			if(_timer == 0) {
				Clock();
				_timer = _period;
				_previousCycle += _clockDivider;
			} else {
				uint32_t cyclesToRun = (targetCycle - _previousCycle) / _clockDivider;
				uint16_t skipCount = _timer > cyclesToRun ? cyclesToRun : _timer;
				_timer -= skipCount;
				_previousCycle += skipCount * _clockDivider;

				if(cyclesToRun == 0) {
					break;
				}
			}
		}
	}
	
	uint8_t ReadRAM(uint16_t addr)
	{
		return 0;
	}

	void AddOutput(uint16_t output)
	{
		if(output != _lastOutput) {
			_synth->offset_inline(_previousCycle, output - _lastOutput, _buffer);
		}
		_lastOutput = output;
	}

	void EndFrame()
	{
		_previousCycle = 0;

		//Update options at the end of the cycle
		UpdateSynthVolume();
	}
};