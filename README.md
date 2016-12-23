Mesen is a cross-platform NES/Famicom emulator for Windows & Linux built in C++ and C#.

If you want to support this project, please consider making a donation:  
<a href="https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=W97QP2LYC9H4W"><img src="https://www.paypalobjects.com/en_US/i/btn/btn_donate_LG.gif" title="Donate to this project using Paypal" alt="Donate to this project using Paypal"/></a>

## Roadmap ##
Things that ***may or may not*** be added in the future, in no particular order:

-Support for more UNIF boards and more NES/Famicom input devices  
-Rewind functionality  
-Debugger improvements (APU state display, memory editor, scripting, etc.)  
-Shaders  
-Improvements to movie file format to support a few things that currently do not work  
-RAR file support  
-Libretro support  
-TAS editor  

## Compiling ##

### Windows ###
1) Open the solution in VS2015  
2) Compile as Release/x64 or Release/x86  
3) Run  

### Linux ###
You will need clang/gcc, Mono/XBuild and SDL2 to compile and run Mesen under Linux.  
The makefile contains some more information at the top.  Running "make" will build the x64 version by default, and then "make run" should start the emulator.  

## LICENSE ##

Mesen is available under the GPL V3 license.  Full text here: http://www.gnu.org/licenses/gpl-3.0.en.html

Copyright (C) 2016 M. Bibaud


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
