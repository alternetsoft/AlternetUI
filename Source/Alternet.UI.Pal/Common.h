#pragma once

#include "CommonBase.h"

#include "Api/DrawingTypes.h"
#include "DelayedValue.h"

namespace Alternet::UI
{
    typedef std::u16string string;

    template<typename T> using optional = std::optional<T>;
    using std::nullopt;

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

    namespace Collections
    {
        template <typename T> size_t IndexOf(const std::vector<T>& vector, const T& value)
        {
            auto it = std::find(vector.begin(), vector.end(), value);
            if (it == vector.end())
                return -1;

            return distance(vector.begin(), it);
        }
    }

    inline int ConvertToIntChecked(size_t value)
    {
        if (value > INT_MAX)
            throw std::overflow_error("size_t to int conversion overflow.");
        return static_cast<int>(value);
    }
}