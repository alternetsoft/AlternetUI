// Auto generated code, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2021.

#pragma once

#include "ListView.h"
#include "ImageList.h"
#include "ApiUtils.h"

using namespace Alternet::UI;

ALTERNET_UI_API ListView* ListView_Create_()
{
    return new ListView();
}

ALTERNET_UI_API int ListView_GetItemsCount(ListView* obj)
{
    return obj->GetItemsCount();
}

ALTERNET_UI_API ImageList* ListView_GetSmallImageList(ListView* obj)
{
    return obj->GetSmallImageList();
}

ALTERNET_UI_API void ListView_SetSmallImageList(ListView* obj, ImageList* value)
{
    obj->SetSmallImageList(value);
}

ALTERNET_UI_API ImageList* ListView_GetLargeImageList(ListView* obj)
{
    return obj->GetLargeImageList();
}

ALTERNET_UI_API void ListView_SetLargeImageList(ListView* obj, ImageList* value)
{
    obj->SetLargeImageList(value);
}

ALTERNET_UI_API ListViewView ListView_GetCurrentView(ListView* obj)
{
    return obj->GetCurrentView();
}

ALTERNET_UI_API void ListView_SetCurrentView(ListView* obj, ListViewView value)
{
    obj->SetCurrentView(value);
}

ALTERNET_UI_API void ListView_InsertItemAt(ListView* obj, int index, const char16_t* text, int columnIndex, int imageIndex)
{
    obj->InsertItemAt(index, text, columnIndex, imageIndex);
}

ALTERNET_UI_API void ListView_RemoveItemAt(ListView* obj, int index)
{
    obj->RemoveItemAt(index);
}

ALTERNET_UI_API void ListView_ClearItems(ListView* obj)
{
    obj->ClearItems();
}

ALTERNET_UI_API void ListView_InsertColumnAt(ListView* obj, int index, const char16_t* header)
{
    obj->InsertColumnAt(index, header);
}

ALTERNET_UI_API void ListView_RemoveColumnAt(ListView* obj, int index)
{
    obj->RemoveColumnAt(index);
}

