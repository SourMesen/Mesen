---
title: Input Options
weight: 5
chapter: false
---

## General Options ##

<div class="imgBox"><div>
	<img src="/images/InputOptions_General.png" />
	<span>Controller Options</span>
</div></div>

**Console Type**: Selects which console to emulate for all input ports. The `NES` and `Famicom` have different accessories and some of the identical accessories (e.g the Zapper) have different behavior on a hardware level.  If you want to connect Famicom-only accessories that plug into the Famicom's expansion port, select `Famicom`.

**Automatically configure controllers when loading a game**: When enabled, when loading any game recognized by Mesen's internal game database, the appropriate controllers will automatically be setup.  For example, loading up a game like Duck Hunt will connect a Zapper to the second port.

## Setting up controllers ##

### NES ###

For each player, select the device you want to use.  To connect more than 2 controllers, check the `Use Four Score accessory` option.  
To setup the key mappings for a controller (or other device-specific options), click the `Setup` button on the right of the player you want to configure.

### Famicom ###

On older Famicoms, the player 1 & 2 controllers are hard-wired and cannot be disconnected -- Mesen does the same.  To connect additional controllers, select the `Four Player Adapter` expansion device.  
To setup the key mappings for a controller (or other device-specific options), click the `Setup` button on the right of the player you want to configure.

## Configuring Key Bindings ##

<div class="imgBox"><div>
	<img src="/images/InputOptions_Controller.png" />
	<span>Controller Setup</span>
</div></div>

Each player can have up to four tabs of key bindings -- this allows you to control the same player with different input devices.  For example, you can setup player 1 to be controlled by your keyboard and your Xbox controller at the same time.

To select a binding for a button, click on the corresponding button in the UI and then press the key or gamepad button you want to use for this button.  
To clear a binding, click on it and then close the popup window by clicking on the X button.  
To clear all bindings for this tab, click on the `Clear Key Bindings`.

To simplify configuration, a number of presets are available -- click on the `Select Preset` button to choose one.

You can also configure that controller's turbo buttons' speed with the `Turbo Speed` slider. *Note: setting the turbo speed to the fastest setting may cause some games to not detect the button presses at all.*

## Advanced Options ##

<div class="imgBox"><div>
	<img src="/images/InputOptions_Advanced.png" />
	<span>Advanced Options</span>
</div></div>

**Hide mouse pointer when using zapper**: Hides the mouse pointer completely when a Zapper is connected. This is useful when using light guns (for PCs) that simulate a mouse.

### Display Controller Input ###

Use these options to display the controller input on the screen.  
{{% notice warning %}}
This will be recorded by the [Video Recorder](/tools.html#video-recorder) - so make sure you turn it off if you do not want it to appear on the video.
{{% /notice %}}

