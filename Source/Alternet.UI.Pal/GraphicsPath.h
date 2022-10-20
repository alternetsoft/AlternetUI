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
        wxGraphicsPath GetPath();

        wxPolygonFillMode GetWxFillMode();
        static wxPolygonFillMode GetWxFillMode(FillMode value);

    private:
        wxWindow* GetWindow();

        wxGraphicsPath _path;
        DrawingContext* _dc = nullptr;

        FillMode _fillMode = FillMode::Alternate;
    };
}
