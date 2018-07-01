#pragma once
#include "stdafx.h"
#include <unordered_map>
#include "IInputRecorder.h"
#include "BatteryManager.h"
#include "Types.h"

class ZipWriter;
class Console;
struct CodeInfo;

class MovieRecorder : public IInputRecorder, public IBatteryRecorder, public IBatteryProvider, public std::enable_shared_from_this<MovieRecorder>
{
private:
	static const uint32_t MovieFormatVersion = 1;

	shared_ptr<Console> _console;
	string _filename;
	string _author;
	string _description;
	unique_ptr<ZipWriter> _writer;
	std::unordered_map<string, vector<uint8_t>> _batteryData;
	stringstream _inputData;
	bool _hasSaveState;
	stringstream _saveStateData;

	void GetGameSettings(stringstream &out);
	void WriteCheat(stringstream &out, CodeInfo &code);
	void WriteString(stringstream &out, string name, string value);
	void WriteInt(stringstream &out, string name, uint32_t value);
	void WriteBool(stringstream &out, string name, bool enabled);

public:
	MovieRecorder(shared_ptr<Console> console);
	virtual ~MovieRecorder();

	bool Record(RecordMovieOptions options);
	bool Stop();

	void RecordInput(vector<shared_ptr<BaseControlDevice>> devices) override;

	// Inherited via IBatteryRecorder
	virtual void OnLoadBattery(string extension, vector<uint8_t> batteryData) override;

	// Inherited via IBatteryProvider
	virtual vector<uint8_t> LoadBattery(string extension) override;
};

namespace MovieKeys
{
	constexpr const char* MesenVersion = "MesenVersion";
	constexpr const char* MovieFormatVersion = "MovieFormatVersion";
	constexpr const char* GameFile = "GameFile";
	constexpr const char* Sha1 = "SHA1";
	constexpr const char* PatchFile = "PatchFile";
	constexpr const char* PatchFileSha1 = "PatchFileSHA1";
	constexpr const char* PatchedRomSha1 = "PatchedRomSHA1";
	constexpr const char* Region = "Region";
	constexpr const char* ConsoleType = "ConsoleType";
	constexpr const char* Controller1 = "Controller1";
	constexpr const char* Controller2 = "Controller2";
	constexpr const char* Controller3 = "Controller3";
	constexpr const char* Controller4 = "Controller4";
	constexpr const char* ExpansionDevice = "ExpansionDevice";
	constexpr const char* CpuClockRate = "CpuClockRate";
	constexpr const char* ExtraScanlinesBeforeNmi = "ExtraScanlinesBeforeNmi";
	constexpr const char* ExtraScanlinesAfterNmi = "ExtraScanlinesAfterNmi";
	constexpr const char* OverclockAdjustApu = "OverclockAdjustApu";
	constexpr const char* DisablePpu2004Reads = "DisablePpu2004Reads";
	constexpr const char* DisablePaletteRead = "DisablePaletteRead";
	constexpr const char* DisableOamAddrBug = "DisableOamAddrBug";
	constexpr const char* UseNes101Hvc101Behavior = "UseNes101Hvc101Behavior";
	constexpr const char* EnableOamDecay = "EnableOamDecay";
	constexpr const char* DisablePpuReset = "DisablePpuReset";
	constexpr const char* ZapperDetectionRadius = "ZapperDetectionRadius";
	constexpr const char* RamPowerOnState = "RamPowerOnState";
	constexpr const char* PpuModel = "PpuModel";
	constexpr const char* DipSwitches = "DipSwitches";
	constexpr const char* InputPollScanline = "InputPollScanline";
};