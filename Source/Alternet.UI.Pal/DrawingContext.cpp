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

    void DrawingContext::FillRectangle(const RectangleF& rectangle, const Color& color)
    {
        auto oldPen = _dc->GetPen();
        auto oldBrush = _dc->GetBrush();

        _dc->SetPen(*wxTRANSPARENT_PEN);
        _dc->SetBrush(wxBrush(color));

        _dc->DrawRectangle(fromDip(rectangle, _dc->GetWindow()));

        _dc->SetPen(oldPen);
        _dc->SetBrush(oldBrush);
    }
}
