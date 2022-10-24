#pragma once
#include "Common.h"
#include "ApiTypes.h"
#include "Brush.h"

namespace Alternet::UI
{
    class LinearGradientBrush : public Brush
    {
#include "Api/LinearGradientBrush.inc"
    public:
        virtual wxGraphicsBrush GetGraphicsBrush(wxGraphicsRenderer* renderer, const wxPoint2DDouble& offset) override;

    private:
        wxPoint _startPoint;
        wxPoint _endPoint;
        wxGraphicsGradientStops _gradientStops;
    };
}
