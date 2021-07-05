// Auto generated code, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2021.

#pragma once

#include "ListBox.h"
#include "ApiUtils.h"

using namespace Alternet::UI;

ALTERNET_UI_API ListBox* ListBox_Create()
{
    return new ListBox();
}

ALTERNET_UI_API int ListBox_GetItemsCount(ListBox* obj)
{
    return obj->GetItemsCount();
}

ALTERNET_UI_API ListBoxSelectionMode ListBox_GetSelectionMode(ListBox* obj)
{
    return obj->GetSelectionMode();
}

ALTERNET_UI_API void ListBox_SetSelectionMode(ListBox* obj, ListBoxSelectionMode value)
{
    obj->SetSelectionMode(value);
}

ALTERNET_UI_API int ListBox_GetSelectedIndicesItemCount(ListBox* obj)
{
    return obj->GetSelectedIndicesItemCount();
}

ALTERNET_UI_API int ListBox_GetSelectedIndicesItemAt(ListBox* obj, int index)
{
    return obj->GetSelectedIndicesItemAt(index);
}

ALTERNET_UI_API void ListBox_InsertItem(ListBox* obj, int index, const char16_t* value)
{
    obj->InsertItem(index, value);
}

ALTERNET_UI_API void ListBox_RemoveItemAt(ListBox* obj, int index)
{
    obj->RemoveItemAt(index);
}

ALTERNET_UI_API void ListBox_ClearItems(ListBox* obj)
{
    obj->ClearItems();
}

ALTERNET_UI_API void ListBox_ClearSelected(ListBox* obj)
{
    obj->ClearSelected();
}

ALTERNET_UI_API void ListBox_SetSelected(ListBox* obj, int index, c_bool value)
{
    obj->SetSelected(index, value);
}

ALTERNET_UI_API void ListBox_SetEventCallback(ListBox::ListBoxEventCallbackType callback)
{
    ListBox::SetEventCallback(callback);
}

