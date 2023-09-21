// <auto-generated> DO NOT MODIFY MANUALLY. Copyright (c) 2023 AlterNET Software.</auto-generated>

#pragma once

#include "CheckBox.h"
#include "ApiUtils.h"
#include "Exceptions.h"

using namespace Alternet::UI;

ALTERNET_UI_API CheckBox* CheckBox_Create_()
{
    return new CheckBox();
}

ALTERNET_UI_API char16_t* CheckBox_GetText_(CheckBox* obj)
{
    return AllocPInvokeReturnString(obj->GetText());
}

ALTERNET_UI_API void CheckBox_SetText_(CheckBox* obj, const char16_t* value)
{
    obj->SetText(value);
}

ALTERNET_UI_API c_bool CheckBox_GetIsChecked_(CheckBox* obj)
{
    return obj->GetIsChecked();
}

ALTERNET_UI_API void CheckBox_SetIsChecked_(CheckBox* obj, c_bool value)
{
    obj->SetIsChecked(value);
}

ALTERNET_UI_API void CheckBox_SetEventCallback_(CheckBox::CheckBoxEventCallbackType callback)
{
    CheckBox::SetEventCallback(callback);
}

