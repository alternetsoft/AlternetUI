#include "HatchBrush.h"

namespace Alternet::UI
{
    HatchBrush::HatchBrush()
    {
    }

    HatchBrush::~HatchBrush()
    {
    }

    void HatchBrush::Initialize(BrushHatchStyle style, const Color& foregroundColor, const Color& backgroundColor)
    {
        _backgroundColor = backgroundColor;
        _brush = wxBrush(foregroundColor, GetWxStyle(style));
    }

    wxGraphicsBrush HatchBrush::GetGraphicsBrush(wxGraphicsRenderer* renderer)
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
            return wxBrushStyle::wxBRUSHSTYLE_VERTICAL_HATCH;
        default:
            wxASSERT(false);
            throw 0;
        }
    }

    wxColor HatchBrush::GetBackgroundColor(BrushHatchStyle style)
    {
        return _backgroundColor;
    }
}
