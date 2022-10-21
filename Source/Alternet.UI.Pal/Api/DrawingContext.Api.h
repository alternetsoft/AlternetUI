// <auto-generated>This code was generated by a tool, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2022.</auto-generated>

#pragma once

#include "DrawingContext.h"
#include "TransformMatrix.h"
#include "Region.h"
#include "Image.h"
#include "Brush.h"
#include "Pen.h"
#include "GraphicsPath.h"
#include "Font.h"
#include "ApiUtils.h"
#include "Exceptions.h"

using namespace Alternet::UI;

ALTERNET_UI_API TransformMatrix* DrawingContext_GetTransform_(DrawingContext* obj)
{
    return MarshalExceptions<TransformMatrix*>([&](){
            return obj->GetTransform();
        });
}

ALTERNET_UI_API void DrawingContext_SetTransform_(DrawingContext* obj, TransformMatrix* value)
{
    MarshalExceptions<void>([&](){
            obj->SetTransform(value);
        });
}

ALTERNET_UI_API Region* DrawingContext_GetClip_(DrawingContext* obj)
{
    return MarshalExceptions<Region*>([&](){
            return obj->GetClip();
        });
}

ALTERNET_UI_API void DrawingContext_SetClip_(DrawingContext* obj, Region* value)
{
    MarshalExceptions<void>([&](){
            obj->SetClip(value);
        });
}

ALTERNET_UI_API DrawingContext* DrawingContext_FromImage_(Image* image)
{
    return MarshalExceptions<DrawingContext*>([&](){
            return DrawingContext::FromImage(image);
        });
}

ALTERNET_UI_API void DrawingContext_FillRectangle_(DrawingContext* obj, Brush* brush, Rect rectangle)
{
    MarshalExceptions<void>([&](){
            obj->FillRectangle(brush, rectangle);
        });
}

ALTERNET_UI_API void DrawingContext_DrawRectangle_(DrawingContext* obj, Pen* pen, Rect rectangle)
{
    MarshalExceptions<void>([&](){
            obj->DrawRectangle(pen, rectangle);
        });
}

ALTERNET_UI_API void DrawingContext_FillEllipse_(DrawingContext* obj, Brush* brush, Rect bounds)
{
    MarshalExceptions<void>([&](){
            obj->FillEllipse(brush, bounds);
        });
}

ALTERNET_UI_API void DrawingContext_DrawEllipse_(DrawingContext* obj, Pen* pen, Rect bounds)
{
    MarshalExceptions<void>([&](){
            obj->DrawEllipse(pen, bounds);
        });
}

ALTERNET_UI_API void DrawingContext_FloodFill_(DrawingContext* obj, Brush* brush, Point point)
{
    MarshalExceptions<void>([&](){
            obj->FloodFill(brush, point);
        });
}

ALTERNET_UI_API void DrawingContext_DrawPath_(DrawingContext* obj, Pen* pen, GraphicsPath* path)
{
    MarshalExceptions<void>([&](){
            obj->DrawPath(pen, path);
        });
}

ALTERNET_UI_API void DrawingContext_FillPath_(DrawingContext* obj, Brush* brush, GraphicsPath* path)
{
    MarshalExceptions<void>([&](){
            obj->FillPath(brush, path);
        });
}

ALTERNET_UI_API void DrawingContext_DrawTextAtPoint_(DrawingContext* obj, const char16_t* text, Point origin, Font* font, Brush* brush)
{
    MarshalExceptions<void>([&](){
            obj->DrawTextAtPoint(text, origin, font, brush);
        });
}

ALTERNET_UI_API void DrawingContext_DrawTextAtRect_(DrawingContext* obj, const char16_t* text, Rect bounds, Font* font, Brush* brush, TextHorizontalAlignment horizontalAlignment, TextVerticalAlignment verticalAlignment, TextTrimming trimming, TextWrapping wrapping)
{
    MarshalExceptions<void>([&](){
            obj->DrawTextAtRect(text, bounds, font, brush, horizontalAlignment, verticalAlignment, trimming, wrapping);
        });
}

ALTERNET_UI_API void DrawingContext_DrawImageAtPoint_(DrawingContext* obj, Image* image, Point origin)
{
    MarshalExceptions<void>([&](){
            obj->DrawImageAtPoint(image, origin);
        });
}

ALTERNET_UI_API void DrawingContext_DrawImageAtRect_(DrawingContext* obj, Image* image, Rect rect)
{
    MarshalExceptions<void>([&](){
            obj->DrawImageAtRect(image, rect);
        });
}

