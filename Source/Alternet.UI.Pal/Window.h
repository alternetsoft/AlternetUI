#pragma once

#include "Common.h"
#include "Control.h"

namespace Alternet::UI
{
    class Frame : public wxFrame
    {
    public:
        Frame(long style);

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
        void OnSizeChanged(wxSizeEvent& event);
        void OnDestroy(wxWindowDestroyEvent& event);

        int GetTopLevelWindowsCount();
        wxWindow* GetNextTopLevelWindow();

        enum class DelayedWindowFlags
        {
            None = 0,
            Todo = 1 << 0
        };

        enum class WindowFlags
        {
            None = 0,
            ShowInTaskbar = 1 << 0
        };

        DelayedFlags<Window, DelayedWindowFlags> _delayedFlags;

        FlagsAccessor<WindowFlags> _flags;
    };
}

template<> struct enable_bitmask_operators<Alternet::UI::Window::DelayedWindowFlags> { static const bool enable = true; };
template<> struct enable_bitmask_operators<Alternet::UI::Window::WindowFlags> { static const bool enable = true; };
