// Auto generated code, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2021.

#pragma once

#include "ComboBox.h"
#include "ApiUtils.h"

using namespace Alternet::UI;

ALTERNET_UI_API ComboBox* ComboBox_Create()
{
    return new ComboBox();
}

ALTERNET_UI_API int ComboBox_GetItemsCount(ComboBox* obj)
{
    return obj->GetItemsCount();
}

ALTERNET_UI_API c_bool ComboBox_GetIsEditable(ComboBox* obj)
{
    return obj->GetIsEditable();
}

ALTERNET_UI_API void ComboBox_SetIsEditable(ComboBox* obj, c_bool value)
{
    obj->SetIsEditable(value);
}

ALTERNET_UI_API int ComboBox_GetSelectedIndex(ComboBox* obj)
{
    return obj->GetSelectedIndex();
}

ALTERNET_UI_API void ComboBox_SetSelectedIndex(ComboBox* obj, int value)
{
    obj->SetSelectedIndex(value);
}

ALTERNET_UI_API char16_t* ComboBox_GetText(ComboBox* obj)
{
    return AllocPInvokeReturnString(obj->GetText());
}

ALTERNET_UI_API void ComboBox_SetText(ComboBox* obj, const char16_t* value)
{
    obj->SetText(value);
}

ALTERNET_UI_API void ComboBox_InsertItem(ComboBox* obj, int index, const char16_t* value)
{
    obj->InsertItem(index, value);
}

ALTERNET_UI_API void ComboBox_RemoveItemAt(ComboBox* obj, int index)
{
    obj->RemoveItemAt(index);
}

ALTERNET_UI_API void ComboBox_ClearItems(ComboBox* obj)
{
    obj->ClearItems();
}

ALTERNET_UI_API void ComboBox_SetEventCallback(ComboBox::ComboBoxEventCallbackType callback)
{
    ComboBox::SetEventCallback(callback);
}

