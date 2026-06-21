#pragma once

#if defined(__WXMSW__)
#define DLLEXPORT_SPEC __declspec(dllexport)
#else
#define DLLEXPORT_SPEC __attribute__((visibility("default")))
#endif

#define ALTERNET_UI_API extern "C" DLLEXPORT_SPEC


#if defined(__WXMSW__)
#include <Windows.h>
#endif

#include "Common.h"

#define c_bool int
#define c_true 1
#define c_false 0

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

