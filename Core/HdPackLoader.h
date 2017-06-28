#pragma once
#include "stdafx.h"
#include "HdData.h"

class HdPackLoader
{
public:
	static bool LoadHdNesPack(string hdPackDefinitionFile, HdPackData &data);

private:
	HdPackData* _data;
	string _hdPackDefinitionFile;
	string _hdPackFolder;
	vector<HdPackBitmapInfo> _hdNesBitmaps;

	HdPackLoader(string hdPackDefinitionFile, HdPackData *data);

	bool LoadPack();
	void InitializeHdPack();
	void LoadCustomPalette();

	void ProcessPatchTag(vector<string> &tokens);
	void ProcessConditionTag(vector<string> &tokens);
	void ProcessTileTag(vector<string> &tokens, vector<HdPackCondition*> conditions);
	void ProcessBackgroundTag(vector<string> &tokens, vector<HdPackCondition*> conditions);
	void ProcessOptionTag(vector<string>& tokens);

	vector<HdPackCondition*> ParseConditionString(string conditionString, vector<unique_ptr<HdPackCondition>> &conditions);
};