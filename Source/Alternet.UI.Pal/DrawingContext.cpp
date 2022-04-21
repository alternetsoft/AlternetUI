#include "DrawingContext.h"
#include "SolidBrush.h"

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

    /*static*/ DrawingContext* DrawingContext::FromImage(Image* image)
    {
        auto bitmap = image->GetBitmap();
        auto dc = new wxMemoryDC(bitmap);
        image->SetBitmap(bitmap); // wxMemoryDC "unshares" bitmap under the hood. So need to reassign it back.
        return new DrawingContext(dc);
    }

    void DrawingContext::DrawImage(Image* image, const Point& origin)
    {
        wxBitmap bitmap = image->GetBitmap();
        _dc->DrawBitmap(bitmap, fromDip(origin, _dc->GetWindow()));
    }

    void DrawingContext::FloodFill(const Point& point, Brush* brush)
    {
        auto solidBrush = dynamic_cast<SolidBrush*>(brush);
        if (solidBrush == nullptr)
        {
            // Only SolidBrush is supported.
            wxASSERT(false);
            throw 0;
        }

        auto oldBrush = _dc->GetBrush();
        _dc->SetBrush(solidBrush->GetWxBrush());

        auto pixelPoint = fromDip(point, _dc->GetWindow());

        wxColor color;
        if (!_dc->GetPixel(pixelPoint, &color))
            return;

        _dc->FloodFill(pixelPoint, color, wxFLOOD_SURFACE);

        _dc->SetBrush(oldBrush);
    }

    void DrawingContext::PushTransform(const Size& translation)
    {
        _translationStack.push(_translation);
        _translation += translation;
    }

    void DrawingContext::Pop()
    {
        _translation = _translationStack.top();
        _translationStack.pop();
    }

    void DrawingContext::FillRectangle(const Rect& rectangle, Brush* brush)
    {
        _graphicsContext->SetPen(*wxTRANSPARENT_PEN);
        _graphicsContext->SetBrush(GetGraphicsBrush(brush));

        auto rect = fromDipF(rectangle.Offset(_translation), _dc->GetWindow());
        _graphicsContext->DrawRectangle(rect.X, rect.Y, rect.Width, rect.Height);
    }

    void DrawingContext::FillEllipse(const Rect& bounds, Brush* brush)
    {
        _graphicsContext->SetPen(*wxTRANSPARENT_PEN);
        _graphicsContext->SetBrush(GetGraphicsBrush(brush));

        auto rect = fromDipF(bounds.Offset(_translation), _dc->GetWindow());
        _graphicsContext->DrawEllipse(rect.X, rect.Y, rect.Width, rect.Height);
    }

    void DrawingContext::DrawRectangle(const Rect& rectangle, Pen* pen)
    {
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
                    rectangle.Height - sizeOffset).Offset(_translation),
                _dc->GetWindow()));

        _dc->SetPen(oldPen);
        _dc->SetBrush(oldBrush);
    }

    void DrawingContext::DrawLine(const Point& a, const Point& b, Pen* pen)
    {
        auto oldPen = _dc->GetPen();

        _dc->SetPen(pen->GetWxPen());

        auto window = _dc->GetWindow();
        _dc->DrawLine(fromDip(a + _translation, window), fromDip(b + _translation, window));

        _dc->SetPen(oldPen);
    }

    void DrawingContext::DrawLines(Point* points, int pointsCount, Pen* pen)
    {
        if (pointsCount <= 2)
            return;

        auto oldPen = _dc->GetPen();

        _dc->SetPen(pen->GetWxPen());

        auto window = _dc->GetWindow();

        std::vector<wxPoint> wxPoints(pointsCount);
        for (int i = 0; i < pointsCount; i++)
            wxPoints[i] = fromDip(points[i], window);

        wxSize wxTranslation = fromDip(_translation, window);

        _dc->DrawLines(pointsCount, &wxPoints[0], wxTranslation.x, wxTranslation.y);

        _dc->SetPen(oldPen);
    }

    void DrawingContext::DrawEllipse(const Rect& bounds, Pen* pen)
    {
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
                    bounds.Height).Offset(_translation),
                _dc->GetWindow()));

        _dc->SetPen(oldPen);
        _dc->SetBrush(oldBrush);
    }

    void DrawingContext::DrawText(const string& text, const Point& origin, Font* font, Brush* brush)
    {
        if (_useDCForText)
        {
            auto solidBrush = dynamic_cast<SolidBrush*>(brush);
            if (solidBrush == nullptr)
            {
                wxASSERT(false);
                throw 0;
            }

            auto oldTextForeground = _dc->GetTextForeground();
            auto oldFont = _dc->GetFont();
            _dc->SetTextForeground(solidBrush->GetWxBrush().GetColour());
            _dc->SetFont(font->GetWxFont());
            _dc->DrawText(wxStr(text), fromDip(origin + _translation, _dc->GetWindow()));
            _dc->SetTextForeground(oldTextForeground);
            _dc->SetFont(oldFont);
        }
        else
        {
            auto o = fromDipF(origin + _translation, _dc->GetWindow());
            _graphicsContext->SetFont(_graphicsContext->CreateFont(font->GetWxFont()));
            _graphicsContext->SetBrush(GetGraphicsBrush(brush));
            _graphicsContext->DrawText(wxStr(text), o.X, o.Y);
        }
    }

    wxGraphicsBrush DrawingContext::GetGraphicsBrush(Brush* brush)
    {
        return brush->GetGraphicsBrush(_graphicsContext->GetRenderer());
    }

    wxGraphicsPen DrawingContext::GetGraphicsPen(Pen* pen)
    {
        return pen->GetGraphicsPen(_graphicsContext->GetRenderer());
    }

    Size DrawingContext::MeasureText(const string& text, Font* font)
    {
        if (_useDCForText)
        {
            wxCoord x = 0, y = 0;
            auto wxFont = font->GetWxFont();
            auto oldFont = _dc->GetFont();
            _dc->SetFont(wxFont); // just passing font as a GetMultiLineTextExtent argument doesn't work on macOS/Linux
            _dc->GetMultiLineTextExtent(wxStr(text), &x, &y, nullptr, &wxFont);
            _dc->SetFont(oldFont);
            return toDip(wxSize(x, y), _dc->GetWindow());
        }
        else
        {
            // todo
            wxASSERT(false);
            throw 0;
        }
    }
}
