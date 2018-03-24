#Welcome to what must be the most terrible makefile ever (but hey, it works)
#Both clang & gcc work fine - clang seems to output faster code
#The only external dependency is SDL2 - everything else is pretty standard.
#Run "make" to build, "make run" to run

#----------------------
#Platform Configuration
#----------------------
#To specify whether you want to build for x86 or x64:
#"MESENPLATFORM=x86 make" or "MESENPLATFORM=x64 make"
#Default is x64

#-----------------------
# Link Time Optimization
#-----------------------
#LTO is supported for clang and gcc (but only seems to help for clang?)
#LTO gives a 25-30% performance boost, so use it whenever you can
#Usage: LTO=true make

MESENFLAGS=
libretro : MESENFLAGS=-D LIBRETRO

CPPC=clang++
GCCOPTIONS=-fPIC -Wall --std=c++14 -O3 $(MESENFLAGS) -Wno-parentheses -Wno-switch

CC=clang
CCOPTIONS=-fPIC -Wall -O3 $(MESENFLAGS)

ifeq ($(MESENPLATFORM),x86)
	MESENPLATFORM=x86

	GCCOPTIONS += -m32
	CCOPTIONS += -m32
else
	MESENPLATFORM=x64
	GCCOPTIONS += -m64
	CCOPTIONS += -m64
endif

ifeq ($(LTO),true)
	CCOPTIONS += -flto
	GCCOPTIONS += -flto
endif

OBJFOLDER=obj.$(MESENPLATFORM)
SHAREDLIB=libMesenCore.$(MESENPLATFORM).dll
LIBRETROLIB=mesen_libretro.$(MESENPLATFORM).so
RELEASEFOLDER=bin/$(MESENPLATFORM)/Release

