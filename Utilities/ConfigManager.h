#pragma once

#include "stdafx.h"
#include "../Utilities/tinyxml2.h"
#include "utf8conv.h"

enum class Config
{
	ShowFPS = 0,
	LimitFPS,
	MRU0,
	MRU1,
	MRU2,
	MRU3,
	MRU4,
	LastGameFolder,
	Player1_ButtonA,
	Player1_ButtonB,
	Player1_Select,
	Player1_Start,
	Player1_Up,
	Player1_Down,
	Player1_Left,
	Player1_Right,
};

class ConfigManager
{
	private:
		static shared_ptr<ConfigManager> Instance;
		static string ConfigTagNames[256];

		bool _loaded;
		wstring _configFilePath;
		tinyxml2::XMLDocument _xmlDocument;
		ConfigManager() { }

		void LoadConfigFile();

		tinyxml2::XMLElement* GetNode(Config config);
		tinyxml2::XMLElement* GetConfigNode();
		
		template<typename T>
		T GetValueAttribute(tinyxml2::XMLElement* element);

		template<>
		int GetValueAttribute<int>(tinyxml2::XMLElement* element)
		{
			return element->IntAttribute("value");
		}

		template<>
		bool GetValueAttribute<bool>(tinyxml2::XMLElement* element)
		{
			return element->BoolAttribute("value");
		}

		template<>
		double GetValueAttribute<double>(tinyxml2::XMLElement* element)
		{
			return element->DoubleAttribute("value");
		}

		template<>
		wstring GetValueAttribute<wstring>(tinyxml2::XMLElement* element)
		{
			if(element->Attribute("value")) {
				return utf8util::UTF16FromUTF8(string(element->Attribute("value")));
			} else {
				return L"";
			}
		}

		template<typename T>
		void SetValueAttribute(tinyxml2::XMLElement* element, T value)
		{
			element->SetAttribute("value", value);
		}
		
		template<>
		void SetValueAttribute<std::wstring>(tinyxml2::XMLElement* element, std::wstring value)
		{
			element->SetAttribute("value", utf8util::UTF8FromUTF16(value).c_str());
		}

	public:
		static shared_ptr<ConfigManager> GetInstance()
		{
			if(ConfigManager::Instance == nullptr) {
				ConfigManager::Instance.reset(new ConfigManager());
			}
			return ConfigManager::Instance;
		}

		template<typename T>
		static T GetValue(Config config)
		{
			return GetInstance()->GetValueAttribute<T>(GetInstance()->GetNode(config));
		}
		
		template<typename T>
		static void SetValue(Config config, T value)
		{
			GetInstance()->SetValueAttribute(GetInstance()->GetNode(config), value);
			GetInstance()->_xmlDocument.SaveFile(utf8util::UTF8FromUTF16(GetInstance()->_configFilePath).c_str(), false);
		}

		static void AddToMRU(wstring romFilepath);
};
