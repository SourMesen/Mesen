// stdafx.h : include file for standard system include files,
// or project specific include files that are used frequently, but
// are changed infrequently
//

#pragma once

#include "targetver.h"

#if _WIN32 || _WIN64
	#if _WIN64
		#define ENVIRONMENT64
	#else
		#define ENVIRONMENT32
	#endif
#endif

#if __GNUC__
	#if __x86_64__ || __ppc64__
		#define ENVIRONMENT64
	#else
		#define ENVIRONMENT32
	#endif
#endif

#ifdef _DEBUG
	#define MESEN_LIBRARY_DEBUG_SUFFIX "Debug"
#else 
	#define MESEN_LIBRARY_DEBUG_SUFFIX "Release"
#endif

#ifdef ENVIRONMENT32
	#define MESEN_LIBRARY_SUFFIX "x86.lib"
#else 
	#define MESEN_LIBRARY_SUFFIX "x64.lib"
#endif

#pragma comment(lib, "Core.lib")
#pragma comment(lib, "Utilities.lib")
#pragma comment(lib, "Windows.lib")
#pragma comment(lib, "SevenZip.lib")
#ifdef PGO
	#pragma comment(lib, "../Dependencies/DirectXTK." MESEN_LIBRARY_DEBUG_SUFFIX ".Static." MESEN_LIBRARY_SUFFIX)
#else
	#pragma comment(lib, "../Dependencies/DirectXTK." MESEN_LIBRARY_DEBUG_SUFFIX "." MESEN_LIBRARY_SUFFIX)	
#endif

#define DllExport __declspec(dllexport)