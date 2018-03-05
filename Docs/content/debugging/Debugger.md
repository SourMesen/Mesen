---
title: Debugger
weight: 10
pre: ""
chapter: false
---

<div class="imgBox"><div>
	<img style="zoom:0.75" src="/images/DebuggerWindow.png" />
	<span>The debugger window</span>
</div></div>

The debugger is the main part of the debugging tools available in Mesen. This window displays the disassembled code, allows you to configure breakpoints, labels and watch values.  It also contains a large number of options and smaller features -- all of which are described below.

## General Usage Tips ##

Generally speaking, the debugger tries to mimic the look and feel of Visual Studio in a lot of ways, including a lot of keyboard shortcuts (especially if you use the C# shortcut settings in Visual Studio).  If you are familiar with any version of VS, the debugger should be easy to use.   

Most elements in the debugger's interface have right-click menu options - make sure you explore the right-click options available in each list and window.

Watch expressions, breakpoints and labels are automatically saved on a per-rom basis in a **Workspace**.  
You can completely reset the workspace for a specific rom by using the **<kbd>File&rarr;Workspace&rarr;Reset Workspace</kbd>** command.

### Shortcuts ###

Use keyboard and mouse shortcuts to speed things up:

#### General ####

- <kbd>**F9**</kbd> to add/remove a breakpoint to the current line of code.
- <kbd>**Left-clicking**</kbd> on the left-most portion of the margin in the code window will set a breakpoint for that line.

#### Search and navigation ####

* <kbd>**Ctrl+F**</kbd>: To perform an incremental search in the code window.
* <kbd>**Ctrl-Shift-F**</kbd>: Find all occurrences of an address or label in the code window.
* <kbd>**Ctrl-G**</kbd>: Go to a specific address.
* <kbd>**Double-click**</kbd> on an address to navigate to it.
* <kbd>**Back/Forward**</kbd> mouse buttons (on a 5-button mouse) allow you to go back and forth in the navigation history.

  
## Code Window ##

<div class="imgBox"><div>
	<img src="/images/CodeWindow.png" />
	<span>Code Window</span>
</div></div>

The code window displays the disassembled code and contains a number of features and options.

### General information ###

* Several options control what is shown in the code window, and how it is shown - see [Display Options](#display-options) below.
* Labels and comments defined in the label list are shown in the code.
* Single-line comments appear on the left, multi-line comments appear on top.
* The instruction that's currently executing is highlighted in yellow.
* Mouse-hovering any label or address will display a tooltip giving additional information (label, comment, current value in memory, etc.)
* You can alter the flow of execution by using the `Set Next Statement` command to change the next instruction to be executed.

### Display Options ###

**Disassemble...**:  

* **Verified code only**: The strictest disassembly mode - this will cause the dissassembler to only disassemble bytes that it knows to be actual code. Everything else will be treated as data.
* **Everything**: In this mode, the disassembler will attempt to disassemble absolutely everything, including bytes that are marked as data.
* **Everything except verified data**: In this mode, the disassembler will disassemble everything *except* bytes that have been used/marked as data.

**Display OP codes in lower case**: When enabled, OP codes are displayed in lowercase letters

**Highlight Unexecuted Code**: When enabled, code that has been identified by the disassembler as code but hasn't been executed yet will be highlighted in green.

**Show Effective Addresses**: When enabled, the resulting address of indirect addressing modes (e.g `LDA $3321, Y`) will be displayed next to the instruction in blue. e.g: `@ $3323`

**Show Only Disassembled Code**: When enabled, everything that has not been disassembled (e.g because it has not been confirmed to be actual code, or because it is data) will be hidden from the code window.

**Byte Code Display**: When enabled, the byte code matching the instructions will be displayed next to them (either on the left, or on the line below based on your selection)

**PRG Address Display**: When enabled, the address matching the actual position of an instructions in PRG ROM will be displayed next to the CPU address (or replace it entirely, based on your selection)

## Emulation Status ##

<div class="imgBox"><div>
	<img src="/images/EmulationStatus.png" />
	<span>Emulation Status</span>
</div></div>

This section of the debugger window displays the CPU and PPU's current state.  
While execution is paused, most fields are editable. Altering the value of any field will change its corresponding value in the emulation core. To undo any edits you have done by mistake, click the "Undo Changes" button.

## Input Button Setup ##

<div class="imgBox"><div>
	<img src="/images/InputButtonSetup.png" />
	<span>Input Button Setup - P1's top and right buttons are pushed</span>
</div></div>

This section lets you force certain buttons to be held down on the NES' controller. This is often useful when trying to debug input-related code.  
Clicking on a button on the mini NES controllers will toggle its state - green buttons are currently being held down.

## Watch Window ##

<div class="imgBox"><div>
	<img src="/images/WatchList.png" />
	<span>Watch Window</span>
</div></div>

The watch window allows you to evaluate expression and see their value. Mesen supports complex expressions in C/C++ style syntax.

**To add a new watch expression**, click on the last empty line in the list and start typing.  
**To edit a watch expression**, double-click on it and start typing.    
**To switch between hex and decimal**, right-click in the watch and toggle the **Hexadecimal Display** option.

### Syntax ###

The used syntax is identical to C/C++ syntax (e.g && for and, || for or, etc.) and should have the same operator precedence as C/C++.

**Note:** Use the $ prefix to denote hexadecimal values.

**Special values**
```text
	A/X/Y/PS/SP: Value of corresponding registers
	PC: Program Counter
	OpPC: Address of the current instruction's first byte
	Irq/Nmi: True if the Irq/Nmi flags are set
	Cycle/Scanline: Current cycle (0-340)/scanline(-1 to 260) of the PPU
	Frame: PPU frame number (since power on/reset)
	Value: Current value being read/written from/to memory
	IsRead: True if the CPU is reading from a memory address
	IsWrite: True if the CPU is writing to a memory address
	Address: Current CPU memory address being read/written
	RomAddress: Current ROM address being read/written
	[<address>]: (Byte) Memory value at <address> (CPU)
	{<address>}: (Word) Memory value at <address> (CPU)
```
**Examples**
```
	[$10] //Displays the value of memory at address $10 (CPU)
	a == 10 || x == $23
	scanline == 10 && (cycle >= 55 && cycle <= 100)
	x == [$150] || y == [10]
	[[$15] + y]   //Reads the value at address $15, adds Y to it and reads the value at the resulting address.
	{$FFFA}  //Returns the NMI handler's address.
```

**Using labels**

Any label defined in the debugger can be used in watch expressions (their value will match the label's address in CPU memory).  
For example, if you have a label called "velocity" that points to 1-byte value at address $30 in the CPU's memory, you can display its value in the watch using the following syntax: `[velocity]`

## Breakpoints ##

<div class="imgBox"><div>
	<img src="/images/BreakpointList.png" />
	<span>Breakpoint List</span>
</div></div>

Breakpoints define conditions under which the execution of the game's code will be suspended.  Any number of breakpoints can be defined. To quickly add or remove a breakpoint for the current line of code, press F9 in the code window.

**To add a breakpoint**, right-click the breakpoint list and select **Add**.  
**To edit a breakpoint**, double-click on it in the list.  
**To disable a breakpoint**, uncheck it.  
**To delete a breakpoint**, right-click on it and select **Delete**  
**To view the breakpoint in the code**, right-click on it and select **Go to location** 

### Breakpoint configuration ###

<div class="imgBox"><div>
	<img src="/images/EditBreakpoint.png" />
	<span>Edit Breakpoint Window</span>
</div></div>

Breakpoints can be set to trigger based on CPU/PPU memory accesses at specific memory addresses. 

**Break On**  
Select whether this breakpoint should occur based on CPU or PPU accesses, and which types of accesses should trigger the breakpoint.

**Address**  
Select which address or address range this breakpoint should apply to.  
It is also possible to specify no address at all by selecting **Any** - in this case, breakpoints will be evaluated on every CPU cycle.  

**Use PRG ROM addresses**: This option makes it so the addresses apply to the PRG ROM itself, instead of their mappings in CPU memory.

**Condition** (optional)  
Conditions allow you to use the same expression syntax as the one used in the [Watch Window](#watch-window) to cause a breakpoint to trigger under very specific conditions.

**Examples**  
To break when the sum of the X and Y registers is 5:

    x + y == 5


To break when the value at memory address $10 is smaller or equal to $40:

	[$10] <= $40

To break when the CPU writes the value $60 to any address:

	IsWrite && Value == $60  
 
## Call Stack ##

<div class="imgBox"><div>
	<img src="/images/CallStack.png" />
	<span>Call Stack example with labels</span>
</div></div>

The call stack displays the currently executing function, and all functions that are below it on the stack. 
The top of the list is the current function, while the row below it is the location that the code will return to once the current function executes the RTS instruction. The call stack also displays NMI and IRQ routine handlers and processes calls to RTI in the same manner as calls to JSR and RTS.  

**Note:** Rows shown in gray and italic represent portions of the call stack that are currently not inside the CPU's memory (e.g because the PRG banks were changed since that point in the execution).  

**Labels:** When labels are defined for the PRG ROM offset matching the function's entry point, that label is shown as the function's name in the call stack.

**To view the code** at any location in the call stack, double-click on the row.  
  

## Labels ##

<div class="imgBox"><div>
	<img src="/images/LabelList.png" />
	<span>Label List</span>
</div></div>

Labels can be used to simplify debugging. They allow you to give names to variables and functions which will be used instead of numeric addresses when viewing the code in the debugger. Labels can also be used to display single and multi-line comments to the code.   
The label list displays every label defined in alphabetical order.

**To add a label**, right-click in the label list and select `Add`.  
**To edit a label**, right-click in the label list and select `Edit`.  
**To delete a label**, right-click in the label list and select `Delete`.  
**To find where a label is used**, right-click in the label list and select `Find Occurrences`.  
**To view the code** at the label's location, double-click on the label in the list.  

### Editing Labels ###

<div class="imgBox"><div>
	<img src="/images/EditLabel.png" />
	<span>Edit Label Window</span>
</div></div>

Various types of labels can be defined:

- **Internal RAM**: Used for labels residing in the $0000-$1FFF memory range (the NES' built-in RAM)
- **PRG ROM**: Used for constants, data, code labels and functions in PRG ROM - the address value represents the offset from the start of PRG ROM (which can exceed $FFFF)
- **Work RAM**: Used for variables in work ram (also called PRG RAM without battery backup) - the address value represents the offset from the start of the ram chip. 
- **Save RAM**: Used for variables in work ram (also called battery-backed PRG RAM) - the address value represents the offset from the start of the ram chip.
- **Register**: These are used to give name to built-in or mapper-specific registers.  For example, the $2000 PPU register could be renamed to "PpuControl". 
   
There are some restrictions on what a label can contain -- in general, they must begin with a letter or an underscore and cannot contain spaces or most non-alphanumeric characters.
Every type of label can also contain a comment.  Comments are shown in the code window as well as in the tooltips that are displayed when putting your cursor over a label in the code window. 

## Function List ##

<div class="imgBox"><div>
	<img src="/images/FunctionList.png" />
	<span>Function List</span>
</div></div>

The function list is similar to the label list and allows you to easily add or edit the name of functions (which are also labels).  

Unlike the label list, which only displays labels, the function list displays all known functions, including those with no labels. This is useful to quickly check an unnamed function's code (by double-clicking on it) and give it a name.  This can help when attempting to reverse-engineer code.

## CPU/PPU Memory Mappings ##

<div class="imgBox inline"><div>
	<img src="/images/CpuPpuMapping.png" />
	<span>CPU/PPU Memory Mappings</span>
</div></div>

The CPU and PPU memory mappings are visual aids that display information about the currently selected PRG/CHR banks and the nametable configuration.  
The banking configuration represents Mesen's internal representation of the mapper in use, which may not exactly match the mapper's specs.  
For example, a mapper with 2x 8kb + 1x 16kb PRG banking is emulated as 4x 8kb internally, so it will appear as 4 8kb banks.


## How To: Edit Code ##

<div class="imgBox"><div>
	<img src="/images/EditCodeExample.png" />
	<span>Using "Edit Selected Code" from the code window allows you to edit the code in the assembler</span>
</div></div>

From the code window, you can select code (via click and drag, or shift+arrow keys) and use the "Edit Selected Code" command to open the [Assembler](/debugging/assembler.html) and edit that section of code.
The assembler recognizes labels and allows you to define temporary labels as well. If the new code is smaller (in terms of bytes) than the original code, the extra bytes will be replaced by NOPs. If the new code is larger, it will override whatever comes next in the code -- a warning will be shown beforehand in this case.
When you're ready to apply your modifications, press the Apply button.

{{% notice tip %}}
If you want to save your code modifications to a .nes file, or as an IPS patch, you can use the **<kbd>File&rarr;Save</kbd>** or **<kbd>File&rarr;Save edits as IPS</kbd>** commands.
{{% /notice %}}
