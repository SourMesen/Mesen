#include "stdafx.h"
#include "NtscFilter.h"
#include "PPU.h"
#include "EmulationSettings.h"

NtscFilter::NtscFilter()
{
	memset(_basePalette, 0, 64 * 3);
	_ntscData = new nes_ntsc_t();
	_ntscSetup = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
}

FrameInfo NtscFilter::GetFrameInfo()
{
	OverscanDimensions overscan = GetOverscan();
	int overscanLeft = overscan.Left > 0 ? NES_NTSC_OUT_WIDTH(overscan.Left) : 0;
	int overscanRight = overscan.Right > 0 ? NES_NTSC_OUT_WIDTH(overscan.Right) : 0;
	return { NES_NTSC_OUT_WIDTH(PPU::ScreenWidth) - overscanLeft - overscanRight, (PPU::ScreenHeight - overscan.Top - overscan.Bottom) * 2, NES_NTSC_OUT_WIDTH(PPU::ScreenWidth), PPU::ScreenHeight * 2, 4 };
}

void NtscFilter::OnBeforeApplyFilter()
{
	bool paletteChanged = false;
	uint32_t* palette = EmulationSettings::GetRgbPalette();
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

	PictureSettings pictureSettings = EmulationSettings::GetPictureSettings();
	NtscFilterSettings ntscSettings = EmulationSettings::GetNtscFilterSettings();
	if(paletteChanged || _ntscSetup.hue != pictureSettings.Hue || _ntscSetup.saturation != pictureSettings.Saturation || _ntscSetup.brightness != pictureSettings.Brightness || _ntscSetup.contrast != pictureSettings.Contrast ||
		_ntscSetup.artifacts != ntscSettings.Artifacts || _ntscSetup.bleed != ntscSettings.Bleed || _ntscSetup.fringing != ntscSettings.Fringing || _ntscSetup.gamma != ntscSettings.Gamma ||
		(_ntscSetup.merge_fields == 1) != ntscSettings.MergeFields || _ntscSetup.resolution != ntscSettings.Resolution || _ntscSetup.sharpness != ntscSettings.Sharpness) {
		
		_ntscData = new nes_ntsc_t();
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

		_ntscSetup.base_palette = EmulationSettings::IsDefaultPalette() ? nullptr : _basePalette;

		nes_ntsc_init(_ntscData, &_ntscSetup);
	}
}

void NtscFilter::ApplyFilter(uint16_t *ppuOutputBuffer)
{
	static bool oddFrame = false;
	oddFrame = !oddFrame;

	uint32_t* ntscBuffer = new uint32_t[NES_NTSC_OUT_WIDTH(256) * 240];
	nes_ntsc_blit(_ntscData, ppuOutputBuffer, PPU::ScreenWidth, oddFrame ? 0 : 1, PPU::ScreenWidth, 240, ntscBuffer, NES_NTSC_OUT_WIDTH(PPU::ScreenWidth)*4);
	DoubleOutputHeight(ntscBuffer);
	delete[] ntscBuffer;
}

void NtscFilter::DoubleOutputHeight(uint32_t *ntscBuffer)
{
	uint32_t* outputBuffer = (uint32_t*)GetOutputBuffer();
	OverscanDimensions overscan = GetOverscan();
	int overscanLeft = overscan.Left > 0 ? NES_NTSC_OUT_WIDTH(overscan.Left) : 0;
	int overscanRight = overscan.Right > 0 ? NES_NTSC_OUT_WIDTH(overscan.Right) : 0;
	int rowWidth = NES_NTSC_OUT_WIDTH(PPU::ScreenWidth);
	int rowWidthOverscan = rowWidth - overscanLeft - overscanRight;

	double scanlineIntensity = 1.0 - EmulationSettings::GetPictureSettings().ScanlineIntensity;

	for(int y = PPU::ScreenHeight - 1 - overscan.Bottom; y >= (int)overscan.Top; y--) {
		uint32_t const* in = ntscBuffer + y * rowWidth;
		uint32_t* out = outputBuffer + (y - overscan.Top) * 2 * rowWidthOverscan;

		for(int x = 0; x < rowWidthOverscan; x++) {
			uint32_t prev = in[overscanLeft];
			uint32_t next = y < 239 ? in[overscanLeft + rowWidth] : 0;
			
			*out = prev;

			/* mix 24-bit rgb without losing low bits */
			uint32_t mixed = (prev + next + ((prev ^ next) & 0x030303)) >> 1;

			if(scanlineIntensity < 1.0) {
				uint8_t r = (mixed >> 16) & 0xFF, g = (mixed >> 8) & 0xFF, b = mixed & 0xFF;
				r = (uint8_t)(r * scanlineIntensity);
				g = (uint8_t)(g * scanlineIntensity);
				b = (uint8_t)(b * scanlineIntensity);
				*(out + rowWidthOverscan) = (r << 16) | (g << 8) | b;
			} else {
				*(out + rowWidthOverscan) = mixed;
			}
			in++;
			out++;
		}
	}
}

NtscFilter::~NtscFilter()
{
	delete _ntscData;
}