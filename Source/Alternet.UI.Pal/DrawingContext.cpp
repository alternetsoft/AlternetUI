#include "DrawingContext.h"
#include "SolidBrush.h"
#include "FloodFill.h"
#include "TextPainter.h"
#include "GenericImage.h"

namespace Alternet::UI
{
    DrawingContext* DrawingContext::CreateMemoryDC(double scaleFactor)
    {
        /*
        wxMemoryDC memoryDC;
        auto pp1 = memoryDC.GetPPI().x;

        double x, y;
        double x1, y1;

        memoryDC.GetUserScale(&x,&y);
        memoryDC.GetLogicalScale(&x1, &y1);

        wxMemoryDC memoryDC2;
        memoryDC2.SetLogicalScale(2, 2);
        auto pp2 = memoryDC2.GetPPI().x;
        */

        auto bitmap = wxBitmap(10, 10);
        bitmap.SetScaleFactor(scaleFactor);
        auto memoryDC = new wxMemoryDC(bitmap);
        auto ppi = memoryDC->GetPPI();
        return new DrawingContext(memoryDC);
    }

    DrawingContext* DrawingContext::CreateMemoryDCFromImage(Image* image)
    {
        wxBitmap bitmap = image->GetBitmap();
        auto memoryDC = new wxMemoryDC(bitmap);
        return new DrawingContext(memoryDC);
    }

    SizeI DrawingContext::GetDpi()
    {
        UseDC();
        return _dc->GetPPI();
    }

    void* DrawingContext::GetWxWidgetDC()
    {
        return _dc;
    }

    void DrawingContext::DrawRotatedText(const string& text, const PointD& location, Font* font,
        const Color& foreColor, const Color& backColor, double angle)
    {
        auto point = fromDip(location, _dc->GetWindow());
        DrawRotatedTextI(text, point, font, foreColor, backColor, angle);
    }

    void DrawingContext::DrawRotatedTextI(const string& text, const PointI& location, Font* font,
        const Color& foreColor, const Color& backColor, double angle)
    {
        bool useBackColor = !backColor.IsEmpty();

        UseDC();

        auto window = DrawingContext::GetWindow(_dc);

        auto backMode = _dc->GetBackgroundMode();

        if (useBackColor)
        {
            _dc->SetBackgroundMode(wxBRUSHSTYLE_SOLID);
        }
        else
        {
            _dc->SetBackgroundMode(wxBRUSHSTYLE_TRANSPARENT);
        }

        auto& oldTextForeground = _dc->GetTextForeground();
        _dc->SetTextForeground(foreColor);

        auto& oldFont = _dc->GetFont();
        _dc->SetFont(font->GetWxFont());

        auto point = location;

        if (useBackColor)
        {
            auto& oldTextBackground = _dc->GetTextBackground();
            _dc->SetTextBackground(backColor);
            _dc->DrawRotatedText(wxStr(text), point, angle);
            _dc->SetTextBackground(oldTextBackground);
        }
        else
            _dc->DrawRotatedText(wxStr(text), point, angle);

        _dc->SetTextForeground(oldTextForeground);
        _dc->SetFont(oldFont);

        _dc->SetBackgroundMode(backMode);
    }

    Image* DrawingContext::GetAsBitmapI(const RectI& subrect)
    {
        UseDC();

        if (subrect.IsZero())
        {
            auto bitmap = _dc->GetAsBitmap();
            auto result = new Image();
            result->_bitmap = bitmap;
            return result;
        }
        else
        {
            wxRect wxr = subrect;
            auto bitmap = _dc->GetAsBitmap(&wxr);
            auto result = new Image();
            result->_bitmap = bitmap;
            return result;
        }
    }

    bool DrawingContext::Blit(const PointD& destPt, const SizeD& sz,
        DrawingContext* source, const PointD& srcPt, int rop,
        bool useMask, const PointD& srcPtMask)
    {
        UseDC();

        auto destPtI = fromDip(destPt, _dc->GetWindow());
        auto szI = fromDip(sz, _dc->GetWindow());
        auto srcPtI = fromDip(srcPt, _dc->GetWindow());
        auto srcPtMaskI = fromDip(srcPtMask, _dc->GetWindow());

        return _dc->Blit(destPtI, szI, source->GetDC(), srcPtI, (wxRasterOperationMode)rop,
            useMask, srcPtMaskI);
    }

    bool DrawingContext::BlitI(const PointI& destPt, const SizeI& sz,
        DrawingContext* source, const PointI& srcPt, int rop,
        bool useMask, const PointI& srcPtMask)
    {
        UseDC();

        return _dc->Blit(destPt, sz, source->GetDC(), srcPt, (wxRasterOperationMode)rop,
            useMask, srcPtMask);
    }

