// <auto-generated> DO NOT MODIFY MANUALLY. Copyright (c) 2024 AlterNET Software.</auto-generated>

#pragma once

#include "Button.h"
#include "Image.h"
#include "ApiUtils.h"
#include "Exceptions.h"

using namespace Alternet::UI;

ALTERNET_UI_API Button* Button_Create_()
{
    #if !defined(__WXMSW__)
    return MarshalExceptions<Button*>([&](){
    #endif
        return new Button();
    #if !defined(__WXMSW__)
    });
    #endif
}

ALTERNET_UI_API c_bool Button_GetImagesEnabled_()
{
    #if !defined(__WXMSW__)
    return MarshalExceptions<c_bool>([&](){
    #endif
        return Button::GetImagesEnabled();
    #if !defined(__WXMSW__)
    });
    #endif
}

ALTERNET_UI_API void Button_SetImagesEnabled_(c_bool value)
{
    #if !defined(__WXMSW__)
    MarshalExceptions<void>([&](){
    #endif
        Button::SetImagesEnabled(value);
    #if !defined(__WXMSW__)
    });
    #endif
}

ALTERNET_UI_API char16_t* Button_GetText_(Button* obj)
{
    #if !defined(__WXMSW__)
    return MarshalExceptions<char16_t*>([&](){
    #endif
        return AllocPInvokeReturnString(obj->GetText());
    #if !defined(__WXMSW__)
    });
    #endif
}

ALTERNET_UI_API void Button_SetText_(Button* obj, const char16_t* value)
{
    #if !defined(__WXMSW__)
    MarshalExceptions<void>([&](){
    #endif
        obj->SetText(value);
    #if !defined(__WXMSW__)
    });
    #endif
}

ALTERNET_UI_API c_bool Button_GetExactFit_(Button* obj)
{
    #if !defined(__WXMSW__)
    return MarshalExceptions<c_bool>([&](){
    #endif
        return obj->GetExactFit();
    #if !defined(__WXMSW__)
    });
    #endif
}

ALTERNET_UI_API void Button_SetExactFit_(Button* obj, c_bool value)
{
    #if !defined(__WXMSW__)
    MarshalExceptions<void>([&](){
    #endif
        obj->SetExactFit(value);
    #if !defined(__WXMSW__)
    });
    #endif
}

ALTERNET_UI_API c_bool Button_GetIsDefault_(Button* obj)
{
    #if !defined(__WXMSW__)
    return MarshalExceptions<c_bool>([&](){
    #endif
        return obj->GetIsDefault();
    #if !defined(__WXMSW__)
    });
    #endif
}

ALTERNET_UI_API void Button_SetIsDefault_(Button* obj, c_bool value)
{
    #if !defined(__WXMSW__)
    MarshalExceptions<void>([&](){
    #endif
        obj->SetIsDefault(value);
    #if !defined(__WXMSW__)
    });
    #endif
}

ALTERNET_UI_API c_bool Button_GetHasBorder_(Button* obj)
{
    #if !defined(__WXMSW__)
    return MarshalExceptions<c_bool>([&](){
    #endif
        return obj->GetHasBorder();
    #if !defined(__WXMSW__)
    });
    #endif
}

ALTERNET_UI_API void Button_SetHasBorder_(Button* obj, c_bool value)
{
    #if !defined(__WXMSW__)
    MarshalExceptions<void>([&](){
    #endif
        obj->SetHasBorder(value);
    #if !defined(__WXMSW__)
    });
    #endif
}

ALTERNET_UI_API c_bool Button_GetIsCancel_(Button* obj)
{
    #if !defined(__WXMSW__)
    return MarshalExceptions<c_bool>([&](){
    #endif
        return obj->GetIsCancel();
    #if !defined(__WXMSW__)
    });
    #endif
}

ALTERNET_UI_API void Button_SetIsCancel_(Button* obj, c_bool value)
{
    #if !defined(__WXMSW__)
    MarshalExceptions<void>([&](){
    #endif
        obj->SetIsCancel(value);
    #if !defined(__WXMSW__)
    });
    #endif
}

ALTERNET_UI_API Image* Button_GetNormalImage_(Button* obj)
{
    #if !defined(__WXMSW__)
    return MarshalExceptions<Image*>([&](){
    #endif
        return obj->GetNormalImage();
    #if !defined(__WXMSW__)
    });
    #endif
}

