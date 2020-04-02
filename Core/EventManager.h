#pragma once
#include "stdafx.h"
#include "Types.h"
#include "../Utilities/SimpleLock.h"

enum class DebugEventType : uint8_t;
struct DebugEventInfo;
struct EventViewerDisplayOptions;
class CPU;
class PPU;
class EmulationSettings;
class Debugger;

class EventManager
{
private:
	CPU *_cpu;
	PPU *_ppu;
	EmulationSettings *_settings;
	Debugger *_debugger;
	vector<DebugEventInfo> _debugEvents;
	vector<DebugEventInfo> _prevDebugEvents;
	vector<DebugEventInfo> _sentEvents;

	vector<DebugEventInfo> _snapshot;
	uint16_t _snapshotScanline = 0;
	uint16_t _snapshotCycle = 0;
	SimpleLock _lock;

	uint32_t _scanlineCount = 262;
	uint16_t *_ppuBuffer = nullptr;

	void DrawEvent(DebugEventInfo &evt, bool drawBackground, uint32_t *buffer, EventViewerDisplayOptions &options);
	void DrawDot(uint32_t x, uint32_t y, uint32_t color, bool drawBackground, uint32_t* buffer);
	void DrawNtscBorders(uint32_t *buffer);
	void DrawPixel(uint32_t *buffer, int32_t x, uint32_t y, uint32_t color);

public:
	EventManager(Debugger *debugger, CPU *cpu, PPU *ppu, EmulationSettings *settings);
	~EventManager();

	void AddSpecialEvent(DebugEventType type);
	void AddDebugEvent(DebugEventType type, uint16_t address = -1, uint8_t value = 0, int16_t breakpointId = -1, int8_t ppuLatch = -1);

	void GetEvents(DebugEventInfo *eventArray, uint32_t &maxEventCount, bool getPreviousFrameData);
	uint32_t GetEventCount(bool getPreviousFrameData);
	void ClearFrameEvents();

	uint32_t TakeEventSnapshot(EventViewerDisplayOptions options);
	void GetDisplayBuffer(uint32_t *buffer, EventViewerDisplayOptions options);

	DebugEventInfo GetEvent(int16_t scanline, uint16_t cycle, EventViewerDisplayOptions &options);
};

struct EventViewerDisplayOptions
{
	uint32_t IrqColor;
	uint32_t NmiColor;
	uint32_t DmcDmaReadColor;
	uint32_t SpriteZeroHitColor;
	uint32_t BreakpointColor;
	uint32_t MapperRegisterReadColor;
	uint32_t MapperRegisterWriteColor;

	uint32_t PpuRegisterReadColors[8];
	uint32_t PpuRegisterWriteColors[8];

	bool ShowMapperRegisterWrites;
	bool ShowMapperRegisterReads;

	bool ShowPpuRegisterWrites[8];
	bool ShowPpuRegisterReads[8];

	bool ShowNmi;
	bool ShowIrq;
	bool ShowDmcDmaReads;
	bool ShowSpriteZeroHit;

	bool ShowMarkedBreakpoints;
	bool ShowPreviousFrameEvents;
	bool ShowNtscBorders;
};