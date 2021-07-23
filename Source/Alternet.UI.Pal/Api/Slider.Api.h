// Auto generated code, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2021.

#pragma once

#include "Slider.h"
#include "ApiUtils.h"

using namespace Alternet::UI;

ALTERNET_UI_API Slider* Slider_Create_()
{
    return new Slider();
}

ALTERNET_UI_API int Slider_GetMinimum_(Slider* obj)
{
    return obj->GetMinimum();
}

ALTERNET_UI_API void Slider_SetMinimum_(Slider* obj, int value)
{
    obj->SetMinimum(value);
}

ALTERNET_UI_API int Slider_GetMaximum_(Slider* obj)
{
    return obj->GetMaximum();
}

ALTERNET_UI_API void Slider_SetMaximum_(Slider* obj, int value)
{
    obj->SetMaximum(value);
}

ALTERNET_UI_API int Slider_GetValue_(Slider* obj)
{
    return obj->GetValue();
}

ALTERNET_UI_API void Slider_SetValue_(Slider* obj, int value)
{
    obj->SetValue(value);
}

ALTERNET_UI_API int Slider_GetSmallChange_(Slider* obj)
{
    return obj->GetSmallChange();
}

ALTERNET_UI_API void Slider_SetSmallChange_(Slider* obj, int value)
{
    obj->SetSmallChange(value);
}

ALTERNET_UI_API int Slider_GetLargeChange_(Slider* obj)
{
    return obj->GetLargeChange();
}

ALTERNET_UI_API void Slider_SetLargeChange_(Slider* obj, int value)
{
    obj->SetLargeChange(value);
}

ALTERNET_UI_API int Slider_GetTickFrequency_(Slider* obj)
{
    return obj->GetTickFrequency();
}

ALTERNET_UI_API void Slider_SetTickFrequency_(Slider* obj, int value)
{
    obj->SetTickFrequency(value);
}

ALTERNET_UI_API void Slider_SetEventCallback_(Slider::SliderEventCallbackType callback)
{
    Slider::SetEventCallback(callback);
}

