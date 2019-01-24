#include "stdafx.h"
#include "DebuggerTypes.h"

struct PerfTrackerData
{
	int frameCount = 0;
	int prevFrameCount = 0;
	bool frameProcessed = false;
	int fps = 60;
	int totalFrame = 1;
	int gameFrame = 1;
	
	int isLagFramePos = 0;
	bool isLagFrame[60] = {};
	
	int fpsChartPos = 0;
	int fpsChartDataPoints[256] = {};
	int totalCpu = 50;
	int cpuEntry = 1;
	int partialCpu = 0;
	int updateCpu = 50;
	int cpuChartPos = 0;
	int cpuChartDataPoints[256] = {};
};

class PerformanceTracker
{
private:
	shared_ptr<Console> _console;
	PerfTrackerData _data;

	int32_t _address = -1;
	AddressType _type = AddressType::InternalRam;
	PerfTrackerMode _mode = PerfTrackerMode::Disabled;
	bool _needReset = false;

	void DrawChart(int *dataPoints, int startPos, int color, int scale, int maxValue);

public:
	PerformanceTracker(shared_ptr<Console> console);
	void Initialize(int32_t address, AddressType type, PerfTrackerMode mode);
	PerfTrackerMode GetMode();
	void ProcessEndOfFrame();
	void ProcessCpuExec(AddressTypeInfo &addressInfo);
};
