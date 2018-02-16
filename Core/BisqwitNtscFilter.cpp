//NTSC filter based on Bisqwit's code/algorithm
//As described here:
//http://forums.nesdev.com/viewtopic.php?p=172329
#include "stdafx.h"
#include <cmath>
#include "BisqwitNtscFilter.h"
#include "PPU.h"
#include "EmulationSettings.h"

BisqwitNtscFilter::BisqwitNtscFilter(int resDivider)
{
	_resDivider = resDivider;
	_stopThread = false;
	_workDone = false;

	const int8_t signalLumaLow[4] = { -29, -15, 22, 71 };
	const int8_t signalLumaHigh[4] = { 32, 66, 105, 105 };

	//Precalculate the low and high signal chosen for each 64 base colors
	for(int i = 0; i <= 0x3F; i++) {
		int r = (i & 0x0F) >= 0x0E ? 0x1D : i;

		int m = signalLumaLow[r / 0x10];
		int q = signalLumaHigh[r / 0x10];
		if((r & 0x0F) == 13) {
			q = m;
		} else if((r & 0x0F) == 0) {
			m = q;
		}
		_signalLow[i] = m;
		_signalHigh[i] = q;
	}

	_extraThread = std::thread([=]() {
		//Worker thread to improve decode speed
		while(!_stopThread) {
			_waitWork.Wait();
			if(_stopThread) {
				break;
			}

			uint32_t* outputBuffer = (uint32_t*)GetOutputBuffer();

			//Adjust outputbuffer to start at the middle of the picture
			if(_keepVerticalRes) {
				outputBuffer += GetOverscan().GetScreenWidth() * 8 / _resDivider * (120 - GetOverscan().Top);
			} else {
				outputBuffer += GetOverscan().GetScreenWidth() * 64 / _resDivider / _resDivider * (120 - GetOverscan().Top);
			}

			DecodeFrame(120, 239 - GetOverscan().Bottom, _ppuOutputBuffer, outputBuffer, (IsOddFrame() ? 8 : 0) + 327360);

			_workDone = true;
		}
	});
}

BisqwitNtscFilter::~BisqwitNtscFilter()
{
	_stopThread = true;
	_waitWork.Signal();
	_extraThread.join();
}

void BisqwitNtscFilter::ApplyFilter(uint16_t *ppuOutputBuffer)
{
	_ppuOutputBuffer = ppuOutputBuffer;

	_workDone = false;
	_waitWork.Signal();
	DecodeFrame(GetOverscan().Top, 120, ppuOutputBuffer, (uint32_t*)GetOutputBuffer(), (IsOddFrame() ? 8 : 0) + GetOverscan().Top*341*8);
	while(!_workDone) {}
}

FrameInfo BisqwitNtscFilter::GetFrameInfo()
{
	OverscanDimensions overscan = GetOverscan();
	if(_keepVerticalRes) {
		return { overscan.GetScreenWidth() * 8 / _resDivider, overscan.GetScreenHeight(), PPU::ScreenWidth, PPU::ScreenHeight, 4 };
	} else {
		return { overscan.GetScreenWidth() * 8 / _resDivider, overscan.GetScreenHeight() * 8 / _resDivider, PPU::ScreenWidth, PPU::ScreenHeight, 4 };
	}
}

void BisqwitNtscFilter::OnBeforeApplyFilter()
{
	PictureSettings pictureSettings = EmulationSettings::GetPictureSettings();
	NtscFilterSettings ntscSettings = EmulationSettings::GetNtscFilterSettings();

	_keepVerticalRes = ntscSettings.KeepVerticalResolution;

	const double pi = std::atan(1.0) * 4;
	int contrast = (int)((pictureSettings.Contrast + 1.0) * (pictureSettings.Contrast + 1.0) * 167941);
	int saturation = (int)((pictureSettings.Saturation + 1.0) * (pictureSettings.Saturation + 1.0) * 144044);
	for(int i = 0; i < 27; i++) {
		_sinetable[i] = (int8_t)(8 * std::sin(i * 2 * pi / 12 + pictureSettings.Hue * pi));
	}

	_yWidth = (int)(12 + ntscSettings.YFilterLength * 22);
	_iWidth = (int)(12 + ntscSettings.IFilterLength * 22);
	_qWidth = (int)(12 + ntscSettings.QFilterLength * 22);

	_y = contrast / _yWidth;

	_ir = (int)(contrast * 1.994681e-6 * saturation / _iWidth);
	_qr = (int)(contrast * 9.915742e-7 * saturation / _qWidth);

	_ig = (int)(contrast * 9.151351e-8 * saturation / _iWidth);
	_qg = (int)(contrast * -6.334805e-7 * saturation / _qWidth);

	_ib = (int)(contrast * -1.012984e-6 * saturation / _iWidth);
	_qb = (int)(contrast * 1.667217e-6 * saturation / _qWidth);
}

