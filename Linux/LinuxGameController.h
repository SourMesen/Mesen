#pragma once
#include <thread>
#include <atomic>

struct libevdev;

class LinuxGameController
{
private:
	int _fd = -1;
	int _deviceID = -1;
	libevdev *_device = nullptr;
	bool _disconnected = false;
	std::thread _eventThread;
	std::atomic<bool> _stopFlag;
	int _axisDefaultValue[0x100];

	LinuxGameController(int deviceID, int fileDescriptor, libevdev *device);
	bool CheckAxis(unsigned int code, bool forPositive);
	void Calibrate();	

public:
	~LinuxGameController();

	static std::shared_ptr<LinuxGameController> GetController(int deviceID);

	bool IsDisconnected();
	int GetDeviceID();
	bool IsButtonPressed(int buttonNumber);
};