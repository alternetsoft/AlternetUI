#pragma once

#include <algorithm>
#include "Types.h"

namespace Alternet::UI
{
    namespace
    {
        const string WhitespaceCharacters = u" \n\r\t";
    }

    inline string TrimStart(const string& s, const string& characters)
    {
        size_t startpos = s.find_first_not_of(characters);
        return (startpos == std::string::npos) ? u"" : s.substr(startpos);
    }
    
    inline string TrimStart(const string& s)
    {
        return TrimStart(s, WhitespaceCharacters);
    }

    inline string TrimEnd(const string& s, const string& characters)
    {
        size_t endpos = s.find_last_not_of(characters);
        return (endpos == std::string::npos) ? u"" : s.substr(0, endpos + 1);
    }

    inline string TrimEnd(const string& s)
    {
        return TrimEnd(s, WhitespaceCharacters);
    }

    inline string Trim(const string& s, const string& characters)
    {
        return TrimEnd(TrimStart(s, characters), characters);
    }

    inline string Trim(const string& s)
    {
        return Trim(s, WhitespaceCharacters);
    }

    // todo: move implementation into source file.
    inline std::vector<string> Split(const string& s, const string& delim, const bool keep_empty = true)
    {
        std::vector<string> result;
        if (delim.empty())
            return result;

        string::const_iterator substart = s.begin(), subend;
        while (true)
        {
            subend = search(substart, s.end(), delim.begin(), delim.end());
            string temp(substart, subend);
            if (keep_empty || !temp.empty())
                result.push_back(temp);

            if (subend == s.end())
                break;

            substart = subend + delim.size();
        }

        return result;
    }

    inline bool EqualsIgnoreCase(const string& str1, const string& str2)
    {
        string str1Cpy(str1);
        string str2Cpy(str2);
        std::transform(str1Cpy.begin(), str1Cpy.end(), str1Cpy.begin(), ::tolower);
        std::transform(str2Cpy.begin(), str2Cpy.end(), str2Cpy.begin(), ::tolower);
        return (str1Cpy == str2Cpy);
    }

    inline bool StartsWith(const string& toCheck, const string& prefix)
    {
        return toCheck.substr(0, prefix.size()) == prefix;
    }

    inline bool EndsWith(const string& toCheck, const string& suffix)
    {
        if (toCheck.length() >= suffix.length())
            return toCheck.compare(toCheck.length() - suffix.length(), suffix.length(), suffix) == 0;

        return false;
    }

    inline bool Contains(const string& toCheck, const string& substring)
    {
        return toCheck.find(substring) != string::npos;
    }

    inline bool StartsWithIgnoreCase(const string& toCheck, const string& prefix)
    {
        return EqualsIgnoreCase(toCheck.substr(0, prefix.size()), prefix);
    }

    // todo: move implementation into source file.
    inline void Replace(string& source, const string& from, const string& to)
    {
        string newString;
        newString.reserve(source.length());  // avoids a few memory allocations

        string::size_type lastPos = 0;
        string::size_type findPos;

        while(string::npos != (findPos = source.find(from, lastPos)))
        {
            newString.append(source, lastPos, findPos - lastPos);
            newString += to;
            lastPos = findPos + from.length();
        }

        // Care for the rest after last occurrence
        newString += source.substr(lastPos);

        source.swap(newString);
    }
}