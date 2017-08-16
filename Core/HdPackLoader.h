#pragma once
#include "stdafx.h"
#include "HdData.h"
#include "../Utilities/ZipReader.h"
#include "../Utilities/VirtualFile.h"

class HdPackLoader
{
public:
	static bool LoadHdNesPack(string definitionFile, HdPackData &outData);
	static bool LoadHdNesPack(VirtualFile &romFile, HdPackData &outData);

private:
	HdPackData* _data;
	bool _loadFromZip = false;
	ZipReader _reader;
	string _hdPackDefinitionFile;
	string _hdPackFolder;
	vector<HdPackBitmapInfo> _hdNesBitmaps;

	HdPackLoader();

	bool InitializeLoader(VirtualFile &romPath, HdPackData *data);
	bool LoadFile(string filename, vector<uint8_t> &fileData);

	bool LoadPack();
	void InitializeHdPack();
	void LoadCustomPalette();

	void InitializeGlobalConditions();

	bool ProcessImgTag(string src);
	void ProcessPatchTag(vector<string> &tokens);
	void ProcessOverscanTag(vector<string> &tokens);
	void ProcessConditionTag(vector<string> &tokens);
	void ProcessTileTag(vector<string> &tokens, vector<HdPackCondition*> conditions);
	void ProcessBackgroundTag(vector<string> &tokens, vector<HdPackCondition*> conditions);
	void ProcessOptionTag(vector<string>& tokens);

	vector<HdPackCondition*> ParseConditionString(string conditionString, vector<unique_ptr<HdPackCondition>> &conditions);
};