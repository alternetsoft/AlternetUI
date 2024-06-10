#pragma once
#include "Common.h"
#include "ApiTypes.h"
#include "Object.h"
#include "Control.h"

namespace Alternet::UI
{
    class Mouse : public Object
    {
#include "Api/Mouse.inc"
    public:
        void OnMouse(int eventKind, wxMouseEvent& e, bool& handled);
    private:
        Control* GetEventTargetControl(wxEvent& e);
    };
}
