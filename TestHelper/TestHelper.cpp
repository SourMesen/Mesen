#if _WIN64 || __x86_64__ || __ppc64__
	#define ENVIRONMENT64
#else
	#define ENVIRONMENT32
#endif

#ifdef ENVIRONMENT32
	#ifdef _DEBUG
		#define MESEN_LIBRARY_PATH "../bin/x86/Debug/"
	#else 
		#define MESEN_LIBRARY_PATH "../bin/x86/Release/"
	#endif
#else
	#ifdef _DEBUG
		#define MESEN_LIBRARY_PATH "../bin/x64/Debug/"
	#else 
		#define MESEN_LIBRARY_PATH "../bin/x64/Release/"
	#endif
#endif

#pragma comment(lib, MESEN_LIBRARY_PATH"Utilities.lib")

#include <Windows.h>
#include <Shlobj.h>
#include <iostream>
#include <thread>
#include <vector>
#include <string>
#include <thread>
#include "../Utilities/FolderUtilities.h"
#include "../Utilities/SimpleLock.h"
#include "../Utilities/Timer.h"
#include "../Core/MessageManager.h"

using namespace std;

typedef void (__stdcall *NotificationListenerCallback)(ConsoleNotificationType);

class InteropNotificationListener : public INotificationListener
{
	NotificationListenerCallback _callback;
public:
	InteropNotificationListener(NotificationListenerCallback callback)
	{
		_callback = callback;
	}

	void ProcessNotification(ConsoleNotificationType type, void* parameter)
	{
		_callback((ConsoleNotificationType)type);
	}
};

extern "C" {
	void __stdcall InitializeEmu(const char* homeFolder, void*, void*);
	int __stdcall RomTestRun(char* filename);
	void __stdcall LoadROM(char* filename);
	void __stdcall Run();
	void __stdcall Stop();
	INotificationListener* __stdcall RegisterNotificationCallback(NotificationListenerCallback callback);
}

std::thread *runThread = nullptr;
std::atomic<int> testIndex;
vector<string> testFilenames;
vector<string> failedTests;
SimpleLock lock;
Timer timer;

void _stdcall OnNotificationReceived(ConsoleNotificationType type)
{
	if(type == ConsoleNotificationType::GameLoaded) {
		runThread = new std::thread(Run);
	}
}

void RunEmu()
{
	Run();
}

void RunTest()
{
	while(true) {
		lock.Acquire();
		size_t index = testIndex++;
		lock.Release();

		if(index < testFilenames.size()) {
			string filepath = testFilenames[index];
			string filename = FolderUtilities::GetFilename(filepath, false);
			string command = "TestHelper.exe /testrom \"" + filepath + "\"";

			lock.Acquire();
			std::cout << std::to_string(index) << ") " << filename << std::endl;
			lock.Release();

			int failedFrames = std::system(command.c_str());
			if(failedFrames != 0) {
				//Test failed
				lock.Acquire();
				failedTests.push_back(filename);
				std::cout << "  ****  " << std::to_string(index) << ") " << filename << " failed (" << failedFrames << ")" << std::endl;
				lock.Release();
			}
		} else {
			break;
		}
	}
}

int main(int argc, char* argv[])
{
	wchar_t path[MAX_PATH];
	SHGetFolderPath(NULL, CSIDL_MYDOCUMENTS, NULL, SHGFP_TYPE_CURRENT, path);
	string mesenFolder = FolderUtilities::CombinePath(utf8::utf8::encode(path), "Mesen");

	if(argc <= 2) {
		string testFolder;
		if(argc == 1) {
			testFolder = FolderUtilities::CombinePath(mesenFolder, "Tests");
		} else {
			testFolder = argv[1];
		}

		vector<std::thread*> testThreads;
		testFilenames = FolderUtilities::GetFilesInFolder(testFolder, "*.mtp", true);
		testIndex = 0;

		timer.Reset();

		for(int i = 0; i < 4; i++) {
			std::thread *testThread = new std::thread(RunTest);
			testThreads.push_back(testThread);
		}

		for(int i = 0; i < 4; i++) {
			testThreads[i]->join();
			delete testThreads[i];
		}

		if(!failedTests.empty()) {
			std::cout << std::endl << std::endl;
			std::cout << "------------" << std::endl;
			std::cout << "Failed tests" << std::endl;
			std::cout << "------------" << std::endl;
			for(string failedTest : failedTests) {
				std::cout << failedTest << std::endl;
			}
		} else {
			std::cout << std::endl << std::endl << "All tests passed.";
		}
		std::cout << std::endl << std::endl << "Elapsed time: " << (timer.GetElapsedMS() / 1000) << " seconds";

		std::getchar();
	} else if(argc == 3) {
		char* testFilename = argv[2];
		RegisterNotificationCallback((NotificationListenerCallback)OnNotificationReceived);

		InitializeEmu(mesenFolder.c_str(), nullptr, nullptr);

		int result = RomTestRun(testFilename);
		if(runThread != nullptr) {
			runThread->join();
			delete runThread;
		}
		return result;
	}
	return 0;
}

