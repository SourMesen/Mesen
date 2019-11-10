#include "stdafx.h"
#include <deque>
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
#include "NotificationManager.h"
#include "RomData.h"
#include "RewindData.h"

MovieRecorder::MovieRecorder(shared_ptr<Console> console)
{
	_console = console;
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
		_console->Pause();

		if(options.RecordFrom == RecordMovieFrom::StartWithoutSaveData) {
			_console->GetBatteryManager()->SetBatteryProvider(shared_from_this());
		}

		//Save existing battery files
		if(options.RecordFrom == RecordMovieFrom::StartWithSaveData) {
			_console->GetBatteryManager()->SetBatteryRecorder(shared_from_this());
		}
		
		_console->GetNotificationManager()->RegisterNotificationListener(shared_from_this());
		if(options.RecordFrom == RecordMovieFrom::CurrentState) {
			_console->GetControlManager()->RegisterInputRecorder(this);
			_console->GetSaveStateManager()->SaveState(_saveStateData);
			_hasSaveState = true;
		} else {
			_console->PowerCycle();
		}
		_console->GetBatteryManager()->SetBatteryRecorder(nullptr);
		_console->Resume();

		MessageManager::DisplayMessage("Movies", "MovieRecordingTo", FolderUtilities::GetFilename(_filename, true));

		return true;
	}
}

