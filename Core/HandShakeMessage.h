#pragma once
#include "stdafx.h"
#include "NetMessage.h"
#include "EmulationSettings.h"

class HandShakeMessage : public NetMessage
{
private:
	const static int CurrentVersion = 1;
	uint32_t _mesenVersion = 0;
	uint32_t _protocolVersion = CurrentVersion;
	char* _playerName = nullptr;
	uint32_t _playerNameLength = 0;
	bool _spectator = false;
	
protected:
	virtual void ProtectedStreamState()
	{
		Stream<uint32_t>(_mesenVersion);
		Stream<uint32_t>(_protocolVersion);
		StreamArray((void**)&_playerName, _playerNameLength);
		Stream<bool>(_spectator);
	}

public:
	HandShakeMessage(void* buffer, uint32_t length) : NetMessage(buffer, length) { }
	HandShakeMessage(string playerName, bool spectator) : NetMessage(MessageType::HandShake)
	{
		_mesenVersion = EmulationSettings::GetMesenVersion();
		_protocolVersion = HandShakeMessage::CurrentVersion;
		CopyString(&_playerName, _playerNameLength, playerName);
		_spectator = spectator;
	}

	string GetPlayerName()
	{
		return string(_playerName);
	}

	bool IsValid()
	{
		return _protocolVersion == CurrentVersion && _mesenVersion == EmulationSettings::GetMesenVersion();
	}

	bool IsSpectator()
	{
		return _spectator;
	}
};
