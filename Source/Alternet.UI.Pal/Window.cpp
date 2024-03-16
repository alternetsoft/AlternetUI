#include "Window.h"
#include "Application.h"
#include "IdManager.h"
#include "NotifyIcon.h"
#include "Button.h"

#include <wx/gdicmn.h>
#include <wx/window.h>

namespace Alternet::UI
{
#define UseDebugPaintColors false

    Frame::Frame(wxWindow* parent,
        wxWindowID id,
        const wxString& title,
        const wxPoint& pos,
        const wxSize& size,
        long style,
        const wxString& name)
            : wxFrame(parent, id, title, pos, size, style, name)
    {
    }

    // ------------

    Window::Window()
        : Window(0)
    {
    }

    void* Window::CreateEx(int kind)
    {
        auto result = new Window(kind);
        return result;
    }

    Window::Window(int kind):
        _frameKind(kind),
        _flags(
            WindowFlags::ShowInTaskbar |
            WindowFlags::SystemMenu |
            WindowFlags::CloseEnabled |
            WindowFlags::HasBorder |
            WindowFlags::HasTitleBar |
            WindowFlags::MaximizeEnabled |
            WindowFlags::MinimizeEnabled |
            WindowFlags::Resizable),
        _delayedFlags(
            *this,
            DelayedWindowFlags::None,
            &Control::IsWxWindowCreated,
            {
            }),
        _title(*this, u"", &Control::IsWxWindowCreated, &Window::RetrieveTitle, 
            &Window::ApplyTitle),
        _menu(*this, nullptr, &Control::IsWxWindowCreated, &Window::RetrieveMenu,   
            &Window::ApplyMenu),
        _toolbar(*this, nullptr, &Control::IsWxWindowCreated, &Window::RetrieveToolbar, 
            &Window::ApplyToolbar)
    {
        GetDelayedValues().Add(&_title);
        GetDelayedValues().Add(&_delayedFlags);
        SetVisible(false);

        CreateWxWindow();
    }

    Window::~Window()
    {
        if (_icon != nullptr)
            _icon->Release();
    }

    void Window::AddInputBinding(const string& managedCommandId, Key key,
        ModifierKeys modifiers)
    {
        if (managedCommandId.empty())
        {
            DebugLogInvalidArg(managedCommandId, u"Command ID must not be empty.");
            return;
        }

        if (_acceleratorsByCommandIds.find(managedCommandId) !=
            _acceleratorsByCommandIds.end())
        {
            DebugLogInvalidArg(managedCommandId,
                u"Input binding with this command ID was already added to this window.");
        }

        auto keyboard = Application::GetCurrent()->GetKeyboardInternal();
        auto wxKey = keyboard->KeyToWxKey(key);
        auto acceleratorFlags = keyboard->ModifierKeysToAcceleratorFlags(modifiers);

        _acceleratorsByCommandIds[managedCommandId] =
            wxAcceleratorEntry(acceleratorFlags, wxKey, IdManager::AllocateId());

        UpdateAcceleratorTable(GetWxWindow());
    }

    void Window::RemoveInputBinding(const string& managedCommandId)
    {
        if (managedCommandId.empty())
        {
            DebugLogInvalidArg(managedCommandId, u"Command ID must not be empty.");
            return;
        }

        auto it = _acceleratorsByCommandIds.find(managedCommandId);
        if (it == _acceleratorsByCommandIds.end()) 
        {
            DebugLogInvalidArg(managedCommandId,
                u"Input binding with this command ID was not found in this window.");
            return;
        }

        IdManager::FreeId(it->second.GetCommand());

        _acceleratorsByCommandIds.erase(it);

        UpdateAcceleratorTable(GetWxWindow());
    }

    void Window::UpdateAcceleratorTable(wxWindow* frame)
    {
        if (frame == nullptr)
            return;

        std::vector<wxAcceleratorEntry> entries;
        for (auto& pair : _acceleratorsByCommandIds)
            entries.push_back(pair.second);

        frame->SetAcceleratorTable(entries.empty() ?
            wxNullAcceleratorTable : wxAcceleratorTable(entries.size(), &entries[0]));
    }

