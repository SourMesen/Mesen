-----------------------
-- Name: Game Boy Mode
-- Author: upsilandre
-----------------------
-- Simulates the game boy's display limitations
-- 1. Adds a 160x144 frame which can be moved by clicking inside it.  Clicking outside of it will hide the black overlay.
-- 2. Restricts the palette to a 4-color palette like the game boy - right-click the screen to change the palette.
-----------------------

function Main()
  mouse = emu.getMouseState()
  if mouse.left then
    if stencil then
      if mouse.x > window_X and mouse.x < window_X + 160 and mouse.y > window_Y and mouse.y < window_Y + 144 then
        window_X = mouse.x - 80
        window_Y = mouse.y - 72
      else
        stencil = false
        holdLeft = true
      end
    else
      if not holdLeft then
        holdLeft = true
        stencil = true
        window_X = mouse.x - 80
        window_Y = mouse.y - 72  
      end  
    end  
  else
    holdLeft = false      
  end
  
  if mouse.right then
    if not holdRight then
      holdRight = true    
      colorB = color1
      color1 = color2
      color2 = color3
      color3 = color4
      color4 = colorB
      cycleP = cycleP + 1
      if cycleP == 4 then
        cycleP = 0
        colorB = color5
        color5 = color6
        color6 = color7
        color7 = color8
        color8 = colorB
      end
    end  
  else
    holdRight = false    
  end

  if stencil then
    if window_X > 0 then  
      emu.drawRectangle(0, window_Y, window_X, 144, 0x000000, true) 
    end
    if window_X < 96 then  
      emu.drawRectangle(window_X + 160, window_Y, 96 - window_X, 144, 0x000000, true) 
    end
    if window_Y > 0 then
      emu.drawRectangle(0, 0, 256, window_Y, 0x000000, true) 
    end
    if window_Y < 96 then    
    emu.drawRectangle(0, window_Y + 144, 256, 96 - window_Y, 0x000000, true)  
    end
  end
  
  for x = 0,0x0f,4 do
    emu.write(x, color1, emu.memType.palette)
    emu.write(x + 1, color2, emu.memType.palette)
    emu.write(x + 2, color3, emu.memType.palette)
    emu.write(x + 3, color4, emu.memType.palette)
    emu.write(x + 16, color1, emu.memType.palette)
    emu.write(x + 17, color6, emu.memType.palette)
    emu.write(x + 18, color7, emu.memType.palette)
    emu.write(x + 19, color8, emu.memType.palette)      
  end
end  

window_X = 48
window_Y = 48
cycleP = 0
colorB = 0
color1 = 0x38
color2 = 0x28
color3 = 0x18
color4 = 0x08
color5 = 0x38
color6 = 0x28
color7 = 0x18
color8 = 0x08
stencil = true
holdLeft = false
holdRight = false
emu.addEventCallback(Main, emu.eventType.startFrame)
emu.displayMessage("Script", "GameBoy mode")
