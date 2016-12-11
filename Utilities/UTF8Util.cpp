#include "stdafx.h"
#include "UTF8Util.h"

#ifdef _MSC_VER
#define WIN32_LEAN_AND_MEAN
#define _WINSOCKAPI_
#include <Windows.h>

namespace utf8 {
	std::wstring utf8::decode(const std::string &str)
	{
		if(str.empty()) return std::wstring();
		int size_needed = MultiByteToWideChar(CP_UTF8, 0, &str[0], (int)str.size(), NULL, 0);
		std::wstring wstrTo(size_needed, 0);
		MultiByteToWideChar(CP_UTF8, 0, &str[0], (int)str.size(), &wstrTo[0], size_needed);
		return wstrTo;
	}

	std::string utf8::encode(const std::wstring &wstr)
	{
		if(wstr.empty()) return std::string();
		int size_needed = WideCharToMultiByte(CP_UTF8, 0, &wstr[0], (int)wstr.size(), NULL, 0, NULL, NULL);
		std::string strTo(size_needed, 0);
		WideCharToMultiByte(CP_UTF8, 0, &wstr[0], (int)wstr.size(), &strTo[0], size_needed, NULL, NULL);
		return strTo;
	}
}
#else
#include <codecvt>
#include <locale>
namespace utf8 {
	std::wstring utf8::decode(const std::string &str)
	{
		std::wstring_convert<std::codecvt_utf8<wchar_t>> myconv;
		return myconv.from_bytes(str);
	}

	std::string utf8::encode(const std::wstring &wstr)
	{
		std::wstring_convert<std::codecvt_utf8<wchar_t>> myconv;
    	return myconv.to_bytes(wstr);
	}
}
#endif