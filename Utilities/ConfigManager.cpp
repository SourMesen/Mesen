#include "stdafx.h"
#include "ConfigManager.h"
#include "FolderUtilities.h"

string ConfigManager::ConfigTagNames[] = { 
	"ShowFPS",
	"LimitFPS",
	"MRU0",
	"MRU1",
	"MRU2",
	"MRU3",
	"MRU4",
	"LastGameFolder",
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

void ConfigManager::LoadConfigFile()
{
	_configFilePath = FolderUtilities::GetHomeFolder() + L"Settings.xml";

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

void ConfigManager::AddToMRU(wstring romFilepath)
{
	wstring MRU0 = ConfigManager::GetValue<wstring>(Config::MRU0);
	wstring MRU1 = ConfigManager::GetValue<wstring>(Config::MRU1);
	wstring MRU2 = ConfigManager::GetValue<wstring>(Config::MRU2);
	wstring MRU3 = ConfigManager::GetValue<wstring>(Config::MRU3);
	wstring MRU4 = ConfigManager::GetValue<wstring>(Config::MRU4);

	if(MRU0.compare(romFilepath) == 0) {
		return;
	} else if(MRU1.compare(romFilepath) == 0) {
		MRU1 = MRU0;
		MRU0 = romFilepath;
	} else if(MRU2.compare(romFilepath) == 0) {
		MRU2 = MRU1;
		MRU1 = MRU0;
		MRU0 = romFilepath;
	} else if(MRU3.compare(romFilepath) == 0) {
		MRU3 = MRU2;
		MRU2 = MRU1;
		MRU1 = MRU0;
		MRU0 = romFilepath;
	} else {
		MRU4 = MRU3;
		MRU3 = MRU2;
		MRU2 = MRU1;
		MRU1 = MRU0;
		MRU0 = romFilepath;
	}

	ConfigManager::SetValue(Config::MRU0, MRU0);
	ConfigManager::SetValue(Config::MRU1, MRU1);
	ConfigManager::SetValue(Config::MRU2, MRU2);
	ConfigManager::SetValue(Config::MRU3, MRU3);
	ConfigManager::SetValue(Config::MRU4, MRU4);
}