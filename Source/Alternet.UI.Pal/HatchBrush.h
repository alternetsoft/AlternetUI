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

        wxBrush GetWxBrush();

    private:
        wxBrushStyle GetWxStyle(BrushHatchStyle style);

        wxBrush _brush;
    };
}
