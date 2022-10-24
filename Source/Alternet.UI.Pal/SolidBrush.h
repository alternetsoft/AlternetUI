#pragma once
#include "Common.h"
#include "ApiTypes.h"
#include "Brush.h"

namespace Alternet::UI
{
    class SolidBrush : public Brush
    {
#include "Api/SolidBrush.inc"
    public:
        virtual wxGraphicsBrush GetGraphicsBrush(wxGraphicsRenderer* renderer, const wxPoint2DDouble& offset) override;

        wxBrush GetWxBrush();

    private:
        wxBrush _brush;
    };
}
