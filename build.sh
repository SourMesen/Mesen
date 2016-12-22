#!/bin/sh
MESENPLATFORM=x64 make clean
MESENPLATFORM=x64 make core -j 4

MESENPLATFORM=x86 make clean
MESENPLATFORM=x86 make core -j 4

cp ./InteropDLL/obj.x64/libMesenCore.x64.dll ./bin
cp ./InteropDLL/obj.x86/libMesenCore.x86.dll ./bin
