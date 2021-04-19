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

    void DrawingContext::DrawText(const string& text, const PointF& origin, const Color& color)
    {
        auto oldTextForeground = _dc->GetTextForeground();
        _dc->SetTextForeground(color);
        _dc->DrawText(wxStr(text), fromDip(origin + _translation, _dc->GetWindow()));
        _dc->SetTextForeground(oldTextForeground);
    }

    SizeF DrawingContext::MeasureText(const string& text)
    {
        return toDip(_dc->GetTextExtent(wxStr(text)), _dc->GetWindow());
    }
}
