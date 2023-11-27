#include "Window.h"
#include "Application.h"
#include "IdManager.h"
#include "NotifyIcon.h"
#include "Button.h"

#include <wx/gdicmn.h>

namespace Alternet::UI
{
#define UseDebugPaintColors false

    Frame::Frame(Window* window, long style) :
        wxFrame(NULL, wxID_ANY, "", wxDefaultPosition, wxDefaultSize, style),
            _window(window)
    {
        _allFrames.push_back(this);
        /* Application::Log(std::to_string(_allFrames.size())); */
    }

    void Frame::RemoveFrame()
    {
        if (_frameRemoved)
            return;
        _frameRemoved = true;

        /*Application::Log(std::to_string(_allFrames.size()));*/

        // Ensure the parking window is closed after all regular windows have been closed.
        _allFrames.erase(std::find(_allFrames.begin(), _allFrames.end(), this));

        if (!ParkingWindow::IsCreated())
            return;

        if (wxTheApp->GetTopWindow() == ParkingWindow::GetWindow())
        {
            if (_allFrames.size() == 0)
            {
                NotifyIcon::DestroyAllNotifyIcons();
                ParkingWindow::Destroy();
            }
            else
            {
                auto win = _allFrames[0];
                if (win->CanBeFocused())
                    win->SetFocus();
            }
        }
    }

    Frame::~Frame()
    {
        RemoveFrame();
    }

    /*static*/ std::vector<Frame*> Frame::GetAllFrames()
    {
        return _allFrames;
    }

    Window* Frame::GetWindow()
    {
        return _window;
    }

    // ------------

    FrameDisabler::FrameDisabler(wxFrame* frameToSkip)
    {
        for (auto frame : Frame::GetAllFrames())
        {
            if (frame != frameToSkip && frame->IsEnabled())
            {
                frame->Enable(false);
                _disabledFrames.push_back(frame);
            }
        }
    }

    FrameDisabler::~FrameDisabler()
    {
        for (auto frame : _disabledFrames)
        {
            frame->Enable(true);
        }
    }

    // ------------

