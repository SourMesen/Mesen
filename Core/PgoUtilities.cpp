#include "stdafx.h"
#include <thread>
#include "PgoUtilities.h"
#include "Types.h"
#include "Debugger.h"
#include "DebuggerTypes.h"
#include "Console.h"
#include "../Utilities/FolderUtilities.h"

extern "C" {
	void __stdcall PgoRunTest(vector<string> testRoms, bool enableDebugger)
	{
		const VideoFilterType filterTypes[13] = { VideoFilterType::BisqwitNtscQuarterRes, VideoFilterType::HQ2x, VideoFilterType::HQ3x, VideoFilterType::HQ4x, VideoFilterType::NTSC, VideoFilterType::Scale2x, VideoFilterType::Scale3x, VideoFilterType::Scale4x, VideoFilterType::xBRZ2x, VideoFilterType::xBRZ3x, VideoFilterType::xBRZ4x, VideoFilterType::xBRZ5x, VideoFilterType::xBRZ6x };
		FolderUtilities::SetHomeFolder("../PGOMesenHome");

		for(size_t i = 0; i < testRoms.size(); i++) {
			std::cout << "Running: " << testRoms[i] << std::endl;

			shared_ptr<Console> console(new Console());
			console->Init();
			console->Initialize(testRoms[i]);
			console->GetSettings()->SetFlags(EmulationFlags::ConsoleMode | EmulationFlags::UseHdPacks);
			console->GetSettings()->SetVideoFilterType(filterTypes[i % 13]);

			if(enableDebugger) {
				console->GetDebugger(true)->SetFlags((uint32_t)DebuggerFlags::BreakOnFirstCycle);
			}

			thread testThread([&console] {
				console->Run();
			});
			std::this_thread::sleep_for(std::chrono::duration<int, std::milli>(5000));
			console->Stop();
			testThread.join();
			console->Release(true);
		}
	}
}