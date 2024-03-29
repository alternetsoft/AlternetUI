// <auto-generated> DO NOT MODIFY MANUALLY. Copyright (c) 2024 AlterNET Software.</auto-generated>

#pragma once

#include "SizerItem.h"
#include "ApiUtils.h"
#include "Exceptions.h"

using namespace Alternet::UI;

ALTERNET_UI_API SizerItem* SizerItem_Create_()
{
    #if !defined(__WXMSW__)
    return MarshalExceptions<SizerItem*>([&](){
    #endif
        return new SizerItem();
    #if !defined(__WXMSW__)
    });
    #endif
}

ALTERNET_UI_API void* SizerItem_CreateSizerItem_(void* window, int proportion, int flag, int border, void* userData)
{
    #if !defined(__WXMSW__)
    return MarshalExceptions<void*>([&](){
    #endif
        return SizerItem::CreateSizerItem(window, proportion, flag, border, userData);
    #if !defined(__WXMSW__)
    });
    #endif
}

ALTERNET_UI_API void* SizerItem_CreateSizerItem2_(void* window, void* sizerFlags)
{
    #if !defined(__WXMSW__)
    return MarshalExceptions<void*>([&](){
    #endif
        return SizerItem::CreateSizerItem2(window, sizerFlags);
    #if !defined(__WXMSW__)
    });
    #endif
}

ALTERNET_UI_API void* SizerItem_CreateSizerItem3_(void* sizer, int proportion, int flag, int border, void* userData)
{
    #if !defined(__WXMSW__)
    return MarshalExceptions<void*>([&](){
    #endif
        return SizerItem::CreateSizerItem3(sizer, proportion, flag, border, userData);
    #if !defined(__WXMSW__)
    });
    #endif
}

ALTERNET_UI_API void* SizerItem_CreateSizerItem4_(void* sizer, void* sizerFlags)
{
    #if !defined(__WXMSW__)
    return MarshalExceptions<void*>([&](){
    #endif
        return SizerItem::CreateSizerItem4(sizer, sizerFlags);
    #if !defined(__WXMSW__)
    });
    #endif
}

ALTERNET_UI_API void* SizerItem_CreateSizerItem5_(int width, int height, int proportion, int flag, int border, void* userData)
{
    #if !defined(__WXMSW__)
    return MarshalExceptions<void*>([&](){
    #endif
        return SizerItem::CreateSizerItem5(width, height, proportion, flag, border, userData);
    #if !defined(__WXMSW__)
    });
    #endif
}

ALTERNET_UI_API void* SizerItem_CreateSizerItem6_(int width, int height, void* sizerFlags)
{
    #if !defined(__WXMSW__)
    return MarshalExceptions<void*>([&](){
    #endif
        return SizerItem::CreateSizerItem6(width, height, sizerFlags);
    #if !defined(__WXMSW__)
    });
    #endif
}

ALTERNET_UI_API void* SizerItem_CreateSizerItem7_()
{
    #if !defined(__WXMSW__)
    return MarshalExceptions<void*>([&](){
    #endif
        return SizerItem::CreateSizerItem7();
    #if !defined(__WXMSW__)
    });
    #endif
}

ALTERNET_UI_API void SizerItem_DeleteWindows_(void* handle)
{
    #if !defined(__WXMSW__)
    MarshalExceptions<void>([&](){
    #endif
        SizerItem::DeleteWindows(handle);
    #if !defined(__WXMSW__)
    });
    #endif
}

ALTERNET_UI_API void SizerItem_DetachSizer_(void* handle)
{
    #if !defined(__WXMSW__)
    MarshalExceptions<void>([&](){
    #endif
        SizerItem::DetachSizer(handle);
    #if !defined(__WXMSW__)
    });
    #endif
}

ALTERNET_UI_API void SizerItem_DetachWindow_(void* handle)
{
    #if !defined(__WXMSW__)
    MarshalExceptions<void>([&](){
    #endif
        SizerItem::DetachWindow(handle);
    #if !defined(__WXMSW__)
    });
    #endif
}

ALTERNET_UI_API SizeI_C SizerItem_GetSize_(void* handle)
{
    #if !defined(__WXMSW__)
    return MarshalExceptions<SizeI_C>([&](){
    #endif
        return SizerItem::GetSize(handle);
    #if !defined(__WXMSW__)
    });
    #endif
}

ALTERNET_UI_API SizeI_C SizerItem_CalcMin_(void* handle)
{
    #if !defined(__WXMSW__)
    return MarshalExceptions<SizeI_C>([&](){
    #endif
        return SizerItem::CalcMin(handle);
    #if !defined(__WXMSW__)
    });
    #endif
}

ALTERNET_UI_API void SizerItem_SetDimension_(void* handle, PointI pos, SizeI size)
{
    #if !defined(__WXMSW__)
    MarshalExceptions<void>([&](){
    #endif
        SizerItem::SetDimension(handle, pos, size);
    #if !defined(__WXMSW__)
    });
    #endif
}