ALTERNET_UI_API void Button_SetNormalImage_(Button* obj, Image* value)
{
    #if !defined(__WXMSW__)
    MarshalExceptions<void>([&](){
    #endif
        obj->SetNormalImage(value);
    #if !defined(__WXMSW__)
    });
    #endif
}

ALTERNET_UI_API Image* Button_GetHoveredImage_(Button* obj)
{
    #if !defined(__WXMSW__)
    return MarshalExceptions<Image*>([&](){
    #endif
        return obj->GetHoveredImage();
    #if !defined(__WXMSW__)
    });
    #endif
}

ALTERNET_UI_API void Button_SetHoveredImage_(Button* obj, Image* value)
{
    #if !defined(__WXMSW__)
    MarshalExceptions<void>([&](){
    #endif
        obj->SetHoveredImage(value);
    #if !defined(__WXMSW__)
    });
    #endif
}

ALTERNET_UI_API Image* Button_GetPressedImage_(Button* obj)
{
    #if !defined(__WXMSW__)
    return MarshalExceptions<Image*>([&](){
    #endif
        return obj->GetPressedImage();
    #if !defined(__WXMSW__)
    });
    #endif
}

ALTERNET_UI_API void Button_SetPressedImage_(Button* obj, Image* value)
{
    #if !defined(__WXMSW__)
    MarshalExceptions<void>([&](){
    #endif
        obj->SetPressedImage(value);
    #if !defined(__WXMSW__)
    });
    #endif
}

ALTERNET_UI_API Image* Button_GetDisabledImage_(Button* obj)
{
    #if !defined(__WXMSW__)
    return MarshalExceptions<Image*>([&](){
    #endif
        return obj->GetDisabledImage();
    #if !defined(__WXMSW__)
    });
    #endif
}

ALTERNET_UI_API void Button_SetDisabledImage_(Button* obj, Image* value)
{
    #if !defined(__WXMSW__)
    MarshalExceptions<void>([&](){
    #endif
        obj->SetDisabledImage(value);
    #if !defined(__WXMSW__)
    });
    #endif
}

ALTERNET_UI_API Image* Button_GetFocusedImage_(Button* obj)
{
    #if !defined(__WXMSW__)
    return MarshalExceptions<Image*>([&](){
    #endif
        return obj->GetFocusedImage();
    #if !defined(__WXMSW__)
    });
    #endif
}

ALTERNET_UI_API void Button_SetFocusedImage_(Button* obj, Image* value)
{
    #if !defined(__WXMSW__)
    MarshalExceptions<void>([&](){
    #endif
        obj->SetFocusedImage(value);
    #if !defined(__WXMSW__)
    });
    #endif
}

ALTERNET_UI_API c_bool Button_GetTextVisible_(Button* obj)
{
    #if !defined(__WXMSW__)
    return MarshalExceptions<c_bool>([&](){
    #endif
        return obj->GetTextVisible();
    #if !defined(__WXMSW__)
    });
    #endif
}

ALTERNET_UI_API void Button_SetTextVisible_(Button* obj, c_bool value)
{
    #if !defined(__WXMSW__)
    MarshalExceptions<void>([&](){
    #endif
        obj->SetTextVisible(value);
    #if !defined(__WXMSW__)
    });
    #endif
}

ALTERNET_UI_API int Button_GetTextAlign_(Button* obj)
{
    #if !defined(__WXMSW__)
    return MarshalExceptions<int>([&](){
    #endif
        return obj->GetTextAlign();
    #if !defined(__WXMSW__)
    });
    #endif
}

ALTERNET_UI_API void Button_SetTextAlign_(Button* obj, int value)
{
    #if !defined(__WXMSW__)
    MarshalExceptions<void>([&](){
    #endif
        obj->SetTextAlign(value);
    #if !defined(__WXMSW__)
    });
    #endif
}

ALTERNET_UI_API void Button_SetImagePosition_(Button* obj, int dir)
{
    #if !defined(__WXMSW__)
    MarshalExceptions<void>([&](){
    #endif
        obj->SetImagePosition(dir);
    #if !defined(__WXMSW__)
    });
    #endif
}

ALTERNET_UI_API void Button_SetImageMargins_(Button* obj, double x, double y)
{
    #if !defined(__WXMSW__)
    MarshalExceptions<void>([&](){
    #endif
        obj->SetImageMargins(x, y);
    #if !defined(__WXMSW__)
    });
    #endif
}

ALTERNET_UI_API void Button_SetEventCallback_(Button::ButtonEventCallbackType callback)
{
    Button::SetEventCallback(callback);
}

