#pragma once
#include "Common.h"
#include "ApiTypes.h"
#include "Object.h"
#include "Image.h"
#include "Font.h"
#include "Brush.h"
#include "Pen.h"
#include "GraphicsPath.h"

namespace Alternet::UI
{
    class TextPainter;

    class DrawingContext : public Object
    {
#include "Api/DrawingContext.inc"
    public:
        DrawingContext(wxDC* dc);
        
    private:
        wxDC* _dc;
        wxGraphicsContext* _graphicsContext = nullptr;

        wxGraphicsRenderer* _dcRenderer = nullptr;

        Size _translation;
        std::stack<Size> _translationStack;

        wxGraphicsBrush GetGraphicsBrush(Brush* brush);
        wxGraphicsPen GetGraphicsPen(Pen* pen);

        TextPainter* GetTextPainter();

        bool _useDCForText = true;
    };
}
