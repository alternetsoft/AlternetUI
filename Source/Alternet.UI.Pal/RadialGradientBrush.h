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
        Point _gradientOrigin;
        Point _center;
        double _radius = 0;
        wxGraphicsGradientStops _gradientStops;
    };
}
