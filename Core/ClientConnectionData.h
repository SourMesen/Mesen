#pragma once

#include "stdafx.h"

class ClientConnectionData
{
public:
	string Host;
	uint16_t Port;

	string PlayerName;

	bool Spectator;

	ClientConnectionData(string host, uint16_t port, string playerName, bool spectator) :
		Host(host), Port(port), PlayerName(playerName), Spectator(spectator)
	{
	}

	~ClientConnectionData()
	{
	}
};