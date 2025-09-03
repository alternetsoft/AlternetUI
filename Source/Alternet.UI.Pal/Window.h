#pragma once

#include "Common.h"
#include "Control.h"
#include "ImageSet.h"
#include "IconSet.h"
#include "MainMenu.h"
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

        bool Layout() override
        {
            return false;
        }
/*
        virtual wxWindow* GetMainWindowOfCompositeControl() override
        {
            auto result = wxFrame::GetMainWindowOfCompositeControl();
            if (result == nullptr)
                return nullptr;
            if(!result->IsEnabled())
                return nullptr;
            return result;
        }
*/

    private:
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
        bool Layout() override
        {
            return false;
        }
    };

    // Dialog =============================================

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
        bool Layout() override
        {
            return false;
        }
    };

    // Window =============================================

    class Window : public Control
    {
#include "Api/Window.inc"
    public:
        Window(int kind);

        inline static wxFont fontOverride = wxNullFont;

        wxWindow* CreateWxWindowCore(wxWindow* parent) override;
        wxWindow* CreateWxWindowUnparented() override;

        wxTopLevelWindow* GetTopLevelWindow();
        Frame* GetFrame();
        wxDialog* GetDialog();
    protected:
        void OnBeforeDestroyWxWindow() override;
        void OnWxWindowDestroyed(wxWindow* window) override;
        virtual void OnActivate(wxActivateEvent& event) override;

        void UpdateWxWindowParent() override;

        void CreateWxWindow() override
        {
            Control::CreateWxWindow();
        }

        virtual void OnWxWindowCreated() override;
    private:
        int _frameKind = 0;

        ModalResult _modalResult = ModalResult::None;

        void OnClose(wxCloseEvent& event);
        void OnSizeChanged(wxSizeEvent& event) override;
        void OnMaximize(wxMaximizeEvent& event);
        void OnIconize(wxIconizeEvent& event);

        string RetrieveTitle();
        void ApplyTitle(const string& value);

        MainMenu* RetrieveMenu();
        void ApplyMenu(MainMenu* const& value);

        long GetWindowStyle();

        void ApplyIcon(wxTopLevelWindow* value);

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
            SystemMenu = 1 << 11,
            PopupWindow = 1 << 12,
        };

        DelayedFlags<Window, DelayedWindowFlags> _delayedFlags;
        FlagsAccessor<WindowFlags> _flags;
        DelayedValue<Window, string> _title;
        WindowState _state = WindowState::Normal;
        DelayedValue<Window, MainMenu*> _menu;
        MainMenu* _storedMenu = nullptr;
        IconSet* _icon = nullptr;
        WindowState _lastState = WindowState::Normal;

        inline static RectD _defaultBounds = RectD(0, 0, 0, 0);
    };
}

template<> struct enable_bitmask_operators<Alternet::UI::Window::DelayedWindowFlags>
{ static const bool enable = true; };

template<> struct enable_bitmask_operators<Alternet::UI::Window::WindowFlags>
{ static const bool enable = true; };
