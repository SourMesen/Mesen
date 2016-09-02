#pragma once

#include "stdafx.h"
#include "BaseControlDevice.h"
#include "IMemoryHandler.h"
#include "Movie.h"
#include "IGameBroadcaster.h"
#include "Snapshotable.h"
#include "../Utilities/SimpleLock.h"
#include "IKeyManager.h"

class BaseControlDevice;
class Zapper;

struct MousePosition
{
	int32_t X;
	int32_t Y;
};

class ControlManager : public Snapshotable, public IMemoryHandler
{
	private:
		static unique_ptr<IKeyManager> _keyManager;
		static shared_ptr<BaseControlDevice> _controlDevices[2];
		static IGameBroadcaster* _gameBroadcaster;
		static MousePosition _mousePosition;

		bool _isLagging = false;
		bool _refreshState = false;

		template<typename T> shared_ptr<T> GetExpansionDevice();

		virtual shared_ptr<BaseControlDevice> GetZapper(uint8_t port);

		static void RegisterControlDevice(shared_ptr<BaseControlDevice> controlDevice, uint8_t port);
		void UnregisterControlDevice(uint8_t port);

	protected:
		uint8_t GetPortValue(uint8_t port);
		virtual void RefreshAllPorts();

		virtual void StreamState(bool saving);

	public:
		ControlManager();
		virtual ~ControlManager();

		void UpdateControlDevices();
		bool GetLagFlag();

		virtual void Reset(bool softReset);
		
		static void RegisterBroadcaster(IGameBroadcaster* gameBroadcaster);
		static void UnregisterBroadcaster(IGameBroadcaster* gameBroadcaster);

		static void RegisterKeyManager(IKeyManager* keyManager);
		static void RefreshKeyState();
		static bool IsKeyPressed(uint32_t keyCode);
		static bool IsMouseButtonPressed(MouseButton button);
		static uint32_t GetPressedKey();
		static string GetKeyName(uint32_t keyCode);
		static uint32_t GetKeyCode(string keyName);
		
		static shared_ptr<BaseControlDevice> GetControlDevice(uint8_t port);

		static void SetMousePosition(double x, double y);
		static MousePosition GetMousePosition();

		static void BroadcastInput(uint8_t port, uint8_t state);

		virtual void GetMemoryRanges(MemoryRanges &ranges)
		{
			ranges.AddHandler(MemoryOperation::Read, 0x4016, 0x4017);
			ranges.AddHandler(MemoryOperation::Write, 0x4016);
		}
		
		virtual uint8_t ReadRAM(uint16_t addr);
		virtual void WriteRAM(uint16_t addr, uint8_t value);
};