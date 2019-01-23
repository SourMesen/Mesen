#include "stdafx.h"
#include <algorithm>
#include "Snapshotable.h"
#include "SaveStateManager.h"

void Snapshotable::StreamStartBlock()
{
	if(_inBlock) {
		throw new std::runtime_error("Cannot start a new block before ending the last block");
	}

	if(!_saving) {
		InternalStream(_blockSize);
		_blockSize = std::min(_blockSize, (uint32_t)0xFFFFF);
		_blockBuffer = new uint8_t[_blockSize];
		ArrayInfo<uint8_t> arrayInfo = { _blockBuffer, _blockSize };
		InternalStream(arrayInfo);
	} else {
		_blockSize = 0x100;
		_blockBuffer = new uint8_t[_blockSize];
	}
	_blockPosition = 0;
	_inBlock = true;
}

void Snapshotable::StreamEndBlock()
{
	_inBlock = false;
	if(_saving) {
		InternalStream(_blockPosition);
		ArrayInfo<uint8_t> arrayInfo = { _blockBuffer, _blockPosition };
		InternalStream(arrayInfo);
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
		ArrayInfo<uint8_t> arrayInfo = { buffer, size };
		InternalStream(arrayInfo);
		delete[] buffer;
	} else {
		uint32_t size = 0;
		InternalStream(size);

		uint8_t *buffer = new uint8_t[size];
		ArrayInfo<uint8_t> arrayInfo = { buffer, size };
		InternalStream(arrayInfo);

		stream.write((char*)buffer, size);
		stream.seekg(0, ios::beg);
		stream.seekp(0, ios::beg);
		snapshotable->LoadSnapshot(&stream, _stateVersion);
		delete[] buffer;
	}
}

void Snapshotable::SaveSnapshot(ostream* file)
{
	_stateVersion = SaveStateManager::FileFormatVersion;

	_streamSize = 0x1000;
	_stream = new uint8_t[_streamSize];
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

void Snapshotable::LoadSnapshot(istream* file, uint32_t stateVersion)
{
	_stateVersion = stateVersion;

	_position = 0;
	_saving = false;

	file->read((char*)&_streamSize, sizeof(_streamSize));
	_stream = new uint8_t[_streamSize];
	file->read((char*)_stream, _streamSize);
	StreamState(_saving);

	delete[] _stream;

	if(_blockBuffer) {
		throw new std::runtime_error("A call to StreamEndBlock is missing.");
	}
}

void Snapshotable::WriteEmptyBlock(ostream* file)
{
	int blockSize = 0;
	file->write((char*)&blockSize, sizeof(blockSize));
}

void Snapshotable::SkipBlock(istream* file)
{
	int blockSize = 0;
	file->read((char*)&blockSize, sizeof(blockSize));
	file->seekg(blockSize, ios::cur);
}