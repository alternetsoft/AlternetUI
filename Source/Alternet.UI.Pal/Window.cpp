#include "Window.h"

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
        _title(*this, u"", &Control::IsWxWindowCreated, &Window::RetrieveTitle, &Window::ApplyTitle)
    {
        GetDelayedValues().Add(&_title);
        GetDelayedValues().Add(&_delayedFlags);
        SetVisible(false);
        CreateWxWindow();
    }

    Window::~Window()
    {
        _flags.Set(WindowFlags::DestroyingWindow, true);

        _panel->Destroy();
        _panel = nullptr;

        _frame->Unbind(wxEVT_SIZE, &Window::OnSizeChanged, this);
        _frame->Unbind(wxEVT_CLOSE_WINDOW, &Window::OnClose, this);
        _frame->Unbind(wxEVT_ACTIVATE, &Window::OnActivate, this);

        _frame->Destroy();
        _frame = nullptr;
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

    WindowStartPosition Window::GetWindowStartPosition()
    {
        return WindowStartPosition();
    }

    void Window::SetWindowStartPosition(WindowStartPosition value)
    {
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

        _frame->Bind(wxEVT_SIZE, &Window::OnSizeChanged, this);
        _frame->Bind(wxEVT_CLOSE_WINDOW, &Window::OnClose, this);
        _frame->Bind(wxEVT_DESTROY, &Window::OnDestroy, this);
        _frame->Bind(wxEVT_ACTIVATE, &Window::OnActivate, this);

        _panel = new wxPanel(_frame);

        return _frame;
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
                if (window->_flags.IsSet(WindowFlags::DestroyingWindow))
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
    }

    bool Window::GetResizable()
    {
        return _flags.IsSet(WindowFlags::Resizable);
    }

    void Window::OnSizeChanged(wxSizeEvent& event)
    {
        event.Skip();
        RaiseEvent(WindowEvent::SizeChanged);
    }

    void Window::SetResizable(bool value)
    {
        if (GetResizable() == value)
            return;

        _flags.Set(WindowFlags::Resizable, value);
        ScheduleRecreateWxWindow();
    }

    void Window::OnDestroy(wxWindowDestroyEvent& event)
    {
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