// <auto-generated> DO NOT MODIFY MANUALLY. Copyright (c) 2023 AlterNET Software.</auto-generated>

#pragma once

#include "CheckListBox.h"
#include "ApiUtils.h"
#include "Exceptions.h"

using namespace Alternet::UI;

ALTERNET_UI_API CheckListBox* CheckListBox_Create_()
{
    return new CheckListBox();
}

ALTERNET_UI_API void* CheckListBox_OpenCheckedIndicesArray_(CheckListBox* obj)
{
    return obj->OpenCheckedIndicesArray();
}

ALTERNET_UI_API int CheckListBox_GetCheckedIndicesItemCount_(CheckListBox* obj, void* array)
{
    return obj->GetCheckedIndicesItemCount(array);
}

ALTERNET_UI_API int CheckListBox_GetCheckedIndicesItemAt_(CheckListBox* obj, void* array, int index)
{
    return obj->GetCheckedIndicesItemAt(array, index);
}

ALTERNET_UI_API void CheckListBox_CloseCheckedIndicesArray_(CheckListBox* obj, void* array)
{
    obj->CloseCheckedIndicesArray(array);
}

ALTERNET_UI_API void CheckListBox_ClearChecked_(CheckListBox* obj)
{
    obj->ClearChecked();
}

ALTERNET_UI_API void CheckListBox_SetChecked_(CheckListBox* obj, int index, c_bool value)
{
    obj->SetChecked(index, value);
}

ALTERNET_UI_API c_bool CheckListBox_IsChecked_(CheckListBox* obj, int item)
{
    return obj->IsChecked(item);
}

ALTERNET_UI_API void CheckListBox_SetEventCallback_(CheckListBox::CheckListBoxEventCallbackType callback)
{
    CheckListBox::SetEventCallback(callback);
}

