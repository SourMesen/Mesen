#include "stdafx.h"
#include "Console.h"

Console::Console(char* filename)
{
	_mapper = MapperFactory::InitializeFromFile(filename);
	_memoryManager.RegisterIODevice(_mapper.get());
	_memoryManager.RegisterIODevice(&_ppu);
	_cpu.reset(new CPU(&_memoryManager));
}

Console::~Console()
{

}

void Console::Reset()
{
	_cpu->Reset();
}

void Console::Run()
{
	_cpu->Exec();
}

void Console::RunBenchmark()
{
	Console console("mario.nes");
	console.Run();
}