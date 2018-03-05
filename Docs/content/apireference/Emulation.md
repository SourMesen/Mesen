---
title: Emulation
weight: 15
pre: ""
chapter: false
---

## getState ##

**Syntax**  

    emu.getState()

**Return value**  
*Table* Current emulation state with the following structure:

	region: int,
	clockRate: int,
	cpu: {
	  status: int,
	  a: int,
	  irqFlag: int,
	  cycleCount: int,
	  pc: int,
	  y: int,
	  x: int,
	  sp: int,
	  nmiFlag: bool
	},
	ppu: {
	  cycle: int,
	  scanline: int,
	  frameCount: int,
	  control: {
		backgroundEnabled: bool,
		intensifyBlue: bool,
		intensifyRed: bool,
		backgroundPatternAddr: int,
		grayscale: bool,
		verticalWrite: bool,
		intensifyGreen: bool,
		nmiOnVBlank: bool,
		spritesEnabled: bool,
		spritePatternAddr: int,
		spriteMask: bool,
		largeSprites: bool,
		backgroundMask: bool
	  },
	  status: {,
		spriteOverflow: bool,
		verticalBlank: bool,
		sprite0Hit: bool
	  },
	  state: {
		status: int,
		lowBitShift: int,
		xScroll: int,
		highBitShift: int,
		videoRamAddr: int,
		control: int,
		mask: int,
		tmpVideoRamAddr: int,
		writeToggle: bool,
		spriteRamAddr: int
	  }  
	},
	apu: {
	  square1: {
		outputVolume: int,
		frequency: float,
		duty: int,
		period: int,
		enabled: bool,
		dutyPosition: int,
		sweepShift: int,
		sweepPeriod: int,
		sweepEnabled: bool,
		sweepNegate: bool
		envelope: {
		  counter: int,
		  loop: bool,
		  divider: int,
		  volume: int,
		  startFlag: bool,
		  constantVolume: bool
		},	
		lengthCounter: {
		  halt: bool,
		  counter: int,
		  reloadValue: int
		}	
	  },
	  square2: {
		outputVolume: int,
		frequency: float,
		duty: int,
		period: int,
		enabled: bool,
		dutyPosition: int,
		sweepShift: int,
		sweepPeriod: int,
		sweepEnabled: bool,
		sweepNegate: bool,
		envelope: {
		  counter: int,
		  loop: bool,
		  divider: int
		  volume: int,
		  startFlag: bool,
		  constantVolume: bool
		},
		lengthCounter: {
		  halt: bool,
		  counter: int,
		  reloadValue: int
		}	
	  },  
	  triangle: {
		outputVolume: int,
		frequency: float,
		sequencePosition: int,
		period: int,
		enabled: bool,
		lengthCounter: {
		  halt: bool,
		  counter: int,
		  reloadValue: int
		}	
	  },
	  noise: {
		modeFlag: bool,
		enabled: bool,
		outputVolume: int,
		frequency: float,
		period: int,
		shiftRegister: int,
		envelope: {
		  counter: int,
		  loop: bool,
		  divider: int,
		  volume: int,
		  startFlag: bool,
		  constantVolume: bool
		},	
		lengthCounter: {
		  halt: bool,
		  counter: int,
		  reloadValue: int
		}
	  },
	  dmc: {
		sampleLength: int,
		irqEnabled: bool,
		loop: bool,
		outputVolume: int,
		bytesRemaining: int,
		sampleAddr: int,
		period: int,
		sampleRate: float
	  },  
	  frameCounter: {
		fiveStepMode: int,
		irqEnabled: int,
		sequencePosition: int
	  }  
	},
	cart: {
	  selectedPrgPages: array,
	  chrRomSize: int,
	  chrRamSize: int,
	  prgPageCount: int,
	  chrPageSize: int,
	  selectedChrPages: array,
	  chrPageCount: int,
	  prgRomSize: int,
	  prgPageSize: int,
	}

**Description**  
Return a table containing information about the state of the CPU, PPU, APU and cartridge.   

## setState ##

**Syntax**  

    emu.setState(state)

**Parameters**  
state - *Table* A table containing the state of the emulation to apply.

**Return value**  
*None* 

**Description**  
Updates the CPU and PPU's state.
The *state* parameter must be a table in the same format as the one returned by [getState()](#getstate)  
**Note:** the state of the APU or cartridge cannot be modified by using setState().

## breakExecution ##

**Syntax**  

    emu.breakExecution()

**Return value**  
*None*

**Description**  
Breaks the execution of the game and displays the debugger window. 

## execute ##

**Syntax**  

    emu.execute(count, type)

**Parameters**  
count - *Integer* The number of cycles or instructions to run before breaking  
type - *Enum* See [executeCountType](/apireference/enums.html#executecounttype)

**Return value**  
*None*

**Description**  
Runs the emulator for the specified number of cycles/instructions and then breaks the execution.   

## reset ##

**Syntax**  

    emu.reset()

**Return value**  
*None*

**Description**  
Resets the current game.  

## resume ##

**Syntax**  

    emu.resume()

**Return value**  
*None*

**Description**  
Resumes execution after breaking. 


## rewind ##

**Syntax**  

    emu.rewind(seconds)

**Parameters**  
seconds - *Integer* The number of seconds to rewind

**Return value**  
*None*

**Description**  
Instantly rewinds the emulation by the number of seconds specified.  
**Note:** this can only be called from within a "StartFrame" event callback.  
