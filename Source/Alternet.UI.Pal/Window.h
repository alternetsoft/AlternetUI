#pragma once

#include "Common.h"
#include "Control.h"
#include "ImageSet.h"

namespace Alternet::UI
{
    class Window;

    class Frame : public wxFrame
    {
    public:
        Frame(Window* window, long style);
        virtual ~Frame();

        static std::vector<Frame*> GetAllFrames();

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
        wxWindow* GetParentingWxWindow(Control* child) override;

        Color RetrieveBackgroundColor() override;
        void ApplyBackgroundColor(const Color& value) override;

    private:

        Frame* _frame = nullptr;
        wxPanel* _panel = nullptr;

        void OnClose(wxCloseEvent& event);
        void OnSizeChanged(wxSizeEvent& event);
        void OnDestroy(wxWindowDestroyEvent& event);
        void OnActivate(wxActivateEvent& event);
        void OnMaximize(wxMaximizeEvent& event);
        void OnIconize(wxIconizeEvent& event);

        Frame* GetFrame();

        string RetrieveTitle();
        void ApplyTitle(const string& value);

        WindowState RetrieveState();
        void ApplyState(const WindowState& value);

        long GetWindowStyle();

        void ApplyIcon(Frame* value);

        enum class DelayedWindowFlags
        {
            None = 0,
            Todo = 1 << 0
        };

        enum class WindowFlags
        {
            None = 0,
            ShowInTaskbar = 1 << 0,
            MinimizeEnabled = 1 << 1,
            MaximizeEnabled = 1 << 2,
            CloseEnabled = 1 << 3,
            AlwaysOnTop = 1 << 4,
            IsToolWindow = 1 << 5,
            Resizable = 1 << 6,
            HasBorder = 1 << 7,
            HasTitleBar = 1 << 8,
            DestroyingWindow = 1 << 9,
            Active = 1 << 10
        };

        DelayedFlags<Window, DelayedWindowFlags> _delayedFlags;

        FlagsAccessor<WindowFlags> _flags;

        DelayedValue<Window, string> _title;
        DelayedValue<Window, WindowState> _state;

        ImageSet* _icon = nullptr;
        
        WindowState _lastState = WindowState::Normal;
    };
}

template<> struct enable_bitmask_operators<Alternet::UI::Window::DelayedWindowFlags> { static const bool enable = true; };
template<> struct enable_bitmask_operators<Alternet::UI::Window::WindowFlags> { static const bool enable = true; };
