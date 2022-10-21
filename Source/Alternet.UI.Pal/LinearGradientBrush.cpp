#include "LinearGradientBrush.h"

namespace Alternet::UI
{
    LinearGradientBrush::LinearGradientBrush()
    {
    }

    LinearGradientBrush::~LinearGradientBrush()
    {
    }

    void LinearGradientBrush::Initialize(const Point& startPoint, const Point& endPoint, Color* gradientStopsColors, int gradientStopsColorsCount, double* gradientStopsOffsets, int gradientStopsOffsetsCount)
    {
    }
    wxGraphicsBrush LinearGradientBrush::GetGraphicsBrush(wxGraphicsRenderer* renderer)
    {
        return wxGraphicsBrush();
    }
}
