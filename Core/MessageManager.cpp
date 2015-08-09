#include "stdafx.h"

#include "MessageManager.h"

IMessageManager* MessageManager::_messageManager = nullptr;
vector<INotificationListener*> MessageManager::_notificationListeners;

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
}

void MessageManager::UnregisterNotificationListener(INotificationListener* notificationListener)
{
	MessageManager::_notificationListeners.erase(std::remove(MessageManager::_notificationListeners.begin(), MessageManager::_notificationListeners.end(), notificationListener), MessageManager::_notificationListeners.end());
}

void MessageManager::SendNotification(ConsoleNotificationType type)
{
	//Iterate on a copy to prevent issues if a notification causes a listener to unregister itself
	vector<INotificationListener*> listeners = MessageManager::_notificationListeners;
	vector<INotificationListener*> processedListeners;

	for(size_t i = 0, len = listeners.size(); i < len; i++) {
		INotificationListener* notificationListener = listeners[i];
		if(std::find(processedListeners.begin(), processedListeners.end(), notificationListener) == processedListeners.end()) {
			//Only send notification if it hasn't been processed already
			notificationListener->ProcessNotification(type);
		}
		processedListeners.push_back(notificationListener);

		if(len != MessageManager::_notificationListeners.size()) {
			//Vector changed, start from the beginning again (can occur when sending a notification caused listeners to unregister themselves)
			i = 0;
			len = MessageManager::_notificationListeners.size();
			listeners = MessageManager::_notificationListeners;
		}
	}
}