#include "Window.h"

namespace Alternet::UI
{
    Frame::Frame() : wxFrame(NULL, wxID_ANY, "")
    {
    }

    // ------------

    Window::Window()
    {
        SetVisible(false);
        CreateWxWindow();
    }

    Window::~Window()
    {
        _panel->Destroy();
        _panel = nullptr;

        _frame->Unbind(wxEVT_SIZE, &Window::OnSizeChanged, this);
        _frame->Unbind(wxEVT_CLOSE_WINDOW, &Window::OnClose, this);

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
        _frame = new Frame();

        _frame->Bind(wxEVT_SIZE, &Window::OnSizeChanged, this);
        _frame->Bind(wxEVT_CLOSE_WINDOW, &Window::OnClose, this);
        _frame->Bind(wxEVT_DESTROY, &Window::OnDestroy, this);

        _panel = new wxPanel(_frame);

        return _frame;
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

    void Window::OnDestroy(wxWindowDestroyEvent& event)
    {
        // Ensure the parking window is closed after all regular windows have been closed.

        if (dynamic_cast<wxFrameBase*>(event.GetWindow()) == nullptr)
            return;
        
        if (!ParkingWindow::IsCreated())
            return;

        if (wxTheApp->GetTopWindow() == ParkingWindow::GetWindow())
            ParkingWindow::Destroy();
    }
}