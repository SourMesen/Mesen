---
title: Preferences
weight: 10
chapter: false
---

## General Options ##

<div class="imgBox"><div>
	<img src="/images/Preferences_General.png" />
	<span>General Options</span>
</div></div>

**Display Language**: Selects in which language the UI is shown -- defaults the user's default language.

**Automatically check for updates**: When enabled, Mesen will check for a new version of the emulator every time the emulator is started.

**Only allow one instance of Mesen at a time**: When enabled, only a single copy of Mesen can be opened at the same time.  This is useful when using file associations to load games by double-clicking on the rom files.

### Pause/Background settings ###

**Hide the pause screen**: When enabled, the `PAUSE` screen is no longer shown when the emulator is paused.

**Pause when a movie finishes playing**: When enabled, the emulator will automatically pause when a movie ends its playback.

**Allow input when in background**: When enabled, gamepad input can still be used even if the window is in the background. *This does not work for keyboard key bindings.*

**Pause when in**: 

* **Background**: When enabled, the emulator will automatically pause when in the background.
* **Menu and config dialogs**: When enabled, the emulator will automatically pause when in menus or configuration dialogs.
* **Debugging tools**: When enabled, the emulator will automatically pause when in debugging tools.

### Misc. Settings ###

**Automatically apply IPS/BPS patches**: When enabled, any IPS or BPS patch file found in the same folder as the ROM file will automatically be applied. (i.e: when loading `MyRom.nes`, if a file called `MyRom.ips` exists in the same folder, it will be loaded automatically.)

**Automatically hide the menu bar**: When enabled, the menu bar will automatically be hidden when not in use, even in windowed mode. *The menu bar is always hidden automatically when playing in fullscreen mode, whether this option is enabled or not.*

**Display confirmation dialog before reset/power cycle/exit**: When enabled, a confirmation dialog will be shown before a reset or a power cycle or before the emulator is closed.

**Display play/record icon when playing or recording a movie**: When enabled, an icon will be shown on the screen whenever a movie is playing or recording.

**Enable developer mode**: When enabled, all debugging tools are moved to a `Debug` menu accessible directly from the main window. This makes debugging tools more accessible as they no longer require opening the debugger before being able to open any other tool.

## Shortcut Keys ##

<div class="imgBox"><div>
	<img src="/images/Preferences_ShortcutKeys.png" />
	<span>Shortcut Keys</span>
</div></div>

Shortcut keys are user-defined shortcuts for various features in Mesen.  Some of these features can only be accessed via shortcut keys (e.g: `Fast Forward` and `Rewind`).  
Most of these options are also available through the main window's menu -- the shortcuts configured in this section will appear next to the matching action in the main window's menu.  

**To change a shortcut**, click on the button for the shortcut you want to change and press the key(s) you want to set for this shortcut.  Once you release a key, all the keys you had pressed will be assigned to the shortcut.

**To clear a key binding**, right-click on the button.

Available shortcuts:

