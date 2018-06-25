---
title: Getting Started
weight: 1
chapter: false
---

## System Requirements ##

### Windows ###

* Windows Vista, 7, 8, 8.1 and 10 are supported
* DirectX 11
* .NET Framework 4.5+

### Linux ###

* glibc 2.24+
* Mono 4.2.1+
* SDL 2

## Installation ##

<div class="imgBox right"><div>
	<img src="/images/ConfigWizard.png" />
	<span>Configuration Wizard</span>
</div></div>

There is no actual installer for Mesen -- run the Mesen.exe application and a first-run configuration wizard will be shown.

**Data Storage Location**: This section of the wizard allows you to select where you prefer to keep Mesen's files.

**Input Mappings**: Select which input types you want to use to play games. There are built-in presets for:

* Xbox controllers
* PS4 controllers
* WASD keyboard layout 
* Arrow keyboard layout

You can select multiple presets at once, but only a single keyboard layout.

**Create a shortcut on my desktop**: Check this option if you want to add a Mesen shortcut to your desktop.

## Using Mesen ##

<div class="imgBox right"><div>
	<img src="/images/GameMenu.png" />
	<span>Game Menu</span>
</div></div>

Mesen's default configuration should work out of the box and allow you to get right into playing games.  

To load a game, use the **<kbd>File&rarr;Open</kbd>** command and select any supported file (`.nes`, `.fds`, `.nsf`, `.nsfe`, `.unf`) you want to load.

Once a game is loaded, you can pause, reset or stop the game via the `Game` menu.  
The game menu also contains additional options for [Famicom Disk System (FDS)](#famicom-disk-system-fds-games) games and [VS System](#vs-system-games) games.

### Famicom Disk System (FDS) games ###

<div class="imgBox right"><div>
	<img src="/images/FdsGameMenu.png" />
	<span>FDS Game Menu</span>
</div></div>

{{% notice warning %}}
FDS games require a BIOS file. When you load a FDS game for the first time, Mesen will ask you for the BIOS file -- without one, running FDS games is not possible.
{{% /notice %}}

FDS games were originally stored on floppy disks - sometimes split across multiple disks and disk sides. The `Game` menu contains a number of additional shortcuts for FDS games to handle these:

* **Switch Disk Side**: Switches between sides A and B of the current disk.
* **Select Disk**: Allows you to select any disk and side combination availble for the current game.
* **Eject Disk**: Ejects the current disk.  *Ejecting the disk is usually unnecessary and only available for the sake of completeness.*

<div class="clear"></div>

### VS System games ###

<div class="imgBox right"><div>
	<img src="/images/VsGameMenu.png" />
	<span>VS System Game Menu</span>
</div></div>

VS System games were originally in the form of arcade cabinets -- unlike FDS games, playing them does not require any special BIOS.

Being arcade cabinets, VS System games typically require the player to insert coins before the game can be played.  Additionally, the arcade cabinets could often be configured via a number of physical DIP switches -- for example, to select how much money needs to be inserted to play, or to alter a game's difficulty.  The `Game` menu offers additional options when playing VS System games to handle these:

* **Game Configuration**: Displays a configuration dialog containing the DIP switch options available for this game.
* **Insert Coin 1**: Inserts a coin into the first coin slot.
* **Insert Coin 2**: Inserts a coin into the second coin slot.


<div class="clear"></div>

### NSF Player ###

<div class="imgBox"><div>
	<img src="/images/NsfPlayer.png" />
	<span>Mesen's NSF Player</span>
</div></div>

NSF and NSFe files are used to store music from NES and Famicom games.  

When loading NSF files into Mesen, the UI will change into a media player style UI. From this UI, you can control the volume, select the track, pause the music or fast forward by holding down the mouse button.

Additionally, the two icons at the top right allow you to toggle the repeat and shuffle playback modes.

<div class="clear"></div>

### Game Selection Screen ###

<div class="imgBox"><div>
	<img src="/images/GameSelectionScreen.png" />
	<span>Game Selection Screen</span>
</div></div>

The game selection screen is shown when no games are currently loaded -- it will display the last 5 games you've played, along with a screenshot of the game at the point where you left off playing.  

You can use this screen via the key bindings for player 1 - e.g press `Left` or `Right` on Player 1's d-pad to change game, and the `A` button to start the game.  You can also navigate the screen with your mouse -- use the arrows on each side of the screen to change game, and click on the game's screenshot to start playing.

<div class="clear"></div>

### Shortcut Keys ###

Mesen has a number of shortcut keys that you may find useful:

* <kbd>**Ctrl-O**</kbd>: Open a file
* <kbd>**Ctrl-R**</kbd>: Reset the game
* <kbd>**Escape**</kbd>: Pause/resume the game
* <kbd>**Alt-1 to Alt-6**</kbd>: Change the video scale.
* <kbd>**F1 to F8**</kbd>: Load save state in the corresponding slot.
* <kbd>**Shift-F1 to Shift-F7**</kbd>: Save a save state in the corresponding slot.
* <kbd>**Ctrl-S**</kbd>: Manually save a save state to a file.
* <kbd>**Ctrl-L**</kbd>: Manually load a save state from a file.
* <kbd>**Tab**</kbd>: Hold the tab key to fast forward the emulation (defaults to 300% speed)
* <kbd>**Backspace**</kbd>: Hold the backspace key to rewind the emulation, frame-by-frame.

{{% notice tip %}}
If you load a state by mistake, you can use the rewind feature to undo the load state action.
{{% /notice %}}

The [shortcut keys](/configuration/preferences.html#shortcut-keys) can be customized in the [preferences](/configuration/preferences.html).

<div class="clear"></div>

### Command-line Options ###

<div class="imgBox"><div>
	<img src="/images/CommandLineOptions.png" />
	<span>Command-line Options</span>
</div></div>

Mesen supports a large number of command-line options.  
To see a full list and some examples, click on the **<kbd>Help&rarr;Command-line Options</kbd>** menu option.