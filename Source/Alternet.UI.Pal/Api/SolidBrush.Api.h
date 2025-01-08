// <auto-generated> DO NOT MODIFY MANUALLY. Copyright (c) 2025 AlterNET Software.</auto-generated>

#pragma once

#include "SolidBrush.h"
#include "ApiUtils.h"
#include "Exceptions.h"

using namespace Alternet::UI;

ALTERNET_UI_API SolidBrush* SolidBrush_Create_()
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<SolidBrush*>([&](){
    #endif
        return new SolidBrush();
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API void SolidBrush_Initialize_(SolidBrush* obj, Color color)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    MarshalExceptions<void>([&](){
    #endif
        obj->Initialize(color);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

