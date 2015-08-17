#include "stdafx.h"
#include <shlobj.h>
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
		throw std::exception("Home folder not specified");
	}
	return _homeFolder;
}

void FolderUtilities::AddKnowGameFolder(string gameFolder)
{
	bool alreadyExists = false;
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

vector<string> FolderUtilities::GetKnowGameFolders()
{
	return _gameFolders;
}

string FolderUtilities::GetSaveFolder()
{
	string folder = CombinePath(GetHomeFolder(), "Saves\\");
	CreateFolder(folder);
	return folder;
}

string FolderUtilities::GetHdPackFolder()
{
	string folder = CombinePath(GetHomeFolder(), "HdPacks\\");
	CreateFolder(folder);
	return folder;
}

string FolderUtilities::GetDebuggerFolder()
{
	string folder = CombinePath(GetHomeFolder(), "Debugger\\");
	CreateFolder(folder);
	return folder;
}

string FolderUtilities::GetSaveStateFolder()
{
	string folder = CombinePath(GetHomeFolder(), "SaveStates\\");
	CreateFolder(folder);
	return folder;
}

string FolderUtilities::GetMovieFolder()
{
	string folder = CombinePath(GetHomeFolder(), + "Movies\\");
	CreateFolder(folder);
	return folder;
}

string FolderUtilities::GetScreenshotFolder()
{
	string folder = CombinePath(GetHomeFolder(), "Screenshots\\");
	CreateFolder(folder);
	return folder;
}

void FolderUtilities::CreateFolder(string folder)
{
	CreateDirectory(utf8::utf8::decode(folder).c_str(), nullptr);
}

vector<string> FolderUtilities::GetFolders(string rootFolder)
{
	vector<string> folders;
#ifdef _WIN32
	HANDLE hFind;
	WIN32_FIND_DATA data;

	hFind = FindFirstFile(utf8::utf8::decode(rootFolder + "*").c_str(), &data);
	if(hFind != INVALID_HANDLE_VALUE) {
		do {
			string filename = utf8::utf8::encode(data.cFileName);
			if(data.dwFileAttributes == FILE_ATTRIBUTE_DIRECTORY && filename.compare(".") != 0 && filename.compare("..") != 0) {
				string subfolder = rootFolder + filename + "\\";
				folders.push_back(subfolder);
				for(string folderName : GetFolders(subfolder.c_str())) {
					folders.push_back(folderName);
				}
			}
		}
		while(FindNextFile(hFind, &data));
		FindClose(hFind);
	}
#endif
	return folders;
}

vector<string> FolderUtilities::GetFilesInFolder(string rootFolder, string mask, bool recursive)
{
	HANDLE hFind;
	WIN32_FIND_DATA data;

	vector<string> folders;
	vector<string> files;
	if(rootFolder[rootFolder.size() - 1] != '/' && rootFolder[rootFolder.size() - 1] != '\\') {
		rootFolder += "/";
	}

	folders.push_back(rootFolder);

	if(recursive) {
		for(string subFolder : GetFolders(rootFolder)) {
			folders.push_back(subFolder);
		}
	}

	for(string folder : folders) {
		hFind = FindFirstFile(utf8::utf8::decode(folder + mask).c_str(), &data);
		if(hFind != INVALID_HANDLE_VALUE) {
			do {
				files.push_back(folder + utf8::utf8::encode(data.cFileName));
			} while(FindNextFile(hFind, &data));
			FindClose(hFind);
		}
	}

	return files;
}

string FolderUtilities::GetFilename(string filepath, bool includeExtension)
{
	size_t index = filepath.find_last_of("/\\");
	string filename = (index == std::string::basic_string::npos) ? filepath : filepath.substr(index + 1);
	if(!includeExtension) {
		filename = filename.substr(0, filename.find_last_of("."));
	}
	return filename;
}

string FolderUtilities::GetFolderName(string filepath)
{
	size_t index = filepath.find_last_of("/\\");
	return filepath.substr(0, index);
}

string FolderUtilities::CombinePath(string folder, string filename)
{
#ifdef WIN32
	string separator = "\\";
#else 
	string separator = "/";
#endif

	if(folder.find_last_of(separator) != folder.length() - 1) {
		folder += separator;
	}

	return folder + filename;
}

int64_t FolderUtilities::GetFileModificationTime(string filepath)
{
	WIN32_FILE_ATTRIBUTE_DATA fileAttrData = {0};
	GetFileAttributesEx(utf8::utf8::decode(filepath).c_str(), GetFileExInfoStandard, &fileAttrData);
	return ((int64_t)fileAttrData.ftLastWriteTime.dwHighDateTime << 32) | (int64_t)fileAttrData.ftLastWriteTime.dwLowDateTime;
}