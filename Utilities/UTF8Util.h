#pragma once

#include <fstream>
#ifdef _MSC_VER
#define WIN32_LEAN_AND_MEAN
#include <Windows.h>
#endif

namespace utf8 {
#ifdef _WIN32
	class utf8
	{
	public:
		static std::wstring decode(const std::string &str)
		{
			if(str.empty()) return std::wstring();
			int size_needed = MultiByteToWideChar(CP_UTF8, 0, &str[0], (int)str.size(), NULL, 0);
			std::wstring wstrTo(size_needed, 0);
			MultiByteToWideChar(CP_UTF8, 0, &str[0], (int)str.size(), &wstrTo[0], size_needed);
			return wstrTo;
		}

		static std::string encode(const std::wstring &wstr)
		{
			if(wstr.empty()) return std::string();
			int size_needed = WideCharToMultiByte(CP_UTF8, 0, &wstr[0], (int)wstr.size(), NULL, 0, NULL, NULL);
			std::string strTo(size_needed, 0);
			WideCharToMultiByte(CP_UTF8, 0, &wstr[0], (int)wstr.size(), &strTo[0], size_needed, NULL, NULL);
			return strTo;
		}
	};

	class ifstream : public std::ifstream
	{
	public:
		ifstream(const std::string& _Str, ios_base::openmode _Mode = ios_base::in, int _Prot = (int)ios_base::_Openprot) : std::ifstream(utf8::decode(_Str), _Mode, _Prot)
		{
		}

		ifstream() : std::ifstream()
		{
		}

		void open(const std::string& _Str, ios_base::openmode _Mode = ios_base::in, int _Prot = (int)ios_base::_Openprot)
		{
			std::ifstream::open(utf8::decode(_Str), _Mode, _Prot);
		}
	};

	class ofstream : public std::ofstream
	{
	public:
		ofstream(const std::string& _Str, ios_base::openmode _Mode = ios_base::in, int _Prot = (int)ios_base::_Openprot) : std::ofstream(utf8::decode(_Str), _Mode, _Prot)
		{
		}

		ofstream() : std::ofstream()
		{
		}

		void open(const std::string& _Str, ios_base::openmode _Mode = ios_base::in, int _Prot = (int)ios_base::_Openprot)
		{
			std::ofstream::open(utf8::decode(_Str), _Mode, _Prot);
		}
	};
#else
	using std::ifstream;
	using std::ofstream;
#endif
}