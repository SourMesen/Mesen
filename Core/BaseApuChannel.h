#pragma once
#include "stdafx.h"
#include "IMemoryHandler.h"
#include "EmulationSettings.h"
#include "Snapshotable.h"
#include "SoundMixer.h"

class BaseApuChannel : public IMemoryHandler, public Snapshotable
{
private:
	SoundMixer *_mixer;
	int8_t _lastOutput;
	uint32_t _previousCycle;
	AudioChannel _channel;
	NesModel _nesModel;

protected:
	uint16_t _timer = 0;
	uint16_t _period = 0;

	AudioChannel GetChannel()
	{
		return _channel;
	}

public:
	virtual void Clock() = 0;
	virtual bool GetStatus() = 0;

	BaseApuChannel(AudioChannel channel, SoundMixer *mixer)
	{
		_channel = channel;
		_mixer = mixer;
		
		Reset(false);
	}

	virtual void Reset(bool softReset)
	{
		_timer = 0;
		_period = 0;
		_lastOutput = 0;
		_previousCycle = 0;
		if(_mixer) {
			//_mixer is null/not needed for MMC5 square channels
			_mixer->Reset();
		}
	}

	virtual void StreamState(bool saving)
	{
		if(!saving) {
			_previousCycle = 0;
		}

		Stream(_lastOutput, _timer, _period, _nesModel);
	}

	void SetNesModel(NesModel model)
	{
		_nesModel = model;
	}

	NesModel GetNesModel()
	{
		if(_nesModel == NesModel::NTSC || _nesModel == NesModel::Dendy) {
			//Dendy APU works with NTSC timings
			return NesModel::NTSC;
		} else {
			return _nesModel;
		}
	}

	virtual void Run(uint32_t targetCycle)
	{
		while(_previousCycle < targetCycle) {
			if(_timer == 0) {
				Clock();
				_timer = _period;
				_previousCycle++;
			} else {
				uint32_t cyclesToRun = targetCycle - _previousCycle;
				uint16_t skipCount = _timer > cyclesToRun ? cyclesToRun : _timer;
				_timer -= skipCount;
				_previousCycle += skipCount;

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

	virtual void AddOutput(int8_t output)
	{
		if(output != _lastOutput) {
			_mixer->AddDelta(_channel, _previousCycle, output - _lastOutput);
			_lastOutput = output;
		}
	}

	void EndFrame()
	{
		_previousCycle = 0;
	}
};