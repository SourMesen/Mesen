#pragma once

#include "stdafx.h"

extern "C" {
#if _WIN32 || _WIN64
#define DllExport2 __declspec(dllexport)
#else
#define DllExport2 __attribute__((visibility("default")))
#define __stdcall
#endif

	DllExport2 void __stdcall PgoRunTest(vector<string> testRoms, bool enableDebugger);
}
