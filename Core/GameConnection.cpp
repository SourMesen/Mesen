#pragma once
#include "stdafx.h"
#include "GameConnection.h"
#include "HandShakeMessage.h"
#include "InputDataMessage.h"
#include "MovieDataMessage.h"
#include "GameInformationMessage.h"
#include "SaveStateMessage.h"

GameConnection::GameConnection(shared_ptr<Socket> socket)
{
	_socket = socket;
}

void GameConnection::ReadSocket()
{
	int bytesReceived = _socket->Recv(_readBuffer + _readPosition, 0x40000 - _readPosition, 0);
	if(bytesReceived > 0) {
		_readPosition += bytesReceived;
	}
}

bool GameConnection::ExtractMessage(char *buffer, uint32_t &messageLength)
{
	messageLength = *(uint32_t*)_readBuffer;

	if(messageLength > 100000) {
		std::cout << "Invalid data received, closing connection" << std::endl;
		_socket->Close();
		return false;
	}

	int packetLength = messageLength + sizeof(messageLength);

	if(_readPosition >= packetLength) {
		memcpy(buffer, _readBuffer+sizeof(messageLength), messageLength);
		memmove(_readBuffer, _readBuffer + packetLength, _readPosition - packetLength);
		_readPosition -= packetLength;
		return true;
	}
	return false;
}

NetMessage* GameConnection::ReadMessage()
{
	ReadSocket();

	uint32_t messageLength;		
	if(ExtractMessage(_messageBuffer, messageLength)) {
		switch((MessageType)_messageBuffer[0]) {
			case MessageType::HandShake: return new HandShakeMessage(_messageBuffer + 1);
			case MessageType::SaveState: return new SaveStateMessage(_messageBuffer + 1, messageLength - 1);
			case MessageType::InputData: return new InputDataMessage(_messageBuffer + 1);
			case MessageType::MovieData: return new MovieDataMessage(_messageBuffer + 1);
			case MessageType::GameInformation: return new GameInformationMessage(_messageBuffer + 1);
		}
	}
	return nullptr;
}

void GameConnection::SendNetMessage(NetMessage &message)
{
	_socketLock.Acquire();
	message.Send(*_socket.get());
	_socketLock.Release();
}

bool GameConnection::ConnectionError()
{
	return _socket->ConnectionError();
}

void GameConnection::ProcessMessages()
{
	NetMessage* message;
	while(message = ReadMessage()) {
		//Loop until all messages have been processed
		ProcessMessage(message);
		delete message;
	}		
}