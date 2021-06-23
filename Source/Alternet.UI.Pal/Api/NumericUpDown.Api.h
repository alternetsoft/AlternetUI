// Auto generated code, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2021.

#pragma once

#include "NumericUpDown.h"
#include "ApiUtils.h"

using namespace Alternet::UI;

ALTERNET_UI_API NumericUpDown* NumericUpDown_Create()
{
    return new NumericUpDown();
}

ALTERNET_UI_API int NumericUpDown_GetMinimum(NumericUpDown* obj)
{
    return obj->GetMinimum();
}

ALTERNET_UI_API void NumericUpDown_SetMinimum(NumericUpDown* obj, int value)
{
    obj->SetMinimum(value);
}

ALTERNET_UI_API int NumericUpDown_GetMaximum(NumericUpDown* obj)
{
    return obj->GetMaximum();
}

ALTERNET_UI_API void NumericUpDown_SetMaximum(NumericUpDown* obj, int value)
{
    obj->SetMaximum(value);
}

ALTERNET_UI_API int NumericUpDown_GetValue(NumericUpDown* obj)
{
    return obj->GetValue();
}

ALTERNET_UI_API void NumericUpDown_SetValue(NumericUpDown* obj, int value)
{
    obj->SetValue(value);
}

ALTERNET_UI_API void NumericUpDown_SetEventCallback(NumericUpDown::NumericUpDownEventCallbackType callback)
{
    NumericUpDown::SetEventCallback(callback);
}

