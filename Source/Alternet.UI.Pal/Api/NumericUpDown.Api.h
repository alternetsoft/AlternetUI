// <auto-generated>This code was generated by a tool, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2023.</auto-generated>

#pragma once

#include "NumericUpDown.h"
#include "ApiUtils.h"
#include "Exceptions.h"

using namespace Alternet::UI;

ALTERNET_UI_API NumericUpDown* NumericUpDown_Create_()
{
    return MarshalExceptions<NumericUpDown*>([&](){
            return new NumericUpDown();
        });
}

ALTERNET_UI_API int NumericUpDown_GetMinimum_(NumericUpDown* obj)
{
    return MarshalExceptions<int>([&](){
            return obj->GetMinimum();
        });
}

ALTERNET_UI_API void NumericUpDown_SetMinimum_(NumericUpDown* obj, int value)
{
    MarshalExceptions<void>([&](){
            obj->SetMinimum(value);
        });
}

ALTERNET_UI_API int NumericUpDown_GetMaximum_(NumericUpDown* obj)
{
    return MarshalExceptions<int>([&](){
            return obj->GetMaximum();
        });
}

ALTERNET_UI_API void NumericUpDown_SetMaximum_(NumericUpDown* obj, int value)
{
    MarshalExceptions<void>([&](){
            obj->SetMaximum(value);
        });
}

ALTERNET_UI_API int NumericUpDown_GetValue_(NumericUpDown* obj)
{
    return MarshalExceptions<int>([&](){
            return obj->GetValue();
        });
}

ALTERNET_UI_API void NumericUpDown_SetValue_(NumericUpDown* obj, int value)
{
    MarshalExceptions<void>([&](){
            obj->SetValue(value);
        });
}

ALTERNET_UI_API void NumericUpDown_SetEventCallback_(NumericUpDown::NumericUpDownEventCallbackType callback)
{
    NumericUpDown::SetEventCallback(callback);
}

