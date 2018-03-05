---
title: Integration with CC65/ASM6
weight: 18
pre: ""
chapter: false
---

When building homebrew software in assembly or C, it is possible to export the labels used in your code and import them into Mesen to simplify the debugging process.  This allows the debugger to know which portions of the ROM correspond to which functions in your code, as well as display your code's comments inside the debugger itself.

## CC65 / CA65 ##

CC65/CA65 are able to produce .DBG files which can be imported into Mesen's debugger.  
To make CC65/CA65 create a .DBG file during the compilation, use the `--dbgfile` command line option.

To import the .DBG file, use the **<kbd>File&rarr;Workspace&rarr;Import Labels</kbd>** command in the debugger window.  

You can also enable the `Auto-load DBG/MLB files` to make Mesen load any .DBG file it finds next to the ROM whenever the debugger is opened or the power cycle button is used.  
**Note:** For this option to work, the ROM file must have the same name as the DBG file (e.g `MyRom.nes` and `MyRom.dbg`) and be inside the same folder.


## ASM6f ##

Integration with ASM6 is possible by using freem's branch of ASM6 named [ASM6f](https://github.com/freem/asm6f).  
This fork contains 2 additional command line options that are useful when using Mesen: `-m` and `-c`

`-m` produces a .mlb (Mesen labels) file that can be imported manually using the **<kbd>File&rarr;Workspace&rarr;Import Labels</kbd>** command.  
`-c` produces a .cdl (Code Data Logger) file which can be imported manually using the **<kbd>Tools&rarr;Code/Data Logger&rarr;Load CDL file</kbd>** command.   

Additionally, you can use the `Auto-load DBG/MLB files` and `Auto-load CDL files` options in the **<kbd>File&rarr;Workspace</kbd>** menu to automatically load MLB and CDL files present in the same folder as the current ROM, with the same filename (e.g `MyRom.nes`, `MyRom.mlb`, `MyRom.cdl`). 
