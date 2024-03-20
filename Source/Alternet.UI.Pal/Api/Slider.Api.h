// <auto-generated> DO NOT MODIFY MANUALLY. Copyright (c) 2024 AlterNET Software.</auto-generated>

#pragma once

#include "Slider.h"
#include "ApiUtils.h"
#include "Exceptions.h"

using namespace Alternet::UI;

ALTERNET_UI_API Slider* Slider_Create_()
{
    #if !defined(__WXMSW__)
    return MarshalExceptions<Slider*>([&](){
    #endif
        return new Slider();
    #if !defined(__WXMSW__)
    });
    #endif
}

ALTERNET_UI_API int Slider_GetMinimum_(Slider* obj)
{
    #if !defined(__WXMSW__)
    return MarshalExceptions<int>([&](){
    #endif
        return obj->GetMinimum();
    #if !defined(__WXMSW__)
    });
    #endif
}

ALTERNET_UI_API void Slider_SetMinimum_(Slider* obj, int value)
{
    #if !defined(__WXMSW__)
    MarshalExceptions<void>([&](){
    #endif
        obj->SetMinimum(value);
    #if !defined(__WXMSW__)
    });
    #endif
}

ALTERNET_UI_API int Slider_GetMaximum_(Slider* obj)
{
    #if !defined(__WXMSW__)
    return MarshalExceptions<int>([&](){
    #endif
        return obj->GetMaximum();
    #if !defined(__WXMSW__)
    });
    #endif
}

ALTERNET_UI_API void Slider_SetMaximum_(Slider* obj, int value)
{
    #if !defined(__WXMSW__)
    MarshalExceptions<void>([&](){
    #endif
        obj->SetMaximum(value);
    #if !defined(__WXMSW__)
    });
    #endif
}

ALTERNET_UI_API int Slider_GetValue_(Slider* obj)
{
    #if !defined(__WXMSW__)
    return MarshalExceptions<int>([&](){
    #endif
        return obj->GetValue();
    #if !defined(__WXMSW__)
    });
    #endif
}

ALTERNET_UI_API void Slider_SetValue_(Slider* obj, int value)
{
    #if !defined(__WXMSW__)
    MarshalExceptions<void>([&](){
    #endif
        obj->SetValue(value);
    #if !defined(__WXMSW__)
    });
    #endif
}

ALTERNET_UI_API int Slider_GetSmallChange_(Slider* obj)
{
    #if !defined(__WXMSW__)
    return MarshalExceptions<int>([&](){
    #endif
        return obj->GetSmallChange();
    #if !defined(__WXMSW__)
    });
    #endif
}

ALTERNET_UI_API void Slider_SetSmallChange_(Slider* obj, int value)
{
    #if !defined(__WXMSW__)
    MarshalExceptions<void>([&](){
    #endif
        obj->SetSmallChange(value);
    #if !defined(__WXMSW__)
    });
    #endif
}

ALTERNET_UI_API int Slider_GetLargeChange_(Slider* obj)
{
    #if !defined(__WXMSW__)
    return MarshalExceptions<int>([&](){
    #endif
        return obj->GetLargeChange();
    #if !defined(__WXMSW__)
    });
    #endif
}

ALTERNET_UI_API void Slider_SetLargeChange_(Slider* obj, int value)
{
    #if !defined(__WXMSW__)
    MarshalExceptions<void>([&](){
    #endif
        obj->SetLargeChange(value);
    #if !defined(__WXMSW__)
    });
    #endif
}

ALTERNET_UI_API int Slider_GetTickFrequency_(Slider* obj)
{
    #if !defined(__WXMSW__)
    return MarshalExceptions<int>([&](){
    #endif
        return obj->GetTickFrequency();
    #if !defined(__WXMSW__)
    });
    #endif
}

ALTERNET_UI_API void Slider_SetTickFrequency_(Slider* obj, int value)
{
    #if !defined(__WXMSW__)
    MarshalExceptions<void>([&](){
    #endif
        obj->SetTickFrequency(value);
    #if !defined(__WXMSW__)
    });
    #endif
}

ALTERNET_UI_API SliderOrientation Slider_GetOrientation_(Slider* obj)
{
    #if !defined(__WXMSW__)
    return MarshalExceptions<SliderOrientation>([&](){
    #endif
        return obj->GetOrientation();
    #if !defined(__WXMSW__)
    });
    #endif
}

ALTERNET_UI_API void Slider_SetOrientation_(Slider* obj, SliderOrientation value)
{
    #if !defined(__WXMSW__)
    MarshalExceptions<void>([&](){
    #endif
        obj->SetOrientation(value);
    #if !defined(__WXMSW__)
    });
    #endif
}

ALTERNET_UI_API SliderTickStyle Slider_GetTickStyle_(Slider* obj)
{
    #if !defined(__WXMSW__)
    return MarshalExceptions<SliderTickStyle>([&](){
    #endif
        return obj->GetTickStyle();
    #if !defined(__WXMSW__)
    });
    #endif
}

ALTERNET_UI_API void Slider_SetTickStyle_(Slider* obj, SliderTickStyle value)
{
    #if !defined(__WXMSW__)
    MarshalExceptions<void>([&](){
    #endif
        obj->SetTickStyle(value);
    #if !defined(__WXMSW__)
    });
    #endif
}

ALTERNET_UI_API void Slider_ClearTicks_(Slider* obj)
{
    #if !defined(__WXMSW__)
    MarshalExceptions<void>([&](){
    #endif
        obj->ClearTicks();
    #if !defined(__WXMSW__)
    });
    #endif
}

ALTERNET_UI_API void Slider_SetEventCallback_(Slider::SliderEventCallbackType callback)
{
    Slider::SetEventCallback(callback);
}

