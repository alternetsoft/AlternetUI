// Auto generated code, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2021.

#pragma once

#include "DrawingContext.h"
#include "Image.h"
#include "ApiUtils.h"

using namespace Alternet::UI;

ALTERNET_UI_API void DrawingContext_FillRectangle(DrawingContext* obj, RectangleF rectangle, Color color)
{
    obj->FillRectangle(rectangle, color);
}

ALTERNET_UI_API void DrawingContext_DrawRectangle(DrawingContext* obj, RectangleF rectangle, Color color)
{
    obj->DrawRectangle(rectangle, color);
}

ALTERNET_UI_API void DrawingContext_DrawText(DrawingContext* obj, const char16_t* text, PointF origin, Color color)
{
    obj->DrawText(text, origin, color);
}

ALTERNET_UI_API void DrawingContext_DrawImage(DrawingContext* obj, Image* image, PointF origin)
{
    obj->DrawImage(image, origin);
}

ALTERNET_UI_API SizeF_C DrawingContext_MeasureText(DrawingContext* obj, const char16_t* text)
{
    return obj->MeasureText(text);
}

ALTERNET_UI_API void DrawingContext_PushTransform(DrawingContext* obj, SizeF translation)
{
    obj->PushTransform(translation);
}

ALTERNET_UI_API void DrawingContext_Pop(DrawingContext* obj)
{
    obj->Pop();
}