void MovieRecorder::GetGameSettings(stringstream &out)
{
	EmulationSettings* settings = _console->GetSettings();
	NesModel model = _console->GetModel();

	WriteString(out, MovieKeys::MesenVersion, settings->GetMesenVersionString());
	WriteInt(out, MovieKeys::MovieFormatVersion, MovieRecorder::MovieFormatVersion);

	VirtualFile romFile = _console->GetRomPath();
	WriteString(out, MovieKeys::GameFile, romFile.GetFileName());
	WriteString(out, MovieKeys::Sha1, romFile.GetSha1Hash());

	VirtualFile patchFile = _console->GetPatchFile();
	if(patchFile.IsValid()) {
		WriteString(out, MovieKeys::PatchFile, patchFile.GetFileName());
		WriteString(out, MovieKeys::PatchFileSha1, patchFile.GetSha1Hash());
		WriteString(out, MovieKeys::PatchedRomSha1, _console->GetRomInfo().Hash.Sha1);
	}

	switch(model) {
		case NesModel::Auto: break; //Console::GetModel() will never return Auto.
		case NesModel::NTSC: WriteString(out, MovieKeys::Region, "NTSC"); break;
		case NesModel::PAL: WriteString(out, MovieKeys::Region, "PAL"); break;
		case NesModel::Dendy: WriteString(out, MovieKeys::Region, "Dendy"); break;
	}

	switch(settings->GetConsoleType()) {
		case ConsoleType::Nes: WriteString(out, MovieKeys::ConsoleType, "NES"); break;
		case ConsoleType::Famicom: WriteString(out, MovieKeys::ConsoleType, "Famicom"); break;
	}
	
	WriteString(out, MovieKeys::Controller1, ControllerTypeNames[(int)settings->GetControllerType(0)]);
	WriteString(out, MovieKeys::Controller2, ControllerTypeNames[(int)settings->GetControllerType(1)]);
	if(settings->CheckFlag(EmulationFlags::HasFourScore)) {
		WriteString(out, MovieKeys::Controller3, ControllerTypeNames[(int)settings->GetControllerType(2)]);
		WriteString(out, MovieKeys::Controller4, ControllerTypeNames[(int)settings->GetControllerType(3)]);
	}

	if(settings->GetConsoleType() == ConsoleType::Famicom) {
		WriteString(out, MovieKeys::ExpansionDevice, ExpansionPortDeviceNames[(int)settings->GetExpansionDevice()]);
	}
	
	WriteInt(out, MovieKeys::ExtraScanlinesBeforeNmi, settings->GetPpuExtraScanlinesBeforeNmi());
	WriteInt(out, MovieKeys::ExtraScanlinesAfterNmi, settings->GetPpuExtraScanlinesAfterNmi());
	WriteInt(out, MovieKeys::InputPollScanline, settings->GetInputPollScanline());
	
	WriteBool(out, MovieKeys::DisablePpu2004Reads, settings->CheckFlag(EmulationFlags::DisablePpu2004Reads));
	WriteBool(out, MovieKeys::DisablePaletteRead, settings->CheckFlag(EmulationFlags::DisablePaletteRead));
	WriteBool(out, MovieKeys::DisableOamAddrBug, settings->CheckFlag(EmulationFlags::DisableOamAddrBug));
	WriteBool(out, MovieKeys::UseNes101Hvc101Behavior, settings->CheckFlag(EmulationFlags::UseNes101Hvc101Behavior));
	WriteBool(out, MovieKeys::EnableOamDecay, settings->CheckFlag(EmulationFlags::EnableOamDecay));
	WriteBool(out, MovieKeys::DisablePpuReset, settings->CheckFlag(EmulationFlags::DisablePpuReset));

	WriteInt(out, MovieKeys::ZapperDetectionRadius, settings->GetZapperDetectionRadius());

	switch(settings->GetRamPowerOnState()) {
		case RamPowerOnState::AllZeros: WriteInt(out, MovieKeys::RamPowerOnState, 0x00); break;
		case RamPowerOnState::AllOnes: WriteInt(out, MovieKeys::RamPowerOnState, 0xFF); break;
		case RamPowerOnState::Random: WriteInt(out, MovieKeys::RamPowerOnState, -1); break; //TODO: Shouldn't be used for movies
	}	

	if(_console->GetDipSwitchCount() > 0) {
		WriteString(out, MovieKeys::DipSwitches, HexUtilities::ToHex(settings->GetDipSwitches()));
	}

	for(CodeInfo &code : _console->GetCheatManager()->GetCheats()) {
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
		_console->GetControlManager()->UnregisterInputRecorder(this);

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

		VirtualFile patchFile = _console->GetPatchFile();
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

void MovieRecorder::ProcessNotification(ConsoleNotificationType type, void *parameter)
{
	if(type == ConsoleNotificationType::GameLoaded) {
		_console->GetControlManager()->RegisterInputRecorder(this);
	}
}

bool MovieRecorder::CreateMovie(string movieFile, std::deque<RewindData> &data, uint32_t startPosition, uint32_t endPosition)
{
	_filename = movieFile;
	_writer.reset(new ZipWriter());
	if(startPosition < data.size() && endPosition <= data.size() && _writer->Initialize(_filename)) {
		vector<shared_ptr<BaseControlDevice>> devices = _console->GetControlManager()->GetControlDevices();
		
		if(startPosition > 0 || _console->GetRomInfo().HasBattery || _console->GetSettings()->GetRamPowerOnState() == RamPowerOnState::Random) {
			//Create a movie from a savestate if we don't start from the beginning (or if the game has save ram, or if the power on ram state is random)
			_hasSaveState = true;
			_saveStateData = stringstream();
			_console->GetSaveStateManager()->GetSaveStateHeader(_saveStateData);
			data[startPosition].GetStateData(_saveStateData);
		}

		_inputData = stringstream();

		for(uint32_t i = startPosition; i < endPosition; i++) {
			RewindData rewindData = data[i];
			for(uint32_t i = 0; i < 30; i++) {
				for(shared_ptr<BaseControlDevice> &device : devices) {
					uint8_t port = device->GetPort();
					if(i < rewindData.InputLogs[port].size()) {
						device->SetRawState(rewindData.InputLogs[port][i]);
						_inputData << ("|" + device->GetTextState());
					}
				}
				_inputData << "\n";
			}
		}

		//Write the movie file
		return Stop();
	}
	return false;
}