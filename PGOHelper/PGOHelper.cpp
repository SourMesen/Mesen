#include <iostream>
#include <thread>
#include <vector>
#include <string>

extern "C" {
	void __stdcall InitializeEmu(char* homeFolder, void*, void*, bool, bool, bool);
	void __stdcall LoadROM(const char* filename, int32_t archiveFileIndex);
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
		"..\\..\\Games\\Mega Man 5 (USA).nes",
		"..\\..\\Games\\Mega Man 6 (USA).nes",
		"..\\..\\Games\\MMC5\\Just Breed (J) [!].nes",
		"..\\..\\Games\\MMC5\\Castlevania III - Dracula's Curse (U) [!].nes",
		"..\\..\\Games\\Blades of Steel (USA).nes",
		"..\\..\\Games\\Kirby's Adventure (USA).nes",
		"..\\..\\Games\\Legend of Zelda, The (USA).nes",
		"..\\..\\Games\\Super Mario Bros. 3 (USA).nes",
		"..\\..\\Games\\Teenage Mutant Ninja Turtles II - The Arcade Game (USA).nes",		
		"..\\..\\Games\\Dragon Warrior III (USA).nes",
		"..\\..\\Games\\Dragon Warrior IV (USA).nes"
	};

	InitializeEmu("C:\\Windows\\Temp\\Mesen", nullptr, nullptr, false, false, false);
	LoadROM(testRoms[0], -1);
	std::cout << "Running: " << testRoms[0] << std::endl;
	thread testThread([testRoms] {
		for(size_t i = 1; i < testRoms.size(); i++) {
			std::this_thread::sleep_for(std::chrono::duration<int, std::milli>(5000));
			std::cout << "Running: " << testRoms[i] << std::endl;
			LoadROM(testRoms[i], -1);
		}
		Stop();
	});
	Run();
	testThread.join();
	return 0;
}

