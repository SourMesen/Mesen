#include "stdafx.h"
#include "ConfigManager.h"
#include <Shlobj.h>

string ConfigManager::ConfigTagNames[] = { 
	"ShowFPS",
	"LimitFPS",
	"MRU0",
	"MRU1",
	"MRU2",
	"MRU3",
	"MRU4",
	"Player1_ButtonA",
	"Player1_ButtonB",
	"Player1_Select",
	"Player1_Start",
	"Player1_Up",
	"Player1_Down",
	"Player1_Left",
	"Player1_Right",
};

shared_ptr<ConfigManager> ConfigManager::Instance = nullptr;

wstring ConfigManager::GetHomeFolder()
{
	wstring folder;
	PWSTR pathName;
	SHGetKnownFolderPath(FOLDERID_LocalAppData, 0, nullptr, &pathName);
	folder = wstring((wchar_t*)pathName) + L"\\NesEMU\\";

	//Make sure it exists
	CreateDirectory(folder.c_str(), nullptr);

	CoTaskMemFree(pathName);

	return folder;
}

void ConfigManager::LoadConfigFile()
{
	_configFilePath = GetHomeFolder() + L"Settings.xml";

	ifstream configFile(_configFilePath, ifstream::in);

	if(configFile) {
		configFile.close();
		_xmlDocument.LoadFile(utf8util::UTF8FromUTF16(_configFilePath).c_str());
	} else {
		_xmlDocument.InsertFirstChild(_xmlDocument.NewDeclaration());

		tinyxml2::XMLElement* mainNode = _xmlDocument.NewElement("Config");
		_xmlDocument.InsertEndChild(mainNode);
	}
}

tinyxml2::XMLElement* ConfigManager::GetConfigNode()
{
	return _xmlDocument.LastChildElement();
}

tinyxml2::XMLElement* ConfigManager::GetNode(Config config)
{
	if(!_loaded) {
		LoadConfigFile();
		_loaded = true;
	}

	tinyxml2::XMLElement* currentNode = GetConfigNode()->FirstChildElement();
	tinyxml2::XMLElement* configNode = nullptr;
	if(currentNode) {
		do {
			if(ConfigTagNames[(int)config].compare(currentNode->Name()) == 0) {
				configNode = currentNode;
				break;
			}
		}
		while(currentNode = currentNode->NextSiblingElement());
	}

	if(!configNode) {
		configNode = _xmlDocument.NewElement(ConfigTagNames[(int)config].c_str());
		GetConfigNode()->InsertEndChild(configNode);
	}

	return configNode;
}