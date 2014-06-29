//////////////////////////////////////////////////////////////////////////
//
// FILE: utf8conv.h
//
// Header file defining prototypes of helper functions for converting 
// strings between Unicode UTF-8 and UTF-16.
// (The implementation file is "utf8conv_inl.h").
//
// UTF-8 text is stored in std::string; 
// UTF-16 text is stored in std::wstring.
//
// This code just uses Win32 Platform SDK and C++ standard library; 
// so it can be used also with the Express editions of Visual Studio.
//
//
// Original code: February 4th, 2011
// Last update:   October 15th, 2011
//
// - Added more information to the utf8_conversion_error class
//   (like the return code of ::GetLastError());
//   moreover, the class now derives from std::runtime_error.
//
// - Added conversion function overloads taking raw C strings as input.
//   (This is more efficient when there are raw C strings already
//   available, because it avoids the creation of temporary
//   new std::[w]string's.)
//
// - UTF-8 conversion functions now detect invalid UTF-8 sequences
//   thanks to MB_ERR_INVALID_CHARS flag, and throw an exception
//   in this case.
//
//
// by Giovanni Dicanio <gdicanio@mvps.org>
//
//////////////////////////////////////////////////////////////////////////


#pragma once


//------------------------------------------------------------------------
//                              INCLUDES
//------------------------------------------------------------------------

#include <stdexcept>    // std::runtime_error
#include <string>       // STL string classes



namespace utf8util {



//------------------------------------------------------------------------
// Exception class representing an error occurred during UTF-8 conversion.
//------------------------------------------------------------------------
class utf8_conversion_error 
    : public std::runtime_error
{
public:
  
    //
    // Naming convention note:
    // -----------------------
    //
    // This exception class is derived from std::runtime_error class,
    // so I chose to use the same naming convention of STL classes
    // (e.g. do_something_intersting() instead of DoSomethingInteresting()).
    //


    // Error code type 
    // (a DWORD, as the return value type from ::GetLastError())
    typedef unsigned long error_code_type;

    // Type of conversion
    enum conversion_type
    {
        conversion_utf8_from_utf16,     // UTF-16 ---> UTF-8
        conversion_utf16_from_utf8      // UTF-8  ---> UTF-16
    };


    // Constructs an UTF-8 conversion error exception 
    // with a raw C string message, conversion type and error code.
    utf8_conversion_error(
        const char * message, 
        conversion_type conversion, 
        error_code_type error_code
    );


    // Constructs an UTF-8 conversion error exception 
    // with a std::string message, conversion type and error code.
    utf8_conversion_error(
        const std::string & message, 
        conversion_type conversion, 
        error_code_type error_code
    );


    // Returns the type of conversion (UTF-8 from UTF-16, or vice versa)
    conversion_type conversion() const;


    // Returns the error code occurred during the conversion
    // (which is typically the return value of ::GetLastError()).
    error_code_type error_code() const;



    //
    // IMPLEMENTATION
    //
private:
    conversion_type m_conversion;   // kind of conversion
    error_code_type m_error_code;   // error code
};

//------------------------------------------------------------------------



//------------------------------------------------------------------------
// Converts a string from UTF-8 to UTF-16.
// On error, can throw an utf8_conversion_error exception.
//------------------------------------------------------------------------
std::wstring UTF16FromUTF8(const std::string & utf8);


//------------------------------------------------------------------------
// Converts a raw C string from UTF-8 to UTF-16.
// On error, can throw an utf8_conversion_error exception.
// If the input pointer is NULL, an empty string is returned.
//------------------------------------------------------------------------
std::wstring UTF16FromUTF8(const char * utf8);


//------------------------------------------------------------------------
// Converts a string from UTF-16 to UTF-8.
// On error, can throw an utf8_conversion_error exception.
//------------------------------------------------------------------------
std::string UTF8FromUTF16(const std::wstring & utf16);


//------------------------------------------------------------------------
// Converts a raw C string from UTF-16 to UTF-8.
// On error, can throw an utf8_conversion_error exception.
// If the input pointer is NULL, an empty string is returned.
//------------------------------------------------------------------------
std::string UTF8FromUTF16(const wchar_t * utf16);


} // namespace utf8util



#include "utf8conv_inl.h"     // inline implementations


//////////////////////////////////////////////////////////////////////////