    bool DrawingContext::StretchBlit(const PointD& dstPt, const SizeD& dstSize,
        DrawingContext* source, const PointD& srcPt, const SizeD& srcSize,
        int rop, bool useMask, const PointD& srcMaskPt)
    {
        UseDC();

        auto dstPtI = fromDip(dstPt, _dc->GetWindow());
        auto srcSizeI = fromDip(srcSize, _dc->GetWindow());
        auto srcPtI = fromDip(srcPt, _dc->GetWindow());
        auto srcMaskPtI = fromDip(srcMaskPt, _dc->GetWindow());
        auto dstSizeI = fromDip(dstSize, _dc->GetWindow());

        return _dc->StretchBlit(dstPtI, dstSizeI, source->GetDC(), srcPtI, srcSizeI,
            (wxRasterOperationMode)rop, useMask, srcMaskPtI);
    }

    bool DrawingContext::StretchBlitI(const PointI& dstPt, const SizeI& dstSize,
        DrawingContext* source, const PointI& srcPt, const SizeI& srcSize,
        int rop, bool useMask, const PointI& srcMaskPt)
    {
        UseDC();

        return _dc->StretchBlit(dstPt, dstSize, source->GetDC(), srcPt, srcSize,
            (wxRasterOperationMode)rop, useMask, srcMaskPt);
    }

    void DrawingContext::ImageFromDrawingContext(Image * image,
        int width, int height, DrawingContext* dc)
    {
        auto wxdc = dc->GetDC();
        image->_bitmap = wxBitmap(width, height, *wxdc);
    }

    void DrawingContext::ImageFromGenericImageDC(Image* image, void* source, DrawingContext* dc)
    {
        auto wxdc = dc->GetDC();
        image->_bitmap = wxBitmap(((GenericImage*)source)->_image, *wxdc);
    }

    bool DrawingContext::GetIsOk()
    {
        return _dc->IsOk();
    }

    void* DrawingContext::GetHandle()
    {
        return _dc->GetHandle();
    }

    DrawingContext::DrawingContext(wxDC* dc) : _dc(dc)
    {
        assert(_dc);
        _graphicsContext = wxGraphicsContext::CreateFromUnknownDC(*_dc);
    }

    InterpolationMode DrawingContext::GetInterpolationMode()
    {
        return _interpolationMode;
    }

    void DrawingContext::SetInterpolationMode(InterpolationMode value)
    {
        _interpolationMode = value;
    }

    DrawingContext::~DrawingContext()
    {
        wxDELETE(_graphicsContext);
        
        if (!_doNotDeleteDC)
            wxDELETE(_dc);
    }

    void DrawingContext::SetDoNotDeleteDC(bool value)
    {
        _doNotDeleteDC = value;
    }

    void DrawingContext::SetIsPrinterDC(bool value)
    {
        _isPrinterDC = value;
    }

    wxGraphicsContext* DrawingContext::GetGraphicsContext()
    {
        return _graphicsContext;
    }

    wxDC* DrawingContext::GetDC()
    {
        return _dc;
    }

    /*static*/ wxWindow* DrawingContext::GetWindow(wxDC* dc)
    {
        auto window = dc->GetWindow();
        if (window == nullptr)
            return ParkingWindow::GetWindow();
        else
            return window;
    }

    Region* DrawingContext::GetClip()
    {
        if (_clip == nullptr)
            return nullptr;

        _clip->AddRef();
        return _clip;
    }

    void DrawingContext::DestroyClippingRegion()
    {
        if (_clip != nullptr)
        {
            _clip->Release();
            _clip = nullptr;
        }
        _dc->DestroyClippingRegion();
        _graphicsContext->ResetClip();
    }

    void DrawingContext::SetClippingRegion(const Rect& rect)
    {
        auto bounds = fromDip(rect, _dc->GetWindow());
        _dc->SetClippingRegion(bounds);
    }

    Rect DrawingContext::GetClippingBox()
    {
        wxRect rect;
        auto result = _dc->GetClippingBox(rect);
        if (result)
            return rect;
        else
            return Rect();
    }

    void DrawingContext::SetClip(Region* value)
    {
        DestroyClippingRegion();
        _clip = value;
        
        if (_clip != nullptr)
        {
            _clip->AddRef();
            _dc->SetDeviceClippingRegion(_clip->GetRegion());
            _graphicsContext->Clip(_clip->GetRegion());
        }
    }

    void DrawingContext::DrawArc(Pen* pen, const Point& center, double radius, double startAngle,
        double sweepAngle)
    {
        auto path = new GraphicsPath(_dc, _graphicsContext);

        path->AddArc(center, radius, startAngle, sweepAngle);
        DrawPath(pen, path);

        path->Release();
    }

    void DrawingContext::FillPie(Brush* brush, const Point& center, double radius, double startAngle,
        double sweepAngle)
    {
        auto path = new GraphicsPath(_dc, _graphicsContext);

        path->AddArc(center, radius, startAngle, sweepAngle);
        path->AddLineTo(center);
        path->CloseFigure();
        FillPath(brush, path);

        path->Release();
    }

