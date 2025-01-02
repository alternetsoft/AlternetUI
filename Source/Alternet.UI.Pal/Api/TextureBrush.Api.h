// <auto-generated> DO NOT MODIFY MANUALLY. Copyright (c) 2025 AlterNET Software.</auto-generated>

#pragma once

#include "TextureBrush.h"
#include "Image.h"
#include "ApiUtils.h"
#include "Exceptions.h"

using namespace Alternet::UI;

ALTERNET_UI_API TextureBrush* TextureBrush_Create_()
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<TextureBrush*>([&](){
    #endif
        return new TextureBrush();
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API void TextureBrush_Initialize_(TextureBrush* obj, Image* image)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    MarshalExceptions<void>([&](){
    #endif
        obj->Initialize(image);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