* **Fast Forward (Hold button)**: Hold down this key to fast forward the game.
* **Toggle Fast Forward**: Start/stop fast forwarding.
* **Rewind (Hold button)**: Hold down this key to rewind the game in real-time.
* **Toggle Rewind**: Start/stop rewinding.
* **Rewind 10 seconds**: Instantly rewinds 10 seconds of gameplay.
* **Rewind 1 minute**: Instantly rewinds 1 minute of gameplay.
* **Pause**: Pauses or unpauses the game.
* **Reset**: Resets the game.
* **Power Cycle**: Power cycles the game.
* **Power Off**: Powers off the game, returning to the game selection screen.
* **Exit**: Exits the emulator.
* **FDS - Insert Next Disk**: Inserts face A of the next disk.
* **FDS - Switch Side**: Switches to the current disk's other side (A or B)
* **FDS - Eject Disk**: Ejects the currently inserted disk
* **VS - Insert Coin 1**: Inserts a coin in slot 1.
* **VS - Insert Coin 2**: Inserts a coin in slot 2.
* **VS - Insert Coin 3**: Inserts a coin in slot 1 of the second console (VS DualSystem only).
* **VS - Insert Coin 4**: Inserts a coin in slot 2 of the second console (VS DualSystem only).
* **VS - Service Button**: Press to activate the service button.
* **VS - Service Button 2**: Press to activate the service button on the second console (VS DualSystem only).
* **Input Barcode**: Inputs a barcode into the connected barcode reader device.
* **Take Screenshot**: Takes a screenshot.
* **Load Random Game**: Loads a random game from your game folder.
* **Run Single Frame**: Press to run a single frame and pause.
* **Set Scale 1x to 6x**: Sets the scale to the matching value.
* **Toggle Fullscreen Mode**: Enters/exits fullscreen mode.
* **Toggle Debug Information**: Turns on/off the debug screen overlay.
* **Toggle FPS Counter**: Turns on/off the FPS counter.
* **Toggle Game Timer**: Turns on/off the game timer.
* **Toggle Frame Counter**: Turns on/off the frame counter.
* **Toggle Lag Counter**: Turns on/off the lag counter.
* **Toggle OSD (On-Screen Display)**: Turns on/off the OSD.
* **Toggle Display on Top**: Turns on/off the display on top option.
* **Toggle Background Layer**: Turns on/off the background layer.
* **Toggle Sprite Layer**: Turns on/off the sprite layer.
* **Enable/Disable Cheat Codes**: Press to toggle cheats on or off.
* **Enable/Disable Audio**: Press to toggle audio output on or off.
* **Toggle Keyboard Mode**: Turns on/off keyboard mode.
* **Toggle Maximum Speed**: Toggles emulation speed between 100% and maximum speed.
* **Increase Speed**: Increases the emulation speed.
* **Decrease Speed**: Decreases the emulation speed.
* **Open File**: Displays the open file dialog.
* **Open Debugger**: Opens the debugger.
* **Open Assembler**: Opens the assembler.
* **Open Ppu Viewer**: Opens the PPU viewer.
* **Open Memory Tools**: Opens the memory tools.
* **Open Script Window**: Opens the script window.
* **Open Trace Logger**: Opens the trace logger.
* **Select Next Save Slot**: Move to the next save state slot.
* **Select Previous Save Slot**: Move to the previous save state slot.
* **Save State**: Save the game's state in the currently selected slot.
* **Load State**: Load the game's state from the currently selected slot.
* **Save State - Slot X**: Save the game's state to the matching slot.
* **Load State - Slot X**: Load the game's state from the matching slot.
* **Load State - Auto Save Slot**: Load the game's state from the auto save slot.
* **Save State to File**: Save the game's state to a user-specified file.
* **Load State from File**: Load the game's state from a user-specified file.
* **Load Last Session**: Restores the game to the state it was the last time you stopped playing it.

## FDS / VS System / NSF settings ##

<div class="imgBox"><div>
	<img src="/images/Preferences_Nsf.png" />
	<span>FDS / VS / NSF settings</span>
</div></div>

### FDS Settings ###

The FDS (Famicom Disk System) is a Famicom-specific add-on that allows games to be stored on special floppy disks. These options help simplify playing FDS games by allowing the emulation to fast-forward through load screens and automatically switch disk when needed.

**Automatically insert disk 1 side A when starting FDS games**: By default, the FDS boots with no disk inserted in the drive.  This option makes it so the player does not need to manually insert disk 1, side A manually.

**Automatically fast forward FDS games when disk or BIOS is loading**: FDS games contain a large number of load screens due to their data being stored on floppy drives.  Mesen needs to emulate this floppy drive's speed to ensure accurate emulation.  This option makes it so Mesen runs the emulation as fast as it can when a game is loading data from the disk, which greatly reduces the time spent on loading screens.

**Automatically switch disks for FDS games**: FDS games are often split into multiple floppy disks, and each disk has 2 separate sides.  Due to this, FDS games often ask the player to change disk, or flip to the other side.  When this option is enabled, Mesen will attempt to detect when a game is asking for another disk and automatically insert it.

### VS DualSystem Settings ###

For VS DualSystem games, 2 separate consoles run at the same time, producing 2 different sets of audio and video. These options allow you to configure which audio/video streams you want to hear/see.

