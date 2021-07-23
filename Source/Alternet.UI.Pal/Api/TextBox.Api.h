// Auto generated code, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2021.

#pragma once

#include "TextBox.h"
#include "ApiUtils.h"

using namespace Alternet::UI;

ALTERNET_UI_API TextBox* TextBox_Create_()
{
    return new TextBox();
}

ALTERNET_UI_API char16_t* TextBox_GetText_(TextBox* obj)
{
    return AllocPInvokeReturnString(obj->GetText());
}

ALTERNET_UI_API void TextBox_SetText_(TextBox* obj, const char16_t* value)
{
    obj->SetText(value);
}

ALTERNET_UI_API c_bool TextBox_GetEditControlOnly_(TextBox* obj)
{
    return obj->GetEditControlOnly();
}

ALTERNET_UI_API void TextBox_SetEditControlOnly_(TextBox* obj, c_bool value)
{
    obj->SetEditControlOnly(value);
}

ALTERNET_UI_API void TextBox_SetEventCallback_(TextBox::TextBoxEventCallbackType callback)
{
    TextBox::SetEventCallback(callback);
}