void BisqwitNtscFilter::RecursiveBlend(int iterationCount, uint64_t *output, uint64_t *currentLine, uint64_t *nextLine, int pixelsPerCycle, bool verticalBlend)
{
	//Blend 2 pixels at once
	uint32_t width = GetOverscan().GetScreenWidth() * pixelsPerCycle / 2;

	double scanlineIntensity = 1.0 - EmulationSettings::GetPictureSettings().ScanlineIntensity;
	if(scanlineIntensity < 1.0 && (iterationCount == 2 || _resDivider == 4)) {
		//Most likely extremely inefficient scanlines, but works
		for(uint32_t x = 0; x < width; x++) {
			uint64_t mixed;
			if(verticalBlend) {
				mixed = ((((currentLine[x] ^ nextLine[x]) & 0xfefefefefefefefeL) >> 1) + (currentLine[x] & nextLine[x]));
			} else {
				mixed = currentLine[x];
			}
			
			uint8_t r = (mixed >> 16) & 0xFF, g = (mixed >> 8) & 0xFF, b = mixed & 0xFF;
			uint8_t r2 = (mixed >> 48) & 0xFF, g2 = (mixed >> 40) & 0xFF, b2 = (mixed >> 32) & 0xFF;
			r = (uint8_t)(r * scanlineIntensity);
			g = (uint8_t)(g * scanlineIntensity);
			b = (uint8_t)(b * scanlineIntensity);
			r2 = (uint8_t)(r2 * scanlineIntensity);
			g2 = (uint8_t)(g2 * scanlineIntensity);
			b2 = (uint8_t)(b2 * scanlineIntensity);

			output[x] = ((uint64_t)r2 << 48) | ((uint64_t)g2 << 40) | ((uint64_t)b2 << 32) | (r << 16) | (g << 8) | b;
		}
	} else {
		if(verticalBlend) {
			for(uint32_t x = 0; x < width; x++) {
				output[x] = ((((currentLine[x] ^ nextLine[x]) & 0xfefefefefefefefeL) >> 1) + (currentLine[x] & nextLine[x]));
			}
		} else {
			memcpy(output, currentLine, width * sizeof(uint64_t));
		}
	}

	iterationCount /= 2;
	if(iterationCount > 0) {
		RecursiveBlend(iterationCount, output - width * iterationCount, currentLine, output, pixelsPerCycle, verticalBlend);
		RecursiveBlend(iterationCount, output + width * iterationCount, output, nextLine, pixelsPerCycle, verticalBlend);
	}
}

void BisqwitNtscFilter::GenerateNtscSignal(int8_t *ntscSignal, int &phase, int rowNumber)
{
	for(int x = 0; x < 256; x++) {
		uint16_t color = _ppuOutputBuffer[(rowNumber << 8) | x];

		int8_t low = _signalLow[color & 0x3F];
		int8_t high = _signalHigh[color & 0x3F];
		int8_t emphasis = color >> 6;

		uint16_t phaseBitmask = _bitmaskLut[(phase - (color & 0x0F)) % 12];

		uint8_t voltage;
		for(int j = 0; j < 8; j++) {
			phaseBitmask <<= 1;
			voltage = high;
			if(phaseBitmask >= 0x40) {
				if(phaseBitmask == 0x1000) {
					phaseBitmask = 1;
				} else {
					voltage = low;
				}
			}

			if(phaseBitmask & emphasis) {
				voltage -= voltage / 4;
			}

			ntscSignal[(x << 3) | j] = voltage;
		}

		phase += _signalsPerPixel;
	}
	phase += (341 - 256) * _signalsPerPixel;
}