ALTERNET_UI_API SizeI_C SizerItem_GetMinSize_(void* handle)
{
    #if !defined(__WXMSW__)
    return MarshalExceptions<SizeI_C>([&](){
    #endif
        return SizerItem::GetMinSize(handle);
    #if !defined(__WXMSW__)
    });
    #endif
}

ALTERNET_UI_API SizeI_C SizerItem_GetMinSizeWithBorder_(void* handle)
{
    #if !defined(__WXMSW__)
    return MarshalExceptions<SizeI_C>([&](){
    #endif
        return SizerItem::GetMinSizeWithBorder(handle);
    #if !defined(__WXMSW__)
    });
    #endif
}

ALTERNET_UI_API SizeI_C SizerItem_GetMaxSize_(void* handle)
{
    #if !defined(__WXMSW__)
    return MarshalExceptions<SizeI_C>([&](){
    #endif
        return SizerItem::GetMaxSize(handle);
    #if !defined(__WXMSW__)
    });
    #endif
}

ALTERNET_UI_API SizeI_C SizerItem_GetMaxSizeWithBorder_(void* handle)
{
    #if !defined(__WXMSW__)
    return MarshalExceptions<SizeI_C>([&](){
    #endif
        return SizerItem::GetMaxSizeWithBorder(handle);
    #if !defined(__WXMSW__)
    });
    #endif
}

ALTERNET_UI_API void SizerItem_SetMinSize_(void* handle, int x, int y)
{
    #if !defined(__WXMSW__)
    MarshalExceptions<void>([&](){
    #endif
        SizerItem::SetMinSize(handle, x, y);
    #if !defined(__WXMSW__)
    });
    #endif
}

ALTERNET_UI_API void SizerItem_SetInitSize_(void* handle, int x, int y)
{
    #if !defined(__WXMSW__)
    MarshalExceptions<void>([&](){
    #endif
        SizerItem::SetInitSize(handle, x, y);
    #if !defined(__WXMSW__)
    });
    #endif
}

ALTERNET_UI_API void SizerItem_SetRatio_(void* handle, int width, int height)
{
    #if !defined(__WXMSW__)
    MarshalExceptions<void>([&](){
    #endif
        SizerItem::SetRatio(handle, width, height);
    #if !defined(__WXMSW__)
    });
    #endif
}

ALTERNET_UI_API void SizerItem_SetRatio2_(void* handle, float ratio)
{
    #if !defined(__WXMSW__)
    MarshalExceptions<void>([&](){
    #endif
        SizerItem::SetRatio2(handle, ratio);
    #if !defined(__WXMSW__)
    });
    #endif
}

ALTERNET_UI_API float SizerItem_GetRatio_(void* handle)
{
    #if !defined(__WXMSW__)
    return MarshalExceptions<float>([&](){
    #endif
        return SizerItem::GetRatio(handle);
    #if !defined(__WXMSW__)
    });
    #endif
}

ALTERNET_UI_API RectI_C SizerItem_GetRect_(void* handle)
{
    #if !defined(__WXMSW__)
    return MarshalExceptions<RectI_C>([&](){
    #endif
        return SizerItem::GetRect(handle);
    #if !defined(__WXMSW__)
    });
    #endif
}

ALTERNET_UI_API void SizerItem_SetId_(void* handle, int id)
{
    #if !defined(__WXMSW__)
    MarshalExceptions<void>([&](){
    #endif
        SizerItem::SetId(handle, id);
    #if !defined(__WXMSW__)
    });
    #endif
}

ALTERNET_UI_API int SizerItem_GetId_(void* handle)
{
    #if !defined(__WXMSW__)
    return MarshalExceptions<int>([&](){
    #endif
        return SizerItem::GetId(handle);
    #if !defined(__WXMSW__)
    });
    #endif
}

ALTERNET_UI_API c_bool SizerItem_IsWindow_(void* handle)
{
    #if !defined(__WXMSW__)
    return MarshalExceptions<c_bool>([&](){
    #endif
        return SizerItem::IsWindow(handle);
    #if !defined(__WXMSW__)
    });
    #endif
}

ALTERNET_UI_API c_bool SizerItem_IsSizer_(void* handle)
{
    #if !defined(__WXMSW__)
    return MarshalExceptions<c_bool>([&](){
    #endif
        return SizerItem::IsSizer(handle);
    #if !defined(__WXMSW__)
    });
    #endif
}

ALTERNET_UI_API c_bool SizerItem_IsSpacer_(void* handle)
{
    #if !defined(__WXMSW__)
    return MarshalExceptions<c_bool>([&](){
    #endif
        return SizerItem::IsSpacer(handle);
    #if !defined(__WXMSW__)
    });
    #endif
}

ALTERNET_UI_API void SizerItem_SetProportion_(void* handle, int proportion)
{
    #if !defined(__WXMSW__)
    MarshalExceptions<void>([&](){
    #endif
        SizerItem::SetProportion(handle, proportion);
    #if !defined(__WXMSW__)
    });
    #endif
}

ALTERNET_UI_API int SizerItem_GetProportion_(void* handle)
{
    #if !defined(__WXMSW__)
    return MarshalExceptions<int>([&](){
    #endif
        return SizerItem::GetProportion(handle);
    #if !defined(__WXMSW__)
    });
    #endif
}

