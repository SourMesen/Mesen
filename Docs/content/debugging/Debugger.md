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

Most elements in the debugger's interface have right-click menu options - make sure you explore the right-click options available in each list and window.

Watch expressions, breakpoints and labels are automatically saved on a per-rom basis in a **Workspace**.  
You can completely reset the workspace for a specific rom by using the **<kbd>File&rarr;Workspace&rarr;Reset Workspace</kbd>** command.


## Customizing the debugger ##

All keyboard shortcuts can be customized in **<kbd>Options&rarr;Customize Shortcut Keys</kbd>**  

The font used in the code window can be customized in **<kbd>Options&rarr;Font Options&rarr;Select Font</kbd>**  
The colors used for syntax highlighting can be changed in **<kbd>Options&rarr;Configure Colors</kbd>**

Various portions of the debugger can be hidden/shown via **<kbd>Options&rarr;Show</kbd>**
  
## Code Window ##

<div class="imgBox"><div>
	<img src="/images/CodeWindow.png" />
	<span>Code Window</span>
</div></div>

The code window displays the disassembled code and contains a number of features and options.

### General information ###

* Several options control what is shown in the code window, and how it is shown - see [Display Options](#display-options) below.
* Labels and comments defined in the label list are shown in the code.
* Single-line comments appear on the right, multi-line comments appear on top.
* The instruction that's currently executing is highlighted in yellow.
* Mouse-hovering any label or address will display a tooltip giving additional information (label, comment, current value in memory, etc.)
* You can alter the flow of execution by using the `Set Next Statement` command to change the next instruction to be executed.

### Disassembly Options ###

**Disassemble...**:  

* **Verified Code**: (Always enabled) Bytes that are known by the debugger to be valid code will be disassembled as code.
* **Verified Data**: Bytes that are known to be data will be disassembled as code (enabling this is not recommended).
* **Unidentified Code/Data**: Bytes that have not been used yet by the CPU will be disassembled as code.

**Show...**:

* **Disassembled Code**: (Always enabled) Any portions of the CPU memory that has been disassembled (based on the previous options) will be shown in the code window.
* **Verified Data**: Verified data blocks will be shown (every byte of the block will be shown in the code window).  Note: this has no effect if verified code is disassembled based on the previous options.
* **Unidentified Code/Data**: Blocks of bytes that are not marked as data nor code will be shown (every byte of the block will be shown in the code window).  Note: this has no effect if unidentified blocks are disassembled based on the previous options.

**Display OP codes in lower case**: When enabled, OP codes are displayed in lowercase letters

**Show Effective Addresses**: When enabled, the resulting address of indirect addressing modes (e.g `LDA $3321, Y`) will be displayed next to the instruction in blue. e.g: `@ $3323`

**Show Memory Values**: When enabled, every line of code that refers to a specific address in memory will display that address' current value on the right. 

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

**Displaying arrays**

The watch window allows you display several consecutive memory values on the same row using a special syntax. e.g:
```
[$30, 16]      //This will display the values of addresses $30 to $3F
[MyLabel, 10]  //This syntax can also be used with labels
```

## Breakpoints ##

<div class="imgBox"><div>
	<img src="/images/BreakpointList.png" />
	<span>Breakpoint List</span>
</div></div>

Breakpoints define conditions under which the execution of the game's code will be suspended.  Any number of breakpoints can be defined.
You can also make a breakpoint appear as a mark on the [Event Viewer](/debugging/eventviewer.html) by checking the `M` column.

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

**Breakpoint Type**  
Select the type of memory for which you want to set a breakpoint.  The valid range of addresses for the breakpoint will vary based on the selected memory type.

**Break On**  
Select which types of accesses (read, write or execute) should trigger the breakpoint.

**Address**  
Select which address or address range this breakpoint should apply to.  
It is also possible to specify no address at all by selecting **Any** - in this case, breakpoints will be evaluated on every CPU cycle.  

**Condition** (optional)  
Conditions allow you to use the same expression syntax as the one used in the [Watch Window](#watch-window) to cause a breakpoint to trigger under very specific conditions.

**Mark on Event Viewer**  
When enabled, a mark will be visible on the [Event Viewer](/debugging/eventviewer.html) whenever this breakpoint's conditions are met. This can be used to add marks to the event viewer based on a variety of conditions by using conditional breakpoints.

**Break Execution**  
This enables the breakpoint - if this is disabled, the execution will not break when the breakpoint is hit.

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
**To add a breakpoint to a label**, right-click in the label list and select `Add breakpoint`.  
**To add the label to the watch**, right-click in the label list and select `Add to watch`.  
**To find where a label is used**, right-click in the label list and select `Find Occurrences`.  
**To view the code** at the label's location, double-click on the label in the list.  

**Note:** Labels shown in gray color and italic font are currently not mapped in CPU memory.

### Editing Labels ###

<div class="imgBox"><div>
	<img src="/images/EditLabel.png" />
	<span>Edit Label Window</span>
</div></div>

Various types of labels can be defined:

- **NES RAM (2 KB)**: Used for labels residing in the $0000-$1FFF memory range (the NES' built-in RAM)
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
For example, a mapper with 2x 8 KB + 1x 16 KB PRG banking is emulated as 4x 8 KB internally, so it will appear as four 8 KB banks.

## Other Options ##

### Display Options ###

The `Show...` submenu contains a number of options to show/hide various elements on the UI.
Specifically, the toolbar, CPU/PPU memory mappings, function/label lists, watch list, breakpoint list and the call stack window.

Additionally, two special tooltip windows can be enabled here:

* **Show Code Preview in Tooltips**: When enabled, label/address tooltips will now show a preview of the code at the target location.
* **Show OP Code Info Tooltips**: When enabled, putting the mouse over an OP code will display a tooltip containing information about the OP code and which CPU flags are affected by it.

### Break Options ###

The `Break Options` submenu contains a number of options to configure under which conditions the debugger will break (even when no breakpoint is hit):

* **Break on power/reset**: Break the emulation whenever a reset or power cycle occurs.
* **Break on unofficial opcodes**: Break the emulation whenever an unofficial opcode is about to execute.
* **Break on BRK**: Break the emulation whenever a BRK instruction is about to execute.
* **Break on CPU crash**: Break the emulation whenever an instruction that will cause the CPU to freeze is about to execute.
* **Break on uninitialized memory read**: Break whenever the code reads from a memory address containing an uninitialized value. **Note**: *This option only works if the debugger has been opened since the last reset or power cycle.*
* **Break when debugger is opened**: The emulation will break when you first open the debugger.
* **Break on debugger focus**: Whenever the debugger's window gains focus (e.g by clicking on it), the emulation will break.


Additionally, you can configure whether or not the debugger window gets focused when a break or pause occurs.

### Copy Options ###

These options configure which portions of the code is copied into the clipboard when copying code from the code window.

### Misc. Options ###

* **Hide Pause Icon**: When enabled, the pause icon that is normally shown whenever execution is paused will be hidden.
* **Draw Partial Frame**: When enabled, the emulator's main window will display the current partially-drawn frame instead of the last complete frame.
* **Show previous frame behind current**: When enabled along with `Draw Partial Frame`, the previous frame's data will be shown behind the current frame.
* **Refresh watch while running**: When enabled, the watch window will continuously refresh itself while the emulation is running (instead of only updating when the execution breaks)

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
