#pragma once
#include "Common.h"
#include "ApiTypes.h"
#include "Object.h"

namespace Alternet::UI
{
    class Mouse : public Object
    {
#include "Api/Mouse.inc"
    public:
        void OnMouseMove(wxMouseEvent& e, bool& handled);

    private:
    
    };
}
