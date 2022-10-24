#pragma once
#include "Common.h"
#include "ApiTypes.h"
#include "Brush.h"

namespace Alternet::UI
{
    class RadialGradientBrush : public Brush
    {
#include "Api/RadialGradientBrush.inc"
    public:
        virtual wxGraphicsBrush GetGraphicsBrush(wxGraphicsRenderer* renderer) override;

    private:
        wxPoint _gradientOrigin;
        wxPoint _center;
        int _radius = 0;
        wxGraphicsGradientStops _gradientStops;
    };
}