    void Window::OnCommand(wxCommandEvent& event)
    {
        auto id = event.GetId();

        optional<string> foundManagedCommandId;
        for (auto& pair : _acceleratorsByCommandIds)
        {
            if (pair.second.GetCommand() == id)
                foundManagedCommandId = pair.first;
        }

        if (!foundManagedCommandId.has_value())
            return;

        CommandEventData data{ const_cast<char16_t*>(foundManagedCommandId.value().c_str()) };
        bool handled = RaiseEvent(WindowEvent::InputBindingCommandExecuted, &data);
        if (!handled)
            event.Skip();
    }

    void Window::OnCharHook(wxKeyEvent& event)
    {
        if (event.m_keyCode == WXK_ESCAPE && _cancelButton != nullptr)
            _cancelButton->RaiseClick();
        else
        if (event.m_keyCode == WXK_RETURN && _acceptButton != nullptr)
            _acceptButton->RaiseClick();
        else
            event.Skip();
    }

    MainMenu* Window::GetMenu()
    {
        return _menu.Get();
    }

    void Window::SetMenu(MainMenu* value)
    {
        _storedMenu = value;
        _menu.Set(value);
    }

    Toolbar* Window::GetToolbar()
    {
        return _toolbar.Get();
    }

    void Window::SetToolbar(Toolbar* value)
    {
        _storedToolbar = value;
        _toolbar.Set(value);
    }

    MainMenu* Window::RetrieveMenu()
    {
        return _storedMenu;
    }

    void Window::ApplyMenu(MainMenu* const& value)
    {
        auto frame = GetFrame();
        if (frame == nullptr)
            return;
        frame->SetMenuBar(value == nullptr ? nullptr : value->GetWxMenuBar());
        frame->Layout();
        frame->PostSizeEvent();
    }

    Toolbar* Window::RetrieveToolbar()
    {
        return _storedToolbar;
    }

    void Window::ApplyToolbar(Toolbar* const& value)
    {
        if (value != nullptr)
            value->SetOwnerWindow(this);
        auto frame = GetFrame();
        if (frame == nullptr)
            return;
        frame->SetToolBar(value == nullptr ? nullptr : value->GetWxToolBar());
        frame->Layout();
        frame->PostSizeEvent();
    }

    void* Window::GetWxStatusBar()
    {
        auto wxWindow = GetFrame();
        if (wxWindow == nullptr)
            return nullptr;
        return wxWindow->GetStatusBar();
    }

    void Window::SetWxStatusBar(void* value)
    {
        auto wxWindow = GetFrame();
        if (wxWindow == nullptr)
            return;
        wxWindow->SetStatusBar((wxStatusBar*)value);
        wxWindow->Layout();
        wxWindow->PostSizeEvent();
    }

    void Window::OnWxWindowDestroyed(wxWindow* window)
    {
        Control::OnWxWindowDestroyed(window);

        bool recreatingWxWindow = IsRecreatingWxWindow();

        /*if (GetModal())
        {
            if (!recreatingWxWindow)
                _flags.Set(WindowFlags::ModalLoopStopRequested, true);

            if (_modalWindowDisabler != nullptr)
            {
                delete _modalWindowDisabler;
                _modalWindowDisabler = nullptr;

                _modalWindows.pop();
                if (!_modalWindows.empty())
                    _modalWindowDisabler = new FrameDisabler(_modalWindows.top()->_frame);
            }
        }*/

        if (_flags.IsSet(WindowFlags::Modal) && !recreatingWxWindow)
            _flags.Set(WindowFlags::Modal, false);

        auto parent = GetParent();
        if (parent != nullptr && !recreatingWxWindow)
            parent->RemoveChild(this);
    }

    void Window::OnBeforeDestroyWxWindow()
    {
        Control::OnBeforeDestroyWxWindow();

        auto wxWindow = GetWxWindow();
        wxWindow->Unbind(wxEVT_SIZE, &Window::OnSizeChanged, this);
        wxWindow->Unbind(wxEVT_MOVE, &Window::OnMove, this);
        wxWindow->Unbind(wxEVT_CLOSE_WINDOW, &Window::OnClose, this);
        wxWindow->Unbind(wxEVT_MAXIMIZE, &Window::OnMaximize, this);
        wxWindow->Unbind(wxEVT_ICONIZE, &Window::OnIconize, this);
        wxWindow->Unbind(wxEVT_MENU, &Window::OnCommand, this);
        wxWindow->Unbind(wxEVT_CHAR_HOOK, &Window::OnCharHook, this);
    }
            
