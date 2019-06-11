#include "stdafx.h"

//TODO: Use non-experimental namespace (once it is officially supported by VC & GCC)
#ifndef LIBRETRO
#include <experimental/filesystem>
namespace fs = std::experimental::filesystem;
#endif

#include <unordered_set>
#include <algorithm>
#include "FolderUtilities.h"
#include "UTF8Util.h"

string FolderUtilities::_homeFolder = "";
string FolderUtilities::_saveFolderOverride = "";
string FolderUtilities::_saveStateFolderOverride = "";
string FolderUtilities::_screenshotFolderOverride = "";
vector<string> FolderUtilities::_gameFolders = vector<string>();

void FolderUtilities::SetHomeFolder(string homeFolder)
{
	_homeFolder = homeFolder;
	CreateFolder(homeFolder);
}

string FolderUtilities::GetHomeFolder()
{
	if(_homeFolder.size() == 0) {
		throw std::runtime_error("Home folder not specified");
	}
	return _homeFolder;
}

void FolderUtilities::AddKnownGameFolder(string gameFolder)
{
	bool alreadyExists = false;
	string lowerCaseFolder = gameFolder;
	std::transform(lowerCaseFolder.begin(), lowerCaseFolder.end(), lowerCaseFolder.begin(), ::tolower);

	for(string folder : _gameFolders) {
		std::transform(folder.begin(), folder.end(), folder.begin(), ::tolower);
		if(folder.compare(lowerCaseFolder) == 0) {
			alreadyExists = true;
			break;
		}
	}

	if(!alreadyExists) {
		_gameFolders.push_back(gameFolder);
	}
}

vector<string> FolderUtilities::GetKnownGameFolders()
{
	return _gameFolders;
}

void FolderUtilities::SetFolderOverrides(string saveFolder, string saveStateFolder, string screenshotFolder)
{
	_saveFolderOverride = saveFolder;
	_saveStateFolderOverride = saveStateFolder;
	_screenshotFolderOverride = screenshotFolder;
}

string FolderUtilities::GetSaveFolder()
{
	string folder;
	if(_saveFolderOverride.empty()) {
		folder = CombinePath(GetHomeFolder(), "Saves");
	} else {
		folder = _saveFolderOverride;
	}
	CreateFolder(folder);
	return folder;
}

string FolderUtilities::GetHdPackFolder()
{
	string folder = CombinePath(GetHomeFolder(), "HdPacks");
	CreateFolder(folder);
	return folder;
}

string FolderUtilities::GetDebuggerFolder()
{
	string folder = CombinePath(GetHomeFolder(), "Debugger");
	CreateFolder(folder);
	return folder;
}

string FolderUtilities::GetSaveStateFolder()
{
	string folder;
	if(_saveStateFolderOverride.empty()) {
		folder = CombinePath(GetHomeFolder(), "SaveStates");
	} else {
		folder = _saveStateFolderOverride;
	}
	CreateFolder(folder);
	return folder;
}

string FolderUtilities::GetScreenshotFolder()
{
	string folder;
	if(_screenshotFolderOverride.empty()) {
		folder = CombinePath(GetHomeFolder(), "Screenshots");
	} else {
		folder = _screenshotFolderOverride;
	}
	CreateFolder(folder);
	return folder;
}

string FolderUtilities::GetRecentGamesFolder()
{
	string folder = CombinePath(GetHomeFolder(), "RecentGames");
	CreateFolder(folder);
	return folder;
}

#ifndef LIBRETRO
void FolderUtilities::CreateFolder(string folder)
{
	std::error_code errorCode;
	fs::create_directory(fs::u8path(folder), errorCode);
}

