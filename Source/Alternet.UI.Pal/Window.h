#pragma once

#include "Common.h"
#include "Control.h"
#include "ImageSet.h"
#include "IconSet.h"
#include "MainMenu.h"
#include "Toolbar.h"
#include "StatusBar.h"

#include <wx/minifram.h>

namespace Alternet::UI
{
    class Window;
    class Button;

    // Frame =============================================

    class Frame : public wxFrame, public wxWidgetExtender
    {
    public:
        Frame(wxWindow* parent,
            wxWindowID id,
            const wxString& title,
            const wxPoint& pos = wxDefaultPosition,
            const wxSize& size = wxDefaultSize,
            long style = wxDEFAULT_FRAME_STYLE,
            const wxString& name = wxASCII_STR(wxFrameNameStr));
        /*virtual ~Frame();*/

        /*static std::vector<wxTopLevelWindow*> GetAllFrames();*/

        /*Window* GetWindow();*/
        
        /*void RemoveFrame();*/

        bool Layout() override
        {
            return false;
        }
    private:
        /*Window* _window;
        bool _frameRemoved = false;

        inline static std::vector<wxTopLevelWindow*> _allFrames;

        BYREF_ONLY(Frame);*/
    };

    class MiniFrame : public wxMiniFrame, public wxWidgetExtender
    {
    public:
        MiniFrame(wxWindow* parent,
            wxWindowID id,
            const wxString& title,
            const wxPoint& pos = wxDefaultPosition,
            const wxSize& size = wxDefaultSize,
            long style = wxCAPTION | wxCLIP_CHILDREN | wxRESIZE_BORDER,
            const wxString& name = wxASCII_STR(wxFrameNameStr))
            : wxMiniFrame(parent, id, title, pos, size, style,name)
        {
        }
    };

    class Dialog : public wxDialog, public wxWidgetExtender
    {
    public:
        Dialog(wxWindow* parent, wxWindowID id,
            const wxString& title,
            const wxPoint& pos = wxDefaultPosition,
            const wxSize& size = wxDefaultSize,
            long style = wxDEFAULT_DIALOG_STYLE,
            const wxString& name = wxASCII_STR(wxDialogNameStr))
            : wxDialog(parent, id, title, pos, size, style, name)
        {
        }
    };

    // FrameDisabler =============================================

    /*class FrameDisabler
    {
    public:
        FrameDisabler(wxTopLevelWindow* frameToSkip);
        virtual ~FrameDisabler();

    private:

        std::vector<wxTopLevelWindow*> _disabledFrames;

        BYREF_ONLY(FrameDisabler);
    };*/

    // Window =============================================

    class Window : public Control
    {
#include "Api/Window.inc"
    public:
        Window(int kind);

        inline static wxFont fontOverride = wxNullFont;

        wxWindow* CreateWxWindowCore(wxWindow* parent) override;
        wxWindow* CreateWxWindowUnparented() override;

        void SetAcceptButton(Button* button);
        Button* GetAcceptButton();

        void SetCancelButton(Button* button);
        Button* GetCancelButton();

        wxTopLevelWindow* GetTopLevelWindow();
        Frame* GetFrame();
        wxDialog* GetDialog();
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
        int _frameKind = 0;

        std::map<string, wxAcceleratorEntry> _acceleratorsByCommandIds;

        ModalResult _modalResult = ModalResult::None;

        //Frame* _frame = nullptr;

        void UpdateAcceleratorTable(wxWindow* frame);

        void OnClose(wxCloseEvent& event);
        void OnSizeChanged(wxSizeEvent& event);
        void OnMove(wxMoveEvent& event);
        void OnMaximize(wxMaximizeEvent& event);
        void OnIconize(wxIconizeEvent& event);
        void OnCommand(wxCommandEvent& event);
        void OnCharHook(wxKeyEvent& event);

        string RetrieveTitle();
        void ApplyTitle(const string& value);

        WindowState RetrieveState();
        void ApplyState(const WindowState& value);

        MainMenu* RetrieveMenu();
        void ApplyMenu(MainMenu* const& value);

        Toolbar* RetrieveToolbar();
        void ApplyToolbar(Toolbar* const& value);

        long GetWindowStyle();

        void ApplyIcon(wxTopLevelWindow* value);

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
            /*ModalLoopStopRequested = 1 << 11,*/
            ShownOnce = 1 << 12,
            SystemMenu = 1 << 13,
            PopupWindow = 1 << 14,
        };

        DelayedFlags<Window, DelayedWindowFlags> _delayedFlags;

        FlagsAccessor<WindowFlags> _flags;

        DelayedValue<Window, string> _title;
        DelayedValue<Window, WindowState> _state;

        DelayedValue<Window, MainMenu*> _menu;
        DelayedValue<Window, Toolbar*> _toolbar;

        MainMenu* _storedMenu = nullptr;
        Toolbar* _storedToolbar = nullptr;
        IconSet* _icon = nullptr;
        WindowState _lastState = WindowState::Normal;

        inline static RectD _defaultBounds = RectD(0, 0, 0, 0);
        /*inline static FrameDisabler* _modalWindowDisabler = nullptr;*/
        /*inline static std::stack<Window*> _modalWindows;*/

        WindowStartLocation _startLocation = WindowStartLocation::Default;

        std::set<Window*> _preservedHiddenOwnedWindows;
    };
}

template<> struct enable_bitmask_operators<Alternet::UI::Window::DelayedWindowFlags> { static const bool enable = true; };
template<> struct enable_bitmask_operators<Alternet::UI::Window::WindowFlags> { static const bool enable = true; };
