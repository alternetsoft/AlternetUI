#include "Pen.h"

namespace Alternet::UI
{
    Pen::Pen()
    {
    }

    Pen::~Pen()
    {
    }

    void Pen::Initialize(
        DashStyle style,
        const Color& color,
        Coord width,
        LineCap lineCap,
        LineJoin lineJoin)
    {
        wxColour wxColor = color;

        if (!wxColor.IsOk())
        {
            wxColor = *wxBLACK;
        }

        _pen = wxPen(wxColor, fromDip(width, nullptr), GetWxStyle(style));
        _pen.SetCap(GetWxPenCap(lineCap));
        _pen.SetJoin(GetWxPenJoin(lineJoin));
    }

    wxGraphicsPen Pen::GetGraphicsPen(wxGraphicsRenderer* renderer)
    {
        wxGraphicsPenInfo info(_pen.GetColour(), _pen.GetWidth(), _pen.GetStyle());
        info.Cap(_pen.GetCap());
        info.Join(_pen.GetJoin());
        
        return renderer->CreatePen(info);
    }

    wxPen Pen::GetWxPen()
    {
        return _pen;
    }

    wxPenStyle Pen::GetWxStyle(DashStyle style)
    {
        switch (style)
        {
            case DashStyle::Solid:
                return wxPenStyle::wxPENSTYLE_SOLID;
            case DashStyle::Dash:
                return wxPenStyle::wxPENSTYLE_SHORT_DASH;
            case DashStyle::Dot:
                return wxPenStyle::wxPENSTYLE_DOT;
            case DashStyle::DashDot:
            case DashStyle::DashDotDot:
                return wxPenStyle::wxPENSTYLE_DOT_DASH;
            case DashStyle::Custom:
                return wxPenStyle::wxPENSTYLE_USER_DASH;
            default:
                return wxPenStyle::wxPENSTYLE_SOLID;
        }
    }

    wxPenCap Pen::GetWxPenCap(LineCap value)
    {
        switch (value)
        {
        case LineCap::Flat:
            return wxPenCap::wxCAP_BUTT;
        case LineCap::Square:
            return wxPenCap::wxCAP_PROJECTING;
        case LineCap::Round:
            return wxPenCap::wxCAP_ROUND;
        default:
            throwExInvalidOp;
        }
    }

    wxPenJoin Pen::GetWxPenJoin(LineJoin value)
    {
        switch (value)
        {
        case LineJoin::Miter:
            return wxPenJoin::wxJOIN_MITER;
        case LineJoin::Bevel:
            return wxPenJoin::wxJOIN_BEVEL;
        case LineJoin::Round:
            return wxPenJoin::wxJOIN_ROUND;
        default:
            throwExInvalidOp;
        }
    }
}
