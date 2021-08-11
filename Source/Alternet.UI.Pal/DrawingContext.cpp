#include "DrawingContext.h"

namespace Alternet::UI
{
    DrawingContext::DrawingContext(wxDC* dc) : _dc(dc)
    {
        assert(_dc);
    }

    DrawingContext::~DrawingContext()
    {
        wxDELETE(_dc);
    }

    void DrawingContext::DrawImage(Image* image, const PointF& origin)
    {
        wxBitmap bitmap(image->GetImage());
        _dc->DrawBitmap(bitmap, fromDip(origin, _dc->GetWindow()));
    }

    void DrawingContext::PushTransform(const SizeF& translation)
    {
        _translationStack.push(_translation);
        _translation += translation;
    }

    void DrawingContext::Pop()
    {
        _translation = _translationStack.top();
        _translationStack.pop();
    }

    void DrawingContext::FillRectangle(const RectangleF& rectangle, const Color& color)
    {
        if (color.A == 0)
            return;

        auto oldPen = _dc->GetPen();
        auto oldBrush = _dc->GetBrush();

        _dc->SetPen(*wxTRANSPARENT_PEN);
        _dc->SetBrush(wxBrush(color));

        _dc->DrawRectangle(fromDip(rectangle.Offset(_translation), _dc->GetWindow()));

        _dc->SetPen(oldPen);
        _dc->SetBrush(oldBrush);
    }

    void DrawingContext::DrawRectangle(const RectangleF& rectangle, const Color& color)
    {
        if (color.A == 0)
            return;

        auto oldPen = _dc->GetPen();
        auto oldBrush = _dc->GetBrush();

        float penThickness = 1;
        _dc->SetPen(wxPen(color, fromDip(penThickness, _dc->GetWindow())));
        _dc->SetBrush(*wxTRANSPARENT_BRUSH);

        // todo: different OSes and DPI levels may require adjustment to the rectangle.
        // maybe we will need to make platform-specific DC implementations?
        _dc->DrawRectangle(
            fromDip(
                RectangleF(
                    rectangle.X/* + penThickness / 2*/,
                    rectangle.Y/* + penThickness / 2*/,
                    rectangle.Width/* - penThickness / 2*/,
                    rectangle.Height/* - penThickness / 2*/).Offset(_translation),
                _dc->GetWindow()));

        _dc->SetPen(oldPen);
        _dc->SetBrush(oldBrush);
    }

    void DrawingContext::DrawText(const string& text, const PointF& origin, Font* font, const Color& color)
    {
        auto oldTextForeground = _dc->GetTextForeground();
        auto oldFont = _dc->GetFont();
        _dc->SetTextForeground(color);
        _dc->SetFont(font->GetWxFont());
        _dc->DrawText(wxStr(text), fromDip(origin + _translation, _dc->GetWindow()));
        _dc->SetTextForeground(oldTextForeground);
        _dc->SetFont(oldFont);
    }

    SizeF DrawingContext::MeasureText(const string& text, Font* font)
    {
        wxCoord x = 0, y = 0;
        auto wxFont = font->GetWxFont();
        _dc->GetTextExtent(wxStr(text), &x, &y, nullptr, nullptr, &wxFont);
        return toDip(wxSize(x, y), _dc->GetWindow());
    }
}
