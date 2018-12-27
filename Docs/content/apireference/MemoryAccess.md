---
title: Memory Access
weight: 30
pre: ""
chapter: false
---

## read / readWord ##

**Syntax**  

    emu.read(address, type, signed = false)
	emu.readWord(address, type, signed = false)

**Parameters**  
address - *Integer* The address/offset to read from.  
type - *Enum* The type of memory to read from. See [memType](/apireference/enums.html#memtype).  
signed - (optional) *Boolean* If true, the value returned will be interpreted as a signed value.

**Return value**  
An 8-bit (read) or 16-bit (readWord) value.

**Description**  
Reads a value from the specified [memory type](/apireference/enums.html#memtype).  

When calling read / readWord with the [memType.cpu](/apireference/enums.html#memtype) or [memType.ppu](/apireference/enums.html#memtype) memory types, emulation side-effects may occur.  
To avoid triggering side-effects, use the [memType.cpuDebug](/apireference/enums.html#memtype) or [memType.ppuDebug](/apireference/enums.html#memtype) types, which will not cause side-effects.


## write / writeWord ##

**Syntax**  

    emu.write(address, value, type)
	emu.writeWord(address, value, type)

**Parameters**  
address - *Integer* The address/offset to write to.    
value - *Integer* The value to write.  
type - *Enum* The type of memory to write to. See [memType](/apireference/enums.html#memtype).  

**Return value**  
*None*

**Description**  
Writes an 8-bit or 16-bit value to the specified [memory type](/apireference/enums.html#memtype).  

Normally read-only types such as PRG-ROM or CHR-ROM can be written to when using [memType.prgRom](/apireference/enums.html#memtype) or [memType.chrRom](/apireference/enums.html#memtype).  
Changes will remain in effect until a power cycle occurs.  
To revert changes done to ROM, see [revertPrgChrChanges](#revertprgchrchanges).

When calling write / writeWord with the [memType.cpu](/apireference/enums.html#memtype) or [memType.ppu](/apireference/enums.html#memtype) memory types, emulation side-effects may occur.  
To avoid triggering side-effects, use the [memType.cpuDebug](/apireference/enums.html#memtype) or [memType.ppuDebug](/apireference/enums.html#memtype) types, which will not cause side-effects.


## revertPrgChrChanges ##

**Syntax**  

    emu.revertPrgChrChanges()

**Return value**  
*None*

**Description**  
Reverts all modifications done to PRG-ROM and CHR-ROM via write/writeWord calls.


## getPrgRomOffset ##

**Syntax**  

    emu.getPrgRomOffset(address)

**Parameters**  
address - *Integer* A CPU address (Valid range: $0000-$FFFF)

**Return value**  
*Integer* The corresponding byte offset in PRG ROM

**Description**  
Returns an integer representing the byte offset of the specified CPU address in PRG ROM based on the mapper's current configuration. Returns -1 when the specified address is not mapped to PRG ROM.


## getChrRomOffset ##

**Syntax**  

    emu.getChrRomOffset(address)

**Parameters**  
address - *Integer* A PPU address (Valid range: $0000-$3FFF)

**Return value**  
*Integer* The corresponding byte offset in CHR ROM

**Description**  
Returns an integer representing the byte offset of the specified PPU address in CHR ROM based on the mapper's current configuration. Returns -1 when the specified address is not mapped to CHR ROM.

## getLabelAddress ##

**Syntax**  

    emu.getLabelAddress(label)

**Parameters**  
label - *String* A label to look up.

**Return value**  
*Integer* The corresponding integer address.

**Description**  
Returns an integer representing the address of the specified label. The return value can be passed directly to `read()` and `readWord()`.

