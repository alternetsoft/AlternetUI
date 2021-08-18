// Auto generated code, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2021.

#pragma once

#include "HatchBrush.h"
#include "ApiUtils.h"

using namespace Alternet::UI;

ALTERNET_UI_API HatchBrush* HatchBrush_Create_()
{
    return new HatchBrush();
}

ALTERNET_UI_API void HatchBrush_Initialize_(HatchBrush* obj, BrushHatchStyle style, Color color)
{
    obj->Initialize(style, color);
}

