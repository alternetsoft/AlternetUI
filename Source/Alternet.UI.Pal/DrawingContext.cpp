#include "DrawingContext.h"
#include "SolidBrush.h"
#include "FloodFill.h"
#include "TextPainter.h"

namespace Alternet::UI
{
    DrawingContext::DrawingContext(wxDC* dc) : _dc(dc)
    {
        assert(_dc);
        _graphicsContext = wxGraphicsContext::CreateFromUnknownDC(*_dc);
    }

    DrawingContext::~DrawingContext()
    {
        wxDELETE(_graphicsContext);
        wxDELETE(_dc);
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

    void DrawingContext::DrawPath(Pen* pen, GraphicsPath* path)
    {
        UseGC();
        _graphicsContext->SetPen(pen->GetWxPen());
        _graphicsContext->DrawPath(path->GetPath(), path->GetWxFillMode());
    }

    void DrawingContext::FillPath(Brush* brush, GraphicsPath* path)
    {
        UseGC();
        _graphicsContext->SetPen(*wxTRANSPARENT_PEN);
        _graphicsContext->SetBrush(GetGraphicsBrush(brush));
        _graphicsContext->FillPath(path->GetPath(), path->GetWxFillMode());
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


        if (NeedToUseDC())
        {
            UseDC();
            _dc->DrawBitmap(bitmap, fromDip(origin, _dc->GetWindow()));
        }
        else
        {
            UseGC();
            auto wxRect = fromDip(Rect(origin, image->GetSize()), _dc->GetWindow());
            _graphicsContext->DrawBitmap(bitmap, wxRect.x, wxRect.y, wxRect.width, wxRect.height);
        }
    }

    void DrawingContext::DrawImageAtRect(Image* image, const Rect& rect)
    {
        UseGC();

        wxBitmap bitmap = image->GetBitmap();
        auto destRect = fromDip(rect, _dc->GetWindow());

        auto oldInterpolationQuality = _graphicsContext->GetInterpolationQuality();
        _graphicsContext->SetInterpolationQuality(wxInterpolationQuality::wxINTERPOLATION_BEST);

        _graphicsContext->DrawBitmap(bitmap, destRect.x, destRect.y, destRect.width, destRect.height);

        _graphicsContext->SetInterpolationQuality(oldInterpolationQuality);
    }

    void DrawingContext::FloodFill(const Point& point, Brush* fillBrush)
    {
        UseDC();

        auto fillSolidBrush = dynamic_cast<SolidBrush*>(fillBrush);
        if (fillSolidBrush == nullptr)
        {
            throwExInvalidArg(fillBrush, u"Only SolidBrush objects are supported");
        }

        auto oldBrush = _dc->GetBrush();
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
        return false;
    }

    void DrawingContext::FillRectangle(const Rect& rectangle, Brush* brush)
    {
        UseGC();

        _graphicsContext->SetPen(*wxTRANSPARENT_PEN);
        _graphicsContext->SetBrush(GetGraphicsBrush(brush));

        auto rect = fromDipF(rectangle, _dc->GetWindow());
        _graphicsContext->DrawRectangle(rect.X, rect.Y, rect.Width, rect.Height);
    }

    void DrawingContext::FillEllipse(const Rect& bounds, Brush* brush)
    {
        UseGC();

        _graphicsContext->SetPen(*wxTRANSPARENT_PEN);
        _graphicsContext->SetBrush(GetGraphicsBrush(brush));

        auto rect = fromDipF(bounds, _dc->GetWindow());
        _graphicsContext->DrawEllipse(rect.X, rect.Y, rect.Width, rect.Height);
    }

    void DrawingContext::DrawRectangle(const Rect& rectangle, Pen* pen)
    {
        if (NeedToUseDC())
        {
            UseDC();

            auto oldPen = _dc->GetPen();
            auto oldBrush = _dc->GetBrush();

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

    void DrawingContext::DrawLine(const Point& a, const Point& b, Pen* pen)
    {
        if (NeedToUseDC())
        {
            UseDC();

            auto oldPen = _dc->GetPen();

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
        }
    }

    void DrawingContext::DrawLines(Point* points, int pointsCount, Pen* pen)
    {
        if (NeedToUseDC())
        {
            UseDC();

            if (pointsCount <= 2)
                return;

            auto oldPen = _dc->GetPen();

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

            auto window = _dc->GetWindow();

            std::vector<wxPoint2DDouble> wxPoints(pointsCount);
            for (int i = 0; i < pointsCount; i++)
                wxPoints[i] = fromDip(points[i], window);

            _graphicsContext->DrawLines(pointsCount, &wxPoints[0]);
        }
    }

    void DrawingContext::DrawEllipse(const Rect& bounds, Pen* pen)
    {
        if (NeedToUseDC())
        {
            UseDC();

            auto oldPen = _dc->GetPen();
            auto oldBrush = _dc->GetBrush();

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

    void DrawingContext::DrawTextAtPoint(
        const string& text,
        const Point& origin,
        Font* font,
        Brush* brush)
    {
        if (NeedToUseDC())
            UseDC();
        else
            UseGC();

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
        if (NeedToUseDC())
            UseDC();
        else
            UseGC();

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

    wxGraphicsBrush DrawingContext::GetGraphicsBrush(Brush* brush)
    {
        return brush->GetGraphicsBrush(_graphicsContext->GetRenderer());
    }

    wxGraphicsPen DrawingContext::GetGraphicsPen(Pen* pen)
    {
        return pen->GetGraphicsPen(_graphicsContext->GetRenderer());
    }

    TextPainter* DrawingContext::GetTextPainter()
    {
        return new TextPainter(_dc, _graphicsContext, NeedToUseDC());
    }

    Size DrawingContext::MeasureText(const string& text, Font* font, double maximumWidth, TextWrapping wrapping)
    {
        if (NeedToUseDC())
            UseDC();
        else
            UseGC();

        return std::unique_ptr<TextPainter>(GetTextPainter())->MeasureText(text, font, maximumWidth, wrapping);
    }
}