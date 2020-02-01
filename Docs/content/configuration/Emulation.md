---
title: Emulation Options
weight: 8
chapter: false
---

## General Options ##

<div class="imgBox"><div>
	<img src="/images/EmulationSettings_General.png" />
	<span>General Options</span>
</div></div>

{{% notice tip %}}
Set any speed value below to 0 to make Mesen run as fast as possible.
{{% /notice %}}

**Emulation Speed**: This configures the regular speed to use when emulating. This should normally be set to `100%`.

**Fast Forward Speed**: This is the alternate speed that is used when the [Fast Forward button](/configuration/preferences.html#shortcut-keys) is held down.

**Rewind Speed**: This configures the speed at which to rewind the gameplay when the [Rewind button](/configuration/preferences.html#shortcut-keys) is held down.

**Run Ahead**: Run ahead allows the reduction of input lag by the number of frames specified. CPU requirements increase proportionally with the number of run ahead frames specified.  

 * Run ahead is currently not compatible with movies or netplay - the movies and netplay menus will be disabled if runahead is turned on.
 * **Note for speedrunners:** Using features such as run ahead to reduce lag typically counts as cheating for the purposes of speed running.

## Advanced Options ##

<div class="imgBox"><div>
	<img src="/images/EmulationSettings_Advanced.png" />
	<span>Advanced Options</span>
</div></div>

### Recommended settings for developers (homebrew / ROM hacking)

{{% notice tip %}}
When developing software for the NES, enabling these options can help you catch bugs that would otherwise be invisible in most emulators and only show up on real hardware. Setting the power on state for RAM to **random values** is also recommended.
{{% /notice %}}

**Enable OAM RAM decay**: On all models, OAM RAM decays whenever rendering is disabled. This causes the values in OAM RAM to decay to a specific value after a certain amount of time has elapsed since the last time the value was read or written (which may cause sprite-related glitches to appear on the screen). No known game relies on this -- the option is offered here mostly for the sake of homebrew software testing. There is a corresponding option to break on decayed OAM reads available in the debugger to help find and debug OAM decay-related bugs.

**Randomize power-on state for mappers**: Cartridges often have a random state at power-on and need to be fully initialized before being used. This option causes Mesen to randomize the power-on state of the most common mappers. This is useful when developing homebrew software.

**Randomize power-on/reset CPU/PPU alignment**: Each time the NES is reset, the CPU and PPU run with a slightly different and random alignment. This option simulates that behavior. When enabled, some test roms may randomily fail from one reset to another, because of the different alignments (this can also happen on the NES.)

**Enable PPU $2006 scroll glitch emulation**: When enabled, a known hardware bug that occurs when the $2006 register is written at specific cycles during rendering. This glitch is known to occur in some licensed NES games and usually results in a single frame displaying with an incorrect scroll position.

**Enable PPU $2000/$2005/$2006 first-write scroll glitch emulation**: When enabled, a known hardware bug that occurs on the first write to the $2005/$2006 registers (or any write to the $2000 register) when the write occurs at cycle during rendering. This glitch is known to occur in some licensed NES games and usually results in a single scanline displaying incorrectly.

**Default power on state for RAM**: On a console, the RAM's state at power on is undetermined and relatively random. This option lets you select Mesen's behavior when initializing RAM - set all bits to 0, set all bits to 1, or randomize the value of each bit.

### Miscellaneous settings ###

{{% notice warning %}}
Several options in this section should NOT be enabled to avoid issues in some games -- they are available here stricly for the sake of completeness (and testing homebrew software, etc.). These options are marked with the `(not recommended)` tag in the UI.
{{% /notice %}}

**Use alternative MMC3 IRQ behavior**: The MMC3 has a number of different variants (A, B and C).  By default, Mesen uses the IRQ behavior for versions B and C.  By turning this option on, Mesen will default to using the MMC3A's IRQ behavior instead. There is usually no reason to enable this.

**Use NES/HVC-101 (Top-loader / AV Famicom) behavior**: The NES and Famicom both had 2 different releases - their original model and the "top loader" model.  Both of these have slightly different behavior when it comes to their input ports.  When enabled, this option causes Mesen to simulate the top loader models.  No games are known to rely on this behavior.

**Do not reset PPU when resetting console**: On the Famicom and top loader NES, the PPU does not reset when pressing the reset button (only the CPU is reset). When enabled, only the CPU resets when the reset button is pressed.

**Disable PPU $2004 reads**: On some early models, the OAM RAM cannot be read via the $2004 register (in this case, $2004 becomes a write-only register). When enabled, this option emulates this behavior.

**Disable PPU OAMADDR bug emulation**: On some models, a bug occurs that corrupts OAM RAM under certain circumstances. When this option is enabled, the bug is no longer emulated. This bug is required for at least 1 game to work properly.

**Disable PPU palette reads**: On some early models, it is not possible to read the palette RAM via $2007 -- when enabled, this option emulates this behavior, making reads to palette RAM return corresponding values in the PPU's memory instead.

**Allow invalid input**: On a NES controller, it is impossible to press both left and right or up and down at the same time on the controller's D-pad.  Some games rely on this and pressing both buttons at once can cause glitches.  When enabled, this option makes it possible to press opposite directional buttons at the same time.


## Overclocking ##

{{% notice warning %}}
Overclocking can cause issues in some games. The safest way to overclock is to increase the `Additional scanlines before NMI` option and leave the other options to their default values.
{{% /notice %}}

<div class="imgBox"><div>
	<img src="/images/EmulationSettings_Overclocking.png" />
	<span>Overclocking Options</span>
</div></div>

**Additional scanlines before NMI**: Increases the number of scanlines in the PPU, *before* the NMI signal is triggered at the end of the visible frame. This effectively gives more time for games to perform calculations, which can reduce slowdowns in games. **This is the preferred option for overclocking.**

**Additional scanlines after NMI**: Increases the number of scanlines in the PPU, *after* the NMI signal is triggered at the end of the visible frame. This effectively gives more time for games to perform calculations, which can reduce slowdowns in games. **This option is less compatible and should only be used if the `before NMI` variation does not work as expected.**

**Show Lag Counter**: When enabled, the lag counter is displayed on the screen. The lag counter keeps track of frames where the game does not attempt to read the input ports -- this is usually an indication of the game running out of time to perform calculations, which usually causes slowdowns.