// <auto-generated>This code was generated by a tool, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2023.</auto-generated>

#pragma once

#include "Control.h"
#include "Font.h"
#include "UnmanagedDataObject.h"
#include "DrawingContext.h"
#include "ApiUtils.h"
#include "Exceptions.h"

using namespace Alternet::UI;

ALTERNET_UI_API Control* Control_GetParentRefCounted_(Control* obj)
{
    return MarshalExceptions<Control*>([&](){
            return obj->GetParentRefCounted();
        });
}

ALTERNET_UI_API char16_t* Control_GetToolTip_(Control* obj)
{
    return MarshalExceptions<char16_t*>([&](){
            return AllocPInvokeReturnString(obj->GetToolTip());
        });
}

ALTERNET_UI_API void Control_SetToolTip_(Control* obj, const char16_t* value)
{
    MarshalExceptions<void>([&](){
            obj->SetToolTip(ToOptional(value));
        });
}

ALTERNET_UI_API c_bool Control_GetAllowDrop_(Control* obj)
{
    return MarshalExceptions<c_bool>([&](){
            return obj->GetAllowDrop();
        });
}

ALTERNET_UI_API void Control_SetAllowDrop_(Control* obj, c_bool value)
{
    MarshalExceptions<void>([&](){
            obj->SetAllowDrop(value);
        });
}

ALTERNET_UI_API Size_C Control_GetSize_(Control* obj)
{
    return MarshalExceptions<Size_C>([&](){
            return obj->GetSize();
        });
}

ALTERNET_UI_API void Control_SetSize_(Control* obj, Size value)
{
    MarshalExceptions<void>([&](){
            obj->SetSize(value);
        });
}

ALTERNET_UI_API Point_C Control_GetLocation_(Control* obj)
{
    return MarshalExceptions<Point_C>([&](){
            return obj->GetLocation();
        });
}

ALTERNET_UI_API void Control_SetLocation_(Control* obj, Point value)
{
    MarshalExceptions<void>([&](){
            obj->SetLocation(value);
        });
}

ALTERNET_UI_API Rect_C Control_GetBounds_(Control* obj)
{
    return MarshalExceptions<Rect_C>([&](){
            return obj->GetBounds();
        });
}

ALTERNET_UI_API void Control_SetBounds_(Control* obj, Rect value)
{
    MarshalExceptions<void>([&](){
            obj->SetBounds(value);
        });
}

ALTERNET_UI_API Size_C Control_GetClientSize_(Control* obj)
{
    return MarshalExceptions<Size_C>([&](){
            return obj->GetClientSize();
        });
}

ALTERNET_UI_API void Control_SetClientSize_(Control* obj, Size value)
{
    MarshalExceptions<void>([&](){
            obj->SetClientSize(value);
        });
}

ALTERNET_UI_API Thickness_C Control_GetIntrinsicLayoutPadding_(Control* obj)
{
    return MarshalExceptions<Thickness_C>([&](){
            return obj->GetIntrinsicLayoutPadding();
        });
}

ALTERNET_UI_API Thickness_C Control_GetIntrinsicPreferredSizePadding_(Control* obj)
{
    return MarshalExceptions<Thickness_C>([&](){
            return obj->GetIntrinsicPreferredSizePadding();
        });
}

ALTERNET_UI_API c_bool Control_GetVisible_(Control* obj)
{
    return MarshalExceptions<c_bool>([&](){
            return obj->GetVisible();
        });
}

ALTERNET_UI_API void Control_SetVisible_(Control* obj, c_bool value)
{
    MarshalExceptions<void>([&](){
            obj->SetVisible(value);
        });
}

ALTERNET_UI_API c_bool Control_GetEnabled_(Control* obj)
{
    return MarshalExceptions<c_bool>([&](){
            return obj->GetEnabled();
        });
}

ALTERNET_UI_API void Control_SetEnabled_(Control* obj, c_bool value)
{
    MarshalExceptions<void>([&](){
            obj->SetEnabled(value);
        });
}

ALTERNET_UI_API c_bool Control_GetUserPaint_(Control* obj)
{
    return MarshalExceptions<c_bool>([&](){
            return obj->GetUserPaint();
        });
}

ALTERNET_UI_API void Control_SetUserPaint_(Control* obj, c_bool value)
{
    MarshalExceptions<void>([&](){
            obj->SetUserPaint(value);
        });
}

ALTERNET_UI_API c_bool Control_GetIsMouseOver_(Control* obj)
{
    return MarshalExceptions<c_bool>([&](){
            return obj->GetIsMouseOver();
        });
}

ALTERNET_UI_API c_bool Control_GetHasWindowCreated_(Control* obj)
{
    return MarshalExceptions<c_bool>([&](){
            return obj->GetHasWindowCreated();
        });
}

ALTERNET_UI_API Color_C Control_GetBackgroundColor_(Control* obj)
{
    return MarshalExceptions<Color_C>([&](){
            return obj->GetBackgroundColor();
        });
}

