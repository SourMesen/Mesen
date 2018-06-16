---
title: Video Options
weight: 6
chapter: false
---

## General Options ##

<div class="imgBox"><div>
	<img src="/images/VideoOptions_General.png" />
	<span>General Options</span>
</div></div>

**Scale**: The scale determines the emulator window's size - use integer factors (e.g: 2x, 3x, 4x) for best results.

**Aspect Ratio**: The NES' internal aspect ratio is almost square (`Default (No Stretching)`), but it used to be displayed on CRT TVs that had a rectangular picture. To simulate a CRT TV, you can use the `Auto` option - it will switch between NTSC and PAL aspect ratios depending on the game you are playing. Using anything other than the `Default (No Stretching)` option may cause pixels to have irregular sizes. You can reduce this effect by using a combination of video filters and the bilinear filtering option.

**Enable integer FPS mode**: Under normal conditions, the NTSC NES runs at 60.1 fps. When playing a 60hz LCD, this causes a lot of dropped frames. This option slows down the emulation by a tiny amount to produce 60 frames per second instead, which reduces the number of dropped frames.

**Enable vertical sync**: Turns on vertical sync -- can help prevent screen tearing on some hardware configurations.

**Use exclusive fullscreen mode**: Turns on exclusive fullscreen mode. This may be useful if you are experiencing screen tearing issues in regular fullscreen despite vertical sync being turned on.

**Requested Refresh Rate**: This option is shown only when exclusive fullsceen mode is enabled. It allows you to select your preferred refresh rate when running in exclusive fullscreen mode.

**Use integer scale values when entering fullscreen mode**: By default, fullscreen mode fills the entire screen. However, this can cause non-integer scaling values to be used -- for example, in 1080p resolution, the scale becomes 4.5x. Since this can cause irregularly shaped pixels, you can use this option to use the nearest integer scale value instead (e.g 4x in this example).

**Use HDNes HD packs**: Enables the use of [HD packs](/hdpacks.html).

**Show FPS**: Displays an FPS counter on the screen.  The first number is the number of frames emulated, the second number is the number of frames displayed on the screen.  These values are usually identical, except when vertical sync is enabled.

## Picture ##

<div class="imgBox"><div>
	<img src="/images/VideoOptions_Picture.png" />
	<span>Picture Options</span>
</div></div>

**Filter**: Allows you to select a video filter. Selecting NTSC filters will cause additional configuration options to appear below.

### Common Options ###

The `Brightness`, `Contrast`, `Hue`, `Saturation`, `Scanline` settings are common to all filters and can even be used without a filter.

**Use bilinear interpolation when scaling**: When enabled, bilinear interpolation is used when stretching (due to scale or aspect ratio). When disabled, nearest neighbor scaling is used. An easy way to get a slightly-softened screen, for example, is to use the `Prescale` filters (which use nearest neighbor scaling), use a bigger scale and enable bilinear filtering. For example, try this configuration:
```text
  Filter: Prescale 3x
  Scale: 4x
  Use bilinear interpolation when scaling: Enabled
```

**Scanlines**: Simulates the scanlines on a CRT TV - the higher the value, the deeper the scanlines appear on the screen.

### NTSC Filter Options ###

There are 2 separate NTSC filters implemented in Mesen.  The `NTSC` filter is blargg's implementation - this filter is very fast, and available in various other emulators. The `NTSC (bisqwit)` filter is an implementation of bisqwit's NTSC filter -- it is slower and produces a different output.

The 2 filters have a different set of options:

**NTSC (blargg)**: `Artifacts`, `Bleed`, `Fringing`, `Gamma`, `Resolution`, `Sharpness`  
**NTSC (bisqwit)**: `Y Filter (Horizontal Blur)`, `I Filter (Horizontal Blur)`, `Q Filter (Horizontal Bleed)`  

Feel free to experiment with the settings and choose what you feel looks best.

## Overscan ##

<div class="imgBox"><div>
	<img src="/images/VideoOptions_Overscan.png" />
	<span>Overscan Options</span>
</div></div>

The overscan settings allow you to cut out pixels on any edge of the screen.  On a CRT TV, a few pixels on each side of the screen was usually invisible to the player.  Because of this, games often have glitches or incorrect palette colors on the edges of the screen -- this is normal and caused by the game itself. Setting a value of 8 or so on each side of the overscan configuration will usually hide most glitches.

It is possible to configure the overscan settings on a per-game basis in the *Game-Specific* tab. All games without specific settings will automatically use the overscan parameters shown in the *Global* tab.

## Palette ##

<div class="imgBox"><div>
	<img src="/images/VideoOptions_Palette.png" />
	<span>Palette Options</span>
</div></div>

This tab allows you to customize the palette used by all games.

{{% notice tip %}}
Click on any color in the palette to manually change its color.
{{% /notice %}}

**Load Preset Palette**: Mesen comes with a number of built-in palette options - you can select them from here.

**Load Palette File**: Use this to load a .pal file into the emulator.

**Export Palette**: Use this to export your current palette into a .pal file.

**Use this palette for VS System games**: By default, VS System games have their own predefined RGB palettes and don't use the palette defined here.  When enabled, this option forces VS System games to ignore their default palette and use this one instead.

## Advanced Options ##

<div class="imgBox"><div>
	<img src="/images/VideoOptions_Advanced.png" />
	<span>Advanced Options</span>
</div></div>

{{% notice warning %}}
These options should not be used if you are looking for accurate emulation.
{{% /notice %}}

**Screen Rotation**: Rotates the display by the specified angle. This is useful to play games (generally homebrew games) designed for a vertical display.

**Disable background**: Disables rendering of the background layer.

**Disable sprite**: Disables rendering of all sprites.

**Force background display in first column**: The NES has a flag that prevents the background from rendering in the first 8 pixels on the left of the screen. When enabled, this option forces the background to be rendered in the first 8 pixels, no matter what the flag's value is.

**Force sprite display in first column**: The NES has a flag that prevents sprites from rendering in the first 8 pixels on the left of the screen. When enabled, this option forces the sprites to be rendered in the first 8 pixels, no matter what the flag's value is.