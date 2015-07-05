#include <tchar.h>

extern "C" {
	void __stdcall LoadROM(wchar_t* filename);
	void __stdcall Run();
}

int _tmain(int argc, _TCHAR* argv[])
{
	LoadROM(argv[1]);
	Run();
	return 0;
}

