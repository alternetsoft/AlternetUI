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

        _dc->DrawBitmap(bitmap, fromDip(origin, _dc->GetWindow()));
    }

    void DrawingContext::DrawImageAtRect(Image* image, const Rect& rect)
    {
        wxBitmap bitmap = image->GetBitmap();
        auto destRect = fromDip(rect, _dc->GetWindow());
        
        auto oldInterpolationQuality = _graphicsContext->GetInterpolationQuality();
        _graphicsContext->SetInterpolationQuality(wxInterpolationQuality::wxINTERPOLATION_BEST);
        
        _graphicsContext->DrawBitmap(bitmap, destRect.x, destRect.y, destRect.width, destRect.height);

        _graphicsContext->SetInterpolationQuality(oldInterpolationQuality);
    }

    void DrawingContext::FloodFill(const Point& point, Brush* fillBrush)
    {
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

    void DrawingContext::DrawTextAtPoint(
        const string& text,
        const Point& origin,
        Font* font,
        Brush* brush)
    {
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
        TextTrimming trimming)
    {
        std::unique_ptr<TextPainter>(GetTextPainter())->DrawTextAtRect(
            text,
            bounds,
            font,
            brush,
            horizontalAlignment,
            verticalAlignment,
            trimming);
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
        return new TextPainter(_dc, _translation);
    }

    Size DrawingContext::MeasureText(const string& text, Font* font, double maximumWidth)
    {
        return std::unique_ptr<TextPainter>(GetTextPainter())->MeasureText(text, font, maximumWidth);
    }
}