vector<string> FolderUtilities::GetFolders(string rootFolder)
{
	vector<string> folders;

	std::error_code errorCode;
	if(!fs::is_directory(fs::u8path(rootFolder), errorCode)) {
		return folders;
	} 

	for(fs::recursive_directory_iterator i(fs::u8path(rootFolder)), end; i != end; i++) {
		if(i.depth() > 1) {
			//Prevent excessive recursion
			i.disable_recursion_pending();
		} else {
			if(fs::is_directory(i->path(), errorCode)) {
				folders.push_back(i->path().u8string());
			}
		}
	}

	return folders;
}

vector<string> FolderUtilities::GetFilesInFolder(string rootFolder, std::unordered_set<string> extensions, bool recursive)
{
	vector<string> files;
	vector<string> folders = { { rootFolder } };

	std::error_code errorCode;
	if(!fs::is_directory(fs::u8path(rootFolder), errorCode)) {
		return files;
	}

	if(recursive) {
		for(fs::recursive_directory_iterator i(fs::u8path(rootFolder)), end; i != end; i++) {
			if(i.depth() > 1) {
				//Prevent excessive recursion
				i.disable_recursion_pending();
			} else {
				string extension = i->path().extension().u8string();
				std::transform(extension.begin(), extension.end(), extension.begin(), ::tolower);
				if(extensions.empty() || extensions.find(extension) != extensions.end()) {
					files.push_back(i->path().u8string());
				}
			}
		}
	} else {
		for(fs::directory_iterator i(fs::u8path(rootFolder)), end; i != end; i++) {
			string extension = i->path().extension().u8string();
			std::transform(extension.begin(), extension.end(), extension.begin(), ::tolower);
			if(extensions.empty() || extensions.find(extension) != extensions.end()) {
				files.push_back(i->path().u8string());
			}
		}
	}

	return files;
}

string FolderUtilities::GetFilename(string filepath, bool includeExtension)
{
	fs::path filename = fs::u8path(filepath).filename();
	if(!includeExtension) {
		filename.replace_extension("");
	}
	return filename.u8string();
}

string FolderUtilities::GetFolderName(string filepath)
{
	return fs::u8path(filepath).remove_filename().u8string();
}

string FolderUtilities::CombinePath(string folder, string filename)
{
	//Windows supports forward slashes for paths, too.  And fs::u8path is abnormally slow.
	if(folder[folder.length() - 1] != '/') {
		return folder + "/" + filename;
	} else {
		return folder + filename;
	}
}

int64_t FolderUtilities::GetFileModificationTime(string filepath)
{
	std::error_code errorCode;
	return fs::last_write_time(fs::u8path(filepath), errorCode).time_since_epoch() / std::chrono::seconds(1);
}
#else

//Libretro: Avoid using filesystem API.

#ifdef _WIN32
static const char* PATHSEPARATOR = "\\";
#else 
static const char* PATHSEPARATOR = "/";
#endif

void FolderUtilities::CreateFolder(string folder)
{
}

vector<string> FolderUtilities::GetFolders(string rootFolder)
{
	return vector<string>();
}

vector<string> FolderUtilities::GetFilesInFolder(string rootFolder, std::unordered_set<string> extensions, bool recursive)
{
	return vector<string>();
}

string FolderUtilities::GetFilename(string filepath, bool includeExtension)
{
	size_t index = filepath.find_last_of(PATHSEPARATOR);
	string filename = (index == std::string::basic_string::npos) ? filepath : filepath.substr(index + 1);
	if(!includeExtension) {
		filename = filename.substr(0, filename.find_last_of("."));
	}
	return filename;
}

string FolderUtilities::GetFolderName(string filepath)
{
	size_t index = filepath.find_last_of(PATHSEPARATOR);
	return filepath.substr(0, index);
}

string FolderUtilities::CombinePath(string folder, string filename)
{
	if(folder.find_last_of(PATHSEPARATOR) != folder.length() - 1) {
		folder += PATHSEPARATOR;
	}
	return folder + filename;
}

int64_t FolderUtilities::GetFileModificationTime(string filepath)
{
	return 0;
}
#endif