#pragma once
#include "Common.h"
#include "ApiTypes.h"
#include "Object.h"

namespace Alternet::UI
{
    class DrawingContext;

    class GraphicsPath : public Object
    {
#include "Api/GraphicsPath.inc"
    public:
        GraphicsPath(wxDC* dc, wxGraphicsContext* graphicsConext);

        wxGraphicsPath GetPath();

        wxPolygonFillMode GetWxFillMode();
        static wxPolygonFillMode GetWxFillMode(FillMode value);

    private:
        wxWindow* GetWindow();

        void InitializePath(wxGraphicsContext* graphicsConext);

        wxGraphicsPath _path;
        
        wxDC* _dc = nullptr;
        wxGraphicsContext* _graphicsConext = nullptr;

        FillMode _fillMode = FillMode::Alternate;
    };
}