    std::vector<Window*> Window::GetOwnedWindows()
    {
        std::vector<Window*> result;

        for (auto child : GetChildren())
        {
            auto childWindow = dynamic_cast<Window*>(child);
            if (childWindow != nullptr)
                result.push_back(childWindow);
        }

        return result;
    }

    void Window::ShowCore()
    {
        bool hasHiddenOwner = GetParent() != nullptr && !GetParent()->GetVisible();

        if (!hasHiddenOwner)
            Control::ShowCore();

        for (auto child : GetOwnedWindows())
        {
            if (_preservedHiddenOwnedWindows.find(child) == _preservedHiddenOwnedWindows.end())
                child->SetVisible(true);
        }

        _preservedHiddenOwnedWindows.clear();
    }

    void Window::HideCore()
    {
        Control::HideCore();
        
        _preservedHiddenOwnedWindows.clear();
        for (auto child : GetOwnedWindows())
        {
            if (!child->GetVisible())
                _preservedHiddenOwnedWindows.insert(child);

            child->SetVisible(false);
        }
    }

    string Window::GetTitle()
    {
        return _title.Get();
    }

    void Window::SetTitle(const string& value)
    {
        _title.Set(value);
    }

    string Window::RetrieveTitle()
    {
        return wxStr(GetTopLevelWindow()->GetTitle());
    }

    void Window::ApplyTitle(const string& value)
    {
        GetTopLevelWindow()->SetTitle(wxStr(value));
    }

    WindowStartLocation Window::GetWindowStartLocation()
    {
        return _startLocation;
    }

    void Window::SetWindowStartLocation(WindowStartLocation value)
    {
        _startLocation = value;
    }

    bool Window::GetIsPopupWindow()
    {
        return _flags.IsSet(WindowFlags::PopupWindow);
    }

    void Window::SetIsPopupWindow(bool value)
    {
        if (GetIsPopupWindow() == value)
            return;

        _flags.Set(WindowFlags::PopupWindow, value);
        ScheduleRecreateWxWindow();
    }

    long Window::GetWindowStyle()
    {
        long style = wxCLIP_CHILDREN;

        if (GetIsPopupWindow())
            style |= wxPOPUP_WINDOW;

        if(GetHasSystemMenu())
            style |= wxSYSTEM_MENU;

        if (GetMinimizeEnabled())
            style |= wxMINIMIZE_BOX;

        if (GetMaximizeEnabled())
            style |= wxMAXIMIZE_BOX;

        if (GetCloseEnabled())
            style |= wxCLOSE_BOX;

        if (GetAlwaysOnTop())
            style |= wxSTAY_ON_TOP;

        if (GetIsToolWindow())
            style |= wxFRAME_TOOL_WINDOW;

        if (GetResizable())
            style |= wxRESIZE_BORDER;

        if (!GetHasBorder())
            style |= wxBORDER_NONE;
        else
            style |= _borderStyle;

        if (GetHasTitleBar())
            style |= wxCAPTION;

        if (!GetShowInTaskbar())
            style |= wxFRAME_NO_TASKBAR;

        return style;
    }

    wxWindow* Window::CreateWxWindowUnparented()
    {
        return CreateWxWindowCore(nullptr);
    }

    void Window::SetDefaultBounds(const RectD& bounds)
    {
        _defaultBounds = bounds;
    }

