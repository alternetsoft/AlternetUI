#pragma once
#include "Common.h"
#include "ApiTypes.h"
#include "Brush.h"

namespace Alternet::UI
{
    class HatchBrush : public Brush
    {
#include "Api/HatchBrush.inc"
    public:
        virtual wxGraphicsBrush GetGraphicsBrush(wxGraphicsRenderer* renderer, const wxPoint2DDouble& offset) override;

        virtual wxBrush GetWxBrush() override;

    private:
        wxBrushStyle GetWxStyle(BrushHatchStyle style);

        wxBrush _brush;
    };
}
