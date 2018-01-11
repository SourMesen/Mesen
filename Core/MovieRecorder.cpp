#include "stdafx.h"
#include "../Utilities/HexUtilities.h"
#include "../Utilities/FolderUtilities.h"
#include "../Utilities/ZipWriter.h"
#include "MovieRecorder.h"
#include "ControlManager.h"
#include "BaseControlDevice.h"
#include "Console.h"
#include "CheatManager.h"
#include "VirtualFile.h"
#include "SaveStateManager.h"

MovieRecorder::MovieRecorder()
{
}

MovieRecorder::~MovieRecorder()
{
}

bool MovieRecorder::Record(RecordMovieOptions options)
{
	_filename = options.Filename;
	_author = options.Author;
	_description = options.Description;
	_writer.reset(new ZipWriter());
	_inputData = stringstream();
	_saveStateData = stringstream();
	_hasSaveState = false;

	if(!_writer->Initialize(_filename)) {
		_writer.reset();
		return false;
	} else {
		Console::Pause();

		if(options.RecordFrom == RecordMovieFrom::StartWithoutSaveData) {
			BatteryManager::SetBatteryProvider(shared_from_this());
		}

		//Save existing battery files
		if(options.RecordFrom == RecordMovieFrom::StartWithSaveData) {
			BatteryManager::SetBatteryRecorder(shared_from_this());
		}
		ControlManager::RegisterInputRecorder(this);
		if(options.RecordFrom == RecordMovieFrom::CurrentState) {
			SaveStateManager::SaveState(_saveStateData);
			_hasSaveState = true;
		} else {
			Console::GetInstance()->PowerCycle();
		}
		BatteryManager::SetBatteryRecorder(nullptr);
		Console::Resume();

		MessageManager::DisplayMessage("Movies", "MovieRecordingTo", FolderUtilities::GetFilename(_filename, true));

		return true;
	}
}

void MovieRecorder::GetGameSettings(stringstream &out)
{
	NesModel model = Console::GetModel();

	WriteString(out, MovieKeys::MesenVersion, EmulationSettings::GetMesenVersionString());
	WriteInt(out, MovieKeys::MovieFormatVersion, MovieRecorder::MovieFormatVersion);

	VirtualFile romFile = Console::GetRomPath();
	WriteString(out, MovieKeys::GameFile, romFile.GetFileName());
	WriteString(out, MovieKeys::Sha1, romFile.GetSha1Hash());

	VirtualFile patchFile = Console::GetPatchFile();
	if(patchFile.IsValid()) {
		WriteString(out, MovieKeys::PatchFile, patchFile.GetFileName());
		WriteString(out, MovieKeys::PatchFileSha1, patchFile.GetSha1Hash());
		WriteString(out, MovieKeys::PatchedRomSha1, Console::GetHashInfo().Sha1Hash);
	}

	switch(model) {
		case NesModel::NTSC: WriteString(out, MovieKeys::Region, "NTSC"); break;
		case NesModel::PAL: WriteString(out, MovieKeys::Region, "PAL"); break;
		case NesModel::Dendy: WriteString(out, MovieKeys::Region, "Dendy"); break;
	}

	switch(EmulationSettings::GetConsoleType()) {
		case ConsoleType::Nes: WriteString(out, MovieKeys::ConsoleType, "NES"); break;
		case ConsoleType::Famicom: WriteString(out, MovieKeys::ConsoleType, "Famicom"); break;
	}
	
	WriteString(out, MovieKeys::Controller1, ControllerTypeNames[(int)EmulationSettings::GetControllerType(0)]);
	WriteString(out, MovieKeys::Controller2, ControllerTypeNames[(int)EmulationSettings::GetControllerType(1)]);
	if(EmulationSettings::CheckFlag(EmulationFlags::HasFourScore)) {
		WriteString(out, MovieKeys::Controller3, ControllerTypeNames[(int)EmulationSettings::GetControllerType(2)]);
		WriteString(out, MovieKeys::Controller4, ControllerTypeNames[(int)EmulationSettings::GetControllerType(3)]);
	}

	if(EmulationSettings::GetConsoleType() == ConsoleType::Famicom) {
		WriteString(out, MovieKeys::ExpansionDevice, ExpansionPortDeviceNames[(int)EmulationSettings::GetExpansionDevice()]);
	}
	
	WriteInt(out, MovieKeys::CpuClockRate, EmulationSettings::GetOverclockRateSetting());
	WriteInt(out, MovieKeys::ExtraScanlinesBeforeNmi, EmulationSettings::GetPpuExtraScanlinesBeforeNmi());
	WriteInt(out, MovieKeys::ExtraScanlinesAfterNmi, EmulationSettings::GetPpuExtraScanlinesAfterNmi());
	WriteInt(out, MovieKeys::InputPollScanline, EmulationSettings::GetInputPollScanline());
	
	if(EmulationSettings::GetOverclockRateSetting() != 100) {
		WriteBool(out, MovieKeys::OverclockAdjustApu, EmulationSettings::GetOverclockAdjustApu());
	}
	WriteBool(out, MovieKeys::DisablePpu2004Reads, EmulationSettings::CheckFlag(EmulationFlags::DisablePpu2004Reads));
	WriteBool(out, MovieKeys::DisablePaletteRead, EmulationSettings::CheckFlag(EmulationFlags::DisablePaletteRead));
	WriteBool(out, MovieKeys::DisableOamAddrBug, EmulationSettings::CheckFlag(EmulationFlags::DisableOamAddrBug));
	WriteBool(out, MovieKeys::UseNes101Hvc101Behavior, EmulationSettings::CheckFlag(EmulationFlags::UseNes101Hvc101Behavior));
	WriteBool(out, MovieKeys::EnableOamDecay, EmulationSettings::CheckFlag(EmulationFlags::EnableOamDecay));
	WriteBool(out, MovieKeys::DisablePpuReset, EmulationSettings::CheckFlag(EmulationFlags::DisablePpuReset));

	WriteInt(out, MovieKeys::ZapperDetectionRadius, EmulationSettings::GetZapperDetectionRadius());

	switch(EmulationSettings::GetRamPowerOnState()) {
		case RamPowerOnState::AllZeros: WriteInt(out, MovieKeys::RamPowerOnState, 0x00); break;
		case RamPowerOnState::AllOnes: WriteInt(out, MovieKeys::RamPowerOnState, 0xFF); break;
		case RamPowerOnState::Random: WriteInt(out, MovieKeys::RamPowerOnState, -1); break; //TODO: Shouldn't be used for movies
	}	

	//VS System flags
	if(Console::GetInstance()->GetAvailableFeatures() == ConsoleFeatures::VsSystem) {
		WriteString(out, MovieKeys::DipSwitches, HexUtilities::ToHex(EmulationSettings::GetDipSwitches()));
		WriteString(out, MovieKeys::PpuModel, PpuModelNames[(int)EmulationSettings::GetPpuModel()]);
	}

	for(CodeInfo &code : CheatManager::GetCheats()) {
		WriteCheat(out, code);
	}
}

