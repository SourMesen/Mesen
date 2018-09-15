---
title: HD Packs
weight: 5
chapter: false
---

HD Packs make it possible to replace a game's graphics and audio with high definition alternatives. This can be used in many ways, for example, one could keep the game's original resolution and simply improve its graphics by adding more colors and shading.

## Using HD packs ##

### Installing HD Packs ###

To install an HD Pack:

* First, load the game for which you want to install the HD Pack.
* Then, click on the **<kbd>Tools&rarr;Install HD Pack</kbd>** menu and select the `.zip` file that contains the HD Pack you want to install.
* A message will be shown indicating whether the installation succeeded or failed.

### Manual installation ###

If the HD Pack installation tool fails to install the HD Pack, you can try to install it manually with these instructions:  

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

The following are the specifications for the hires.txt file, as of version "103".

### &lt;ver&gt; tag ###

**Syntax**: `<ver>[integer]`  
**Example**: `<ver>103`

The format's version number (currently 103).

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

HD Packs support a number of conditionals that can be used to replace graphics when certain conditions are met.  
This is useful to resolve conflicts that can occur in some games (where the same tile & palette can be reused in multiple distinct objects), etc.  
The sections below describe every available condition type.

#### Built-in Conditions ####

A number of built-in conditions can be used to check the value of some flags:  

* `hmirror`: True if the current pixel is a sprite pixel, and the sprite is mirrored horizontally.
* `vmirror`: True if the current pixel is a sprite pixel, and the sprite is mirrored vertically.
* `bgpriority`: True if the current pixel is a sprite pixel, and the sprite is marked as a background priority sprite.

**Example:** `[hmirror]<tile>...`

#### tileNearby / spriteNearby ####

The tileNearby and spriteNearby conditions are used to check whether a specific tile or sprite exists in the vicinity of the current pixel. If a matching tile/sprite is found, the condition will be true.  

**Syntax**: `<condition>[name - text], [conditionType - text], [x value - integer], [y value - integer], [tile data], [palette data - hex]`  
**Example (CHR ROM)**: `<condition>myCondition,tileNearby,8,0,10,0F100017`  
**Example (CHR RAM)**: `<condition>myCondition,tileNearby,8,0,D2C2C2C7CF2FFEFC2C3C3C3830D00000,0F100017`

**Notes**:  
`tileNearby` and `spriteNearby` use positive or negative X/Y offsets to the current position. e.g:  
`<condition>myCondition2,tileNearby,-8,0,[tile data],[palette data]`  
In this case, `myCondition2` will be true if the tile 8 pixels to the left of the current tile matches the tile+palette data specified.

For CHR ROM games, `tile data` is an integer representing the position of the original tile in CHR ROM.  
For CHR RAM games, `tile data` is a 32-character hexadecimal string representing all 16 bytes of the tile.  
`palette data` is always an 8-character hexadecimal string representing all 4 bytes of the palette used for the tile.  For sprites, the first byte is always "FF".

#### tileAtPosition / spriteAtPosition ####

The tileAtPosition and spriteAtPosition conditions are used to check whether a specific tile or sprite exists at the specified coordinates. If a matching tile/sprite is found, the condition will be true.  

**Syntax**: `<condition>[name - text], [conditionType - text], [x value - integer], [y value - integer], [tile data], [palette data - hex]`  
**Example (CHR ROM)**: `<condition>myCondition,tileAtPosition,8,0,10,0F100017`  
**Example (CHR RAM)**: `<condition>myCondition,tileAtPosition,8,0,D2C2C2C7CF2FFEFC2C3C3C3830D00000,0F100017`

**Notes**:  
`tileAtPosition` and `spriteAtPosition` use the X/Y parameters as screen coordinates. e.g:  
`<condition>myCondition,tileAtPosition,10,10,[tile data],[palette data]`  
In this case, `myCondition` will be true if the tile at position 10,10 on the NES' screen (256x240 resolution) matches the tile+palette data given.

For CHR ROM games, `tile data` is an integer representing the position of the original tile in CHR ROM.  
For CHR RAM games, `tile data` is a 32-character hexadecimal string representing all 16 bytes of the tile.  
`palette data` is always an 8-character hexadecimal string representing all 4 bytes of the palette used for the tile.  For sprites, the first byte is always "FF".

#### memoryCheck / ppuMemoryCheck ####

The memoryCheck and ppuMemoryCheck conditions are used to compare the value stored at 2 different memory addresses together. (Use the `ppuMemoryCheck` variant to check PPU memory)

**Syntax**: `<condition>[name - text], [conditionType - text], [memory address 1 - hex], [operator - string], [memory address 2 - hex]`  
**Supported operators**: `==`, `!=`, `>`, `<`, `>=`, `<=`  
**Example**: `<condition>myCondition,memoryCheck,8FFF,>,8000` (If the value stored at $8FFF is greater than the value stored at $8000, the condition will be true)

