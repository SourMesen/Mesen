// stdafx.h : include file for standard system include files,
// or project specific include files that are used frequently, but
// are changed infrequently
//

#pragma once

#include "targetver.h"

#define WIN32_LEAN_AND_MEAN             // Exclude rarely-used stuff from Windows headers
// Windows Header Files:
#include <windows.h>
#include <tchar.h>

#ifdef _DEBUG
	#pragma comment(lib, "../Debug/Core.lib")
	#pragma comment(lib, "../Debug/Utilities.lib")
	#pragma comment(lib, "../Debug/Windows.lib")
	#pragma comment(lib, "../Windows/DirectXTK/DirectXTK.debug.lib")
	#pragma comment(lib, "../Core/Nes_Apu/Nes_Apu.debug.lib")
#else 
	#pragma comment(lib, "../Release/Core.lib")
	#pragma comment(lib, "../Release/Utilities.lib")
	#pragma comment(lib, "../Release/Windows.lib")
	#pragma comment(lib, "../Windows/DirectXTK/DirectXTK.lib")
	#pragma comment(lib, "../Core/Nes_Apu/Nes_Apu.lib")
#endif

#define DllExport __declspec(dllexport)

// TODO: reference additional headers your program requires here
