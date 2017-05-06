#include "stdafx.h"

//TODO: Use non-experimental namespace (once it is officially supported by VC & GCC)
#include <experimental/filesystem>
namespace fs = std::experimental::filesystem;

#include <algorithm>
#include "FolderUtilities.h"
#include "UTF8Util.h"

string FolderUtilities::_homeFolder = "";
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
		if(folder.compare(gameFolder) == 0) {
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

string FolderUtilities::GetSaveFolder()
{
	string folder = CombinePath(GetHomeFolder(), "Saves");
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
	string folder = CombinePath(GetHomeFolder(), "SaveStates");
	CreateFolder(folder);
	return folder;
}

string FolderUtilities::GetMovieFolder()
{
	string folder = CombinePath(GetHomeFolder(), + "Movies");
	CreateFolder(folder);
	return folder;
}

string FolderUtilities::GetScreenshotFolder()
{
	string folder = CombinePath(GetHomeFolder(), "Screenshots");
	CreateFolder(folder);
	return folder;
}

string FolderUtilities::GetRecentGamesFolder()
{
	string folder = CombinePath(GetHomeFolder(), "RecentGames");
	CreateFolder(folder);
	return folder;
}

void FolderUtilities::CreateFolder(string folder)
{
	fs::create_directory(fs::u8path(folder));
}

vector<string> FolderUtilities::GetFolders(string rootFolder)
{
	vector<string> folders;

	if(!fs::is_directory(fs::u8path(rootFolder))) {
		return folders;
	} 

	for(fs::recursive_directory_iterator i(fs::u8path(rootFolder)), end; i != end; i++) {
		if(fs::is_directory(i->path())) {
			folders.push_back(i->path().u8string());
		}
	}

	return folders;
}

vector<string> FolderUtilities::GetFilesInFolder(string rootFolder, string mask, bool recursive)
{
	vector<string> files;
	vector<string> folders = { { rootFolder } };

	if(!fs::is_directory(fs::u8path(rootFolder))) {
		return files;
	}

	if(recursive) {
		for(string subFolder : GetFolders(rootFolder)) {
			folders.push_back(subFolder);
		}
	}

	for(string folder : folders) {
		for(fs::directory_iterator i(fs::u8path(folder.c_str())), end; i != end; i++) {
			string extension = i->path().extension().u8string();
			std::transform(extension.begin(), extension.end(), extension.begin(), ::tolower);
			if(extension == mask) {
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
	return (fs::u8path(folder) / fs::u8path(filename)).u8string();
}

int64_t FolderUtilities::GetFileModificationTime(string filepath)
{
	return fs::last_write_time(fs::u8path(filepath)).time_since_epoch() / std::chrono::seconds(1);
}