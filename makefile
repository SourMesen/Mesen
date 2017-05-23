#Welcome to what must be the most terrible makefile ever (but hey, it works)
#Both clang & gcc work fine - clang seems to output faster code
#The only external dependency is SDL2 - everything else is pretty standard.
#Run "make" to build, "make run" to run
#To specify whether you want to build for x86 or x64:
#"MESENPLATFORM=x86 make" or "MESENPLATFORM=x64 make"
#Default is x64

CPPC=clang++
GCCOPTIONS=-fPIC -Wall --std=c++14 -O3

CC=clang
CCOPTIONS=-fPIC -Wall -O3

ifeq ($(MESENPLATFORM),x86)
	MESENPLATFORM=x86

	GCCOPTIONS += -m32
	CCOPTIONS += -m32
else
	MESENPLATFORM=x64
	GCCOPTIONS += -m64
	CCOPTIONS += -m64
endif

OBJFOLDER=obj.$(MESENPLATFORM)
SHAREDLIB=libMesenCore.$(MESENPLATFORM).dll
RELEASEFOLDER=bin/$(MESENPLATFORM)/Release

COREOBJ=$(patsubst Core/%.cpp,Core/$(OBJFOLDER)/%.o,$(wildcard Core/*.cpp))
UTILOBJ=$(patsubst Utilities/%.cpp,Utilities/$(OBJFOLDER)/%.o,$(wildcard Utilities/*.cpp)) $(patsubst Utilities/HQX/%.cpp,Utilities/$(OBJFOLDER)/%.o,$(wildcard Utilities/HQX/*.cpp)) $(patsubst Utilities/xBRZ/%.cpp,Utilities/$(OBJFOLDER)/%.o,$(wildcard Utilities/xBRZ/*.cpp)) $(patsubst Utilities/KreedSaiEagle/%.cpp,Utilities/$(OBJFOLDER)/%.o,$(wildcard Utilities/KreedSaiEagle/*.cpp)) $(patsubst Utilities/Scale2x/%.cpp,Utilities/$(OBJFOLDER)/%.o,$(wildcard Utilities/Scale2x/*.cpp))
LINUXOBJ=$(patsubst Linux/%.cpp,Linux/$(OBJFOLDER)/%.o,$(wildcard Linux/*.cpp)) 
LIBEVDEVOBJ=$(patsubst Linux/libevdev/%.c,Linux/$(OBJFOLDER)/%.o,$(wildcard Linux/libevdev/*.c))
SEVENZIPOBJ=$(patsubst SevenZip/%.c,SevenZip/$(OBJFOLDER)/%.o,$(wildcard SevenZip/*.c))


all: ui

ui: InteropDLL/$(OBJFOLDER)/$(SHAREDLIB)
	mkdir -p $(RELEASEFOLDER)/Dependencies
	rm -f $(RELEASEFOLDER)/Dependencies/*
	cd UpdateHelper && xbuild /property:Configuration="Release" /property:Platform="AnyCPU"
	cp "bin/Any CPU/Release/MesenUpdater.exe" $(RELEASEFOLDER)/Dependencies/
	cp GUI.NET/Dependencies/* $(RELEASEFOLDER)/Dependencies/
	cp InteropDLL/$(OBJFOLDER)/$(SHAREDLIB) $(RELEASEFOLDER)/Dependencies/$(SHAREDLIB)	
	cd $(RELEASEFOLDER)/Dependencies && zip ../Dependencies.zip *	
	cd GUI.NET && xbuild /property:Configuration="Release" /property:Platform="$(MESENPLATFORM)" /property:PreBuildEvent="" /property:DefineConstants="HIDETESTMENU;DISABLEAUTOUPDATE"

core: InteropDLL/$(OBJFOLDER)/$(SHAREDLIB)

runtests:
	cd TestHelper/$(OBJFOLDER) && ./testhelper

rungametests:
	cd TestHelper/$(OBJFOLDER) && ./testhelper ~/Mesen/TestGames

testhelper: InteropDLL/$(OBJFOLDER)/$(SHAREDLIB)
	mkdir -p TestHelper/$(OBJFOLDER)
	ar -rcs TestHelper/$(OBJFOLDER)/libSevenZip.a $(SEVENZIPOBJ)
	ar -rcs TestHelper/$(OBJFOLDER)/libMesenLinux.a $(LINUXOBJ) $(LIBEVDEVOBJ)
	ar -rcs TestHelper/$(OBJFOLDER)/libUtilities.a $(UTILOBJ)
	ar -rcs TestHelper/$(OBJFOLDER)/libCore.a $(COREOBJ)	
	cd TestHelper/$(OBJFOLDER) && $(CPPC) $(GCCOPTIONS) -Wl,-z,defs -Wno-parentheses -Wno-switch -o testhelper ../*.cpp ../../InteropDLL/ConsoleWrapper.cpp -L ./ -lCore -lMesenLinux -lUtilities -lSevenZip -pthread -lSDL2 -lstdc++fs

SevenZip/$(OBJFOLDER)/%.o: SevenZip/%.c
	mkdir -p SevenZip/$(OBJFOLDER) && cd SevenZip/$(OBJFOLDER) && $(CC) $(CCOPTIONS) -c $(patsubst SevenZip/%, ../%, $<)
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
	mkdir -p Core/$(OBJFOLDER) && cd Core/$(OBJFOLDER) && $(CPPC) $(GCCOPTIONS) -Wno-parentheses -Wno-switch -c $(patsubst Core/%, ../%, $<)
Linux/$(OBJFOLDER)/%.o: Linux/%.cpp
	mkdir -p Linux/$(OBJFOLDER) && cd Linux/$(OBJFOLDER) && $(CPPC) $(GCCOPTIONS) -Wno-parentheses -Wno-switch -c $(patsubst Linux/%, ../%, $<)
Linux/$(OBJFOLDER)/%.o: Linux/libevdev/%.c
	mkdir -p Linux/$(OBJFOLDER) && cd Linux/$(OBJFOLDER) && $(CC) $(CCOPTIONS) -Wno-parentheses -Wno-switch -c $(patsubst Linux/%, ../%, $<)

InteropDLL/$(OBJFOLDER)/$(SHAREDLIB): $(SEVENZIPOBJ) $(UTILOBJ) $(COREOBJ) $(LIBEVDEVOBJ) $(LINUXOBJ) InteropDLL/ConsoleWrapper.cpp InteropDLL/DebugWrapper.cpp
	mkdir -p InteropDLL/$(OBJFOLDER)
	ar -rcs InteropDLL/$(OBJFOLDER)/libSevenZip.a $(SEVENZIPOBJ)
	ar -rcs InteropDLL/$(OBJFOLDER)/libMesenLinux.a $(LINUXOBJ) $(LIBEVDEVOBJ)
	ar -rcs InteropDLL/$(OBJFOLDER)/libUtilities.a $(UTILOBJ)
	ar -rcs InteropDLL/$(OBJFOLDER)/libCore.a $(COREOBJ)
	cd InteropDLL/$(OBJFOLDER) && $(CPPC) $(GCCOPTIONS) -Wl,-z,defs -Wno-parentheses -Wno-switch -shared -o $(SHAREDLIB) ../*.cpp -L . -lMesenLinux -lCore -lUtilities -lSevenZip -pthread -lSDL2 -lstdc++fs

run:
	MONO_LOG_LEVEL=debug mono $(RELEASEFOLDER)/Mesen.exe

clean:
	rm SevenZip/$(OBJFOLDER) -r -f
	rm Utilities/$(OBJFOLDER) -r -f
	rm Core/$(OBJFOLDER) -r -f
	rm Linux/$(OBJFOLDER) -r -f
	rm InteropDLL/$(OBJFOLDER) -r -f
	rm TestHelper/$(OBJFOLDER) -r -f
	rm $(RELEASEFOLDER) -r -f
