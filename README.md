# Mesen

Mesen is a cross-platform NES/Famicom emulator for Windows & Linux built in C++ and C#.

If you want to support this project, please consider making a donation:

[![Donate](https://www.mesen.ca/images/donate.png)](https://www.mesen.ca/Donate.php)

[Website (https://www.mesen.ca)](https://www.mesen.ca)  
[Documentation (https://www.mesen.ca/docs)](https://www.mesen.ca/docs)

## Development Builds

Development builds of the latest commit are available from Appveyor. For stable release builds, see the **Releases** section below.

**Warning:** These are development builds and may be ***unstable***. Using them may also increase the chances of your settings being corrupted, or having issues when upgrading to the next official release. Additionally, these builds are currently not optimized via PGO and will typically run 20-30% slower than the official release builds.

Windows: [![Build status](https://ci.appveyor.com/api/projects/status/d4i7rqbfi386wdyw/branch/master?svg=true)](https://ci.appveyor.com/project/Sour/mesen/build/artifacts)

Linux: [![Build status](https://ci.appveyor.com/api/projects/status/uuoxwu7o5kkqjp4e/branch/master?svg=true)](https://ci.appveyor.com/project/Sour/mesen-nyf7v/build/artifacts)

## Releases

### Windows

The latest version is available on the [website](https://www.mesen.ca).  Older releases are available from the [releases tab on GitHub](https://github.com/SourMesen/Mesen/releases).

### Ubuntu

The official releases (same downloads as the Windows builds above) also contain the Linux version of Mesen, built under Ubuntu 16 - you should be able to use that in most cases if you are using Ubuntu.

The Linux version is a standard .NET executable file and requires Mono to run - you may need to configure your environment to allow it to automatically run .exe files through Mono, or manually run Mesen by using mono (e.g: "mono Mesen.exe").

The following packages need to be installed to run Mesen:

* mono-complete
* libsdl2-2.0
* gnome-themes-standard

**Note:** **Mono 5.18 or higher is recommended**, some older versions of Mono (e.g 4.2.2) have some stability and performance issues which can cause crashes and slow down the UI.
The default Mono version in Ubuntu 18.04 is 4.6.2 (which also causes some layout issues in Mesen).  To install the latest version of Mono, follow the instructions here: https://www.mono-project.com/download/stable/#download-lin

### Arch Linux

Packages are available here: <https://aur.archlinux.org/packages/mesen>

## Roadmap

Things that ***may or may not*** be added in the future, in no particular order:

* Support for more UNIF boards and more NES/Famicom input devices
* Shaders
* TAS editor

## Compiling

See [COMPILING.md](COMPILING.md)

## License

Mesen is available under the GPL V3 license.  Full text here: <http://www.gnu.org/licenses/gpl-3.0.en.html>

Copyright (C) 2014-2019 M. Bibaud

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
