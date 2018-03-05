---
title: Debugging Tools
weight: 5
chapter: false
toc: false
---

The debugging capabilities of Mesen are split across a number of different tools, including the debugger itself:

[Assembler](/debugging/assembler.html): Allows you to edit a game's code or run custom code on-the-fly.  
[Debugger](/debugging/debugger.html): View the code, add breakpoints, labels, watch values, and much more.   
[Memory Tools](/debugging/memorytools.html): Contains a hex editor and performance profiler.      
[PPU Viewer](/debugging/ppuviewer.html):  Displays information on the nametables, sprites, CHR banks and palette. Contains a CHR graphic editor and a palette editor.    
[Script Window](/debugging/scriptwindow.html):  Allows the execution of Lua scripts, which can communicate with the emulation via an API.   
[Trace Logger](/debugging/tracelogger.html):  View or log to a file the execution trace of the CPU and PPU.

Additionally, some other smaller features are available from the main debugger window.  e.g:

-  [Import labels from CA65/CC65 or ASM6](/debugging/debuggerintegration.html)  
-  Save any modification done to PRG/CHR ROM via the [CHR Viewer](/debugging/ppuviewer.html#chr-viewer), [Assembler](/debugging/assembler.html) or the [Memory Viewer](/debugging/memorytools.html#memory-viewer) as a new `.nes` file, or as an `.ips` patch file
-  Edit a rom's iNES header