COREOBJ=$(patsubst Core/%.cpp,Core/$(OBJFOLDER)/%.o,$(wildcard Core/*.cpp))
UTILOBJ=$(patsubst Utilities/%.cpp,Utilities/$(OBJFOLDER)/%.o,$(wildcard Utilities/*.cpp)) $(patsubst Utilities/HQX/%.cpp,Utilities/$(OBJFOLDER)/%.o,$(wildcard Utilities/HQX/*.cpp)) $(patsubst Utilities/xBRZ/%.cpp,Utilities/$(OBJFOLDER)/%.o,$(wildcard Utilities/xBRZ/*.cpp)) $(patsubst Utilities/KreedSaiEagle/%.cpp,Utilities/$(OBJFOLDER)/%.o,$(wildcard Utilities/KreedSaiEagle/*.cpp)) $(patsubst Utilities/Scale2x/%.cpp,Utilities/$(OBJFOLDER)/%.o,$(wildcard Utilities/Scale2x/*.cpp))
LINUXOBJ=$(patsubst Linux/%.cpp,Linux/$(OBJFOLDER)/%.o,$(wildcard Linux/*.cpp)) 
LIBEVDEVOBJ=$(patsubst Linux/libevdev/%.c,Linux/$(OBJFOLDER)/%.o,$(wildcard Linux/libevdev/*.c))
SEVENZIPOBJ=$(patsubst SevenZip/%.c,SevenZip/$(OBJFOLDER)/%.o,$(wildcard SevenZip/*.c))
LUAOBJ=$(patsubst Lua/%.c,Lua/$(OBJFOLDER)/%.o,$(wildcard Lua/*.c))


all: ui

ui: InteropDLL/$(OBJFOLDER)/$(SHAREDLIB)
	mkdir -p $(RELEASEFOLDER)/Dependencies
	rm -f $(RELEASEFOLDER)/Dependencies/*
	cd UpdateHelper && xbuild /property:Configuration="Release" /property:Platform="AnyCPU"
	cp "bin/Any CPU/Release/MesenUpdater.exe" $(RELEASEFOLDER)/Dependencies/
	cp GUI.NET/Dependencies/* $(RELEASEFOLDER)/Dependencies/
	cp InteropDLL/$(OBJFOLDER)/$(SHAREDLIB) $(RELEASEFOLDER)/Dependencies/$(SHAREDLIB)	
	cd $(RELEASEFOLDER)/Dependencies && zip ../Dependencies.zip *	
	cd GUI.NET && xbuild /property:Configuration="Release" /property:Platform="$(MESENPLATFORM)" /property:PreBuildEvent="" /property:DefineConstants="HIDETESTMENU,DISABLEAUTOUPDATE"

libretro: Libretro/$(OBJFOLDER)/$(LIBRETROLIB)
	cp ./Libretro/$(OBJFOLDER)/$(LIBRETROLIB) ./bin

core: InteropDLL/$(OBJFOLDER)/$(SHAREDLIB)

runtests:
	cd TestHelper/$(OBJFOLDER) && ./testhelper

rungametests:
	cd TestHelper/$(OBJFOLDER) && ./testhelper ~/Mesen/TestGames

testhelper: InteropDLL/$(OBJFOLDER)/$(SHAREDLIB)
	mkdir -p TestHelper/$(OBJFOLDER)
	$(CPPC) $(GCCOPTIONS) -Wl,-z,defs -o testhelper TestHelper/*.cpp InteropDLL/ConsoleWrapper.cpp $(SEVENZIPOBJ) $(LUAOBJ) $(LINUXOBJ) $(LIBEVDEVOBJ) $(UTILOBJ) $(COREOBJ) -pthread -lSDL2 -lstdc++fs
	mv testhelper TestHelper/$(OBJFOLDER)

SevenZip/$(OBJFOLDER)/%.o: SevenZip/%.c
	mkdir -p SevenZip/$(OBJFOLDER) && cd SevenZip/$(OBJFOLDER) && $(CC) $(CCOPTIONS) -c $(patsubst SevenZip/%, ../%, $<)
Lua/$(OBJFOLDER)/%.o: Lua/%.c
	mkdir -p Lua/$(OBJFOLDER) && cd Lua/$(OBJFOLDER) && $(CC) $(CCOPTIONS) -c $(patsubst Lua/%, ../%, $<)	
Utilities/$(OBJFOLDER)/%.o: Utilities/%.cpp
	mkdir -p Utilities/$(OBJFOLDER) && cd Utilities/$(OBJFOLDER) && $(CPPC) $(GCCOPTIONS) -c $(patsubst Utilities/%, ../%, $<)
Utilities/$(OBJFOLDER)/%.o: Utilities/HQX/%.cpp
	mkdir -p Utilities/$(OBJFOLDER) && cd Utilities/$(OBJFOLDER) && $(CPPC) $(GCCOPTIONS) -c $(patsubst Utilities/%, ../%, $<)
Utilities/$(OBJFOLDER)/%.o: Utilities/xBRZ/%.cpp
	mkdir -p Utilities/$(OBJFOLDER) && cd Utilities/$(OBJFOLDER) && $(CPPC) $(GCCOPTIONS) -c $(patsubst Utilities/%, ../%, $<)
Utilities/$(OBJFOLDER)/%.o: Utilities/KreedSaiEagle/%.cpp
	mkdir -p Utilities/$(OBJFOLDER) && cd Utilities/$(OBJFOLDER) && $(CPPC) $(GCCOPTIONS) -c $(patsubst Utilities/%, ../%, $<)
Utilities/$(OBJFOLDER)/%.o: Utilities/Scale2x/%.cpp
	mkdir -p Utilities/$(OBJFOLDER) && cd Utilities/$(OBJFOLDER) && $(CPPC) $(GCCOPTIONS) -c $(patsubst Utilities/%, ../%, $<)
Core/$(OBJFOLDER)/%.o: Core/%.cpp
	mkdir -p Core/$(OBJFOLDER) && cd Core/$(OBJFOLDER) && $(CPPC) $(GCCOPTIONS)  -c $(patsubst Core/%, ../%, $<)
Linux/$(OBJFOLDER)/%.o: Linux/%.cpp
	mkdir -p Linux/$(OBJFOLDER) && cd Linux/$(OBJFOLDER) && $(CPPC) $(GCCOPTIONS) -c $(patsubst Linux/%, ../%, $<)
Linux/$(OBJFOLDER)/%.o: Linux/libevdev/%.c
	mkdir -p Linux/$(OBJFOLDER) && cd Linux/$(OBJFOLDER) && $(CC) $(CCOPTIONS) -c $(patsubst Linux/%, ../%, $<)

InteropDLL/$(OBJFOLDER)/$(SHAREDLIB): $(SEVENZIPOBJ) $(LUAOBJ) $(UTILOBJ) $(COREOBJ) $(LIBEVDEVOBJ) $(LINUXOBJ) InteropDLL/ConsoleWrapper.cpp InteropDLL/DebugWrapper.cpp
	mkdir -p InteropDLL/$(OBJFOLDER)
	$(CPPC) $(GCCOPTIONS) -Wl,-z,defs -shared -o $(SHAREDLIB) InteropDLL/*.cpp $(SEVENZIPOBJ) $(LUAOBJ) $(LINUXOBJ) $(LIBEVDEVOBJ) $(UTILOBJ) $(COREOBJ) -pthread -lSDL2 -lstdc++fs
	mv $(SHAREDLIB) InteropDLL/$(OBJFOLDER)


Libretro/$(OBJFOLDER)/$(LIBRETROLIB): $(SEVENZIPOBJ) $(UTILOBJ) $(COREOBJ) $(LUAOBJ) Libretro/libretro.cpp
	mkdir -p Libretro/$(OBJFOLDER)
	$(CPPC) $(GCCOPTIONS) -Wl,-z,defs -shared -o $(LIBRETROLIB) Libretro/*.cpp $(SEVENZIPOBJ) $(UTILOBJ) $(COREOBJ) $(LUAOBJ) -pthread
	mv $(LIBRETROLIB) Libretro/$(OBJFOLDER) 

debug:
	MONO_LOG_LEVEL=debug mono $(RELEASEFOLDER)/Mesen.exe

run:
	mono $(RELEASEFOLDER)/Mesen.exe

clean:
	rm Lua/$(OBJFOLDER) -r -f
	rm SevenZip/$(OBJFOLDER) -r -f
	rm Utilities/$(OBJFOLDER) -r -f
	rm Core/$(OBJFOLDER) -r -f
	rm Linux/$(OBJFOLDER) -r -f
	rm InteropDLL/$(OBJFOLDER) -r -f
	rm Libretro/$(OBJFOLDER) -r -f
	rm TestHelper/$(OBJFOLDER) -r -f
	rm $(RELEASEFOLDER) -r -f
