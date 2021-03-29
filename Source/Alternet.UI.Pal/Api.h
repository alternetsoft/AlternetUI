#pragma once

#if defined(__WXMSW__)
#define DLLEXPORT_SPEC __declspec(dllexport)
#else
#define DLLEXPORT_SPEC __attribute__((visibility("default")))
#endif

#define ALTERNET_UI_API extern "C" DLLEXPORT_SPEC