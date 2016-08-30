#include "stdafx.h"

#define DIRECTINPUT_VERSION 0x0800
#include <thread>
#include <windows.h>
#include <dinput.h>
#include <dinputd.h>
#include <wbemidl.h>
#include <oleauto.h>
#include "DirectInputManager.h"
#include <algorithm>

LPDIRECTINPUT8 DirectInputManager::_directInput = nullptr;
vector<DirectInputData> DirectInputManager::_joysticks;
std::vector<GUID> DirectInputManager::_xinputDeviceGuids;
std::vector<GUID> DirectInputManager::_directInputDeviceGuids;
bool DirectInputManager::_needToUpdate;

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

bool DirectInputManager::ProcessDevice(const GUID* deviceGuid, bool checkOnly)
{
	auto comp = [=](GUID guid) {
		return guid.Data1 == deviceGuid->Data1 &&
			guid.Data2 == deviceGuid->Data2 &&
			guid.Data3 == deviceGuid->Data3 &&
			memcmp(guid.Data4, deviceGuid->Data4, sizeof(guid.Data4)) == 0;
	};

	bool knownXInputDevice = std::find_if(_xinputDeviceGuids.begin(), _xinputDeviceGuids.end(), comp) != _xinputDeviceGuids.end();
	bool knownDirectInputDevice = std::find_if(_directInputDeviceGuids.begin(), _directInputDeviceGuids.end(), comp) != _directInputDeviceGuids.end();
	if(knownXInputDevice || knownDirectInputDevice) {
		return false;
	} else {
		bool isXInput = IsXInputDevice(deviceGuid);
		if(!checkOnly) {
			if(isXInput) {
				_xinputDeviceGuids.push_back(*deviceGuid);
			} else {
				_directInputDeviceGuids.push_back(*deviceGuid);
			}
		}
		return !isXInput;
	}
}

