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
        void OnMouseDown(wxMouseEvent& e, MouseButton changedButton, bool& handled);
        void OnMouseUp(wxMouseEvent& e, MouseButton changedButton, bool& handled);
        void OnMouseWheel(wxMouseEvent& e, bool& handled);

    private:
    
    };
}
