// Auto generated code, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2021.

#pragma once

#include "Slider.h"
#include "ApiUtils.h"

using namespace Alternet::UI;

ALTERNET_UI_API Slider* Slider_Create()
{
    return new Slider();
}

ALTERNET_UI_API int Slider_GetMinimum(Slider* obj)
{
    return obj->GetMinimum();
}

ALTERNET_UI_API void Slider_SetMinimum(Slider* obj, int value)
{
    obj->SetMinimum(value);
}

ALTERNET_UI_API int Slider_GetMaximum(Slider* obj)
{
    return obj->GetMaximum();
}

ALTERNET_UI_API void Slider_SetMaximum(Slider* obj, int value)
{
    obj->SetMaximum(value);
}

ALTERNET_UI_API int Slider_GetValue(Slider* obj)
{
    return obj->GetValue();
}

ALTERNET_UI_API void Slider_SetValue(Slider* obj, int value)
{
    obj->SetValue(value);
}

ALTERNET_UI_API int Slider_GetSmallChange(Slider* obj)
{
    return obj->GetSmallChange();
}

ALTERNET_UI_API void Slider_SetSmallChange(Slider* obj, int value)
{
    obj->SetSmallChange(value);
}

ALTERNET_UI_API int Slider_GetLargeChange(Slider* obj)
{
    return obj->GetLargeChange();
}

ALTERNET_UI_API void Slider_SetLargeChange(Slider* obj, int value)
{
    obj->SetLargeChange(value);
}

ALTERNET_UI_API int Slider_GetTickFrequency(Slider* obj)
{
    return obj->GetTickFrequency();
}

ALTERNET_UI_API void Slider_SetTickFrequency(Slider* obj, int value)
{
    obj->SetTickFrequency(value);
}

ALTERNET_UI_API void Slider_SetEventCallback(Slider::SliderEventCallbackType callback)
{
    Slider::SetEventCallback(callback);
}

