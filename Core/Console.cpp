#include "stdafx.h"
#include "Console.h"
#include "Timer.h"

Console::Console(string filename)
{
	_mapper = MapperFactory::InitializeFromFile(filename);
	_memoryManager.RegisterIODevice(_mapper.get());
	_memoryManager.RegisterIODevice(&_ppu);
	_cpu.reset(new CPU(&_memoryManager));
}

Console::~Console()
{
	_cpu.release();
}

void Console::Reset()
{
	_cpu->Reset();
}

void Console::Run()
{
	while(true) {
		_cpu->Exec();
		_ppu.Exec();
	}
}

void Console::RunTest(bool callback(Console*))
{
	while(true) {
		_cpu->Exec();
		_ppu.Exec();

		if(callback(this)) {
			break;
		}
	}
}

void Console::RunTests()
{
	/*Console *console = new Console("mario.nes");
	console->Run();
	delete console;*/

	vector<std::string> testROMs = { { 
		"01-basics", 
		"02-implied",
		//"03-immediate",
		//"04-zero_page", 
		//"05-zp_xy", 
		//"06-absolute",
		//"07-abs_xy", 
		"08-ind_x", 
		"09-ind_y", 
		"10-branches",
		"11-stack", 
		"12-jmp_jsr", 
		"13-rts", 
		"14-rti", 
		//"15-brk", 
		"16-special"
	} };

	for(string testROM : testROMs) {
		Console *console = new Console(string("TestSuite/") + testROM + ".nes");
		if(testROM == "nestest") {
			console->RunTest([] (Console *console) {
				auto state = console->_cpu->GetState();
				std::stringstream ss;
				ss << std::hex << (short)state.PC << " A:" << (short)state.A << " X:" << (short)state.X << " Y:" << (short)state.Y << std::endl;
				OutputDebugStringA(ss.str().c_str());
				return false;
			});
		} else {
			console->RunTest([] (Console *console) {
				//static std::ofstream output("test.log", ios::out | ios::binary);
				static bool testStarted = false;
				uint8_t testStatus = console->_memoryManager.Read(0x6000);
				
				State state = console->_cpu->GetState();
				//output << std::hex << (short)state.PC << " A:" << (short)state.A << " X:" << (short)state.X << " Y:" << (short)state.Y << std::endl;
				
				if(testStatus == 0x81) {
					//need reset
					std::cout << "reset needed";
				} else if(testStatus == 0x80) {
					testStarted = true;
				} else if(testStatus < 0x80 && testStarted) {
					char *result = console->_memoryManager.GetTestResult();
					std::cout << result;
					delete[] result;
					testStarted = false;
					return true;
				}
				return false;
			});
		}
		delete console;
	}
}