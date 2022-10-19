#pragma once
#include "Common.h"
#include "ApiTypes.h"
#include "Object.h"
#include "Image.h"
#include "Font.h"
#include "Brush.h"
#include "Pen.h"
#include "GraphicsPath.h"
#include "TransformMatrix.h"

namespace Alternet::UI
{
    class TextPainter;

    class DrawingContext : public Object
    {
#include "Api/DrawingContext.inc"
    public:
        DrawingContext(wxDC* dc);
        
        wxGraphicsContext* GetGraphicsContext();
        wxDC* GetDC();

        static wxWindow* GetWindow(wxDC* dc);

    private:
        void SetTransformCore(const wxAffineMatrix2D& value);

        std::stack<wxAffineMatrix2D> _transformStack;

        wxDC* _dc;
        wxGraphicsContext* _graphicsContext = nullptr;

        wxGraphicsRenderer* _dcRenderer = nullptr;

        wxGraphicsBrush GetGraphicsBrush(Brush* brush);
        wxGraphicsPen GetGraphicsPen(Pen* pen);

        TextPainter* GetTextPainter();

        bool _useDCForText = true;
    };
}