#### memoryCheckConstant / ppuMemoryCheckConstant ####

The memoryCheck and ppuMemoryCheck conditions are used to compare the value stored at a memory address with a constant.  (Use the `ppuMemoryCheckConstant` variant to check PPU memory)

**Syntax**: `<condition>[name - text], [conditionType - text], [memory address - hex], [operator - string], [constant - hex]`  
**Supported operators**: `==`, `!=`, `>`, `<`, `>=`, `<=`  
**Example**: `<condition>myCondition,memoryCheck,8FFF,==,3F` (If the value stored at $8FFF is equal to $3F, the condition will be true)

#### frameRange ####

The frameRange conditions can be used to conditionally replace tiles based on the current frame number.
The condition is true when the following expression is true:  
`[current frame number] % [divisorValue] >= [compareValue]`

**Syntax**: `<condition>[name - text], frameRange, [divisorValue - integer], [compareValue - integer]`  
**Example**: `<condition>myCondition,frameRange,8,10` (This condition will be true for the last 2 frames out of every 10 frames)


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

**Syntax**: `<background>[name - text], [brightness level - 0.0 to 1.0], [horizontal scroll ratio (optional) - float], [vertical scroll ratio (optional) - float], [showBehindBackgroundPrioritySprites (optional) - Y or N]`  
**Example**: `<background>myBackground.png,1.0,0,0,N`

`<background>` tags meant to be used alongside conditions to add a background image under certain conditions (e.g on a specific screen, for example).

The `Horizontal Scroll Ratio` and `Vertical Scroll Ratio` parameters are optional (their default value is `0.0`) and can be used to specify at what speed the background picture should scroll compared to the NES' scrolling.  
This can be used to create simple parallax scrolling effects.

When the `Show Behind Background Priority Sprites` parameter is enabled (`Y`), the background priority sprites will be shown in front of the background image.

### &lt;options&gt; tag ###

**Syntax**: `<options>[option1 - text], [option2 - text], [...]`  
**Example**: `<options>disableSpriteLimit`

**Available options**:  
`disableSpriteLimit`: Forces the emulator to disable the sprite limit when the HD pack is loaded.  
`disableContours`: Disables the outline effect that appears around sprites/tiles when using the `<background>` feature.  

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

It is also possible to invert the result of a condition by prepending a an exclamation mark (`!`) to it:  
`[!myCondition]<tile>...`

## Replacing audio in games ##

Audio replacement in HD packs in Mesen works in a similar fashion to the MSU-1 for the SNES.  It adds a number of read/write registers in memory and they can be used to play OGG files specified via `<bgm>` and `<sfx>` tags.  

### Register Write Operations ###

#### $4100: Playback Options ####

This register allows you to set the BGM loop flag (bit 0) on or off.

#### $4101: Playback Control ####

This register allows you to pause/resume or stop sounds.

**Bit 0**: When set, pauses/resumes the BGM track.  
**Bit 1**: When set, the BGM track is stopped.  
**Bit 2**: When set, all SFX tracks are stopped.  

#### $4102: BGM Volume ####

Sets the current volume for the BGM tracks (0: Muted, 255: Maximum volume).  
Setting this register affects the currently playing BGM track - this can be used for fade in/out effects.

#### $4103: SFX Volume ####

Sets the current volume for the SFX tracks (0: Muted, 255: Maximum volume).  
Setting this register affects all currently playing SFX - this can be used for fade in/out effects.

#### $4104: Album Number ####

Selects the current album (0 - 255).  
This allows up to 65536 BGM and SFX tracks to be defined.  
Writing to this register has no immediate effect - only subsequent writes to the `Play BGM Track` and `Play SFX Track` registers will be affected.

#### $4105: Play BGM Track ####

Starts playback of the specified BGM track from the specified album (based on the `Album Number` register).  
Only a single BGM track can be playing at any given time.

#### $4106: Play SFX Track ####

Starts playback of the specified SFX track from the specified album (based on the `Album Number` register).  
Any number of SFX tracks can be played at once.

### Register Read Operations ###

#### $4100: Status ####

This register returns the current playback status.  

**Bit 0**: Set when a BGM track is currently playing.  
**Bit 1**: Set when at least one SFX track is currently playing.  
**Bit 2**: Set if an error occurred (e.g the last play BGM/SFX failed because the specified Album+Track number combination was invalid)

#### $4101: Revision Number ####

Returns the current revision of the HD Audio API.  This value is currently set to `1`.

#### $4102/$4103/$4104: Signature ####

These registers return the ASCII string `NEA` (NES Enhanced Audio) - this can be used to detect whether or not the audio API is available.

## File Format Changelog ##

### Version 103 ###

* Added a `Mask` parameter to the memoryCheck and memoryCheckConstant conditions

### Version 102 ###

* Operands for memoryCheck/memoryCheckConstant conditions must now be specified in hex (used to be decimal)
* Added the `Show Behind Background Priority Sprites` option to the `<background>` tag
