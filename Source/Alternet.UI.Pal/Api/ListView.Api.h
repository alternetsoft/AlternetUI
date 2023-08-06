// <auto-generated>This code was generated by a tool, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2023.</auto-generated>

#pragma once

#include "ListView.h"
#include "ImageList.h"
#include "ApiUtils.h"
#include "Exceptions.h"

using namespace Alternet::UI;

ALTERNET_UI_API ListView* ListView_Create_()
{
    return MarshalExceptions<ListView*>([&](){
            return new ListView();
        });
}

ALTERNET_UI_API c_bool ListView_GetHasBorder_(ListView* obj)
{
    return MarshalExceptions<c_bool>([&](){
            return obj->GetHasBorder();
        });
}

ALTERNET_UI_API void ListView_SetHasBorder_(ListView* obj, c_bool value)
{
    MarshalExceptions<void>([&](){
            obj->SetHasBorder(value);
        });
}

ALTERNET_UI_API int64_t ListView_GetItemsCount_(ListView* obj)
{
    return MarshalExceptions<int64_t>([&](){
            return obj->GetItemsCount();
        });
}

ALTERNET_UI_API ImageList* ListView_GetSmallImageList_(ListView* obj)
{
    return MarshalExceptions<ImageList*>([&](){
            return obj->GetSmallImageList();
        });
}

ALTERNET_UI_API void ListView_SetSmallImageList_(ListView* obj, ImageList* value)
{
    MarshalExceptions<void>([&](){
            obj->SetSmallImageList(value);
        });
}

ALTERNET_UI_API ImageList* ListView_GetLargeImageList_(ListView* obj)
{
    return MarshalExceptions<ImageList*>([&](){
            return obj->GetLargeImageList();
        });
}

ALTERNET_UI_API void ListView_SetLargeImageList_(ListView* obj, ImageList* value)
{
    MarshalExceptions<void>([&](){
            obj->SetLargeImageList(value);
        });
}

ALTERNET_UI_API ListViewView ListView_GetCurrentView_(ListView* obj)
{
    return MarshalExceptions<ListViewView>([&](){
            return obj->GetCurrentView();
        });
}

ALTERNET_UI_API void ListView_SetCurrentView_(ListView* obj, ListViewView value)
{
    MarshalExceptions<void>([&](){
            obj->SetCurrentView(value);
        });
}

ALTERNET_UI_API ListViewSelectionMode ListView_GetSelectionMode_(ListView* obj)
{
    return MarshalExceptions<ListViewSelectionMode>([&](){
            return obj->GetSelectionMode();
        });
}

ALTERNET_UI_API void ListView_SetSelectionMode_(ListView* obj, ListViewSelectionMode value)
{
    MarshalExceptions<void>([&](){
            obj->SetSelectionMode(value);
        });
}

ALTERNET_UI_API c_bool ListView_GetAllowLabelEdit_(ListView* obj)
{
    return MarshalExceptions<c_bool>([&](){
            return obj->GetAllowLabelEdit();
        });
}

ALTERNET_UI_API void ListView_SetAllowLabelEdit_(ListView* obj, c_bool value)
{
    MarshalExceptions<void>([&](){
            obj->SetAllowLabelEdit(value);
        });
}

ALTERNET_UI_API int64_t ListView_GetTopItemIndex_(ListView* obj)
{
    return MarshalExceptions<int64_t>([&](){
            return obj->GetTopItemIndex();
        });
}

ALTERNET_UI_API ListViewGridLinesDisplayMode ListView_GetGridLinesDisplayMode_(ListView* obj)
{
    return MarshalExceptions<ListViewGridLinesDisplayMode>([&](){
            return obj->GetGridLinesDisplayMode();
        });
}

ALTERNET_UI_API void ListView_SetGridLinesDisplayMode_(ListView* obj, ListViewGridLinesDisplayMode value)
{
    MarshalExceptions<void>([&](){
            obj->SetGridLinesDisplayMode(value);
        });
}

ALTERNET_UI_API ListViewSortMode ListView_GetSortMode_(ListView* obj)
{
    return MarshalExceptions<ListViewSortMode>([&](){
            return obj->GetSortMode();
        });
}

ALTERNET_UI_API void ListView_SetSortMode_(ListView* obj, ListViewSortMode value)
{
    MarshalExceptions<void>([&](){
            obj->SetSortMode(value);
        });
}

ALTERNET_UI_API c_bool ListView_GetColumnHeaderVisible_(ListView* obj)
{
    return MarshalExceptions<c_bool>([&](){
            return obj->GetColumnHeaderVisible();
        });
}

ALTERNET_UI_API void ListView_SetColumnHeaderVisible_(ListView* obj, c_bool value)
{
    MarshalExceptions<void>([&](){
            obj->SetColumnHeaderVisible(value);
        });
}

ALTERNET_UI_API int64_t ListView_GetFocusedItemIndex_(ListView* obj)
{
    return MarshalExceptions<int64_t>([&](){
            return obj->GetFocusedItemIndex();
        });
}

ALTERNET_UI_API void ListView_SetFocusedItemIndex_(ListView* obj, int64_t value)
{
    MarshalExceptions<void>([&](){
            obj->SetFocusedItemIndex(value);
        });
}

ALTERNET_UI_API void* ListView_OpenSelectedIndicesArray_(ListView* obj)
{
    return MarshalExceptions<void*>([&](){
            return obj->OpenSelectedIndicesArray();
        });
}

ALTERNET_UI_API int ListView_GetSelectedIndicesItemCount_(ListView* obj, void* array)
{
    return MarshalExceptions<int>([&](){
            return obj->GetSelectedIndicesItemCount(array);
        });
}

