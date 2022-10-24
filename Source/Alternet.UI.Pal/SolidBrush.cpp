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
        _brush = wxBrush(color);
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
