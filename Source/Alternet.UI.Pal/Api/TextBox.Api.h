// Auto generated code, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2021.

#pragma once

#include "TextBox.h"
#include "ApiUtils.h"

using namespace Alternet::UI;

ALTERNET_UI_API TextBox* TextBox_Create()
{
    return new TextBox();
}

ALTERNET_UI_API char16_t* TextBox_GetText(TextBox* obj)
{
    return AllocPInvokeReturnString(obj->GetText());
}

ALTERNET_UI_API void TextBox_SetText(TextBox* obj, const char16_t* value)
{
    obj->SetText(value);
}

ALTERNET_UI_API void TextBox_SetEventCallback(TextBox::TextBoxEventCallbackType callback)
{
    TextBox::SetEventCallback(callback);
}

