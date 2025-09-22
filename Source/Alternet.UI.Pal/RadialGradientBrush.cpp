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
        Coord radius,
        const Point& gradientOrigin,
        Color* gradientStopsColors,
        int gradientStopsColorsCount,
        Coord* gradientStopsOffsets,
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

    wxGraphicsBrush RadialGradientBrush::GetGraphicsBrush(
        wxGraphicsRenderer* renderer,
        const wxPoint2DDouble& offset)
    {
        return renderer->CreateRadialGradientBrush(
            offset.m_x + _gradientOrigin.x,
            offset.m_y + _gradientOrigin.y,
            offset.m_x + _center.x,
            offset.m_y + _center.y,
            _radius,
            _gradientStops);
    }

    wxBrush RadialGradientBrush::GetWxBrush()
    {
        return *wxTRANSPARENT_BRUSH;
    }
}
