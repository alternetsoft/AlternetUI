// <auto-generated>This code was generated by a tool, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2023.</auto-generated>

#pragma once

#include "TreeView.h"
#include "ImageList.h"
#include "ApiUtils.h"
#include "Exceptions.h"

using namespace Alternet::UI;

ALTERNET_UI_API TreeView* TreeView_Create_()
{
    return MarshalExceptions<TreeView*>([&](){
            return new TreeView();
        });
}

ALTERNET_UI_API c_bool TreeView_GetHasBorder_(TreeView* obj)
{
    return MarshalExceptions<c_bool>([&](){
            return obj->GetHasBorder();
        });
}

ALTERNET_UI_API void TreeView_SetHasBorder_(TreeView* obj, c_bool value)
{
    MarshalExceptions<void>([&](){
            obj->SetHasBorder(value);
        });
}

ALTERNET_UI_API ImageList* TreeView_GetImageList_(TreeView* obj)
{
    return MarshalExceptions<ImageList*>([&](){
            return obj->GetImageList();
        });
}

ALTERNET_UI_API void TreeView_SetImageList_(TreeView* obj, ImageList* value)
{
    MarshalExceptions<void>([&](){
            obj->SetImageList(value);
        });
}

ALTERNET_UI_API void* TreeView_GetRootItem_(TreeView* obj)
{
    return MarshalExceptions<void*>([&](){
            return obj->GetRootItem();
        });
}

ALTERNET_UI_API TreeViewSelectionMode TreeView_GetSelectionMode_(TreeView* obj)
{
    return MarshalExceptions<TreeViewSelectionMode>([&](){
            return obj->GetSelectionMode();
        });
}

ALTERNET_UI_API void TreeView_SetSelectionMode_(TreeView* obj, TreeViewSelectionMode value)
{
    MarshalExceptions<void>([&](){
            obj->SetSelectionMode(value);
        });
}

ALTERNET_UI_API c_bool TreeView_GetShowLines_(TreeView* obj)
{
    return MarshalExceptions<c_bool>([&](){
            return obj->GetShowLines();
        });
}

ALTERNET_UI_API void TreeView_SetShowLines_(TreeView* obj, c_bool value)
{
    MarshalExceptions<void>([&](){
            obj->SetShowLines(value);
        });
}

ALTERNET_UI_API c_bool TreeView_GetShowRootLines_(TreeView* obj)
{
    return MarshalExceptions<c_bool>([&](){
            return obj->GetShowRootLines();
        });
}

ALTERNET_UI_API void TreeView_SetShowRootLines_(TreeView* obj, c_bool value)
{
    MarshalExceptions<void>([&](){
            obj->SetShowRootLines(value);
        });
}

ALTERNET_UI_API c_bool TreeView_GetShowExpandButtons_(TreeView* obj)
{
    return MarshalExceptions<c_bool>([&](){
            return obj->GetShowExpandButtons();
        });
}

ALTERNET_UI_API void TreeView_SetShowExpandButtons_(TreeView* obj, c_bool value)
{
    MarshalExceptions<void>([&](){
            obj->SetShowExpandButtons(value);
        });
}

ALTERNET_UI_API void* TreeView_GetTopItem_(TreeView* obj)
{
    return MarshalExceptions<void*>([&](){
            return obj->GetTopItem();
        });
}

ALTERNET_UI_API c_bool TreeView_GetFullRowSelect_(TreeView* obj)
{
    return MarshalExceptions<c_bool>([&](){
            return obj->GetFullRowSelect();
        });
}

ALTERNET_UI_API void TreeView_SetFullRowSelect_(TreeView* obj, c_bool value)
{
    MarshalExceptions<void>([&](){
            obj->SetFullRowSelect(value);
        });
}

ALTERNET_UI_API c_bool TreeView_GetAllowLabelEdit_(TreeView* obj)
{
    return MarshalExceptions<c_bool>([&](){
            return obj->GetAllowLabelEdit();
        });
}

ALTERNET_UI_API void TreeView_SetAllowLabelEdit_(TreeView* obj, c_bool value)
{
    MarshalExceptions<void>([&](){
            obj->SetAllowLabelEdit(value);
        });
}

ALTERNET_UI_API void* TreeView_OpenSelectedItemsArray_(TreeView* obj)
{
    return MarshalExceptions<void*>([&](){
            return obj->OpenSelectedItemsArray();
        });
}

ALTERNET_UI_API int TreeView_GetSelectedItemsItemCount_(TreeView* obj, void* array)
{
    return MarshalExceptions<int>([&](){
            return obj->GetSelectedItemsItemCount(array);
        });
}

ALTERNET_UI_API void* TreeView_GetSelectedItemsItemAt_(TreeView* obj, void* array, int index)
{
    return MarshalExceptions<void*>([&](){
            return obj->GetSelectedItemsItemAt(array, index);
        });
}

ALTERNET_UI_API void TreeView_CloseSelectedItemsArray_(TreeView* obj, void* array)
{
    MarshalExceptions<void>([&](){
            obj->CloseSelectedItemsArray(array);
        });
}

ALTERNET_UI_API int TreeView_GetItemCount_(TreeView* obj, void* parentItem)
{
    return MarshalExceptions<int>([&](){
            return obj->GetItemCount(parentItem);
        });
}

