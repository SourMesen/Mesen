---
title: Enums
weight: 55
pre: ""
chapter: false
---

## eventType ##

**Syntax** 

	emu.eventType.[value]

**Values**

```text
reset = 0,          Triggered when a soft reset occurs
nmi = 1,            Triggered when an nmi occurs
irq = 2,            Triggered when an irq occurs
startFrame = 3,     Triggered at the start of a frame (cycle 0, scanline -1)
endFrame = 4,       Triggered at the end of a frame (cycle 0, scanline 241)
codeBreak = 5,      Triggered when code execution breaks (e.g due to a breakpoint, etc.)
stateLoaded = 6,    Triggered when a user manually loads a savestate
stateSaved = 7,     Triggered when a user manually saves a savestate
inputPolled = 8,    Triggered when the emulation core polls the state of the input devices for the next frame
spriteZeroHit = 9,  Triggered when the PPU sets the sprite zero hit flag 
scriptEnded = 10    Triggered when the current Lua script ends (script window closed, execution stopped, etc.)
```

**Description**  
Used by [addEventCallback](/apireference/callbacks.html#addeventcallback) / [removeEventCallback](/apireference/callbacks.html#removeeventcallback) calls.
 
## executeCountType ##

**Syntax** 

	emu.executeCountType.[value]

**Values**
	
```text
cpuCycles = 0,          Count the number of CPU cycles
ppuCycles = 1,          Count the number of PPU cycles
cpuInstructions = 2     Count the number of CPU instructions
```
	
**Description**  
Used by [execute](/apireference/emulation.html#execute) calls.
 
## memCallbackType ##

**Syntax** 

	emu.memCallbackType.[value]

**Values**

```text
cpuRead = 0,     Triggered when a read instruction is executed
cpuWrite = 1,    Triggered when a write instruction is executed
cpuExec = 2,     Triggered when any memory read occurs due to the CPU's code execution
ppuRead = 3,     Triggered when the PPU reads from its memory bus
ppuWrite = 4     Triggered when the PPU writes to its memory bus
```

**Description**  
Used by [addMemoryCallback](/apireference/callbacks.html#addmemorycallback) / [removeMemoryCallback](/apireference/callbacks.html#removememorycallback) calls.
 
## memType ##

**Syntax** 

	emu.memType.[value]

**Values**

```text
cpu = 0,              CPU memory - $0000 to $FFFF          Warning: Reading or writing to this memory type may cause side-effects!
ppu = 1,              PPU memory - $0000 to $3FFF          Warning: Reading or writing to this memory type may cause side-effects!
palette = 2,          Palette memory - $00 to $3F
oam = 3,              OAM memory - $00 to $FF
secondaryOam = 4,     Secondary OAM memory - $00 to $1F
prgRom = 5,           PRG ROM - Range varies by game
chrRom = 6,           CHR ROM - Range varies by game
chrRam = 7,           CHR RAM - Range varies by game
workRam = 8,          Work RAM - Range varies by game
saveRam = 9,          Save RAM - Range varies by game
cpuDebug = 256,       CPU memory - $0000 to $FFFF          Same as memType.cpu but does NOT cause any side-effects.
ppuDebug = 257        PPU memory - $0000 to $3FFF          Same as memType.ppu but does NOT cause any side-effects.
```	

**Description**  
Used by [read](/apireference/memoryaccess.html#read-readword) / [write](/apireference/memoryaccess.html#write-writeword) calls.


## counterMemType ##

**Syntax** 

	emu.counterMemType.[value]

**Values**

```text
nesRam = 0,
prgRom = 1,
workRam = 2,
saveRam = 3
```	

**Description**  
Used by [getAccessCounters](/apireference/misc.html#getaccesscounters) calls.


## counterOpType ##

**Syntax** 

	emu.counterOpType.[value]

**Values**

```text
read = 0,
write = 1,
exec = 2
```	

**Description**  
Used by [getAccessCounters](/apireference/misc.html#getaccesscounters) calls.