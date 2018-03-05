---
title: Trace Logger
weight: 15
chapter: false
---

<div class="imgBox"><div>
	<img src="/images/TraceLogger.png" />
	<span>Trace Logger</span>
</div></div>

### Basic Information ###

The trace logger displays the execution log of the CPU.  It can display the last 30,000 CPU instructions executed. Additionally, it is also possible to log these instructions to the disk by using the `Start Logging` button. Log files can rapidly grow in size (to several GBs worth of data in a few seconds), so it is recommended to log for the shortest amount of time needed.

### Display Options ###

A number of options that toggle the display of several elements exist: `Registers`, `CPU Cycles`, `PPU Cycles`, `PPU Scanline`, `Show Effective Addresses`, `Byte Code`, `Frame Count`, `Additional Information (IRQ, NMI, etc.)`.  Adjust these based on your needs.

Additionally, you can alter the way some elements are displayed:

* **Status Flag Format**: Offers a number of different ways to display the CPU's status flags.
* **Indent code based on stack pointer**: When enabled, the log's lines will be indented by 1 character for every byte pushed to the stack. This is useful to quickly be able to identify function calls, for example.
* **Use Labels**: When enabled, addresses that match known labels will be replaced by their label instead.

### Conditional Logging ###

The `Condition` field accepts conditional statements in the same format as [breakpoints](/debugging/debugger.html#breakpoint-configuration).  

When a condition is entered, only instructions that match the given condition will be logged. This can be used, for example, to log cartridge register writes (e.g: `IsWrite && Address >= $8000`), PPU register reads (e.g: `IsRead && Address >= $2000 && Address <= $3FFF`) or when a specific portion of CPU memory is being executed (e.g: `pc >= $8100 && pc <= $8150`).  
Conditions are very flexible and can be used to check just about any condition -- use them to your advantage.