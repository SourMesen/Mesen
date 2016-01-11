#include "stdafx.h"
#include "EmulationSettings.h"

uint32_t EmulationSettings::_flags = EmulationFlags::LowLatency;

bool EmulationSettings::_audioEnabled = true;
uint32_t EmulationSettings::_audioLatency = 20000;
double EmulationSettings::_channelVolume[5] = { 0.5f, 0.5f, 0.5f, 0.5f, 0.5f };

NesModel EmulationSettings::_model = NesModel::Auto;

uint32_t EmulationSettings::_emulationSpeed = 100;

OverscanDimensions EmulationSettings::_overscan;
VideoFilterType EmulationSettings::_videoFilterType = VideoFilterType::None;
uint32_t EmulationSettings::_videoScale = 1;