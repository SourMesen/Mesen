#pragma once
#include "stdafx.h"
#include <deque>
#include "GameConnection.h"
#include "../Utilities/AutoResetEvent.h"
#include "../Utilities/SimpleLock.h"
#include "StandardController.h"

class ClientConnectionData;

class GameClientConnection : public GameConnection, public INotificationListener
{
private:
	std::deque<uint8_t> _inputData[4];
	atomic<uint32_t> _inputSize[4];
	AutoResetEvent _waitForInput[4];
	SimpleLock _writeLock;
	atomic<bool> _shutdown;
	atomic<bool> _enableControllers = false;
	atomic<uint32_t> _minimumQueueSize = 3;

	vector<PlayerInfo> _playerList;

	shared_ptr<BaseControlDevice> _controlDevice;
	shared_ptr<BaseControlDevice> _newControlDevice;
	uint32_t _lastInputSent = 0x00;
	bool _gameLoaded = false;
	uint8_t _controllerPort = GameConnection::SpectatorPort;

private:
	void SendHandshake();
	void SendControllerSelection(uint8_t port);
	void ClearInputData();
	void PushControllerState(uint8_t port, uint8_t state);
	void DisableControllers();

protected:
	void ProcessMessage(NetMessage* message);

public:
	GameClientConnection(shared_ptr<Socket> socket, shared_ptr<ClientConnectionData> connectionData);
	~GameClientConnection();

	void ProcessNotification(ConsoleNotificationType type, void* parameter);

	uint8_t GetControllerState(uint8_t port);
	void SendInput();

	void SelectController(uint8_t port);
	uint8_t GetAvailableControllers();
	uint8_t GetControllerPort();
};