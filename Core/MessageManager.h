#pragma once

#include "stdafx.h"

#include "IMessageManager.h"
#include "INotificationListener.h"
#include <unordered_map>
#include "../Utilities/SimpleLock.h"

class MessageManager
{
private:
	static IMessageManager* _messageManager;
	static vector<INotificationListener*> _notificationListeners;
	static std::unordered_map<string, string> _enResources;
	static std::unordered_map<string, string> _frResources;
	static std::unordered_map<string, string> _jaResources;
	static std::unordered_map<string, string> _ruResources;
	static std::unordered_map<string, string> _esResources;

	static SimpleLock _logLock;
	static std::list<string> _log;

public:
	static string Localize(string key);

	static void RegisterMessageManager(IMessageManager* messageManager);
	static void DisplayMessage(string title, string message, string param1 = "", string param2 = "");
	static void DisplayToast(string title, string message, uint8_t* iconData, uint32_t iconSize);

	static void Log(string message = "");
	static string GetLog();

	static void RegisterNotificationListener(INotificationListener* notificationListener);
	static void UnregisterNotificationListener(INotificationListener* notificationListener);
	static void SendNotification(ConsoleNotificationType type, void* parameter = nullptr);
};