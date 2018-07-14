#pragma once

#include "stdafx.h"
#include "BaseVideoFilter.h"

class DefaultVideoFilter : public BaseVideoFilter
{
private:
	double _redDecodeTable[256][8];
	double _greenDecodeTable[256][8];
	double _blueDecodeTable[256][8];

	double _yiqToRgbMatrix[6];
	PictureSettings _pictureSettings;
	bool _needToProcess = false;

	void InitDecodeTables();
	void InitConversionMatrix(double hueShift, double saturationShift);

	void RgbToYiq(double r, double g, double b, double &y, double &i, double &q);
	void YiqToRgb(double y, double i, double q, double &r, double &g, double &b);

protected:
	void DecodePpuBuffer(uint16_t *ppuOutputBuffer, uint32_t* outputBuffer, bool displayScanlines);
	uint32_t ProcessIntensifyBits(uint16_t ppuPixel, double scanlineIntensity = 1.0);
	void OnBeforeApplyFilter();

public:
	DefaultVideoFilter(shared_ptr<Console> console);
	void ApplyFilter(uint16_t *ppuOutputBuffer);
	FrameInfo GetFrameInfo();
};