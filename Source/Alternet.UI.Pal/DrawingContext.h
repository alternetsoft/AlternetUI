#pragma once
#include "Common.h"
#include "ApiTypes.h"
#include "Object.h"
#include "Image.h"
#include "Font.h"
#include "Brush.h"
#include "Pen.h"

namespace Alternet::UI
{
    class DrawingContext : public Object
    {
#include "Api/DrawingContext.inc"
    public:
        DrawingContext(wxDC* dc);

    private:
        wxDC* _dc;
        wxGraphicsContext* _graphicsContext = nullptr;

        wxGraphicsRenderer* _dcRenderer = nullptr;
        wxGraphicsContext* _dcGraphicsContext = nullptr;

        SizeF _translation;
        std::stack<SizeF> _translationStack;

        wxGraphicsBrush GetGraphicsBrush(Brush* brush);
        wxGraphicsPen GetGraphicsPen(Pen* pen);

        bool _useDCForText = true;
    };
}