ALTERNET_UI_API Size_C DrawingContext_MeasureText_(DrawingContext* obj, const char16_t* text, Font* font, double maximumWidth, TextWrapping textWrapping)
{
    return MarshalExceptions<Size_C>([&](){
            return obj->MeasureText(text, font, maximumWidth, textWrapping);
        });
}

ALTERNET_UI_API void DrawingContext_Push_(DrawingContext* obj)
{
    MarshalExceptions<void>([&](){
            obj->Push();
        });
}

ALTERNET_UI_API void DrawingContext_Pop_(DrawingContext* obj)
{
    MarshalExceptions<void>([&](){
            obj->Pop();
        });
}

ALTERNET_UI_API void DrawingContext_DrawLine_(DrawingContext* obj, Pen* pen, Point a, Point b)
{
    MarshalExceptions<void>([&](){
            obj->DrawLine(pen, a, b);
        });
}

ALTERNET_UI_API void DrawingContext_DrawLines_(DrawingContext* obj, Pen* pen, Point* points, int pointsCount)
{
    MarshalExceptions<void>([&](){
            obj->DrawLines(pen, points, pointsCount);
        });
}

ALTERNET_UI_API void DrawingContext_DrawArc_(DrawingContext* obj, Pen* pen, Point center, double radius, double startAngle, double sweepAngle)
{
    MarshalExceptions<void>([&](){
            obj->DrawArc(pen, center, radius, startAngle, sweepAngle);
        });
}

ALTERNET_UI_API void DrawingContext_FillPie_(DrawingContext* obj, Brush* brush, Point center, double radius, double startAngle, double sweepAngle)
{
    MarshalExceptions<void>([&](){
            obj->FillPie(brush, center, radius, startAngle, sweepAngle);
        });
}

ALTERNET_UI_API void DrawingContext_DrawPie_(DrawingContext* obj, Pen* pen, Point center, double radius, double startAngle, double sweepAngle)
{
    MarshalExceptions<void>([&](){
            obj->DrawPie(pen, center, radius, startAngle, sweepAngle);
        });
}

ALTERNET_UI_API void DrawingContext_DrawBezier_(DrawingContext* obj, Pen* pen, Point startPoint, Point controlPoint1, Point controlPoint2, Point endPoint)
{
    MarshalExceptions<void>([&](){
            obj->DrawBezier(pen, startPoint, controlPoint1, controlPoint2, endPoint);
        });
}

ALTERNET_UI_API void DrawingContext_DrawBeziers_(DrawingContext* obj, Pen* pen, Point* points, int pointsCount)
{
    MarshalExceptions<void>([&](){
            obj->DrawBeziers(pen, points, pointsCount);
        });
}

ALTERNET_UI_API void DrawingContext_DrawCircle_(DrawingContext* obj, Pen* pen, Point center, double radius)
{
    MarshalExceptions<void>([&](){
            obj->DrawCircle(pen, center, radius);
        });
}

ALTERNET_UI_API void DrawingContext_FillCircle_(DrawingContext* obj, Brush* brush, Point center, double radius)
{
    MarshalExceptions<void>([&](){
            obj->FillCircle(brush, center, radius);
        });
}

ALTERNET_UI_API void DrawingContext_DrawRoundedRectangle_(DrawingContext* obj, Pen* pen, Rect rect, double cornerRadius)
{
    MarshalExceptions<void>([&](){
            obj->DrawRoundedRectangle(pen, rect, cornerRadius);
        });
}

ALTERNET_UI_API void DrawingContext_FillRoundedRectangle_(DrawingContext* obj, Brush* brush, Rect rect, double cornerRadius)
{
    MarshalExceptions<void>([&](){
            obj->FillRoundedRectangle(brush, rect, cornerRadius);
        });
}

ALTERNET_UI_API void DrawingContext_DrawPolygon_(DrawingContext* obj, Pen* pen, Point* points, int pointsCount)
{
    MarshalExceptions<void>([&](){
            obj->DrawPolygon(pen, points, pointsCount);
        });
}

ALTERNET_UI_API void DrawingContext_FillPolygon_(DrawingContext* obj, Brush* brush, Point* points, int pointsCount, FillMode fillMode)
{
    MarshalExceptions<void>([&](){
            obj->FillPolygon(brush, points, pointsCount, fillMode);
        });
}

ALTERNET_UI_API void DrawingContext_DrawRectangles_(DrawingContext* obj, Pen* pen, Rect* rects, int rectsCount)
{
    MarshalExceptions<void>([&](){
            obj->DrawRectangles(pen, rects, rectsCount);
        });
}

ALTERNET_UI_API void DrawingContext_FillRectangles_(DrawingContext* obj, Brush* brush, Rect* rects, int rectsCount)
{
    MarshalExceptions<void>([&](){
            obj->FillRectangles(brush, rects, rectsCount);
        });
}

