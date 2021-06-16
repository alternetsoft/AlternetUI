// Auto generated code, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2021.

#pragma once

#include "Window.h"
#include "ApiUtils.h"

using namespace Alternet::UI;

ALTERNET_UI_API Window* Window_Create()
{
    return new Window();
}

ALTERNET_UI_API char16_t* Window_GetTitle(Window* obj)
{
    return AllocPInvokeReturnString(obj->GetTitle());
}

ALTERNET_UI_API void Window_SetTitle(Window* obj, const char16_t* value)
{
    obj->SetTitle(value);
}

ALTERNET_UI_API WindowStartPosition Window_GetWindowStartPosition(Window* obj)
{
    return obj->GetWindowStartPosition();
}

ALTERNET_UI_API void Window_SetWindowStartPosition(Window* obj, WindowStartPosition value)
{
    obj->SetWindowStartPosition(value);
}

ALTERNET_UI_API void Window_SetEventCallback(Window::WindowEventCallbackType callback)
{
    Window::SetEventCallback(callback);
}

