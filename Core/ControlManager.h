#pragma once

#include "stdafx.h"
#include "../Utilities/SimpleLock.h"
#include "IMemoryHandler.h"
#include "Snapshotable.h"

class BaseControlDevice;
class Zapper;
class SystemActionManager;
class IInputRecorder;
class IInputProvider;
struct ControlDeviceState;
enum class ControllerType;
enum class ExpansionPortDevice;

class ControlManager : public Snapshotable, public IMemoryHandler
{
private:
	static ControlManager* _instance;
	static vector<IInputRecorder*> _inputRecorders;
	static vector<IInputProvider*> _inputProviders;
	static SimpleLock _deviceLock;

	//Static so that power cycle does not reset its value
	static uint32_t _pollCounter;

	vector<shared_ptr<BaseControlDevice>> _controlDevices;

	shared_ptr<BaseControlDevice> _systemActionManager;
	shared_ptr<BaseControlDevice> _mapperControlDevice;


	uint32_t _lagCounter = 0;
	bool _isLagging = false;

	uint8_t GetOpenBusMask(uint8_t port);
	void RegisterControlDevice(shared_ptr<BaseControlDevice> controlDevice);

protected:
	virtual void StreamState(bool saving) override;
	virtual ControllerType GetControllerType(uint8_t port);

public:

	ControlManager(shared_ptr<BaseControlDevice> systemActionManager, shared_ptr<BaseControlDevice> mapperControlDevice);
	virtual ~ControlManager();

	void UpdateControlDevices();
	void UpdateInputState();

	uint32_t GetLagCounter();
	void ResetLagCounter();

	static uint32_t GetPollCounter();
	static void ResetPollCounter();

	virtual void Reset(bool softReset);

	static void RegisterInputProvider(IInputProvider* provider);
	static void UnregisterInputProvider(IInputProvider* provider);

	static void RegisterInputRecorder(IInputRecorder* recorder);
	static void UnregisterInputRecorder(IInputRecorder* recorder);

	static vector<ControlDeviceState> GetPortStates();

	static shared_ptr<BaseControlDevice> GetControlDevice(uint8_t port);
	static shared_ptr<BaseControlDevice> CreateControllerDevice(ControllerType type, uint8_t port);
	static shared_ptr<BaseControlDevice> CreateExpansionDevice(ExpansionPortDevice type);
	static bool HasKeyboard();

	virtual void GetMemoryRanges(MemoryRanges &ranges) override
	{
		ranges.AddHandler(MemoryOperation::Read, 0x4016, 0x4017);
		ranges.AddHandler(MemoryOperation::Write, 0x4016);
	}

	virtual uint8_t ReadRAM(uint16_t addr) override;
	virtual void WriteRAM(uint16_t addr, uint8_t value) override;
};
