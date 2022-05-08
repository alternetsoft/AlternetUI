#include "Window.h"

namespace Alternet::UI
{
    Frame::Frame(long style) : wxFrame(NULL, wxID_ANY, "", wxDefaultPosition, wxDefaultSize, style)
    {
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
            })
    {
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
        return wxStr(_frame->GetTitle());
    }

    void Window::SetTitle(const string& value)
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
        _frame = new Frame(wxMINIMIZE_BOX | wxMAXIMIZE_BOX | wxRESIZE_BORDER | wxSYSTEM_MENU | wxCAPTION | wxCLOSE_BOX | wxCLIP_CHILDREN | (GetShowInTaskbar() ? 0 : wxFRAME_NO_TASKBAR));

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

    wxWindow* Window::GetParentingWxWindow()
    {
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

    int Window::GetTopLevelWindowsCount()
    {
        int count = 0;
        wxWindowList::compatibility_iterator node = wxTopLevelWindows.GetFirst();
        while (node)
        {
            count++;
            node = node->GetNext();
        }

        return count;
    }

    void Window::OnDestroy(wxWindowDestroyEvent& event)
    {
        // Ensure the parking window is closed after all regular windows have been closed.

        if (dynamic_cast<wxFrameBase*>(event.GetWindow()) == nullptr)
            return;
        
        if (!ParkingWindow::IsCreated())
            return;

        if (wxTheApp->GetTopWindow() == ParkingWindow::GetWindow())
        {
            if (GetTopLevelWindowsCount() == 2)
                ParkingWindow::Destroy();
            else
                GetNextTopLevelWindow()->SetFocus();
        }
    }

    void Window::OnActivate(wxActivateEvent& event)
    {
    }

    wxWindow* Window::GetNextTopLevelWindow()
    {
        wxWindowList::compatibility_iterator node = wxTopLevelWindows.GetFirst();
        while (node)
        {
            auto window = node->GetData();

            if (!ParkingWindow::IsCreated() || window != ParkingWindow::GetWindow())
                return window;

            node = node->GetNext();
        }

        return nullptr;
    }
    
    Frame* Window::GetFrame()
    {
        return dynamic_cast<Frame*>(GetWxWindow());
    }
}