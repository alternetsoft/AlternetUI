#include "LinearGradientBrush.h"

namespace Alternet::UI
{
    LinearGradientBrush::LinearGradientBrush()
    {
    }

    LinearGradientBrush::~LinearGradientBrush()
    {
    }

    void LinearGradientBrush::Initialize(
        const Point& startPoint,
        const Point& endPoint,
        Color* gradientStopsColors,
        int gradientStopsColorsCount,
        Coord* gradientStopsOffsets,
        int gradientStopsOffsetsCount)
    {
        if (gradientStopsColorsCount != gradientStopsOffsetsCount)
            throwExNoInfo;

        _startPoint = fromDip(startPoint, nullptr);
        _endPoint = fromDip(endPoint, nullptr);
        _gradientStops = wxGraphicsGradientStops();

        for (int i = 0; i < gradientStopsColorsCount; i++)
            _gradientStops.Add(gradientStopsColors[i], gradientStopsOffsets[i]);
    }

    wxGraphicsBrush LinearGradientBrush::GetGraphicsBrush(wxGraphicsRenderer* renderer, const wxPoint2DDouble& offset)
    {
        return renderer->CreateLinearGradientBrush(
            offset.m_x + _startPoint.x,
            offset.m_y + _startPoint.y,
            offset.m_x + _endPoint.x,
            offset.m_y + _endPoint.y,
            _gradientStops);
    }

    wxBrush LinearGradientBrush::GetWxBrush()
    {
        return *wxTRANSPARENT_BRUSH;
    }
}
