// <auto-generated> DO NOT MODIFY MANUALLY. Copyright (c) 2023 AlterNET Software.</auto-generated>

#pragma once

#include "SolidBrush.h"
#include "ApiUtils.h"
#include "Exceptions.h"

using namespace Alternet::UI;

ALTERNET_UI_API SolidBrush* SolidBrush_Create_()
{
    return new SolidBrush();
}

ALTERNET_UI_API void SolidBrush_Initialize_(SolidBrush* obj, Color color)
{
    obj->Initialize(color);
}

