---
title: Memory Tools
weight: 14
chapter: false
---

## Memory Viewer ##

<div class="imgBox"><div>
	<img src="/images/HexEditor.png" />
	<span>Memory Viewer</span>
</div></div>

The memory viewer offers read and write access to all types of ROM and RAM:

* CPU Memory
* PPU Memory
* Palette RAM
* OAM RAM
* Secondary OAM RAM
* PRG ROM
* CHR ROM
* CHR RAM
* Work RAM
* Save RAM

{{% notice tip %}}
PRG ROM and CHR ROM *are* writable from the memory viewer.  Any change will remain in effect until a power cycle. If you want to save your PRG/CHR ROM modifications to a .nes file, or as an IPS patch, you can use the **<kbd>File&rarr;Save</kbd>** or **<kbd>File&rarr;Save edits as IPS</kbd>** commands in the [debugger window](/debugging/debugger.html).
{{% /notice %}}

### Editing Memory Values ###

There are 2 ways to edit memory values:

* **Using the hex view**: Click on the byte you want to change in the hex view (on the left), and type hexadecimal values to replace it.

* **Using the text view**: Click on the section you want to change in the text view (on the right), and type ASCII text to replace. This method is rather limited and usually not very useful unless the ROM uses ASCII values for its text.

### Importing / Exporting ###

For most types of memory, it is possible to export its contents to a binary file as well as import it back from a file.  Use the `Import` and `Export` commands to do this.

### Highlighting ###

There are a number of highlighting/coloring options in the memory viewer.  

<kbd>**View&rarr;Highlight**</kbd> has coloring options for addresses that were recently read, written or executed (colored in <span class="blue">blue</span>, <span class="red">red</span> and <span class="green">green</span>, respectively). A fade-out period can also be configured, after which the byte will revert to its normal black color.  

<kbd>**View&rarr;De-emphasize**</kbd> offers options to display bytes matching certain conditions (unused, read, written or executed) in <span class="gray">gray</span>.

### Freezing values ###

Using the right-click menu, you can `Freeze` values in CPU Memory. Frozen addresses are shown in <span class="magenta">magenta</span>.  
Frozen addresses will no longer be affected by write operations - effectively making those addresses read-only for the CPU.  It is still possible to manually edit the value of frozen addresses using the memory viewer.

### Using TBL Files ###

<div class="imgBox"><div>
	<img src="/images/HexEditorTbl.png" />
	<span>Memory Viewer with a TBL file loaded to display Japanese text</span>
</div></div>

TBL files are text files that define mappings between sequences of bytes and text characters.  For example, it might define the byte $95 as the character 'A'.  

Normally, when no TBL file is loaded, the memory viewer will display each byte's standard ASCII representation on the right-hand side.
Once a TBL file is loaded, the text representation of the data will be updated to reflect the TBL mappings. This is useful, for example, when translating text.


## Memory Access Counters ##

<div class="imgBox"><div>
	<img src="/images/MemoryAccessCounters.png" />
	<span>Memory Access Counters</span>
</div></div>

When active, the debugger keeps track of all CPU and PPU memory reads, writes and executions.  It is possible to view these counters here.  

Use the `Sort By` option to sort the list based on different criteria.  

The `Reset` button allows you to reset all counters back to 0 -- this is useful when you are trying to gather data for a specific portion of the execution.  

Use the `Highlight uninitialized memory reads` option to track down any reads done to RAM memory before the RAM memory has been initialized after a power cycle -- reading from uninitialized memory can produce random behavior, which is usually unwanted.

## Profiler ##

<div class="imgBox"><div>
	<img src="/images/Profiler.png" />
	<span>Code Profiler</span>
</div></div>

The profiler automatically collects data about all function calls done by the code, as well as the number of clock cycles spent in each respective function.  

Using the profiler makes it is easy to find the bottlenecks in a game's code, which can help code optimization efforts.

If you are familiar with Visual Studio's profiler, these columns should be familiar:  

* **Call Count**: The number of times this function was called during profiling
* **Inclusive Time (Cyc)**: The amount of CPU cycles spent within this function (including the cycles spent by all functions called by this function)
* **Inclusive Time (%)**: The relative portion of CPU time spent within this function (including the time spent by all functions called by this function)
* **Exclusive Time (Cyc)**: The amount of CPU cycles spent within this function (functions called by this function are excluded)
* **Exclusive Time (%)**: The relative portion of CPU time spent within this function (functions called by this function are excluded)
* **Call Count**: The number of times this function was called during profiling

Use the `Reset` button to reset the profiler's data -- use this when you want to profile a specific portion of the execution.