#include "Window.h"

namespace Alternet::UI
{
    Frame::Frame() : wxFrame(NULL, wxID_ANY, "")
    {
    }

    // ------------

    Window::Window()
    {
        CreateWxWindow();
    }

    Window::~Window()
    {
        _panel->Destroy();
        _panel = nullptr;

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

    wxWindow* Window::CreateWxWindowCore(wxWindow* parent)
    {
        _frame = new Frame();

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

    void Window::OnDestroy(wxWindowDestroyEvent& event)
    {
        if (dynamic_cast<wxFrameBase*>(event.GetWindow()) == nullptr)
            return;
        
        if (!IsParkingWindowCreated())
            return;

        if (wxTheApp->GetTopWindow() == GetParkingWindow())
            DestroyParkingWindow();
    }
}