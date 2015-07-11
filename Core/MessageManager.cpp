#include "stdafx.h"

#include "MessageManager.h"

IMessageManager* MessageManager::_messageManager = nullptr;
list<INotificationListener*> MessageManager::_notificationListeners;

void MessageManager::RegisterMessageManager(IMessageManager* messageManager)
{
	MessageManager::_messageManager = messageManager;
}

void MessageManager::DisplayMessage(string title, string message)
{
	if(MessageManager::_messageManager) {
		MessageManager::_messageManager->DisplayMessage(title, message);
	}
}

void MessageManager::DisplayToast(string title, string message, uint8_t* iconData, uint32_t iconSize)
{
	if(MessageManager::_messageManager) {
		MessageManager::_messageManager->DisplayToast(shared_ptr<ToastInfo>(new ToastInfo(title, message, 4000, iconData, iconSize)));
	}
}
void MessageManager::RegisterNotificationListener(INotificationListener* notificationListener)
{
	MessageManager::_notificationListeners.push_back(notificationListener);
	MessageManager::_notificationListeners.unique();
}

void MessageManager::UnregisterNotificationListener(INotificationListener* notificationListener)
{
	MessageManager::_notificationListeners.remove(notificationListener);
}

void MessageManager::SendNotification(ConsoleNotificationType type)
{
	list<INotificationListener*> listeners = MessageManager::_notificationListeners;
	
	//Iterate on a copy to prevent issues if a notification causes a listener to unregister itself
	for(INotificationListener* notificationListener : listeners) {
		notificationListener->ProcessNotification(type);
	}
}