ALTERNET_UI_API void Control_SetBackgroundColor_(Control* obj, Color value)
{
    MarshalExceptions<void>([&](){
            obj->SetBackgroundColor(value);
        });
}

ALTERNET_UI_API Color_C Control_GetForegroundColor_(Control* obj)
{
    return MarshalExceptions<Color_C>([&](){
            return obj->GetForegroundColor();
        });
}

ALTERNET_UI_API void Control_SetForegroundColor_(Control* obj, Color value)
{
    MarshalExceptions<void>([&](){
            obj->SetForegroundColor(value);
        });
}

ALTERNET_UI_API Font* Control_GetFont_(Control* obj)
{
    return MarshalExceptions<Font*>([&](){
            return obj->GetFont();
        });
}

ALTERNET_UI_API void Control_SetFont_(Control* obj, Font* value)
{
    MarshalExceptions<void>([&](){
            obj->SetFont(value);
        });
}

ALTERNET_UI_API c_bool Control_GetIsMouseCaptured_(Control* obj)
{
    return MarshalExceptions<c_bool>([&](){
            return obj->GetIsMouseCaptured();
        });
}

ALTERNET_UI_API c_bool Control_GetTabStop_(Control* obj)
{
    return MarshalExceptions<c_bool>([&](){
            return obj->GetTabStop();
        });
}

ALTERNET_UI_API void Control_SetTabStop_(Control* obj, c_bool value)
{
    MarshalExceptions<void>([&](){
            obj->SetTabStop(value);
        });
}

ALTERNET_UI_API c_bool Control_GetIsFocused_(Control* obj)
{
    return MarshalExceptions<c_bool>([&](){
            return obj->GetIsFocused();
        });
}

ALTERNET_UI_API void* Control_GetHandle_(Control* obj)
{
    return MarshalExceptions<void*>([&](){
            return obj->GetHandle();
        });
}

ALTERNET_UI_API void* Control_GetWxWidget_(Control* obj)
{
    return MarshalExceptions<void*>([&](){
            return obj->GetWxWidget();
        });
}

ALTERNET_UI_API c_bool Control_GetIsScrollable_(Control* obj)
{
    return MarshalExceptions<c_bool>([&](){
            return obj->GetIsScrollable();
        });
}

ALTERNET_UI_API void Control_SetIsScrollable_(Control* obj, c_bool value)
{
    MarshalExceptions<void>([&](){
            obj->SetIsScrollable(value);
        });
}

ALTERNET_UI_API void* Control_GetContainingSizer_(Control* obj)
{
    return MarshalExceptions<void*>([&](){
            return obj->GetContainingSizer();
        });
}

ALTERNET_UI_API void* Control_GetSizer_(Control* obj)
{
    return MarshalExceptions<void*>([&](){
            return obj->GetSizer();
        });
}

ALTERNET_UI_API void Control_SetSizer_(Control* obj, void* sizer, c_bool deleteOld)
{
    MarshalExceptions<void>([&](){
            obj->SetSizer(sizer, deleteOld);
        });
}

ALTERNET_UI_API void Control_SetSizerAndFit_(Control* obj, void* sizer, c_bool deleteOld)
{
    MarshalExceptions<void>([&](){
            obj->SetSizerAndFit(sizer, deleteOld);
        });
}

ALTERNET_UI_API void Control_SetScrollBar_(Control* obj, ScrollBarOrientation orientation, c_bool visible, int value, int largeChange, int maximum)
{
    MarshalExceptions<void>([&](){
            obj->SetScrollBar(orientation, visible, value, largeChange, maximum);
        });
}

ALTERNET_UI_API c_bool Control_IsScrollBarVisible_(Control* obj, ScrollBarOrientation orientation)
{
    return MarshalExceptions<c_bool>([&](){
            return obj->IsScrollBarVisible(orientation);
        });
}

ALTERNET_UI_API int Control_GetScrollBarValue_(Control* obj, ScrollBarOrientation orientation)
{
    return MarshalExceptions<int>([&](){
            return obj->GetScrollBarValue(orientation);
        });
}

ALTERNET_UI_API int Control_GetScrollBarLargeChange_(Control* obj, ScrollBarOrientation orientation)
{
    return MarshalExceptions<int>([&](){
            return obj->GetScrollBarLargeChange(orientation);
        });
}

ALTERNET_UI_API int Control_GetScrollBarMaximum_(Control* obj, ScrollBarOrientation orientation)
{
    return MarshalExceptions<int>([&](){
            return obj->GetScrollBarMaximum(orientation);
        });
}

ALTERNET_UI_API Size_C Control_GetDPI_(Control* obj)
{
    return MarshalExceptions<Size_C>([&](){
            return obj->GetDPI();
        });
}

