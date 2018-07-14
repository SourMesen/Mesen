#include "stdafx.h"
#include "NtscFilter.h"
#include "PPU.h"
#include "EmulationSettings.h"
#include "Console.h"

NtscFilter::NtscFilter(shared_ptr<Console> console) : BaseVideoFilter(console)
{
	memset(_basePalette, 0, 64 * 3);
	_ntscData = new nes_ntsc_t();
	_ntscSetup = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
	_ntscBuffer = new uint32_t[NES_NTSC_OUT_WIDTH(256) * 240];
}

FrameInfo NtscFilter::GetFrameInfo()
{
	OverscanDimensions overscan = GetOverscan();
	uint32_t overscanLeft = overscan.Left > 0 ? NES_NTSC_OUT_WIDTH(overscan.Left) : 0;
	uint32_t overscanRight = overscan.Right > 0 ? NES_NTSC_OUT_WIDTH(overscan.Right) : 0;

	if(_keepVerticalRes) {
		return {
			(NES_NTSC_OUT_WIDTH(PPU::ScreenWidth) - overscanLeft - overscanRight),
			(PPU::ScreenHeight - overscan.Top - overscan.Bottom),
			NES_NTSC_OUT_WIDTH(PPU::ScreenWidth),
			PPU::ScreenHeight,
			4
		};
	} else {
		return {
			NES_NTSC_OUT_WIDTH(PPU::ScreenWidth) - overscanLeft - overscanRight,
			(PPU::ScreenHeight - overscan.Top - overscan.Bottom) * 2,
			NES_NTSC_OUT_WIDTH(PPU::ScreenWidth),
			PPU::ScreenHeight * 2,
			4
		};
	}
}

void NtscFilter::OnBeforeApplyFilter()
{
	bool paletteChanged = false;
	uint32_t* palette = _console->GetSettings()->GetRgbPalette();
	for(int i = 0; i < 64; i++) {
		uint8_t r = (palette[i] >> 16) & 0xFF;
		uint8_t g = (palette[i] >> 8) & 0xFF;
		uint8_t b = palette[i] & 0xFF;

		if(_basePalette[i * 3] != r || _basePalette[i * 3 + 1] != g || _basePalette[i * 3 + 2] != b) {
			paletteChanged = true;

			_basePalette[i * 3] = (palette[i] >> 16) & 0xFF;
			_basePalette[i * 3 + 1] = (palette[i] >> 8) & 0xFF;
			_basePalette[i * 3 + 2] = palette[i] & 0xFF;
		}
	}

	PictureSettings pictureSettings = _console->GetSettings()->GetPictureSettings();
	NtscFilterSettings ntscSettings = _console->GetSettings()->GetNtscFilterSettings();

	_keepVerticalRes = ntscSettings.KeepVerticalResolution;

	if(paletteChanged || _ntscSetup.hue != pictureSettings.Hue || _ntscSetup.saturation != pictureSettings.Saturation || _ntscSetup.brightness != pictureSettings.Brightness || _ntscSetup.contrast != pictureSettings.Contrast ||
		_ntscSetup.artifacts != ntscSettings.Artifacts || _ntscSetup.bleed != ntscSettings.Bleed || _ntscSetup.fringing != ntscSettings.Fringing || _ntscSetup.gamma != ntscSettings.Gamma ||
		(_ntscSetup.merge_fields == 1) != ntscSettings.MergeFields || _ntscSetup.resolution != ntscSettings.Resolution || _ntscSetup.sharpness != ntscSettings.Sharpness) {
		
		_ntscSetup.hue = pictureSettings.Hue;
		_ntscSetup.saturation = pictureSettings.Saturation;
		_ntscSetup.brightness = pictureSettings.Brightness;
		_ntscSetup.contrast = pictureSettings.Contrast;

		_ntscSetup.artifacts = ntscSettings.Artifacts;
		_ntscSetup.bleed = ntscSettings.Bleed;
		_ntscSetup.fringing = ntscSettings.Fringing;
		_ntscSetup.gamma = ntscSettings.Gamma;
		_ntscSetup.merge_fields = (int)ntscSettings.MergeFields;
		_ntscSetup.resolution = ntscSettings.Resolution;
		_ntscSetup.sharpness = ntscSettings.Sharpness;

		_ntscSetup.base_palette = _console->GetSettings()->IsDefaultPalette() ? nullptr : _basePalette;

		nes_ntsc_init(_ntscData, &_ntscSetup);
	}
}

