#include "Window.h"

namespace Alternet::UI
{
    Frame::Frame() : wxFrame(NULL, wxID_ANY, "")
    {
    }

    // ------------

    Window::Window() : _frame(new Frame())
    {
        _frame->SetSizer(new wxBoxSizer(wxVERTICAL));
#ifdef __WXMSW__
        _frame->SetBackgroundColour(wxColor(0xffffff));
#endif
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

    void Window::Show()
    {
        _frame->Show();
    }

    void Window::AddChildControl(Control* value)
    {
        value->CreateWxWindow(_frame);
    }
}