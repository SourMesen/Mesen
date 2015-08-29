#include "stdafx.h"
#include "UPnPPortMapper.h"

#ifdef WIN32
#include <winsock2.h>
#include <natupnp.h>
#include <ws2tcpip.h>

bool UPnPPortMapper::AddNATPortMapping(uint16_t internalPort, uint16_t externalPort, IPProtocol protocol) 
{
	bool result = false;
		
	CoInitializeEx(nullptr, COINIT_MULTITHREADED);

	IUPnPNAT *nat = nullptr;
	HRESULT hResult = CoCreateInstance(__uuidof(UPnPNAT), nullptr, CLSCTX_ALL, __uuidof(IUPnPNAT), (void**)&nat);

	BSTR proto = SysAllocString((protocol == IPProtocol::TCP) ? L"TCP" : L"UDP");

	if(SUCCEEDED(hResult) && nat) {
		IStaticPortMappingCollection *spmc = nullptr;
		hResult = nat->get_StaticPortMappingCollection(&spmc);
		if(SUCCEEDED(hResult) && spmc) {
			IStaticPortMapping *spm = nullptr;
			hResult = spmc->get_Item(externalPort, proto, &spm);
			if(spm != nullptr) {
				//An identical mapping already exists, remove it
				if(RemoveNATPortMapping(externalPort, protocol)) {
					std::cout << "Removed existing UPnP mapping." << std::endl;
					spm->Release();
					spm = nullptr;
				}
			}

			if(!SUCCEEDED(hResult) || spm == nullptr) {
				std::cout << "Attempting to automatically forward port via UPnP..." << std::endl;

				wstring localIP = GetLocalIP();
				BSTR desc = SysAllocString(L"Mesen NetPlay");
				BSTR clientStr = SysAllocString(localIP.c_str());
				hResult = spmc->Add(externalPort, proto, internalPort, clientStr, true, desc, &spm);
				SysFreeString(clientStr);
				SysFreeString(desc);
					
				if(SUCCEEDED(hResult) && spm) {
					//Successfully added a new port mapping
					std::cout << std::dec << "Forwarded port " << externalPort << " to IP ";
					std::wcout << localIP.c_str() << std::endl;
					spm->Release();
					result = true;
				} else {
					std::cout << "Unable to add UPnP port mapping.  ";
					std::cout << "IP: ";
					std::wcout << localIP.c_str();
					std::cout << " HRESULT: 0x" << std::hex << hResult << std::endl;
				}
			} else {
				std::cout << "Unable to add UPnP port mapping." << std::endl;
			}
			spmc->Release();
		}
		nat->Release();
	}

	SysFreeString(proto);
	
	CoUninitialize();

	return result;
}

bool UPnPPortMapper::RemoveNATPortMapping(uint16_t externalPort, IPProtocol protocol) 
{
	IUPnPNAT *nat = nullptr;
	IStaticPortMappingCollection *spmc;
	bool result = false;

	CoInitializeEx(nullptr, COINIT_MULTITHREADED);

	HRESULT hResult = ::CoCreateInstance(__uuidof(UPnPNAT), nullptr, CLSCTX_ALL, __uuidof(IUPnPNAT), (void**)&nat);

	BSTR proto = SysAllocString((protocol == IPProtocol::TCP) ? L"TCP" : L"UDP");

	if(SUCCEEDED(hResult) && nat) {
		hResult = nat->get_StaticPortMappingCollection(&spmc);
		if(SUCCEEDED(hResult) && spmc) {
			spmc->Remove(externalPort, proto);
			spmc->Release();
			result = true;
		}
		nat->Release();
	}

	SysFreeString(proto);

	CoUninitialize();

	return result;
}

wstring UPnPPortMapper::GetLocalIP()
{
	wstring localIP;

	ADDRINFOW *result = nullptr;
	ADDRINFOW hints;

	ZeroMemory(&hints, sizeof(hints));
	hints.ai_family = AF_INET;
	hints.ai_socktype = SOCK_STREAM;
	hints.ai_protocol = IPPROTO_TCP;

	wchar_t hostName[255];
	DWORD hostSize = 255;
	GetComputerName(hostName, &hostSize);

	if(GetAddrInfoW(hostName, nullptr, &hints, &result) == 0) {
		wchar_t ipAddr[255];
		DWORD ipSize = 255;
		if(WSAAddressToString(result->ai_addr, (DWORD)result->ai_addrlen, nullptr, ipAddr, &ipSize) == 0) {
			localIP = ipAddr;
		}
		FreeAddrInfoW(result);
	}
	return localIP;
}

#else
	
bool UPnPPortMapper::AddNATPortMapping(uint16_t internalPort, uint16_t externalPort, IPProtocol protocol) 
{
	return false;
}

bool UPnPPortMapper::RemoveNATPortMapping(uint16_t externalPort, IPProtocol protocol) 
{
	return false;
}

wstring UPnPPortMapper::GetLocalIP()
{
	return L"";
}	
	
#endif