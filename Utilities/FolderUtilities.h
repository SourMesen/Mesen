#pragma once

#include "stdafx.h"

class FolderUtilities
{
private:
	static wstring _homeFolder;
	static vector<wstring> _gameFolders;

public:
	static void SetHomeFolder(wstring homeFolder);
	static wstring GetHomeFolder();

	static void AddKnowGameFolder(wstring gameFolder);
	static vector<wstring> GetKnowGameFolders();

	static wstring GetSaveFolder();
	static wstring GetSaveStateFolder();
	static wstring GetMovieFolder();
	static wstring GetScreenshotFolder();

	static vector<wstring> GetFolders(wstring rootFolder);
	static vector<wstring> GetFilesInFolder(wstring rootFolder, wstring mask, bool recursive);

	static wstring GetFilename(wstring filepath, bool includeExtension);
	static wstring GetFolderName(wstring filepath);

	static int64_t GetFileModificationTime(wstring filepath);

	static wstring CombinePath(wstring folder, wstring filename);
};