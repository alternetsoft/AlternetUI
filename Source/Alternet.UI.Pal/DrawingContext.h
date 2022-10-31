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
#include "Region.h"

namespace Alternet::UI
{
    class TextPainter;

    class DrawingContext : public Object
    {
#include "Api/DrawingContext.inc"
    public:
        DrawingContext(wxDC* dc, optional<std::function<void()>> onUseDC = nullopt);
        
        wxGraphicsContext* GetGraphicsContext();
        wxDC* GetDC();

        void SetDoNotDeleteDC(bool value);

        static wxWindow* GetWindow(wxDC* dc);

    private:
        void SetTransformCore(const wxAffineMatrix2D& value);
        void ApplyTransform(bool useDC);

        void UseDC();
        void UseGC();

        bool NeedToUseDC();

        std::stack<wxAffineMatrix2D> _transformStack;

        wxAffineMatrix2D _currentTransform;
        wxPoint _currentTranslation;

        wxDC* _dc;
        wxGraphicsContext* _graphicsContext = nullptr;

        wxGraphicsRenderer* _dcRenderer = nullptr;

        wxGraphicsBrush GetGraphicsBrush(Brush* brush, const wxPoint2DDouble& offset);
        wxGraphicsPen GetGraphicsPen(Pen* pen);

        TextPainter* GetTextPainter();

        Region* _clip = nullptr;

        bool _nonIdentityTransformSet = false;

        bool _doNotDeleteDC = false;

        optional<std::function<void()>> _onUseDC;
    };
}
