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
#pragma pack(push, 1)

    extern "C"
    {
        struct NativeStringSpan_C
        {
            void* Pointer;   // UTF-16 on Windows, UTF-8 on Linux/macOS
            int Length;   // number of code units (chars or bytes)
        };
    }

#pragma pack(pop)

    struct NativeStringSpan : NativeStringSpan_C
    {
        NativeStringSpan()
        {
			Pointer = nullptr;
			Length = 0;
        }
        
        NativeStringSpan(const NativeStringSpan_C& c)
        {
			Pointer = c.Pointer;
			Length = c.Length;
        }
    };

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

    inline const wxString ArrayStringToString(const wxArrayString& items)
    {
        wxString joined;
        for (size_t i = 0; i < items.size(); ++i)
        {
            joined.append(items[i]);
            joined += L'\0';
        }
        return joined;
    }

    inline const NativeStringSpan_C wxStr(const wxString& s)
    {
        int length = static_cast<int>(s.length());

		if (length == 0)
		{
			return { nullptr, 0 };
		}

#if defined(__WXMSW__)
        // Windows: UTF-16
        const wchar_t* buf = s.c_str().AsWChar();

        return { (void*)buf, length };
#else
        // Linux/macOS: UTF-8
        const char* buf = s.utf8_str();
        return { (void*)buf, length };
#endif
    }

    inline wxString StringToWx(const void* text, size_t textLength)
    {
        if (text == nullptr)
        {
            return wxString();
        }

        if (textLength == 0)
        {
            return wxString();
        }

#ifdef __WXMSW__    
        // reinterpret as wide char pointer
        const wchar_t* wstr = reinterpret_cast<const wchar_t*>(text);

        // construct wxString directly
        wxString str(wstr, textLength);
#else
        const char* utf8 = reinterpret_cast<const char*>(text);
        wxString str(utf8, wxConvUTF8, textLength);
#endif

        return str;
    }

    inline wxString wxStr(const NativeStringSpan& value)
    {
        return StringToWx(value.Pointer, value.Length);
    }

    static void LogMessage(std::string msg)
    {
        wxLogMessage(msg.c_str());
    }
}

