#pragma once

#include "stdafx.h"

class Snapshotable
{
	uint8_t* _stream;
	uint32_t _position;
	bool _saving;

	protected:
		virtual void StreamState(bool saving) = 0;

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
				value = *((T*)(_stream + _position));
				_position += sizeof(T);
			}
		}

		template<typename T>
		void StreamArray(T* value, uint32_t length)
		{
			uint32_t typeSize = sizeof(*value);
			if(_saving) {
				uint8_t* bytes = (uint8_t*)value;
				for(uint32_t i = 0, len = length*typeSize; i < len; i++) {
					_stream[_position++] = bytes[i];
				}
			} else {
				for(uint32_t i = 0; i < length*typeSize; i++) {
					((uint8_t*)value)[i] = _stream[_position];
					_position++;
				}
			}
		}

	public:
		void SaveSnapshot(ofstream* file)
		{
			_stream = new uint8_t[0xFFFF];
			memset((char*)_stream, 0, 0xFFFF);
			_position = 0;
			_saving = true;

			StreamState(_saving);
			file->write((char*)_stream, 0xFFFF);

			delete[] _stream;
		}

		void LoadSnapshot(ifstream* file)
		{
			_stream = new uint8_t[0xFFFF];
			_position = 0;
			_saving = false;
			
			file->read((char*)_stream, 0xFFFF);
			StreamState(_saving);

			delete[] _stream;
		}
};