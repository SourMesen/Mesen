#pragma once
#include "stdafx.h"
#include "GameConnection.h"
#include "VirtualController.h"

class GameClientConnection : public GameConnection
{
private:
	vector<unique_ptr<VirtualController>> _virtualControllers;
	IControlDevice* _controlDevice;
	uint8_t _lastInputSent = 0x00;
	bool _gameLoaded = false;
	uint8_t _controllerPort = 255;

private:
	void SendHandshake();
	void InitializeVirtualControllers();
	void DisposeVirtualControllers();

protected:
	void ProcessMessage(NetMessage* message);

public:
	GameClientConnection(shared_ptr<Socket> socket);
	~GameClientConnection();
	
	void SendInput();
};