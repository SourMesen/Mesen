---
title: Miscellaneous
weight: 40
pre: ""
chapter: false
---

## Save States ##

There are 2 separate save state APIs.  
The first one is synchronous and uses the [saveSavestate](#savesavestate) and [loadSavestate](#loadsavestate) functions. Its main restriction is that it can only be used inside "startFrame" or "cpuExec" callback functions.  
The second API is asynchronous and uses an internally-managed "save slot" system to hold states in memory. It uses the following functions: [saveSavestateAsync](#savesavestateasync), [loadSavestateAsync](#loadsavestateasync), [getSavestateData](#getsavestatedata) and [clearSavestateData](#clearsavestatedata).


### saveSavestate ###

**Syntax**  

    emu.saveSavestate()

**Return value**  
*String* A string containing a binary blob representing the emulation's current state.

**Description**  
Creates a savestate and returns it as a binary string. (The savestate is not saved on disk)   
**Note:** this can only be called from within a "startFrame" event callback or "cpuExec" memory callback.

  
### saveSavestateAsync ###
  
**Syntax**  

    emu.saveSavestateAsync(slotNumber)

**Parameters**  
slotNumber - *Integer* A slot number to which the savestate data will be stored (slotNumber must be >= 0)
	
**Return value**  
*None*

**Description**  
Queues a save savestate request.  As soon at the emulator is able to process the request, it will be saved into the specified slot.  
This API is asynchronous because save states can only be taken in-between 2 CPU instructions, not in the middle of an instruction.
When called while the CPU is in-between 2 instructions (e.g: inside the callback of an cpuExec or startFrame event), the save state will be taken immediately and its data will be available via [getSavestateData](#getsavestatedata) right after the call to saveSavestateAsync.  
The savestate can be loaded by calling the [loadSavestateAsync](#loadsavestateasync) function.

  
  
### loadSavestate ###

**Syntax**  

    emu.loadSavestate(savestate)

**Parameters**  
savestate - *String* A binary blob representing a savestate, as returned by [saveSavestate()](#savesavestate)

**Return value**  
*None*

**Description**  
Loads the specified savestate.  
**Note:** this can only be called from within a "startFrame" event callback or "cpuExec" memory callback.


### loadSavestateAsync ###

**Syntax**  

    emu.loadSavestateAsync(slotNumber)

**Parameters**  
slotNumber - *Integer* The slot number to load the savestate data from (must be a slot number that was used in a preceding [saveSavestateAsync](#savesavestateasync) call)

**Return value**  
*Boolean* Returns true if the slot number was valid.

**Description**  
Queues a load savestate request.  As soon at the emulator is able to process the request, the savestate will be loaded from the specified slot.  
This API is asynchronous because save states can only be loaded in-between 2 CPU instructions, not in the middle of an instruction.
When called while the CPU is in-between 2 instructions (e.g: inside the callback of an cpuExec or startFrame event), the savestate will be loaded immediately.


### getSavestateData ###

**Syntax**  

    emu.getSavestateData(slotNumber)

**Parameters**  
slotNumber - *Integer* The slot number to get the savestate data from (must be a slot number that was used in a preceding [saveSavestateAsync](#savesavestateasync) call)

**Return value**  
*String* A binary string containing the savestate

**Description**  
Returns the savestate stored in the specified savestate slot.


### clearSavestateData ###

**Syntax**  

    emu.clearSavestateData(slotNumber)

**Parameters**  
slotNumber - *Integer* The slot number to get the savestate data from (must be a slot number that was used in a preceding [saveSavestateAsync](#savesavestateasync) call)

**Return value**  
*None*

**Description**  
Clears the specified savestate slot (any savestate data in that slot will be removed from memory).


## Cheats ##

### addCheat ###

**Syntax**  

    emu.addCheat(cheatCode)

**Parameters**  
cheatCode - *String* A game genie format cheat code.

**Return value**  
*None* 

**Description**  
Activates a game genie cheat code (6 or 8 characters).  
**Note:** cheat codes added via this function are not permanent and not visible in the UI.

### clearCheats ###

**Syntax**  

    emu.clearCheats()

**Return value**  
*None* 

**Description**  
Removes all active cheat codes (has no impact on cheat codes saved within the UI)


## Misc ##

### getLogWindowLog ###

**Syntax**  

    emu.getLogWindowLog()

**Return value**  
*String* A string containing the log shown in the log window

**Description**  
Returns the same text as what is shown in the emulator's Log Window.

### getRomInfo ###

**Syntax**  

    emu.getRomInfo()

**Return value**  
*Table* Information about the current ROM with the following structure:

```text
name: string,            Filename of the current ROM
path: string,            Full path to the current ROM (including parent compressed archive when needed)
fileCrc32Hash: int,      The CRC32 value for the whole ROM file
fileSha1Hash: string,    The SHA1 hash for the whole ROM file
prgChrCrc32Hash: int,	 The CRC32 value for the file, excluding its 16-byte header
prgChrMd5Hash: string,   The MD5 hash for the file, excluding its 16-byte header
format: int,			 Value that represents the ROM format:  1 = iNES, 2 = UNIF, 3 = FDS, 4 = NSF
isChrRam: bool           true when the game uses CHR RAM, false otherwise
```
	
**Description**  
Returns information about the ROM file that is currently running.



### getScriptDataFolder ###

**Syntax**  

    emu.getScriptDataFolder()

**Return value**  
*String* The script's data folder

**Description**  
This function returns the path to a unique folder (based on the script's filename) where the script should store its data (if any data needs to be saved).  
The data will be saved in subfolders inside the LuaScriptData folder in Mesen's home folder.


### takeScreenshot ###

**Syntax**  

    emu.takeScreenshot()

**Return value**  
*String* A binary string containing a PNG image. 

**Description**  
Takes a screenshot and returns a PNG file as a string.  
The screenshot is not saved to the disk.