void MovieRecorder::WriteCheat(stringstream &out, CodeInfo &code)
{
	out << "Cheat " << 
		HexUtilities::ToHex(code.Address) << " " << 
		HexUtilities::ToHex(code.Value) << " " << 
		(code.IsRelativeAddress ? "true" : "false") << " " <<
		(code.CompareValue < 0 ? HexUtilities::ToHex((uint8_t)code.CompareValue) : "") << "\n";
}

void MovieRecorder::WriteString(stringstream &out, string name, string value)
{
	out << name << " " << value << "\n";
}

void MovieRecorder::WriteInt(stringstream &out, string name, uint32_t value)
{
	out << name << " " << std::to_string(value) << "\n";
}

void MovieRecorder::WriteBool(stringstream &out, string name, bool enabled)
{
	out << name << " " << (enabled ? "true" : "false") << "\n";
}

bool MovieRecorder::Stop()
{
	if(_writer) {
		ControlManager::UnregisterInputRecorder(this);

		_writer->AddFile(_inputData, "Input.txt");

		stringstream out;
		GetGameSettings(out);
		_writer->AddFile(out, "GameSettings.txt");

		if(!_author.empty() || !_description.empty()) {
			stringstream movieInfo;
			WriteString(movieInfo, "Author", _author);
			movieInfo << "Description\n" << _description;
			_writer->AddFile(movieInfo, "MovieInfo.txt");
		}

		VirtualFile patchFile = Console::GetPatchFile();
		vector<uint8_t> patchData;
		if(patchFile.IsValid() && patchFile.ReadFile(patchData)) {
			_writer->AddFile(patchData, "PatchData.dat");
		}

		if(_hasSaveState) {
			_writer->AddFile(_saveStateData, "SaveState.mst");
		}

		for(auto kvp : _batteryData) {
			_writer->AddFile(kvp.second, "Battery" + kvp.first);
		}

		bool result = _writer->Save();
		if(result) {
			MessageManager::DisplayMessage("Movies", "MovieSaved", FolderUtilities::GetFilename(_filename, true));
		}
		return result;
	}

	return false;
}

void MovieRecorder::RecordInput(vector<shared_ptr<BaseControlDevice>> devices)
{
	for(shared_ptr<BaseControlDevice> &device : devices) {
		_inputData << ("|" + device->GetTextState());
	}
	_inputData << "\n";
}

void MovieRecorder::OnLoadBattery(string extension, vector<uint8_t> batteryData)
{
	_batteryData[extension] = batteryData;
}

vector<uint8_t> MovieRecorder::LoadBattery(string extension)
{
	return vector<uint8_t>();
}
