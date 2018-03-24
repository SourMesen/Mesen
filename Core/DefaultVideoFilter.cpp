#include "stdafx.h"
#include "DefaultVideoFilter.h"
#include "EmulationSettings.h"
#define _USE_MATH_DEFINES
#include <math.h>
#include <algorithm>
#include "PPU.h"
#include "DebugHud.h"

DefaultVideoFilter::DefaultVideoFilter()
{
	InitDecodeTables();

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
	return { overscan.GetScreenWidth(), overscan.GetScreenHeight(), PPU::ScreenWidth, PPU::ScreenHeight, 4 };
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
	uint32_t* out = outputBuffer;
	OverscanDimensions overscan = GetOverscan();
	double scanlineIntensity = 1.0 - EmulationSettings::GetPictureSettings().ScanlineIntensity;
	for(uint32_t i = overscan.Top, iMax = 240 - overscan.Bottom; i < iMax; i++) {
		if(displayScanlines && (i + overscan.Top) % 2 == 0) {
			for(uint32_t j = overscan.Left, jMax = 256 - overscan.Right; j < jMax; j++) {
				*out = ProcessIntensifyBits(ppuOutputBuffer[i * 256 + j], scanlineIntensity);
				out++;
			}
		} else {
			for(uint32_t j = overscan.Left, jMax = 256 - overscan.Right; j < jMax; j++) {
				*out = ProcessIntensifyBits(ppuOutputBuffer[i * 256 + j]);
				out++;
			}
		}
	}
}

void DefaultVideoFilter::ApplyFilter(uint16_t *ppuOutputBuffer)
{
	DecodePpuBuffer(ppuOutputBuffer, (uint32_t*)GetOutputBuffer(), EmulationSettings::GetVideoFilterType() <= VideoFilterType::BisqwitNtsc);
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

void DefaultVideoFilter::InitDecodeTables()
{
	for(int i = 0; i < 256; i++) {
		for(int j = 0; j < 8; j++) {
			double redColor = i;
			double greenColor = i;
			double blueColor = i;
			if(j & 0x01) {
				//Intensify red
				redColor *= 1.1;
				greenColor *= 0.9;
				blueColor *= 0.9;
			}
			if(j & 0x02) {
				//Intensify green
				greenColor *= 1.1;
				redColor *= 0.9;
				blueColor *= 0.9;
			}
			if(j & 0x04) {
				//Intensify blue
				blueColor *= 1.1;
				redColor *= 0.9;
				greenColor *= 0.9;
			}

			redColor = (redColor > 255 ? 255 : redColor) / 255.0;
			greenColor = (greenColor > 255 ? 255 : greenColor) / 255.0;
			blueColor = (blueColor > 255 ? 255 : blueColor) / 255.0;

			_redDecodeTable[i][j] = redColor;
			_greenDecodeTable[i][j] = greenColor;
			_blueDecodeTable[i][j] = blueColor;
		}
	}
}

uint32_t DefaultVideoFilter::ProcessIntensifyBits(uint16_t ppuPixel, double scanlineIntensity)
{
	uint32_t pixelOutput = EmulationSettings::GetRgbPalette()[ppuPixel & 0x3F];
	uint32_t intensifyBits = (ppuPixel >> 6) & 0x07;

	if(intensifyBits || _needToProcess || scanlineIntensity < 1.0) {
		//Incorrect emphasis bit implementation, but will do for now.
		double redChannel = _redDecodeTable[((pixelOutput & 0xFF0000) >> 16)][intensifyBits];
		double greenChannel = _greenDecodeTable[((pixelOutput & 0xFF00) >> 8)][intensifyBits];
		double blueChannel = _blueDecodeTable[(pixelOutput & 0xFF)][intensifyBits];

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
	} else {
		return pixelOutput;
	}
}