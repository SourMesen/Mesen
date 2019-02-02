#pragma once

#include "stdafx.h"

#include "IMessageManager.h"
#include <unordered_map>
#include "../Utilities/SimpleLock.h"

class MessageManager
{
private:
	static IMessageManager* _messageManager;
	static std::unordered_map<string, string> _enResources;
	static std::unordered_map<string, string> _frResources;
	static std::unordered_map<string, string> _jaResources;
	static std::unordered_map<string, string> _ruResources;
	static std::unordered_map<string, string> _esResources;
	static std::unordered_map<string, string> _ukResources;
	static std::unordered_map<string, string> _ptResources;
	static std::unordered_map<string, string> _caResources;
	static std::unordered_map<string, string> _zhResources;

	static bool _osdEnabled;
	static SimpleLock _logLock;
	static SimpleLock _messageLock;
	static std::list<string> _log;
	
public:
	static void SetOsdState(bool enabled);

	static string Localize(string key);

	static void RegisterMessageManager(IMessageManager* messageManager);
	static void UnregisterMessageManager(IMessageManager* messageManager);
	static void DisplayMessage(string title, string message, string param1 = "", string param2 = "");

	static void Log(string message = "");
	static string GetLog();
};
