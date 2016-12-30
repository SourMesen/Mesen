#include <iostream>
#include <thread>
#include <vector>
#include <string>

enum class VideoFilterType
{
	None = 0,
	NTSC = 1,
	BisqwitNtscQuarterRes = 2,
	BisqwitNtscHalfRes = 3,
	BisqwitNtsc = 4,
	xBRZ2x = 5,
	xBRZ3x = 6,
	xBRZ4x = 7,
	xBRZ5x = 8,
	xBRZ6x = 9,
	HQ2x = 10,
	HQ3x = 11,
	HQ4x = 12,
	Scale2x = 13,
	Scale3x = 14,
	Scale4x = 15,
	_2xSai = 16,
	Super2xSai = 17,
	SuperEagle = 18,
	Prescale2x = 19,
	Prescale3x = 20,
	Prescale4x = 21,
	HdPack = 999
};


extern "C" {
	void __stdcall SetVideoFilter(VideoFilterType filter);
	void __stdcall InitializeEmu(char* homeFolder, void*, void*, bool, bool, bool);
	void __stdcall LoadROM(const char* filename, int32_t archiveFileIndex, char* ipsFile);
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
	LoadROM(testRoms[0], -1, "");
	std::cout << "Running: " << testRoms[0] << std::endl;

	thread testThread([testRoms] {
		VideoFilterType filterTypes[13] = {
			VideoFilterType::BisqwitNtscQuarterRes,
			VideoFilterType::HQ2x,
			VideoFilterType::HQ3x,
			VideoFilterType::HQ4x,
			VideoFilterType::NTSC,
			VideoFilterType::Scale2x,
			VideoFilterType::Scale3x,
			VideoFilterType::Scale4x,
			VideoFilterType::xBRZ2x,
			VideoFilterType::xBRZ3x,
			VideoFilterType::xBRZ4x,
			VideoFilterType::xBRZ5x,
			VideoFilterType::xBRZ6x,
		};

		for(size_t i = 1; i < testRoms.size(); i++) {
			std::this_thread::sleep_for(std::chrono::duration<int, std::milli>(5000));
			std::cout << "Running: " << testRoms[i] << std::endl;
			SetVideoFilter(filterTypes[i % 13]);
			LoadROM(testRoms[i], -1, "");
		}
		std::this_thread::sleep_for(std::chrono::duration<int, std::milli>(5000));
		Stop();
	});
	Run();
	testThread.join();
	return 0;
}

