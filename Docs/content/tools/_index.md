---
title: Tools
weight: 3
chapter: false
---

<div class="imgBox right"><div>
	<img src="/images/ToolsMenu.png" />
	<span>Tools Menu</span>
</div></div>

## Netplay ##

### Hosting a game ###

<div class="imgBox"><div>
	<img src="/images/NetplayHost.png" />
	<span>Server Configuration</span>
</div></div>

**Server name**: This name will be shown to clients when they connect.

**Port**: The port used for the communication.  Mesen will attempt to automatically port-forward this port on your router when you start the server -- if this fails, you will have to manually forward the port on your router to allow people outside of your LAN to connect to your server.

The other options on this screen have not been implemented yet and are disabled for now.

<div class="clear"></div>

### Connecting to a server ###

<div class="imgBox"><div>
	<img src="/images/NetplayConnect.png" />
	<span>Connect to...</span>
</div></div>

**Host**: The host name of the server you want to connect to.  This is usually an IP address but can also be a domain name.

**Port**: The port to connect to -- this must match the `Port` value used by the server's host.

**Join as a spectator**: When enabled, you will join the server without taking control of a specific controller. An unlimited number of spectators can join a game, but only 4 people can take control of a controller.

Once you are connected to a server, you can select your controller via the **<kbd>Tools&rarr;Netplay&rarr;Select Controller</kbd>** menu.

## Movies ##

<div class="imgBox"><div>
	<img src="/images/MovieRecordingOptions.png" />
	<span>Movie Recording Options</span>
</div></div>

`Movies` are files that can be created by Mesen and played back within Mesen itself.  They are essentially a recording of the events that occurred during recording. To record an actual video file, see [Video Recorder](#video-recorder).

When you start recording, a configuration dialog is shown that allows you to select a number of options.

* **Save to**: The location where the movie will be saved. Press the **Browse** button to change it.
* **Record from**: Selects where the recording should start:
	* **Power on**: This will reset the game and start recording from the start. Your save data (.sav files) will be excluded from the movie file - after the reset, you will start the game as if it had never been run yet.
	* **Power on, with save data**: This will reset the game and start recording from the start. Your save data (.sav files, etc.) will be included in the movie file.
	* **Current state**: This will start recording from the current state -- in this case, the movie file will contain a save state.
* **Author**: The movie's author (optional) - this will be saved in the movie file.
* **Description**: A description of the movie's content (optional) - this will be saved in the movie file.

To play a movie file, select it via the **<kbd>Tools&rarr;Movies&rarr;Play</kbd>** command.

## Cheats ##

<div class="imgBox"><div>
	<img src="/images/CheatList.png" />
	<span>Cheats Window</span>
</div></div>

Mesen supports cheats in a number of different formats, including Game Genie and Pro Action Rocky codes.  

It is also possible to import cheats from the built-in [Cheat Database](#from-the-cheat-database) or from [XML or CHT files](#from-xml-cht-files).

Select a game on the left to see all the cheats currently available for that game.

**To add a new cheat code**, click the `Add Cheat` button.  
**To edit a cheat code**, double-click on it in the list.  
**To delete a cheat code**, select it from the list and click the `Delete` button.

**To import cheats**, click the `Import` button.  
**To export cheats to an XML file**, click the `Export` button.

**To disable a specific cheat**, uncheck it from the list.  
**To disable all cheats**, check the `Disable all cheats` option.

<div class="clear"></div>

### Adding/Editing Cheats ###

<div class="imgBox"><div>
	<img src="/images/EditCheat.png" />
	<span>Edit Cheat</span>
</div></div>

When adding a cheat, you must first select the game to which it should be applied.  By default, the game that is currently loaded will be selected.  

You must give each cheat a name, which will be used to display it in the cheats list.

The `Code` section lets you enter the actual cheat -- select `Game Genie` for Game Genie codes.  
If you want to create a custom code, select the `Custom` action.  

When creating custom codes, the `Memory` / `Game Code` options select whether the code should be applied to a specific CPU address (`Memory`) or a specific offset in the PRG ROM (`Game Code`).

<div class="clear"></div>

### Importing Cheats ###

#### From the Cheat Database ####

<div class="imgBox"><div>
	<img src="/images/CheatDbImport.png" />
	<span>Import cheats from the built-in cheat database</span>
</div></div>

To import from the cheats database, click `Import` and select `From Cheat Database`.  

In the next dialog, select the game for which you want to import cheats.  You can type in the `Search` bar at the top to filter the game list.  Once you've selected a game, press OK -- this will import all cheats for that game into the cheats list.  You can then manually enable any cheat you want to use.

By default, the game that is currently loaded will be selected for you.  Having no game selected when the dialog opens indicates that there are no built-in cheats available for the game that is currently running.

#### From XML/CHT files ####

To import cheats from external files (FCEUX's `.cht` files or Nestopia's `.xml` files), click `Import`, and select `From File (XML, CHT)`.  
In the next dialog, select the file you want to import.  
For FCEUX's `.cht` files, you will also need to select the game for which you are importing cheats for.  
Once you're done, click `OK` -- the cheats will be imported and added to the cheats list.

## Sound Recorder ##

The sound recorder allows you to record uncompressed `.wav` files.  The `.wav` file will use the exact same output settings as Mesen's [audio options](/configuration/audio.html) -- this means the sample rate will match Mesen's current sample rate, and that any sound modification (volume, panning, equalizer or effects) will also be applied to the `.wav` files.

To start recording, use the **<kbd>Tools&rarr;Sound Recorder&rarr;Record</kbd>** command.  
To stop an on-going recording, use the **<kbd>Tools&rarr;Sound Recorder&rarr;Stop Recording</kbd>** command.

## Video Recorder ##

<div class="imgBox"><div>
	<img src="/images/VideoRecording.png" />
	<span>Video Recorder</span>
</div></div>

Much like the sound recorder, the video recorder allows you to record `.avi` files.

Before you start a recording, you can select where to save the `.avi` file and which video codec to use.  All video codecs are lossless codecs -- the only reason to reduce the compression level to a lower level is to improve performance in the event your computer is having a hard time recording the video and running the emulation at its normal speed at the same time.

To start recording, use the **<kbd>Tools&rarr;Video Recorder&rarr;Record</kbd>** command.  
To stop an on-going recording, use the **<kbd>Tools&rarr;Video Recorder&rarr;Stop Recording</kbd>** command.

## Log Window ##

<div class="imgBox"><div>
	<img src="/images/LogWindow.png" />
	<span>Log Window</span>
</div></div>

The Log Window displays a number of useful information -- mostly about the roms you load.  
It is also sometimes used as a way to log errors or warnings.

## Debugger ##

See [Debugging Tools](/debugging.html)


## HD Pack Builder ##

See [HD Packs](/hdpacks.html)
