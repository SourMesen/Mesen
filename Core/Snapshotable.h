#pragma once

#include "stdafx.h"

class Snapshotable
{
	uint8_t* _stream;
	uint32_t _position;
	uint32_t _streamSize;
	bool _saving;

	protected:
		virtual void StreamState(bool saving) = 0;

		void Stream(Snapshotable* snapshotable)
		{
			stringstream stream;
			if(_saving) {
				snapshotable->SaveSnapshot(&stream);
				uint32_t size = (uint32_t)stream.tellp();
				stream.seekg(0, ios::beg);
				stream.seekp(0, ios::beg);

				uint8_t *buffer = new uint8_t[size];
				stream.read((char*)buffer, size);
				Stream<uint32_t>(size);
				StreamArray<uint8_t>(buffer, size);
				delete[] buffer;
			} else {
				uint32_t size = 0;
				Stream<uint32_t>(size);
				
				if(_position + size <= _streamSize) {
					uint8_t *buffer = new uint8_t[size];
					StreamArray(buffer, size);

					stream.write((char*)buffer, size);
					stream.seekg(0, ios::beg);
					stream.seekp(0, ios::beg);
					snapshotable->LoadSnapshot(&stream);
					delete[] buffer;
				} else {
					_position = _streamSize;
				}
			}
		}

		template<typename T>
		void Stream(T &value)
		{
			if(_saving) {
				uint8_t* bytes = (uint8_t*)&value;
				int typeSize = sizeof(T);
				for(int i = 0; i < typeSize; i++) {
					_stream[_position++] = bytes[i];
				}
			} else {
				if(_position + sizeof(T) <= _streamSize) {
					value = *((T*)(_stream + _position));
					_position += sizeof(T);
				} else {
					_position = _streamSize;
				}
			}
		}

		template<typename T>
		void StreamArray(T* value, uint32_t length)
		{
			uint32_t typeSize = sizeof(T);
			if(_saving) {
				uint8_t* bytes = (uint8_t*)value;
				for(uint32_t i = 0, len = length*typeSize; i < len; i++) {
					_stream[_position++] = bytes[i];
				}
			} else {
				for(uint32_t i = 0, len = length*typeSize; i < len;  i++) {
					if(_position < _streamSize) {
						((uint8_t*)value)[i] = _stream[_position];
						_position++;
					} else {
						((uint8_t*)value)[i] = 0;
						_position = _streamSize;
					}
				}
			}
		}

	public:
		void SaveSnapshot(ostream* file)
		{
			_stream = new uint8_t[0xFFFFF];
			memset((char*)_stream, 0, 0xFFFFF);
			_position = 0;
			_saving = true;

			StreamState(_saving);
			file->write((char*)&_position, sizeof(_position));
			file->write((char*)_stream, _position);

			delete[] _stream;
		}

		void LoadSnapshot(istream* file)
		{
			_stream = new uint8_t[0xFFFFF];
			memset((char*)_stream, 0, 0xFFFFF);
			_position = 0;
			_saving = false;
			
			file->read((char*)&_streamSize, sizeof(_streamSize));
			file->read((char*)_stream, _streamSize);
			StreamState(_saving);

			delete[] _stream;
		}
};