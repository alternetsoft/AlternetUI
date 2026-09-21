#include "TextureBrush.h"

namespace Alternet::UI
{
    TextureBrush::TextureBrush()
    {
    }

    TextureBrush::~TextureBrush()
    {
    }

    void TextureBrush::Initialize(Image* image)
    {
        if (image == nullptr)
        {
            _brush = *wxTRANSPARENT_BRUSH;
            return;
        }

        auto bitmap = Image::GetWxBitmap(image);
        _brush = wxBrush(bitmap);
    }

    wxGraphicsBrush TextureBrush::GetGraphicsBrush(wxGraphicsRenderer* renderer, const wxPoint2DDouble& offset)
    {
        return renderer->CreateBrush(_brush);
    }

    wxBrush TextureBrush::GetWxBrush()
    {
        return _brush;
    }

}
