#include "RadialGradientBrush.h"

namespace Alternet::UI
{
    RadialGradientBrush::RadialGradientBrush()
    {
    }

    RadialGradientBrush::~RadialGradientBrush()
    {
    }

    void RadialGradientBrush::Initialize(
        const Point& center,
        double radius,
        const Point& gradientOrigin,
        Color* gradientStopsColors,
        int gradientStopsColorsCount,
        double* gradientStopsOffsets,
        int gradientStopsOffsetsCount)
    {
        if (gradientStopsColorsCount != gradientStopsOffsetsCount)
            throwExNoInfo;

        _center = fromDip(center, nullptr);
        _radius = fromDip(radius, nullptr);
        _gradientOrigin = fromDip(gradientOrigin, nullptr);
        _gradientStops = wxGraphicsGradientStops();
        
        for (int i = 0; i < gradientStopsColorsCount; i++)
            _gradientStops.Add(gradientStopsColors[i], gradientStopsOffsets[i]);
    }

    wxGraphicsBrush RadialGradientBrush::GetGraphicsBrush(wxGraphicsRenderer* renderer)
    {
        return renderer->CreateRadialGradientBrush(
            _gradientOrigin.x,
            _gradientOrigin.y,
            _center.x,
            _center.y,
            _radius,
            _gradientStops);
    }
}
