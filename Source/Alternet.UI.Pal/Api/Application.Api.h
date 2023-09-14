// <auto-generated>This code was generated by a tool, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2023.</auto-generated>

#pragma once

#include "Application.h"
#include "Keyboard.h"
#include "Mouse.h"
#include "Clipboard.h"
#include "Window.h"
#include "ApiUtils.h"
#include "Exceptions.h"

using namespace Alternet::UI;

ALTERNET_UI_API Application* Application_Create_()
{
    return new Application();
}

ALTERNET_UI_API char16_t* Application_GetName_(Application* obj)
{
    return AllocPInvokeReturnString(obj->GetName());
}

ALTERNET_UI_API void Application_SetName_(Application* obj, const char16_t* value)
{
    obj->SetName(value);
}

ALTERNET_UI_API Keyboard* Application_GetKeyboard_(Application* obj)
{
    return obj->GetKeyboard();
}

ALTERNET_UI_API Mouse* Application_GetMouse_(Application* obj)
{
    return obj->GetMouse();
}

ALTERNET_UI_API Clipboard* Application_GetClipboard_(Application* obj)
{
    return obj->GetClipboard();
}

ALTERNET_UI_API char16_t* Application_GetDisplayName_(Application* obj)
{
    return AllocPInvokeReturnString(obj->GetDisplayName());
}

ALTERNET_UI_API void Application_SetDisplayName_(Application* obj, const char16_t* value)
{
    obj->SetDisplayName(value);
}

ALTERNET_UI_API char16_t* Application_GetAppClassName_(Application* obj)
{
    return AllocPInvokeReturnString(obj->GetAppClassName());
}

ALTERNET_UI_API void Application_SetAppClassName_(Application* obj, const char16_t* value)
{
    obj->SetAppClassName(value);
}

ALTERNET_UI_API char16_t* Application_GetVendorName_(Application* obj)
{
    return AllocPInvokeReturnString(obj->GetVendorName());
}

ALTERNET_UI_API void Application_SetVendorName_(Application* obj, const char16_t* value)
{
    obj->SetVendorName(value);
}

ALTERNET_UI_API char16_t* Application_GetVendorDisplayName_(Application* obj)
{
    return AllocPInvokeReturnString(obj->GetVendorDisplayName());
}

ALTERNET_UI_API void Application_SetVendorDisplayName_(Application* obj, const char16_t* value)
{
    obj->SetVendorDisplayName(value);
}

ALTERNET_UI_API c_bool Application_GetInUixmlPreviewerMode_(Application* obj)
{
    return obj->GetInUixmlPreviewerMode();
}

ALTERNET_UI_API void Application_SetInUixmlPreviewerMode_(Application* obj, c_bool value)
{
    obj->SetInUixmlPreviewerMode(value);
}

ALTERNET_UI_API c_bool Application_GetInvokeRequired_(Application* obj)
{
    return obj->GetInvokeRequired();
}

ALTERNET_UI_API void Application_ThrowError_(int value)
{
    Application::ThrowError(value);
}

ALTERNET_UI_API void Application_SetSystemOptionInt_(const char16_t* name, int value)
{
    Application::SetSystemOptionInt(name, value);
}

ALTERNET_UI_API void Application_Run_(Application* obj, Window* window)
{
    obj->Run(window);
}

ALTERNET_UI_API void Application_WakeUpIdle_(Application* obj)
{
    obj->WakeUpIdle();
}

ALTERNET_UI_API void Application_Exit_(Application* obj)
{
    obj->Exit();
}

ALTERNET_UI_API void Application_SuppressDiagnostics_(int flags)
{
    Application::SuppressDiagnostics(flags);
}

ALTERNET_UI_API void Application_BeginInvoke_(Application* obj, PInvokeCallbackActionType action)
{
    obj->BeginInvoke(action);
}

ALTERNET_UI_API void Application_ProcessPendingEvents_(Application* obj)
{
    obj->ProcessPendingEvents();
}

ALTERNET_UI_API c_bool Application_HasPendingEvents_(Application* obj)
{
    return obj->HasPendingEvents();
}

ALTERNET_UI_API void Application_SetEventCallback_(Application::ApplicationEventCallbackType callback)
{
    Application::SetEventCallback(callback);
}

