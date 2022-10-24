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
        _gradientStops = wxGraphicsGradientStops(gradientStopsColors[0], gradientStopsColors[gradientStopsColorsCount - 1]);
        
        for (int i = 1; i < gradientStopsColorsCount - 1; i++)
            _gradientStops.Add(gradientStopsColors[i], gradientStopsOffsets[i]);

        auto s = _gradientStops.Item(0);
        auto s1 = _gradientStops.Item(1);
    }

    wxGraphicsBrush RadialGradientBrush::GetGraphicsBrush(wxGraphicsRenderer* renderer)
    {
        auto s = _gradientStops.Item(0);

        wxGraphicsGradientStops ss(*wxRED, *wxGREEN);

        return renderer->CreateRadialGradientBrush(
            _center.x,
            _center.y,
            _gradientOrigin.x,
            _gradientOrigin.y,
            _radius,
            ss);
            //_gradientStops);
    }
}