    void DrawingContext::DrawPie(Pen* pen, const Point& center, double radius, double startAngle,
        double sweepAngle)
    {
        auto path = new GraphicsPath(_dc, _graphicsContext);

        path->AddArc(center, radius, startAngle, sweepAngle);
        path->AddLineTo(center);
        path->CloseFigure();
        DrawPath(pen, path);

        path->Release();
    }

    void DrawingContext::Pie(Pen* pen, Brush* brush, const Point& center, double radius, double startAngle,
        double sweepAngle)
    {
        auto path = new GraphicsPath(_dc, _graphicsContext);
        path->AddArc(center, radius, startAngle, sweepAngle);
        path->AddLineTo(center);
        path->CloseFigure();
        Path(pen, brush, path);
        path->Release();
    }

    void DrawingContext::DrawBezier(Pen* pen, const Point& startPoint, const Point& controlPoint1,
        const Point& controlPoint2, const Point& endPoint)
    {
        auto path = new GraphicsPath(_dc, _graphicsContext);

        path->AddBezier(startPoint, controlPoint1, controlPoint2, endPoint);
        DrawPath(pen, path);

        path->Release();
    }

    void DrawingContext::DrawBeziers(Pen* pen, Point* points, int pointsCount)
    {
        auto path = new GraphicsPath(_dc, _graphicsContext);

        path->StartFigure(points[0]);

        for (int i = 1; i <= pointsCount - 3; i += 3)
        {
            path->AddBezierTo(points[i], points[i + 1], points[i + 2]);
        }
        
        DrawPath(pen, path);

        path->Release();
    }

    void DrawingContext::DrawCircle(Pen* pen, const Point& center, double radius)
    {
        auto diameter = radius * 2;
        DrawEllipse(pen, Rect(center - Size(radius, radius), Size(diameter, diameter)));
    }

    void DrawingContext::FillCircle(Brush* brush, const Point& center, double radius)
    {
        auto diameter = radius * 2;
        FillEllipse(brush, Rect(center - Size(radius, radius), Size(diameter, diameter)));
    }

    void DrawingContext::Circle(Pen* pen, Brush* brush, const Point& center, double radius)
    {
        auto diameter = radius * 2;
        Ellipse(pen, brush, Rect(center - Size(radius, radius), Size(diameter, diameter)));
    }

    void DrawingContext::RoundedRectangle(Pen* pen, Brush* brush, const Rect& rect, double cornerRadius)
    {
        UseGC();
        _graphicsContext->SetPen(pen->GetWxPen());
        _graphicsContext->SetBrush(GetGraphicsBrush(brush, wxPoint2DDouble(rect.X, rect.Y)));
        auto r = fromDipF(rect, _dc->GetWindow());
        _graphicsContext->DrawRoundedRectangle(r.X, r.Y, r.Width, r.Height, cornerRadius);
    }

    void DrawingContext::DrawRoundedRectangle(Pen* pen, const Rect& rect, double cornerRadius)
    {
        UseGC();
        _graphicsContext->SetPen(pen->GetWxPen());
        _graphicsContext->SetBrush(*wxTRANSPARENT_BRUSH);
        auto r = fromDipF(rect, _dc->GetWindow());
        _graphicsContext->DrawRoundedRectangle(r.X, r.Y, r.Width, r.Height, cornerRadius);
    }

    void DrawingContext::FillRoundedRectangle(Brush* brush, const Rect& rect, double cornerRadius)
    {
        UseGC();
        _graphicsContext->SetPen(*wxTRANSPARENT_PEN);
        _graphicsContext->SetBrush(GetGraphicsBrush(brush, wxPoint2DDouble(rect.X, rect.Y)));
        auto r = fromDipF(rect, _dc->GetWindow());
        _graphicsContext->DrawRoundedRectangle(r.X, r.Y, r.Width, r.Height, cornerRadius);
    }

    void DrawingContext::DrawPath(Pen* pen, GraphicsPath* path)
    {
        UseGC();
        _graphicsContext->SetPen(pen->GetWxPen());
        _graphicsContext->StrokePath(path->GetPath());
    }

    void DrawingContext::FillPath(Brush* brush, GraphicsPath* path)
    {
        UseGC();

        auto bounds = fromDipF(path->GetBounds(), _dc->GetWindow());

        _graphicsContext->SetPen(*wxTRANSPARENT_PEN);
        _graphicsContext->SetBrush(GetGraphicsBrush(brush, wxPoint2DDouble(bounds.X, bounds.Y)));

        _graphicsContext->FillPath(path->GetPath(), path->GetWxFillMode());
    }

    void DrawingContext::Path(Pen* pen, Brush* brush, GraphicsPath* path)
    {
        UseGC();

        auto bounds = fromDipF(path->GetBounds(), _dc->GetWindow());

        _graphicsContext->SetPen(pen->GetWxPen());
        _graphicsContext->SetBrush(GetGraphicsBrush(brush, wxPoint2DDouble(bounds.X, bounds.Y)));

        _graphicsContext->DrawPath(path->GetPath(), path->GetWxFillMode());
    }

