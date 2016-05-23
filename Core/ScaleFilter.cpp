#include "stdafx.h"
#include "PPU.h"
#include "ScaleFilter.h"
#include "../Utilities/xBRZ/xbrz.h"
#include "../Utilities/HQX/hqx.h"
#include "../Utilities/Scale2x/scalebit.h"
#include "../Utilities/KreedSaiEagle/SaiEagle.h"

ScaleFilter::ScaleFilter(ScaleFilterType scaleFilterType, uint32_t scale)
{
	_scaleFilterType = scaleFilterType;
	_filterScale = scale;
	_decodedPpuBuffer = new uint32_t[PPU::PixelCount];

	if(_scaleFilterType == ScaleFilterType::HQX) {
		hqxInit();
	}
}

ScaleFilter::~ScaleFilter()
{
	delete[] _decodedPpuBuffer;
}

FrameInfo ScaleFilter::GetFrameInfo()
{
	OverscanDimensions overscan = GetOverscan();
	return{ overscan.GetScreenWidth()*_filterScale, overscan.GetScreenHeight()*_filterScale, 4 };
}

void ScaleFilter::ApplyFilter(uint16_t *ppuOutputBuffer)
{
	OverscanDimensions overscan = EmulationSettings::GetOverscanDimensions();

	uint32_t* outputBuffer = _decodedPpuBuffer;
	for(uint32_t i = overscan.Top, iMax = 240 - overscan.Bottom; i < iMax; i++) {
		for(uint32_t j = overscan.Left, jMax = 256 - overscan.Right; j < jMax; j++) {
			*outputBuffer = ProcessIntensifyBits(ppuOutputBuffer[i * 256 + j]);
			outputBuffer++;
		}
	}

	uint32_t height = overscan.GetScreenHeight();
	uint32_t width = overscan.GetScreenWidth();

	if(_scaleFilterType == ScaleFilterType::xBRZ) {
		xbrz::scale(_filterScale, _decodedPpuBuffer, (uint32_t*)GetOutputBuffer(), width, height, xbrz::ColorFormat::ARGB);
	} else if(_scaleFilterType == ScaleFilterType::HQX) {
		switch(_filterScale) {
			case 2: hq2x_32(_decodedPpuBuffer, (uint32_t*)GetOutputBuffer(), width, height); break;
			case 3: hq3x_32(_decodedPpuBuffer, (uint32_t*)GetOutputBuffer(), width, height); break;
			case 4: hq4x_32(_decodedPpuBuffer, (uint32_t*)GetOutputBuffer(), width, height); break;
		}
	} else if(_scaleFilterType == ScaleFilterType::Scale2x) {
		scale(_filterScale, GetOutputBuffer(), width*sizeof(uint32_t)*_filterScale, _decodedPpuBuffer, width*sizeof(uint32_t), 4, width, height);
	} else if(_scaleFilterType == ScaleFilterType::_2xSai) {
		twoxsai_generic_xrgb8888(width, height, _decodedPpuBuffer, width, (uint32_t*)GetOutputBuffer(), width * _filterScale);
	} else if(_scaleFilterType == ScaleFilterType::Super2xSai) {
		supertwoxsai_generic_xrgb8888(width, height, _decodedPpuBuffer, width, (uint32_t*)GetOutputBuffer(), width * _filterScale);
	} else if(_scaleFilterType == ScaleFilterType::SuperEagle) {
		supereagle_generic_xrgb8888(width, height, _decodedPpuBuffer, width, (uint32_t*)GetOutputBuffer(), width * _filterScale);
	}
}