#pragma once
#include "Common.h"
#include "ApiTypes.h"
#include "Object.h"
#include "Image.h"
#include "Font.h"

namespace Alternet::UI
{
    // https://docs.wxwidgets.org/3.2/classwx_d_c.html
    // https://docs.wxwidgets.org/3.2/classwx_graphics_context.html

    class TextPainter;                            
                                          
    class DrawingContext : public Object
    {
#include "Api/DrawingContext.inc"
    public:                                
        DrawingContext(wxDC* dc, bool useDirectDraw = true);
        DrawingContext(wxGraphicsContext* graphicsContext, wxDC* dc);

        wxGraphicsContext* GetGraphicsContext();     
        wxDC* GetDC();

        void SetDoNotDeleteDC(bool value);

        static wxWindow* GetWindow(wxDC* dc);

    private:
        static wxInterpolationQuality GetInterpolationQuality(InterpolationMode mode);

        wxAffineMatrix2D _currentTransform;

        InterpolationMode _interpolationMode = InterpolationMode::HighQuality;

        wxDC* _dc = nullptr;
        wxGraphicsContext* _graphicsContext = nullptr;

        bool _doNotDeleteDC = false;
    };
}
