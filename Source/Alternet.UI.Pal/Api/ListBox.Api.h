// <auto-generated> DO NOT MODIFY MANUALLY. Copyright (c) 2025 AlterNET Software.</auto-generated>

#pragma once

#include "ListBox.h"
#include "ApiUtils.h"
#include "Exceptions.h"

using namespace Alternet::UI;

ALTERNET_UI_API ListBox* ListBox_Create_()
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<ListBox*>([&](){
    #endif
        return new ListBox();
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API c_bool ListBox_GetHasBorder_(ListBox* obj)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<c_bool>([&](){
    #endif
        return obj->GetHasBorder();
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API void ListBox_SetHasBorder_(ListBox* obj, c_bool value)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    MarshalExceptions<void>([&](){
    #endif
        obj->SetHasBorder(value);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API int ListBox_GetItemsCount_(ListBox* obj)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<int>([&](){
    #endif
        return obj->GetItemsCount();
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API ListBoxSelectionMode ListBox_GetSelectionMode_(ListBox* obj)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<ListBoxSelectionMode>([&](){
    #endif
        return obj->GetSelectionMode();
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API void ListBox_SetSelectionMode_(ListBox* obj, ListBoxSelectionMode value)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    MarshalExceptions<void>([&](){
    #endif
        obj->SetSelectionMode(value);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API void* ListBox_OpenSelectedIndicesArray_(ListBox* obj)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<void*>([&](){
    #endif
        return obj->OpenSelectedIndicesArray();
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API int ListBox_GetSelectedIndicesItemCount_(ListBox* obj, void* array)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<int>([&](){
    #endif
        return obj->GetSelectedIndicesItemCount(array);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API int ListBox_GetSelectedIndicesItemAt_(ListBox* obj, void* array, int index)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<int>([&](){
    #endif
        return obj->GetSelectedIndicesItemAt(array, index);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API void ListBox_CloseSelectedIndicesArray_(ListBox* obj, void* array)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    MarshalExceptions<void>([&](){
    #endif
        obj->CloseSelectedIndicesArray(array);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API void* ListBox_CreateEx_(int64_t styles)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<void*>([&](){
    #endif
        return ListBox::CreateEx(styles);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API void ListBox_InsertItem_(ListBox* obj, int index, const char16_t* value)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    MarshalExceptions<void>([&](){
    #endif
        obj->InsertItem(index, value);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API void ListBox_RemoveItemAt_(ListBox* obj, int index)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    MarshalExceptions<void>([&](){
    #endif
        obj->RemoveItemAt(index);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API void ListBox_ClearItems_(ListBox* obj)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    MarshalExceptions<void>([&](){
    #endif
        obj->ClearItems();
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API void ListBox_ClearSelected_(ListBox* obj)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    MarshalExceptions<void>([&](){
    #endif
        obj->ClearSelected();
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API void ListBox_SetSelected_(ListBox* obj, int index, c_bool value)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    MarshalExceptions<void>([&](){
    #endif
        obj->SetSelected(index, value);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API void ListBox_EnsureVisible_(ListBox* obj, int itemIndex)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    MarshalExceptions<void>([&](){
    #endif
        obj->EnsureVisible(itemIndex);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API int ListBox_ItemHitTest_(ListBox* obj, PointD position)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<int>([&](){
    #endif
        return obj->ItemHitTest(position);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API void ListBox_SetItem_(ListBox* obj, int index, const char16_t* value)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    MarshalExceptions<void>([&](){
    #endif
        obj->SetItem(index, value);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API void ListBox_SetEventCallback_(ListBox::ListBoxEventCallbackType callback)
{
    ListBox::SetEventCallback(callback);
}

