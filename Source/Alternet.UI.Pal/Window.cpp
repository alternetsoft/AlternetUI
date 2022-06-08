#include "Window.h"
#include "Application.h"
#include "IdManager.h"

namespace Alternet::UI
{
    Frame::Frame(Window* window, long style) : wxFrame(NULL, wxID_ANY, "", wxDefaultPosition, wxDefaultSize, style), _window(window)
    {
        _allFrames.push_back(this);
    }

    Frame::~Frame()
    {
        // Ensure the parking window is closed after all regular windows have been closed.
        _allFrames.erase(std::find(_allFrames.begin(), _allFrames.end(), this));

        if (!ParkingWindow::IsCreated())
            return;

        if (wxTheApp->GetTopWindow() == ParkingWindow::GetWindow())
        {
            if (_allFrames.size() == 0)
                ParkingWindow::Destroy();
            else
                _allFrames[0]->SetFocus();
        }
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
                //{DelayedWindowFlags::ShowInTaskbar, std::make_tuple(&Window::RetrieveShowInTaskbar, &Window::ApplyShowInTaskbar)},
            }),
        _title(*this, u"", &Control::IsWxWindowCreated, &Window::RetrieveTitle, &Window::ApplyTitle),
        _state(*this, WindowState::Normal, &Control::IsWxWindowCreated, &Window::RetrieveState, &Window::ApplyState),
        _minimumSize(*this, Size(), &Control::IsWxWindowCreated, &Window::RetrieveMinimumSize, &Window::ApplyMinimumSize),
        _maximumSize(*this, Size(), &Control::IsWxWindowCreated, &Window::RetrieveMaximumSize, &Window::ApplyMaximumSize),
        _menu(*this, nullptr, &Control::IsWxWindowCreated, &Window::RetrieveMenu, &Window::ApplyMenu)
    {
        GetDelayedValues().Add(&_title);
        GetDelayedValues().Add(&_state);
        GetDelayedValues().Add(&_delayedFlags);
        GetDelayedValues().Add(&_minimumSize);
        GetDelayedValues().Add(&_maximumSize);
        SetVisible(false);
        CreateWxWindow();
    }

    Window::~Window()
    {
        if (_icon != nullptr)
            _icon->Release();
    }

    void Window::AddInputBinding(const string& managedCommandId, Key key, ModifierKeys modifiers)
    {
        if (managedCommandId.empty())
            throwExInvalidArg(managedCommandId, u"Command ID must not be empty.");

        if (_acceleratorsByCommandIds.find(managedCommandId) != _acceleratorsByCommandIds.end())
            throwExInvalidArg(managedCommandId, u"Input binding with this command ID was already added to this window.");

        auto keyboard = Application::GetCurrent()->GetKeyboardInternal();
        auto wxKey = keyboard->KeyToWxKey(key);
        auto acceleratorFlags = keyboard->ModifierKeysToAcceleratorFlags(modifiers);

        _acceleratorsByCommandIds[managedCommandId] = wxAcceleratorEntry(acceleratorFlags, wxKey, IdManager::AllocateId());

        UpdateAcceleratorTable();
    }

    void Window::RemoveInputBinding(const string& managedCommandId)
    {
        if (managedCommandId.empty())
            throwExInvalidArg(managedCommandId, u"Command ID must not be empty.");

        auto it = _acceleratorsByCommandIds.find(managedCommandId);
        if (it == _acceleratorsByCommandIds.end())
            throwExInvalidArg(managedCommandId, u"Input binding with this command ID was not found in this window.");

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
        for (auto pair : _acceleratorsByCommandIds)
            entries.push_back(pair.second);

        frame->SetAcceleratorTable(entries.empty() ? wxNullAcceleratorTable : wxAcceleratorTable(entries.size(), &entries[0]));
    }

    void Window::OnCommand(wxCommandEvent& event)
    {
        auto id = event.GetId();

        optional<string> foundManagedCommandId;
        for (auto pair : _acceleratorsByCommandIds)
        {
            if (pair.second.GetCommand() == id)
                foundManagedCommandId = pair.first;
        }

        if (!foundManagedCommandId.has_value())
            return;

        CommandEventData data{ const_cast<char16_t*>(foundManagedCommandId.value().c_str()) };
        bool cancelled = RaiseEvent(WindowEvent::InputBindingCommandExecuted, &data);
        if (!cancelled)
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
        else
            throwExInvalidArgEnumValue(value);
    }

    Size Window::RetrieveMinimumSize()
    {
        return _appliedMinimumSize;
    }

    void Window::ApplyMinimumSize(const Size& value)
    {
        auto window = GetWxWindow();
        auto size = fromDip(value, window);
        window->SetMinSize(size == wxSize() ? wxDefaultSize : size);
    }

    Size Window::RetrieveMaximumSize()
    {
        return _appliedMaximumSize;
    }

    void Window::ApplyMaximumSize(const Size& value)
    {
        auto window = GetWxWindow();
        auto size = fromDip(value, window);
        window->SetMaxSize(size == wxSize() ? wxDefaultSize : size);
    }

    MainMenu* Window::RetrieveMenu()
    {
        return _storedMenu;
    }

    void Window::ApplyMenu(MainMenu* const& value)
    {
        _frame->SetMenuBar(value == nullptr ? nullptr : value->GetWxMenuBar());
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
    }

    void Window::OnBeforeDestroyWxWindow()
    {
        Control::OnBeforeDestroyWxWindow();

        auto wxWindow = GetWxWindow();

        wxWindow->Unbind(wxEVT_SIZE, &Window::OnSizeChanged, this);
        wxWindow->Unbind(wxEVT_MOVE, &Window::OnMove, this);
        wxWindow->Unbind(wxEVT_CLOSE_WINDOW, &Window::OnClose, this);
        wxWindow->Unbind(wxEVT_ACTIVATE, &Window::OnActivate, this);
        wxWindow->Unbind(wxEVT_MAXIMIZE, &Window::OnMaximize, this);
        wxWindow->Unbind(wxEVT_ICONIZE, &Window::OnIconize, this);
        wxWindow->Unbind(wxEVT_MENU, &Window::OnCommand, this);

        _frame = nullptr;
        _panel = nullptr;
    }

    void Window::ApplyBounds(const Rect& value)
    {
        auto wxWindow = GetWxWindow();
        wxRect rect(fromDip(value, wxWindow));

        if (_startLocation == WindowStartLocation::Manual || _flags.IsSet(WindowFlags::ShownOnce))
        {
            wxWindow->SetSize(rect);
        }
        else
        {
            wxWindow->SetSize(rect.width, rect.height);
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
        default:
            throwExInvalidOp;
        }
    }

    void Window::ShowCore()
    {
        if (!_flags.IsSet(WindowFlags::ShownOnce))
        {
            _flags.Set(WindowFlags::ShownOnce, true);
            ApplyDefaultLocation();
        }

        Control::ShowCore();
    }

    void Window::ApplyIcon(Frame* value)
    {
        // From wxWidgets code:
        // FIXME: SetIcons(wxNullIconBundle) should unset existing icons,
        //        but we currently don't do that

        value->SetIcons(_icon == nullptr ? wxNullIconBundle : *(_icon->GetIconBundle()));
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

    long Window::GetWindowStyle()
    {
        long style = wxSYSTEM_MENU | wxCLIP_CHILDREN;

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


        _panel = new wxPanel(_frame);

        return _frame;
    }

    Size Window::GetMinimumSize()
    {
        return _minimumSize.Get();
    }

    void Window::SetMinimumSize(const Size& value)
    {
        _minimumSize.Set(value);
        _appliedMinimumSize = value;
        auto limited = CoerceSize(GetSize());
        if (limited.has_value())
            SetSize(limited.value());
    }

    Size Window::GetMaximumSize()
    {
        return _maximumSize.Get();
    }

    void Window::SetMaximumSize(const Size& value)
    {
        _maximumSize.Set(value);
        _appliedMaximumSize = value;
        auto limited = CoerceSize(GetSize());
        if (limited.has_value())
            SetSize(limited.value());
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

    wxWindow* Window::GetParentingWxWindow(Control* child)
    {
        if (dynamic_cast<Window*>(child) != nullptr)
            return _frame;

        return _panel;
    }

    Color Window::RetrieveBackgroundColor()
    {
        return _panel->GetBackgroundColour();
    }

    void Window::ApplyBackgroundColor(const Color& value)
    {
        _panel->SetBackgroundColour(value);
    }

    ImageSet* Window::GetIcon()
    {
        if (_icon != nullptr)
            _icon->AddRef();
        return _icon;
    }

    void Window::SetIcon(ImageSet* value)
    {
        if (_icon != nullptr)
            _icon->Release();
        _icon = value;
        if (_icon != nullptr)
            _icon->AddRef();
        if (IsWxWindowCreated())
            ApplyIcon(GetFrame());
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
        auto frame = GetFrame();
        auto children = frame->GetChildren();
        auto items = new std::vector<Window*>();
        for (int i = 0; i < children.GetCount(); i++)
        {
            auto childFrame = dynamic_cast<Frame*>(children[i]);
            if (childFrame != nullptr)
                items->push_back(childFrame->GetWindow());
        }

        return items;
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
            if (window->GetIsActive())
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
        if (RaiseEvent(WindowEvent::Closing))
            event.Veto();
        else
            event.Skip();
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
        bool active = event.GetActive();
        _flags.Set(WindowFlags::Active, active);
        
        if (active)
            RaiseEvent(WindowEvent::Activated);
        else
            RaiseEvent(WindowEvent::Deactivated);
    }

    void Window::OnMaximize(wxMaximizeEvent& event)
    {
        _lastState = RetrieveState();
        RaiseEvent(WindowEvent::StateChanged);
    }

    void Window::OnIconize(wxIconizeEvent& event)
    {
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

    optional<Size> Window::CoerceSize(const Size& value)
    {
        auto minSize = GetMinimumSize();
        auto maxSize = GetMaximumSize();

        bool limitNeeded = false;
        Size limitedSize = value;

        if (minSize.Width != 0 && minSize.Width > value.Width)
        {
            limitedSize.Width = minSize.Width;
            limitNeeded = true;
        }

        if (minSize.Height != 0 && minSize.Height > value.Height)
        {
            limitedSize.Height = minSize.Height;
            limitNeeded = true;
        }

        if (maxSize.Width != 0 && maxSize.Width < value.Width)
        {
            limitedSize.Width = maxSize.Width;
            limitNeeded = true;
        }

        if (maxSize.Height != 0 && maxSize.Height < value.Height)
        {
            limitedSize.Height = maxSize.Height;
            limitNeeded = true;
        }

        if (limitNeeded)
            return limitedSize;
        else
            return nullopt;
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