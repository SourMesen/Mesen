#include "stdafx.h"
#include "EmulationSettings.h"

uint32_t EmulationSettings::Flags = 0;
uint32_t EmulationSettings::AudioLatency = 20000;
double EmulationSettings::ChannelVolume[5] = { 0.5f, 0.5f, 0.5f, 0.5f, 0.5f };
NesModel EmulationSettings::Model = NesModel::Auto;
OverscanDimensions EmulationSettings::Overscan;
uint32_t EmulationSettings::EmulationSpeed = 100;