#pragma once
#include "stdafx.h"
#include "BaseControlDevice.h"
#include "KeyManager.h"
#include "IKeyManager.h"
#include "PPU.h"
#include "MemoryManager.h"

class Zapper : public BaseControlDevice
{
protected:
	bool HasCoordinates() override { return true; }

	string GetKeyNames() override
	{
		return "F";
	}

	enum Buttons { Fire };

	void InternalSetStateFromInput() override
	{
		if(_console->GetSettings()->InputEnabled()) {
			SetPressedState(Buttons::Fire, KeyManager::IsMouseButtonPressed(MouseButton::LeftButton));
		}

		MousePosition pos = KeyManager::GetMousePosition();
		if(KeyManager::IsMouseButtonPressed(MouseButton::RightButton)) {
			pos.X = -1;
			pos.Y = -1;
		}
		SetCoordinates(pos);
	}

	bool IsLightFound()
	{
		return StaticIsLightFound(GetCoordinates(), _console);
	}

public:
	Zapper(shared_ptr<Console> console, uint8_t port) : BaseControlDevice(console, port)
	{
	}

	uint8_t ReadRAM(uint16_t addr) override
	{
		uint8_t output = 0;
		if((IsExpansionDevice() && addr == 0x4017) || IsCurrentPort(addr)) {
			output = (IsLightFound() ? 0 : 0x08) | (IsPressed(Zapper::Buttons::Fire) ? 0x10 : 0x00);
		}
		return output;
	}

	void WriteRAM(uint16_t addr, uint8_t value) override
	{
	}

	static bool StaticIsLightFound(MousePosition pos, shared_ptr<Console> console)
	{
		PPU* ppu = console ? console->GetPpu() : nullptr;
		if(!ppu) {
			return false;
		}

		int32_t scanline = ppu->GetCurrentScanline();
		int32_t cycle = ppu->GetCurrentCycle();
		int radius = (int)console->GetSettings()->GetZapperDetectionRadius();

		if(pos.X >= 0 && pos.Y >= 0) {
			for(int yOffset = -radius; yOffset <= radius; yOffset++) {
				int yPos = pos.Y + yOffset;
				if(yPos >= 0 && yPos < PPU::ScreenHeight) {
					for(int xOffset = -radius; xOffset <= radius; xOffset++) {
						int xPos = pos.X + xOffset;
						if(xPos >= 0 && xPos < PPU::ScreenWidth) {
							if(scanline >= yPos && (scanline - yPos <= 20) && (scanline != yPos || cycle > xPos) && ppu->GetPixelBrightness(xPos, yPos) >= 85) {
								//Light cannot be detected if the Y/X position is further ahead than the PPU, or if the PPU drew a dark color
								return true;
							}
						}
					}
				}
			}
		}

		return false;
	}
};