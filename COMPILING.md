### Windows

#### *Standalone*

1) Open the solution in Visual Studio 2019
2) Set "GUI.NET" as the Startup Project
3) Compile as Release/x64 or Release/x86
4) Run Mesen.exe

Note: When loading the the solution in Visual Studio make sure all the projects are loaded successfully.
Note: If you get an error about the project targeted .NET Framework 4.5 chose to download the missing packages then install ".NET Framework 4.5 targeting pack" from the Visual Studio Installer.

#### *Libretro*

1) Open the solution in Visual Studio 2019
2) Compile as Libretro/x64 or Libretro/x86
3) Use the "mesen_libretro.dll" file in bin/(x64 or x86)/Libretro/mesen_libretro.dll

Note: It's also possible to build the Libretro core via MINGW by using the makefile in the Libretro subfolder.

### Linux

#### *Standalone*

To compile Mesen under Linux you will need clang 7.0+ or gcc 9.0+ (Mesen requires a C++17 compiler with support for the filesystem API.) Additionally, Mesen has the following dependencies:

* Mono 5.18+  (package: mono-devel) 
* SDL2  (package: libsdl2-dev)

**Note:** **Mono 5.18 or higher is recommended**, some older versions of Mono (e.g 4.2.2) have some stability and performance issues which can cause crashes and slow down the UI.
The default Mono version in Ubuntu 18.04 is 4.6.2 (which also causes some layout issues in Mesen).  To install the latest version of Mono, follow the instructions here: https://www.mono-project.com/download/stable/#download-lin

The makefile contains some more information at the top.  Running "make" will build the x64 version by default, and then "make run" should start the emulator.
LTO is supported under clang, which gives a large performance boost (25-30%+), so turning it on is highly recommended (see makefile for details).

#### *Libretro*

To compile the Libretro core you will need a version of clang/gcc that supports C++14.
Run "make" from the "Libretro" subfolder to build the Libretro core.
