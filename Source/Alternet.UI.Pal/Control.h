#pragma once

#include "Common.h"

namespace Alternet::UI
{
    class Control
    {
    public:
        Control();
        virtual ~Control();

        virtual wxWindowBase* GetControl() = 0;

        virtual wxWindow* CreateWxWindow(wxWindow* parent);

    private:

        BYREF_ONLY(Control);
    };

}
