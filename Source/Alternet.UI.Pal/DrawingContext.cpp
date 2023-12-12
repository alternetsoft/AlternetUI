#include "DrawingContext.h"
#include "SolidBrush.h"
#include "FloodFill.h"
#include "TextPainter.h"
#include "GenericImage.h"

namespace Alternet::UI
{
    Size DrawingContext::GetDpi()
    {
        UseDC();
        return _dc->GetPPI();
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

    DrawingContext::DrawingContext(wxDC* dc,
        optional<std::function<void()>> onUseDC /*= nullopt*/) : _dc(dc), _onUseDC(onUseDC)
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
        if (pointsCount == 0)
            return;

        if ((pointsCount - 1) % 3 != 0)
            throwExInvalidArg(
                pointsCount,
                u"The number of points in the array should be a multiple of 3 plus 1, such as 4, 7, or 10.");

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

    void DrawingContext::DrawImageAtPoint(Image* image, const Point& origin)
    {
        wxBitmap bitmap = image->GetBitmap();
        auto window = _dc->GetWindow();

        auto pt = fromDip(origin, _dc->GetWindow());

        if (NeedToUseDC())
        {
            UseDC();
            _dc->DrawBitmap(bitmap, pt);
        }
        else
        {
            UseGC();
            auto wxr = wxRect(pt, image->GetPixelSize());
            _graphicsContext->DrawBitmap(bitmap, wxr.x, wxr.y, wxr.width, wxr.height);
        }
    }

    void DrawingContext::DrawImageAtRect(Image* image, const Rect& destinationRect)
    {
        UseGC();

        wxBitmap bitmap = image->GetBitmap();
        auto destRect = fromDip(destinationRect, _dc->GetWindow());

        auto oldInterpolationQuality = _graphicsContext->GetInterpolationQuality();
        _graphicsContext->SetInterpolationQuality(GetInterpolationQuality(_interpolationMode));

        _graphicsContext->DrawBitmap(bitmap, destRect.x, destRect.y, destRect.width, destRect.height);

        _graphicsContext->SetInterpolationQuality(oldInterpolationQuality);
    }

    void DrawingContext::DrawImagePortionAtRect(Image* image, const Rect& destinationRect,
        const Rect& sourceRect)
    {
        wxBitmap bitmap = image->GetBitmap();
        auto wxSourceRect = fromDip(sourceRect, _dc->GetWindow());
        auto wxDestinationRect = fromDip(destinationRect, _dc->GetWindow());

        wxMemoryDC sourceBitmapDC(bitmap);
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

    /*Color DrawingContext::GetPixel(const Point& p)
    {
        UseDC();
        wxColor seedColor;
        if (!_dc->GetPixel(p, &seedColor))
            return seedColor;
        return seedColor;
    }*/

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

    void DrawingContext::Push()
    {
        _transformStack.push(_dc->GetTransformMatrix());
    }

    void DrawingContext::Pop()
    {
        if (_transformStack.empty())
            return;

        SetTransformCore(_transformStack.top());
        _transformStack.pop();
    }

    TransformMatrix* DrawingContext::GetTransform()
    {
        auto result = new TransformMatrix(_currentTransform);
        result->AddRef();
        return result;
    }

    void DrawingContext::SetTransform(TransformMatrix* value)
    {
        SetTransformCore(value->GetMatrix());
    }

    void DrawingContext::SetTransformCore(const wxAffineMatrix2D& value)
    {
        _currentTransform = value;

        wxMatrix2D m;
        wxPoint2DDouble t;
        value.Get(&m, &t);
        _currentTranslation = wxPoint((int)t.m_x, (int)t.m_y);

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
        if (_onUseDC != nullopt)
            _onUseDC.value()();

        ApplyTransform(/*useDC:*/true);
    }

    void DrawingContext::UseGC()
    {
        if (_onUseDC != nullopt)
            _onUseDC.value()();

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
        if (NeedToUseDC())
        {
            UseDC();

            auto& oldPen = _dc->GetPen();
            auto& oldBrush = _dc->GetBrush();

            _dc->SetPen(*wxTRANSPARENT_PEN);
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

            _graphicsContext->SetPen(*wxTRANSPARENT_PEN);
            _graphicsContext->SetBrush(GetGraphicsBrush(brush, wxPoint2DDouble(rect.X, rect.Y)));

            _graphicsContext->DrawRectangle(rect.X, rect.Y, rect.Width, rect.Height);
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

    void DrawingContext::GetPartialTextExtents(const string& text,
        double* widths, int widthsCount, Font* font, void* control)
    {

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

    void DrawingContext::DrawText(const string& text, const Point& location, Font* font,
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

        auto point = fromDip(location, window);

        if (useBackColor)
        {
            auto& oldTextBackground = _dc->GetTextBackground();
            _dc->SetTextBackground(backColor);
            _dc->DrawText(wxStr(text), point);
            _dc->SetTextBackground(oldTextBackground);
        }
        else
            _dc->DrawText(wxStr(text), point);

        _dc->SetTextForeground(oldTextForeground);
        _dc->SetFont(oldFont);

        _dc->SetBackgroundMode(backMode);
    }

    void DrawingContext::DrawTextAtPoint(
        const string& text,
        const Point& origin,
        Font* font,
        Brush* brush)
    {
        //if (NeedToUseDC())
        //    UseDC();
        //else
        //    UseGC();
#if __WXMSW__
        UseDC();
#else
        _dc->ResetTransformMatrix();
#endif

        std::unique_ptr<TextPainter>(GetTextPainter())->DrawTextAtPoint(
            text,
            origin,
            font,
            brush);
    }

    void DrawingContext::DrawTextAtRect(
        const string& text,
        const Rect& bounds,
        Font* font,
        Brush* brush,
        TextHorizontalAlignment horizontalAlignment,
        TextVerticalAlignment verticalAlignment,
        TextTrimming trimming,
        TextWrapping wrapping)
    {
        //if (NeedToUseDC())
        //    UseDC();
        //else
        //    UseGC();
#if __WXMSW__
        UseDC();
#else
        _dc->ResetTransformMatrix();
#endif

        std::unique_ptr<TextPainter>(GetTextPainter())->DrawTextAtRect(
            text,
            bounds,
            font,
            brush,
            horizontalAlignment,
            verticalAlignment,
            trimming,
            wrapping);
    }

    wxGraphicsBrush DrawingContext::GetGraphicsBrush(Brush* brush, const wxPoint2DDouble& offset)
    {
        return brush->GetGraphicsBrush(_graphicsContext->GetRenderer(), offset);
    }

    wxGraphicsPen DrawingContext::GetGraphicsPen(Pen* pen)
    {
        return pen->GetGraphicsPen(_graphicsContext->GetRenderer());
    }

    TextPainter* DrawingContext::GetTextPainter()
    {
        wxPoint translation;
#ifndef __WXMSW__
        translation = _currentTranslation;
#endif
        return new TextPainter(_dc, _graphicsContext, /*NeedToUseDC()*/true, translation);
    }

    Size DrawingContext::MeasureText(const string& text, Font* font, double maximumWidth,
        TextWrapping wrapping)
    {
        //if (NeedToUseDC())
        //    UseDC();
        //else
        //    UseGC();
#if __WXMSW__
        UseDC();
#else
        _dc->ResetTransformMatrix();
#endif

        return std::unique_ptr<TextPainter>(GetTextPainter())->MeasureText(text, font,
            maximumWidth, wrapping);
    }

    Size DrawingContext::GetTextExtentSimple(const string& text, Font* font, void* control)
    {
#if __WXMSW__
#else
        _dc->ResetTransformMatrix();
#endif

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
#if __WXMSW__
#else
        _dc->ResetTransformMatrix();
#endif

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