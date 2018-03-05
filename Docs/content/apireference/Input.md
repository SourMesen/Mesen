---
title: Input
weight: 20
pre: ""
chapter: false
---

## getInput ##

**Syntax**  

    emu.getInput(port)

**Parameters**  
port - *Integer* The port number to read (0 to 3)

**Return value**  
*Table* A table containing the status of all 8 buttons. 

**Description**  
Returns a table containing the status of all 8 buttons: { a, b, select, start, up, down, left, right }   

## setInput ##

**Syntax**  

    emu.setInput(port, input)

**Parameters**  
port - *Integer* The port number to apply the input to (0 to 3)  
input - *Table* A table containing the state of some (or all) of the 8 buttons (same format as returned by [getInput](#getinput))

**Return value**  
*None* 

**Description**  
Buttons enabled or disabled via setInput will keep their state until the next *inputPolled* event.  
If a button's value is not specified to either true or false in the *input* argument, then the player retains control of that button.  For example, setInput(0, { select = false, start = false}) will prevent the player 1 from using both the start and select buttons, but all other buttons will still work as normal.
To properly control the emulation, it is recommended to use this function within a callback for the *inputPolled* event. Otherwise, the inputs may not be applied before the ROM has the chance to read them.

## getMouseState ##

**Syntax**  

    emu.getMouseState()

**Return value**  
*Table* The mouse's state 

**Description**  
Returns a table containing the position and the state of all 3 buttons: { x, y, left, middle, right }   

## isKeyPressed ##

**Syntax**  

    emu.isKeyPressed(keyName)

**Parameters**  
keyName - *String* The name of the key to check

**Return value**  
*Boolean* The key's state (true = pressed)

**Description**  
Returns whether or not a specific key is pressed. The "keyName" must be the same as the string shown in the UI when the key is bound to a button.
