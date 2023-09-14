// <auto-generated>This code was generated by a tool, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2023.</auto-generated>

#pragma once

#include "TabControl.h"
#include "Control.h"
#include "ApiUtils.h"
#include "Exceptions.h"

using namespace Alternet::UI;

ALTERNET_UI_API TabControl* TabControl_Create_()
{
    return new TabControl();
}

ALTERNET_UI_API int TabControl_GetPageCount_(TabControl* obj)
{
    return obj->GetPageCount();
}

ALTERNET_UI_API int TabControl_GetSelectedPageIndex_(TabControl* obj)
{
    return obj->GetSelectedPageIndex();
}

ALTERNET_UI_API void TabControl_SetSelectedPageIndex_(TabControl* obj, int value)
{
    obj->SetSelectedPageIndex(value);
}

ALTERNET_UI_API TabAlignment TabControl_GetTabAlignment_(TabControl* obj)
{
    return obj->GetTabAlignment();
}

ALTERNET_UI_API void TabControl_SetTabAlignment_(TabControl* obj, TabAlignment value)
{
    obj->SetTabAlignment(value);
}

ALTERNET_UI_API void TabControl_InsertPage_(TabControl* obj, int index, Control* page, const char16_t* title)
{
    obj->InsertPage(index, page, title);
}

ALTERNET_UI_API void TabControl_RemovePage_(TabControl* obj, int index, Control* page)
{
    obj->RemovePage(index, page);
}

ALTERNET_UI_API void TabControl_SetPageTitle_(TabControl* obj, int index, const char16_t* title)
{
    obj->SetPageTitle(index, title);
}

ALTERNET_UI_API Size_C TabControl_GetTotalPreferredSizeFromPageSize_(TabControl* obj, Size pageSize)
{
    return obj->GetTotalPreferredSizeFromPageSize(pageSize);
}

ALTERNET_UI_API void TabControl_SetEventCallback_(TabControl::TabControlEventCallbackType callback)
{
    TabControl::SetEventCallback(callback);
}

