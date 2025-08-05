#include "DrawingContext.h"
#include "SolidBrush.h"
#include "GenericImage.h"
#include <algorithm>

namespace Alternet::UI
{
    DrawingContext::DrawingContext(wxDC* dc) : _dc(dc)
    {
        assert(_dc);

#ifdef  __WXMSW__
        wxGraphicsRenderer* renderer = wxGraphicsRenderer::GetDirect2DRenderer();
        _graphicsContext = renderer->CreateContextFromUnknownDC(*_dc);
#else
        _graphicsContext = wxGraphicsContext::CreateFromUnknownDC(*_dc);
#endif
    }

    DrawingContext::~DrawingContext()
    {
        wxDELETE(_graphicsContext);

        if (!_doNotDeleteDC)
            wxDELETE(_dc);
    }

    DrawingContext* DrawingContext::CreateMemoryDC(double scaleFactor)
    {
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
        return _dc->GetPPI();
    }

    void* DrawingContext::GetWxWidgetDC()
    {
        return _dc;
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

    InterpolationMode DrawingContext::GetInterpolationMode()
    {
        return _interpolationMode;
    }

    void DrawingContext::SetInterpolationMode(InterpolationMode value)
    {
        _interpolationMode = value;
    }

    void DrawingContext::SetDoNotDeleteDC(bool value)
    {
        _doNotDeleteDC = value;
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

    void DrawingContext::SetClippingRect(const Rect& rect)
    {
        auto bounds = fromDip(rect, _dc->GetWindow());
        _graphicsContext->Clip(bounds.x, bounds.y, bounds.width, bounds.height);
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

    void DrawingContext::SetClippingRegion(Region* region)
    {
        _graphicsContext->Clip(region->GetRegion());
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

    void DrawingContext::Pie(Pen* pen, Brush* brush, const Point& center,
        double radius, double startAngle,
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
        _graphicsContext->SetPen(pen->GetWxPen());
        _graphicsContext->SetBrush(GetGraphicsBrush(brush, wxPoint2DDouble(rect.X, rect.Y)));
        auto r = fromDipF(rect, _dc->GetWindow());
        _graphicsContext->DrawRoundedRectangle(r.X, r.Y, r.Width, r.Height, cornerRadius);
    }

    void DrawingContext::DrawRoundedRectangle(Pen* pen, const Rect& rect, double cornerRadius)
    {
        _graphicsContext->SetPen(pen->GetWxPen());
        _graphicsContext->SetBrush(*wxTRANSPARENT_BRUSH);
        auto r = fromDipF(rect, _dc->GetWindow());
        _graphicsContext->DrawRoundedRectangle(r.X, r.Y, r.Width, r.Height, cornerRadius);
    }

    void DrawingContext::Save()
    {
        _graphicsContext->PushState();
    }

    void DrawingContext::Restore()
    {
        _graphicsContext->PopState();
    }

    void DrawingContext::FillRoundedRectangle(Brush* brush, const Rect& rect, double cornerRadius)
    {
        _graphicsContext->SetPen(*wxTRANSPARENT_PEN);
        _graphicsContext->SetBrush(GetGraphicsBrush(brush, wxPoint2DDouble(rect.X, rect.Y)));
        auto r = fromDipF(rect, _dc->GetWindow());
        _graphicsContext->DrawRoundedRectangle(r.X, r.Y, r.Width, r.Height, cornerRadius);
    }

    void DrawingContext::DrawPath(Pen* pen, GraphicsPath* path)
    {
        _graphicsContext->SetPen(pen->GetWxPen());
        _graphicsContext->StrokePath(path->GetPath());
    }

    void DrawingContext::FillPath(Brush* brush, GraphicsPath* path)
    {
        auto bounds = fromDipF(path->GetBounds(), _dc->GetWindow());

        _graphicsContext->SetPen(*wxTRANSPARENT_PEN);
        _graphicsContext->SetBrush(GetGraphicsBrush(brush, wxPoint2DDouble(bounds.X, bounds.Y)));

        _graphicsContext->FillPath(path->GetPath(), path->GetWxFillMode());
    }

    void DrawingContext::Path(Pen* pen, Brush* brush, GraphicsPath* path)
    {
        auto bounds = fromDipF(path->GetBounds(), _dc->GetWindow());

        _graphicsContext->SetPen(pen->GetWxPen());
        _graphicsContext->SetBrush(GetGraphicsBrush(brush, wxPoint2DDouble(bounds.X, bounds.Y)));
        _graphicsContext->DrawPath(path->GetPath(), path->GetWxFillMode());
    }

    void DrawingContext::DrawPolygon(Pen* pen, Point* points, int pointsCount)
    {
        auto path = new GraphicsPath(_dc, _graphicsContext);

        path->AddLines(points, pointsCount);
        path->CloseFigure();
        DrawPath(pen, path);

        path->Release();
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

    /*static*/ DrawingContext* DrawingContext::FromImage(Image* image)
    {
        auto bitmap = image->GetBitmap();
        auto dc = new wxMemoryDC(bitmap);
        image->SetBitmap(bitmap); // wxMemoryDC unshared bitmap, so need to reassign it back.
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

        auto wxr = wxRect(pt, image->GetPixelSize());
        _graphicsContext->DrawBitmap(bitmap, wxr.x, wxr.y, wxr.width, wxr.height);
    }

    void DrawingContext::DrawImageAtRect(Image* image, const Rect& destinationRect, bool useMask)
    {
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

    void DrawingContext::SetTransformValues(
        double m11, double m12, double m21, double m22, double dx, double dy)
    {
        wxMatrix2D m(m11, m12, m21, m22);
        wxPoint2DDouble t(dx, dy);

        wxAffineMatrix2D matrix;
        matrix.Set(m, t);

        _currentTransform = matrix;

        _graphicsContext->SetTransform(_graphicsContext->CreateMatrix(_currentTransform));
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
        auto rect = fromDipF(rectangle, _dc->GetWindow());

        _graphicsContext->SetPen(pen->GetWxPen());
        _graphicsContext->SetBrush(GetGraphicsBrush(brush, wxPoint2DDouble(rect.X, rect.Y)));

        _graphicsContext->DrawRectangle(rect.X, rect.Y, rect.Width, rect.Height);
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
        _graphicsContext->SetPen(*wxTRANSPARENT_PEN);
        _graphicsContext->SetBrush(GetGraphicsBrush(brush, wxPoint2DDouble(rectangle.X, rectangle.Y)));
        _graphicsContext->DrawRectangle(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
    }

    void DrawingContext::DrawPoint(Pen* pen, double x, double y)
    {
        auto& oldPen = _dc->GetPen();
        _dc->SetPen(pen->GetWxPen());
        _dc->DrawPoint(fromDip(Point(x, y), _dc->GetWindow()));
        _dc->SetPen(oldPen);
    }

    void DrawingContext::FillEllipse(Brush* brush, const Rect& bounds)
    {
        auto rect = fromDipF(bounds, _dc->GetWindow());

        _graphicsContext->SetPen(*wxTRANSPARENT_PEN);
        _graphicsContext->SetBrush(GetGraphicsBrush(brush, wxPoint2DDouble(rect.X, rect.Y)));

        _graphicsContext->DrawEllipse(rect.X, rect.Y, rect.Width, rect.Height);
    }

    void DrawingContext::Ellipse(Pen* pen, Brush* brush, const Rect& bounds)
    {
        auto rect = fromDipF(bounds, _dc->GetWindow());

        _graphicsContext->SetPen(pen->GetWxPen());
        _graphicsContext->SetBrush(GetGraphicsBrush(brush, wxPoint2DDouble(rect.X, rect.Y)));

        _graphicsContext->DrawEllipse(rect.X, rect.Y, rect.Width, rect.Height);
    }

    void DrawingContext::DrawRectangle(Pen* pen, const Rect& rectangle)
    {
        _graphicsContext->SetPen(pen->GetWxPen());
        _graphicsContext->SetBrush(*wxTRANSPARENT_BRUSH);

        auto rect = fromDip(rectangle, _dc->GetWindow());

        _graphicsContext->DrawRectangle(rect.x, rect.y, rect.width, rect.height);
    }

    void DrawingContext::DrawLine(Pen* pen, const Point& a, const Point& b)
    {
        _graphicsContext->SetPen(pen->GetWxPen());

        auto window = _dc->GetWindow();
        auto p1 = fromDip(a, window);
        auto p2 = fromDip(b, window);
        _graphicsContext->StrokeLine(p1.x, p1.y, p2.x, p2.y);

        _graphicsContext->Flush();
    }

    void DrawingContext::DrawLines(Pen* pen, Point* points, int pointsCount)
    {
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

    void DrawingContext::DrawEllipse(Pen* pen, const Rect& bounds)
    {
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

    void DrawingContext::DrawText(const string& text, const PointD& location,
        Font* font, const Color& foreColor, Brush* backColor, double angle, bool useBrush)
    {
        auto window = DrawingContext::GetWindow(_dc);

        auto point = fromDip(location, window);

        auto x = static_cast<double>(point.x);
        auto y = static_cast<double>(point.y);

        wxColour wxForeColor = foreColor;

        if (!wxForeColor.IsOk())
        {
            return;
        }

        wxGraphicsFont gFont = _graphicsContext->CreateFont(font->GetWxFont(), wxForeColor);
        _graphicsContext->SetFont(gFont);

        wxString wxText = wxStr(text);

        if (useBrush)
        {
            wxGraphicsBrush gBrush = _graphicsContext->CreateBrush(backColor->GetWxBrush());
            if (angle == 0)
                _graphicsContext->DrawText(wxText, x, y, gBrush);
            else
                _graphicsContext->DrawText(wxText, x, y, angle, gBrush);
        }
        else
        {
            if(angle == 0)
                _graphicsContext->DrawText(wxText, x, y);
            else
                _graphicsContext->DrawText(wxText, x, y, angle);
        }
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
        auto wxf = font->GetWxFont();

        auto wxw = (wxWindow*)control;

        if (wxw == nullptr)
            wxw = _dc->GetWindow();

        wxDouble height;
        wxDouble width;

        _graphicsContext->SetFont(wxf, *wxBLACK);

        auto wText = wxStr(text);

        _graphicsContext->GetTextExtent(wText, &width, &height, nullptr, nullptr);
        
        /*int fontSize = wxf.GetPointSize();*/

        /*width += fontSize * 0.2;*/

        /*width = std::ceil(width);
        */

        height = std::ceil(height);

        width = toDip(width, wxw);
        height = toDip(height, wxw);

        return Size(width, height);
    }    
}