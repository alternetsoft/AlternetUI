// Auto generated code, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2021.

#pragma once

#include "GroupBox.h"
#include "ApiUtils.h"

using namespace Alternet::UI;

ALTERNET_UI_API GroupBox* GroupBox_Create()
{
    return new GroupBox();
}

ALTERNET_UI_API char16_t* GroupBox_GetTitle(GroupBox* obj)
{
    return AllocPInvokeReturnString(obj->GetTitle());
}

ALTERNET_UI_API void GroupBox_SetTitle(GroupBox* obj, const char16_t* value)
{
    obj->SetTitle(ToOptional(value));
}

