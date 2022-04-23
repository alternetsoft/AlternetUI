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

        Size _translation;
        std::stack<Size> _translationStack;

        wxGraphicsBrush GetGraphicsBrush(Brush* brush);
        wxGraphicsPen GetGraphicsPen(Pen* pen);

        bool _useDCForText = true;

        void ManualFloodFill(wxPoint point, wxColor seedColor, wxColor color);
    };
}
