#include "stdafx.h"
#include <commdlg.h>
#include <shlobj.h>
#include "FolderUtilities.h"

wstring FolderUtilities::GetHomeFolder()
{
	wstring folder;
	PWSTR pathName;
	SHGetKnownFolderPath(FOLDERID_Documents, 0, nullptr, &pathName);
	folder = wstring((wchar_t*)pathName) + L"\\NesEMU\\";

	//Make sure it exists
	CreateDirectory(folder.c_str(), nullptr);

	CoTaskMemFree(pathName);

	return folder;
}

wstring FolderUtilities::GetSaveFolder()
{
	wstring folder = GetHomeFolder() + L"Saves\\";
	CreateDirectory(folder.c_str(), nullptr);
	return folder;
}

wstring FolderUtilities::GetSaveStateFolder()
{
	wstring folder = GetHomeFolder() + L"SaveStates\\";
	CreateDirectory(folder.c_str(), nullptr);
	return folder;
}

wstring FolderUtilities::GetMovieFolder()
{
	wstring folder = GetHomeFolder() + L"Movies\\";
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
		
wstring FolderUtilities::OpenFile(LPCWSTR filter, wstring defaultFolder, bool forSave, wstring defaultExt)
{
	return OpenFile(filter, defaultFolder.c_str(), forSave, defaultExt.c_str());
}

wstring FolderUtilities::OpenFile(LPCWSTR filter, LPCWSTR defaultFolder, bool forSave, LPCWSTR defaultExt)
{
	wchar_t buffer[2000];
	buffer[0] = '\0';

	OPENFILENAME ofn;
	ZeroMemory(&ofn, sizeof(ofn));
	ofn.lStructSize = sizeof(ofn);
	ofn.hwndOwner = nullptr;
	ofn.lpstrDefExt = defaultExt;
	ofn.lpstrFile = buffer;
	ofn.nMaxFile = sizeof(buffer);
	ofn.lpstrFilter = filter;
	ofn.nFilterIndex = 1;
	ofn.lpstrFileTitle = nullptr;
	ofn.nMaxFileTitle = 0;
	ofn.lpstrInitialDir = defaultFolder;
	if(forSave) {
		ofn.Flags = OFN_OVERWRITEPROMPT;
		GetSaveFileName(&ofn);
	} else {
		ofn.Flags = OFN_PATHMUSTEXIST | OFN_FILEMUSTEXIST;
		GetOpenFileName(&ofn);
	}

	return wstring(buffer);
}