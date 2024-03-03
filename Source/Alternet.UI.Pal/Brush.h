#pragma once
#include "Common.h"
#include "ApiTypes.h"
#include "Object.h"

namespace Alternet::UI
{
    class Brush : public Object
    {
#include "Api/Brush.inc"
    public:
        virtual wxGraphicsBrush GetGraphicsBrush(
            wxGraphicsRenderer* renderer,
            const wxPoint2DDouble& offset)
        {
            return renderer->CreateBrush(*wxTRANSPARENT_BRUSH);
        }

        virtual wxBrush GetWxBrush()
        {
            return *wxTRANSPARENT_BRUSH;
        }
    private:
    
    };
}
