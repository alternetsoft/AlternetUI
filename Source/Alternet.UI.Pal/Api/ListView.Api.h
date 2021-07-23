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

ALTERNET_UI_API int ListView_GetItemsCount_(ListView* obj)
{
    return obj->GetItemsCount();
}

ALTERNET_UI_API ImageList* ListView_GetSmallImageList_(ListView* obj)
{
    return obj->GetSmallImageList();
}

ALTERNET_UI_API void ListView_SetSmallImageList_(ListView* obj, ImageList* value)
{
    obj->SetSmallImageList(value);
}

ALTERNET_UI_API ImageList* ListView_GetLargeImageList_(ListView* obj)
{
    return obj->GetLargeImageList();
}

ALTERNET_UI_API void ListView_SetLargeImageList_(ListView* obj, ImageList* value)
{
    obj->SetLargeImageList(value);
}

ALTERNET_UI_API ListViewView ListView_GetCurrentView_(ListView* obj)
{
    return obj->GetCurrentView();
}

ALTERNET_UI_API void ListView_SetCurrentView_(ListView* obj, ListViewView value)
{
    obj->SetCurrentView(value);
}

ALTERNET_UI_API ListViewSelectionMode ListView_GetSelectionMode_(ListView* obj)
{
    return obj->GetSelectionMode();
}

ALTERNET_UI_API void ListView_SetSelectionMode_(ListView* obj, ListViewSelectionMode value)
{
    obj->SetSelectionMode(value);
}

ALTERNET_UI_API void* ListView_OpenSelectedIndicesArray_(ListView* obj)
{
    return obj->OpenSelectedIndicesArray();
}

ALTERNET_UI_API int ListView_GetSelectedIndicesItemCount_(ListView* obj, void* array)
{
    return obj->GetSelectedIndicesItemCount(array);
}

ALTERNET_UI_API int ListView_GetSelectedIndicesItemAt_(ListView* obj, void* array, int index)
{
    return obj->GetSelectedIndicesItemAt(array, index);
}

ALTERNET_UI_API void ListView_CloseSelectedIndicesArray_(ListView* obj, void* array)
{
    obj->CloseSelectedIndicesArray(array);
}

ALTERNET_UI_API void ListView_InsertItemAt_(ListView* obj, int index, const char16_t* text, int columnIndex, int imageIndex)
{
    obj->InsertItemAt(index, text, columnIndex, imageIndex);
}

ALTERNET_UI_API void ListView_RemoveItemAt_(ListView* obj, int index)
{
    obj->RemoveItemAt(index);
}

ALTERNET_UI_API void ListView_ClearItems_(ListView* obj)
{
    obj->ClearItems();
}

ALTERNET_UI_API void ListView_InsertColumnAt_(ListView* obj, int index, const char16_t* header)
{
    obj->InsertColumnAt(index, header);
}

ALTERNET_UI_API void ListView_RemoveColumnAt_(ListView* obj, int index)
{
    obj->RemoveColumnAt(index);
}

ALTERNET_UI_API void ListView_ClearSelected_(ListView* obj)
{
    obj->ClearSelected();
}

ALTERNET_UI_API void ListView_SetSelected_(ListView* obj, int index, c_bool value)
{
    obj->SetSelected(index, value);
}

ALTERNET_UI_API void ListView_SetEventCallback_(ListView::ListViewEventCallbackType callback)
{
    ListView::SetEventCallback(callback);
}