void BisqwitNtscFilter::DecodeFrame(int startRow, int endRow, uint16_t *ppuOutputBuffer, uint32_t* outputBuffer, int startPhase)
{
	int pixelsPerCycle = 8 / _resDivider;
	int phase = startPhase;
	int8_t rowSignal[256 * _signalsPerPixel];
	uint32_t rowPixelGap = GetOverscan().GetScreenWidth() * pixelsPerCycle;
	if(!_keepVerticalRes) {
		rowPixelGap *= pixelsPerCycle;
	}
	
	uint32_t* orgBuffer = outputBuffer;

	for(int y = startRow; y <= endRow; y++) {
		int startCycle = phase % 12;
		
		//Convert the PPU's output to an NTSC signal
		GenerateNtscSignal(rowSignal, phase, y);

		//Convert the NTSC signal to RGB
		NtscDecodeLine(256 * _signalsPerPixel, rowSignal, outputBuffer, (startCycle + 7) % 12);

		outputBuffer += rowPixelGap;
	}

	if(!_keepVerticalRes) {
		//Generate the missing vertical lines
		outputBuffer = orgBuffer;
		int lastRow = 239 - GetOverscan().Bottom;
		bool verticalBlend = EmulationSettings::GetNtscFilterSettings().VerticalBlend;
		for(int y = startRow; y <= endRow; y++) {
			uint64_t* currentLine = (uint64_t*)outputBuffer;
			uint64_t* nextLine = y == lastRow ? currentLine : (uint64_t*)(outputBuffer + rowPixelGap);
			uint64_t* buffer = (uint64_t*)(outputBuffer + rowPixelGap / 2);

			RecursiveBlend(4 / _resDivider, buffer, currentLine, nextLine, pixelsPerCycle, verticalBlend);

			outputBuffer += rowPixelGap;
		}
	}
}

/**
* NTSC_DecodeLine(Width, Signal, Target, Phase0)
*
* Convert NES NTSC graphics signal into RGB using integer arithmetics only.
*
* Width: Number of NTSC signal samples.
*        For a 256 pixels wide screen, this would be 256*8. 283*8 if you include borders.
*
* Signal: An array of Width samples.
*         The following sample values are recognized:
*          -29 = Luma 0 low   32 = Luma 0 high (-38 and  6 when attenuated)
*          -15 = Luma 1 low   66 = Luma 1 high (-28 and 31 when attenuated)
*           22 = Luma 2 low  105 = Luma 2 high ( -1 and 58 when attenuated)
*           71 = Luma 3 low  105 = Luma 3 high ( 34 and 58 when attenuated)
*         In this scale, sync signal would be -59 and colorburst would be -40 and 19,
*         but these are not interpreted specially in this function.
*         The value is calculated from the relative voltage with:
*                   floor((voltage-0.518)*1000/12)-15
*
* Target: Pointer to a storage for Width RGB32 samples (00rrggbb).
*         Note that the function will produce a RGB32 value for _every_ half-clock-cycle.
*         This means 2264 RGB samples if you render 283 pixels per scanline (incl. borders).
*         The caller can pick and choose those columns they want from the signal
*         to render the picture at their desired resolution.
*
* Phase0: An integer in range 0-11 that describes the phase offset into colors on this scanline.
*         Would be generated from the PPU clock cycle counter at the start of the scanline.
*         In essence it conveys in one integer the same information that real NTSC signal
*         would convey in the colorburst period in the beginning of each scanline.
*/
void BisqwitNtscFilter::NtscDecodeLine(int width, const int8_t* signal, uint32_t* target, int phase0)
{
	auto Read = [=](int pos) -> char { return pos >= 0 ? signal[pos] : 0; };
	auto Cos = [=](int pos) -> char { return _sinetable[(pos + 36) % 12 + phase0]; };
	auto Sin = [=](int pos) -> char { return _sinetable[(pos + 36) % 12 + 3 + phase0]; };

	int brightness = (int)(EmulationSettings::GetPictureSettings().Brightness * 750);
	int ysum = brightness, isum = 0, qsum = 0;
	int leftOverscan = GetOverscan().Left * 8;
	int rightOverscan = width - GetOverscan().Right * 8;

	for(int s = 0; s < rightOverscan; s++) {
		ysum += Read(s) - Read(s - _yWidth);
		isum += Read(s) * Cos(s) - Read(s - _iWidth) * Cos(s - _iWidth);
		qsum += Read(s) * Sin(s) - Read(s - _qWidth) * Sin(s - _qWidth);

		if(!(s % _resDivider) && s >= leftOverscan) {
			int r = std::min(255, std::max(0, (ysum*_y + isum*_ir + qsum*_qr) / 65536));
			int g = std::min(255, std::max(0, (ysum*_y + isum*_ig + qsum*_qg) / 65536));
			int b = std::min(255, std::max(0, (ysum*_y + isum*_ib + qsum*_qb) / 65536));

			*target = 0xFF000000 | (r << 16) | (g << 8) | b;
			target++;
		}
	}
}
