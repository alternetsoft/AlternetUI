// <auto-generated> DO NOT MODIFY MANUALLY. Copyright (c) 2023 AlterNET Software.</auto-generated>

#pragma once

#include "LinkLabel.h"
#include "ApiUtils.h"
#include "Exceptions.h"

using namespace Alternet::UI;

ALTERNET_UI_API LinkLabel* LinkLabel_Create_()
{
    return new LinkLabel();
}

ALTERNET_UI_API Color_C LinkLabel_GetHoverColor_(LinkLabel* obj)
{
    return obj->GetHoverColor();
}

ALTERNET_UI_API void LinkLabel_SetHoverColor_(LinkLabel* obj, Color value)
{
    obj->SetHoverColor(value);
}

ALTERNET_UI_API Color_C LinkLabel_GetNormalColor_(LinkLabel* obj)
{
    return obj->GetNormalColor();
}

ALTERNET_UI_API void LinkLabel_SetNormalColor_(LinkLabel* obj, Color value)
{
    obj->SetNormalColor(value);
}

ALTERNET_UI_API Color_C LinkLabel_GetVisitedColor_(LinkLabel* obj)
{
    return obj->GetVisitedColor();
}

ALTERNET_UI_API void LinkLabel_SetVisitedColor_(LinkLabel* obj, Color value)
{
    obj->SetVisitedColor(value);
}

ALTERNET_UI_API c_bool LinkLabel_GetVisited_(LinkLabel* obj)
{
    return obj->GetVisited();
}

ALTERNET_UI_API void LinkLabel_SetVisited_(LinkLabel* obj, c_bool value)
{
    obj->SetVisited(value);
}

ALTERNET_UI_API char16_t* LinkLabel_GetText_(LinkLabel* obj)
{
    return AllocPInvokeReturnString(obj->GetText());
}

ALTERNET_UI_API void LinkLabel_SetText_(LinkLabel* obj, const char16_t* value)
{
    obj->SetText(value);
}

ALTERNET_UI_API char16_t* LinkLabel_GetUrl_(LinkLabel* obj)
{
    return AllocPInvokeReturnString(obj->GetUrl());
}

ALTERNET_UI_API void LinkLabel_SetUrl_(LinkLabel* obj, const char16_t* value)
{
    obj->SetUrl(value);
}

ALTERNET_UI_API void LinkLabel_SetEventCallback_(LinkLabel::LinkLabelEventCallbackType callback)
{
    LinkLabel::SetEventCallback(callback);
}

