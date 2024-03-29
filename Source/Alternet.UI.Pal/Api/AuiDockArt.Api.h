// <auto-generated> DO NOT MODIFY MANUALLY. Copyright (c) 2024 AlterNET Software.</auto-generated>

#pragma once

#include "AuiDockArt.h"
#include "ApiUtils.h"
#include "Exceptions.h"

using namespace Alternet::UI;

ALTERNET_UI_API AuiDockArt* AuiDockArt_Create_()
{
    #if !defined(__WXMSW__)
    return MarshalExceptions<AuiDockArt*>([&](){
    #endif
        return new AuiDockArt();
    #if !defined(__WXMSW__)
    });
    #endif
}

ALTERNET_UI_API Color_C AuiDockArt_GetColor_(void* handle, int id)
{
    #if !defined(__WXMSW__)
    return MarshalExceptions<Color_C>([&](){
    #endif
        return AuiDockArt::GetColor(handle, id);
    #if !defined(__WXMSW__)
    });
    #endif
}

ALTERNET_UI_API int AuiDockArt_GetMetric_(void* handle, int id)
{
    #if !defined(__WXMSW__)
    return MarshalExceptions<int>([&](){
    #endif
        return AuiDockArt::GetMetric(handle, id);
    #if !defined(__WXMSW__)
    });
    #endif
}

ALTERNET_UI_API void AuiDockArt_SetColor_(void* handle, int id, Color color)
{
    #if !defined(__WXMSW__)
    MarshalExceptions<void>([&](){
    #endif
        AuiDockArt::SetColor(handle, id, color);
    #if !defined(__WXMSW__)
    });
    #endif
}

ALTERNET_UI_API void AuiDockArt_SetMetric_(void* handle, int id, int value)
{
    #if !defined(__WXMSW__)
    MarshalExceptions<void>([&](){
    #endif
        AuiDockArt::SetMetric(handle, id, value);
    #if !defined(__WXMSW__)
    });
    #endif
}

