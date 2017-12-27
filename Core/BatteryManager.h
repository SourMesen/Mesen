#pragma once
#include "stdafx.h"

class IBatteryProvider
{
public:
	virtual vector<uint8_t> LoadBattery(string extension) = 0;
};

class IBatteryRecorder
{
public:
	virtual void OnLoadBattery(string extension, vector<uint8_t> batteryData) = 0;
};

class BatteryManager
{
private:
	static string _romName;
	static bool _saveEnabled;
	static string GetBasePath();

	static std::weak_ptr<IBatteryProvider> _provider;
	static std::weak_ptr<IBatteryRecorder> _recorder;

	BatteryManager() = delete;

public:
	static void Initialize(string romName);

	static void SetSaveEnabled(bool enabled);
	
	static void SetBatteryProvider(shared_ptr<IBatteryProvider> provider);
	static void SetBatteryRecorder(shared_ptr<IBatteryRecorder> recorder);
	
	static void SaveBattery(string extension, uint8_t* data, uint32_t length);
	
	static vector<uint8_t> LoadBattery(string extension);
	static void LoadBattery(string extension, uint8_t* data, uint32_t length);
};