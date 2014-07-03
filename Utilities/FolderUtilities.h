#pragma once

#include "stdafx.h"

class FolderUtilities
{
	public:
		static wstring GetHomeFolder();
		static wstring GetSaveFolder();
		static wstring GetSaveStateFolder();
		static wstring GetMovieFolder();

		static vector<wstring> GetFolders(wstring rootFolder);
		static vector<wstring> GetFilesInFolder(wstring rootFolder, wstring mask, bool recursive);

		static wstring GetFilename(wstring filepath, bool includeExtension);
		static wstring GetFolderName(wstring filepath);
		
		static wstring OpenFile(LPCWSTR filter, wstring defaultFolder, bool forSave, wstring defaultExt = L"");
		static wstring OpenFile(LPCWSTR filter, LPCWSTR defaultFolder, bool forSave, LPCWSTR defaultExt = L"");
};