#!/bin/sh

#This is a build script used for official releases which does not build the UI
#Read the makefile and use "make" to build Mesen normally

if [ "$1" = libretro ]; then
	MESENPLATFORM=x64 make clean
	LTO=true MESENPLATFORM=x64 make libretro -j 16

	MESENPLATFORM=x86 make clean
	LTO=true MESENPLATFORM=x86 make libretro -j 16
else
	MESENPLATFORM=x64 make clean
	LTO=true MESENPLATFORM=x64 make core -j 16

	MESENPLATFORM=x86 make clean
	LTO=true MESENPLATFORM=x86 make core -j 16

	cp ./InteropDLL/obj.x64/libMesenCore.x64.dll ./bin
	cp ./InteropDLL/obj.x86/libMesenCore.x86.dll ./bin
fi
