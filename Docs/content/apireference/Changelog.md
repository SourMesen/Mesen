---
title: Changelog
weight: 1
pre: ""
chapter: false
---

## Changes between 0.9.4 and 0.9.5 ##

### New features ###

* New functions: 

	* **<kbd>[getScreenBuffer](/apireference/drawing.html#getscreenbuffer)</kbd>**
	* **<kbd>[setScreenBuffer](/apireference/drawing.html#setscreenbuffer)</kbd>**
	* **<kbd>[getAccessCounters](/apireference/misc.html#getaccesscounters)</kbd>**
	* **<kbd>[resetAccessCounters](/apireference/misc.html#resetaccesscounters)</kbd>**

* New enums: 
	
	* **<kbd>[counterMemType](/apireference/enums.html#countermemtype)</kbd>**
	* **<kbd>[counterOpType](/apireference/enums.html#counteroptype)</kbd>**
	
## Changes between 0.9.3 and 0.9.4 ##

### New features ###

* New functions: 

	* **<kbd>[getLogWindowLog](/apireference/misc.html#getlogwindowlog)</kbd>**
	* **<kbd>[getRomInfo](/apireference/misc.html#getrominfo)</kbd>**
	* **<kbd>[getScriptDataFolder](/apireference/misc.html#getscriptdatafolder)</kbd>**
	* **<kbd>[isKeyPressed](/apireference/input.html#iskeypressed)</kbd>**
	* **<kbd>[clearSavestateData](/apireference/misc.html#clearsavestatedata)</kbd>**
	* **<kbd>[getSavestateData](/apireference/misc.html#getsavestatedata)</kbd>**	
	* **<kbd>[loadSavestateAsync](/apireference/misc.html#loadsavestateasync)</kbd>**
	* **<kbd>[saveSavestateAsync](/apireference/misc.html#savesavestateasync)</kbd>**
	
* New event callbacks: [**<kbd>inputPolled</kbd>**, **<kbd>stateLoaded</kbd>**, **<kbd>stateSaved</kbd>**](/apireference/enums.html#eventtype)
* New memory types: [**<kbd>cpuDebug</kbd>**, **<kbd>ppuDebug</kbd>**](/apireference/enums.html#memtype)

### Breaking changes ###
* Removed the **<kbd>debugRead</kbd>**, **<kbd>debugReadWord</kbd>**, **<kbd>debugWrite</kbd>** and **<kbd>debugWriteWord</kbd>** functions.  They have been replaced by the [memType.cpuDebug](/apireference/enums.html#memtype) and [memType.ppuDebug](/apireference/enums.html#memtype) enum values.
* The behavior of the **<kbd>[setInput](/apireference/input.html#setinput)</kbd>** function has changed.
* The return values for the APU portion of the **<kbd>[getState](/apireference/emulation.html#getstate)</kbd>** function has slightly changed.