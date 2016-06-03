#pragma once

#include "stdafx.h"
#include <algorithm>
#include "Snapshotable.h"

void Snapshotable::StreamStartBlock()
{
	if(_inBlock) {
		throw new std::runtime_error("Cannot start a new block before ending the last block");
	}

	_blockBuffer = new uint8_t[0xFFFFF];
	if(!_saving) {
		InternalStream(_blockSize);
		InternalStream(ArrayInfo<uint8_t>{_blockBuffer, std::min((uint32_t)0xFFFFF, _blockSize)});
	}
	_blockPosition = 0;
	_inBlock = true;
}

void Snapshotable::StreamEndBlock()
{
	_inBlock = false;
	if(_saving) {
		InternalStream(_blockPosition);
		InternalStream(ArrayInfo<uint8_t>{_blockBuffer, _blockPosition});
	}

	delete[] _blockBuffer;
	_blockBuffer = nullptr;
}

void Snapshotable::Stream(Snapshotable* snapshotable)
{
	stringstream stream;
	if(_saving) {
		snapshotable->SaveSnapshot(&stream);
		uint32_t size = (uint32_t)stream.tellp();
		stream.seekg(0, ios::beg);
		stream.seekp(0, ios::beg);

		uint8_t *buffer = new uint8_t[size];
		stream.read((char*)buffer, size);
		InternalStream(size);
		InternalStream(ArrayInfo<uint8_t>{buffer, size});
		delete[] buffer;
	} else {
		uint32_t size = 0;
		InternalStream(size);

		if(_position + size <= _streamSize) {
			uint8_t *buffer = new uint8_t[size];
			InternalStream(ArrayInfo<uint8_t>{buffer, size});

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

void Snapshotable::SaveSnapshot(ostream* file)
{
	_stream = new uint8_t[0xFFFFF];
	memset((char*)_stream, 0, 0xFFFFF);
	_position = 0;
	_saving = true;

	StreamState(_saving);
	file->write((char*)&_position, sizeof(_position));
	file->write((char*)_stream, _position);

	delete[] _stream;

	if(_blockBuffer) {
		throw new std::runtime_error("A call to StreamEndBlock is missing.");
	}
}

void Snapshotable::LoadSnapshot(istream* file)
{
	_stream = new uint8_t[0xFFFFF];
	memset((char*)_stream, 0, 0xFFFFF);
	_position = 0;
	_saving = false;

	file->read((char*)&_streamSize, sizeof(_streamSize));
	file->read((char*)_stream, _streamSize);
	StreamState(_saving);

	delete[] _stream;

	if(_blockBuffer) {
		throw new std::runtime_error("A call to StreamEndBlock is missing.");
	}
}