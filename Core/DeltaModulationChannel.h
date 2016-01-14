#pragma once
#include "stdafx.h"
#include "IMemoryHandler.h"
#include "BaseApuChannel.h"

class MemoryManager;

class DeltaModulationChannel : public BaseApuChannel<127>
{
private:	
	const vector<uint16_t> _dmcPeriodLookupTableNtsc = { { 428, 380, 340, 320, 286, 254, 226, 214, 190, 160, 142, 128, 106,  84,  72,  54 } };
	const vector<uint16_t> _dmcPeriodLookupTablePal = { { 398, 354, 316, 298, 276, 236, 210, 198, 176, 148, 132, 118,  98,  78,  66,  50 } };
	static DeltaModulationChannel *Instance;

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
	bool _needToRun = false;

	void InitSample();
	void FillReadBuffer();
	
	void Clock();

public:
	DeltaModulationChannel(AudioChannel channel, SoundMixer* mixer, MemoryManager* memoryManager);

	virtual void Reset(bool softReset);
	virtual void StreamState(bool saving);

	bool IrqPending(uint32_t cyclesToRun);
	bool NeedToRun();
	bool GetStatus();
	void GetMemoryRanges(MemoryRanges &ranges);
	void WriteRAM(uint16_t addr, uint8_t value);

	void SetEnabled(bool enabled);
	void StartDmcTransfer();
	static void SetReadBuffer();
};