    void DrawingContext::DrawPolygon(Pen* pen, Point* points, int pointsCount)
    {
        if (NeedToUseDC())
        {
            UseDC();

            if (pointsCount <= 2)
                return;

            auto& oldPen = _dc->GetPen();

            _dc->SetPen(pen->GetWxPen());

            auto window = _dc->GetWindow();

            std::vector<wxPoint> wxPoints(pointsCount);
            for (int i = 0; i < pointsCount; i++)
                wxPoints[i] = fromDip(points[i], window);

            _dc->DrawPolygon(pointsCount, &wxPoints[0]);

            _dc->SetPen(oldPen);
        }
        else
        {
            auto path = new GraphicsPath(_dc, _graphicsContext);

            path->AddLines(points, pointsCount);
            path->CloseFigure();
            DrawPath(pen, path);

            path->Release();
        }
    }

    void DrawingContext::FillPolygon(Brush* brush, Point* points, int pointsCount, FillMode fillMode)
    {
        auto path = new GraphicsPath(_dc, _graphicsContext);

        path->AddLines(points, pointsCount);
        path->CloseFigure();
        FillPath(brush, path);

        path->Release();
    }

    void DrawingContext::Polygon(Pen* pen, Brush* brush, Point* points,
        int pointsCount, FillMode fillMode)
    {
        auto path = new GraphicsPath(_dc, _graphicsContext);

        path->AddLines(points, pointsCount);
        path->CloseFigure();
        Path(pen, brush, path);

        path->Release();
    }

    void DrawingContext::DrawRectangles(Pen* pen, Rect* rects, int rectsCount)
    {
        for (int i = 0; i < rectsCount; i++)
            DrawRectangle(pen, rects[i]);
    }

    void DrawingContext::FillRectangles(Brush* brush, Rect* rects, int rectsCount)
    {
        for (int i = 0; i < rectsCount; i++)
            FillRectangle(brush, rects[i]);
    }

    /*static*/ DrawingContext* DrawingContext::FromImage(Image* image)
    {
        auto bitmap = image->GetBitmap();
        auto dc = new wxMemoryDC(bitmap);
        image->SetBitmap(bitmap); // wxMemoryDC "unshares" bitmap under the hood. So need to reassign it back.
        return new DrawingContext(dc);
    }

    /*static*/ DrawingContext* DrawingContext::FromScreen()
    {
        return new DrawingContext(new wxScreenDC());
    }

    void DrawingContext::DrawImageAtPoint(Image* image, const Point& origin, bool useMask)
    {
        wxBitmap bitmap = image->GetBitmap();
        auto window = _dc->GetWindow();

        auto pt = fromDip(origin, _dc->GetWindow());

        if (NeedToUseDC())
        {
            UseDC();
            _dc->DrawBitmap(bitmap, pt, useMask);
        }
        else
        {
            UseGC();
            auto wxr = wxRect(pt, image->GetPixelSize());
            _graphicsContext->DrawBitmap(bitmap, wxr.x, wxr.y, wxr.width, wxr.height);
        }
    }

    void DrawingContext::DrawImageAtRect(Image* image, const Rect& destinationRect, bool useMask)
    {
        UseGC();

        wxBitmap bitmap = image->GetBitmap();
        auto destRect = fromDip(destinationRect, _dc->GetWindow());

        auto oldInterpolationQuality = _graphicsContext->GetInterpolationQuality();
        _graphicsContext->SetInterpolationQuality(GetInterpolationQuality(_interpolationMode));

        _graphicsContext->DrawBitmap(
            bitmap,
            destRect.x,
            destRect.y,
            destRect.width,
            destRect.height);

        _graphicsContext->SetInterpolationQuality(oldInterpolationQuality);
    }

    void DrawingContext::DrawImagePortionAtRect(Image* image, const Rect& destinationRect,
        const Rect& sourceRect)
    {
        auto sourceRectI = fromDipI(sourceRect, _dc->GetWindow());
        auto destinationRectI = fromDipI(destinationRect, _dc->GetWindow());
        DrawImagePortionAtPixelRect(image, destinationRectI, sourceRectI);
    }

