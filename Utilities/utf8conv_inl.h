//////////////////////////////////////////////////////////////////////////
//
// FILE: utf8conv_inl.h
//
// by Giovanni Dicanio <gdicanio@mvps.org>
//
// Private header file containing implementations of inline functions.
// The public header file for this module is "utf8conv.h"; 
// users should *not* #include this private header file directly.
//
//////////////////////////////////////////////////////////////////////////

#pragma once


#include <string.h>     // strlen()

#include <Windows.h>    // Win32 Platform SDK main header



namespace utf8util {


//------------------------------------------------------------------------
//      Implementation of utf8_conversion_error class methods
//------------------------------------------------------------------------

inline utf8_conversion_error::utf8_conversion_error(
    const char * message, 
    conversion_type conversion, 
    error_code_type error_code
    ) : 
        std::runtime_error(message),
        m_conversion(conversion),
        m_error_code(error_code)
{
}


inline utf8_conversion_error::utf8_conversion_error(
    const std::string & message, 
    conversion_type conversion, 
    error_code_type error_code
    ) : 
        std::runtime_error(message),
        m_conversion(conversion),
        m_error_code(error_code)
{
}


inline utf8_conversion_error::conversion_type utf8_conversion_error::conversion() const
{
    return m_conversion;
}


inline utf8_conversion_error::error_code_type utf8_conversion_error::error_code() const
{
    return m_error_code;
}



//------------------------------------------------------------------------
//              Implementation of module functions
//------------------------------------------------------------------------


inline std::wstring UTF16FromUTF8(const std::string & utf8)
{
    //
    // Special case of empty input string
    //
    if (utf8.empty())
        return std::wstring();


    // Fail if an invalid input character is encountered
    const DWORD conversionFlags = MB_ERR_INVALID_CHARS;


    //
    // Get length (in wchar_t's) of resulting UTF-16 string
    //
    const int utf16Length = ::MultiByteToWideChar(
        CP_UTF8,            // convert from UTF-8
        conversionFlags,    // flags
        utf8.data(),        // source UTF-8 string
        utf8.length(),      // length (in chars) of source UTF-8 string
        NULL,               // unused - no conversion done in this step
        0                   // request size of destination buffer, in wchar_t's
        );
    if (utf16Length == 0)
    {
        // Error
        DWORD error = ::GetLastError();

        throw utf8_conversion_error(
            (error == ERROR_NO_UNICODE_TRANSLATION) ? 
                "Invalid UTF-8 sequence found in input string." :
                "Can't get length of UTF-16 string (MultiByteToWideChar failed).", 
            utf8_conversion_error::conversion_utf16_from_utf8,
            error);      
    }


    //
    // Allocate destination buffer for UTF-16 string
    //
    std::wstring utf16;
    utf16.resize(utf16Length);


    //
    // Do the conversion from UTF-8 to UTF-16
    //
    if ( ! ::MultiByteToWideChar(
        CP_UTF8,            // convert from UTF-8
        0,                  // validation was done in previous call, 
                            // so speed up things with default flags
        utf8.data(),        // source UTF-8 string
        utf8.length(),      // length (in chars) of source UTF-8 string
        &utf16[0],          // destination buffer
        utf16.length()      // size of destination buffer, in wchar_t's
        ) )
    {
        // Error
        DWORD error = ::GetLastError();
        throw utf8_conversion_error(
            "Can't convert string from UTF-8 to UTF-16 (MultiByteToWideChar failed).", 
            utf8_conversion_error::conversion_utf16_from_utf8,
            error);
    }


    //
    // Return resulting UTF-16 string
    //
    return utf16;
}



inline std::wstring UTF16FromUTF8(const char * utf8)
{
    //
    // Special case of empty input string
    //
    if (utf8 == NULL || *utf8 == '\0')
        return std::wstring();


    // Prefetch the length of the input UTF-8 string
    const int utf8Length = static_cast<int>(strlen(utf8));

    // Fail if an invalid input character is encountered
    const DWORD conversionFlags = MB_ERR_INVALID_CHARS;

    //
    // Get length (in wchar_t's) of resulting UTF-16 string
    //
    const int utf16Length = ::MultiByteToWideChar(
        CP_UTF8,            // convert from UTF-8
        conversionFlags,    // flags
        utf8,               // source UTF-8 string
        utf8Length,         // length (in chars) of source UTF-8 string
        NULL,               // unused - no conversion done in this step
        0                   // request size of destination buffer, in wchar_t's
        );
    if (utf16Length == 0)
    {
        // Error
        DWORD error = ::GetLastError();
        throw utf8_conversion_error(
            (error == ERROR_NO_UNICODE_TRANSLATION) ? 
            "Invalid UTF-8 sequence found in input string." :
            "Can't get length of UTF-16 string (MultiByteToWideChar failed).", 
            utf8_conversion_error::conversion_utf16_from_utf8,
            error);
    }


    //
    // Allocate destination buffer for UTF-16 string
    //
    std::wstring utf16;
    utf16.resize(utf16Length);


    //
    // Do the conversion from UTF-8 to UTF-16
    //
    if ( ! ::MultiByteToWideChar(
        CP_UTF8,            // convert from UTF-8
        0,                  // validation was done in previous call, 
                            // so speed up things with default flags
        utf8,               // source UTF-8 string
        utf8Length,         // length (in chars) of source UTF-8 string
        &utf16[0],          // destination buffer
        utf16.length()      // size of destination buffer, in wchar_t's
        ) )
    {
        // Error
        DWORD error = ::GetLastError();
        throw utf8_conversion_error(
            "Can't convert string from UTF-8 to UTF-16 (MultiByteToWideChar failed).", 
            utf8_conversion_error::conversion_utf16_from_utf8,
            error);
    }


    //
    // Return resulting UTF-16 string
    //
    return utf16;
}



inline std::string UTF8FromUTF16(const std::wstring & utf16)
{
    //
    // Special case of empty input string
    //
    if (utf16.empty())
        return std::string();


    //
    // Get length (in chars) of resulting UTF-8 string
    //
    const int utf8Length = ::WideCharToMultiByte(
        CP_UTF8,            // convert to UTF-8
        0,                  // default flags
        utf16.data(),       // source UTF-16 string
        utf16.length(),     // source string length, in wchar_t's,
        NULL,               // unused - no conversion required in this step
        0,                  // request buffer size
        NULL, NULL          // unused
        );
    if (utf8Length == 0)
    {
        // Error
        DWORD error = ::GetLastError();
        throw utf8_conversion_error(
            "Can't get length of UTF-8 string (WideCharToMultiByte failed).", 
            utf8_conversion_error::conversion_utf8_from_utf16,
            error);
    }


    //
    // Allocate destination buffer for UTF-8 string
    //
    std::string utf8;
    utf8.resize(utf8Length);


    //
    // Do the conversion from UTF-16 to UTF-8
    //
    if ( ! ::WideCharToMultiByte(
        CP_UTF8,                // convert to UTF-8
        0,                      // default flags
        utf16.data(),           // source UTF-16 string
        utf16.length(),         // source string length, in wchar_t's,
        &utf8[0],               // destination buffer
        utf8.length(),          // destination buffer size, in chars
        NULL, NULL              // unused
        ) )
    {
        // Error
        DWORD error = ::GetLastError();
        throw utf8_conversion_error(
            "Can't convert string from UTF-16 to UTF-8 (WideCharToMultiByte failed).", 
            utf8_conversion_error::conversion_utf8_from_utf16,
            error);
    }


    //
    // Return resulting UTF-8 string
    //
    return utf8;
}



inline std::string UTF8FromUTF16(const wchar_t * utf16)
{
    //
    // Special case of empty input string
    //
    if (utf16 == NULL || *utf16 == L'\0')
        return std::string();


    // Prefetch the length of the input UTF-16 string
    const int utf16Length = static_cast<int>(wcslen(utf16));
  

    //
    // Get length (in chars) of resulting UTF-8 string
    //
    const int utf8Length = ::WideCharToMultiByte(
        CP_UTF8,            // convert to UTF-8
        0,                  // default flags
        utf16,              // source UTF-16 string
        utf16Length,        // source string length, in wchar_t's,
        NULL,               // unused - no conversion required in this step
        0,                  // request buffer size
        NULL, NULL          // unused
        );
    if (utf8Length == 0)
    {
        // Error
        DWORD error = ::GetLastError();
        throw utf8_conversion_error(
            "Can't get length of UTF-8 string (WideCharToMultiByte failed).", 
            utf8_conversion_error::conversion_utf8_from_utf16,
            error);
    }


    //
    // Allocate destination buffer for UTF-8 string
    //
    std::string utf8;
    utf8.resize(utf8Length);


    //
    // Do the conversion from UTF-16 to UTF-8
    //
    if ( ! ::WideCharToMultiByte(
        CP_UTF8,                // convert to UTF-8
        0,                      // default flags
        utf16,                  // source UTF-16 string
        utf16Length,            // source string length, in wchar_t's,
        &utf8[0],               // destination buffer
        utf8.length(),          // destination buffer size, in chars
        NULL, NULL              // unused
        ) )
    {
        // Error
        DWORD error = ::GetLastError();
        throw utf8_conversion_error(
            "Can't convert string from UTF-16 to UTF-8 (WideCharToMultiByte failed).", 
            utf8_conversion_error::conversion_utf8_from_utf16,
            error);
    }


    //
    // Return resulting UTF-8 string
    //
    return utf8;
}



} // namespace utf8util


//////////////////////////////////////////////////////////////////////////

