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
    public:
        Window();
        virtual ~Window();

        string GetTitle();
        void SetTitle(const string& value);

        void Show();

        void AddChildControl(Control* value);
    private:
        Frame* _frame;

        BYREF_ONLY(Window);
    };
}
