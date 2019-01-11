---
title: Lua API reference
weight: 55
chapter: false
toc: false
---

This section documents the Mesen-specific Lua API that is available in scripts via the [script window](/debugging/scriptwindow.html).

## Changelog ##

Lua scripting is still a relatively recent feature and the API is not quite stable yet. To get a list of the major changes between different versions of Mesen, take a look at the [Changelog](/apireference/changelog.html).

## API References ##

* [Callbacks](/apireference/callbacks.html)
* [Drawing](/apireference/drawing.html)
* [Emulation](/apireference/emulation.html)
* [Input](/apireference/input.html)
* [Logging](/apireference/logging.html)
* [Memory Access](/apireference/memoryaccess.html)
* [Miscellaneous](/apireference/misc.html)
* [Enums](/apireference/enums.html)


## Test Runner Mode ##

Mesen can be started in a headless test runner mode that can be used to implement automated testing by using Lua scripts.  

To start Mesen in headless mode, use the `--testrunner` command line option and specify both a game and a Lua script to run:
```
Mesen.exe --testrunner MyGame.nes MyTest.lua
```

This will start Mesen (headless), load the game and the Lua script and start executing the game at maximum speed until the Lua script calls the <kbd>[emu.stop()](/apireference/emulation.html#stop)</kbd> function. The <kbd>[emu.stop()](/apireference/emulation.html#stop)</kbd> function can specify an exit code, which will be returned by the Mesen process, which can be used to validate whether the test passed or failed.