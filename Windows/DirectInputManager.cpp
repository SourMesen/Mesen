#include "stdafx.h"

#define DIRECTINPUT_VERSION 0x0800
#include <thread>
#include <windows.h>
#include <dinput.h>
#include <dinputd.h>
#include "DirectInputManager.h"

LPDIRECTINPUT8 DirectInputManager::_directInput = nullptr;
vector<DirectInputData> DirectInputManager::_joysticks;

bool DirectInputManager::Initialize()
{
	HRESULT hr;

	// Register with the DirectInput subsystem and get a pointer to a IDirectInput interface we can use.
	// Create a DInput object
	if(FAILED(hr = DirectInput8Create(GetModuleHandle(nullptr), DIRECTINPUT_VERSION, IID_IDirectInput8, (VOID**)&_directInput, nullptr))) {
		return false;
	}

	IDirectInputJoyConfig8* pJoyConfig = nullptr;
	if(FAILED(hr = _directInput->QueryInterface(IID_IDirectInputJoyConfig8, (void**)&pJoyConfig))) {
		return false;
	}

	if(pJoyConfig) {
		pJoyConfig->Release();
	}

	return UpdateDeviceList();
}

bool DirectInputManager::UpdateDeviceList()
{
	HRESULT hr;

	for(DirectInputData &data : _joysticks) {
		data.joystick->Unacquire();
		data.joystick->Release();
	}
	_joysticks.clear();

	// Enumerate devices
	if(FAILED(hr = _directInput->EnumDevices(DI8DEVCLASS_GAMECTRL, EnumJoysticksCallback, nullptr, DIEDFL_ALLDEVICES))) {
		return false;
	}

	// Make sure we got a joystick
	if(_joysticks.empty()) {
		return false;
	}

	for(DirectInputData& data : _joysticks) {
		// Set the data format to "simple joystick" - a predefined data format 
		// A data format specifies which controls on a device we are interested in, and how they should be reported.
		// This tells DInput that we will be passing a DIJOYSTATE2 structure to IDirectInputDevice::GetDeviceState().
		if(FAILED(hr = data.joystick->SetDataFormat(&c_dfDIJoystick2))) {
			return false;
		}

		// Set the cooperative level to let DInput know how this device should interact with the system and with other DInput applications.
		if(FAILED(hr = data.joystick->SetCooperativeLevel(_hWnd, DISCL_NONEXCLUSIVE | DISCL_BACKGROUND))) {
			return false;
		}

		// Enumerate the joystick objects. The callback function enabled user interface elements for objects that are found, and sets the min/max values property for discovered axes.
		if(FAILED(hr = data.joystick->EnumObjects(EnumObjectsCallback, data.joystick, DIDFT_ALL))) {
			return false;
		}
	}

	//Sleeping apparently lets us read accurate "default" values, otherwise a PS4 controller returns all 0s, despite not doing so normally
	RefreshState();
	std::this_thread::sleep_for(std::chrono::duration<int, std::milli>(100));
	RefreshState();
	for(DirectInputData &joystick : _joysticks) {
		joystick.defaultState = joystick.state;
	}

	return true;
}

//-----------------------------------------------------------------------------
// Name: EnumJoysticksCallback()
// Desc: Called once for each enumerated joystick. If we find one, create a
//       device interface on it so we can play with it.
//-----------------------------------------------------------------------------
int DirectInputManager::EnumJoysticksCallback(const DIDEVICEINSTANCE* pdidInstance, void* pContext)
{
	HRESULT hr;

	// Obtain an interface to the enumerated joystick.
	LPDIRECTINPUTDEVICE8 pJoystick = nullptr;
	hr = _directInput->CreateDevice(pdidInstance->guidInstance, &pJoystick, nullptr);

	if(SUCCEEDED(hr)) {
		DIJOYSTATE2 state;
		memset(&state, 0, sizeof(state));
		DirectInputData data{ pJoystick, state, state };
		_joysticks.push_back(data);
	}
	return DIENUM_CONTINUE;
}