    void DrawingContext::DrawImagePortionAtPixelRect(Image* image, const RectI& destinationRect,
        const RectI& sourceRect)
    {
        wxRect wxSourceRect = sourceRect;
        wxRect wxDestinationRect = destinationRect;

        wxBitmap bitmap = image->GetBitmap();

        wxMemoryDC sourceBitmapDC(bitmap);

        if (wxSourceRect.GetSize() == wxDestinationRect.GetSize())
        {
            UseDC();
            _dc->Blit(
                wxDestinationRect.GetLeftTop(),
                wxDestinationRect.GetSize(),
                &sourceBitmapDC,
                wxSourceRect.GetLeftTop());
            return;
        }

        if (_interpolationMode == InterpolationMode::None)
        {
            UseDC();

            _dc->StretchBlit(
                wxDestinationRect.GetLeftTop(),
                wxDestinationRect.GetSize(),
                &sourceBitmapDC,
                wxSourceRect.GetLeftTop(),
                wxSourceRect.GetSize());
        }
        else
        {
            UseGC();

            wxMemoryDC newDC, oldDC;
            oldDC.SelectObject(bitmap);
            wxBitmap tempBitmap(wxSourceRect.width, wxSourceRect.height);
            newDC.SelectObject(tempBitmap);
            newDC.Blit(0, 0, wxSourceRect.width, wxSourceRect.height, &oldDC, wxSourceRect.x,
                wxSourceRect.y);
            oldDC.SelectObject(wxNullBitmap);
            newDC.SelectObject(wxNullBitmap);

            auto oldInterpolationQuality = _graphicsContext->GetInterpolationQuality();
            _graphicsContext->SetInterpolationQuality(GetInterpolationQuality(_interpolationMode));

            _graphicsContext->DrawBitmap(
                tempBitmap,
                wxDestinationRect.x,
                wxDestinationRect.y,
                wxDestinationRect.width,
                wxDestinationRect.height);

            _graphicsContext->SetInterpolationQuality(oldInterpolationQuality);
        }
    }

    void DrawingContext::SetPixel(const PointD& p, Pen* pen)
    {
        UseDC();
        auto pixelPoint = fromDip(p, _dc->GetWindow());
        
        auto& oldPen = _dc->GetPen();
        _dc->SetPen(pen->GetWxPen());
        _dc->DrawPoint(pixelPoint);
        _dc->SetPen(oldPen);
    }

    Color DrawingContext::GetPixel(const PointD& p)
    {
        UseDC();
        auto pixelPoint = fromDip(p, _dc->GetWindow());
        wxColor seedColor;
        if (!_dc->GetPixel(pixelPoint, &seedColor))
            return seedColor;
        return seedColor;
    }

    void DrawingContext::FloodFill(Brush* fillBrush, const Point& point)
    {
        UseDC();

        auto fillSolidBrush = dynamic_cast<SolidBrush*>(fillBrush);
        if (fillSolidBrush == nullptr)
        {
            throwExInvalidArg(fillBrush, u"Only SolidBrush objects are supported");
        }

        auto& oldBrush = _dc->GetBrush();
        _dc->SetBrush(fillSolidBrush->GetWxBrush());

        auto pixelPoint = fromDip(point, _dc->GetWindow());

#ifdef __WXOSX_COCOA__
        // wxDC::FloodFill is not implemented on macOS.
        Alternet::UI::FloodFill(_dc, pixelPoint, fillSolidBrush->GetWxBrush().GetColour());
#else
        wxColor seedColor;
        if (!_dc->GetPixel(pixelPoint, &seedColor))
            return;

        _dc->FloodFill(pixelPoint, seedColor, wxFLOOD_SURFACE);
#endif

        _dc->SetBrush(oldBrush);
    }

    void DrawingContext::SetTransformValues(
        double m11, double m12, double m21, double m22, double dx, double dy)
    {
        wxMatrix2D m(m11, m12, m21, m22);
        wxPoint2DDouble t(dx, dy);

        wxAffineMatrix2D matrix;
        matrix.Set(m, t);

        _currentTransform = matrix;
        /*_currentTranslation = wxPoint((int)t.m_x, (int)t.m_y);*/
        _nonIdentityTransformSet = !_currentTransform.IsIdentity();

        // Setting transform on DC and GC at the same time doesn't work.
        // So apply them before every operation on by one if needed.

        _dc->ResetTransformMatrix();
        _graphicsContext->SetTransform(_graphicsContext->CreateMatrix());
    }

    void DrawingContext::ApplyTransform(bool useDC)
    {
        if (!_nonIdentityTransformSet)
            return;

        if (useDC)
        {
            _dc->SetTransformMatrix(_currentTransform);
            _graphicsContext->SetTransform(_graphicsContext->CreateMatrix());
        }
        else
        {
            _dc->ResetTransformMatrix();
            _graphicsContext->SetTransform(_graphicsContext->CreateMatrix(_currentTransform));
        }
    }

    void DrawingContext::UseDC()
    {
        ApplyTransform(/*useDC:*/true);
    }

    void DrawingContext::UseGC()
    {
        ApplyTransform(/*useDC:*/false);
    }

    bool DrawingContext::NeedToUseDC()
    {
#ifdef __WXOSX_MAC__
        return _isPrinterDC;
#else
        return false;
#endif
    }

    /*static*/ wxInterpolationQuality DrawingContext::GetInterpolationQuality(InterpolationMode mode)
    {
        switch (mode)
        {
        case InterpolationMode::None:
            return wxINTERPOLATION_NONE;
        case InterpolationMode::LowQuality:
            return wxINTERPOLATION_FAST;
        case InterpolationMode::MediumQuality:
            return wxINTERPOLATION_GOOD;
        case InterpolationMode::HighQuality:
            return wxINTERPOLATION_BEST;
        default:
            throwExNoInfo;
        }
    }

