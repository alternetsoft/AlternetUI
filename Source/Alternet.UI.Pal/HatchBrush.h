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
        virtual wxGraphicsBrush GetGraphicsBrush(wxGraphicsRenderer* renderer) override;

        wxBrush GetWxBrush();

    private:
        wxBrushStyle GetWxStyle(BrushHatchStyle style);
        wxColor GetBackgroundColor(BrushHatchStyle style);

        wxColor _backgroundColor;
        wxBrush _brush;
    };
}
