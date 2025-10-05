#ifdef __WXMSW__

#define ISOLATION_AWARE_ENABLED 1

#include "WindowsVisualThemeSupport.h"
#include "Common.h"

#pragma comment(linker,"/manifestdependency:\"type='win32' name='Microsoft.Windows.Common-Controls' version='6.0.0.0' processorArchitecture='*' publicKeyToken='6595b64144ccf1df' language='*'\"")

namespace Alternet::UI
{
    namespace
    {
        HMODULE GetThisDllModuleHandle()
        {
            HMODULE moduleHandle = NULL;

            auto result = GetModuleHandleEx(
                GET_MODULE_HANDLE_EX_FLAG_FROM_ADDRESS | GET_MODULE_HANDLE_EX_FLAG_UNCHANGED_REFCOUNT,
                (LPCWSTR)&GetThisDllModuleHandle,
                &moduleHandle);

            if (result == 0)
				throw std::runtime_error("Failed to get module handle");

            return moduleHandle;
        }
    }

    WindowsVisualThemeSupport::WindowsVisualThemeSupport()
    {
        ACTCTX context{ sizeof(ACTCTX) };

        context.hModule = GetThisDllModuleHandle();
        context.lpResourceName = ISOLATIONAWARE_MANIFEST_RESOURCE_ID;
        context.dwFlags = ACTCTX_FLAG_HMODULE_VALID | ACTCTX_FLAG_RESOURCE_NAME_VALID;

        activationContextHandle = CreateActCtx(&context);
        if (activationContextHandle == INVALID_HANDLE_VALUE)
            throw std::runtime_error("Failed to create activation context");
    }

    WindowsVisualThemeSupport::~WindowsVisualThemeSupport()
    {
        if (activationContextHandle != INVALID_HANDLE_VALUE)
        {
            ReleaseActCtx(activationContextHandle);
            activationContextHandle = INVALID_HANDLE_VALUE;
        }
    }

    /*static*/ WindowsVisualThemeSupport& WindowsVisualThemeSupport::GetInstance()
    {
        if (instance == nullptr)
            instance = new WindowsVisualThemeSupport();
        return *instance;
    }

    ULONG_PTR WindowsVisualThemeSupport::Enable()
    {
        assert(activationContextHandle != INVALID_HANDLE_VALUE);

        if (IsEnabled())
            return NULL;

        ULONG_PTR activation = NULL;
        if (ActivateActCtx(activationContextHandle, &activation) == FALSE)
            throw std::runtime_error("Failed to activate context");

        return activation;
    }

    void WindowsVisualThemeSupport::Disable(ULONG_PTR activation)
    {
        assert(activationContextHandle != INVALID_HANDLE_VALUE);
        if (activation == NULL)
            return;

        if (DeactivateActCtx(0, activation) == FALSE)
            throw std::runtime_error("Failed to deactivate context");
    }

    bool WindowsVisualThemeSupport::IsEnabled()
    {
        assert(activationContextHandle != INVALID_HANDLE_VALUE);

        HANDLE currentContextHandle = NULL;
        if (GetCurrentActCtx(&currentContextHandle) == FALSE)
            throw std::runtime_error("Failed to get current context");

        return currentContextHandle == activationContextHandle;
    }
}
#endif