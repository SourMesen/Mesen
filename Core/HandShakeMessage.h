#pragma once
#include "stdafx.h"
#include "NetMessage.h"

class HandShakeMessage : public NetMessage
{
private:
	const static int CurrentVersion = 1;
	uint32_t _protocolVersion = CurrentVersion;
	wchar_t *_playerName = nullptr;
	uint32_t _playerNameLength = 0;
	void* _avatarData = nullptr;
	uint32_t _avatarSize = 0;
	
protected:
	virtual void ProtectedStreamState()
	{
		Stream<uint32_t>(_protocolVersion);
		StreamArray((void**)&_playerName, _playerNameLength);
		StreamArray(&_avatarData, _avatarSize);
	}

public:
	HandShakeMessage(void* buffer, uint32_t length) : NetMessage(buffer, length) { }
	HandShakeMessage(wstring playerName, uint8_t* avatarData, uint32_t avatarSize) : NetMessage(MessageType::HandShake)
	{
		_protocolVersion = 1;
		CopyString(&_playerName, _playerNameLength, playerName);
		_avatarSize = avatarSize;
		_avatarData = avatarData;
	}
	
	wstring GetPlayerName()
	{
		return wstring(_playerName);
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
};
