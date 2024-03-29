#include "HatchBrush.h"

namespace Alternet::UI
{
    HatchBrush::HatchBrush()
    {
    }

    HatchBrush::~HatchBrush()
    {
    }

    void HatchBrush::Initialize(BrushHatchStyle style, const Color& color)
    {
        _brush = wxBrush(color, GetWxStyle(style));
    }

    wxGraphicsBrush HatchBrush::GetGraphicsBrush(wxGraphicsRenderer* renderer, const wxPoint2DDouble& offset)
    {
        return renderer->CreateBrush(_brush);
    }

    wxBrush HatchBrush::GetWxBrush()
    {
        return _brush;
    }

    wxBrushStyle HatchBrush::GetWxStyle(BrushHatchStyle style)
    {
        switch (style)
        {
        case BrushHatchStyle::BackwardDiagonal:
            return wxBrushStyle::wxBRUSHSTYLE_BDIAGONAL_HATCH;
        case BrushHatchStyle::ForwardDiagonal:
            return wxBrushStyle::wxBRUSHSTYLE_FDIAGONAL_HATCH;
        case BrushHatchStyle::DiagonalCross:
            return wxBrushStyle::wxBRUSHSTYLE_CROSSDIAG_HATCH;
        case BrushHatchStyle::Cross:
            return wxBrushStyle::wxBRUSHSTYLE_CROSS_HATCH;
        case BrushHatchStyle::Horizontal:
            return wxBrushStyle::wxBRUSHSTYLE_HORIZONTAL_HATCH;
        case BrushHatchStyle::Vertical:
        default:
            return wxBrushStyle::wxBRUSHSTYLE_VERTICAL_HATCH;
        }
    }
}