ALTERNET_UI_API int64_t ListView_GetSelectedIndicesItemAt_(ListView* obj, void* array, int index)
{
    return MarshalExceptions<int64_t>([&](){
            return obj->GetSelectedIndicesItemAt(array, index);
        });
}

ALTERNET_UI_API void ListView_CloseSelectedIndicesArray_(ListView* obj, void* array)
{
    MarshalExceptions<void>([&](){
            obj->CloseSelectedIndicesArray(array);
        });
}

ALTERNET_UI_API void ListView_InsertItemAt_(ListView* obj, int64_t index, const char16_t* text, int64_t columnIndex, int imageIndex)
{
    MarshalExceptions<void>([&](){
            obj->InsertItemAt(index, text, columnIndex, imageIndex);
        });
}

ALTERNET_UI_API void ListView_RemoveItemAt_(ListView* obj, int64_t index)
{
    MarshalExceptions<void>([&](){
            obj->RemoveItemAt(index);
        });
}

ALTERNET_UI_API void ListView_ClearItems_(ListView* obj)
{
    MarshalExceptions<void>([&](){
            obj->ClearItems();
        });
}

ALTERNET_UI_API void ListView_InsertColumnAt_(ListView* obj, int64_t index, const char16_t* header, double width, ListViewColumnWidthMode widthMode)
{
    MarshalExceptions<void>([&](){
            obj->InsertColumnAt(index, header, width, widthMode);
        });
}

ALTERNET_UI_API void ListView_RemoveColumnAt_(ListView* obj, int64_t index)
{
    MarshalExceptions<void>([&](){
            obj->RemoveColumnAt(index);
        });
}

ALTERNET_UI_API void ListView_ClearSelected_(ListView* obj)
{
    MarshalExceptions<void>([&](){
            obj->ClearSelected();
        });
}

ALTERNET_UI_API void ListView_SetSelected_(ListView* obj, int64_t index, c_bool value)
{
    MarshalExceptions<void>([&](){
            obj->SetSelected(index, value);
        });
}

ALTERNET_UI_API void* ListView_ItemHitTest_(ListView* obj, Point point)
{
    return MarshalExceptions<void*>([&](){
            return obj->ItemHitTest(point);
        });
}

ALTERNET_UI_API ListViewHitTestLocations ListView_GetHitTestResultLocations_(ListView* obj, void* hitTestResult)
{
    return MarshalExceptions<ListViewHitTestLocations>([&](){
            return obj->GetHitTestResultLocations(hitTestResult);
        });
}

ALTERNET_UI_API int64_t ListView_GetHitTestResultItemIndex_(ListView* obj, void* hitTestResult)
{
    return MarshalExceptions<int64_t>([&](){
            return obj->GetHitTestResultItemIndex(hitTestResult);
        });
}

ALTERNET_UI_API int64_t ListView_GetHitTestResultColumnIndex_(ListView* obj, void* hitTestResult)
{
    return MarshalExceptions<int64_t>([&](){
            return obj->GetHitTestResultColumnIndex(hitTestResult);
        });
}

ALTERNET_UI_API void ListView_FreeHitTestResult_(ListView* obj, void* hitTestResult)
{
    MarshalExceptions<void>([&](){
            obj->FreeHitTestResult(hitTestResult);
        });
}

ALTERNET_UI_API void ListView_BeginLabelEdit_(ListView* obj, int64_t itemIndex)
{
    MarshalExceptions<void>([&](){
            obj->BeginLabelEdit(itemIndex);
        });
}

ALTERNET_UI_API Rect_C ListView_GetItemBounds_(ListView* obj, int64_t itemIndex, ListViewItemBoundsPortion portion)
{
    return MarshalExceptions<Rect_C>([&](){
            return obj->GetItemBounds(itemIndex, portion);
        });
}

ALTERNET_UI_API void ListView_Clear_(ListView* obj)
{
    MarshalExceptions<void>([&](){
            obj->Clear();
        });
}

ALTERNET_UI_API void ListView_EnsureItemVisible_(ListView* obj, int64_t itemIndex)
{
    MarshalExceptions<void>([&](){
            obj->EnsureItemVisible(itemIndex);
        });
}

ALTERNET_UI_API void ListView_SetItemText_(ListView* obj, int64_t itemIndex, int64_t columnIndex, const char16_t* text)
{
    MarshalExceptions<void>([&](){
            obj->SetItemText(itemIndex, columnIndex, text);
        });
}

ALTERNET_UI_API void ListView_SetItemImageIndex_(ListView* obj, int64_t itemIndex, int64_t columnIndex, int imageIndex)
{
    MarshalExceptions<void>([&](){
            obj->SetItemImageIndex(itemIndex, columnIndex, imageIndex);
        });
}

ALTERNET_UI_API void ListView_SetColumnWidth_(ListView* obj, int64_t columnIndex, double fixedWidth, ListViewColumnWidthMode widthMode)
{
    MarshalExceptions<void>([&](){
            obj->SetColumnWidth(columnIndex, fixedWidth, widthMode);
        });
}

ALTERNET_UI_API void ListView_SetColumnTitle_(ListView* obj, int64_t columnIndex, const char16_t* text)
{
    MarshalExceptions<void>([&](){
            obj->SetColumnTitle(columnIndex, text);
        });
}

ALTERNET_UI_API void ListView_SetEventCallback_(ListView::ListViewEventCallbackType callback)
{
    ListView::SetEventCallback(callback);
}

