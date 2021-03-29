#ifdef __WXMSW__
#pragma once

#include "Common.h"
#include <windows.h>

namespace Alternet::UI
{
    class WindowsVisualThemeSupport
    {
    public:
        ~WindowsVisualThemeSupport();

        static WindowsVisualThemeSupport& GetInstance();

        ULONG_PTR Enable();
        void Disable(ULONG_PTR activation);
        bool IsEnabled();

    private:
        WindowsVisualThemeSupport();

        inline static WindowsVisualThemeSupport* instance = nullptr;

        HANDLE activationContextHandle = INVALID_HANDLE_VALUE;

        BYREF_ONLY(WindowsVisualThemeSupport);
    };
}
#endif