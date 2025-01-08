// <auto-generated> DO NOT MODIFY MANUALLY. Copyright (c) 2025 AlterNET Software.</auto-generated>

#pragma once

#include "Label.h"
#include "ApiUtils.h"
#include "Exceptions.h"

using namespace Alternet::UI;

ALTERNET_UI_API Label* Label_Create_()
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<Label*>([&](){
    #endif
        return new Label();
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API c_bool Label_IsEllipsized_(Label* obj)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<c_bool>([&](){
    #endif
        return obj->IsEllipsized();
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API void Label_Wrap_(Label* obj, int width)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    MarshalExceptions<void>([&](){
    #endif
        obj->Wrap(width);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

