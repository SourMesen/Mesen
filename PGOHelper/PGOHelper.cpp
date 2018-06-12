#ifdef _WIN32
#else
	#define __stdcall	
#endif

#include <iostream>
#include <thread>
#include <vector>
#include <string>
#include <algorithm>
#include <unordered_set>
#include <experimental/filesystem>
namespace fs = std::experimental::filesystem;
using std::string;
using std::vector;
using std::thread;

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
	Prescale6x = 22,
	Prescale8x = 23,
	Prescale10x = 24,
	HdPack = 999
};


extern "C" {
	void __stdcall SetFlags(uint64_t flags);
	void __stdcall SetVideoFilter(VideoFilterType filter);
	void __stdcall InitializeEmu(const char* homeFolder, void*, void*, bool, bool, bool);
	void __stdcall LoadROM(const char* filename, const char* patchFile);
	void __stdcall Run();
	void __stdcall Release();
	void __stdcall Stop();
	void __stdcall DebugInitialize();
}

vector<string> GetFilesInFolder(string rootFolder, std::unordered_set<string> extensions)
{
	vector<string> files;
	vector<string> folders = { { rootFolder } };
	
	std::error_code errorCode;
	if(!fs::is_directory(fs::u8path(rootFolder), errorCode)) {
		return files;
	}

	for(string folder : folders) {
		for(fs::directory_iterator i(fs::u8path(folder.c_str())), end; i != end; i++) {
			string extension = i->path().extension().u8string();
			std::transform(extension.begin(), extension.end(), extension.begin(), ::tolower);
			if(extensions.find(extension) != extensions.end()) {
				files.push_back(i->path().u8string());
			}
		}
	}

	return files;
}

int main(int argc, char* argv[])
{
	vector<string> testRoms = GetFilesInFolder("../PGOGames", { ".nes" });

	string homeFolder = "../PGOMesenHome";

	SetFlags(0x8000000000000000 | 0x20); //EmulationFlags::ConsoleMode | UseHdPacks
	InitializeEmu(homeFolder.c_str(), nullptr, nullptr, false, false, false);
	LoadROM(testRoms[0].c_str(), "");
	std::cout << "Running: " << testRoms[0] << std::endl;

	thread testThread([testRoms] {
		VideoFilterType filterTypes[13] = {
			VideoFilterType::BisqwitNtscQuarterRes, VideoFilterType::HQ2x, VideoFilterType::HQ3x, VideoFilterType::HQ4x, VideoFilterType::NTSC, VideoFilterType::Scale2x, VideoFilterType::Scale3x, VideoFilterType::Scale4x, VideoFilterType::xBRZ2x, VideoFilterType::xBRZ3x, VideoFilterType::xBRZ4x, VideoFilterType::xBRZ5x, VideoFilterType::xBRZ6x
		};

		for(size_t i = 1; i < testRoms.size(); i++) {
			std::this_thread::sleep_for(std::chrono::duration<int, std::milli>(5000));
			std::cout << "Running: " << testRoms[i] << std::endl;
			SetVideoFilter(filterTypes[i % 13]);
			LoadROM(testRoms[i].c_str(), "");
			DebugInitialize();
		}
		std::this_thread::sleep_for(std::chrono::duration<int, std::milli>(5000));
		Stop();
		Release();
	});
	Run();
	testThread.join();
	return 0;
}

