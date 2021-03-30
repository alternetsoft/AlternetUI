#pragma once

#include "Common.h"
#include "Control.h"

namespace Alternet::UI
{
    class Frame : public wxFrame
    {
    public:
        Frame();

    private:

        BYREF_ONLY(Frame);
    };

    class Window
    {
#include "Api/Window.inc"
    public:

    private:
        Frame* _frame;
    };
}
