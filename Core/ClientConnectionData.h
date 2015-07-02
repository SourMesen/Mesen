#pragma once

#include "stdafx.h"

class ClientConnectionData
{
public:
	string Host;
	uint16_t Port;

	wstring PlayerName;
	uint8_t* AvatarData;
	uint32_t AvatarSize;

	ClientConnectionData(string host, uint16_t port, wstring playerName, uint8_t* avatarData, uint32_t avatarSize) :
		Host(host), Port(port), PlayerName(playerName), AvatarSize(avatarSize)
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