ALTERNET_UI_API void SizerItem_SetFlag_(void* handle, int flag)
{
    #if !defined(__WXMSW__)
    MarshalExceptions<void>([&](){
    #endif
        SizerItem::SetFlag(handle, flag);
    #if !defined(__WXMSW__)
    });
    #endif
}

ALTERNET_UI_API int SizerItem_GetFlag_(void* handle)
{
    #if !defined(__WXMSW__)
    return MarshalExceptions<int>([&](){
    #endif
        return SizerItem::GetFlag(handle);
    #if !defined(__WXMSW__)
    });
    #endif
}

ALTERNET_UI_API void SizerItem_SetBorder_(void* handle, int border)
{
    #if !defined(__WXMSW__)
    MarshalExceptions<void>([&](){
    #endif
        SizerItem::SetBorder(handle, border);
    #if !defined(__WXMSW__)
    });
    #endif
}

ALTERNET_UI_API int SizerItem_GetBorder_(void* handle)
{
    #if !defined(__WXMSW__)
    return MarshalExceptions<int>([&](){
    #endif
        return SizerItem::GetBorder(handle);
    #if !defined(__WXMSW__)
    });
    #endif
}

ALTERNET_UI_API void* SizerItem_GetWindow_(void* handle)
{
    #if !defined(__WXMSW__)
    return MarshalExceptions<void*>([&](){
    #endif
        return SizerItem::GetWindow(handle);
    #if !defined(__WXMSW__)
    });
    #endif
}

ALTERNET_UI_API void* SizerItem_GetSizer_(void* handle)
{
    #if !defined(__WXMSW__)
    return MarshalExceptions<void*>([&](){
    #endif
        return SizerItem::GetSizer(handle);
    #if !defined(__WXMSW__)
    });
    #endif
}

ALTERNET_UI_API SizeI_C SizerItem_GetSpacer_(void* handle)
{
    #if !defined(__WXMSW__)
    return MarshalExceptions<SizeI_C>([&](){
    #endif
        return SizerItem::GetSpacer(handle);
    #if !defined(__WXMSW__)
    });
    #endif
}

ALTERNET_UI_API c_bool SizerItem_IsShown_(void* handle)
{
    #if !defined(__WXMSW__)
    return MarshalExceptions<c_bool>([&](){
    #endif
        return SizerItem::IsShown(handle);
    #if !defined(__WXMSW__)
    });
    #endif
}

ALTERNET_UI_API void SizerItem_Show_(void* handle, c_bool show)
{
    #if !defined(__WXMSW__)
    MarshalExceptions<void>([&](){
    #endif
        SizerItem::Show(handle, show);
    #if !defined(__WXMSW__)
    });
    #endif
}

ALTERNET_UI_API void SizerItem_SetUserData_(void* handle, void* userData)
{
    #if !defined(__WXMSW__)
    MarshalExceptions<void>([&](){
    #endif
        SizerItem::SetUserData(handle, userData);
    #if !defined(__WXMSW__)
    });
    #endif
}

ALTERNET_UI_API void* SizerItem_GetUserData_(void* handle)
{
    #if !defined(__WXMSW__)
    return MarshalExceptions<void*>([&](){
    #endif
        return SizerItem::GetUserData(handle);
    #if !defined(__WXMSW__)
    });
    #endif
}

ALTERNET_UI_API PointI_C SizerItem_GetPosition_(void* handle)
{
    #if !defined(__WXMSW__)
    return MarshalExceptions<PointI_C>([&](){
    #endif
        return SizerItem::GetPosition(handle);
    #if !defined(__WXMSW__)
    });
    #endif
}

ALTERNET_UI_API c_bool SizerItem_InformFirstDirection_(void* handle, int direction, int size, int availableOtherDir)
{
    #if !defined(__WXMSW__)
    return MarshalExceptions<c_bool>([&](){
    #endif
        return SizerItem::InformFirstDirection(handle, direction, size, availableOtherDir);
    #if !defined(__WXMSW__)
    });
    #endif
}

ALTERNET_UI_API void SizerItem_AssignWindow_(void* handle, void* window)
{
    #if !defined(__WXMSW__)
    MarshalExceptions<void>([&](){
    #endif
        SizerItem::AssignWindow(handle, window);
    #if !defined(__WXMSW__)
    });
    #endif
}

ALTERNET_UI_API void SizerItem_AssignSizer_(void* handle, void* sizer)
{
    #if !defined(__WXMSW__)
    MarshalExceptions<void>([&](){
    #endif
        SizerItem::AssignSizer(handle, sizer);
    #if !defined(__WXMSW__)
    });
    #endif
}

ALTERNET_UI_API void SizerItem_AssignSpacer_(void* handle, int w, int h)
{
    #if !defined(__WXMSW__)
    MarshalExceptions<void>([&](){
    #endif
        SizerItem::AssignSpacer(handle, w, h);
    #if !defined(__WXMSW__)
    });
    #endif
}

