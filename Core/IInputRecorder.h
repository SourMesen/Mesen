#pragma once

class BaseControlDevice;

class IInputRecorder
{
public:
	virtual void RecordInput(BaseControlDevice *device) = 0;
	virtual void EndFrame() { }
};