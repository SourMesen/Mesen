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

void FolderUtilities::CreateFolder(string folder)
{
	fs::create_directory(folder);
}

vector<string> FolderUtilities::GetFolders(string rootFolder)
{
	vector<string> folders;

	if(!fs::is_directory(rootFolder)) {
		return folders;
	} 

	for(fs::recursive_directory_iterator i(rootFolder), end; i != end; i++) {
		if(fs::is_directory(i->path())) {
			folders.push_back(i->path().string());
		}
	}

	return folders;
}

vector<string> FolderUtilities::GetFilesInFolder(string rootFolder, string mask, bool recursive)
{
	vector<string> files;
	vector<string> folders = { { rootFolder } };

	if(!fs::is_directory(rootFolder)) {
		return files;
	}

	if(recursive) {
		for(string subFolder : GetFolders(rootFolder)) {
			folders.push_back(subFolder);
		}
	}

	for(string folder : folders) {
		for(fs::directory_iterator i(fs::path(folder.c_str())), end; i != end; i++) {
			string extension = i->path().extension().string();
			std::transform(extension.begin(), extension.end(), extension.begin(), ::tolower);
			if(extension == mask) {
				files.push_back(i->path().string());
			}
		}
	}

	return files;
}

string FolderUtilities::GetFilename(string filepath, bool includeExtension)
{
	fs::path filename = fs::path(filepath).filename();
	if(!includeExtension) {
		filename.replace_extension("");
	}
	return filename.string();
}

string FolderUtilities::GetFolderName(string filepath)
{
	return fs::path(filepath).remove_filename().string();
}

string FolderUtilities::CombinePath(string folder, string filename)
{
	return fs::path(folder).append(filename).string();
}

int64_t FolderUtilities::GetFileModificationTime(string filepath)
{
	return fs::last_write_time(fs::path(filepath)).time_since_epoch() / std::chrono::seconds(1);
}