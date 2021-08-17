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

    wxGraphicsBrush SolidBrush::GetGraphicsBrush(wxGraphicsRenderer* renderer)
    {
        return renderer->CreateBrush(_brush);
    }
    wxBrush SolidBrush::GetWxBrush()
    {
        return _brush;
    }
}
