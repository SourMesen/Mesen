#pragma once

#include "stdafx.h"

class ClientConnectionData
{
public:
	string Host;
	uint16_t Port;

	string PlayerName;
	uint8_t* AvatarData;
	uint32_t AvatarSize;

	bool Spectator;

	ClientConnectionData(string host, uint16_t port, string playerName, uint8_t* avatarData, uint32_t avatarSize, bool spectator) :
		Host(host), Port(port), PlayerName(playerName), AvatarSize(avatarSize), Spectator(spectator)
	{
		if(avatarSize > 0) {
			AvatarData = new uint8_t[avatarSize];
			memcpy(AvatarData, avatarData, avatarSize);
		} else {
			AvatarData = nullptr;
		}
	}

	~ClientConnectionData()
	{
		if(AvatarData) {
			delete[] AvatarData;
		}
	}
};