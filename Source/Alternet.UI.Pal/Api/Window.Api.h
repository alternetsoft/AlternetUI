#pragma once

#include "ApiUtils.h"
#include "Window.h"

using namespace Alternet::UI;

ALTERNET_UI_API Window* Window_Create()
{
    return new Window();
}

ALTERNET_UI_API void Window_Destroy(Window* obj)
{
    delete obj;
}

ALTERNET_UI_API void Window_SetTitle(Window* obj, const char16_t* value)
{
    obj->SetTitle(value);
}

ALTERNET_UI_API char16_t* Window_GetTitle(Window* obj)
{
    return AllocPInvokeReturnString(obj->GetTitle());
}

ALTERNET_UI_API void Window_Show(Window* obj)
{
    obj->Show();
}

ALTERNET_UI_API void Window_AddChildControl(Window* obj, Control* value)
{
    obj->AddChildControl(value);
}