    void DrawingContext::Rectangle(Pen* pen, Brush* brush, const Rect& rectangle)
    {
        if (NeedToUseDC())
        {
            UseDC();

            auto& oldPen = _dc->GetPen();
            auto& oldBrush = _dc->GetBrush();

            _dc->SetPen(pen->GetWxPen());
            _dc->SetBrush(brush->GetWxBrush());

            _dc->DrawRectangle(
                fromDip(
                    Rect(
                        rectangle.X,
                        rectangle.Y,
                        rectangle.Width,
                        rectangle.Height),
                    _dc->GetWindow()));

            _dc->SetPen(oldPen);
            _dc->SetBrush(oldBrush);
        }
        else
        {
            UseGC();

            auto rect = fromDipF(rectangle, _dc->GetWindow());

            _graphicsContext->SetPen(pen->GetWxPen());
            _graphicsContext->SetBrush(GetGraphicsBrush(brush, wxPoint2DDouble(rect.X, rect.Y)));

            _graphicsContext->DrawRectangle(rect.X, rect.Y, rect.Width, rect.Height);
        }
    }

    void DrawingContext::FillRectangle(Brush* brush, const Rect& rectangle)
    {
        auto r = fromDip(
            Rect(
                rectangle.X,
                rectangle.Y,
                rectangle.Width,
                rectangle.Height),
            _dc->GetWindow());
        FillRectangleI(brush, r);
    }

