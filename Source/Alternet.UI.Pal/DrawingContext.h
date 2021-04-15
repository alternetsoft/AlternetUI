#pragma once
#include "Common.h"
#include "ApiTypes.h"
#include "Object.h"

namespace Alternet::UI
{
    class DrawingContext : public Object
    {
#include "Api/DrawingContext.inc"
    public:
        DrawingContext(wxDC* dc);

    private:
        wxDC* _dc;
    };
}
