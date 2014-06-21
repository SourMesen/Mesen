#include "stdafx.h"
#include "Console.h"
#include "Timer.h"

Console::Console(wstring filename)
{
	_mapper = MapperFactory::InitializeFromFile(filename);
	_memoryManager.reset(new MemoryManager(_mapper->GetHeader()));
	_cpu.reset(new CPU(_memoryManager.get()));
	_ppu.reset(new PPU(_memoryManager.get()));	
	_memoryManager->RegisterIODevice(_mapper.get());
	_memoryManager->RegisterIODevice(_ppu.get());

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
	while(!_stopped) { }
}

void Console::Run()
{
	Timer timer;
	uint32_t lastFrameCount = 0;
	while(true) {
		_cpu->Exec();
		_ppu->Exec();

		if(_stop) {
			break;
		}

		if(timer.GetElapsedMS() > 2000) {
			uint32_t frameCount = _ppu->GetFrameCount();
			std::cout << ((frameCount - lastFrameCount) / (timer.GetElapsedMS() / 1000)) << std::endl;
			timer.Reset();
			lastFrameCount = frameCount;
		}
	}
	_stopped = true;
}

void Console::RunTest(bool callback(Console*))
{
	Timer timer;
	uint32_t lastFrameCount = 0;
	while(true) {
		if(callback(this)) {
			break;
		}

		_cpu->Exec();
		_ppu->Exec();

		if(timer.GetElapsedMS() > 2000) {
			uint32_t frameCount = _ppu->GetFrameCount();
			std::cout << ((frameCount - lastFrameCount) / (timer.GetElapsedMS() / 1000)) << std::endl;
			timer.Reset();
			lastFrameCount = frameCount;
		}
	}
}

void Console::Load(wstring filename)
{
	(new Console(filename))->Run();
}

void Console::RunTests()
{
	//(new Console("TestSuite/mario.nes"))->Run();
/*

	vector<wstring> testROMs {
		//"Bomberman",
		L"IceClimber",
		//"Excitebike",
		"dk",
		//"mario",
		"01-basics",
		"02-implied",
		"03-immediate",
		"04-zero_page",
		"05-zp_xy",
		"06-absolute",
		"07-abs_xy",
		"08-ind_x",
		"09-ind_y",
		"10-branches",
		"11-stack",
		"12-jmp_jsr",
		"13-rts",
		"14-rti",
		"15-brk",
		"16-special"
	};

	for(wstring testROM : testROMs) {
		Console *console = new Console(wstring(L"TestSuite/") + testROM + L".nes");
		if(testROM == L"nestest") {
			console->RunTest([] (Console *console) {
				State state = console->_cpu->GetState();
				std::cout << std::hex << std::uppercase << 
							 "A:"  << std::setfill('0') << std::setw(2) << (short)state.A << 
							" X:" << std::setfill('0') << std::setw(2) << (short)state.X << 
							" Y:" << std::setfill('0') << std::setw(2) << (short)state.Y << 
							" S:" << std::setfill('0') << std::setw(2) << (short)state.SP << 
							" P:........ $" << 
							std::setfill('0') << std::setw(4) << (short)state.PC <<std::endl;
				return false;
			});
		} else {
			console->RunTest([] (Console *console) {
				//static std::ofstream output("test.log", ios::out | ios::binary);
				static bool testStarted = false;
				uint8_t testStatus = console->_memoryManager->Read(0x6000);
				
				State state = console->_cpu->GetState();
				/*output << std::hex << std::uppercase << 
							 "A:"  << std::setfill('0') << std::setw(2) << (short)state.A << 
							" X:" << std::setfill('0') << std::setw(2) << (short)state.X << 
							" Y:" << std::setfill('0') << std::setw(2) << (short)state.Y << 
							" S:" << std::setfill('0') << std::setw(2) << (short)state.SP << 
							" P:........ $" << 
							std::setfill('0') << std::setw(4) << (short)state.PC <<std::endl;*/
			/*	
				if(testStatus == 0x81) {
					//need reset
					std::cout << "reset needed";
				} else if(testStatus == 0x80) {
					testStarted = true;
				} else if(testStatus < 0x80 && testStarted) {
					char *result = console->_memoryManager->GetTestResult();
					std::cout << result;
					delete[] result;
					testStarted = false;
					return true;
				}
				return false;
			});
		}
		delete console;
	}*/
}