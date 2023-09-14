// <auto-generated>This code was generated by a tool, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2023.</auto-generated>

#pragma once

#include "DateTimePicker.h"
#include "ApiUtils.h"
#include "Exceptions.h"

using namespace Alternet::UI;

ALTERNET_UI_API DateTimePicker* DateTimePicker_Create_()
{
    return new DateTimePicker();
}

ALTERNET_UI_API c_bool DateTimePicker_GetHasBorder_(DateTimePicker* obj)
{
    return obj->GetHasBorder();
}

ALTERNET_UI_API void DateTimePicker_SetHasBorder_(DateTimePicker* obj, c_bool value)
{
    obj->SetHasBorder(value);
}

ALTERNET_UI_API DateTime_C DateTimePicker_GetValue_(DateTimePicker* obj)
{
    return obj->GetValue();
}

ALTERNET_UI_API void DateTimePicker_SetValue_(DateTimePicker* obj, DateTime value)
{
    obj->SetValue(value);
}

ALTERNET_UI_API DateTime_C DateTimePicker_GetMinValue_(DateTimePicker* obj)
{
    return obj->GetMinValue();
}

ALTERNET_UI_API void DateTimePicker_SetMinValue_(DateTimePicker* obj, DateTime value)
{
    obj->SetMinValue(value);
}

ALTERNET_UI_API DateTime_C DateTimePicker_GetMaxValue_(DateTimePicker* obj)
{
    return obj->GetMaxValue();
}

ALTERNET_UI_API void DateTimePicker_SetMaxValue_(DateTimePicker* obj, DateTime value)
{
    obj->SetMaxValue(value);
}

ALTERNET_UI_API int DateTimePicker_GetValueKind_(DateTimePicker* obj)
{
    return obj->GetValueKind();
}

ALTERNET_UI_API void DateTimePicker_SetValueKind_(DateTimePicker* obj, int value)
{
    obj->SetValueKind(value);
}

ALTERNET_UI_API int DateTimePicker_GetPopupKind_(DateTimePicker* obj)
{
    return obj->GetPopupKind();
}

ALTERNET_UI_API void DateTimePicker_SetPopupKind_(DateTimePicker* obj, int value)
{
    obj->SetPopupKind(value);
}

ALTERNET_UI_API void DateTimePicker_SetRange_(DateTimePicker* obj, c_bool useMinValue, c_bool useMaxValue)
{
    obj->SetRange(useMinValue, useMaxValue);
}

ALTERNET_UI_API void DateTimePicker_SetEventCallback_(DateTimePicker::DateTimePickerEventCallbackType callback)
{
    DateTimePicker::SetEventCallback(callback);
}

