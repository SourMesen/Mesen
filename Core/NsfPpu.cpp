#include "stdafx.h"
#include "NsfPpu.h"
#include "Console.h"
#include "NotificationManager.h"

void NsfPpu::DrawPixel()
{
}

void NsfPpu::SendFrame()
{
	_console->GetNotificationManager()->SendNotification(ConsoleNotificationType::PpuFrameDone);
}
