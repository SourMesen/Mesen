#include "stdafx.h"
#include "PlatformUtilities.h"

#if !defined(LIBRETRO) && defined(_WIN32)
#include <Windows.h>
#endif

void PlatformUtilities::DisableScreensaver()
{
	//Prevent screensaver/etc from starting while using the emulator
	//DirectInput devices apparently do not always count as user input
	#if !defined(LIBRETRO) && defined(_WIN32)
	SetThreadExecutionState(ES_SYSTEM_REQUIRED | ES_DISPLAY_REQUIRED | ES_CONTINUOUS);
	#endif
}

void PlatformUtilities::EnableScreensaver()
{
	#if !defined(LIBRETRO) && defined(_WIN32)
	SetThreadExecutionState(ES_CONTINUOUS);
	#endif
}