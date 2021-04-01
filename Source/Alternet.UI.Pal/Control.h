#pragma once

#include "Common.h"

namespace Alternet::UI
{
    class Control
    {
#include "Api/Control.inc"
    public:
        virtual wxWindowBase* GetControl() = 0;

        virtual wxWindow* CreateWxWindow(wxWindow* parent);

    private:

    };
}
