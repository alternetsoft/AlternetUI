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

    class Window : public Control
    {
#include "Api/Window.inc"
    public:
        wxWindow* CreateWxWindowCore(wxWindow* parent) override;

    protected:
        wxWindow* GetParentingWxWindow() override;

        Color RetrieveBackgroundColor() override;
        void ApplyBackgroundColor(const Color& value) override;

    private:

        Frame* _frame = nullptr;
        wxPanel* _panel = nullptr;

        void OnClose(wxCloseEvent& event);
        void OnDestroy(wxWindowDestroyEvent& event);
    };
}
