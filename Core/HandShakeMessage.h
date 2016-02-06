#pragma once
#include "stdafx.h"
#include "NetMessage.h"

class HandShakeMessage : public NetMessage
{
private:
	const static int CurrentVersion = 1;
	uint32_t _protocolVersion = CurrentVersion;
	char* _playerName = nullptr;
	uint32_t _playerNameLength = 0;
	void* _avatarData = nullptr;
	uint32_t _avatarSize = 0;
	bool _spectator = false;
	
protected:
	virtual void ProtectedStreamState()
	{
		Stream<uint32_t>(_protocolVersion);
		StreamArray((void**)&_playerName, _playerNameLength);
		StreamArray(&_avatarData, _avatarSize);
		Stream<bool>(_spectator);
	}

public:
	HandShakeMessage(void* buffer, uint32_t length) : NetMessage(buffer, length) { }
	HandShakeMessage(string playerName, uint8_t* avatarData, uint32_t avatarSize, bool spectator) : NetMessage(MessageType::HandShake)
	{
		_protocolVersion = 1;
		CopyString(&_playerName, _playerNameLength, playerName);
		_avatarSize = avatarSize;
		_avatarData = avatarData;
		_spectator = spectator;
	}
	
	string GetPlayerName()
	{
		return string(_playerName);
	}

	uint8_t* GetAvatarData()
	{
		return (uint8_t*)_avatarData;
	}

	uint32_t GetAvatarSize()
	{
		return _avatarSize;
	}

	bool IsValid()
	{
		return _protocolVersion == CurrentVersion;
	}

	bool IsSpectator()
	{
		return _spectator;
	}
};