    wxWindow* Window::CreateWxWindowCore(wxWindow* parent)
    {
#define KindWindow 0
#define KindDialog 1
#define KindMiniFrame 2

        auto style = GetWindowStyle();

        wxPoint position = wxDefaultPosition;
        wxSize size = wxDefaultSize;

        auto bounds = GetBounds();

        if (bounds.IsEmpty())
            bounds = _defaultBounds;

        if (!bounds.IsEmpty())
        {
            wxRect rect(fromDip(bounds, nullptr));
            position = wxPoint(rect.x, rect.y);
            size = wxSize(rect.width, rect.height);
        }

        wxTopLevelWindow* frame;

        switch(_frameKind)
        {
        case KindWindow:
        default:
            frame = new Frame(nullptr,
                wxID_ANY,
                "",
                position,
                size,
                style);
            break;
        case KindMiniFrame:
            frame = new MiniFrame(nullptr,
                wxID_ANY,
                "",
                position,
                size,
                style);
            break;
        case KindDialog:
            frame = new Dialog(parent,
                wxID_ANY,
                "",
                position,
                size,
                style);
            break;
        }

        ApplyIcon(frame);
        UpdateAcceleratorTable(frame);

        frame->Bind(wxEVT_SIZE, &Window::OnSizeChanged, this);
        frame->Bind(wxEVT_MOVE, &Window::OnMove, this);
        frame->Bind(wxEVT_CLOSE_WINDOW, &Window::OnClose, this);
        frame->Bind(wxEVT_MAXIMIZE, &Window::OnMaximize, this);
        frame->Bind(wxEVT_ICONIZE, &Window::OnIconize, this);
        frame->Bind(wxEVT_MENU, &Window::OnCommand, this);
        frame->Bind(wxEVT_CHAR_HOOK, &Window::OnCharHook, this);

        auto panelColor =
            wxSystemSettings::GetColour(wxSystemColour::wxSYS_COLOUR_BTNFACE);
        frame->SetBackgroundColour(panelColor);

        return frame;
    }

    void Window::SetAcceptButton(Button* button)
    {
        if (_acceptButton != nullptr)
            _acceptButton->Release();

        _acceptButton = button;

        if (_acceptButton != nullptr)
            _acceptButton->AddRef();
    }

    Button* Window::GetAcceptButton()
    {
        return _acceptButton;
    }

    void Window::SetCancelButton(Button* button)
    {
        if (_cancelButton != nullptr)
            _cancelButton->Release();

        _cancelButton = button;

        if (_cancelButton != nullptr)
            _cancelButton->AddRef();
    }

    Button* Window::GetCancelButton()
    {
        return _cancelButton;
    }

    ModalResult Window::GetModalResult()
    {
        return _modalResult;
    }

    void Window::SetModalResult(ModalResult value)
    {
        _modalResult = value;

        auto dialog = GetDialog();

        if (dialog == nullptr || !dialog->IsModal())
            return;

        if (_modalResult == ModalResult::Accepted)
        {
            dialog->EndModal(wxID_OK);
            return;
        }

        if (_modalResult == ModalResult::Canceled)
        {
            dialog->EndModal(wxID_CANCEL);
            return;
        }
    }

    bool Window::GetModal()
    {
        return _flags.IsSet(WindowFlags::Modal);
    }

    void Window::ShowModal(void* owner)
    {
        while (IsRecreatingWxWindow())
        {
            wxMilliSleep(1);
            wxTheApp->GetMainLoop()->Yield();
        }

        _flags.Set(WindowFlags::Modal, true);

        auto dialog = GetDialog();

        if (dialog == nullptr)
            return;

        dialog->ShowModal();

        _flags.Set(WindowFlags::Modal, false);
    }

    void Window::Close()
    {
        GetTopLevelWindow()->Close();
    }

    bool Window::GetShowInTaskbar()
    {
        return _flags.IsSet(WindowFlags::ShowInTaskbar);
    }

    void Window::SetShowInTaskbar(bool value)
    {
        if (GetShowInTaskbar() == value)
            return;

        _flags.Set(WindowFlags::ShowInTaskbar, value);
        ScheduleRecreateWxWindow();
    }

    bool Window::GetMinimizeEnabled()
    {
        return _flags.IsSet(WindowFlags::MinimizeEnabled);
    }

    void Window::SetMinimizeEnabled(bool value)
    {
        if (GetMinimizeEnabled() == value)
            return;

        _flags.Set(WindowFlags::MinimizeEnabled, value);
        ScheduleRecreateWxWindow();
    }

    bool Window::GetMaximizeEnabled()
    {
        return _flags.IsSet(WindowFlags::MaximizeEnabled);
    }

    void Window::SetMaximizeEnabled(bool value)
    {
        if (GetMaximizeEnabled() == value)
            return;

        _flags.Set(WindowFlags::MaximizeEnabled, value);
        ScheduleRecreateWxWindow();
    }

    bool Window::GetCloseEnabled()
    {
        return _flags.IsSet(WindowFlags::CloseEnabled);
    }

    void Window::SetCloseEnabled(bool value)
    {
        if (GetCloseEnabled() == value)
            return;

        _flags.Set(WindowFlags::CloseEnabled, value);
        ScheduleRecreateWxWindow();
    }

