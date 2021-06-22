// Auto generated code, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2021.

#pragma once

#include "ProgressBar.h"
#include "ApiUtils.h"

using namespace Alternet::UI;

ALTERNET_UI_API ProgressBar* ProgressBar_Create()
{
    return new ProgressBar();
}

ALTERNET_UI_API int ProgressBar_GetMinimum(ProgressBar* obj)
{
    return obj->GetMinimum();
}

ALTERNET_UI_API void ProgressBar_SetMinimum(ProgressBar* obj, int value)
{
    obj->SetMinimum(value);
}

ALTERNET_UI_API int ProgressBar_GetMaximum(ProgressBar* obj)
{
    return obj->GetMaximum();
}

ALTERNET_UI_API void ProgressBar_SetMaximum(ProgressBar* obj, int value)
{
    obj->SetMaximum(value);
}

ALTERNET_UI_API int ProgressBar_GetValue(ProgressBar* obj)
{
    return obj->GetValue();
}

ALTERNET_UI_API void ProgressBar_SetValue(ProgressBar* obj, int value)
{
    obj->SetValue(value);
}

ALTERNET_UI_API c_bool ProgressBar_GetIsIndeterminate(ProgressBar* obj)
{
    return obj->GetIsIndeterminate();
}

ALTERNET_UI_API void ProgressBar_SetIsIndeterminate(ProgressBar* obj, c_bool value)
{
    obj->SetIsIndeterminate(value);
}

