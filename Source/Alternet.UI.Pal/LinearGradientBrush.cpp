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
        double* gradientStopsOffsets,
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

    wxGraphicsBrush LinearGradientBrush::GetGraphicsBrush(wxGraphicsRenderer* renderer)
    {
        return renderer->CreateLinearGradientBrush(
            _startPoint.x,
            _startPoint.y,
            _endPoint.x,
            _endPoint.y,
            _gradientStops);
    }
}
