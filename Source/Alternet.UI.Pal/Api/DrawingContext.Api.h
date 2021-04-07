// Auto generated code, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2021.

#pragma once

#include "DrawingContext.h"
#include "ApiUtils.h"

using namespace Alternet::UI;

ALTERNET_UI_API DrawingContext* DrawingContext_Create()
{
    return new DrawingContext();
}

ALTERNET_UI_API void DrawingContext_Destroy(DrawingContext* obj)
{
    delete obj;
}

ALTERNET_UI_API void DrawingContext_FillRectangle(DrawingContext* obj, RectangleF rectangle, Color color)
{
    obj->FillRectangle(rectangle, color);
}

