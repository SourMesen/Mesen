#include "stdafx.h"
#include "Console.h"
#include "Timer.h"

uint32_t Console::Flags = 0;

Console::Console(wstring filename)
{
	_romFilename = filename;

	_mapper = MapperFactory::InitializeFromFile(filename);
	_memoryManager.reset(new MemoryManager(_mapper->GetHeader()));
	_cpu.reset(new CPU(_memoryManager.get()));
	_ppu.reset(new PPU(_memoryManager.get()));	
	_apu.reset(new APU());

	_controlManager.reset(new ControlManager());

	_memoryManager->RegisterIODevice(_mapper.get());
	_memoryManager->RegisterIODevice(_ppu.get());
	_memoryManager->RegisterIODevice(_apu.get());
	_memoryManager->RegisterIODevice(_controlManager.get());

	Reset();
}

Console::~Console()
{
}

void Console::Reset()
{
	_cpu->Reset();
}

void Console::Stop()
{
	_stop = true;
}

void Console::SetFlags(int flags)
{
	Console::Flags |= flags;
}

void Console::ClearFlags(int flags)
{
	Console::Flags  ^= flags;
}

bool Console::CheckFlag(int flag)
{
	return (Console::Flags & flag) == flag;
}

void Console::Run()
{
	Timer clockTimer;
	Timer fpsTimer;
	uint32_t lastFrameCount = 0;
	double elapsedTime = 0;
	uint32_t cycleCount = 0;
	while(true) { 
		uint32_t executedCycles = _cpu->Exec();
		_ppu->Exec();
		_apu->Exec(executedCycles);

		cycleCount += executedCycles;

		if(CheckFlag(EmulationFlags::LimitFPS) && cycleCount >= 29780) {
			double targetTime = 16.638935108153078202995008319468;
			elapsedTime = clockTimer.GetElapsedMS();
			while(targetTime > elapsedTime) {
				if(targetTime - elapsedTime > 2) {
					std::this_thread::sleep_for(std::chrono::duration<int, std::milli>((int)(targetTime - elapsedTime - 1)));
				}
				elapsedTime = clockTimer.GetElapsedMS();
			}
			cycleCount = 0;
			clockTimer.Reset();
		}
		
		if(fpsTimer.GetElapsedMS() > 1000) {
			uint32_t frameCount = _ppu->GetFrameCount();
			std::cout << ((frameCount - lastFrameCount) / (fpsTimer.GetElapsedMS() / 1000)) << std::endl;
			lastFrameCount = frameCount;
			fpsTimer.Reset();
		}

		if(_stop) {
			break;
		}
	}
}

bool Console::RunTest(uint8_t *expectedResult)
{
	Timer timer;
	uint8_t maxWait = 30;
	while(true) {
		_cpu->Exec();
		_ppu->Exec();

		if(timer.GetElapsedMS() > 100) {
			if(memcmp(_ppu->GetFrameBuffer(), expectedResult, 256 * 240 * 4)) {
				return true;
			}
			
			timer.Reset();
			maxWait--;

			if(maxWait == 0) {
				return false;
			}
		}
	}

	return false;
}

void Console::SaveTestResult()
{
	wstring filename = _romFilename + L".trt";
	
	ofstream testResultFile(filename, ios::out | ios::binary);

	if(!testResultFile) {
		throw std::exception("File could not be opened");
	}

	uint8_t* buffer = new uint8_t[256 * 240 * 4];
	memcpy(buffer, _ppu->GetFrameBuffer(), 256 * 240 * 4);

	testResultFile.write((char *)buffer, 256 * 240 * 4);
	
	testResultFile.close();

	delete[] buffer;
}