    void DrawingContext::FillRectangleI(Brush* brush, const RectI& rectangle)
    {
        if (NeedToUseDC())
        {
            UseDC();

            auto& oldPen = _dc->GetPen();
            auto& oldBrush = _dc->GetBrush();

            _dc->SetPen(*wxTRANSPARENT_PEN);
            _dc->SetBrush(brush->GetWxBrush());

            _dc->DrawRectangle(rectangle);

            _dc->SetPen(oldPen);
            _dc->SetBrush(oldBrush);
        }
        else
        {
            UseGC();

            _graphicsContext->SetPen(*wxTRANSPARENT_PEN);
            _graphicsContext->SetBrush(GetGraphicsBrush(brush, wxPoint2DDouble(rectangle.X, rectangle.Y)));
            _graphicsContext->DrawRectangle(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
        }
    }

    void DrawingContext::DrawPoint(Pen* pen, double x, double y)
    {
        //if (NeedToUseDC())
        //{
            UseDC();
            auto& oldPen = _dc->GetPen();
            _dc->SetPen(pen->GetWxPen());
            _dc->DrawPoint(fromDip(Point(x, y), _dc->GetWindow()));
            _dc->SetPen(oldPen);
        //}
        //else
        //{
        //    UseGC();

        //    auto point = fromDipF(Point(x,y), _dc->GetWindow());

        //    _graphicsContext->SetPen(pen->GetWxPen());
        //    _graphicsContext->DrawPoint(point);
        //}
    }

    void DrawingContext::FillEllipse(Brush* brush, const Rect& bounds)
    {
        if (NeedToUseDC())
        {
            UseDC();

            auto& oldPen = _dc->GetPen();
            auto& oldBrush = _dc->GetBrush();

            _dc->SetPen(*wxTRANSPARENT_PEN);
            _dc->SetBrush(brush->GetWxBrush());

            _dc->DrawEllipse(
                fromDip(
                    Rect(
                        bounds.X,
                        bounds.Y,
                        bounds.Width,
                        bounds.Height),
                    _dc->GetWindow()));

            _dc->SetPen(oldPen);
            _dc->SetBrush(oldBrush);
        }
        else
        {
            UseGC();

            auto rect = fromDipF(bounds, _dc->GetWindow());

            _graphicsContext->SetPen(*wxTRANSPARENT_PEN);
            _graphicsContext->SetBrush(GetGraphicsBrush(brush, wxPoint2DDouble(rect.X, rect.Y)));

            _graphicsContext->DrawEllipse(rect.X, rect.Y, rect.Width, rect.Height);
        }
    }

    void DrawingContext::Ellipse(Pen* pen, Brush* brush, const Rect& bounds)
    {
        if (NeedToUseDC())
        {
            UseDC();

            auto& oldPen = _dc->GetPen();
            auto& oldBrush = _dc->GetBrush();

            _dc->SetPen(pen->GetWxPen());
            _dc->SetBrush(brush->GetWxBrush());

            _dc->DrawEllipse(
                fromDip(
                    Rect(
                        bounds.X,
                        bounds.Y,
                        bounds.Width,
                        bounds.Height),
                    _dc->GetWindow()));

            _dc->SetPen(oldPen);
            _dc->SetBrush(oldBrush);
        }
        else
        {
            UseGC();

            auto rect = fromDipF(bounds, _dc->GetWindow());

            _graphicsContext->SetPen(pen->GetWxPen());
            _graphicsContext->SetBrush(GetGraphicsBrush(brush, wxPoint2DDouble(rect.X, rect.Y)));

            _graphicsContext->DrawEllipse(rect.X, rect.Y, rect.Width, rect.Height);
        }
    }

    void DrawingContext::DrawRectangle(Pen* pen, const Rect& rectangle)
    {
        bool needToUseDC;
#ifdef __WXMSW__
        needToUseDC = true;
#else
        needToUseDC = NeedToUseDC();
#endif
        if (needToUseDC)
        {
            UseDC();

            auto& oldPen = _dc->GetPen();
            auto& oldBrush = _dc->GetBrush();

            auto penThickness = pen->GetWxPen().GetWidth();
            _dc->SetPen(pen->GetWxPen());
            _dc->SetBrush(*wxTRANSPARENT_BRUSH);

            // todo: different OSes and DPI levels may require adjustment to the rectangle.
            // maybe we will need to make platform-specific DC implementations?

#ifdef __WXOSX_COCOA__
            auto locationOffset = 0;
            auto sizeOffset = -penThickness;
#elif __WXMSW__
            auto locationOffset = penThickness / 2;
            auto sizeOffset = penThickness / 2;
#else
            auto locationOffset = 0;
            auto sizeOffset = 0;
#endif

            _dc->DrawRectangle(
                fromDip(
                    Rect(
                        rectangle.X + locationOffset,
                        rectangle.Y + locationOffset,
                        rectangle.Width - sizeOffset,
                        rectangle.Height - sizeOffset),
                    _dc->GetWindow()));

            _dc->SetPen(oldPen);
            _dc->SetBrush(oldBrush);
        }
        else
        {
            UseGC();

            _graphicsContext->SetPen(pen->GetWxPen());
            _graphicsContext->SetBrush(*wxTRANSPARENT_BRUSH);

            auto rect = fromDip(rectangle, _dc->GetWindow());

            _graphicsContext->DrawRectangle(rect.x, rect.y, rect.width, rect.height);
        }
    }

    void DrawingContext::DrawLine(Pen* pen, const Point& a, const Point& b)
    {
        if (NeedToUseDC())
        {
            UseDC();

            auto& oldPen = _dc->GetPen();

            _dc->SetPen(pen->GetWxPen());

            auto window = _dc->GetWindow();
            _dc->DrawLine(fromDip(a, window), fromDip(b, window));

            _dc->SetPen(oldPen);
        }
        else
        {
            UseGC();

            _graphicsContext->SetPen(pen->GetWxPen());

            auto window = _dc->GetWindow();
            auto p1 = fromDip(a, window);
            auto p2 = fromDip(b, window);
            _graphicsContext->StrokeLine(p1.x, p1.y, p2.x, p2.y);

            _graphicsContext->Flush();
        }
    }

    void DrawingContext::DrawLines(Pen* pen, Point* points, int pointsCount)
    {
        if (NeedToUseDC())
        {
            UseDC();

            if (pointsCount <= 2)
                return;

            auto& oldPen = _dc->GetPen();

            _dc->SetPen(pen->GetWxPen());

            auto window = _dc->GetWindow();

            std::vector<wxPoint> wxPoints(pointsCount);
            for (int i = 0; i < pointsCount; i++)
                wxPoints[i] = fromDip(points[i], window);

            _dc->DrawLines(pointsCount, &wxPoints[0]);

            _dc->SetPen(oldPen);
        }
        else
        {
            UseGC();

            if (pointsCount <= 2)
                return;

            _graphicsContext->SetPen(pen->GetWxPen());
            _graphicsContext->SetBrush(*wxTRANSPARENT_BRUSH);

            auto window = _dc->GetWindow();

            std::vector<wxPoint2DDouble> wxPoints(pointsCount);
            for (int i = 0; i < pointsCount; i++)
                wxPoints[i] = fromDip(points[i], window);

            _graphicsContext->DrawLines(pointsCount, &wxPoints[0]);
        }
    }

    void DrawingContext::DrawEllipse(Pen* pen, const Rect& bounds)
    {
        if (NeedToUseDC())
        {
            UseDC();

            auto& oldPen = _dc->GetPen();
            auto& oldBrush = _dc->GetBrush();

            _dc->SetPen(pen->GetWxPen());
            _dc->SetBrush(*wxTRANSPARENT_BRUSH);

            _dc->DrawEllipse(
                fromDip(
                    Rect(
                        bounds.X,
                        bounds.Y,
                        bounds.Width,
                        bounds.Height),
                    _dc->GetWindow()));

            _dc->SetPen(oldPen);
            _dc->SetBrush(oldBrush);
        }
        else
        {
            UseGC();
            _graphicsContext->SetPen(pen->GetWxPen());
            _graphicsContext->SetBrush(*wxTRANSPARENT_BRUSH);

            auto rect = fromDip(
                Rect(
                    bounds.X,
                    bounds.Y,
                    bounds.Width,
                    bounds.Height),
                _dc->GetWindow());
            _graphicsContext->DrawEllipse(rect.x, rect.y, rect.width, rect.height);
        }
    }

    static void _DrawText(wxDC* dc, const string& text, wxPoint point)
    {
#ifdef __WXMSW__
        dc->DrawText(wxStr(text), point);
#else
        dc->DrawText(wxStr(text), point);
#endif
    }

    void DrawingContext::DrawText(const string& text, const PointD& location, Font* font,
        const Color& foreColor, const Color& backColor)
    {
        bool useBackColor = !backColor.IsEmpty();

        UseDC();
        
        auto window = DrawingContext::GetWindow(_dc);

        auto backMode = _dc->GetBackgroundMode();

        if (useBackColor)
        {
            _dc->SetBackgroundMode(wxBRUSHSTYLE_SOLID);
        }
        else
        {
            _dc->SetBackgroundMode(wxBRUSHSTYLE_TRANSPARENT);
        }

        auto& oldTextForeground = _dc->GetTextForeground();
        _dc->SetTextForeground(foreColor);

        auto& oldFont = _dc->GetFont();
        _dc->SetFont(font->GetWxFont());

        PointD locationTranslated = location;

#ifndef __WXMSW__
        /*locationTranslated.X += _currentTranslation.x;
        locationTranslated.Y += _currentTranslation.y;*/
#endif

        auto point = fromDip(locationTranslated, window);

        if (useBackColor)
        {
            auto& oldTextBackground = _dc->GetTextBackground();
            _dc->SetTextBackground(backColor);
            _DrawText(_dc, text, point);
            _dc->SetTextBackground(oldTextBackground);
        }
        else
            _DrawText(_dc, text, point);

        _dc->SetTextForeground(oldTextForeground);
        _dc->SetFont(oldFont);

        _dc->SetBackgroundMode(backMode);
    }

    wxGraphicsBrush DrawingContext::GetGraphicsBrush(Brush* brush, const wxPoint2DDouble& offset)
    {
        return brush->GetGraphicsBrush(_graphicsContext->GetRenderer(), offset);
    }

    wxGraphicsPen DrawingContext::GetGraphicsPen(Pen* pen)
    {
        return pen->GetGraphicsPen(_graphicsContext->GetRenderer());
    }

    Size DrawingContext::GetTextExtentSimple(const string& text, Font* font, void* control)
    {
/*#if __WXMSW__
#else
        _dc->ResetTransformMatrix();
#endif*/

        auto wxf = font->GetWxFont();

        auto wxw = (wxWindow*)control;

        if (true)
        {
            UseDC();

            if (wxw == nullptr)
                wxw = _dc->GetWindow();

            int width = 0;
            int height = 0;

            auto& oldFont = _dc->GetFont();
            _dc->SetFont(wxf);
            _dc->GetTextExtent(wxStr(text), &width, &height, nullptr, nullptr, &wxf);
            _dc->SetFont(oldFont);

            return toDip(wxSize(width, height), wxw);
        }
        else
        {
            UseGC();

            if (wxw == nullptr)
                wxw = _dc->GetWindow();

            wxDouble width;
            wxDouble height;

            _graphicsContext->SetFont(wxf, *wxBLACK);
            _graphicsContext->GetTextExtent(wxStr(text), &width, &height, nullptr, nullptr);

            return toDip(wxSize(width, height), wxw);
        }
    }

    Rect DrawingContext::GetTextExtent(const string& text, Font* font, void* control)
    {
/*#if __WXMSW__
#else
        _dc->ResetTransformMatrix();
#endif*/

        auto wxf = font->GetWxFont();

        auto wxw = (wxWindow*)control;

        if (true)
        {
            UseDC();

            if (wxw == nullptr)
                wxw = _dc->GetWindow();

            int width = 0;
            int height = 0;
            int descent = 0;
            int externalLeading = 0;

            auto& oldFont = _dc->GetFont();

            _dc->SetFont(wxf);
            _dc->GetTextExtent(wxStr(text), &width, &height, &descent, &externalLeading, &wxf);
            _dc->SetFont(oldFont);

            auto widthF = toDip(width, wxw);
            auto heightF = toDip(height, wxw);
            auto descentF = toDip(descent, wxw);
            auto externalLeadingF = toDip(externalLeading, wxw);
            return Rect(descentF, externalLeadingF, widthF, heightF);
        }
        else
        {
            UseGC();

            if (wxw == nullptr)
                wxw = _dc->GetWindow();

            wxDouble width;
            wxDouble height;
            wxDouble descent;
            wxDouble externalLeading;

            _graphicsContext->SetFont(wxf, *wxBLACK);
            _graphicsContext->GetTextExtent(wxStr(text), &width, &height, &descent, &externalLeading);

            auto widthF = toDip(width, wxw);
            auto heightF = toDip(height, wxw);
            auto descentF = toDip(descent, wxw);
            auto externalLeadingF = toDip(externalLeading, wxw);

            return Rect(descentF, externalLeadingF, widthF, heightF);
        }
    }
}