    Window::Window():
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
                //{DelayedWindowFlags::ShowInTaskbar, std::make_tuple(&Window::RetrieveShowInTaskbar,
                //  &Window::ApplyShowInTaskbar)},
            }),
        _title(*this, u"", &Control::IsWxWindowCreated, &Window::RetrieveTitle, 
            &Window::ApplyTitle),
        _state(*this, WindowState::Normal, &Control::IsWxWindowCreated, 
            &Window::RetrieveState, &Window::ApplyState),
        _menu(*this, nullptr, &Control::IsWxWindowCreated, &Window::RetrieveMenu,   
            &Window::ApplyMenu),
        _toolbar(*this, nullptr, &Control::IsWxWindowCreated, &Window::RetrieveToolbar, 
            &Window::ApplyToolbar)
    {
        GetDelayedValues().Add(&_title);
        GetDelayedValues().Add(&_state);
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
            throwExInvalidArg(managedCommandId, u"Command ID must not be empty.");

        if (_acceleratorsByCommandIds.find(managedCommandId) != 
            _acceleratorsByCommandIds.end())
            throwExInvalidArg(managedCommandId,
                u"Input binding with this command ID was already added to this window.");

        auto keyboard = Application::GetCurrent()->GetKeyboardInternal();
        auto wxKey = keyboard->KeyToWxKey(key);
        auto acceleratorFlags = keyboard->ModifierKeysToAcceleratorFlags(modifiers);

        _acceleratorsByCommandIds[managedCommandId] =
            wxAcceleratorEntry(acceleratorFlags, wxKey, IdManager::AllocateId());

        UpdateAcceleratorTable();
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

        UpdateAcceleratorTable();
    }

    void Window::UpdateAcceleratorTable()
    {
        auto frame = _frame;
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

    WindowState Window::RetrieveState()
    {
        if (_frame->IsMaximized())
            return WindowState::Maximized;

        if (_frame->IsIconized())
            return WindowState::Minimized;

        return WindowState::Normal;
    }

    void Window::ApplyState(const WindowState& value)
    {
        if (value == WindowState::Maximized)
            _frame->Maximize();
        else if (value == WindowState::Minimized)
            _frame->Iconize();
        else if (value == WindowState::Normal)
        {
            if (_frame->IsMaximized())
                _frame->Maximize(false);
            else if (_frame->IsIconized())
                _frame->Iconize(false);
        }
    }

    MainMenu* Window::RetrieveMenu()
    {
        return _storedMenu;
    }

    void Window::ApplyMenu(MainMenu* const& value)
    {
        _frame->SetMenuBar(value == nullptr ? nullptr : value->GetWxMenuBar());
        _frame->Layout();
        _frame->PostSizeEvent();
    }

    Toolbar* Window::RetrieveToolbar()
    {
        return _storedToolbar;
    }

    void Window::ApplyToolbar(Toolbar* const& value)
    {
        if (value != nullptr)
            value->SetOwnerWindow(this);
        _frame->SetToolBar(value == nullptr ? nullptr : value->GetWxToolBar());
        _frame->Layout();
        _frame->PostSizeEvent();
    }

    void* Window::GetWxStatusBar()
    {
        auto wxWindow = GetFrame();
        return wxWindow->GetStatusBar();
    }

    void Window::SetWxStatusBar(void* value)
    {
        auto wxWindow = GetFrame();
        wxWindow->SetStatusBar((wxStatusBar*)value);
        wxWindow->Layout();
        wxWindow->PostSizeEvent();
    }

    void Window::OnWxWindowDestroyed(wxWindow* window)
    {
        Control::OnWxWindowDestroyed(window);

        bool recreatingWxWindow = IsRecreatingWxWindow();

        if (GetModal())
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
        }

        if (_flags.IsSet(WindowFlags::Modal) && !recreatingWxWindow)
            _flags.Set(WindowFlags::Modal, false);

        auto parent = GetParent();
        if (parent != nullptr && !recreatingWxWindow)
            parent->RemoveChild(this);
    }

    void Window::OnBeforeDestroyWxWindow()
    {
        Control::OnBeforeDestroyWxWindow();

        auto wxWindow = GetFrame();

        wxWindow->RemoveFrame();

        wxWindow->Unbind(wxEVT_SIZE, &Window::OnSizeChanged, this);
        wxWindow->Unbind(wxEVT_MOVE, &Window::OnMove, this);
        wxWindow->Unbind(wxEVT_CLOSE_WINDOW, &Window::OnClose, this);
        wxWindow->Unbind(wxEVT_ACTIVATE, &Window::OnActivate, this);
        wxWindow->Unbind(wxEVT_MAXIMIZE, &Window::OnMaximize, this);
        wxWindow->Unbind(wxEVT_ICONIZE, &Window::OnIconize, this);
        wxWindow->Unbind(wxEVT_MENU, &Window::OnCommand, this);
        wxWindow->Unbind(wxEVT_CHAR_HOOK, &Window::OnCharHook, this);

        _frame = nullptr;
    }

    void Window::ApplyBounds(const Rect& value)
    {
        if (value.IsEmpty())
            return;

    //    Control::ApplyBounds(value);
        auto wxWindow = GetWxWindow();
        auto rect = fromDip(value, wxWindow);


        if (_startLocation == WindowStartLocation::Manual ||
            _flags.IsSet(WindowFlags::ShownOnce))
        {
            wxWindow->SetSize(rect);
            Control::LogRectMethod("ApplyBounds", value, rect);
        }
        else
        {
            wxWindow->SetSize(rect.width, rect.height);
            Control::LogRectMethod("ApplyBounds WH", value, rect);
        }

        wxWindow->Refresh();
    }

    void Window::ApplyDefaultLocation()
    {
        auto wxWindow = GetWxWindow();

        switch (_startLocation)
        {
        case WindowStartLocation::Default:
            break;
        case WindowStartLocation::Manual:
            break;
        case WindowStartLocation::CenterScreen:
            wxWindow->Center();
            break;
        case WindowStartLocation::CenterOwner:
            wxWindow->CenterOnParent();
            break;
        }
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
        if (!_flags.IsSet(WindowFlags::ShownOnce))
        {
            _flags.Set(WindowFlags::ShownOnce, true);
            ApplyDefaultLocation();
        }

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
        return wxStr(_frame->GetTitle());
    }

    void Window::ApplyTitle(const string& value)
    {
        _frame->SetTitle(wxStr(value));
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

    wxWindow* Window::CreateWxWindowCore(wxWindow* parent)
    {
        auto style = GetWindowStyle();

        _frame = new Frame(this, style);

        ApplyIcon(_frame);
        UpdateAcceleratorTable();

        _frame->Bind(wxEVT_SIZE, &Window::OnSizeChanged, this);
        _frame->Bind(wxEVT_MOVE, &Window::OnMove, this);
        _frame->Bind(wxEVT_CLOSE_WINDOW, &Window::OnClose, this);
        _frame->Bind(wxEVT_ACTIVATE, &Window::OnActivate, this);
        _frame->Bind(wxEVT_MAXIMIZE, &Window::OnMaximize, this);
        _frame->Bind(wxEVT_ICONIZE, &Window::OnIconize, this);
        _frame->Bind(wxEVT_MENU, &Window::OnCommand, this);
        _frame->Bind(wxEVT_CHAR_HOOK, &Window::OnCharHook, this);

        auto panelColor =
            wxSystemSettings::GetColour(wxSystemColour::wxSYS_COLOUR_BTNFACE);

        if(UseDebugPaintColors)
            panelColor = wxTheColourDatabase->Find("RED");
        _frame->SetBackgroundColour(panelColor);

        return _frame;
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
    }

    bool Window::GetModal()
    {
        return _flags.IsSet(WindowFlags::Modal);
    }

    void Window::ShowModal()
    {
        while (IsRecreatingWxWindow())
        {
            wxMilliSleep(1);
            wxTheApp->GetMainLoop()->Yield();
        }

        _flags.Set(WindowFlags::Modal, true);

        if (_modalWindowDisabler != nullptr)
            delete _modalWindowDisabler;

        _modalWindowDisabler = new FrameDisabler(_frame);
        _modalWindows.push(this);

        SetVisible(true);

        // HACK: because wxWidgets doesnt support modal windows with menu,
        // we have to simulate modal loop but processing messages and sleeping.
        // Will need to implement this properly somehow.
        
        while (!_flags.IsSet(WindowFlags::ModalLoopStopRequested))
        {

#ifdef __WXOSX_COCOA__
            unsigned long delay = 20;
#else
            unsigned long delay = 1;
#endif

            wxMilliSleep(delay);
            wxTheApp->GetMainLoop()->Yield();
        }

        _flags.Set(WindowFlags::ModalLoopStopRequested, false);
    }

    void Window::Close()
    {
        _frame->Close();
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

    Color Window::RetrieveBackgroundColor()
    {
        return _frame->GetBackgroundColour();
    }

    void Window::ApplyBackgroundColor(const Color& value)
    {
        //_frame->SetBackgroundColour(value);
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
                ApplyIcon(GetFrame());
        }
    }

    void Window::ApplyIcon(Frame* value)
    {
        if (_icon == nullptr)
        {

        }
        else
        {
            value->SetIcons(IconSet::IconBundle(_icon));
        }
    }

    WindowState Window::GetState()
    {
        return _state.Get();
    }

    void Window::SetState(WindowState value)
    {
        _state.Set(value);
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

    bool Window::GetIsActive()
    {
        return _flags.IsSet(WindowFlags::Active);
    }

    /*static*/ Window* Window::GetActiveWindow()
    {
        auto allFrames = Frame::GetAllFrames();
        for (auto frame : allFrames)
        {
            auto window = frame->GetWindow();
            if (window->GetIsActive() && window->GetVisible())
            {
                if (window->IsDestroyingWxWindow())
                    return nullptr;
                
                window->AddRef();
                return window;
            }
        }

        return nullptr;
    }

    void Window::Activate()
    {
        GetFrame()->SetFocus();
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

        auto newState = RetrieveState();
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

    void Window::OnActivate(wxActivateEvent& event)
    {
        event.Skip();
        bool active = event.GetActive();
        _flags.Set(WindowFlags::Active, active);
        
        if (active)
            RaiseEvent(WindowEvent::Activated);
        else
            RaiseEvent(WindowEvent::Deactivated);
    }

    void Window::OnMaximize(wxMaximizeEvent& event)
    {
        event.Skip();
        _lastState = RetrieveState();
        RaiseEvent(WindowEvent::StateChanged);
    }

    void Window::OnIconize(wxIconizeEvent& event)
    {
        event.Skip();
        _lastState = RetrieveState();
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

    Frame* Window::GetFrame()
    {
        return dynamic_cast<Frame*>(GetWxWindow());
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
}
