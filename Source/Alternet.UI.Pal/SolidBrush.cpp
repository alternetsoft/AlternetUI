#include "SolidBrush.h"

namespace Alternet::UI
{
    SolidBrush::SolidBrush()
    {
    }

    SolidBrush::~SolidBrush()
    {
    }

    void SolidBrush::Initialize(const Color& color)
    {
        wxColour wxColor = color;

        if (!wxColor.IsOk())
        {
            wxColor.Set(0, 0, 0, 0);
        }

        _brush = wxBrush(wxColor);
    }

    wxGraphicsBrush SolidBrush::GetGraphicsBrush(wxGraphicsRenderer* renderer, const wxPoint2DDouble& offset)
    {
        return renderer->CreateBrush(_brush);
    }

    wxBrush SolidBrush::GetWxBrush()
    {
        return _brush;
    }
}