**Show video for [...]**: Selects whether both screens should be shown, or just one of them.

**Play audio for [...]**: Selects whether both audio streams should be played, or just one of them.


### NSF Settings ###

**Move to the next track after [x] milliseconds of silence**: If the currently playing track outputs no audio for over X milliseconds, the player will automatically move to the next track.  *Note: This only applies for `.nsf` files, not `.nsfe` files.*

**Limit track run time to [x] seconds**: Once the current track has been playing for over X seconds, the player will automatically move to the next track.  *Note: This only applies for `.nsf` files, not `.nsfe` files.*

**Enable APU IRQs for NSF files**: When enabled, APU IRQs will be allowed when NSF files are playing. Some NSF files do not properly disable APU IRQs in their initialization code, which will cause them to break if this option is enabled. *This option should not be enabled unless absolutely necessary.*


## Folders and File Associations ##

<div class="imgBox"><div>
	<img src="/images/Preferences_FoldersFiles.png" />
	<span>Folders and File Associations</span>
</div></div>

**File Associations**: Use these options to associate Mesen with these file types.  This will allow you to double-click on these files in a file explorer to open them in Mesen.

**Data Storage Location**: Mesen can either store its data in your user profile or in the same folder as the executable file itself. This is configured when you launch Mesen for the first time, but can also be changed here.  When changing from one option to the other, Mesen will automatically copy all files from one folder to the other, allowing you to keep your save data and all other important files automatically.

**Folder Overrides**: On top of configuring the main folder where Mesen keeps its data, you may also specify custom locations for certain folders used by Mesen to store user-created files such as recordings or save data.

## Advanced Options ##

<div class="imgBox"><div>
	<img src="/images/Preferences_Advanced.png" />
	<span>Advanced Options</span>
</div></div>

**Disable built-in game database**: Mesen contains a built-in database containing information on thousands of rom files -- it uses this database to use the most appropriate settings when loading a game (e.g `NTSC` vs `PAL`) and to fix incorrect file headers. *Disabling this option is not recommended.*

**Disable high resolution timer**: Mesen normally forces Windows' timer resolution down to 1 millisecond when a game is running. Keeping a low timer resolution helps keep the video and audio output as smooth as possible. Enabling this option disables Mesen's default behavior and keeps the timer interval to its regular value, which may slightly improve battery life on a laptop (which is the only reason why this option exists). *Disabling this option is not recommended.*

**Keep rewind data for the last [x] minutes**: The rewind feature in Mesen periodically takes save states and keeps them in memory to allow the emulator to rewind the game. These save states take a minimal amount of memory (roughly 1MB per minute). To limit the amount of memory that Mesen can use for rewind data, this configures the number of minutes that it is possible to rewind for.

### Window Settings ###

**Always display on top of other windows**: When enabled, Mesen's window will always be displayed above all other windows.

**Do not allow the main window to be resized using the mouse**: When enabled, the main window can only be resized by changing the video scaling option.

### UI Settings ###

**Disable on-screen display (OSD)**: When enabled, all on-screen messages are disabled.

**Display additional information in title bar**: When enabled, additional information is shown in the title bar (such as filter, scale, etc.), next to the game's name.

**Show full file path in recent file list**: When enabled, the recent files menu will display the file's full path instead of just its name.

**Show frame counter**: When enabled, the number of frames emulated since the last reset will be shown on the screen.

**Show game timer**: When enabled, the amount of time elapsed since the last reset will be shown on the screen.

**Show game configuration dialog when loading VS System games**: VS System arcade cabinets usually have a number of options that can be physically configured via DIP switches in the arcade cabinet itself.  When enabled, this option makes it so the DIP switches' configuration dialog is shown every time a VS System game is loaded. *This dialog can also be manually accessed from the `Game` menu.*

### Game Selection Screen Settings ###

**Start game from power-on instead of resuming the previous state**: Normally, when using the game selection screen to start a game, the game resumes where you left off. When this option is enabled, the game starts over from power on.

**Disable game selection screen**: When enabled, the game selection screen is hidden and cannot be used.