#pragma once
#include "Common.h"
#include "ApiTypes.h"
#include "Object.h"
#include "Brush.h"
#include "Image.h"

namespace Alternet::UI
{
    class TextureBrush : public Brush
    {
#include "Api/TextureBrush.inc"
    public:
        virtual wxGraphicsBrush GetGraphicsBrush(wxGraphicsRenderer* renderer, const wxPoint2DDouble& offset) override;

        virtual wxBrush GetWxBrush() override;

    private:
        wxBrush _brush;
    };
}
