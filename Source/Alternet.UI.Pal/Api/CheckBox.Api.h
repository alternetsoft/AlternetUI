// Auto generated code, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2021.

#pragma once

#include "CheckBox.h"
#include "ApiUtils.h"

using namespace Alternet::UI;

ALTERNET_UI_API CheckBox* CheckBox_Create_()
{
    return new CheckBox();
}

ALTERNET_UI_API char16_t* CheckBox_GetText(CheckBox* obj)
{
    return AllocPInvokeReturnString(obj->GetText());
}

ALTERNET_UI_API void CheckBox_SetText(CheckBox* obj, const char16_t* value)
{
    obj->SetText(value);
}

ALTERNET_UI_API c_bool CheckBox_GetIsChecked(CheckBox* obj)
{
    return obj->GetIsChecked();
}

ALTERNET_UI_API void CheckBox_SetIsChecked(CheckBox* obj, c_bool value)
{
    obj->SetIsChecked(value);
}

ALTERNET_UI_API void CheckBox_SetEventCallback(CheckBox::CheckBoxEventCallbackType callback)
{
    CheckBox::SetEventCallback(callback);
}

