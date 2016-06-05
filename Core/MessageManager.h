#pragma once

#include "stdafx.h"

#include "IMessageManager.h"
#include "INotificationListener.h"
#include <unordered_map>

class MessageManager
{
private:
	static IMessageManager* _messageManager;
	static vector<INotificationListener*> _notificationListeners;
	static std::unordered_map<string, string> _enResources;
	static std::unordered_map<string, string> _frResources;
	static std::unordered_map<string, string> _jaResources;

public:
	static string Localize(string key);

	static void RegisterMessageManager(IMessageManager* messageManager);
	static void DisplayMessage(string title, string message, string param1 = "", string param2 = "");
	static void DisplayToast(string title, string message, uint8_t* iconData, uint32_t iconSize);

	static void RegisterNotificationListener(INotificationListener* notificationListener);
	static void UnregisterNotificationListener(INotificationListener* notificationListener);
	static void SendNotification(ConsoleNotificationType type, void* parameter = nullptr);
};