ALTERNET_UI_API void Control_SetMouseCapture_(Control* obj, c_bool value)
{
    MarshalExceptions<void>([&](){
            obj->SetMouseCapture(value);
        });
}

ALTERNET_UI_API void Control_AddChild_(Control* obj, Control* control)
{
    MarshalExceptions<void>([&](){
            obj->AddChild(control);
        });
}

ALTERNET_UI_API void Control_RemoveChild_(Control* obj, Control* control)
{
    MarshalExceptions<void>([&](){
            obj->RemoveChild(control);
        });
}

ALTERNET_UI_API void Control_Invalidate_(Control* obj)
{
    MarshalExceptions<void>([&](){
            obj->Invalidate();
        });
}

ALTERNET_UI_API void Control_Update_(Control* obj)
{
    MarshalExceptions<void>([&](){
            obj->Update();
        });
}

ALTERNET_UI_API Size_C Control_GetPreferredSize_(Control* obj, Size availableSize)
{
    return MarshalExceptions<Size_C>([&](){
            return obj->GetPreferredSize(availableSize);
        });
}

ALTERNET_UI_API DragDropEffects Control_DoDragDrop_(Control* obj, UnmanagedDataObject* data, DragDropEffects allowedEffects)
{
    return MarshalExceptions<DragDropEffects>([&](){
            return obj->DoDragDrop(data, allowedEffects);
        });
}

ALTERNET_UI_API DrawingContext* Control_OpenPaintDrawingContext_(Control* obj)
{
    return MarshalExceptions<DrawingContext*>([&](){
            return obj->OpenPaintDrawingContext();
        });
}

ALTERNET_UI_API DrawingContext* Control_OpenClientDrawingContext_(Control* obj)
{
    return MarshalExceptions<DrawingContext*>([&](){
            return obj->OpenClientDrawingContext();
        });
}

ALTERNET_UI_API void Control_BeginUpdate_(Control* obj)
{
    MarshalExceptions<void>([&](){
            obj->BeginUpdate();
        });
}

ALTERNET_UI_API void Control_EndUpdate_(Control* obj)
{
    MarshalExceptions<void>([&](){
            obj->EndUpdate();
        });
}

ALTERNET_UI_API void Control_RecreateWindow_(Control* obj)
{
    MarshalExceptions<void>([&](){
            obj->RecreateWindow();
        });
}

ALTERNET_UI_API Control* Control_HitTest_(Point screenPoint)
{
    return MarshalExceptions<Control*>([&](){
            return Control::HitTest(screenPoint);
        });
}

ALTERNET_UI_API Point_C Control_ClientToScreen_(Control* obj, Point point)
{
    return MarshalExceptions<Point_C>([&](){
            return obj->ClientToScreen(point);
        });
}

ALTERNET_UI_API Point_C Control_ScreenToClient_(Control* obj, Point point)
{
    return MarshalExceptions<Point_C>([&](){
            return obj->ScreenToClient(point);
        });
}

ALTERNET_UI_API Int32Point_C Control_ScreenToDevice_(Control* obj, Point point)
{
    return MarshalExceptions<Int32Point_C>([&](){
            return obj->ScreenToDevice(point);
        });
}

ALTERNET_UI_API Point_C Control_DeviceToScreen_(Control* obj, Int32Point point)
{
    return MarshalExceptions<Point_C>([&](){
            return obj->DeviceToScreen(point);
        });
}

ALTERNET_UI_API Control* Control_GetFocusedControl_()
{
    return MarshalExceptions<Control*>([&](){
            return Control::GetFocusedControl();
        });
}

ALTERNET_UI_API c_bool Control_SetFocus_(Control* obj)
{
    return MarshalExceptions<c_bool>([&](){
            return obj->SetFocus();
        });
}

ALTERNET_UI_API void Control_FocusNextControl_(Control* obj, c_bool forward, c_bool nested)
{
    MarshalExceptions<void>([&](){
            obj->FocusNextControl(forward, nested);
        });
}

ALTERNET_UI_API void Control_BeginInit_(Control* obj)
{
    MarshalExceptions<void>([&](){
            obj->BeginInit();
        });
}

ALTERNET_UI_API void Control_EndInit_(Control* obj)
{
    MarshalExceptions<void>([&](){
            obj->EndInit();
        });
}

ALTERNET_UI_API void Control_Destroy_(Control* obj)
{
    MarshalExceptions<void>([&](){
            obj->Destroy();
        });
}

ALTERNET_UI_API void Control_SaveScreenshot_(Control* obj, const char16_t* fileName)
{
    MarshalExceptions<void>([&](){
            obj->SaveScreenshot(fileName);
        });
}

ALTERNET_UI_API void Control_SendSizeEvent_(Control* obj)
{
    MarshalExceptions<void>([&](){
            obj->SendSizeEvent();
        });
}

ALTERNET_UI_API void Control_SetEventCallback_(Control::ControlEventCallbackType callback)
{
    Control::SetEventCallback(callback);
}

