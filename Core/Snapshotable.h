#pragma once

#include "stdafx.h"

class Snapshotable;

template<typename T>
struct ArrayInfo
{
	T* Array;
	uint32_t ElementCount;
};

template<typename T>
struct ValueInfo
{
	T* Value;
	T DefaultValue;
};

struct SnapshotInfo
{
	Snapshotable* Entity;
};

template<typename T>
struct EmptyInfo
{
	T Empty;
};

class Snapshotable
{
private:
	uint8_t* _stream;
	uint32_t _position;
	uint32_t _streamSize;

	bool _inBlock = false;
	uint8_t* _blockBuffer = nullptr;
	uint32_t _blockSize = 0;
	uint32_t _blockPosition = 0;

	bool _saving;

private:
	template<typename T>
	void StreamElement(T &value, T defaultValue = T())
	{
		if(_saving) {
			uint8_t* bytes = (uint8_t*)&value;
			int typeSize = sizeof(T);
			for(int i = 0; i < typeSize; i++) {
				if(_inBlock) {
					_blockBuffer[_blockPosition++] = bytes[i];
				} else {
					_stream[_position++] = bytes[i];
				}
			}
		} else {
			if(_inBlock) {
				if(_blockPosition + sizeof(T) <= _blockSize) {
					value = *((T*)(_blockBuffer + _blockPosition));
					_blockPosition += sizeof(T);
				} else {
					value = defaultValue;
					_blockPosition = _blockSize;
				}
			} else {
				if(_position + sizeof(T) <= _streamSize) {
					value = *((T*)(_stream + _position));
					_position += sizeof(T);
				} else {
					value = defaultValue;
					_position = _streamSize;
				}
			}
		}
	}

	template<typename T>
	void InternalStream(EmptyInfo<T> &info)
	{
		if(_inBlock) {
			_blockPosition += sizeof(T);
		} else {
			_position += sizeof(T);
		}
	}

	template<typename T>
	void InternalStream(ArrayInfo<T> &info)
	{
		T* pointer = info.Array;

		uint32_t count = info.ElementCount;
		StreamElement<uint32_t>(count);

		if(!_saving) {
			//Reset array to 0 before loading from file
			memset(info.Array, 0, sizeof(T) * info.ElementCount);
		}

		//Load the number of elements requested, or the maximum possible (based on what is present in the save state)
		for(uint32_t i = 0; i < info.ElementCount && i < count; i++) {
			StreamElement<T>(*pointer);
			pointer++;
		}
	}

	template<typename T>
	void InternalStream(ValueInfo<T> &info)
	{
		StreamElement<T>(*info.Value, info.DefaultValue);
	}

	template<typename T>
	void InternalStream(T &value)
	{
		StreamElement<T>(value);
	}

	template<>
	void InternalStream(SnapshotInfo &info)
	{
		if(info.Entity != nullptr) {
			Stream(info.Entity);
		}
	}

	void RecursiveStream()
	{
	}

	template<typename T, typename... T2>
	void RecursiveStream(T &value, T2&... args)
	{
		InternalStream(value);
		RecursiveStream(args...);
	}

	void StreamStartBlock();
	void StreamEndBlock();

protected:
	virtual void StreamState(bool saving) = 0;

	void Stream(Snapshotable* snapshotable);

	template<typename... T>
	void Stream(T&... args)
	{
		StreamStartBlock();
		RecursiveStream(args...);
		StreamEndBlock();
	}

public:
	void SaveSnapshot(ostream* file);
	void LoadSnapshot(istream* file);
};