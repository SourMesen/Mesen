---
title: Integration with compilers
weight: 40
pre: ""
chapter: false
---

When building homebrew software in assembly or C, it is possible to export the labels used in your code and import them into Mesen to simplify the debugging process.  This allows the debugger to know which portions of the ROM correspond to which functions in your code, as well as display your code's comments inside the debugger itself.

## Integration with compilers ##

### CC65 / CA65 ###

CC65/CA65 are able to produce .DBG files which can be imported into Mesen's debugger.  
To make CC65/CA65 create a .DBG file during the compilation, use the `--dbgfile` command line option.

To import the .DBG file, use the **<kbd>File&rarr;Workspace&rarr;Import Labels</kbd>** command in the debugger window.  

You can also enable the `Auto-load DBG/MLB files` to make Mesen load any .DBG file it finds next to the ROM whenever the debugger is opened or the power cycle button is used.  
**Note:** For this option to work, the ROM file must have the same name as the DBG file (e.g `MyRom.nes` and `MyRom.dbg`) and be inside the same folder.

#### Source View ####

<div class="imgBox"><div>
	<img src="/images/SourceView.png" />
	<span>Source View</span>
</div></div>

When a .DBG file is loaded, 2 additional options appear in the code window's right-click menu:

* **Switch to Source View**: This turns on `Source View` mode, which allows you to debug the game using the original code files, rather than the disassembly.  This can be used for both assembly and C projects.
* **Show source code as comments**: When enabled, the debugger will show the original source code to the right of the disassembly, as comments.


### ASM6f ###

Integration with ASM6 is possible by using freem's branch of ASM6 named [ASM6f](https://github.com/freem/asm6f).  
This fork contains 2 additional command line options that are useful when using Mesen: `-m` and `-c`

`-m` produces a .mlb (Mesen labels) file that can be imported manually using the **<kbd>File&rarr;Workspace&rarr;Import Labels</kbd>** command.  
`-c` produces a .cdl (Code Data Logger) file which can be imported manually using the **<kbd>Tools&rarr;Code/Data Logger&rarr;Load CDL file</kbd>** command.   

Additionally, you can use the `Auto-load DBG/MLB files` and `Auto-load CDL files` options in the **<kbd>File&rarr;Workspace</kbd>** menu to automatically load MLB and CDL files present in the same folder as the current ROM, with the same filename (e.g `MyRom.nes`, `MyRom.mlb`, `MyRom.cdl`). 


### NESASM ###

Mesen can also import the `.fns` symbol files that NESASM produces. However, due to limitations in the `.fns` format, these labels can only be reliably imported for games containing exactly 32kB of PRG ROM.  If you are creating a larger game, CC65/CA65 offers the best integration features at the moment.

## Importing and exporting labels ##

<div class="imgBox"><div>
	<img src="/images/WorkspaceMenu.png" />
	<span>Workspace Menu</span>
</div></div>

Mesen can also import and export labels in `.mlb` format. This is the same format as the label files produced by ASM6f. The ability to import labels can be used to integrate the debugger with your own workflow (e.g by creating your own scripts that produce `.mlb` files)

<div style="clear:both"></div>

### Mesen Label Files (.mlb) ###

The `.mlb` files used by Mesen to import/export labels is a simple text format.  For example, this defines a label and comment on byte $100 of PRG ROM:
```
P:100:MyLabel:This is a comment
```
The format also supports multi-byte labels, defined by giving specifying an address range:
```
R:200-2FF:ShadowOam
```

The first letter on each row is used to specify the label's type:
```
P: PRG ROM labels
R: RAM labels (for the NES' internal 2kb RAM)
S: Save RAM labels
W: Work RAM labels
G: Register labels (these are used to define PPU/APU/Mapper registers, etc.)
```

### Import Settings ###

<div class="imgBox"><div>
	<img src="/images/ImportSettings.png" />
	<span>Import Settings</span>
</div></div>

For fine-grain control over the DBG/MLB file imports, the `Import Settings` window can be used.  
This allows you to configure which types of labels/comments should be imported, as well as choosing whether or not Mesen should delete all existing labels before importing DBG/MLB files.