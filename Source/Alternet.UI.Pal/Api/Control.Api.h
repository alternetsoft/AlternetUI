// Auto generated code, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2021.

#pragma once

#include "Control.h"
#include "ApiUtils.h"

using namespace Alternet::UI;

ALTERNET_UI_API void Control_Destroy(Control* obj)
{
    delete obj;
}

ALTERNET_UI_API SizeF_C Control_GetSize(Control* obj)
{
    return obj->GetSize();
}

ALTERNET_UI_API void Control_SetSize(Control* obj, SizeF value)
{
    obj->SetSize(value);
}

ALTERNET_UI_API PointF_C Control_GetLocation(Control* obj)
{
    return obj->GetLocation();
}

ALTERNET_UI_API void Control_SetLocation(Control* obj, PointF value)
{
    obj->SetLocation(value);
}

ALTERNET_UI_API RectangleF_C Control_GetBounds(Control* obj)
{
    return obj->GetBounds();
}

ALTERNET_UI_API void Control_SetBounds(Control* obj, RectangleF value)
{
    obj->SetBounds(value);
}

ALTERNET_UI_API c_bool Control_GetVisible(Control* obj)
{
    return obj->GetVisible();
}

ALTERNET_UI_API void Control_SetVisible(Control* obj, c_bool value)
{
    obj->SetVisible(value);
}

ALTERNET_UI_API void Control_AddChild(Control* obj, Control* control)
{
    obj->AddChild(control);
}

ALTERNET_UI_API void Control_RemoveChild(Control* obj, Control* control)
{
    obj->RemoveChild(control);
}

ALTERNET_UI_API SizeF_C Control_GetPreferredSize(Control* obj, SizeF availableSize)
{
    return obj->GetPreferredSize(availableSize);
}

