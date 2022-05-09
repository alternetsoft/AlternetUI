#pragma once

#include "Common.h"
#include "Control.h"

namespace Alternet::UI
{
    class Window;

    class Frame : public wxFrame
    {
    public:
        Frame(Window* window, long style);
        virtual ~Frame();

        Window* GetWindow();

    private:
        Window* _window;

        inline static std::vector<Frame*> _allFrames;

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
        void OnActivate(wxActivateEvent& event);

        wxWindow* GetNextTopLevelWindow();

        Frame* GetFrame();

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