void NtscFilter::ApplyFilter(uint16_t *ppuOutputBuffer)
{
	nes_ntsc_blit(_ntscData, ppuOutputBuffer, PPU::ScreenWidth, IsOddFrame() ? 0 : 1, PPU::ScreenWidth, 240, _ntscBuffer, NES_NTSC_OUT_WIDTH(PPU::ScreenWidth)*4);
	GenerateArgbFrame(_ntscBuffer);
}

void NtscFilter::GenerateArgbFrame(uint32_t *ntscBuffer)
{
	uint32_t* outputBuffer = GetOutputBuffer();
	OverscanDimensions overscan = GetOverscan();
	int overscanLeft = overscan.Left > 0 ? NES_NTSC_OUT_WIDTH(overscan.Left) : 0;
	int overscanRight = overscan.Right > 0 ? NES_NTSC_OUT_WIDTH(overscan.Right) : 0;
	int rowWidth = NES_NTSC_OUT_WIDTH(PPU::ScreenWidth);
	int rowWidthOverscan = rowWidth - overscanLeft - overscanRight;

	if(_keepVerticalRes) {
		ntscBuffer += rowWidth * overscan.Top + overscanLeft;
		for(uint32_t i = 0, len = overscan.GetScreenHeight(); i < len; i++) {
			memcpy(outputBuffer, ntscBuffer, rowWidthOverscan * sizeof(uint32_t));
			outputBuffer += rowWidthOverscan;
			ntscBuffer += rowWidth;
		}
	} else {
		double scanlineIntensity = 1.0 - _console->GetSettings()->GetPictureSettings().ScanlineIntensity;
		bool verticalBlend = _console->GetSettings()->GetNtscFilterSettings().VerticalBlend;

		for(int y = PPU::ScreenHeight - 1 - overscan.Bottom; y >= (int)overscan.Top; y--) {
			uint32_t const* in = ntscBuffer + y * rowWidth;
			uint32_t* out = outputBuffer + (y - overscan.Top) * 2 * rowWidthOverscan;

			if(verticalBlend || scanlineIntensity < 1.0) {
				for(int x = 0; x < rowWidthOverscan; x++) {
					uint32_t prev = in[overscanLeft];
					uint32_t next = y < 239 ? in[overscanLeft + rowWidth] : 0;

					*out = 0xFF000000 | prev;

					/* mix 24-bit rgb without losing low bits */
					uint32_t mixed;
					if(verticalBlend) {
						mixed = (prev + next + ((prev ^ next) & 0x030303)) >> 1;
					} else {
						mixed = prev;
					}

					if(scanlineIntensity < 1.0) {
						uint8_t r = (mixed >> 16) & 0xFF, g = (mixed >> 8) & 0xFF, b = mixed & 0xFF;
						r = (uint8_t)(r * scanlineIntensity);
						g = (uint8_t)(g * scanlineIntensity);
						b = (uint8_t)(b * scanlineIntensity);
						*(out + rowWidthOverscan) = 0xFF000000 | (r << 16) | (g << 8) | b;
					} else {
						*(out + rowWidthOverscan) = 0xFF000000 | mixed;
					}
					in++;
					out++;
				}
			} else {
				memcpy(out, in + overscanLeft, rowWidthOverscan * sizeof(uint32_t));
				memcpy(out + rowWidthOverscan, in + overscanLeft, rowWidthOverscan * sizeof(uint32_t));
			}
		}
	}
}

NtscFilter::~NtscFilter()
{
	delete _ntscData;
	delete[] _ntscBuffer;
}