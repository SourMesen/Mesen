---
title: Script Window
weight: 16
chapter: false
---

<div class="imgBox"><div>
	<img src="/images/ScriptWindow.png" />
	<span>Script Window</span>
</div></div>

The Script Window allows [Lua](https://www.lua.org/) scripting via a [Mesen-specific API](/apireference.html). Using this API, you can interact with the emulation core to perform a variety of things (e.g: logging, drawing, implementing an AI).

The code editor contains an autocomplete feature for all of Mesen's API -- typing `emu.` will display an autocomplete popup displaying all available functions.  Select a function in the list to see its parameters, return value and description.  
Multiple script windows can be opened at once to combine the effects of several scripts together.  

Scripts can be loaded from the disk and edited in any other text editor.  When using an external editor to modify the script, it is suggested to turn on the **<kbd>Script&rarr;Auto-reload when file changes</kbd>** option to automatically reload the script each time it is saved to the disk.

To start a script, press **<kbd>F5</kbd>**.  
To stop a script, press **<kbd>Escape</kbd>** or close the script window.

The Log Window at the bottom will display any Lua errors that occur while running the script.
Additionally, any calls to `emu.log()` will also display their content in the log window.