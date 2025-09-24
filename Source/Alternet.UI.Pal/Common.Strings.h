#pragma once

#include <string>
#include <wx/string.h>
#include "Common.Base.h"
#include <stdint.h>
/*
#include <unicode/unistr.h>
*/

#ifdef __GNUC__
#pragma GCC diagnostic ignored "-Wdeprecated"
#endif

namespace Alternet::UI
{

/*
    std::u16string wstringToU16String(const std::wstring& wstr) {
        icu::UnicodeString unicodeStr(reinterpret_cast<const UChar*>(wstr.data()), wstr.length());
        return std::u16string(reinterpret_cast<const char16_t*>(unicodeStr.getBuffer()), unicodeStr.length());
    }

    std::wstring u16StringToWstring(const std::u16string& u16str) {
        icu::UnicodeString unicodeStr(reinterpret_cast<const UChar*>(u16str.data()), u16str.length());
        return std::wstring(reinterpret_cast<const wchar_t*>(unicodeStr.getBuffer()), unicodeStr.length());
    }
*/
    /*
    std::u16string To_UTF16(const std::u32string& s)
    {
        std::wstring_convert<std::codecvt_utf16<char32_t>, char32_t> conv;
        std::string bytes = conv.to_bytes(s);
        return std::u16string(reinterpret_cast<const char16_t*>(bytes.c_str()), bytes.length() / sizeof(char16_t));
    }

    std::u32string To_UTF32(const std::u16string& s)
    {
        const char16_t* pData = s.c_str();
        std::wstring_convert<std::codecvt_utf16<char32_t>, char32_t> conv;
        return conv.from_bytes(reinterpret_cast<const char*>(pData), reinterpret_cast<const char*>(pData + s.length()));
    }
    */

    static void LogMessage(std::string msg)
    {
        wxLogMessage(msg.c_str());
    }

    typedef std::u16string string;

#if !defined(__WXMSW__)

    // https://stackoverflow.com/a/42899668
    inline std::u16string make_u16string(const std::wstring& ws)
        /* Creates a UTF-16 string from a wide-character string.  Any wide characters
         * outside the allowed range of UTF-16 are mapped to the sentinel value U+FFFD,
         * per the Unicode documentation. (http://www.unicode.org/faq/private_use.html
         * retrieved 12 March 2017.) Unpaired surrogates in ws are also converted to
         * sentinel values.  Noncharacters, however, are left intact.  As a fallback,
         * if wide characters are the same size as char16_t, this does a more trivial
         * construction using that implicit conversion.
         */
    {
        /* We assume that, if this test passes, a wide-character string is already
         * UTF-16, or at least converts to it implicitly without needing surrogate
         * pairs.
         */
        if (sizeof(wchar_t) == sizeof(char16_t)) {
            return std::u16string(ws.begin(), ws.end());
        }
        else {
            /* The conversion from UTF-32 to UTF-16 might possibly require surrogates.
             * A surrogate pair suffices to represent all wide characters, because all
             * characters outside the range will be mapped to the sentinel value
             * U+FFFD.  Add one character for the terminating NUL.
             */
            const size_t max_len = 2 * ws.length() + 1;
            // Our temporary UTF-16 string.
            std::u16string result;

            result.reserve(max_len);

            for (const wchar_t& wc : ws) {
                const std::wint_t chr = wc;

                if (chr < 0 || chr > 0x10FFFF || (chr >= 0xD800 && chr <= 0xDFFF)) {
                    // Invalid code point.  Replace with sentinel, per Unicode standard:
                    constexpr char16_t sentinel = u'\uFFFD';
                    result.push_back(sentinel);
                }
                else if (chr < 0x10000UL) { // In the BMP.
                    result.push_back(static_cast<char16_t>(wc));
                }
                else {
                    const char16_t leading = static_cast<char16_t>(
                        ((chr - 0x10000UL) / 0x400U) + 0xD800U);
                    const char16_t trailing = static_cast<char16_t>(
                        ((chr - 0x10000UL) % 0x400U) + 0xDC00U);

                    result.append({ leading, trailing });
                } // end if
            } // end for

           /* The returned string is shrunken to fit, which might not be the Right
            * Thing if there is more to be added to the string.
            */
            result.shrink_to_fit();

            // We depend here on the compiler to optimize the move constructor.
            return result;
        } // end if
        // Not reached.
    }

#endif

    inline char16_t wcharToChar16(wchar_t value)
    {
        char16_t value16;
        if (sizeof(wchar_t) != sizeof(char16_t))
        {
#if !defined(__WXMSW__)
            std::wstring s(1, value);
            auto str = make_u16string(s);
            value16 = str[0];
#else
            throw 0;
#endif
        }
        else
        {
            value16 = value;
        }

        return value16;
    }

    inline wxString FromSmartString(void* text, int charLength)
    {
        if (!text || charLength <= 0)
            return wxString();

#if defined(__WXMSW__)
        // On Windows: UTF-16 input, reinterpret as wchar_t*
        const wchar_t* wide = reinterpret_cast<const wchar_t*>(text);
        return wxString(wide, charLength);
#else
        // On Linux/macOS: UTF-8 input, reinterpret as char*
        const char* utf8 = static_cast<const char*>(text);
        return wxString::FromUTF8(utf8, charLength);
#endif
    }

    inline wxString wxStr(const string& value)
    {
#if defined(__WXMSW__)
        return (wchar_t*)value.c_str();
#else
        // https://stackoverflow.com/a/8540710
        std::wstring_convert<std::codecvt_utf16<wchar_t, 0x10ffff, std::little_endian>, wchar_t> conv;
        auto s = conv.from_bytes(
            reinterpret_cast<const char*> (&value[0]),
            reinterpret_cast<const char*> (&value[0] + value.size()));
        return s.c_str();
#endif
    }

    inline string wxStr(const wxString& value)
    {
#if defined(__WXMSW__)
        return (char16_t*)value.c_str().AsWChar();
#else
        return make_u16string(value.c_str().AsWChar());
#endif
    }
}

