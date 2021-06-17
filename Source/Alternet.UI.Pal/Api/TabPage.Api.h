// Auto generated code, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2021.

#pragma once

#include "TabPage.h"
#include "ApiUtils.h"

using namespace Alternet::UI;

ALTERNET_UI_API TabPage* TabPage_Create()
{
    return new TabPage();
}

ALTERNET_UI_API char16_t* TabPage_GetTitle(TabPage* obj)
{
    return AllocPInvokeReturnString(obj->GetTitle());
}

ALTERNET_UI_API void TabPage_SetTitle(TabPage* obj, const char16_t* value)
{
    obj->SetTitle(value);
}

