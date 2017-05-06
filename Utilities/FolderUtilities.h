#pragma once

#include "stdafx.h"

class FolderUtilities
{
private:
	static string _homeFolder;
	static vector<string> _gameFolders;

public:
	static void SetHomeFolder(string homeFolder);
	static string GetHomeFolder();

	static void AddKnownGameFolder(string gameFolder);
	static vector<string> GetKnownGameFolders();

	static string GetSaveFolder();
	static string GetSaveStateFolder();
	static string GetMovieFolder();
	static string GetScreenshotFolder();
	static string GetHdPackFolder();
	static string GetDebuggerFolder();
	static string GetRecentGamesFolder();

	static vector<string> GetFolders(string rootFolder);
	static vector<string> GetFilesInFolder(string rootFolder, string mask, bool recursive);

	static string GetFilename(string filepath, bool includeExtension);
	static string GetFolderName(string filepath);

	static void CreateFolder(string folder);

	static int64_t GetFileModificationTime(string filepath);

	static string CombinePath(string folder, string filename);
};