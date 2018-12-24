#pragma once

#include "stdafx.h"

#define DUMMYCPU
#define CPU DummyCpu
#include "CPU.h"
#include "CPU.cpp"
#undef CPU
#undef DUMMYCPU
