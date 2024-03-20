// <auto-generated> DO NOT MODIFY MANUALLY. Copyright (c) 2024 AlterNET Software.</auto-generated>

#pragma once

#include "WxTreeViewFactory.h"
#include "ApiUtils.h"
#include "Exceptions.h"

using namespace Alternet::UI;

ALTERNET_UI_API WxTreeViewFactory* WxTreeViewFactory_Create_()
{
    #if !defined(__WXMSW__)
    return MarshalExceptions<WxTreeViewFactory*>([&](){
    #endif
        return new WxTreeViewFactory();
    #if !defined(__WXMSW__)
    });
    #endif
}

ALTERNET_UI_API void WxTreeViewFactory_SetItemBold_(void* handle, void* item, c_bool bold)
{
    #if !defined(__WXMSW__)
    MarshalExceptions<void>([&](){
    #endif
        WxTreeViewFactory::SetItemBold(handle, item, bold);
    #if !defined(__WXMSW__)
    });
    #endif
}

ALTERNET_UI_API Color_C WxTreeViewFactory_GetItemTextColor_(void* handle, void* item)
{
    #if !defined(__WXMSW__)
    return MarshalExceptions<Color_C>([&](){
    #endif
        return WxTreeViewFactory::GetItemTextColor(handle, item);
    #if !defined(__WXMSW__)
    });
    #endif
}

ALTERNET_UI_API Color_C WxTreeViewFactory_GetItemBackgroundColor_(void* handle, void* item)
{
    #if !defined(__WXMSW__)
    return MarshalExceptions<Color_C>([&](){
    #endif
        return WxTreeViewFactory::GetItemBackgroundColor(handle, item);
    #if !defined(__WXMSW__)
    });
    #endif
}

ALTERNET_UI_API void WxTreeViewFactory_SetItemTextColor_(void* handle, void* item, Color color)
{
    #if !defined(__WXMSW__)
    MarshalExceptions<void>([&](){
    #endif
        WxTreeViewFactory::SetItemTextColor(handle, item, color);
    #if !defined(__WXMSW__)
    });
    #endif
}

ALTERNET_UI_API void WxTreeViewFactory_SetItemBackgroundColor_(void* handle, void* item, Color color)
{
    #if !defined(__WXMSW__)
    MarshalExceptions<void>([&](){
    #endif
        WxTreeViewFactory::SetItemBackgroundColor(handle, item, color);
    #if !defined(__WXMSW__)
    });
    #endif
}

ALTERNET_UI_API void WxTreeViewFactory_ResetItemTextColor_(void* handle, void* item)
{
    #if !defined(__WXMSW__)
    MarshalExceptions<void>([&](){
    #endif
        WxTreeViewFactory::ResetItemTextColor(handle, item);
    #if !defined(__WXMSW__)
    });
    #endif
}

ALTERNET_UI_API void WxTreeViewFactory_ResetItemBackgroundColor_(void* handle, void* item)
{
    #if !defined(__WXMSW__)
    MarshalExceptions<void>([&](){
    #endif
        WxTreeViewFactory::ResetItemBackgroundColor(handle, item);
    #if !defined(__WXMSW__)
    });
    #endif
}

