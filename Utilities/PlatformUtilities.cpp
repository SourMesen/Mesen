#include "stdafx.h"
#include "PlatformUtilities.h"

#ifdef WIN32
#include <Windows.h>
#endif

void PlatformUtilities::DisableScreensaver()
{
	//Prevent screensaver/etc from starting while using the emulator
	//DirectInput devices apparently do not always count as user input
	#ifdef WIN32
	SetThreadExecutionState(ES_SYSTEM_REQUIRED | ES_DISPLAY_REQUIRED | ES_CONTINUOUS);
	#endif // WIN32
}

void PlatformUtilities::EnableScreensaver()
{
	#ifdef WIN32
	SetThreadExecutionState(ES_CONTINUOUS);
	#endif // WIN32
}