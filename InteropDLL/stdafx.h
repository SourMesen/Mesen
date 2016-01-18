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

#ifdef ENVIRONMENT32
	#ifdef _DEBUG
		#define MESEN_LIBRARY_PATH "../bin/x86/Debug/"
		#define MESEN_LIBRARY_SUFFIX ".Debug.x86.lib"
	#else 
		#define MESEN_LIBRARY_PATH "../bin/x86/Release/"
		#define MESEN_LIBRARY_SUFFIX ".Release.x86.lib"
	#endif
#else
	#ifdef _DEBUG
		#define MESEN_LIBRARY_PATH "../bin/x64/Debug/"
		#define MESEN_LIBRARY_SUFFIX ".Debug.x64.lib"
	#else 
		#define MESEN_LIBRARY_PATH "../bin/x64/Release/"
		#define MESEN_LIBRARY_SUFFIX ".Release.x64.lib"
	#endif
#endif

#pragma comment(lib, MESEN_LIBRARY_PATH"Core.lib")
#pragma comment(lib, MESEN_LIBRARY_PATH"Utilities.lib")
#pragma comment(lib, MESEN_LIBRARY_PATH"Windows.lib")
#pragma comment(lib, "../Dependencies/DirectXTK" MESEN_LIBRARY_SUFFIX)

#define DllExport __declspec(dllexport)