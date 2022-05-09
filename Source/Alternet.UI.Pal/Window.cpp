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

    Window* Frame::GetWindow()
    {
        return _window;
    }

    // ------------

    Window::Window():
        _flags(WindowFlags::ShowInTaskbar),
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

    wxWindow* Window::CreateWxWindowCore(wxWindow* parent)
    {
        auto style = wxMINIMIZE_BOX | wxMAXIMIZE_BOX | wxRESIZE_BORDER | wxSYSTEM_MENU | wxCAPTION | wxCLOSE_BOX | wxCLIP_CHILDREN;

        if (!GetShowInTaskbar())
            style |= wxFRAME_NO_TASKBAR;

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
        RecreateWxWindowIfNeeded();
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

    void Window::OnClose(wxCloseEvent& event)
    {
        if (RaiseEvent(WindowEvent::Closing))
            event.Veto();
    }

    void Window::OnSizeChanged(wxSizeEvent& event)
    {
        event.Skip();
        RaiseEvent(WindowEvent::SizeChanged);
    }

    void Window::OnDestroy(wxWindowDestroyEvent& event)
    {
    }

    void Window::OnActivate(wxActivateEvent& event)
    {
    }

    Frame* Window::GetFrame()
    {
        return dynamic_cast<Frame*>(GetWxWindow());
    }
}