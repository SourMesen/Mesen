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
	_decodedPpuBuffer = new uint32_t[PPU::PixelCount];

	if(!_hqxInitDone && _scaleFilterType == ScaleFilterType::HQX) {
		hqxInit();
		_hqxInitDone = true;
	}
}

ScaleFilter::~ScaleFilter()
{
	delete[] _decodedPpuBuffer;
}

FrameInfo ScaleFilter::GetFrameInfo()
{
	OverscanDimensions overscan = GetOverscan();
	return{ overscan.GetScreenWidth()*_filterScale, overscan.GetScreenHeight()*_filterScale, PPU::ScreenWidth*_filterScale, PPU::ScreenHeight*_filterScale, 4 };
}

void ScaleFilter::ApplyPrescaleFilter()
{
	uint32_t* outputBuffer = (uint32_t*)GetOutputBuffer();

	OverscanDimensions overscan = GetOverscan();
	uint32_t height = overscan.GetScreenHeight();
	uint32_t width = overscan.GetScreenWidth();

	uint32_t *inputBuffer = _decodedPpuBuffer;
	for(uint32_t y = 0; y < height; y++) {
		for(uint32_t x = 0; x < width; x++) {
			for(uint32_t i = 0; i < _filterScale; i++) {
				*(outputBuffer++) = *inputBuffer;
			}
			inputBuffer++;
		}
		for(uint32_t i = 1; i< _filterScale; i++) {
			memcpy(outputBuffer, outputBuffer - width*_filterScale, width*_filterScale *4);
			outputBuffer += width*_filterScale;
		}
	}
}

void ScaleFilter::ApplyFilter(uint16_t *ppuOutputBuffer)
{
	DecodePpuBuffer(ppuOutputBuffer, _decodedPpuBuffer, false);

	OverscanDimensions overscan = EmulationSettings::GetOverscanDimensions();
	uint32_t height = overscan.GetScreenHeight();
	uint32_t width = overscan.GetScreenWidth();
	uint32_t* outputBuffer = (uint32_t*)GetOutputBuffer();

	if(_scaleFilterType == ScaleFilterType::xBRZ) {
		xbrz::scale(_filterScale, _decodedPpuBuffer, outputBuffer, width, height, xbrz::ColorFormat::ARGB);
	} else if(_scaleFilterType == ScaleFilterType::HQX) {
		switch(_filterScale) {
			case 2: hq2x_32(_decodedPpuBuffer, outputBuffer, width, height); break;
			case 3: hq3x_32(_decodedPpuBuffer, outputBuffer, width, height); break;
			case 4: hq4x_32(_decodedPpuBuffer, outputBuffer, width, height); break;
		}
	} else if(_scaleFilterType == ScaleFilterType::Scale2x) {
		scale(_filterScale, outputBuffer, width*sizeof(uint32_t)*_filterScale, _decodedPpuBuffer, width*sizeof(uint32_t), 4, width, height);
	} else if(_scaleFilterType == ScaleFilterType::_2xSai) {
		twoxsai_generic_xrgb8888(width, height, _decodedPpuBuffer, width, outputBuffer, width * _filterScale);
	} else if(_scaleFilterType == ScaleFilterType::Super2xSai) {
		supertwoxsai_generic_xrgb8888(width, height, _decodedPpuBuffer, width, outputBuffer, width * _filterScale);
	} else if(_scaleFilterType == ScaleFilterType::SuperEagle) {
		supereagle_generic_xrgb8888(width, height, _decodedPpuBuffer, width, outputBuffer, width * _filterScale);
	} else if(_scaleFilterType == ScaleFilterType::Prescale) {
		ApplyPrescaleFilter();
	}

	double scanlineIntensity = 1.0 - EmulationSettings::GetPictureSettings().ScanlineIntensity;
	if(scanlineIntensity < 1.0) {
		for(int y = 1, yMax = height * _filterScale; y < yMax; y += 2) {
			for(int x = 0, xMax = width * _filterScale; x < xMax; x++) {
				uint32_t &color = outputBuffer[y*xMax + x];
				uint8_t r = (color >> 16) & 0xFF, g = (color >> 8) & 0xFF, b = color & 0xFF;
				r = (uint8_t)(r * scanlineIntensity);
				g = (uint8_t)(g * scanlineIntensity);
				b = (uint8_t)(b * scanlineIntensity);
				color = (r << 16) | (g << 8) | b;
			}
		}
	}
}