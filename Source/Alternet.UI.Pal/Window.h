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

        void OnPaint(wxPaintEvent& event);

        Frame* _frame;

        wxPanel* _panel;
        wxButton* _button;
        wxTextCtrl* _textBox;
    };
}
