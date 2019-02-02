### Windows

#### *Standalone*

1) Open the solution in VS2017
2) Compile as Release/x64 or Release/x86
3) Run

#### *Libretro*

1) Open the solution in VS2017
2) Compile as Libretro/x64 or Libretro/x86
3) Use the "mesen_libretro.dll" file in bin/(x64 or x86)/Libretro/mesen_libretro.dll

### Linux

#### *Standalone*

To compile Mesen under Linux you will need a recent version of clang/gcc.  This is because Mesen requires a C++14 compiler, along with support for the filesystem API (C++17). Additionally, Mesen has the following dependencies:

* Mono 5.18+  (package: mono-devel) 
* SDL2  (package: libsdl2-dev)

**Note:** **Mono 5.18 or higher is recommended**, some older versions of Mono (e.g 4.2.2) have some stability and performance issues which can cause crashes and slow down the UI.
The default Mono version in Ubuntu 18.04 is 4.6.2 (which also causes some layout issues in Mesen).  To install the latest version of Mono, follow the instructions here: https://www.mono-project.com/download/stable/#download-lin

The makefile contains some more information at the top.  Running "make" will build the x64 version by default, and then "make run" should start the emulator.
LTO is supported under clang, which gives a large performance boost (25-30%+), so turning it on is highly recommended (see makefile for details).

#### *Libretro*

To compile the libretro core you will need a recent version of clang/gcc.  This is because Mesen requires a C++14 compiler, along with support for the filesystem API (C++17).

Running "make libretro" will build the core and put it in "bin/mesen_libretro.(x64 or x86).so".
LTO is supported under clang, which gives a large performance boost (25-30%+), so turning it on is highly recommended (see makefile for details).

**Note:** There is also another makefile in the Libretro folder - this is used by the RetroArch's buildbot to build the core.  You can also try using this makefile if you are having issues with the one in the root folder.
