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

    private:
        wxWindow* GetWindow();

        void MoveToIfNeeded(wxPoint point);

        wxGraphicsPath _path;
        DrawingContext* _dc = nullptr;

        bool _startFigure = false;
    };
}