    void Window::UpdateWxWindowParent()
    {
    }

    IconSet* Window::GetIcon()
    {
        if (_icon != nullptr)
            _icon->AddRef();
        return _icon;
    }

    void Window::SetIcon(IconSet* value)
    {
        if (_icon == value)
            return;
        if (_icon != nullptr)
            _icon->Release();
        _icon = value;
        if (_icon != nullptr)
            _icon->AddRef();
        if (IsWxWindowCreated())
        {
            if(_icon == nullptr)
                ScheduleRecreateWxWindow();
            else
                ApplyIcon(GetTopLevelWindow());
        }
    }

    void Window::ApplyIcon(wxTopLevelWindow* value)
    {
        if (_icon == nullptr)
        {

        }
        else
        {
            if (value == nullptr)
                return;
            value->SetIcons(IconSet::IconBundle(_icon));
        }
    }

    WindowState Window::GetState()
    {
        auto frame = GetTopLevelWindow();
        if (frame->IsMaximized())
            return WindowState::Maximized;

        if (frame->IsIconized())
            return WindowState::Minimized;

        return WindowState::Normal;
    }

    void Window::SetState(WindowState value)
    {
        if (GetState() == value)
            return;

        auto frame = GetTopLevelWindow();
        if (value == WindowState::Maximized)
            frame->Maximize();
        else if (value == WindowState::Minimized)
            frame->Iconize();
        else if (value == WindowState::Normal)
        {
            if (frame->IsMaximized())
                frame->Maximize(false);
            else if (frame->IsIconized())
                frame->Iconize(false);
        }
    }

    void* Window::OpenOwnedWindowsArray()
    {
        return new std::vector<Window*>(GetOwnedWindows());
    }

    int Window::GetOwnedWindowsItemCount(void* array)
    {
        return ((std::vector<Window*>*)array)->size();
    }

    Window* Window::GetOwnedWindowsItemAt(void* array, int index)
    {
        auto window = (*((std::vector<Window*>*)array))[index];
        window->AddRef();
        return window;
    }

    void Window::CloseOwnedWindowsArray(void* array)
    {
        delete (std::vector<Window*>*)array;
    }

    /*static*/ Window* Window::GetActiveWindow()
    {
        wxTopLevelWindow* window = NULL;
        wxWindowList::compatibility_iterator node = wxTopLevelWindows.GetFirst();
        while (node)
        {
            wxWindow* win = node->GetData();
            if (!wxPendingDelete.Member(win))
            {
                auto topLevelWindow = dynamic_cast<wxTopLevelWindow*>(win);
                if (topLevelWindow != nullptr
                    && topLevelWindow->IsActive() && topLevelWindow->IsVisible())
                {
                    window = topLevelWindow;
                    break;
                }
            }
            node = node->GetNext();
        }

        if (window == nullptr)
            return nullptr;

        auto extender = dynamic_cast<wxWidgetExtender*>(window);
        auto palControl = extender->_palControl;
        auto result = dynamic_cast<Window*>(palControl);
        result->AddRef();
        return result;
    }

    void Window::Activate()
    {
        GetWxWindow()->SetFocus();
    }

    bool Window::GetAlwaysOnTop()
    {
        return _flags.IsSet(WindowFlags::AlwaysOnTop);
    }

    void Window::SetAlwaysOnTop(bool value)
    {
        if (GetAlwaysOnTop() == value)
            return;

        _flags.Set(WindowFlags::AlwaysOnTop, value);
        ScheduleRecreateWxWindow();
    }

    bool Window::GetIsToolWindow()
    {
        return _flags.IsSet(WindowFlags::IsToolWindow);
    }

    void Window::SetIsToolWindow(bool value)
    {
        if (GetIsToolWindow() == value)
            return;

        _flags.Set(WindowFlags::IsToolWindow, value);
        ScheduleRecreateWxWindow();
    }

    void Window::OnClose(wxCloseEvent& event)
    {
        bool cancel = RaiseEvent(WindowEvent::Closing);

        if (cancel)
            event.Veto();
        else
        {
            event.Skip();

            auto parent = GetParent();

            if (parent != nullptr) 
            {
                parent->RemoveChild(this);
            }

            for (auto window : GetOwnedWindows())
            {
                RemoveChild(window);
                window->Close();
            }
        }
    }

