// <auto-generated>This code was generated by a tool, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2022.</auto-generated>

#pragma once

#include "Mouse.h"
#include "ApiUtils.h"

using namespace Alternet::UI;

ALTERNET_UI_API Mouse* Mouse_Create_()
{
    return new Mouse();
}

ALTERNET_UI_API Point_C Mouse_GetMousePosition_(Mouse* obj)
{
    return obj->GetMousePosition();
}

ALTERNET_UI_API void Mouse_SetEventCallback_(Mouse::MouseEventCallbackType callback)
{
    Mouse::SetEventCallback(callback);
}

