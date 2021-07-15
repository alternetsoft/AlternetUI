// Auto generated code, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2021.

#pragma once

#include "ListView.h"
#include "ApiUtils.h"

using namespace Alternet::UI;

ALTERNET_UI_API ListView* ListView_Create()
{
    return new ListView();
}

ALTERNET_UI_API int ListView_GetItemsCount(ListView* obj)
{
    return obj->GetItemsCount();
}

ALTERNET_UI_API ListViewView ListView_GetCurrentView(ListView* obj)
{
    return obj->GetCurrentView();
}

ALTERNET_UI_API void ListView_SetCurrentView(ListView* obj, ListViewView value)
{
    obj->SetCurrentView(value);
}

ALTERNET_UI_API void ListView_InsertItemAt(ListView* obj, int index, const char16_t* value)
{
    obj->InsertItemAt(index, value);
}

ALTERNET_UI_API void ListView_RemoveItemAt(ListView* obj, int index)
{
    obj->RemoveItemAt(index);
}

ALTERNET_UI_API void ListView_ClearItems(ListView* obj)
{
    obj->ClearItems();
}