    bool Window::GetResizable()
    {
        return _flags.IsSet(WindowFlags::Resizable);
    }

    void Window::OnSizeChanged(wxSizeEvent& event)
    {
        event.Skip();
        RaiseEvent(WindowEvent::SizeChanged);

        auto newState = GetState();
        if (_lastState != newState)
        {
            _lastState = newState;
            RaiseEvent(WindowEvent::StateChanged);
        }
    }

    void Window::OnMove(wxMoveEvent& event)
    {
        event.Skip();
        RaiseEvent(WindowEvent::LocationChanged);
    }

    void Window::SetResizable(bool value)
    {
        if (GetResizable() == value)
            return;

        _flags.Set(WindowFlags::Resizable, value);
        ScheduleRecreateWxWindow();
    }

    void Window::OnMaximize(wxMaximizeEvent& event)
    {
        event.Skip();
        _lastState = GetState();
        RaiseEvent(WindowEvent::StateChanged);
    }

    void Window::OnIconize(wxIconizeEvent& event)
    {
        event.Skip();
        _lastState = GetState();
        RaiseEvent(WindowEvent::StateChanged);
    }

    bool Window::GetHasBorder()
    {
        return _flags.IsSet(WindowFlags::HasBorder);
    }

    void Window::SetHasBorder(bool value)
    {
        if (GetHasBorder() == value)
            return;

        _flags.Set(WindowFlags::HasBorder, value);
        ScheduleRecreateWxWindow();
    }

    wxTopLevelWindow* Window::GetTopLevelWindow()
    {
        return dynamic_cast<wxTopLevelWindow*>(GetWxWindow());
    }

    Frame* Window::GetFrame()
    {
        return dynamic_cast<Frame*>(GetWxWindow());
    }

    wxDialog* Window::GetDialog()
    {
        return dynamic_cast<Dialog*>(GetWxWindow());
    }

    bool Window::GetHasSystemMenu()
    {
        return _flags.IsSet(WindowFlags::SystemMenu);
    }
    
    void Window::SetHasSystemMenu(bool value)
    {
        if (GetHasSystemMenu() == value)
            return;

        _flags.Set(WindowFlags::SystemMenu, value);
        ScheduleRecreateWxWindow();
    }

    bool Window::GetHasTitleBar()
    {
        return _flags.IsSet(WindowFlags::HasTitleBar);
    }
    
    void Window::SetHasTitleBar(bool value)
    {
        if (GetHasTitleBar() == value)
            return;

        _flags.Set(WindowFlags::HasTitleBar, value);
        ScheduleRecreateWxWindow();
    }

    class wxStockGDIOverride : public wxStockGDI, public wxModule
    {
    public:
        inline static wxStockGDI* old_instance = nullptr;

        virtual const wxFont* GetFont(Item item) wxOVERRIDE;

        virtual bool OnInit() wxOVERRIDE;
        virtual void OnExit() wxOVERRIDE;

    private:
        typedef wxStockGDI super;
        wxDECLARE_DYNAMIC_CLASS(wxStockGDIOverride);
    };

    wxIMPLEMENT_DYNAMIC_CLASS(wxStockGDIOverride, wxModule);

    bool wxStockGDIOverride::OnInit()
    {
        old_instance = ms_instance;
        // Override default instance
        ms_instance = this;
        return true;
    }

    void wxStockGDIOverride::OnExit()
    {
    }

    static bool stockGridHooked = false;
    static wxStockGDIOverride* StockGDIOverride;

    const wxFont* wxStockGDIOverride::GetFont(Item item)
    {
        if (item == FONT_NORMAL && Window::fontOverride.IsOk())
        {
            auto font = &Window::fontOverride;
            ms_stockObject[item] = font;
            return font;
        }
        else
        {
            auto font = const_cast<wxFont*>(super::GetFont(item));
            return font;
        }
    }

    void Window::SetParkingWindowFont(Font* font)
    {
        if (!stockGridHooked)
        {
            StockGDIOverride = new wxStockGDIOverride();
            stockGridHooked = true;
        }

        if (font == nullptr)
            fontOverride = wxNullFont;
        else
            fontOverride = font->GetWxFont();

        auto parkingWindow = ParkingWindow::GetWindow();
        if (parkingWindow != nullptr)
            parkingWindow->SetFont(fontOverride);
    }
}