//-----------------------------------------------------------------------------
// Name: EnumObjectsCallback()
// Desc: Callback function for enumerating objects (axes, buttons, POVs) on a 
//       joystick. This function enables user interface elements for objects
//       that are found to exist, and scales axes min/max values.
//-----------------------------------------------------------------------------
int DirectInputManager::EnumObjectsCallback(const DIDEVICEOBJECTINSTANCE* pdidoi, void* pContext)
{
	LPDIRECTINPUTDEVICE8 joystick = (LPDIRECTINPUTDEVICE8)pContext;

	static int nSliderCount = 0;  // Number of returned slider controls
	static int nPOVCount = 0;     // Number of returned POV controls

	// For axes that are returned, set the DIPROP_RANGE property for the enumerated axis in order to scale min/max values.
	if(pdidoi->dwType & DIDFT_AXIS) {
		DIPROPRANGE diprg;
		diprg.diph.dwSize = sizeof(DIPROPRANGE);
		diprg.diph.dwHeaderSize = sizeof(DIPROPHEADER);
		diprg.diph.dwHow = DIPH_BYID;
		diprg.diph.dwObj = pdidoi->dwType; // Specify the enumerated axis
		diprg.lMin = -1000;
		diprg.lMax = +1000;

		// Set the range for the axis
		if(FAILED(joystick->SetProperty(DIPROP_RANGE, &diprg.diph))) {
			return DIENUM_STOP;
		}
	}

	return DIENUM_CONTINUE;
}


void DirectInputManager::RefreshState()
{
	for(DirectInputData &joystick : _joysticks) {
		UpdateInputState(joystick);
	}
}

int DirectInputManager::GetJoystickCount()
{
	return (int)_joysticks.size();
}

bool DirectInputManager::IsPressed(int port, int button)
{
	if(port >= _joysticks.size()) {
		return false;
	}

	DIJOYSTATE2& state = _joysticks[port].state;
	DIJOYSTATE2& defaultState = _joysticks[port].defaultState;
	int deadRange = 500;

	bool povCentered = (LOWORD(state.rgdwPOV[0]) == 0xFFFF);

	switch(button) {
		case 0x00: return state.lY - defaultState.lY < -deadRange;
		case 0x01: return state.lY - defaultState.lY > deadRange;
		case 0x02: return state.lX - defaultState.lX < -deadRange;
		case 0x03: return state.lX - defaultState.lX > deadRange;
		case 0x04: return state.lRy - defaultState.lRy < -deadRange;
		case 0x05: return state.lRy - defaultState.lRy > deadRange;
		case 0x06: return state.lRx - defaultState.lRx < -deadRange;
		case 0x07: return state.lRx - defaultState.lRx > deadRange;
		case 0x08: return state.lZ - defaultState.lZ < -deadRange;
		case 0x09: return state.lZ - defaultState.lZ > deadRange;
		case 0x0A: return state.lRz - defaultState.lRz < -deadRange;
		case 0x0B: return state.lRz - defaultState.lRz > deadRange;
		case 0x0C: return !povCentered && (state.rgdwPOV[0] >= 31500 || state.rgdwPOV[0] <= 4500);
		case 0x0D: return !povCentered && state.rgdwPOV[0] >= 13500 && state.rgdwPOV[0] <= 22500;
		case 0x0E: return !povCentered && state.rgdwPOV[0] >= 4500 && state.rgdwPOV[0] <= 13500;
		case 0x0F: return !povCentered && state.rgdwPOV[0] >= 22500 && state.rgdwPOV[0] <= 31500;
		default: return state.rgbButtons[button - 0x10] != 0;
	}

	return false;
}

bool DirectInputManager::UpdateInputState(DirectInputData &data)
{
	HRESULT hr;

	// Poll the device to read the current state
	hr = data.joystick->Poll();
	if(FAILED(hr)) {
		// DInput is telling us that the input stream has been interrupted. We aren't tracking any state between polls, so
		// we don't have any special reset that needs to be done. We just re-acquire and try again.
		hr = data.joystick->Acquire();
		while(hr == DIERR_INPUTLOST) {
			hr = data.joystick->Acquire();
		}

		// hr may be DIERR_OTHERAPPHASPRIO or other errors.  This may occur when the app is minimized or in the process of 
		// switching, so just try again later 
		if(FAILED(hr)) {
			return true;
		}
	}

	// Get the input's device state
	if(FAILED(hr = data.joystick->GetDeviceState(sizeof(DIJOYSTATE2), &data.state))) {
		return false; // The device should have been acquired during the Poll()
	}

	return true;
}


DirectInputManager::DirectInputManager(HWND hWnd)
{
	_hWnd = hWnd;
	Initialize();
}

DirectInputManager::~DirectInputManager()
{
	for(DirectInputData &data: _joysticks) {
		data.joystick->Unacquire();
		data.joystick->Release();
	}
	_joysticks.clear();

	if(_directInput) {
		_directInput->Release();
		_directInput = nullptr;
	}
}
