// Auto generated code, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2021.

#pragma once

#include "Button.h"
#include "ApiUtils.h"

using namespace Alternet::UI;

ALTERNET_UI_API Button* Button_Create()
{
    return new Button();
}

ALTERNET_UI_API char16_t* Button_GetText(Button* obj)
{
    return AllocPInvokeReturnString(obj->GetText());
}

ALTERNET_UI_API void Button_SetText(Button* obj, const char16_t* value)
{
    obj->SetText(value);
}

ALTERNET_UI_API void Button_SetEventCallback(Button::ButtonEventCallbackType callback)
{
    Button::SetEventCallback(callback);
}

