#pragma once
#include "stdafx.h"
#include "MessageType.h"
#include "../Utilities/Socket.h"

class NetMessage
{
protected:
	MessageType _type;
	bool _sending;
	vector<uint8_t> _buffer;
	uint32_t _position = 0;
	vector<void*> _pointersToRelease;

	template<typename T>
	void Stream(T &value)
	{
		if(_sending) {
			uint8_t* bytes = (uint8_t*)&value;
			int typeSize = sizeof(T);
			for(int i = 0; i < typeSize; i++) {
				_buffer.push_back(bytes[i]);
			}
		} else {
			value = *((T*)(&_buffer[0] + _position));
			_position += sizeof(T);
		}
	}

	void StreamArray(void** value, uint32_t &length)
	{
		if(_sending) {
			Stream<uint32_t>(length);
			uint8_t* bytes = (uint8_t*)(*value);
			for(uint32_t i = 0, len = length; i < len; i++) {
				_buffer.push_back(bytes[i]);
				_position++;
			}
		} else {
			Stream<uint32_t>(length);
			if(*value == nullptr) {
				*value = (void*)new uint8_t[length];
				_pointersToRelease.push_back(*value);
			}
			uint8_t* bytes = (uint8_t*)(*value);
			for(uint32_t i = 0, len = length; i < len; i++) {
				bytes[i] = _buffer[_position];
				_position++;
			}
		}
	}

	void StreamState()
	{
		Stream<MessageType>(_type);
		ProtectedStreamState();
	}

	NetMessage(MessageType type)
	{
		_type = type;
		_sending = true;
	}

	NetMessage(void* buffer, uint32_t length)
	{
		_buffer.assign((uint8_t*)buffer, (uint8_t*)buffer + length);
		_sending = false;
	}

public:
	virtual ~NetMessage() 
	{	
		for(void *pointer : _pointersToRelease) {
			delete[] pointer;
		}
	}

	void Initialize()
	{
		StreamState();
	}

	MessageType GetType()
	{
		return _type;
	}

	void Send(Socket &socket)
	{
		StreamState();
		uint32_t messageLength = (uint32_t)_buffer.size();
		socket.BufferedSend((char*)&messageLength, sizeof(messageLength));
		socket.BufferedSend((char*)&_buffer[0], messageLength);
		socket.SendBuffer();
	}

	void CopyString(wchar_t** dest, uint32_t &length, wstring src)
	{
		length = (uint32_t)(src.length() + 1)*sizeof(wchar_t);
		*dest = (wchar_t*)new uint8_t[length];
		memcpy(*dest, src.c_str(), length);
		_pointersToRelease.push_back(*dest);
	}

protected:
	virtual void ProtectedStreamState() = 0;
};
