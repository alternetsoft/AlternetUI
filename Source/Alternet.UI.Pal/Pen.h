#pragma once
#include "Common.h"
#include "ApiTypes.h"
#include "Object.h"

namespace Alternet::UI
{
    class Pen : public Object
    {
#include "Api/Pen.inc"
    public:
        wxGraphicsPen GetGraphicsPen(wxGraphicsRenderer* renderer);

        wxPen GetWxPen();

    private:
        wxPenStyle GetWxStyle(PenDashStyle style);
        wxPenCap GetWxPenCap(LineCap value);
        wxPenJoin GetWxPenJoin(LineJoin value);

        wxPen _pen;
    };
}
