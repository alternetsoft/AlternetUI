// Auto generated code, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2021.

#pragma once

#include "RadioButton.h"
#include "ApiUtils.h"

using namespace Alternet::UI;

ALTERNET_UI_API RadioButton* RadioButton_Create_()
{
    return new RadioButton();
}

ALTERNET_UI_API char16_t* RadioButton_GetText_(RadioButton* obj)
{
    return AllocPInvokeReturnString(obj->GetText());
}

ALTERNET_UI_API void RadioButton_SetText_(RadioButton* obj, const char16_t* value)
{
    obj->SetText(value);
}

ALTERNET_UI_API c_bool RadioButton_GetIsChecked_(RadioButton* obj)
{
    return obj->GetIsChecked();
}

ALTERNET_UI_API void RadioButton_SetIsChecked_(RadioButton* obj, c_bool value)
{
    obj->SetIsChecked(value);
}

ALTERNET_UI_API void RadioButton_SetEventCallback_(RadioButton::RadioButtonEventCallbackType callback)
{
    RadioButton::SetEventCallback(callback);
}

