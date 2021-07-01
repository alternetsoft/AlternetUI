// Auto generated code, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2021.

#pragma once

#include "Control.h"
#include "DrawingContext.h"
#include "ApiUtils.h"

using namespace Alternet::UI;

ALTERNET_UI_API Control* Control_GetParent(Control* obj)
{
    return obj->GetParent();
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

ALTERNET_UI_API SizeF_C Control_GetClientSize(Control* obj)
{
    return obj->GetClientSize();
}

ALTERNET_UI_API void Control_SetClientSize(Control* obj, SizeF value)
{
    obj->SetClientSize(value);
}

ALTERNET_UI_API Thickness_C Control_GetIntrinsicLayoutPadding(Control* obj)
{
    return obj->GetIntrinsicLayoutPadding();
}

ALTERNET_UI_API Thickness_C Control_GetIntrinsicPreferredSizePadding(Control* obj)
{
    return obj->GetIntrinsicPreferredSizePadding();
}

ALTERNET_UI_API c_bool Control_GetVisible(Control* obj)
{
    return obj->GetVisible();
}

ALTERNET_UI_API void Control_SetVisible(Control* obj, c_bool value)
{
    obj->SetVisible(value);
}

ALTERNET_UI_API c_bool Control_GetIsMouseOver(Control* obj)
{
    return obj->GetIsMouseOver();
}

ALTERNET_UI_API Color_C Control_GetBackgroundColor(Control* obj)
{
    return obj->GetBackgroundColor();
}

ALTERNET_UI_API void Control_SetBackgroundColor(Control* obj, Color value)
{
    obj->SetBackgroundColor(value);
}

ALTERNET_UI_API Color_C Control_GetForegroundColor(Control* obj)
{
    return obj->GetForegroundColor();
}

ALTERNET_UI_API void Control_SetForegroundColor(Control* obj, Color value)
{
    obj->SetForegroundColor(value);
}

ALTERNET_UI_API void Control_SetMouseCapture(Control* obj, c_bool value)
{
    obj->SetMouseCapture(value);
}

ALTERNET_UI_API void Control_AddChild(Control* obj, Control* control)
{
    obj->AddChild(control);
}

ALTERNET_UI_API void Control_RemoveChild(Control* obj, Control* control)
{
    obj->RemoveChild(control);
}

ALTERNET_UI_API void Control_Update(Control* obj)
{
    obj->Update();
}

ALTERNET_UI_API SizeF_C Control_GetPreferredSize(Control* obj, SizeF availableSize)
{
    return obj->GetPreferredSize(availableSize);
}

ALTERNET_UI_API DrawingContext* Control_OpenPaintDrawingContext(Control* obj)
{
    return obj->OpenPaintDrawingContext();
}

ALTERNET_UI_API DrawingContext* Control_OpenClientDrawingContext(Control* obj)
{
    return obj->OpenClientDrawingContext();
}

ALTERNET_UI_API void Control_SetEventCallback(Control::ControlEventCallbackType callback)
{
    Control::SetEventCallback(callback);
}

