extern "C" {
	void __stdcall LoadROM(char* filename);
	void __stdcall Run();
}

int main(int argc, char* argv[])
{
	LoadROM(argv[1]);
	Run();
	return 0;
}

