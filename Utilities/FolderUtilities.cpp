#include "stdafx.h"
#include <commdlg.h>
#include <shlobj.h>
#include "FolderUtilities.h"

wstring FolderUtilities::_homeFolder = L"";

void FolderUtilities::SetHomeFolder(wstring homeFolder)
{
	_homeFolder = homeFolder;
	CreateDirectory(homeFolder.c_str(), nullptr);
}

wstring FolderUtilities::GetHomeFolder()
{
	if(_homeFolder.size() == 0) {
		throw std::exception("Home folder not specified");
	}
	return _homeFolder;
}

wstring FolderUtilities::GetSaveFolder()
{
	wstring folder = CombinePath(GetHomeFolder(), L"Saves\\");
	CreateDirectory(folder.c_str(), nullptr);
	return folder;
}

wstring FolderUtilities::GetSaveStateFolder()
{
	wstring folder = CombinePath(GetHomeFolder(), L"SaveStates\\");
	CreateDirectory(folder.c_str(), nullptr);
	return folder;
}

wstring FolderUtilities::GetMovieFolder()
{
	wstring folder = CombinePath(GetHomeFolder(), + L"Movies\\");
	CreateDirectory(folder.c_str(), nullptr);
	return folder;
}

wstring FolderUtilities::GetScreenshotFolder()
{
	wstring folder = CombinePath(GetHomeFolder(), L"Screenshots\\");
	CreateDirectory(folder.c_str(), nullptr);
	return folder;
}

vector<wstring> FolderUtilities::GetFolders(wstring rootFolder)
{
	HANDLE hFind;
	WIN32_FIND_DATA data;

	vector<wstring> folders;

	hFind = FindFirstFile((rootFolder + L"*").c_str(), &data);
	if(hFind != INVALID_HANDLE_VALUE) {
		do {
			if(data.dwFileAttributes == FILE_ATTRIBUTE_DIRECTORY && wcscmp(data.cFileName, L".") != 0 && wcscmp(data.cFileName, L"..") != 0) {
				wstring subfolder = rootFolder + data.cFileName + L"\\";
				folders.push_back(subfolder);
				for(wstring folderName : GetFolders(subfolder.c_str())) {
					folders.push_back(folderName);
				}
			}
		}
		while(FindNextFile(hFind, &data));
		FindClose(hFind);
	}

	return folders;
}

vector<wstring> FolderUtilities::GetFilesInFolder(wstring rootFolder, wstring mask, bool recursive)
{
	HANDLE hFind;
	WIN32_FIND_DATA data;

	vector<wstring> folders;
	vector<wstring> files;
	if(rootFolder[rootFolder.size() - 1] != '/' && rootFolder[rootFolder.size() - 1] != '\\') {
		rootFolder += L"/";
	}

	folders.push_back(rootFolder);

	if(recursive) {
		for(wstring subFolder : GetFolders(rootFolder)) {
			folders.push_back(subFolder);
		}
	}

	for(wstring folder : folders) {
		hFind = FindFirstFile((folder + mask).c_str(), &data);
		if(hFind != INVALID_HANDLE_VALUE) {
			do {
				files.push_back(folder + data.cFileName);
			} while(FindNextFile(hFind, &data));
			FindClose(hFind);
		}
	}

	return files;
}

wstring FolderUtilities::GetFilename(wstring filepath, bool includeExtension)
{
	int index = filepath.find_last_of(L"/\\");
	wstring filename = (index == std::string::basic_string::npos) ? filepath : filepath.substr(index + 1);
	if(!includeExtension) {
		filename = filename.substr(0, filename.find_last_of(L"."));
	}
	return filename;
}

wstring FolderUtilities::GetFolderName(wstring filepath)
{
	int index = filepath.find_last_of(L"/\\");
	return filepath.substr(0, index);
}

wstring FolderUtilities::CombinePath(wstring folder, wstring filename)
{
#ifdef WIN32
	wstring separator = L"\\";
#else 
	wstring separator = L"/";
#endif

	if(folder.find_last_of(separator) != folder.length() - 1) {
		folder += separator;
	}

	return folder + filename;
}

int64_t FolderUtilities::GetFileModificationTime(wstring filepath)
{
	WIN32_FILE_ATTRIBUTE_DATA fileAttrData = {0};
	GetFileAttributesEx(filepath.c_str(), GetFileExInfoStandard, &fileAttrData);
	return ((int64_t)fileAttrData.ftLastWriteTime.dwHighDateTime << 32) | (int64_t)fileAttrData.ftLastWriteTime.dwLowDateTime;
}