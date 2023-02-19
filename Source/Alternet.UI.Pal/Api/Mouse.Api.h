// <auto-generated>This code was generated by a tool, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2023.</auto-generated>

#pragma once

#include "Mouse.h"
#include "ApiUtils.h"
#include "Exceptions.h"

using namespace Alternet::UI;

ALTERNET_UI_API Mouse* Mouse_Create_()
{
    return MarshalExceptions<Mouse*>([&](){
            return new Mouse();
        });
}

ALTERNET_UI_API Point_C Mouse_GetPosition_(Mouse* obj)
{
    return MarshalExceptions<Point_C>([&](){
            return obj->GetPosition();
        });
}

ALTERNET_UI_API MouseButtonState Mouse_GetButtonState_(Mouse* obj, MouseButton button)
{
    return MarshalExceptions<MouseButtonState>([&](){
            return obj->GetButtonState(button);
        });
}

ALTERNET_UI_API void Mouse_SetEventCallback_(Mouse::MouseEventCallbackType callback)
{
    Mouse::SetEventCallback(callback);
}

