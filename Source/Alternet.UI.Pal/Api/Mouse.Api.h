// <auto-generated> DO NOT MODIFY MANUALLY. Copyright (c) 2025 AlterNET Software.</auto-generated>

#pragma once

#include "Mouse.h"
#include "ApiUtils.h"
#include "Exceptions.h"

using namespace Alternet::UI;

ALTERNET_UI_API Mouse* Mouse_Create_()
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<Mouse*>([&](){
    #endif
        return new Mouse();
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API PointI_C Mouse_GetPosition_()
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<PointI_C>([&](){
    #endif
        return Mouse::GetPosition();
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API MouseButtonState Mouse_GetButtonState_(MouseButton button)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<MouseButtonState>([&](){
    #endif
        return Mouse::GetButtonState(button);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API void Mouse_SetEventCallback_(Mouse::MouseEventCallbackType callback)
{
    Mouse::SetEventCallback(callback);
}