//-----------------------------------------------------------------------------
// Enum each PNP device using WMI and check each device ID to see if it contains 
// "IG_" (ex. "VID_045E&PID_028E&IG_00").  If it does, then it's an XInput device
// Unfortunately this information can not be found by just using DirectInput 
//-----------------------------------------------------------------------------
bool DirectInputManager::IsXInputDevice(const GUID* pGuidProductFromDirectInput)
{
	IWbemLocator*           pIWbemLocator = NULL;
	IEnumWbemClassObject*   pEnumDevices = NULL;
	IWbemClassObject*       pDevices[20] = { 0 };
	IWbemServices*          pIWbemServices = NULL;
	BSTR                    bstrNamespace = NULL;
	BSTR                    bstrDeviceID = NULL;
	BSTR                    bstrClassName = NULL;
	DWORD                   uReturned = 0;
	bool                    bIsXinputDevice = false;
	UINT                    iDevice = 0;
	VARIANT                 var;
	HRESULT                 hr;

	// CoInit if needed
	hr = CoInitialize(NULL);
	bool bCleanupCOM = SUCCEEDED(hr);

	// Create WMI
	hr = CoCreateInstance(__uuidof(WbemLocator), NULL, CLSCTX_INPROC_SERVER, __uuidof(IWbemLocator), (LPVOID*)&pIWbemLocator);
	if(FAILED(hr) || pIWbemLocator == NULL) {
		goto LCleanup;
	}

	bstrNamespace = SysAllocString(L"\\\\.\\root\\cimv2"); 
	bstrClassName = SysAllocString(L"Win32_PNPEntity");
	bstrDeviceID = SysAllocString(L"DeviceID");

	// Connect to WMI 
	hr = pIWbemLocator->ConnectServer(bstrNamespace, NULL, NULL, 0L, 0L, NULL, NULL, &pIWbemServices);
	if(FAILED(hr) || pIWbemServices == NULL) {
		goto LCleanup;
	}

	// Switch security level to IMPERSONATE. 
	CoSetProxyBlanket(pIWbemServices, RPC_C_AUTHN_WINNT, RPC_C_AUTHZ_NONE, NULL, RPC_C_AUTHN_LEVEL_CALL, RPC_C_IMP_LEVEL_IMPERSONATE, NULL, EOAC_NONE);

	hr = pIWbemServices->CreateInstanceEnum(bstrClassName, 0, NULL, &pEnumDevices);
	if(FAILED(hr) || pEnumDevices == NULL) {
		goto LCleanup;
	}

	// Loop over all devices
	for(;; ) {
		// Get 20 at a time
		hr = pEnumDevices->Next(10000, 20, pDevices, &uReturned);
		if(FAILED(hr) || uReturned == 0 || bIsXinputDevice) {
			break;
		}

		for(iDevice = 0; iDevice < uReturned; iDevice++) {
			// For each device, get its device ID
			hr = pDevices[iDevice]->Get(bstrDeviceID, 0L, &var, NULL, NULL);
			if(SUCCEEDED(hr) && var.vt == VT_BSTR && var.bstrVal != NULL) {
				// Check if the device ID contains "IG_".  If it does, then it's an XInput device
				// This information can not be found from DirectInput 
				if(wcsstr(var.bstrVal, L"IG_")) {
					// If it does, then get the VID/PID from var.bstrVal
					DWORD dwPid = 0, dwVid = 0;
					WCHAR* strVid = wcsstr(var.bstrVal, L"VID_");
					if(strVid && swscanf_s(strVid, L"VID_%4X", &dwVid) != 1) {
						dwVid = 0;
					}
					WCHAR* strPid = wcsstr(var.bstrVal, L"PID_");
					if(strPid && swscanf_s(strPid, L"PID_%4X", &dwPid) != 1) {
						dwPid = 0;
					}

					// Compare the VID/PID to the DInput device
					DWORD dwVidPid = MAKELONG(dwVid, dwPid);
					if(dwVidPid == pGuidProductFromDirectInput->Data1) {
						bIsXinputDevice = true;
						pDevices[iDevice]->Release();
						pDevices[iDevice] = nullptr;
						break;
					}
				}
			}
			VariantClear(&var);
			pDevices[iDevice]->Release();
			pDevices[iDevice] = nullptr;
		}
	}

LCleanup:
	if(bstrNamespace) {
		SysFreeString(bstrNamespace);
	}
	if(bstrDeviceID) {
		SysFreeString(bstrDeviceID);
	}
	if(bstrClassName) {
		SysFreeString(bstrClassName);
	}
	for(iDevice = 0; iDevice < 20; iDevice++) {
		if(pDevices[iDevice]) {
			pDevices[iDevice]->Release();
		}
	}
	if(pEnumDevices) {
		pEnumDevices->Release();
	}
	if(pIWbemLocator) {
		pIWbemLocator->Release();
	}
	if(pIWbemServices) {
		pIWbemServices->Release();
	}

	if(bCleanupCOM) {
		CoUninitialize();
	}

	return bIsXinputDevice;
}

bool DirectInputManager::NeedToUpdate()
{
	HRESULT hr;
	_needToUpdate = false;
	// Enumerate devices
	if(FAILED(hr = _directInput->EnumDevices(DI8DEVCLASS_GAMECTRL, NeedToUpdateCallback, nullptr, DIEDFL_ALLDEVICES))) {
		return false;
	}
	return _needToUpdate;
}

int DirectInputManager::NeedToUpdateCallback(const DIDEVICEINSTANCE* pdidInstance, void* pContext)
{
	if(ProcessDevice(&pdidInstance->guidProduct, true)) {
		_needToUpdate = true;
		return DIENUM_STOP;
	}

	return DIENUM_CONTINUE;
}


bool DirectInputManager::UpdateDeviceList()
{
	HRESULT hr;

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

	if(ProcessDevice(&pdidInstance->guidProduct, false)) {
		// Obtain an interface to the enumerated joystick.
		LPDIRECTINPUTDEVICE8 pJoystick = nullptr;
		hr = _directInput->CreateDevice(pdidInstance->guidInstance, &pJoystick, nullptr);

		if(SUCCEEDED(hr)) {
			DIJOYSTATE2 state;
			memset(&state, 0, sizeof(state));
			DirectInputData data{ pJoystick, state, state };
			_joysticks.push_back(data);
		}
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
	if(port >= (int)_joysticks.size()) {
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
