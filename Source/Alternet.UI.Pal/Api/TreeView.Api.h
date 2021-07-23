// Auto generated code, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2021.

#pragma once

#include "TreeView.h"
#include "ImageList.h"
#include "ApiUtils.h"

using namespace Alternet::UI;

ALTERNET_UI_API TreeView* TreeView_Create_()
{
    return new TreeView();
}

ALTERNET_UI_API ImageList* TreeView_GetImageList(TreeView* obj)
{
    return obj->GetImageList();
}

ALTERNET_UI_API void TreeView_SetImageList(TreeView* obj, ImageList* value)
{
    obj->SetImageList(value);
}

ALTERNET_UI_API void* TreeView_GetRootItem(TreeView* obj)
{
    return obj->GetRootItem();
}

ALTERNET_UI_API TreeViewSelectionMode TreeView_GetSelectionMode(TreeView* obj)
{
    return obj->GetSelectionMode();
}

ALTERNET_UI_API void TreeView_SetSelectionMode(TreeView* obj, TreeViewSelectionMode value)
{
    obj->SetSelectionMode(value);
}

ALTERNET_UI_API void* TreeView_OpenSelectedItemsArray(TreeView* obj)
{
    return obj->OpenSelectedItemsArray();
}

ALTERNET_UI_API int TreeView_GetSelectedItemsItemCount(TreeView* obj, void* array)
{
    return obj->GetSelectedItemsItemCount(array);
}

ALTERNET_UI_API void* TreeView_GetSelectedItemsItemAt(TreeView* obj, void* array, int index)
{
    return obj->GetSelectedItemsItemAt(array, index);
}

ALTERNET_UI_API void TreeView_CloseSelectedItemsArray(TreeView* obj, void* array)
{
    obj->CloseSelectedItemsArray(array);
}

ALTERNET_UI_API int TreeView_GetItemCount(TreeView* obj, void* parentItem)
{
    return obj->GetItemCount(parentItem);
}

ALTERNET_UI_API void TreeView_InsertItemAt(TreeView* obj, void* parentItem, int index, const char16_t* text, int imageIndex)
{
    obj->InsertItemAt(parentItem, index, text, imageIndex);
}

ALTERNET_UI_API void TreeView_RemoveItem(TreeView* obj, void* item)
{
    obj->RemoveItem(item);
}

ALTERNET_UI_API void TreeView_ClearItems(TreeView* obj, void* parentItem)
{
    obj->ClearItems(parentItem);
}

ALTERNET_UI_API void TreeView_ClearSelected(TreeView* obj)
{
    obj->ClearSelected();
}

ALTERNET_UI_API void TreeView_SetSelected(TreeView* obj, void* item, c_bool value)
{
    obj->SetSelected(item, value);
}

ALTERNET_UI_API void TreeView_SetEventCallback(TreeView::TreeViewEventCallbackType callback)
{
    TreeView::SetEventCallback(callback);
}

