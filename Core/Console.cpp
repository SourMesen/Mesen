#include "stdafx.h"
#include "Console.h"
#include "MapperFactory.h"
#include "../Utilities/Timer.h"

uint32_t Console::Flags = 0;
uint32_t Console::CurrentFPS = 0;

Console::Console(wstring filename)
{
	_romFilename = filename;

	_mapper = MapperFactory::InitializeFromFile(filename);
	_memoryManager.reset(new MemoryManager(_mapper));
	_cpu.reset(new CPU(_memoryManager.get()));
	_ppu.reset(new PPU(_memoryManager.get()));	
	_apu.reset(new APU(_memoryManager.get()));

	_controlManager.reset(new ControlManager());

	_memoryManager->RegisterIODevice(_mapper.get());
	_memoryManager->RegisterIODevice(_ppu.get());
	_memoryManager->RegisterIODevice(_apu.get());
	_memoryManager->RegisterIODevice(_controlManager.get());

	ResetComponents(false);
}

Console::~Console()
{
}

void Console::Reset()
{
	_reset = true;
}

void Console::ResetComponents(bool softReset)
{
	_cpu->Reset(softReset);
	_ppu->Reset();
	_apu->Reset();
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
	Console::Flags &= ~flags;
}

bool Console::CheckFlag(int flag)
{
	return (Console::Flags & flag) == flag;
}

uint32_t Console::GetFPS()
{
	return Console::CurrentFPS;
}

void Console::Run()
{
	Timer clockTimer;
	Timer fpsTimer;
	uint32_t lastFrameCount = 0;
	double elapsedTime = 0;
	double targetTime = 16.6666666666666666;
	while(true) { 
		uint32_t executedCycles = _cpu->Exec();
		_ppu->Exec();

		bool frameDone = _apu->Exec(executedCycles);

		if(frameDone) {
			_cpu->EndFrame();
			_ppu->EndFrame();
		}

		if(CheckFlag(EmulationFlags::LimitFPS) && frameDone) {
			elapsedTime = clockTimer.GetElapsedMS();
			while(targetTime > elapsedTime) {
				if(targetTime - elapsedTime > 2) {
					std::this_thread::sleep_for(std::chrono::duration<int, std::milli>((int)(targetTime - elapsedTime - 1)));
				}
				elapsedTime = clockTimer.GetElapsedMS();
			}
			clockTimer.Reset();
		}
		
		if(fpsTimer.GetElapsedMS() > 1000) {
			uint32_t frameCount = _ppu->GetFrameCount();
			Console::CurrentFPS = (int)(std::round((double)(frameCount - lastFrameCount) / (fpsTimer.GetElapsedMS() / 1000)));
			lastFrameCount = frameCount;
			fpsTimer.Reset();
		}

		if(!_saveStateFilename.empty()) {
			SaveState();
		} else if(!_loadStateFilename.empty()) {
			LoadState();
		}

		if(_stop) {
			_stop = false;
			break;
		}

		if(_reset) {
			clockTimer.Reset();
			fpsTimer.Reset();
			lastFrameCount = 0;
			elapsedTime = 0;
			ResetComponents(true);
			_reset = false;
		}
	}
}

void Console::SaveState(wstring filename)
{
	_saveStateFilename = filename;
}

void Console::SaveState()
{
	ofstream file(_saveStateFilename, ios::out | ios::binary);

	if(file) {
		_cpu->SaveSnapshot(&file);
		_ppu->SaveSnapshot(&file);
		_memoryManager->SaveSnapshot(&file);
		_mapper->SaveSnapshot(&file);
		_apu->SaveSnapshot(&file);
		file.close();
	}

	_saveStateFilename.clear();
}

bool Console::LoadState(wstring filename)
{
	ifstream file(filename, ios::out | ios::binary);

	if(file) {
		file.close();
		_loadStateFilename = filename;
		return true;
	}
	return false;
}

void Console::LoadState()
{
	ifstream file(_loadStateFilename, ios::out | ios::binary);
	
	if(file) {
		_cpu->LoadSnapshot(&file);
		_ppu->LoadSnapshot(&file);
		_memoryManager->LoadSnapshot(&file);
		_mapper->LoadSnapshot(&file);
		_apu->LoadSnapshot(&file);
		file.close();
	}

	_loadStateFilename.clear();
}

bool Console::RunTest(uint8_t *expectedResult)
{
	Timer timer;
	uint8_t maxWait = 60;
	uint8_t* lastFrameBuffer = new uint8_t[256 * 240 * 4];
	while(true) {
		uint32_t executedCycles = _cpu->Exec();
		_ppu->Exec();

		if(_apu->Exec(executedCycles)) {
			_cpu->EndFrame();
			_ppu->EndFrame();
		}

		if(timer.GetElapsedMS() > 100) {
			if(memcmp(_ppu->GetFrameBuffer(), expectedResult, 256 * 240 * 4) == 0) {
				return true;
			}

			timer.Reset();

			if(memcmp(lastFrameBuffer, _ppu->GetFrameBuffer(), 256 * 240 * 4) != 0) {
				memcpy(lastFrameBuffer, _ppu->GetFrameBuffer(), 256 * 240 * 4);
				maxWait = 60;
			}

			maxWait--;
			if(maxWait == 0) {
				return false;
			}
		}
	}
	
	delete[] lastFrameBuffer;

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