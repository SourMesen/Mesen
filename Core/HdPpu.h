#pragma once
#include "stdafx.h"
#include "PPU.h"

struct HdScreenInfo;
struct HdPackData;
class ControlManager;
class Console;

class HdPpu : public PPU
{
private:
	HdScreenInfo *_screenInfo[2];
	HdScreenInfo *_info;
	uint32_t _version;
	HdPackData *_hdData = nullptr;

protected:
	void DrawPixel() override;

public:
	HdPpu(shared_ptr<Console> console, HdPackData* hdData);
	~HdPpu();

	void SendFrame() override;
};