//This is based on the code in lsnes' cscd.cpp file
//A few modifications were done to improve compression speed
#include "stdafx.h"
#include <cstring>
#include "CamstudioCodec.h"
#include "miniz.h"

CamstudioCodec::~CamstudioCodec()
{
	if(_prevFrame) {
		delete[] _prevFrame;
		_prevFrame = nullptr;
	}

	if(_currentFrame) {
		delete[] _currentFrame;
		_currentFrame = nullptr;
	}

	if(_buffer) {
		delete[] _buffer;
		_buffer = nullptr;
	}

	if(_compressBuffer) {
		delete[] _compressBuffer;
		_compressBuffer = nullptr;
	}

	deflateEnd(&_compressor);
}

bool CamstudioCodec::SetupCompress(int width, int height, uint32_t compressionLevel)
{
	_compressionLevel = compressionLevel;
	_orgHeight = height;
	_orgWidth = width;

	_width = (width + 3) & ~3;
	_height = (height + 3) & ~3;

	_prevFrame = new uint8_t[_width*_height*3]; //24-bit RGB
	_currentFrame = new uint8_t[_width*height*3]; //24-bit RGB
	_buffer = new uint8_t[_width*height*3]; //24-bit RGB

	_compressBufferLength = compressBound(_width*_height * 3) + 2;
	_compressBuffer = new uint8_t[_compressBufferLength];
	
	memset(_prevFrame, 0, _width * _height * 3);
	memset(_currentFrame, 0, _width * _height * 3);
	memset(_buffer, 0, _width * _height * 3);
	memset(_compressBuffer, 0, _compressBufferLength);
	
	deflateInit(&_compressor, compressionLevel);

	return true;
}

void CamstudioCodec::LoadRow(uint8_t* inPointer, uint8_t* outPointer)
{
	for(int x = 0; x < _orgWidth; x++) {
		outPointer[0] = inPointer[0];
		outPointer[1] = inPointer[1];
		outPointer[2] = inPointer[2];
		outPointer += 3;
		inPointer += 4;
	}
}

int CamstudioCodec::CompressFrame(bool isKeyFrame, uint8_t *frameData, uint8_t** compressedData)
{
	deflateReset(&_compressor);

	_compressor.next_out = _compressBuffer + 2;
	_compressor.avail_out = _compressBufferLength - 2;

	_compressBuffer[0] = (isKeyFrame ? 0x03 : 0x02) | (_compressionLevel << 4);
	_compressBuffer[1] = 8; //8-bit per color

	uint8_t* rowBuffer = _currentFrame;
	for(int y = 0; y < _height; y++) {
		if(y < _height - _orgHeight) {
			memset(rowBuffer, 0, _width * 3);
		} else {
			LoadRow(frameData + (_height - y - 1) * _orgWidth * 4, rowBuffer);
		}
		rowBuffer += _width * 3;
	}

	if(isKeyFrame) {
		_compressor.next_in = _currentFrame;
	} else {
		for(int i = 0, len = _width * _height * 3; i < len; i++) {
			_buffer[i] = _currentFrame[i] - _prevFrame[i];
		}
		_compressor.next_in = _buffer;
	}

	memcpy(_prevFrame, _currentFrame, _width*_height*3);
	
	_compressor.avail_in = _height * _width * 3;
	deflate(&_compressor, MZ_FINISH);
	
	*compressedData = _compressBuffer;
	return _compressor.total_out + 2;
}

const char* CamstudioCodec::GetFourCC()
{
	return "CSCD";
}