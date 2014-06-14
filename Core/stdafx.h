// stdafx.h : include file for standard system include files,
// or project specific include files that are used frequently, but
// are changed infrequently
//

#pragma once

#include "targetver.h"

#include <stdio.h>
#include <tchar.h>

#include <stdint.h>
#include <memory>

#include <iostream>
#include <fstream>
#include <string>
#include <memory>
#include <thread>
#include <list>
#include <vector>

#include <windows.h>


class IMemoryHandler
{
	public:
		virtual uint8_t MemoryRead(uint16_t addr) = 0;
		virtual void MemoryWrite(uint16_t addr, uint8_t value) = 0;
};


// TODO: reference additional headers your program requires here
