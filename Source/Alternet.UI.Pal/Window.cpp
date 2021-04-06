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
        //delete _frame;
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
        _panel = new wxPanel(_frame);

        return _frame;
    }

    wxWindow* Window::GetParentingWxWindow()
    {
        return _panel;
    }
}