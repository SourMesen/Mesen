#include "stdafx.h"
#include "DefaultVideoFilter.h"
#include "EmulationSettings.h"
#define _USE_MATH_DEFINES
#include <math.h>
#include <algorithm>

DefaultVideoFilter::DefaultVideoFilter()
{
	InitConversionMatrix(_pictureSettings.Hue, _pictureSettings.Saturation);
}

void DefaultVideoFilter::InitConversionMatrix(double hueShift, double saturationShift)
{
	_pictureSettings.Hue = hueShift;
	_pictureSettings.Saturation = saturationShift;

	double hue = hueShift * M_PI - 15 * M_PI / 180;
	double sat = saturationShift + 1;

	double baseValues[6] = { 0.956f, 0.621f, -0.272f, -0.647f, -1.105f, 1.702f };

	double s = sin(hue) * sat;
	double c = cos(hue) * sat;

	double *output = _yiqToRgbMatrix;
	double *input = baseValues;
	for(int n = 0; n < 3; n++) {
		double i = *input++;
		double q = *input++;
		*output++ = i * c - q * s;
		*output++ = i * s + q * c;
	}
}

FrameInfo DefaultVideoFilter::GetFrameInfo()
{
	OverscanDimensions overscan = GetOverscan();
	return { overscan.GetScreenWidth(), overscan.GetScreenHeight(), 4 };
}

void DefaultVideoFilter::OnBeforeApplyFilter()
{
	PictureSettings currentSettings = EmulationSettings::GetPictureSettings();
	if(_pictureSettings.Hue != currentSettings.Hue || _pictureSettings.Saturation != currentSettings.Saturation) {
		InitConversionMatrix(currentSettings.Hue, currentSettings.Saturation);
	}
	_pictureSettings = currentSettings;
	_needToProcess = _pictureSettings.Hue != 0 || _pictureSettings.Saturation != 0 || _pictureSettings.Brightness || _pictureSettings.Contrast;
}

void DefaultVideoFilter::DecodePpuBuffer(uint16_t *ppuOutputBuffer, uint32_t* outputBuffer, bool displayScanlines)
{
	OverscanDimensions overscan = EmulationSettings::GetOverscanDimensions();
	double scanlineIntensity = 1.0 - EmulationSettings::GetPictureSettings().ScanlineIntensity;
	for(uint32_t i = overscan.Top, iMax = 240 - overscan.Bottom; i < iMax; i++) {
		if(displayScanlines && (i + overscan.Top) % 2 == 0) {
			for(uint32_t j = overscan.Left, jMax = 256 - overscan.Right; j < jMax; j++) {
				*outputBuffer = ProcessIntensifyBits(ppuOutputBuffer[i * 256 + j], scanlineIntensity);
				outputBuffer++;
			}
		} else {
			for(uint32_t j = overscan.Left, jMax = 256 - overscan.Right; j < jMax; j++) {
				*outputBuffer = ProcessIntensifyBits(ppuOutputBuffer[i * 256 + j]);
				outputBuffer++;
			}
		}
	}
}

void DefaultVideoFilter::ApplyFilter(uint16_t *ppuOutputBuffer)
{
	DecodePpuBuffer(ppuOutputBuffer, (uint32_t*)GetOutputBuffer(), true);
}

void DefaultVideoFilter::RgbToYiq(double r, double g, double b, double &y, double &i, double &q)
{
	y = r * 0.299f + g * 0.587f + b * 0.114f;
	i = r * 0.596f - g * 0.275f - b * 0.321f;
	q = r * 0.212f - g * 0.523f + b * 0.311f;
}

void DefaultVideoFilter::YiqToRgb(double y, double i, double q, double &r, double &g, double &b)
{
	r = std::max(0.0, std::min(1.0, (y + _yiqToRgbMatrix[0] * i + _yiqToRgbMatrix[1] * q)));
	g = std::max(0.0, std::min(1.0, (y + _yiqToRgbMatrix[2] * i + _yiqToRgbMatrix[3] * q)));
	b = std::max(0.0, std::min(1.0, (y + _yiqToRgbMatrix[4] * i + _yiqToRgbMatrix[5] * q)));
}

uint32_t DefaultVideoFilter::ProcessIntensifyBits(uint16_t ppuPixel, double scanlineIntensity)
{
	uint32_t pixelOutput = EmulationSettings::GetRgbPalette()[ppuPixel & 0x3F];

	//Incorrect emphasis bit implementation, but will do for now.
	double redChannel = (double)((pixelOutput & 0xFF0000) >> 16);
	double greenChannel = (double)((pixelOutput & 0xFF00) >> 8);
	double blueChannel = (double)(pixelOutput & 0xFF);

	if(ppuPixel & 0x40) {
		//Intensify red
		redChannel *= 1.1;
		greenChannel *= 0.9;
		blueChannel *= 0.9;
	}
	if(ppuPixel & 0x80) {
		//Intensify green
		greenChannel *= 1.1;
		redChannel *= 0.9;
		blueChannel *= 0.9;
	}
	if(ppuPixel & 0x100) {
		//Intensify blue
		blueChannel *= 1.1;
		redChannel *= 0.9;
		greenChannel *= 0.9;
	}

	redChannel = (float)(redChannel > 255 ? 255 : redChannel) / 255.0;
	greenChannel = (float)(greenChannel > 255 ? 255 : greenChannel) / 255.0;
	blueChannel = (float)(blueChannel > 255 ? 255 : blueChannel) / 255.0;

	//Apply brightness, contrast, hue & saturation
	if(_needToProcess) {
		double y, i, q;
		RgbToYiq(redChannel, greenChannel, blueChannel, y, i, q);
		y *= _pictureSettings.Contrast * 0.5f + 1;
		y += _pictureSettings.Brightness * 0.5f;
		YiqToRgb(y, i, q, redChannel, greenChannel, blueChannel);
	}

	if(scanlineIntensity < 1.0) {
		redChannel *= scanlineIntensity;
		greenChannel *= scanlineIntensity;
		blueChannel *= scanlineIntensity;
	}

	int r = std::min(255, (int)(redChannel * 255));
	int g = std::min(255, (int)(greenChannel * 255));
	int b = std::min(255, (int)(blueChannel * 255));

	return 0xFF000000 | (r << 16) | (g << 8) | b;
}