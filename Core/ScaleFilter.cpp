#include "stdafx.h"
#include "PPU.h"
#include "ScaleFilter.h"
#include "../Utilities/xBRZ/xbrz.h"
#include "../Utilities/HQX/hqx.h"
#include "../Utilities/Scale2x/scalebit.h"
#include "../Utilities/KreedSaiEagle/SaiEagle.h"

bool ScaleFilter::_hqxInitDone = false;

ScaleFilter::ScaleFilter(ScaleFilterType scaleFilterType, uint32_t scale)
{
	_scaleFilterType = scaleFilterType;
	_filterScale = scale;

	if(!_hqxInitDone && _scaleFilterType == ScaleFilterType::HQX) {
		hqxInit();
		_hqxInitDone = true;
	}
}

ScaleFilter::~ScaleFilter()
{
	if(_outputBuffer) {
		delete[] _outputBuffer;
	}
}

uint32_t ScaleFilter::GetScale()
{
	return _filterScale;
}

void ScaleFilter::ApplyPrescaleFilter(uint32_t *inputArgbBuffer)
{
	uint32_t* outputBuffer = _outputBuffer;

	for(uint32_t y = 0; y < _height; y++) {
		for(uint32_t x = 0; x < _width; x++) {
			for(uint32_t i = 0; i < _filterScale; i++) {
				*(outputBuffer++) = *inputArgbBuffer;
			}
			inputArgbBuffer++;
		}
		for(uint32_t i = 1; i < _filterScale; i++) {
			memcpy(outputBuffer, outputBuffer - _width*_filterScale, _width*_filterScale *4);
			outputBuffer += _width*_filterScale;
		}
	}
}

void ScaleFilter::UpdateOutputBuffer(uint32_t width, uint32_t height)
{
	if(!_outputBuffer || width != _width || height != _height) {
		if(_outputBuffer) {
			delete[] _outputBuffer;
		}

		_width = width;
		_height = height;
		_outputBuffer = new uint32_t[_width*_height*_filterScale*_filterScale];
	}
}

uint32_t* ScaleFilter::ApplyFilter(uint32_t *inputArgbBuffer, uint32_t width, uint32_t height)
{
	UpdateOutputBuffer(width, height);

	if(_scaleFilterType == ScaleFilterType::xBRZ) {
		xbrz::scale(_filterScale, inputArgbBuffer, _outputBuffer, width, height, xbrz::ColorFormat::ARGB);
	} else if(_scaleFilterType == ScaleFilterType::HQX) {
		hqx(_filterScale, inputArgbBuffer, _outputBuffer, width, height);
	} else if(_scaleFilterType == ScaleFilterType::Scale2x) {
		scale(_filterScale, _outputBuffer, width*sizeof(uint32_t)*_filterScale, inputArgbBuffer, width*sizeof(uint32_t), 4, width, height);
	} else if(_scaleFilterType == ScaleFilterType::_2xSai) {
		twoxsai_generic_xrgb8888(width, height, inputArgbBuffer, width, _outputBuffer, width * _filterScale);
	} else if(_scaleFilterType == ScaleFilterType::Super2xSai) {
		supertwoxsai_generic_xrgb8888(width, height, inputArgbBuffer, width, _outputBuffer, width * _filterScale);
	} else if(_scaleFilterType == ScaleFilterType::SuperEagle) {
		supereagle_generic_xrgb8888(width, height, inputArgbBuffer, width, _outputBuffer, width * _filterScale);
	} else if(_scaleFilterType == ScaleFilterType::Prescale) {
		ApplyPrescaleFilter(inputArgbBuffer);
	}

	double scanlineIntensity = 1.0 - EmulationSettings::GetPictureSettings().ScanlineIntensity;
	if(scanlineIntensity < 1.0) {
		for(int y = 1, yMax = height * _filterScale; y < yMax; y += 2) {
			for(int x = 0, xMax = width * _filterScale; x < xMax; x++) {
				uint32_t &color = _outputBuffer[y*xMax + x];
				uint8_t r = (color >> 16) & 0xFF, g = (color >> 8) & 0xFF, b = color & 0xFF;
				r = (uint8_t)(r * scanlineIntensity);
				g = (uint8_t)(g * scanlineIntensity);
				b = (uint8_t)(b * scanlineIntensity);
				color = (r << 16) | (g << 8) | b;
			}
		}
	}

	return _outputBuffer;
}