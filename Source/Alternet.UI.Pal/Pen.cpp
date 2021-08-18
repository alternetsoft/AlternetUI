#include "Pen.h"

namespace Alternet::UI
{
    Pen::Pen()
    {
    }

    Pen::~Pen()
    {
    }

    void Pen::Initialize(PenDashStyle style, const Color& color, float width)
    {
        _pen = wxPen(color, fromDip(width, nullptr), GetWxStyle(style));
    }

    wxGraphicsPen Pen::GetGraphicsPen(wxGraphicsRenderer* renderer)
    {
        return renderer->CreatePen(wxGraphicsPenInfo(_pen.GetColour(), _pen.GetWidth(), _pen.GetStyle()));
    }

    wxPen Pen::GetWxPen()
    {
        return _pen;
    }

    wxPenStyle Pen::GetWxStyle(PenDashStyle style)
    {
        switch (style)
        {
        case PenDashStyle::Solid:
            return wxPenStyle::wxPENSTYLE_SOLID;
        case PenDashStyle::Dot:
            return wxPenStyle::wxPENSTYLE_DOT;
        case PenDashStyle::LongDash:
            return wxPenStyle::wxPENSTYLE_LONG_DASH;
        case PenDashStyle::ShortDash:
            return wxPenStyle::wxPENSTYLE_SHORT_DASH;
        case PenDashStyle::DashDot:
            return wxPenStyle::wxPENSTYLE_DOT_DASH;
        case PenDashStyle::Custom:
            return wxPenStyle::wxPENSTYLE_USER_DASH;
        default:
            wxASSERT(false);
            throw 0;
        }
    }
}
