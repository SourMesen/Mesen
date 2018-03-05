---
title: HD Packs
weight: 5
chapter: false
---

HD Packs make it possible to replace a game's graphics and audio with high definition alternatives. This can be used in many ways, for example, one could keep the game's original resolution and simply improve its graphics by adding more colors and shading.

## Using HD packs ##

To use HD packs, first make sure to turn on the [Enable HDNes HD Packs](/configuration/video.html#general-options) option.  
To install a HD Pack, you should extract it in a subfolder inside the `HdPacks` folder in Mesen's data folder. You can open this folder by clicking on the `Open Mesen Folder` button in the [Preferences](/configuration/preferences.html#general-options) window.
The subfolder should have the same name as the rom file itself.

For HD packs created with Mesen's [HD Pack Builder](#using-the-hd-pack-builder), you can also put them in `.zip` format in the `HdPacks` folder, without unzipping them nor worrying about renaming the file.

## Using the HD Pack Builder ##

<div class="imgBox"><div>
	<img src="/images/HdPackBuilder.png" />
	<span>HD Pack Builder</span>
</div></div>

The HD Pack Builder can be used to record a game's graphics into PNG image files and build a matching `hires.txt` file. These are the 2 basic elements needed for HD Packs: a tile map (the PNG files) and a definition file that specifies where each tile is on the tile map (the `hires.txt` file).

The basic concept of this tool is to use it to record gameplay of a game from start to finish, attempting to trigger every possible animation or graphics during gameplay.  This will create a complete set of tiles (saved in PNG files) and a single hires.txt file that contains all that is needed to replace the tiles with HD tiles.  Once this is done, the only thing left to do, in most cases, is to replace the graphics in the PNG files with better alternatives (e.g higher resolution, more colors, etc.).

A number of options exist to control the way the PNG files are generated:

**Scale/Filter**: Selects the scale and video filter to use when generating the PNG files for the HD Pack. Use the "Prescale" filters to generate the tiles at a larger scale without applying any transformation to the pixels.

**CHR Bank Size**: This option is only available for CHR RAM games.  CHR RAM games have no fixed "banks" - they are dynamically created by the game's code. This option alters the HD Pack Builder's behavior when grouping the tiles into the PNG files - a smaller bank size will usually result in less PNG files (but depending on the game's code, larger values may produce better results).

**Group blank tiles**: This option groups all the blank tiles sequentially into the same PNG files - this helps reduce the number of PNG files produced by removing almost-empty PNG files containing only blank tiles.

**Sort pages by usage frequency**: When this option is enabled, the tiles in PNG files are sorted by the frequency at which they are shown on the screen while recording (more common palettes will be grouped together in the first PNG for a specific bank number. If this option is unchecked, the PNGs will be sorted by palette - each PNG will only contain up to 4 different colors in this case.

**Use 8x16 sprite display mode**: When enabled, this option will alter the display order of CHR banks that contain only sprites to make the sprites easier to edit in the PNG file.

**Ignore tiles at the edges of the screen (overscan)**: When enabled, this will make the builder ignore any pixels in the overscan area. This is useful in games that contain glitches on the outer edge of the screen. Incorrect palette combinations due to these glitches will be ignored and won't be shown in the PNG files.

Before you start recording, select the options you want to use and the location to which you want to save the HD Pack, then press the `Start Recording` button.


## File Format (hires.txt) ##

The following are the specifications for the hires.txt file, as of version "100".

### &lt;ver&gt; tag ###

**Syntax**: `<ver>[integer]`  
**Example**: `<ver>100`

The format's version number -- this is currently 100 for Mesen.

### &lt;scale&gt; tag ###

**Syntax**: `<scale>[integer]`  
**Example**: `<scale>4`

The scale used for the replacement graphics -- this can be any integer number (minimum: 1). Anything above 8-10x will probably have a very hard time running on any computer. It is suggested to use scales between 1x and 4x.

### &lt;overscan&gt; tag ###

**Syntax**: `<overscan>[top],[right],[bottom],[left]`  
**Example**: `<overscan>8,8,8,8`

The overscan values to use when the HD Pack is loaded - this is useful for games that produce glitches on the edges of the screen.

### &lt;patch&gt; tag ###

**Syntax**: `<patch>[filename],[sha1 hash]`  
**Example**: `<patch>MyPatch.ips,26aec27ef0fc1a6fd282937b918ebdd1fb480891`

Specifies a patch file to apply if the loaded ROM matches the specified SHA1 hash.  
The patches can be in either `.ips` or `.bps` format.
Multiple `<patch>` tags with different SHA1 hashes can be present in the same `hires.txt` file.

### &lt;img&gt; tag ###

**Syntax**: `<img>[filename]`  
**Example**: `<img>Tileset01.png`

Specifies a PNG file that contains tile graphics -- each `<img>` tag is indexed (starting from 0) according to the order it appears in the `hires.txt` file, the tag's index must be used when using `<tile>` tags.

### &lt;condition&gt; tag ###

**Syntax**: `<condition>[name - text], [conditionType - text], [x value - integer], [y value - integer], [tile data], [palette data - hex]`  
**Example (CHR ROM)**: `<condition>myCondition,tileNearby,8,0,10,0F100017`  
**Example (CHR RAM)**: `<condition>myCondition,tileNearby,8,0,D2C2C2C7CF2FFEFC2C3C3C3830D00000,0F100017`

For CHR ROM games, `tile data` is an integer representing the position of the original tile in CHR ROM.  
For CHR RAM games, `tile data` is a 32-character hexadecimal string representing all 16 bytes of the tile.  
`palette data` is always an 8-character hexadecimal string representing all 4 bytes of the palette used for the tile.  For sprites, the first byte is always "FF".

`conditionType` can be any of: `tileAtPosition`, `tileNearby`, `spriteAtPosition` and `spriteNearby`. 

`tileAtPosition` and `spriteAtPosition` use the X/Y parameters as screen coordinates. e.g:  
`<condition>myCondition,tileAtPosition,10,10,[tile data],[palette data]`  
In this case, `myCondition` will be true if the tile at position 10,10 on the NES' screen (256x240 resolution) matches the tile+palette data given.

`tileNearby` and `spriteNearby` use positive or negative X/Y offsets to the current position. e.g:  
`<condition>myCondition2,tileNearby,-8,0,[tile data],[palette data]`  
In this case, `myCondition2` will be true if the tile 8 pixels to the left of the current tile matches the tile+palette data specified.


### &lt;tile&gt; tag ###

**Syntax**: `<tile>[img index - integer], [tile data], [palette data], [x - integer], [y - integer], [brightness - 0.0 to 1.0], [default tile - Y or N]`  
**Example (CHR ROM)**: `<tile>0,20,FF16360F,0,0,1,N`  
**Example (CHR RAM)**: `<tile>0,0E0E079C1E3EA7076101586121010000,0F100017,176,1168,1,N`

For CHR ROM games, `tile data` is an integer representing the position of the original tile in CHR ROM.  
For CHR RAM games, `tile data` is a 32-character hexadecimal string representing all 16 bytes of the tile.  
`palette data` is always an 8-character hexadecimal string representing all 4 bytes of the palette used for the tile.  For sprites, the first byte is always "FF".

`<tile>` define mappings between the original game's tile data and their replacements in the PNG file.
The `tile data` and `palette data` are used to identify the original tile, while the `img index`, `x` and `y` fields are used to specify in which PNG file the replacement can be found, and at what x,y coordinates in that PNG file.

`brightness` can be used to reuse the same HD tile for multiple original tiles -- this can be useful when a game has fade in and out effects.  
When `default tile` is enabled (with `Y`), the tile is marked as the `default tile` for all palettes.  Whenever a tile appears on the screen that matches the tile data, but has no rules matching its palette data, the default tile will be used instead.


### &lt;background&gt; tag ###

**Syntax**: `<background>[name - text], [brightness level - 0.0 to 1.0]`  
**Example**: `<background>myBackground.png,1.0`

`<background>` tags meant to be used alongside conditions to add a background image under certain conditions (e.g on a specific screen, for example).


### &lt;options&gt; tag ###

**Syntax**: `<options>[option1 - text], [...]`  
**Example**: `<options>disableSpriteLimit`

Currently, the only flag available is `disableSpriteLimit` which forces the emulator to disable the sprite limit when the HD pack is loaded.

### &lt;bgm&gt; tag ###

**Syntax**: `<bgm>[album - integer],[track - integer],[filename - ogg]`  
**Example**: `<bgm>0,0,myBgm.ogg`

Used to assign a background music track (`.ogg` audio file) to a specific album and track number.
Album and track numbers are used to form a unique ID for each bgm, allowing up to 64k different bgm tracks.

### &lt;sfx&gt; tag ###

**Syntax**: `<sfc>[album - integer],[track - integer],[filename - ogg]`  
**Example**: `<sfc>0,0,myBgm.ogg`

Used to assign a sound effect (`.ogg` audio file) to a specific album and track number.
Album and track numbers are used to form a unique ID for each sound effect, allowing up to 64k different sound effects.

## Using conditions ##

To use conditions, add the condition's name at the start of the line. e.g:  
`[myConditionName]<tile>...`

Conditions can only be applied to `<tile>` or `<background>` tags. When a condition is applied to a `<tile>` or `<background>` tag, that rule will only be used if the condition is met.

The first matching rule (in the order they are written in the `hires.txt` file) will be used. So conditional tiles MUST be placed before tiles with no conditions (for the same `tile data`+`palette data`) to have any effect.

You can also make it so multiple conditions must be met for a rule to be used by joining each condition name with a &:  
`[cond1&cond2]<tile>...`

## Replacing audio in games ##

Audio replacement in HD packs in Mesen works in a similar fashion to the MSU-1 for the SNES.  It adds a number of read/write registers in memory and they can be used to play OGG files specified via `<bgm>` and `<sfx>` tags.  

`TO BE COMPLETED`.

### $4100/$3002: Playback Options ###

### $4101/$3012: Playback Control ###

### $4102/$3022: BGM Volume ###

### $4103/$3032: SFX Volume ###

### $4104/$3042: Album Number ###

### $4105/$3052: Play BGM Track ###

### $4106/$3062: Play SFX Track ###

