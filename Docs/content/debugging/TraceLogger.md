---
title: Trace Logger
weight: 35
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

### Custom Formatting ###

The trace logger's output can be customized by enabling the **Format Override** option and editing the format string.
The following tags can be used to customize the format:

```text
[ByteCode]: The byte code for the instruction (1 to 3 bytes).
[Disassembly]: The disassembly for the current instruction.
[EffectiveAddress]: The effective address used for indirect addressing modes.
[MemoryValue]: The value stored at the memory location referred to by the instruction.
[PC]: Program Counter
[A]: A register
[X]: X register
[Y]: Y register
[SP]: Stack Pointer
[P]: Processor Flags
[Cycle]: The current PPU cycle.
[Scanline]: The current PPU scanline.
[FrameCount]: The current PPU frame.
[CycleCount]: The current CPU cycle (32-bit signed value, resets to 0 at power on)
```

You can also specify some options by using a comma. e.g:
```text
[Cycle,3] will display the cycle and pad out the output to always be 3 characters wide.
[Scanline,h] will display the scanline in hexadecimal.
[Align,50]: Align is a special tag that is useful when trying to align some content. [Align,50] will make the next tag start on column 50.
```

### Conditional Logging ###

The `Condition` field accepts conditional statements in the same format as [breakpoints](/debugging/debugger.html#breakpoint-configuration).  

When a condition is entered, only instructions that match the given condition will be logged. This can be used, for example, to log cartridge register writes (e.g: `IsWrite && Address >= $8000`), PPU register reads (e.g: `IsRead && Address >= $2000 && Address <= $3FFF`) or when a specific portion of CPU memory is being executed (e.g: `pc >= $8100 && pc <= $8150`).  
Conditions are very flexible and can be used to check just about any condition -- use them to your advantage.