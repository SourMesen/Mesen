#include <iostream>
#include <thread>
#include <vector>
#include <string>

extern "C" {
	void __stdcall InitializeEmu(char* homeFolder, void*, void*);
	void __stdcall LoadROM(char* filename);
	void __stdcall Run();
	void __stdcall Stop();
}

int main(int argc, char* argv[])
{
	using namespace std;
	vector<char*> testRoms{
		"..\\..\\Games\\Super Dodge Ball (USA).nes",
		"..\\..\\Games\\Super Mario Bros. (USA).nes",
		"..\\..\\Games\\Mega Man 2 (USA).nes",
		"..\\..\\Games\\Mega Man 3 (USA).nes",
		"..\\..\\Games\\Mega Man 4 (USA).nes",
		"..\\..\\Games\\MMC5\\Just Breed (J) [!].nes"
	};

	InitializeEmu("C:\\Windows\\Temp\\Mesen", nullptr, nullptr);
	LoadROM(testRoms[0]);
	std::cout << "Running: " << testRoms[0] << std::endl;
	thread testThread([testRoms] {
		for(size_t i = 1; i < testRoms.size(); i++) {
			std::this_thread::sleep_for(std::chrono::duration<int, std::milli>(5000));
			std::cout << "Running: " << testRoms[i] << std::endl;
			LoadROM(testRoms[i]);
		}
		Stop();
	});
	Run();
	testThread.join();
	return 0;
}

