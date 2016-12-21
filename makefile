#Welcome to what must be the most terrible makefile ever (but hey, it works)
#Both clang & gcc work fine - clang seems to output faster code
#The only external dependency is SDL2 - everything else is pretty standard.
#Run "make" to build, "make run" to run

COREOBJ=$(patsubst Core/%.cpp,Core/obj/%.o,$(wildcard Core/*.cpp))
UTILOBJ=$(patsubst Utilities/%.cpp,Utilities/obj/%.o,$(wildcard Utilities/*.cpp)) $(patsubst Utilities/HQX/%.cpp,Utilities/obj/%.o,$(wildcard Utilities/HQX/*.cpp)) $(patsubst Utilities/xBRZ/%.cpp,Utilities/obj/%.o,$(wildcard Utilities/xBRZ/*.cpp)) $(patsubst Utilities/KreedSaiEagle/%.cpp,Utilities/obj/%.o,$(wildcard Utilities/KreedSaiEagle/*.cpp)) $(patsubst Utilities/Scale2x/%.cpp,Utilities/obj/%.o,$(wildcard Utilities/Scale2x/*.cpp))
LINUXOBJ=$(patsubst Linux/%.cpp,Linux/obj/%.o,$(wildcard Linux/*.cpp)) 
LIBEVDEVOBJ=$(patsubst Linux/libevdev/%.c,Linux/obj/%.o,$(wildcard Linux/libevdev/*.c))
SEVENZIPOBJ=$(patsubst SevenZip/%.c,SevenZip/obj/%.o,$(wildcard SevenZip/*.c))

CPPC=clang++
GCCOPTIONS=-fPIC -Wall --std=c++14 -O3

CC=clang
CCOPTIONS=-fPIC -Wall -O3

SHAREDLIB=libMesenCore.dll
RELEASEFOLDER=bin/x64/Release

all: ui

ui: $(SHAREDLIB)
	mkdir -p $(RELEASEFOLDER)/Dependencies
	rm -f $(RELEASEFOLDER)/Dependencies/*		
	cp GUI.NET/Dependencies/* $(RELEASEFOLDER)/Dependencies/
	cp InteropDLL/obj/$(SHAREDLIB) $(RELEASEFOLDER)/Dependencies/libMesenCore.x64.dll	
	zip $(RELEASEFOLDER)/Dependencies.zip $(RELEASEFOLDER)/Dependencies/*
	cd GUI.NET && xbuild /property:Configuration="Release" /property:Platform="x64" /property:PreBuildEvent="" /property:DefineConstants="HIDETESTMENU"

runtests:
	cd TestHelper/obj && ./testhelper

rungametests:
	cd TestHelper/obj && ./testhelper ~/Mesen/TestGames

testhelper: $(SHAREDLIB)
	mkdir -p TestHelper/obj
	ar -rcs TestHelper/obj/libSevenZip.a $(SEVENZIPOBJ)
	ar -rcs TestHelper/obj/libMesenLinux.a $(LINUXOBJ) $(LIBEVDEVOBJ)
	ar -rcs TestHelper/obj/libUtilities.a $(UTILOBJ)
	ar -rcs TestHelper/obj/libCore.a $(COREOBJ)	
	cd TestHelper/obj && $(CPPC) $(GCCOPTIONS) -Wl,-z,defs -Wno-parentheses -Wno-switch -o testhelper ../*.cpp ../../InteropDLL/ConsoleWrapper.cpp -L ./ -lCore -lMesenLinux -lUtilities -lSevenZip -pthread -lSDL2 -lstdc++fs

SevenZip/obj/%.o: SevenZip/%.c
	mkdir -p SevenZip/obj && cd SevenZip/obj && $(CC) $(CCOPTIONS) -c $(patsubst SevenZip/%, ../%, $<)
Utilities/obj/%.o: Utilities/%.cpp
	mkdir -p Utilities/obj && cd Utilities/obj && $(CPPC) $(GCCOPTIONS) -c $(patsubst Utilities/%, ../%, $<)
Utilities/obj/%.o: Utilities/HQX/%.cpp
	mkdir -p Utilities/obj && cd Utilities/obj && $(CPPC) $(GCCOPTIONS) -c $(patsubst Utilities/%, ../%, $<)
Utilities/obj/%.o: Utilities/xBRZ/%.cpp
	mkdir -p Utilities/obj && cd Utilities/obj && $(CPPC) $(GCCOPTIONS) -c $(patsubst Utilities/%, ../%, $<)
Utilities/obj/%.o: Utilities/KreedSaiEagle/%.cpp
	mkdir -p Utilities/obj && cd Utilities/obj && $(CPPC) $(GCCOPTIONS) -c $(patsubst Utilities/%, ../%, $<)
Utilities/obj/%.o: Utilities/Scale2x/%.cpp
	mkdir -p Utilities/obj && cd Utilities/obj && $(CPPC) $(GCCOPTIONS) -c $(patsubst Utilities/%, ../%, $<)
Core/obj/%.o: Core/%.cpp
	mkdir -p Core/obj && cd Core/obj && $(CPPC) $(GCCOPTIONS) -Wno-parentheses -Wno-switch -c $(patsubst Core/%, ../%, $<)
Linux/obj/%.o: Linux/%.cpp
	mkdir -p Linux/obj && cd Linux/obj && $(CPPC) $(GCCOPTIONS) -Wno-parentheses -Wno-switch -c $(patsubst Linux/%, ../%, $<)
Linux/obj/%.o: Linux/libevdev/%.c
	mkdir -p Linux/obj && cd Linux/obj && $(CC) $(CCOPTIONS) -Wno-parentheses -Wno-switch -c $(patsubst Linux/%, ../%, $<)

$(SHAREDLIB): $(SEVENZIPOBJ) $(UTILOBJ) $(COREOBJ) $(LIBEVDEVOBJ) $(LINUXOBJ) InteropDLL/ConsoleWrapper.cpp InteropDLL/DebugWrapper.cpp
	mkdir -p InteropDLL/obj
	ar -rcs InteropDLL/obj/libSevenZip.a $(SEVENZIPOBJ)
	ar -rcs InteropDLL/obj/libMesenLinux.a $(LINUXOBJ) $(LIBEVDEVOBJ)
	ar -rcs InteropDLL/obj/libUtilities.a $(UTILOBJ)
	ar -rcs InteropDLL/obj/libCore.a $(COREOBJ)
	cd InteropDLL/obj && $(CPPC) $(GCCOPTIONS) -Wl,-z,defs -Wno-parentheses -Wno-switch -shared -o $(SHAREDLIB) ../*.cpp -L . -lCore -lMesenLinux -lUtilities -lSevenZip -pthread -lSDL2 -lstdc++fs

run:
	MONO_LOG_LEVEL=debug mono bin/x64/Release/Mesen.exe

clean:
	rm SevenZip/obj -r -f
	rm Utilities/obj -r -f
	rm Core/obj -r -f
	rm Linux/obj -r -f
	rm InteropDLL/obj -r -f
	rm TestHelper/obj -r -f
