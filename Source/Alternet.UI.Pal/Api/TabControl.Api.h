// Auto generated code, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2021.

#pragma once

#include "TabControl.h"
#include "Control.h"
#include "ApiUtils.h"

using namespace Alternet::UI;

ALTERNET_UI_API TabControl* TabControl_Create_()
{
    return new TabControl();
}

ALTERNET_UI_API int TabControl_GetPageCount(TabControl* obj)
{
    return obj->GetPageCount();
}

ALTERNET_UI_API void TabControl_InsertPage(TabControl* obj, int index, Control* page, const char16_t* title)
{
    obj->InsertPage(index, page, title);
}

ALTERNET_UI_API void TabControl_RemovePage(TabControl* obj, int index, Control* page)
{
    obj->RemovePage(index, page);
}

ALTERNET_UI_API SizeF_C TabControl_GetTotalPreferredSizeFromPageSize(TabControl* obj, SizeF pageSize)
{
    return obj->GetTotalPreferredSizeFromPageSize(pageSize);
}