ALTERNET_UI_API void* TreeView_InsertItem_(TreeView* obj, void* parentItem, void* insertAfter, const char16_t* text, int imageIndex, c_bool parentIsExpanded)
{
    return MarshalExceptions<void*>([&](){
            return obj->InsertItem(parentItem, insertAfter, text, imageIndex, parentIsExpanded);
        });
}

ALTERNET_UI_API void TreeView_RemoveItem_(TreeView* obj, void* item)
{
    MarshalExceptions<void>([&](){
            obj->RemoveItem(item);
        });
}

ALTERNET_UI_API void TreeView_ClearItems_(TreeView* obj, void* parentItem)
{
    MarshalExceptions<void>([&](){
            obj->ClearItems(parentItem);
        });
}

ALTERNET_UI_API void TreeView_ClearSelected_(TreeView* obj)
{
    MarshalExceptions<void>([&](){
            obj->ClearSelected();
        });
}

ALTERNET_UI_API void TreeView_SetSelected_(TreeView* obj, void* item, c_bool value)
{
    MarshalExceptions<void>([&](){
            obj->SetSelected(item, value);
        });
}

ALTERNET_UI_API void TreeView_ExpandAll_(TreeView* obj)
{
    MarshalExceptions<void>([&](){
            obj->ExpandAll();
        });
}

ALTERNET_UI_API void TreeView_CollapseAll_(TreeView* obj)
{
    MarshalExceptions<void>([&](){
            obj->CollapseAll();
        });
}

ALTERNET_UI_API void* TreeView_ItemHitTest_(TreeView* obj, Point point)
{
    return MarshalExceptions<void*>([&](){
            return obj->ItemHitTest(point);
        });
}

ALTERNET_UI_API TreeViewHitTestLocations TreeView_GetHitTestResultLocations_(TreeView* obj, void* hitTestResult)
{
    return MarshalExceptions<TreeViewHitTestLocations>([&](){
            return obj->GetHitTestResultLocations(hitTestResult);
        });
}

ALTERNET_UI_API void* TreeView_GetHitTestResultItem_(TreeView* obj, void* hitTestResult)
{
    return MarshalExceptions<void*>([&](){
            return obj->GetHitTestResultItem(hitTestResult);
        });
}

ALTERNET_UI_API void TreeView_FreeHitTestResult_(TreeView* obj, void* hitTestResult)
{
    MarshalExceptions<void>([&](){
            obj->FreeHitTestResult(hitTestResult);
        });
}

ALTERNET_UI_API c_bool TreeView_IsItemSelected_(TreeView* obj, void* item)
{
    return MarshalExceptions<c_bool>([&](){
            return obj->IsItemSelected(item);
        });
}

ALTERNET_UI_API void TreeView_SetFocused_(TreeView* obj, void* item, c_bool value)
{
    MarshalExceptions<void>([&](){
            obj->SetFocused(item, value);
        });
}

ALTERNET_UI_API c_bool TreeView_IsItemFocused_(TreeView* obj, void* item)
{
    return MarshalExceptions<c_bool>([&](){
            return obj->IsItemFocused(item);
        });
}

ALTERNET_UI_API void TreeView_SetItemText_(TreeView* obj, void* item, const char16_t* text)
{
    MarshalExceptions<void>([&](){
            obj->SetItemText(item, text);
        });
}

ALTERNET_UI_API char16_t* TreeView_GetItemText_(TreeView* obj, void* item)
{
    return MarshalExceptions<char16_t*>([&](){
            return AllocPInvokeReturnString(obj->GetItemText(item));
        });
}

ALTERNET_UI_API void TreeView_SetItemImageIndex_(TreeView* obj, void* item, int imageIndex)
{
    MarshalExceptions<void>([&](){
            obj->SetItemImageIndex(item, imageIndex);
        });
}

ALTERNET_UI_API int TreeView_GetItemImageIndex_(TreeView* obj, void* item)
{
    return MarshalExceptions<int>([&](){
            return obj->GetItemImageIndex(item);
        });
}

ALTERNET_UI_API void TreeView_BeginLabelEdit_(TreeView* obj, void* item)
{
    MarshalExceptions<void>([&](){
            obj->BeginLabelEdit(item);
        });
}

ALTERNET_UI_API void TreeView_EndLabelEdit_(TreeView* obj, void* item, c_bool cancel)
{
    MarshalExceptions<void>([&](){
            obj->EndLabelEdit(item, cancel);
        });
}

ALTERNET_UI_API void TreeView_ExpandAllChildren_(TreeView* obj, void* item)
{
    MarshalExceptions<void>([&](){
            obj->ExpandAllChildren(item);
        });
}

ALTERNET_UI_API void TreeView_CollapseAllChildren_(TreeView* obj, void* item)
{
    MarshalExceptions<void>([&](){
            obj->CollapseAllChildren(item);
        });
}

ALTERNET_UI_API void TreeView_EnsureVisible_(TreeView* obj, void* item)
{
    MarshalExceptions<void>([&](){
            obj->EnsureVisible(item);
        });
}

ALTERNET_UI_API void TreeView_ScrollIntoView_(TreeView* obj, void* item)
{
    MarshalExceptions<void>([&](){
            obj->ScrollIntoView(item);
        });
}

ALTERNET_UI_API void TreeView_SetEventCallback_(TreeView::TreeViewEventCallbackType callback)
{
    TreeView::SetEventCallback(callback);
}

