#pragma once
#include "Common.h"
#include "ApiTypes.h"
#include "Object.h"
#include "Image.h"

namespace Alternet::UI
{
    class DrawingContext : public Object
    {
#include "Api/DrawingContext.inc"
    public:
        DrawingContext(wxDC* dc);

    private:
        wxDC* _dc;

        SizeF _translation;
        std::stack<SizeF> _translationStack;
    };
}
