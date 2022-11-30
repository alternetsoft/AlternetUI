#pragma once

#include "Common.h"
#include "Control.h"
#include "ImageSet.h"
#include "MainMenu.h"
#include "Toolbar.h"

namespace Alternet::UI
{
    class Window;
    class Button;

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

        void SetAcceptButton(Button* button);
        Button* GetAcceptButton();

        void SetCancelButton(Button* button);
        Button* GetCancelButton();

    protected:
        Color RetrieveBackgroundColor() override;
        void ApplyBackgroundColor(const Color& value) override;

        void ApplyBounds(const Rect& value) override;

        void OnBeforeDestroyWxWindow() override;
        void OnWxWindowDestroyed(wxWindow* window) override;

        void ShowCore() override;
        void HideCore() override;

        void UpdateWxWindowParent() override;
    private:

        Button* _acceptButton = nullptr;
        Button* _cancelButton = nullptr;

        std::map<string, wxAcceleratorEntry> _acceleratorsByCommandIds;

        ModalResult _modalResult = ModalResult::None;

        Frame* _frame = nullptr;
        wxPanel* _panel = nullptr;

        void UpdateAcceleratorTable();

        void OnClose(wxCloseEvent& event);
        void OnSizeChanged(wxSizeEvent& event);
        void OnMove(wxMoveEvent& event);
        void OnActivate(wxActivateEvent& event);
        void OnMaximize(wxMaximizeEvent& event);
        void OnIconize(wxIconizeEvent& event);
        void OnCommand(wxCommandEvent& event);
        void OnCharHook(wxKeyEvent& event);

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

        MainMenu* RetrieveMenu();
        void ApplyMenu(MainMenu* const& value);

        Toolbar* RetrieveToolbar();
        void ApplyToolbar(Toolbar* const& value);

        long GetWindowStyle();

        void ApplyIcon(Frame* value);

        void ApplyDefaultLocation();

        std::vector<Window*> GetOwnedWindows();

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
            Active = 1 << 9,
            Modal = 1 << 10,
            ModalLoopStopRequested = 1 << 11,
            ShownOnce = 1 << 12
        };

        DelayedFlags<Window, DelayedWindowFlags> _delayedFlags;

        FlagsAccessor<WindowFlags> _flags;

        DelayedValue<Window, string> _title;
        DelayedValue<Window, WindowState> _state;

        DelayedValue<Window, Size> _minimumSize;
        DelayedValue<Window, Size> _maximumSize;
        
        DelayedValue<Window, MainMenu*> _menu;
        DelayedValue<Window, Toolbar*> _toolbar;

        MainMenu* _storedMenu = nullptr;
        Toolbar* _storedToolbar = nullptr;

        Size _appliedMinimumSize;
        Size _appliedMaximumSize;

        ImageSet* _icon = nullptr;
        
        WindowState _lastState = WindowState::Normal;

        inline static FrameDisabler* _modalWindowDisabler = nullptr;
        inline static std::stack<Window*> _modalWindows;

        WindowStartLocation _startLocation = WindowStartLocation::Default;

        std::set<Window*> _preservedHiddenOwnedWindows;
    };
}

template<> struct enable_bitmask_operators<Alternet::UI::Window::DelayedWindowFlags> { static const bool enable = true; };
template<> struct enable_bitmask_operators<Alternet::UI::Window::WindowFlags> { static const bool enable = true; };
