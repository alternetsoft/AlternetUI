#pragma once

#if defined(__WXMSW__)
#include <Windows.h>
#endif

#include "Common.h"

inline void* AllocPInvokeMemory(size_t size)
{
#if defined(__WXMSW__)
    return ::CoTaskMemAlloc(size);
#else
    return malloc(size);
#endif
}

inline void FreePInvokeMemory(void* ptr)
{
#if defined(__WXMSW__)
    ::CoTaskMemFree(ptr);
#else
    return free(ptr);
#endif
}

inline char16_t* AllocPInvokeReturnString(const Alternet::UI::string& value)
{
    auto bufferSize = (value.length() + 1) * sizeof(char16_t);
    auto buffer = AllocPInvokeMemory(bufferSize);
    memcpy(buffer, (void*)&value[0], bufferSize);
    return (char16_t*)buffer;
}