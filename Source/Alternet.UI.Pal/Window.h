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

    class FrameDisabler
    {
    public:
        FrameDisabler(wxFrame* frameToSkip);
        virtual ~FrameDisabler();

    private:

        std::vector<wxFrame*> _disabledFrames;

        BYREF_ONLY(FrameDisabler);
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

        void ApplyBounds(const Rect& value) override;

        void OnWxWindowDestroying() override;

        void ShowCore() override;

    private:

        ModalResult _modalResult = ModalResult::None;

        Frame* _frame = nullptr;
        wxPanel* _panel = nullptr;

        void OnClose(wxCloseEvent& event);
        void OnSizeChanged(wxSizeEvent& event);
        void OnMove(wxMoveEvent& event);
        void OnActivate(wxActivateEvent& event);
        void OnMaximize(wxMaximizeEvent& event);
        void OnIconize(wxIconizeEvent& event);

        Frame* GetFrame();

        optional<Size> CoerceSize(const Size& value);

        string RetrieveTitle();
        void ApplyTitle(const string& value);

        WindowState RetrieveState();
        void ApplyState(const WindowState& value);

        Size RetrieveMinimumSize();
        void ApplyMinimumSize(const Size& value);

        Size RetrieveMaximumSize();
        void ApplyMaximumSize(const Size& value);

        long GetWindowStyle();

        void ApplyIcon(Frame* value);

        void ApplyDefaultLocation();

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
            Active = 1 << 10,
            Modal = 1 << 11,
            ModalLoopStopRequested = 1 << 12,
            ShownOnce = 1 << 13
        };

        DelayedFlags<Window, DelayedWindowFlags> _delayedFlags;

        FlagsAccessor<WindowFlags> _flags;

        DelayedValue<Window, string> _title;
        DelayedValue<Window, WindowState> _state;

        DelayedValue<Window, Size> _minimumSize;
        DelayedValue<Window, Size> _maximumSize;

        Size _appliedMinimumSize;
        Size _appliedMaximumSize;

        ImageSet* _icon = nullptr;
        
        WindowState _lastState = WindowState::Normal;

        inline static FrameDisabler* _modalWindowDisabler = nullptr;
        inline static std::stack<Window*> _modalWindows;

        WindowStartLocation _startLocation = WindowStartLocation::Default;
    };
}

template<> struct enable_bitmask_operators<Alternet::UI::Window::DelayedWindowFlags> { static const bool enable = true; };
template<> struct enable_bitmask_operators<Alternet::UI::Window::WindowFlags> { static const bool enable = true; };
